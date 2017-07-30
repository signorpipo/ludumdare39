using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGame : MonoBehaviour {
    private float m_energy;
    private float m_money;
    private float m_love;


    private Arrow m_directionArrow;
    public Arrow m_arrowPrefab;
    public Bar m_potencyBar;

    public Ball m_ballPrefab;
    private Ball m_ball;

    private enum GameState
    {
        POTENCY,
        DIRECTION,
        SHOOT,
        END
    }

    private GameState m_gameState;
    private void Start()
    {

        m_directionArrow = Instantiate(m_arrowPrefab);
        m_potencyBar.Initialize();
        m_ball = Instantiate(m_ballPrefab);
        m_gameState = GameState.DIRECTION;
    }

    // Update is called once per frame
    void Update () {
        if (m_gameState == GameState.DIRECTION)
        {
            m_directionArrow.UpdateDirection();
            if (Input.GetMouseButtonDown(0))
            {
                m_ball.SetDirection(m_directionArrow.GetCurrentDirection());
                m_gameState = GameState.POTENCY;
               
            }
        } else if (m_gameState == GameState.POTENCY)
        {            
            m_potencyBar.UpdateValue();
            if (Input.GetMouseButtonDown(0))
            {
                m_ball.SetPotency(m_potencyBar.GetCurrentValue());
                m_gameState = GameState.SHOOT;
            }
        } else if (m_gameState == GameState.SHOOT)
        {
            m_ball.Move();
            m_gameState = GameState.END;
        }

	}
    
}
