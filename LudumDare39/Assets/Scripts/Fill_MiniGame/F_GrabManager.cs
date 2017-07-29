using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TryDockEvent(F_Pluggable i_ToDock);

public class F_GrabManager : MonoBehaviour {
    
    public TryDockEvent OnTryDockEvent;

    public F_Pluggable[] m_Grabbables;

    private F_Pluggable m_Grabbed;

    public void Start()
    {
        foreach(F_Pluggable grabbable in m_Grabbables)
        {
            grabbable.OnClickEvent += GrabObject;
        }
    }

    private void Update()
    {
        if(m_Grabbed != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(OnTryDockEvent != null)
                {
                    OnTryDockEvent(m_Grabbed);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ReleaseGrabbed();
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
                m_Grabbed.gameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, 1);
            }
        }
    }

    public void ReleaseGrabbed()
    {
        m_Grabbed.GoToInitial();
        m_Grabbed = null;
    }

    public void GrabObject(F_Pluggable i_ToGrab)
    {
        m_Grabbed = i_ToGrab;
    }

}
