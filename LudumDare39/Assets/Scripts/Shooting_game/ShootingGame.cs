using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGame : MonoBehaviour {
    private float m_energy;
    private float m_money;
    private float m_love;


    private Arrow m_directionArrow;
    public Arrow m_arrowPrefab;
    public Bar m_potencyBarPrefab;
    private Bar m_potencyBar;
    public Basket m_basketPrefab;
    private Basket m_basket;

    public Ball m_ballPrefab;
    private Ball m_ball;

    public Walls m_floor;
    public Walls m_wall;


    private enum GameState
    {
        POTENCY,
        DIRECTION,
        SHOOT,
        END,
        WIN,
        LOSE
    }

    private GameState m_gameState;
    private void Start()
    {

        m_directionArrow = Instantiate(m_arrowPrefab);

        m_potencyBar = Instantiate(m_potencyBarPrefab);
        m_potencyBar.Initialize();

        m_basket = Instantiate(m_basketPrefab);
        m_basket.Initialize();

        m_ball = Instantiate(m_ballPrefab);

        m_gameState = GameState.DIRECTION;
    }

    // Update is called once per frame
    void Update () {
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
                m_gameState = GameState.END;
                break;
            case GameState.END:
                m_basket.onWinEvent += onWinEvent;
                m_wall.onLoseEvent += onLoseEvent;
                m_floor.onLoseEvent += onLoseEvent;
                break;
            case GameState.WIN:
                Debug.Log("You win");            
                break;
            case GameState.LOSE:
                Debug.Log("You lose");
                break;

        }

	}
    
    public void onWinEvent()
    {
        m_gameState = GameState.WIN;
    }

    public void onLoseEvent()
    {
        m_gameState = GameState.LOSE;
    }

    /*private void OnGUI()
    {
        if (m_gameState == GameState.WIN)
        {
            GUI.Label(new Rect(0, 0, 100, 100), "You win");
        }
        else if (m_gameState == GameState.LOSE)
        {
            GUI.Label(new Rect(0, 0, 100, 100), "You lose");
        }
    }*/
}
