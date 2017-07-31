using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeLevelManager : AbstarcMinigameManager
{

    enum STATES{
        START,
        RUNNING,
        END
    };

    [SerializeField]
    private bool m_ForceStart = false;

    [SerializeField]
    private int m_TimeLeft = 10;

    [SerializeField]
    private float m_EnemyVerticalSpeed = -5.0f;

    [SerializeField]
    private Text m_TimeLeftUI;
    [SerializeField]
    private GameObject m_TimeLeftObj;

    [SerializeField]
    private Text m_CentralText;
    [SerializeField]
    private GameObject m_CentralObj;

    [SerializeField]
    private GameObject m_Player;

    [SerializeField]
    private GameObject m_SpawnManager;


    private const float m_TimeBeforExitStartState = 2.0f;

    private float m_ElapsedTime = 0;

    private STATES m_CurrentState = STATES.START;


    private bool m_LevelFinishedSuccesfully = false;


    private bool m_GameEnable = false;

    private void DoStart()
    {
        m_TimeLeftUI.text = "Time left: " + m_TimeLeft;

        m_CentralText.text = "Save Your Ice Cream";

        m_Player.GetComponent<PlayerController>().OnPlayerFailure += OnPlayerFailure;
        m_SpawnManager.SetActive(false);
        m_SpawnManager.GetComponent<SpawnManager>().EnemyVerticalSPeed = m_EnemyVerticalSpeed;

        m_GameEnable = true;
    }

    public override void StartMinigame
    (
    int i_Type,
    float i_TimeValue,
    float i_NumItemValue,
    float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        m_TimeLeft = (int)(i_TimeValue * (30 - 10) + 10);
        m_SpawnManager.GetComponent<SpawnManager>().SpawnRate = ((0.5f - 0.2f) * i_NumItemValue + 0.2f);
        m_EnemyVerticalSpeed = -(((1.0f - i_ItemVelocityValue) * (13 - 5) + 5));

        ColorizeBckManager.BckTypes type = ColorizeBckManager.BckTypes.BCK_PLANNING;
        switch(i_Type)
        {
            case 0: type = ColorizeBckManager.BckTypes.BCK_PHYSICS; break;
            case 1: type = ColorizeBckManager.BckTypes.BCK_MONEY; break;
            case 2: type = ColorizeBckManager.BckTypes.BCK_SOCIAL; break;
        }
        FindObjectOfType<ColorizeBckManager>().SetUncoloredBckType(type);
        DoStart();
    }




    private void Start()
    {
        m_SpawnManager.SetActive(false);
        if (m_ForceStart)
        {
            DoStart();
            
        }
    }


    private void Update()
    {
        if (m_GameEnable)
        {
            m_ElapsedTime += Time.deltaTime;
            switch (m_CurrentState)
            {
                case STATES.START:
                    DoInit();
                    break;
                case STATES.RUNNING:
                    DoRunning();
                    break;
                case STATES.END:
                    DoEnd();
                    break;

            }
        }


    }

    private void DoInit()
    {
        if (m_ElapsedTime >= m_TimeBeforExitStartState)
        {
            m_CurrentState = STATES.RUNNING;

            m_ElapsedTime = 0;

        }
    }

    private void DoRunning()
    {
        m_CentralText.enabled = false;
        m_CentralObj.SetActive(false);
        m_SpawnManager.SetActive(true);
        if (m_ElapsedTime >= 1 && m_TimeLeft != 0)
        {
            m_TimeLeftUI.text = "Time left: " + --m_TimeLeft;

            m_ElapsedTime = 0;

        }

        if (m_TimeLeft == 0)
        {
            m_CurrentState = STATES.END;
            m_LevelFinishedSuccesfully = true;
        }
    }

    private void DoEnd()
    {
        m_CentralText.enabled = true;
        m_CentralObj.SetActive(true);
        m_SpawnManager.SetActive(false);
        if (m_LevelFinishedSuccesfully)
        {
            m_CentralText.text = "Good Work!";

            SceneEnded(1.0f);
 
        }
        else
        {
            m_CentralText.text = "Bleah! Holy Shit!";
            SceneEnded(.0f);

        }
    } 


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);      
    }


    private void OnPlayerFailure()
    {
        m_LevelFinishedSuccesfully = false;
        m_CurrentState = STATES.END;
    }

}
