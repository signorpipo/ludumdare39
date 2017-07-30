using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private float m_potency;
    private Vector3 m_direction;

    public void SetPotency(float i_potency)
    {
        m_potency = i_potency;
    }

    public void SetDirection(Vector2 i_direction)
    {
        m_direction = i_direction;
    }

    public void Move()
    {
        this.GetComponent<Rigidbody2D>().AddForce(m_direction.normalized * m_potency, ForceMode2D.Impulse);
    }
}
