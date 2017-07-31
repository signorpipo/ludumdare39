using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void F_TryDockEvent(F_Pluggable i_ToDock);

public class F_GrabManager : MonoBehaviour {
    
    public F_TryDockEvent OnTryDockEvent;

    private F_Pluggable[] m_Grabbables;

    private F_Pluggable m_Grabbed;

    private bool m_SkipFirst;
    private bool m_Enabled;

    public void Initialize(F_Pluggable[] i_Grabbables)
    {
        m_Grabbables = i_Grabbables;
        foreach (F_Pluggable grabbable in m_Grabbables)
        {
            grabbable.OnClickEvent += GrabObject;
        }
    }

    private void Update()
    {
        if(m_Grabbed != null && m_Enabled)
        {
            if (m_SkipFirst)
            {
                m_SkipFirst = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if(OnTryDockEvent != null)
                {
                    OnTryDockEvent(m_Grabbed);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                m_Grabbed.Turn(F_Direction.RIGHT);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                m_Grabbed.Turn(F_Direction.LEFT);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                m_Grabbed.Turn(F_Direction.RIGHT);
            }
            else
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_Grabbed.gameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0.9f);
            }
        }
    }

    public void DisableInput()
    {
        m_Enabled = false;
    }

    public void EnableInput()
    {
        m_Enabled = true;
    }

    public void ReleaseGrabbed(bool i_ToInitial)
    {
        if (i_ToInitial)
        {
            m_Grabbed.GoToInitial();
        }
        m_Grabbed.transform.position = new Vector3(m_Grabbed.transform.position.x, m_Grabbed.transform.position.y, 1);
        m_Grabbed = null;
    }

    public void GrabObject(F_Pluggable i_ToGrab)
    {
        if(!IsGrabbing() && m_Enabled)
        {
            m_Grabbed = i_ToGrab;
            m_SkipFirst = true;
        }
    }

    public bool IsGrabbing()
    {
        return m_Grabbed != null;
    }

    public F_Pluggable GetGrabbed()
    {
        return m_Grabbed;
    }

}
