
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGame : AbstarcMinigameManager
{

    [SerializeField]
    private Arrow m_arrowPrefab;
    private Arrow m_directionArrow;

    [SerializeField]
    public Bar m_potencyBarPrefab;
    private Bar m_potencyBar;

    [SerializeField]
    public Basket m_basketPrefab;
    private Basket m_basket;

    [SerializeField]
    public Ball m_ballPrefab;
    private Ball m_ball;

    [SerializeField]
    private Walls m_floor;
    [SerializeField]
    private Walls m_wall;

    [SerializeField]
    private int m_tries = 5;
    private int m_currentTry = 1;
    private int m_score = 0;

    [SerializeField]
    private Text m_text;
    [SerializeField]
    private Text m_time;

    [SerializeField]
    private float m_unmodifiedTimer = 5;
    private float m_adjustedTime;
    private float m_timeLeft;
    private enum GameState
    {
        POTENCY,
        DIRECTION,
        SHOOT,
        SHOTINAIR,
        REPORTSCORE,
        TIMEUP,
        IDLE
    }

    private GameState m_gameState;

    public override void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue)
    {
        float difficultySpeed = i_ItemVelocityValue;

        m_directionArrow = Instantiate(m_arrowPrefab);
        m_directionArrow.Initialize(difficultySpeed);

        m_potencyBar = Instantiate(m_potencyBarPrefab);
        m_potencyBar.Initialize(difficultySpeed);

        m_basket = Instantiate(m_basketPrefab);
        m_basket.Initialize();

        m_ball = Instantiate(m_ballPrefab);
        m_ball.Initialize();

        m_adjustedTime = m_unmodifiedTimer + 10 * i_TimeValue;
        m_timeLeft = m_adjustedTime;
        m_gameState = GameState.DIRECTION;
    }

    private void ResetScene()
    {
        m_currentTry++;

        m_timeLeft = m_adjustedTime;
       
        m_gameState = GameState.DIRECTION;
        m_basket.Reset();
        m_ball.Reset();
        m_potencyBar.Reset();
        m_directionArrow.Reset();
    }

    private void Start()
    {

        m_directionArrow = Instantiate(m_arrowPrefab);
        m_directionArrow.Initialize(0.5f);

        m_potencyBar = Instantiate(m_potencyBarPrefab);
        m_potencyBar.Initialize(0.5f);

        m_basket = Instantiate(m_basketPrefab);
        m_basket.Initialize();

        m_ball = Instantiate(m_ballPrefab);
        m_ball.Initialize();

        m_gameState = GameState.DIRECTION;

        m_basket.onScoreEvent += onScoreEvent;
        m_wall.onMissEvent += onMissEvent;
        m_floor.onMissEvent += onMissEvent;

        m_adjustedTime = m_unmodifiedTimer;
        m_timeLeft = m_adjustedTime;
    }

    // Update is called once per frame
    void Update()
    {
        m_time.text = "Time left: " + (int)m_timeLeft;
        m_timeLeft -= Time.deltaTime;
        

        switch (m_gameState)
        {
            case GameState.DIRECTION:
                
                m_directionArrow.UpdateDirection();
                if (Input.GetMouseButtonDown(0))
                {
                    m_ball.SetDirection(m_directionArrow.GetCurrentDirection());
                    m_gameState = GameState.POTENCY;

                }
                break;
            case GameState.POTENCY:
                m_potencyBar.UpdateValue();
                if (Input.GetMouseButtonDown(0))
                {
                    m_ball.SetPotency(m_potencyBar.GetCurrentValue());
                    m_gameState = GameState.SHOOT;
                }
                break;
            case GameState.SHOOT:
                m_ball.Move();
                m_gameState = GameState.SHOTINAIR;
                break;
            /*case GameState.SHOTINAIR:
                m_basket.onScoreEvent += onScoreEvent;
                m_wall.onMissEvent += onMissEvent;
                m_floor.onMissEvent += onMissEvent;
                break;*/
            case GameState.REPORTSCORE:
                StartCoroutine(ShowMessageRoutine("Final Score: " + m_score/m_tries, 2, onEndGame));
                m_time.enabled = false;
                m_gameState = GameState.IDLE;
                break;
            case GameState.TIMEUP:
                StartCoroutine(ShowMessageRoutine("Sorry, time's up! Your current score: " + m_score, 1, null));
                ResetScene();
                break;
        }
        if (m_gameState != GameState.IDLE && m_timeLeft <= 0.0f)
        {
            m_gameState = GameState.IDLE;
            StartCoroutine(ShowMessageRoutine("Sorry, time's up! Your current score: " + m_score, 1, onTimeUp));
        }

    }

    public void onScoreEvent()
    {
        m_score++;
        if (m_currentTry < m_tries)
        {
            StartCoroutine(ShowMessageRoutine("You scored! Current score: " + m_currentTry, 1, onEndTry));
        }
        else if (m_currentTry == m_tries)
        {
            m_gameState = GameState.REPORTSCORE;
        }
    }

    public void onMissEvent()
    {
        if (m_currentTry < m_tries)
        {
            StartCoroutine(ShowMessageRoutine("You missed... Current score: " + m_currentTry, 1, onEndTry));
        }
        else if (m_currentTry == m_tries)
        {
            m_gameState = GameState.REPORTSCORE;
        }
    }

    IEnumerator ShowMessageRoutine(string message, float delay, Action i_callback)
    {
        m_text.text = message;
        m_text.enabled = true;
        yield return new WaitForSeconds(delay);
        m_text.enabled = false;

        if (i_callback != null)
        {
            i_callback();
        }
    }

    private void onEndTry()
    {
        ResetScene();
    }

    private void onTimeUp()
    {
        if (m_currentTry < m_tries)
        {
            ResetScene();
        }
        else if (m_currentTry == m_tries)
        {
            m_gameState = GameState.REPORTSCORE;
        }
    }

    private void onEndGame()
    {
        SceneEnded(m_score / m_tries);
    }

}
