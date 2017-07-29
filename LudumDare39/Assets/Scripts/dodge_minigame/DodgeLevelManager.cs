using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeLevelManager : MonoBehaviour {

    [SerializeField]
    private int m_TimeLeft = 50;

    [SerializeField]
    private Text m_TimeLeftUI;

    [SerializeField]
    private GameObject m_Player;

    private float m_ElapsedTime = 0;

    private void Start()
    {
        m_TimeLeftUI.text = "Time left: " + m_TimeLeft;

        m_Player.GetComponent<PlayerController>().OnPlayerFailure += OnPlayerFailure;
    }

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;
        if( m_ElapsedTime >= 1 && m_TimeLeft != 0)
        {
            m_TimeLeftUI.text = "Time left: " + --m_TimeLeft;

            m_ElapsedTime = 0;
        }

        if(m_TimeLeft == 0)
        {
            // Level is finished
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);      
    }


    private void OnPlayerFailure()
    {
        Debug.Log("Player Failure");
    }

}
