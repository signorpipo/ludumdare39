using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WeekendResume : MonoBehaviour {

	[SerializeField]
	private string m_loseText;
	[SerializeField]
	private string m_highPsychophysicsText;
	[SerializeField]
	private string m_highMoneyText;
	[SerializeField]
	private string m_highSocialText;


	void Start ()
	{
		Text result = GetComponent<Text>();
		GameManager gm = FindObjectOfType<GameManager>();

		float psy = gm.CurrentPsychophysicsValue;
		float mon = gm.CurrentMoneyValue;
		float soc = gm.CurrentSocialValue;

		if(psy<=0||mon<=0||soc<=0)
		{
			result.text = m_loseText;
		}

		else

		{
			if(psy > mon)
			{
				if(psy > soc)
				{
					result.text = m_highPsychophysicsText;
				}
				else
				{
					result.text = m_highMoneyText;
				}
			}

			else

			{
				if(mon>soc)
				{
					result.text = m_highMoneyText;
				}
				else
				{
					result.text = m_highSocialText;
				}
			}
		}
	}

}
