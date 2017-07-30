using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    [SerializeField]
    private float m_maxValue = 15.0f;
    [SerializeField]
    private float m_minValue = 5.0f;

    [SerializeField]
    private float m_baseIncrement = 0.1f;
    private float m_adjustedIncrement;

    private float m_value;
    private bool increment = true;
    private float m_offset;
    private float m_adjMaxValue;

    private Vector3 m_fillerInitialPosition;
    public GameObject m_filler;

    public void Initialize(float i_difficultySpeed)
    {
        m_value = m_minValue;
        m_offset = m_minValue;
        m_adjMaxValue = m_maxValue - m_offset;
        m_fillerInitialPosition = m_filler.transform.position;
        m_adjustedIncrement = m_baseIncrement + i_difficultySpeed;
    }

    public void Reset()
    {
        m_value = m_minValue;
        m_filler.transform.localScale = new Vector3(0.5f, 0.0f, 1);
        m_filler.transform.position = m_fillerInitialPosition;
    }

    public void UpdateValue()
    {


        if (increment)
        {
            m_value += m_adjustedIncrement;           
            float yScale = ((m_value - m_offset)* 2.5f) / m_adjMaxValue;
            m_filler.transform.localScale = new Vector3(0.5f, yScale, 1);
            m_filler.transform.Translate(new Vector3(0, 1.25f/ (m_adjMaxValue/ m_adjustedIncrement), 0));
        } else
        {
            m_value -= m_adjustedIncrement;
            float yScale = ((m_value - m_offset)* 2.5f) / m_adjMaxValue;
            m_filler.transform.localScale = new Vector3(0.5f, yScale, 1);
            m_filler.transform.Translate(new Vector3(0, -1.25f / (m_adjMaxValue/ m_adjustedIncrement), 0));
        }

        if (m_value >= m_maxValue)
        {
            increment = false;
        }

        if (m_value <= m_minValue)
        {
            increment = true;
        }
    }

    public float GetCurrentValue()
    {
        return m_value;
    }
}
