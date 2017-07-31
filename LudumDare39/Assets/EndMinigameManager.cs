using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMinigameManager : AbstarcMinigameManager {

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
        m_oldValues = GameManager.Instance.OldValues;

        m_newValues[0] = i_TimeValue ;
        m_newValues[1] = i_NumItemValue;
        m_newValues[2] = i_ItemVelocityValue;

        m_startTime = Time.time;
        //StartCoroutine("slideSlider");
    }

    private void Update()
    {
        if(m_timer<=1)
        {
        m_timer += Time.deltaTime;
        //float t = (Time.time - m_startTime) / m_duration;
        m_energySlider.value = Mathf.SmoothStep(m_oldValues[0], m_newValues[0], m_timer);
        m_moneySlider.value = Mathf.SmoothStep(m_oldValues[1], m_newValues[1], m_timer);
        m_socialSlider.value = Mathf.SmoothStep(m_oldValues[2], m_newValues[2], m_timer);
        }
    }


    /* private IEnumerator slideSlider()
     {
         float t = (Time.time - m_startTime) / m_duration;
         while(t<=1)
         {
             m_energySlider.value = Mathf.SmoothStep(m_oldValues[0], m_newValues[0], t);
             m_moneySlider.value = Mathf.SmoothStep(m_oldValues[1], m_newValues[1], t);
             m_socialSlider.value = Mathf.SmoothStep(m_oldValues[2], m_newValues[2], t);
             yield return null;
         }

     }*/

    public void OnContinue()
    {
        SceneEnded(0);
    }
}
