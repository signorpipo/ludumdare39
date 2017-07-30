using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ScoreEvent();

public class Basket : MonoBehaviour {
    public GameObject m_colliderLeft;
    public GameObject m_colliderRight;
    public ScoreEvent onScoreEvent;

    public void Initialize()
    {
        m_colliderLeft.GetComponent<EdgeCollider2D>().enabled = false;
        m_colliderRight.GetComponent<EdgeCollider2D>().enabled = false;
    }

    public void Reset()
    {
        m_colliderLeft.GetComponent<EdgeCollider2D>().enabled = false;
        m_colliderRight.GetComponent<EdgeCollider2D>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        m_colliderLeft.GetComponent<EdgeCollider2D>().enabled = true;
        m_colliderRight.GetComponent<EdgeCollider2D>().enabled = true;

        if (onScoreEvent != null)
        {
            onScoreEvent();
        }

    }
}
