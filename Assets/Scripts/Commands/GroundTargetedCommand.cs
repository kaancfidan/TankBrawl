using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class GroundTargetedCommand : Command
{
    protected Vector3 m_Target;

    public Vector3 Target
    {
        get { return m_Target; }
    }

    protected GroundTargetedCommand(Vector3 target, TankController controller)
        : base(controller)
    {
        m_Target = target;
    }
}
