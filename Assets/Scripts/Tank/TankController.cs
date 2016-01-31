using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public GameObject m_MoveCommandArrows;

    public AudioClip[] m_MoveVoiceClips;
    public AudioClip[] m_ShootVoiceClips;

    private List<Command> m_CommandQueue = new List<Command>();
    private TankStatus m_Status = TankStatus.Normal;
    private Rigidbody m_RigidBody;
    private NavMeshAgent m_Agent;

    public AudioSource m_VoiceAudio;

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

    private TankStatus CheckStatus()
    {
        var upVectorSimilarity = Vector3.Dot(transform.up, Vector3.up);

        if (transform.position.y > 2)
        {
            return TankStatus.Thrown;
        }
        else if (upVectorSimilarity <= 0.98 && upVectorSimilarity > 0.5)
        {
            return TankStatus.Tipped;
        }
        else if (upVectorSimilarity <= 0.5)
        {
            return TankStatus.Turned;
        }
        else
        {
            return TankStatus.Normal;
        }
    }

    private void FixedUpdate()
    {
        m_Status = CheckStatus();

        if (m_Status != TankStatus.Normal)
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
        if(m_Status == TankStatus.Normal)
        {
            if (CurrentCommand == null || !CurrentCommand.IsBlocking)
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    m_CommandQueue.Clear();
                }

                m_CommandQueue.Add(command);

                AudioClip[] voiceClips;

                if (command is MoveCommand)
                {
                    voiceClips = m_MoveVoiceClips;
                    var moveCommand = command as MoveCommand;
                    var moveCommandInstance = Instantiate(m_MoveCommandArrows, moveCommand.Target, Quaternion.identity) as GameObject;

                    Destroy(moveCommandInstance, 0.667f);
                }
                else if (command is ShootCommand)
                    voiceClips = m_ShootVoiceClips;
                else
                    voiceClips = null;            
                
                PlayVoiceClip(voiceClips);
            }
        }
    }

    private void PlayVoiceClip(AudioClip[] voiceClips)
    {
        if (voiceClips != null && !m_VoiceAudio.isPlaying)
        {
            int selectedClip = (int)(Random.value * voiceClips.Length);
            m_VoiceAudio.clip = voiceClips[selectedClip];
            m_VoiceAudio.Play();
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
