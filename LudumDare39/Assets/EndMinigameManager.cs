using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMinigameManager : AbstarcMinigameManager {

    [SerializeField]
    private Text m_prevGameName;

    [SerializeField]
    private Slider m_energySlider;

    [SerializeField]
    private Slider m_moneySlider;

    [SerializeField]
    private Slider m_socialSlider;

    [SerializeField]
    private float m_duration = 1.0f;

    private float[] m_oldValues;
    private float[] m_newValues = new float[3];
    private float m_startTime = 0.0f;
    private float m_timer = 0.0f;

    public override void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue)
    {
        //m_prevGameName.text = SceneLoaderSingleManager.Instance.ActualScene();
        m_prevGameName.text = GameManager.Instance.m_currentSceneName;
        m_oldValues = GameManager.Instance.OldValues;

        m_newValues[0] = i_TimeValue * 100;
        m_newValues[1] = i_NumItemValue * 100;
        m_newValues[2] = i_ItemVelocityValue * 100;

        m_startTime = Time.time;
    }

    private void Update()
    {
        if(m_timer<=1)
        {
        m_timer += Time.deltaTime;
        m_energySlider.value = Mathf.SmoothStep(m_oldValues[0], m_newValues[0], m_timer);
        m_moneySlider.value = Mathf.SmoothStep(m_oldValues[1], m_newValues[1], m_timer);
        m_socialSlider.value = Mathf.SmoothStep(m_oldValues[2], m_newValues[2], m_timer);
        }
    }
    public void OnContinue()
    {
        SceneEnded(0);
    }
}
