using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Minigames Settings")]

	[SerializeField]
	private List<MinigameInterface> m_minigames;

    [SerializeField]
    private List<DailyProperties> m_weekDaysList;

	private int m_numberOfSelectedGames = 3;

	[Header("GUI References")]

	[SerializeField]
	private MinigameInfo m_minigameGUITilePrefab = null;

	[SerializeField]
	private Transform m_verticalLayoutMinigames = null;

	[SerializeField]
	private List<GameObject> m_middlePanels = null;

    [SerializeField]
    private Transform m_dailyBonusPanel = null;

    [SerializeField]
	private StatsCalculator m_statsCalculator = null;

	private List<MinigameInterface> m_seletedMinigames = new List<MinigameInterface>();

	// riferimento a panel pool minigames

	// riferimento a panel selected minigames

	// Use this for initialization
	void Start ()
	{
        GameManager gameManager = GameManager.Instance;
        gameManager.SetWeekDays(m_weekDaysList);

        for (int index = 0; index < m_minigames.Count; ++index)
		{
			MinigameInfo tileInfo = Instantiate(m_minigameGUITilePrefab, m_verticalLayoutMinigames);
			tileInfo.transform.localScale = new Vector3(1, 1, 1);
			MinigameInterface temp = m_minigames[index];
			tileInfo.TileSetup(temp.GetName(), temp.GetPsychophysicsOutputValue(), temp.GetMoneyOutputValue(), temp.GetSocialOutputvalue(), index);
		}

		m_statsCalculator = GetComponent<StatsCalculator>();

		for (int index = 0; index < m_middlePanels.Count; ++index)
		{
			m_middlePanels[index].GetComponent<Slot>().onNewSon += UpdateSelectedMinigames;
			m_middlePanels[index].GetComponent<Slot>().onTrashedSon += UpdateSelectedMinigames;
		}

         m_dailyBonusPanel.GetChild(0).GetComponent<Text>().text += m_weekDaysList[gameManager.m_weekDaysCounter].m_psychophysicsBonus + "%";
         m_dailyBonusPanel.GetChild(1).GetComponent<Text>().text += m_weekDaysList[gameManager.m_weekDaysCounter].m_moneyBonus + "%";
         m_dailyBonusPanel.GetChild(2).GetComponent<Text>().text += m_weekDaysList[gameManager.m_weekDaysCounter].m_socialBonus + "%";
    }

    //void OnDisable()
    //{
    //	for (int i = 0; i < m_middlePanels.Count; ++i)
    //	{
    //		m_middlePanels[i].GetComponent<Slot>().onNewSon -= UpdateSelectedMinigames;
    //		m_middlePanels[i].GetComponent<Slot>().onTrashedSon -= UpdateSelectedMinigames;
    //	}
    //}


    public void StartGame()
    {
        if (3 == m_seletedMinigames.Count)
        {
            GameManager gameManager = GameManager.Instance;
            gameManager.SetSelectedMiniGames(m_seletedMinigames);
            gameManager.StartGame();
        }
        else
        {
            Debug.Log("Gesù è l'unico e vero Signore");
        }
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
