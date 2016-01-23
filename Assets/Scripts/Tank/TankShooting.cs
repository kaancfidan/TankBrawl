using UnityEngine;
using UnityEngine.UI;

public class ShootCommand : Command
{
    public Transform FireTransform;
    private GameObject m_GameObject;
    public Rigidbody m_Shell;
    public Rigidbody m_Tank;
    private NavMeshAgent m_Agent;
    public Transform m_FireTransform;
    public AudioSource m_ShootingAudio;
    public AudioClip m_FireClip;

    public ShootCommand(Vector3 target,
        Rigidbody tank,
        NavMeshAgent agent,
        Rigidbody shell,
        Transform fireTransform,
        AudioSource shootingAudio,
        AudioClip fireClip)
    : base(target)
    {
        m_Tank = tank;
        m_Agent = agent;
        m_Shell = shell;
        m_FireTransform = fireTransform;
        m_ShootingAudio = shootingAudio;
        m_FireClip = fireClip;
    }

    public override void Execute()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            GameObject.Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        shellInstance.velocity = 18 * m_FireTransform.forward;

        m_Tank.isKinematic = false;
        m_Tank.velocity = m_Agent.velocity;
        m_Agent.enabled = false;
        m_Tank.AddForce(-m_FireTransform.forward * 200);

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        base.Execute();
    }
}

public class TankShooting : MonoBehaviour
{    
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;         
    public AudioSource m_ShootingAudio;  
    public AudioClip m_FireClip;

    private TankController m_Controller;
    private Rigidbody m_RigidBody;
    private NavMeshAgent m_Agent;

    private void Awake()
    {
        m_Controller = GetComponent<TankController>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_Controller.Command(new ShootCommand(Vector3.zero, m_RigidBody, m_Agent, m_Shell, m_FireTransform, m_ShootingAudio, m_FireClip));
        }
        
    }
}