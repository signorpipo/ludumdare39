using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void F_TimesUpEvent();

public class F_Timer : MonoBehaviour
{

    private event F_TimesUpEvent OnGameStart;
    private event F_TimesUpEvent OnTimesUp;

    [SerializeField]
    private GameObject m_LevelTimer;
    [SerializeField]
    private GameObject m_TitleText;
    [SerializeField]
    private GameObject m_StartText;
    [SerializeField]
    private GameObject m_EndText;

    private float m_TimerSeconds = 10;

    private string m_StartTitle = "Title";
    private string m_StartMessage = "Start!";
    private string m_TimesUpMessage = "Times Up!";

    private float m_ElapsedTime = 0.0f;
    private bool m_GameStarted = false;
    private float m_Seconds;
    private bool m_IsRunningMode = false;

    public void StartTimer(F_TimesUpEvent i_OnGameStart, F_TimesUpEvent i_OnTimesUp, string i_StartTitle, string i_StartMessage, int i_GameStartSeconds, string i_TimesUpMessage, int i_TimerSeconds)
    {
        OnGameStart += i_OnGameStart;
        OnTimesUp += i_OnTimesUp;
        m_StartTitle = i_StartTitle;
        m_StartMessage = i_StartMessage;
        m_TimesUpMessage = i_TimesUpMessage;
        m_TimerSeconds = i_TimerSeconds;

        m_IsRunningMode = true;
        m_Seconds = i_GameStartSeconds;
        if (m_StartMessage.Length > 0)
        {
            m_StartText.GetComponentInChildren<Text>().text = m_StartMessage;
            m_StartText.SetActive(true);
            m_TitleText.GetComponentInChildren<Text>().text = m_StartTitle;
            m_TitleText.SetActive(true);
        }
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
            if (m_ElapsedTime >= 1.0f)
            {
                --m_Seconds;
                m_ElapsedTime = 0.0f;

                if (m_GameStarted)
                {
                    m_LevelTimer.GetComponentInChildren<Text>().text = "Time Left: " + (int)m_Seconds;
                }
            }

            if (m_Seconds <= 0)
            {
                if (!m_GameStarted)
                {
                    m_GameStarted = true;
                    m_Seconds = m_TimerSeconds;

                    m_StartText.SetActive(false);
                    m_TitleText.SetActive(false);

                    m_LevelTimer.GetComponentInChildren<Text>().text = "Time Left: "+m_Seconds;
                    m_LevelTimer.SetActive(true);

                    if (OnGameStart != null)
                    {
                        OnGameStart();
                    }

                }
                else
                {
                    m_IsRunningMode = false;

                    if (m_TimesUpMessage.Length > 0)
                    {
                        m_EndText.GetComponentInChildren<Text>().text = m_TimesUpMessage;
                        m_EndText.SetActive(true);
                    }

                    if (OnTimesUp != null)
                    {
                        OnTimesUp();
                    }
                }

            }
        }
        else
        {
            m_ElapsedTime = 0.0f;
        }
    }
    
    public int GetSecondsLeft()
    {
        return (int)m_Seconds;
    }

    public void DecreaseTime(float i_Delta)
    {
        m_Seconds -= i_Delta;
    }
}
