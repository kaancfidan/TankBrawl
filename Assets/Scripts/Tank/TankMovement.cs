using System.Collections;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private float m_OriginalPitch;

    private Transform m_Turret;
    private NavMeshAgent m_Agent;
    private Rigidbody m_RigidBody;

    private TankController m_Controller;


    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Controller = GetComponent<TankController>();
    }

    private void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
        m_Turret = transform.FindChild("TankRenderers").FindChild("TankTurret");
    }

    private void Update()
    {
        Move();
        TurnTurret();
        EngineAudio();
    }

    private void EngineAudio()
    {
        if (Vector3.Magnitude(m_Agent.velocity) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    private void TurnTurret()
    {
        var hit = new RaycastHit();
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

        if (hit.transform != null && hit.transform.name != name)
        {
            var lookAtVector = hit.point - transform.position;
            lookAtVector.y = 0; // do not let the turret look up.

            var q = Quaternion.LookRotation(lookAtVector, Vector3.up);
            m_Turret.rotation = q;
        }
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var hit = new RaycastHit();
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);

            m_Controller.Command(new MoveCommand(hit.point, m_Controller, m_Agent));
        }
    }
}