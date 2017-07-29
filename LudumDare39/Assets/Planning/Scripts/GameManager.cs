using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Singleton<GameManager> m_gameManager;

    public GameObject m_sceneLoader = null;

    public float m_currentPsychophysicsValue = 0.0f;

    public float m_currentMoneyValue = 0.0f;

    public float m_currentSocialValue = 0.0f;

    private m_sceneLoaderManager = null;

    void Start()
    {
        m_sceneLoaderManager = m_sceneLoader.GetComponent<SceneLoaderManager>();
    }

    void Update()
    {

    }

    public void LoadNextScene()
    {

    }
}

