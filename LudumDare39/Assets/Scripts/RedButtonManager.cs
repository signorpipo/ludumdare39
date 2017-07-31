using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedButtonManager : AbstarcMinigameManager {

    [SerializeField]
    private Color[] m_buttonType;

    [SerializeField]
    private Image m_buttonImage;

    [SerializeField]
    private CM_Timer m_Timer;

    [SerializeField]
    private Text m_TimerText;

    [SerializeField]
    private Text m_Tutorial;

    private float timer = 3.0f;
    public override void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue)
    {
        m_buttonImage.color = m_buttonType[i_Type];
        timer += i_Type * 7;

        m_Timer.StartTimer((int)timer, "", "", m_TimerText);
        m_Timer.OnTimesUp += Patience;

        m_Tutorial.text = "Don't press the ";
        switch(i_Type)
        {
            case 0: m_Tutorial.text += "Red"; break;
            case 1: m_Tutorial.text += "Blue"; break;
            case 2: m_Tutorial.text += "Green"; break;
        }
        m_Tutorial.text += " Button";
    }

    public void Patience()
    {
        SceneEnded(1);
    }

    public void Impatience()
    {
        SceneEnded(0);
    }
}
