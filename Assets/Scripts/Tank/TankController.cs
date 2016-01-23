using System.Collections.Generic;
using UnityEngine;


public abstract class Command
{
    protected Vector3 m_Target;
    public bool IsRunning = false;

    protected Command(Vector3 target)
    {
        m_Target = target;
    }

    public virtual void Execute()
    {
        IsRunning = true;
    }
    public virtual bool IsFinished()
    {
        return true;
    }
}

public enum TankMoveStatus
{
    Normal,
    Tipped,
    Turned
}

public class TankController : MonoBehaviour
{
    private List<Command> m_CommandQueue = new List<Command>();
    private TankMoveStatus m_Status = TankMoveStatus.Normal;
    private Rigidbody m_RigidBody;
    private NavMeshAgent m_Agent;

    public TankMoveStatus Status
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

        if (upVectorSimilarity < 1 && upVectorSimilarity > 0.5)
        {
            m_Status = TankMoveStatus.Tipped;
            m_RigidBody.isKinematic = false;
            m_Agent.enabled = false;
        }
        else if(upVectorSimilarity <= 0.5)
        {
            m_Status = TankMoveStatus.Turned;
            m_RigidBody.isKinematic = false;
            m_Agent.enabled = false;
        }
        else
        {
            m_Status = TankMoveStatus.Normal;
        }

        if (Status != TankMoveStatus.Normal)
        {
            m_CommandQueue.Clear();
        }
        else if (m_CommandQueue.Count > 0)
        { 
            {
                if (!m_CommandQueue[0].IsRunning) // Execute the first command.
                {
                    m_CommandQueue[0].Execute();
                }
                else if (m_CommandQueue[0].IsFinished()) // Current command finished, switch to next.
                {
                    m_CommandQueue.RemoveAt(0);
                }
            }

        }

    }

    public void Command(Command command)
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            m_CommandQueue.Clear();
        }

        m_CommandQueue.Add(command);
    }

}
