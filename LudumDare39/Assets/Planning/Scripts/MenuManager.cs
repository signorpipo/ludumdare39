using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[Header("Minigames Settings")]

	[SerializeField]
	private List<MinigameInterface> m_minigames;

	private int m_numberOfSelectedGames = 3;

	[Header("GUI References")]

	[SerializeField]
	private MinigameInfo m_minigameGUITilePrefab = null;

	[SerializeField]
	private Transform m_verticalLayoutMinigames = null;

	[SerializeField]
	private List<GameObject> m_middlePanels = null;

	[SerializeField]
	private StatsCalculator m_statsCalculator = null;

	private List<MinigameInterface> m_seletedMinigames = new List<MinigameInterface>();

	// riferimento a panel pool minigames

	// riferimento a panel selected minigames

	// Use this for initialization
	void Start ()
	{  
		for (int index = 0; index < m_minigames.Count; ++index)
		{
			MinigameInfo tileInfo = Instantiate(m_minigameGUITilePrefab, m_verticalLayoutMinigames);
			tileInfo.transform.localScale = new Vector3(1, 1, 1);
			MinigameInterface temp = m_minigames[index];
			tileInfo.TileSetup(temp.GetName(), temp.GetPsychophysicsOutputValue(), temp.GetMoneyOutputValue(), temp.GetSocialOutputvalue(), index);
		}

		m_statsCalculator = GetComponent<StatsCalculator>();

		for (int i = 0; i < m_middlePanels.Count; ++i)
		{
			m_middlePanels[i].GetComponent<Slot>().onNewSon += UpdateSelectedMinigames;
			m_middlePanels[i].GetComponent<Slot>().onTrashedSon += UpdateSelectedMinigames;
		}
	}

	void OnDisable()
	{
		for (int i = 0; i < m_middlePanels.Count; ++i)
		{
			m_middlePanels[i].GetComponent<Slot>().onNewSon -= UpdateSelectedMinigames;
			m_middlePanels[i].GetComponent<Slot>().onTrashedSon -= UpdateSelectedMinigames;
		}
	}

	public void StartGame()
	{
		// Per ogni oggetto in lista di selezione
			// salva informazioni sul minigame
			// verifica che siano consistenti (grandezza lista == 3)
			// invoca metodo da SceneLoader del tipo LoadScenes(scene1, scene2, scene3);
	}

	public List<MinigameInterface> GetSelectedMinigamesList()
	{
		return m_seletedMinigames;
	}

	private void UpdateSelectedMinigames()
	{
		m_seletedMinigames.Clear();

		m_statsCalculator.ResetSliders();

		for (int index = 0; index < m_middlePanels.Count; ++index)
		{
			GameObject panel = m_middlePanels[index];

			if (panel.transform.childCount > 0)
			{
				MinigameInfo tileInfo = panel.transform.GetChild(0).GetComponent<MinigameInfo>();
				m_seletedMinigames.Add(m_minigames[tileInfo.GetIndex()]);

				m_statsCalculator.UpdateSliders(
					m_seletedMinigames[m_seletedMinigames.Count - 1].GetPsychophysicsOutputValue(),
					m_seletedMinigames[m_seletedMinigames.Count - 1].GetMoneyOutputValue(),
					m_seletedMinigames[m_seletedMinigames.Count - 1].GetSocialOutputvalue()
					);
			}
		}
	}
}
