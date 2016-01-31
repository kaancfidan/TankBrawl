using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MoveCommand : GroundTargetedCommand
{
    public MoveCommand(Vector3 target, TankController controller, NavMeshAgent agent)
    : base(target, controller)
    {
        m_Agent = agent;
    }

    public override void Execute()
    {
        m_Controller.MakeKinematic();
        m_Agent.destination = m_Target;

        base.Execute();
    }

    public override bool IsFinished()
    {
        float distance = Vector3.Magnitude(m_Agent.transform.position - m_Target);
        return (distance <= 0.15f);
    }

    private NavMeshAgent m_Agent;
}