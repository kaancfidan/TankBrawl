using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{    
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;         
    public AudioSource m_ShootingAudio;  
    public AudioClip m_FireClip;
    public GameObject m_ExplosionPrefab;
    public AudioSource m_VoiceAudio;

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