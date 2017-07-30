using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Singleton<GameManager> m_gameManager;

    [SerializeField]
    private GameObject m_sceneLoader = null;

    [SerializeField]
    private List<string> m_scenesNames = null;

    [SerializeField]
    private float m_currentPsychophysicsValue = 0.0f;
    [SerializeField]
    private float m_currentMoneyValue = 0.0f;
    [SerializeField]
    private float m_currentSocialValue = 0.0f;

    public float CurrentPsychophysicsValue
    {
        get
        {
            return m_currentPsychophysicsValue;
        }
    }
    public float CurrentMoneyValue
    {
        get
        {
            return m_currentMoneyValue;
        }
    }
    public float CurrentSocialValue
    {
        get
        {
            return m_currentSocialValue;
        }
    }

    private SceneLoaderManager m_sceneLoaderManager = null;

    void Start()
    {
        m_sceneLoaderManager = m_sceneLoader.GetComponent<SceneLoaderManager>();
        m_sceneLoaderManager.SetAllScenesNames(m_scenesNames);
        m_sceneLoaderManager.LoadAllScenes();
    }

    /// <summary>
    /// Load next scene from scene load manager class
    /// </summary>
    public void LoadNextScene(float PsychophysicsChange, float MoneyChange, float SocialChange)
    {
        m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + PsychophysicsChange, 0.0f, 100.0f);
        m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + MoneyChange, 0.0f, 100.0f);
        m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + SocialChange, 0.0f, 100.0f);

        m_sceneLoaderManager.LoadNextScene();
    }
}

