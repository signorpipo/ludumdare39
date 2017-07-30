using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsCalculator : MonoBehaviour
{
    [SerializeField]
    private GameManager m_gameManager = null;

    [Header("Front Sliders")]

    [SerializeField]
    private Slider m_leftSliderFront = null;

    [SerializeField]
    private Slider m_middleSliderFront = null;

    [SerializeField]
    private Slider m_rightSliderFront = null;

    [Header("Back Sliders")]

    [SerializeField]
    private Slider m_leftSliderBack = null;

    [SerializeField]
    private Slider m_middleSliderBack = null;

    [SerializeField]
    private Slider m_rightSliderBack = null;

    // Use this for initialization
    void Start()
    {
        SetStaticSliders();
    }

    public void UpdateSliders(float psychophysicsValue, float moneyValue, float socialValue)
    {
        m_leftSliderFront.value += psychophysicsValue;
        m_middleSliderFront.value += moneyValue;
        m_rightSliderFront.value += socialValue;
    }

    public void SetStaticSliders()
    {
        m_leftSliderBack.value = m_gameManager.CurrentPsychophysicsValue;
        m_middleSliderBack.value = m_gameManager.CurrentMoneyValue;
        m_rightSliderBack.value = m_gameManager.CurrentSocialValue;

        ResetSliders();
    }

    public void ResetSliders()
    {
        m_leftSliderFront.value = m_gameManager.CurrentPsychophysicsValue;
        m_middleSliderFront.value = m_gameManager.CurrentMoneyValue;
        m_rightSliderFront.value = m_gameManager.CurrentSocialValue;
    }
}
