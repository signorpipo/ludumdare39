using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Scene
{
    [SerializeField]
    private string m_name;
}

public class GameManager : MonoBehaviour
{
    Singleton<GameManager> m_gameManager;

    [SerializeField]
    private GameObject m_sceneLoader = null;

    [SerializeField]
    private List<Scene> m_scenesNames = null;

    public float m_currentPsychophysicsValue = 0.0f;

    public float m_currentMoneyValue = 0.0f;

    public float m_currentSocialValue = 0.0f;

    private SceneLoaderManager m_sceneLoaderManager = null;

    void Start()
    {
        m_sceneLoaderManager = m_sceneLoader.GetComponent<SceneLoaderManager>();
        m_sceneLoaderManager.LoadScenes();
    }

    void Update()
    {

    }

    public void LoadNextScene()
    {

    }
}

