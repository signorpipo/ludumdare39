using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinigameInterface
{
    [SerializeField]
    private string m_name = "Default";

    [SerializeField]
    private int m_type = 0;

    [SerializeField]
    private float m_psychophysicsOutputValue = 0.0f;

    [SerializeField]
    private float m_moneyOutputValue = 0.0f;

    [SerializeField]
    private float m_socialOutputValue = 0.0f;

    public string GetName()
    {
        return m_name;
    }

    public int GetGameVersion()
    {
        return m_type;
    }

    public float GetPsychophysicsOutputValue()
    {
        return m_psychophysicsOutputValue;
    }

    public float GetMoneyOutputValue()
    {
        return m_moneyOutputValue;
    }

    public float GetSocialOutputvalue()
    {
        return m_socialOutputValue;
    }
}
