using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameInfo : MonoBehaviour {

	[SerializeField]
	private Text m_NameText;
	[SerializeField]
	private Text m_PsyphyText;
    [SerializeField]
    private Text m_MoneyText;
    [SerializeField]
    private Text m_SocialText;

    private string m_Name;

	public void Setup (string i_Name, float i_PsychoPhysicsValue, float i_MoneyValue, float i_SocialValue )
	{
		m_Name = i_Name;
		m_NameText.text = i_Name;
		m_PsyphyText.text = "" + i_PsychoPhysicsValue;
		m_MoneyText.text = "" + i_MoneyValue;
		m_SocialText.text = "" + i_SocialValue;
	}
}
