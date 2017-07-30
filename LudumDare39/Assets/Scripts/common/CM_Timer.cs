using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void TimesUp();

public class CM_Timer : MonoBehaviour {

    [SerializeField]
    private Text m_UIText;
    [SerializeField]
    private int m_Seconds = 10;

    [SerializeField]
    private bool m_IsRunningMode = false;

    [SerializeField]
    private string m_PrefixMessage = "";
    [SerializeField]
    private string m_PostfixMessage = "";

    private float m_ElapsedTime = 0.0f;

    public event TimesUp OnTimesUp;




    public void StartTimer(int i_Seconds, string i_PrefixMessage, string i_PostfixMessage, Text i_UIText)
    {
        m_Seconds = i_Seconds;
        m_PrefixMessage = i_PrefixMessage;
        m_PostfixMessage = i_PostfixMessage;
        m_UIText = i_UIText;
        m_IsRunningMode = true;
    }

    public void StopTimer()
    {
        m_IsRunningMode = false;
    }

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        if (m_IsRunningMode)
        {
            /* Handle Timer */
            if(m_ElapsedTime >= 1.0f)
            {
                --m_Seconds;
                m_ElapsedTime = 0.0f;
            }

            if(m_UIText != null)
            {
                m_UIText.text = m_PrefixMessage + m_Seconds + m_PostfixMessage;
            }

            if( m_Seconds <= 0)
            {
                m_IsRunningMode = false;
                if (OnTimesUp != null)
                    OnTimesUp();

            }
        }
        else
        {
            m_ElapsedTime = 0.0f;
        }
    }




}
