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

    public float m_currentPsychophysicsValue = 0.0f;

    public float m_currentMoneyValue = 0.0f;

    public float m_currentSocialValue = 0.0f;

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
    public void LoadNextScene()
    {
        m_sceneLoaderManager.LoadNextScene();
    }
}

