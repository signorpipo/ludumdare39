using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Carica le scene una alla volta. Compreso la main
/// </summary>
public class SceneLoaderSingleManager : Singleton<SceneLoaderSingleManager>
{
    //Singleton<SceneLoaderSingleManager> m_sceneManager;
    protected SceneLoaderSingleManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    //Scena principale
    private string main_scene = "PlanningMenu";
    private string end_minigame_scene = "EndMinigame";
    private string end_game_scene = "EndScene";
    private bool m_isMinigame = true;
    private static SceneLoaderSingleManager instance = null;
    private List<string> list_select_minigame_scene;

    private string run_scene;
    private int index = 0;

    public bool IsMinigame
    {
        get{ return m_isMinigame;}
    }

    /// <summary>
    /// Imposta la scena come in esecuzione. 
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        run_scene = main_scene;
    }


    /// <summary>
    /// Cambia la scena con la prossima della lista, finita la lista torna alla principale
    /// </summary>
    public void LoadNextScene()
    {
        if (m_isMinigame)
        {
            if (index < list_select_minigame_scene.Count)
            {
                m_isMinigame = false;
                SceneManager.LoadScene(list_select_minigame_scene[index], LoadSceneMode.Single);

                run_scene = list_select_minigame_scene[index];
                Debug.Log("Caricate le scene in unity");
                index++;
                Debug.Log("Attivata scena numero " + index + " nome: " + run_scene);
            }
            else
            {
                index = 0;
                run_scene = main_scene;
                Debug.Log("Finite scene minigame, riattivata scena principale " + main_scene);
                SceneManager.LoadScene(main_scene);
            }
        }
        else
        {
            m_isMinigame = true;
            SceneManager.LoadScene(end_minigame_scene);
            run_scene = end_minigame_scene;
            Debug.Log("Caricato livello di recap gioco");

            
        }
    }

    /// <summary>
    /// Carica una lista di nomi di scene.
    /// </summary>
    /// <param name="scenes"> contiene una lista di nomi in riferimento alle scene</param>
    public void SetSelectedScenesNames(List<string> scenes)
    {
        list_select_minigame_scene = scenes;
    }

    /// <summary>
    /// Ritorna il nome della scena attualmente in corso.
    /// </summary>
    /// <returns>nome della scena attualmente in corso</returns>
    public string ActualScene()
    {
        return run_scene;
    }

    public void Reset()
    {
        index = 0;
        m_isMinigame = true;
    }

    internal void LoadEndGame()
    {
        SceneManager.LoadScene(end_game_scene);
        run_scene = end_game_scene;
        Debug.Log("Caricato endlevel");
    }
}
