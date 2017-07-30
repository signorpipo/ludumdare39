using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    [SerializeField]
    private Vector3 m_baseUnitRotation = new Vector3(0, 0, 1);
    [SerializeField]
    private float adjustmentFactor = 2;

    private Vector3 m_adjustedUnitRotation;

    private bool m_invert = false;

    public void Initialize(float i_difficultySpeed)
    {
        m_adjustedUnitRotation = new Vector3(0, 0, m_baseUnitRotation.z + (adjustmentFactor * i_difficultySpeed));
    }

    public void Reset()
    {
        m_invert = false;
        transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void UpdateDirection()
    {
        if (transform.rotation.eulerAngles.z >= 90)
        {
            m_invert = true;
        } else if (transform.rotation.eulerAngles.z <= 1)
        {
            m_invert = false;
        }

        if (m_invert)
        {
            transform.Rotate(-m_adjustedUnitRotation);
        } else
        {
            transform.Rotate(m_adjustedUnitRotation);
        }
    }

    public Vector2 GetCurrentDirection()
    {
        return transform.right;
    
    }
}
