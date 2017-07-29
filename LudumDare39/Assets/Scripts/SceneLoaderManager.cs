using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour {

    //Scena principale
    public string main_scene;

    private List<string> list_minigame_scene;
    private List<string> list_select_minigame_scene;
    private static SceneLoaderManager instance = null;
    private string run_scene;
    private int index = 0;

    /// <summary>
    /// Imposta la scena come in esecuzione.
    /// </summary>
    void Start(){
        run_scene = main_scene;
    }

    /// <summary>
    /// Cambia la scena in quella successiva. Se le scene sono finite, torna alla principale. l'indice della scena index viene resettato
    /// </summary>
    void SingleScene() {
        if(index < list_minigame_scene.Count) {
            run_scene = list_minigame_scene[index];
            SceneManager.LoadScene(run_scene, LoadSceneMode.Single);
            index++;
        }
        else {
            index = 0;
            run_scene = main_scene;
            SceneManager.LoadScene(main_scene, LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// Cambia la scena con la prossima della lista, finita la lista torna alla principale
    /// </summary>
    void nextPlayerScene()
    {
        if (index < list_minigame_scene.Count)
        {
            run_scene = list_select_minigame_scene[index];
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(list_minigame_scene[index]));
            index++;
            Debug.Log("Attivata scena numero " + index + " nome: " +  run_scene);
        }
        else
        {
            index = 0;
            run_scene = main_scene;
            Debug.Log("Finite scene minigame, riattivata scena principale" + main_scene);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(main_scene));
        }
    }

    /// <summary>
    /// Carica tutte le scene presenti nella lista. 
    /// </summary>
    void MultiScene() {
        for(int i=0;i<list_minigame_scene.Count;++i) {
            SceneManager.LoadScene(list_minigame_scene[i], LoadSceneMode.Additive);
            Debug.Log("Caricate le scene in unity");
        }
    }

    /// <summary>
    /// Carica una lista di nomi di scene.
    /// </summary>
    /// <param name="scenes"> contiene una lista di nomi in riferimento alle scene</param>
    public void LoadScenes(List<string> scenes) {
        list_minigame_scene = scenes;
    }

    /// <summary>
    /// Carica una lista di nomi di scene.
    /// </summary>
    /// <param name="scenes"> contiene una lista di nomi in riferimento alle scene</param>
    public void LoadScenesPlayer(List<string> scenes)
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
