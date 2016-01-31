using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f; 
    public Text m_MessageText;              
    public GameObject m_TankPrefab;
    public TankManager m_Tank;          


    private TankManager[] m_EnemyTanks;           
    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
/*    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;       
*/

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        //SpawnEnemies();

        StartCoroutine(GameLoop());
    }


    private void SpawnEnemies()
    {
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            m_EnemyTanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_EnemyTanks[i].m_SpawnPoint.position, m_EnemyTanks[i].m_SpawnPoint.rotation) as GameObject;
            m_EnemyTanks[i].m_PlayerNumber = i + 1;
            m_EnemyTanks[i].Setup();
        }
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

/*        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
*/    }


    private IEnumerator RoundStarting()
    {
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        yield return null;
    }


    private IEnumerator RoundEnding()
    {
        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            if (m_EnemyTanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }

/*
    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            if (m_EnemyTanks[i].m_Instance.activeSelf)
                return m_EnemyTanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            if (m_EnemyTanks[i].m_Wins == m_NumRoundsToWin)
                return m_EnemyTanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            message += m_EnemyTanks[i].m_ColoredPlayerText + ": " + m_EnemyTanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }
*/

    private void ResetAllTanks()
    {
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            m_EnemyTanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            m_EnemyTanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_EnemyTanks.Length; i++)
        {
            m_EnemyTanks[i].DisableControl();
        }
    }
}