using System.Collections.Generic;
using UnityEngine;


public abstract class Command
{
    private bool m_IsRunning = false;

    protected TankController m_Controller;
    protected bool m_IsBlocking = false;

    public bool IsRunning
    {
        get
        {
            return m_IsRunning;
        }
    }
    public bool IsBlocking
    {
        get
        {
            return m_IsBlocking;
        }
    }

    protected Command(TankController controller)
    {
        m_Controller = controller;
    }

    public virtual void Execute()
    {
        m_IsRunning = true;
    }
    public virtual bool IsFinished()
    {
        return true;
    }
}

public abstract class GroundTargetedCommand : Command
{
    protected Vector3 m_Target;

    protected GroundTargetedCommand(Vector3 target, TankController controller)
        :base(controller)
    {
        m_Target = target;
    }
}

public enum TankStatus
{
    Normal,
    Tipped,
    Turned,
    Thrown
}

public class TankController : MonoBehaviour
{
    private List<Command> m_CommandQueue = new List<Command>();
    private TankStatus m_Status = TankStatus.Normal;
    private Rigidbody m_RigidBody;
    private NavMeshAgent m_Agent;

    private Command CurrentCommand
    {
        get
        {
            if (m_CommandQueue.Count > 0)
                return m_CommandQueue[0];
            else
                return null;
        }
    }

    public TankStatus Status
    {
        get
        {
            return m_Status;
        }
    }

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        var upVectorSimilarity = Vector3.Dot(transform.up, Vector3.up);

        if(transform.position.y > 0.15)
        {
            m_Status = TankStatus.Thrown;
        }
        else if (upVectorSimilarity <= 0.98 && upVectorSimilarity > 0.5)
        {
            m_Status = TankStatus.Tipped;
        }
        else if(upVectorSimilarity <= 0.5)
        {
            m_Status = TankStatus.Turned;
        }
        else
        {
            m_Status = TankStatus.Normal;
        }

        if (Status != TankStatus.Normal)
        {
            if (CurrentCommand != null && !CurrentCommand.IsBlocking)
                m_CommandQueue.Remove(CurrentCommand);
            else if(CurrentCommand != null && CurrentCommand.IsRunning && CurrentCommand.IsFinished())
                m_CommandQueue.Remove(CurrentCommand);
        }
        else if (CurrentCommand != null)
        { 
            if (!CurrentCommand.IsRunning) // Execute the first command.
            {
                CurrentCommand.Execute();
            }
            else if (CurrentCommand.IsFinished()) // Current command finished, switch to next.
            {
                m_CommandQueue.Remove(CurrentCommand);
            }
        }
    }

    public void Command(Command command)
    {
        if (CurrentCommand == null || !CurrentCommand.IsBlocking)
        {
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                m_CommandQueue.Clear();
            }

            m_CommandQueue.Add(command);
        }
    }

    public void MakeDynamic()
    {
        if(m_Agent.enabled)
        {
            m_RigidBody.velocity = m_Agent.velocity;
            m_Agent.enabled = false;
            m_RigidBody.isKinematic = false;
        }            
    }

    public void MakeKinematic()
    {
        if(!m_Agent.enabled)
        {
            m_Agent.velocity = m_RigidBody.velocity;
            m_Agent.enabled = true;
            m_RigidBody.isKinematic = true;
        }
    }

}
