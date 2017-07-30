using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Carica le scene una alla volta. Compreso la main
/// </summary>
public class SceneLoaderSingleManager : MonoBehaviour {

    //Scena principale
    public string main_scene;
    private static SceneLoaderSingleManager instance = null;
    private List<string> list_select_minigame_scene;
    
    private string run_scene;
    private int index = 0;

    /// <summary>
    /// Imposta la scena come in esecuzione.
    /// </summary>
    void Start() {
        run_scene = main_scene;
    }

    /// <summary>
    /// Cambia la scena con la prossima della lista, finita la lista torna alla principale
    /// </summary>
    public void LoadNextScene()
    {
        if (index < list_select_minigame_scene.Count)
        {
            SceneManager.LoadScene(list_select_minigame_scene[index], LoadSceneMode.Additive);

            if (run_scene != main_scene) {
                SceneManager.UnloadScene(SceneManager.GetSceneByName(run_scene));
            }

            run_scene = list_select_minigame_scene[index];
            Debug.Log("Caricate le scene in unity");
            index++;
            Debug.Log("Attivata scena numero " + index + " nome: " +  run_scene);
        }
        else
        {
            index = 0;
            run_scene = main_scene;
            Debug.Log("Finite scene minigame, riattivata scena principale " + main_scene);
            SceneManager.LoadScene(main_scene);
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
    public string ActualScene(){
        return run_scene;
    }
}
