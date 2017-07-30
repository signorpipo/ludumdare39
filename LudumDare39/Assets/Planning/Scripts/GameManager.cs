using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    GameManager m_gameManager;

    private GameObject m_sceneLoader = null;

    private float m_currentPsychophysicsValue = 20.0f;

    private float m_currentMoneyValue = 20.0f;

    private float m_currentSocialValue = 20.0f;

    private int m_selectedGamesCounter = 0;

    public int m_weekDaysCounter = 0;

    // Initialized/updated when start button has been pressed, cleared when 3 minigames have been played
    private List<MinigameInterface> m_selectedMinigames = new List<MinigameInterface>();

    private List<DailyProperties> m_weekDays = new List<DailyProperties>();

    private SceneLoaderSingleManager m_sceneLoaderManager = null;

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

    public void SetSelectedMiniGames(List<MinigameInterface> selectedMinigames)
    {
        m_selectedMinigames = selectedMinigames;
    }

    public void SetWeekDays(List<DailyProperties> weekDays)
    {
        m_weekDays = weekDays;
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

    private void SceneLoaded(Scene i_scene, LoadSceneMode i_mode)
    {
        AbstarcMinigameManager CurrentGame = FindObjectOfType<AbstarcMinigameManager>();
        CurrentGame.StartMinigame(m_selectedMinigames[m_selectedGamesCounter].GetGameVersion(), m_currentPsychophysicsValue, m_currentMoneyValue, m_currentSocialValue);
        CurrentGame.onSceneEnded += LoadNextSceneAndUpdateStats;
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    public void ClearSelectedMinigames()
    {
        m_selectedMinigames.Clear();
        m_selectedGamesCounter = 0;
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    /// <summary>
    /// Load next scene from scene load manager class
    /// </summary>
    public void LoadNextSceneAndUpdateStats(float resultMutator)
    {
        if (resultMutator > 0)
        {
            if (m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue() >= 0)
            {
                m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue() * resultMutator * 
                    m_weekDays[m_weekDaysCounter].m_psychophysicsBonus, 
                    0.0f, 100.0f);
            }
            else
            {
                m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() >= 0)
            {
                m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() * resultMutator *
                    m_weekDays[m_weekDaysCounter].m_moneyBonus, 0.0f, 100.0f);
            }
            else
            {
                m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue() >= 0)
            {
                m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue() * resultMutator *
                    m_weekDays[m_weekDaysCounter].m_socialBonus, 0.0f, 100.0f);
            }
            else
            {
                m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue(), 0.0f, 100.0f);
            }
        }
        else
        {
            if (m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue() < 0)
            {
                m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() < 0)
            {
                m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue(), 0.0f, 100.0f);
            }

            if (m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue() < 0)
            {
                m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue(), 0.0f, 100.0f);
            }
        }
        
        m_selectedGamesCounter++;
        if (m_selectedGamesCounter == 3)
        {
            ClearSelectedMinigames();
            m_weekDaysCounter++;
        }
        else
        {
             SceneManager.sceneLoaded += SceneLoaded;
        }

        m_sceneLoaderManager.LoadNextScene();
    }
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
