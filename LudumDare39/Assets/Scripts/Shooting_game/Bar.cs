using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    public float MAX_VALUE = 15.0f;
    public float MIN_VALUE = 5.0f;
    private bool increment = true;
    private float m_value;
    public float m_increment = 0.4f;
    private float m_offset;
    private float m_adjMaxValue;
    public GameObject m_filler;

    public void Initialize()
    {
        m_value = MIN_VALUE;
        m_offset = MIN_VALUE;
        m_adjMaxValue = MAX_VALUE - m_offset;
    }
    public void UpdateValue()
    {


        if (increment)
        {
            m_value += m_increment;           
            float yScale = ((m_value - m_offset)* 2.5f) / m_adjMaxValue;
            m_filler.transform.localScale = new Vector3(0.5f, yScale, 1);
            m_filler.transform.Translate(new Vector3(0, 1.25f/ (m_adjMaxValue/ m_increment), 0));
        } else
        {
            m_value -= m_increment;
            float yScale = ((m_value - m_offset)* 2.5f) / m_adjMaxValue;
            m_filler.transform.localScale = new Vector3(0.5f, yScale, 1);
            m_filler.transform.Translate(new Vector3(0, -1.25f / (m_adjMaxValue/ m_increment), 0));
        }

        if (m_value >= MAX_VALUE)
        {
            increment = false;
        }

        if (m_value <= MIN_VALUE)
        {
            increment = true;
        }
    }

    public float GetCurrentValue()
    {
        return m_value;
    }
}
