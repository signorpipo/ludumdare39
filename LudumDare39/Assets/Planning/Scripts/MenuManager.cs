using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private List<MinigameInterface> m_minigames;

    [SerializeField]
    private int m_numberOfSelectedGames = 3;

    private List<MinigameInterface> m_seletedMinigames;

    // riferimento a panel pool minigames

    // riferimento a panel selected minigames

	// Use this for initialization
	void Start ()
    {
        // per ogni oggetto in lista dei minagames
            // crea un "bottone" nel panel pool
            // imposta informazioni su panel pool minigames
	}

    public void StartGame()
    {
        // Per ogni oggetto in lista di selezione
            // salva informazioni sul minigame
            // verifica che siano consistenti (grandezza lista == 3)
            // invoca metodo da SceneLoader del tipo LoadScenes(scene1, scene2, scene3);
    }

    void Update()
    {
        // se la lista della UI è completa
        for (int index = 0; index < m_numberOfSelectedGames; ++index)
        {
            // se esiste qualcosa a quella posizione sulla lista dell'interfaccia
            m_seletedMinigames.Add(new MinigameInterface()); // add minigame interface at position index
        }
    }

    public List<MinigameInterface> GetSelectedMinigamesList()
    {
        return m_seletedMinigames;
    }
}
