using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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