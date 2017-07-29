using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCalculator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_menuManagerPrefab = null;

    private MenuManager m_menuManager = null;

    private float m_finalPsychophysicsOutputValue = 0.0f;

    private float m_finalMoneyOutputValue = 0.0f;

    private float m_finalSocialOutputValue = 0.0f;

    // Use this for initialization
    void Start ()
    {
        m_menuManager = m_menuManagerPrefab.GetComponent<MenuManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        List<MinigameInterface> m_seletedMinigames = m_menuManager.GetSelectedMinigamesList();
        int numberOfMinigames = m_seletedMinigames.Count;

        for (int index = 0; index < numberOfMinigames; ++index)
        {
            m_finalPsychophysicsOutputValue += m_seletedMinigames[index].GetPsychophysicsOutputValue();
            m_finalMoneyOutputValue += m_seletedMinigames[index].GetMoneyOutputValue();
            m_finalSocialOutputValue += m_seletedMinigames[index].GetSocialOutputvalue();
        }

        m_finalPsychophysicsOutputValue /= numberOfMinigames;
        m_finalMoneyOutputValue /= numberOfMinigames;
        m_finalSocialOutputValue /= numberOfMinigames;
    }
}
