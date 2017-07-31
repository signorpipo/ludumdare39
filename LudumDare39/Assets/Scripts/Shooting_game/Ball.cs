using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Vector3 m_initialPosition;
    private float m_potency;
    private Vector3 m_direction;

    public void Initialize()
    {
        m_initialPosition = transform.position;
    }

    public void Reset()
    {
        GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().inertia = 0.0f;
        transform.position = m_initialPosition;
    }

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
