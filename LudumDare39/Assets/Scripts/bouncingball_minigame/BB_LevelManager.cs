using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BB_LevelManager : MonoBehaviour {

    [SerializeField]
    private GameObject m_TimerPrefab;
    [SerializeField]
    private Text m_LevelTimerText;
    [SerializeField]
    private Text m_MiddleMessage;

    private CM_Timer m_StartTimer;
    private CM_Timer m_RunningTimer;

    [SerializeField]
    private GameObject m_PlayerObject;
    [SerializeField]
    private GameObject m_Ball;
    [SerializeField]
    private GameObject m_Basket;


    private bool m_GameCompleteSuccessfully = false;

    private void Awake()
    {
       // m_CurrentState = STATES.INIT;

        if (m_TimerPrefab != null)
        {
            m_StartTimer = Instantiate(m_TimerPrefab).GetComponent<CM_Timer>();
            m_RunningTimer = Instantiate(m_TimerPrefab).GetComponent<CM_Timer>();
        }
        else
        {
            Debug.Log("ERROR: TimerPrefab in BB_LevelManager is null");
        }

        m_PlayerObject.SetActive(false);
        m_Ball.SetActive(false);

        m_Basket.GetComponent<BB_Basket>().OnBasketDone += OnSuccessfully;
    }
    // Use this for initialization
    void Start ()
    {     
        m_MiddleMessage.text = "Slam-dunk!";
        m_StartTimer.OnTimesUp += OnStartTimerEnd;
        m_StartTimer.StartTimer(2, "Slam-dunk!", "", null);
    }

    private void OnStartTimerEnd() 
    {
        m_MiddleMessage.enabled = false;
        StartTheGame();
    }

    private void StartTheGame()
    {
        m_RunningTimer.OnTimesUp += OnFailure;
        m_RunningTimer.StartTimer(10, "TIME LEFT: ", "", m_LevelTimerText);
        m_PlayerObject.SetActive(true);
        m_Ball.SetActive(true);
    }


    private void OnFailure()
    {
        m_GameCompleteSuccessfully = false;
        m_RunningTimer.StopTimer();
        m_MiddleMessage.text = "Oh noooooooo";
        m_MiddleMessage.enabled = true;
        m_Ball.SetActive(false);

    }

    private void OnSuccessfully()
    {
        m_GameCompleteSuccessfully = true;
        m_RunningTimer.StopTimer();
        m_MiddleMessage.text = "Good Work!";
        m_MiddleMessage.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!m_GameCompleteSuccessfully)
            OnFailure();
    }



}
