using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public GameObject m_MoveCommandArrows;

    public AudioClip[] m_MoveVoiceClips;
    public AudioClip[] m_ShootVoiceClips;

    private List<Command> m_CommandQueue = new List<Command>();
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

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (CurrentCommand != null)
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
            m_Agent.enabled = false;
            m_RigidBody.isKinematic = false;
        }            
    }

    public void MakeKinematic()
    {
        if(!m_Agent.enabled)
        {
            m_Agent.enabled = true;
            m_Agent.velocity = m_RigidBody.velocity;
            m_RigidBody.isKinematic = true;
        }
    }

}
