using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeLevelManager : MonoBehaviour {

    enum STATES{
        START,
        RUNNING,
        END
    };

    [SerializeField]
    private int m_TimeLeft = 50;

    [SerializeField]
    private Text m_TimeLeftUI;
    [SerializeField]
    private Text m_CentralText;

    [SerializeField]
    private GameObject m_Player;

    [SerializeField]
    private GameObject m_SpawnManager;


    private const float m_TimeBeforExitStartState = 2.0f;

    private float m_ElapsedTime = 0;

    private STATES m_CurrentState = STATES.START;


    private bool m_LevelFinishedSuccesfully = false;

    private void Start()
    {
        m_TimeLeftUI.text = "Time left: " + m_TimeLeft;

        m_CentralText.text = "Save you Ice Cream!";

        m_Player.GetComponent<PlayerController>().OnPlayerFailure += OnPlayerFailure;
        m_SpawnManager.SetActive(false);
    }


    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;
        switch (m_CurrentState)
        {
            case STATES.START:
                DoStart();
                break;
            case STATES.RUNNING:
                DoRunning();
                break;
            case STATES.END:
                DoEnd();
                break;

        }


    }

    private void DoStart()
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
        m_SpawnManager.SetActive(false);
        if (m_LevelFinishedSuccesfully)
        {
            m_CentralText.text = "Good Work!";
        }
        else
        {
            m_CentralText.text = "Bleah! Holy Shit!";
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
