using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
   GameManager m_gameManager;

    //[SerializeField]
    private GameObject m_sceneLoader = null;

    /*[SerializeField]
    private MenuManager m_menuManager = null;*/

    //[SerializeField]
    private float m_currentPsychophysicsValue = 20.0f;
    //[SerializeField]
    private float m_currentMoneyValue = 20.0f;
    //[SerializeField]
    private float m_currentSocialValue = 20.0f;

    private int count = 0;

    // Initialized/updated when start button has been pressed, cleared when 3 minigames have been played
    private List<MinigameInterface> m_selectedMinigames = new List<MinigameInterface>();

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

    private SceneLoaderSingleManager m_sceneLoaderManager = null;

    void Start()
    {
        //m_gameManager = GameManager.Instance;

    }

    public void SetSelectedMiniGames(List<MinigameInterface> selectedMinigames)
    {
        m_selectedMinigames = selectedMinigames;
    }

    public void StartGame()
    {
        m_sceneLoaderManager = SceneLoaderSingleManager.Instance;

        List<string> selectedScenesNames = new List<string>();

        for (int index = 0; index < m_selectedMinigames.Count; ++index)
        {
            selectedScenesNames.Add(m_selectedMinigames[index].GetName());
        }

        m_sceneLoaderManager.SetSelectedScenesNames(selectedScenesNames);
        m_sceneLoaderManager.LoadNextScene();

        SceneManager.sceneLoaded += SceneLoaded;
    }

    /*public void StartGame()
    {
        m_selectedMinigames = m_menuManager.GetSelectedMinigamesList();
        int countl = m_menuManager.GetSelectedMinigamesList().Count;
 
        if (countl == 3)
        {
            List<string> selectedScenesNames = new List<string>();

            for (int index = 0; index < countl; ++index)
            {
                selectedScenesNames.Add(m_selectedMinigames[index].GetName());
            }

            m_sceneLoaderManager.SetSelectedScenesNames(selectedScenesNames);
            m_sceneLoaderManager.LoadNextScene();

            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            Debug.Log("Gesù è l'unico e vero Signore");
        }
    }*/

    private void SceneLoaded(Scene i_scene, LoadSceneMode i_mode)
    {
        AbstarcMinigameManager CurrentGame = FindObjectOfType<AbstarcMinigameManager>();
        CurrentGame.StartMinigame(m_selectedMinigames[count].GetGameVersion(), m_currentPsychophysicsValue, m_currentMoneyValue, m_currentSocialValue);
        CurrentGame.onSceneEnded += LoadNextSceneAndUpdateStats;
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    public void ClearSelectedMinigames()
    {
        m_selectedMinigames.Clear();
        count = 0;
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    /// <summary>
    /// Load next scene from scene load manager class
    /// </summary>
    public void LoadNextSceneAndUpdateStats(float resultMutator)
    {
        if (resultMutator > 0)
        {
            if (m_selectedMinigames[count].GetPsychophysicsOutputValue() >= 0)
            {
                m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + m_selectedMinigames[count].GetPsychophysicsOutputValue() * resultMutator, 0.0f, 100.0f);
            }
            else
            {
                m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + m_selectedMinigames[count].GetPsychophysicsOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[count].GetMoneyOutputValue() >= 0)
            {
                m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + m_selectedMinigames[count].GetMoneyOutputValue() * resultMutator, 0.0f, 100.0f);
            }
            else
            {
                m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + m_selectedMinigames[count].GetMoneyOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[count].GetSocialOutputvalue() >= 0)
            {
                m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + m_selectedMinigames[count].GetSocialOutputvalue() * resultMutator, 0.0f, 100.0f);
            }
            else
            {
                m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + m_selectedMinigames[count].GetSocialOutputvalue(), 0.0f, 100.0f);
            }
        }
        else
        {
            if (m_selectedMinigames[count].GetPsychophysicsOutputValue() < 0)
            {
                m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + m_selectedMinigames[count].GetPsychophysicsOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[count].GetMoneyOutputValue() < 0)
            {
                m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + m_selectedMinigames[count].GetMoneyOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[count].GetSocialOutputvalue() < 0)
            {
                m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + m_selectedMinigames[count].GetSocialOutputvalue(), 0.0f, 100.0f);
            }
        }
        
        count++;
        if (count == 3)
        {
            ClearSelectedMinigames();
        }
        else
        {
             SceneManager.sceneLoaded += SceneLoaded;
        }

        m_sceneLoaderManager.LoadNextScene();
    }
}

