using UnityEngine;
using UnityEngine.UI;

public class ShootCommand : Command
{
    public Transform FireTransform;
    public Rigidbody m_Shell;
    public Rigidbody m_RigidBody;
    public Transform m_FireTransform;
    public AudioSource m_ShootingAudio;
    public AudioClip m_FireClip;

    public ShootCommand(TankController controller,
        Rigidbody rigidBody,
        Rigidbody shell,
        Transform fireTransform,
        AudioSource shootingAudio,
        AudioClip fireClip)
    : base(controller)
    {
        m_RigidBody = rigidBody;
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

        shellInstance.velocity = 15 * m_FireTransform.forward;

        m_Controller.MakeDynamic();
        m_RigidBody.AddForce(-m_FireTransform.forward * 200);

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        base.Execute();
    }
}

public class ExplodeCommand : Command
{
    private Rigidbody m_RigidBody;
    private GameObject m_ExplosionPrefab;
    private GameObject m_Explosion;
    private float m_Timer = 0;
    private bool m_IsExploded = false;
    private TankHealth m_Health;

    public ExplodeCommand(TankController controller, Rigidbody rigidBody, TankHealth health, GameObject explosionPrefab)
        :base(controller)
    {
        m_IsBlocking = true;
        m_RigidBody = rigidBody;
        m_Health = health;
        m_ExplosionPrefab = explosionPrefab;
    }

    public override void Execute()
    {
        m_Controller.MakeDynamic();
        base.Execute();
    }

    public override bool IsFinished()
    {
        m_Timer += Time.deltaTime;

        if(m_Timer < 1)
        {
            return false;
        }
        else if (m_Timer >= 1 && !m_IsExploded)
        {
            m_Explosion = GameObject.Instantiate(m_ExplosionPrefab);
            var explosionParticles = m_Explosion.GetComponent<ParticleSystem>();
            explosionParticles.transform.position = m_RigidBody.transform.position;
            explosionParticles.Play();

            m_RigidBody.AddForce(Vector3.up * 150);

            if(m_Health.CurrentHealth > 10)
                m_Health.TakeDamage(10);

            m_IsExploded = true;

            GameObject.Destroy(m_Explosion, explosionParticles.duration);
            return false;
        }
        else if(m_Timer >= 1.1 && m_IsExploded)
        {
            return true;
        }
        else
        {
            return false;
        }        
    }
}

public class TankShooting : MonoBehaviour
{    
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;         
    public AudioSource m_ShootingAudio;  
    public AudioClip m_FireClip;
    public GameObject m_ExplosionPrefab;

    private TankController m_Controller;
    private TankHealth m_Health;
    private Rigidbody m_RigidBody;

    private void Awake()
    {
        m_Controller = GetComponent<TankController>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Health = GetComponent<TankHealth>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_Controller.Command(new ShootCommand(m_Controller, m_RigidBody, m_Shell, m_FireTransform, m_ShootingAudio, m_FireClip));
        }
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            m_Controller.Command(new ExplodeCommand(m_Controller, m_RigidBody, m_Health, m_ExplosionPrefab));
        }
        
    }
}