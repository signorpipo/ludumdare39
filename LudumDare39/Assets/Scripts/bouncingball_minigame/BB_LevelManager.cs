using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//ordine da facile a difficile
//Numero tentativi 3 a 1
//gravità 1 e 15
//tempo 15 a 5
public class BB_LevelManager : AbstarcMinigameManager
{
    [SerializeField]
    private bool m_ForceStart = false;

    [SerializeField]
    private int m_AttemptsNumber = 3;
    [SerializeField]
    private GameObject m_TimerPrefab;
    [SerializeField]
    private Text m_LevelTimerText;
    [SerializeField]
    private GameObject m_MiddleMessage;
    [SerializeField]
    private Text m_AttemptsOrLife;

    private CM_Timer m_StartTimer;
    private CM_Timer m_RunningTimer;

    [SerializeField]
    private GameObject m_PlayerObject;
    [SerializeField]
    private GameObject m_Ball;
    [SerializeField]
    private GameObject m_Basket;

    private int m_GameTimer = 10;


    private Vector3 m_BallInitialPosition;
    private Vector3 m_BlockInitialPosition;
    private Quaternion m_BlockInitialRotation;


    private bool m_GameCompleteSuccessfully = false;

    // 1 facile, 0 difficile
    public override void StartMinigame
        (
        int i_Type, 
        float i_TimeValue, 
        float i_NumItemValue, 
        float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        m_GameTimer = (int)(i_TimeValue * (15 - 5) + 5);
        m_AttemptsNumber = (int)((3 - 1) * i_NumItemValue + 1);
        m_Ball.GetComponent<Rigidbody2D>().gravityScale = (int)((1.0f - i_ItemVelocityValue) * (15 - 1) + 1);


        DoStart();
    }

    private void DoStart()
    {
       // m_MiddleMessage.text = "Slam-dunk!";
        m_StartTimer.OnTimesUp += OnStartTimerEnd;
        m_StartTimer.StartTimer(2, "Slam-dunk!", "", null);

        m_AttemptsOrLife.text = "Life: " + m_AttemptsNumber;
    }

    private void Awake()
    {
        // m_CurrentState = STATES.INIT;

        m_BallInitialPosition = new Vector3( m_Ball.transform.position.x, m_Ball.transform.position.y, m_Ball.transform.position.z);
        m_BlockInitialPosition = new Vector3( m_PlayerObject.transform.position.x, m_PlayerObject.transform.position.y, m_PlayerObject.transform.position.z);
        m_BlockInitialRotation = m_PlayerObject.transform.rotation;

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
        if(m_ForceStart)
        {
            DoStart();
        }
    }

    private void OnStartTimerEnd() 
    {
        m_MiddleMessage.SetActive(false);
        StartTheGame();
    }

    private void StartTheGame()
    {
        m_Ball.transform.position = m_BallInitialPosition;
        m_PlayerObject.transform.position = m_BlockInitialPosition;

        m_PlayerObject.transform.rotation = m_BlockInitialRotation;

        m_RunningTimer.OnTimesUp += OnFailure;
        m_RunningTimer.StartTimer(m_GameTimer, "Time Left: ", "", m_LevelTimerText);
        m_PlayerObject.SetActive(true);
        m_Ball.SetActive(true);
    }


    private void OnFailure()
    {
        m_GameCompleteSuccessfully = false;
        m_RunningTimer.StopTimer();
        //m_MiddleMessage.text = "Oh noooooooo";
        m_MiddleMessage.SetActive(true);
        m_Ball.SetActive(false);

        if(--m_AttemptsNumber > 0)
        {
            m_StartTimer.OnTimesUp += OnStartTimerEnd;
            m_StartTimer.StartTimer(2, "Slam-dunk!", "", null);
        }
        else
        {
            SceneEnded(.0f);
        }

        m_AttemptsOrLife.text = "Life: " + m_AttemptsNumber;
    }

    private void OnSuccessfully()
    {
        m_GameCompleteSuccessfully = true;
        m_RunningTimer.StopTimer();
       // m_MiddleMessage.text = "Good Work!";
      //  m_MiddleMessage.SetActive(true);

        SceneEnded(1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_PlayerObject.SetActive(false);
        if (!m_GameCompleteSuccessfully)
            OnFailure();
    }



}
