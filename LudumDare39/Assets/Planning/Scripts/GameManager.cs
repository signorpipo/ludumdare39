using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    GameManager m_gameManager;

    private GameObject m_sceneLoader = null;

    const float m_defaultPsychophysicsValue = 60.0f;

    const float m_defaultMoneyValue = 60.0f;

    const float m_defaultSocialValue = 60.0f;

    private float m_currentPsychophysicsValue = m_defaultPsychophysicsValue;

    private float m_currentMoneyValue = m_defaultMoneyValue;

    private float m_currentSocialValue = m_defaultSocialValue;

    private int m_selectedGamesCounter = 0;

    public int m_weekDaysCounter = 0;

    private float[] m_oldValues = new float[3];

    public float[] OldValues
    {
        get { return m_oldValues; }
    }

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
        CurrentGame.StartMinigame(m_selectedMinigames[m_selectedGamesCounter].GetGameVersion(), m_currentPsychophysicsValue / 100, m_currentMoneyValue / 100, m_currentSocialValue / 100);
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
        if (!m_sceneLoaderManager.IsMinigame)
        {
            m_oldValues[0] = m_currentPsychophysicsValue;
            m_oldValues[1] = m_currentMoneyValue;
            m_oldValues[2] = m_currentSocialValue;

            if (resultMutator > 0)
            {
                if (m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue() > 0)
                {
                    m_currentPsychophysicsValue = Mathf.Clamp(m_currentPsychophysicsValue + m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue() * resultMutator +
                        /*m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue() * resultMutator **/ m_weekDays[m_weekDaysCounter].m_psychophysicsBonus / 3.0f, 0.0f, 100.0f);
                }
                else
                {
                    m_currentPsychophysicsValue = CalculateLosingValue(m_currentPsychophysicsValue, m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue());
                }

                if (m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() > 0)
                {
                    m_currentMoneyValue = Mathf.Clamp(m_currentMoneyValue + m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() * resultMutator +
                        /*m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() * resultMutator **/ m_weekDays[m_weekDaysCounter].m_moneyBonus / 3.0f, 0.0f, 100.0f);
                }
                else
                {
                    m_currentMoneyValue = CalculateLosingValue(m_currentMoneyValue, m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue());
                }

                if (m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue() > 0)
                {
                    m_currentSocialValue = Mathf.Clamp(m_currentSocialValue + m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue() * resultMutator +
                        /*m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() * resultMutator **/ m_weekDays[m_weekDaysCounter].m_socialBonus / 3.0f, 0.0f, 100.0f);
                }
                else
                {
                    m_currentSocialValue = CalculateLosingValue(m_currentSocialValue, m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue());
                }
            }
            else
            {
                if (m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue() < 0)
                {
                    m_currentPsychophysicsValue = CalculateLosingValue(m_currentPsychophysicsValue, m_selectedMinigames[m_selectedGamesCounter].GetPsychophysicsOutputValue());
                }

                if (m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue() < 0)
                {
                    m_currentMoneyValue = CalculateLosingValue(m_currentMoneyValue, m_selectedMinigames[m_selectedGamesCounter].GetMoneyOutputValue());
                }

                if (m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue() < 0)
                {
                    m_currentSocialValue = CalculateLosingValue(m_currentSocialValue, m_selectedMinigames[m_selectedGamesCounter].GetSocialOutputvalue());
                }
            }

            }
        else
        {
            m_selectedGamesCounter++;
        }
           
            if (m_selectedGamesCounter == 3)
            {
                ClearSelectedMinigames();
                m_weekDaysCounter++;
                if (m_weekDaysCounter >= m_weekDays.Count || m_currentPsychophysicsValue <= 0 || m_currentMoneyValue <= 0 || m_currentSocialValue <= 0)
                {
                    m_sceneLoaderManager.LoadEndGame();
                }
                else
                {
                    m_sceneLoaderManager.LoadNextScene();
                }
            }
            else
            {
                SceneManager.sceneLoaded += SceneLoaded;
                m_sceneLoaderManager.LoadNextScene();
            }


    }

    private float CalculateLosingValue(float initialValue, float malus)
    {
        if (malus < 35)
        {
            malus *= 1;
        }
        else if (malus < 70)
        {
            malus *= 1.25f;
        } 
        else
        {
            malus *= 1.5f;
        }

        return Mathf.Clamp(m_currentPsychophysicsValue + malus, 0.0f, 100.0f);
    }

    private void EndGameCondition()
    {
        if (m_weekDaysCounter >= m_weekDays.Count || m_currentPsychophysicsValue <= 0 || m_currentMoneyValue <= 0 || m_currentSocialValue <= 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    public void ResetGame()
    {
        m_currentPsychophysicsValue = m_defaultPsychophysicsValue;

        m_currentMoneyValue = m_defaultMoneyValue;

        m_currentSocialValue = m_defaultSocialValue;

        m_selectedGamesCounter = 0;

        m_weekDaysCounter = 0;
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
