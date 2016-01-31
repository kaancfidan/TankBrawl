using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ExplodeCommand : Command
{
    private Rigidbody m_RigidBody;
    private GameObject m_ExplosionPrefab;
    private GameObject m_Explosion;
    private float m_Timer = 0;
    private bool m_IsExploded = false;
    private TankHealth m_Health;

    public ExplodeCommand(TankController controller, Rigidbody rigidBody, TankHealth health, GameObject explosionPrefab)
        : base(controller)
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

        if (m_Timer < 1)
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

            if (m_Health.CurrentHealth > 10)
                m_Health.TakeDamage(10);

            m_IsExploded = true;

            GameObject.Destroy(m_Explosion, explosionParticles.duration);
            return false;
        }
        else if (m_Timer >= 1.1 && m_IsExploded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
