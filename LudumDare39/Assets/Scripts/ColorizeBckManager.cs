using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorizeBckManager : MonoBehaviour
{
    public enum BckTypes
    {
        BCK_SOCIAL = 0,
        BCK_PLANNING,
        BCK_PHYSICS,
        BCK_MONEY
    }

    public List<ColorizeBck> ColorizingBck;

    public Transform MainCharacter;

	// Use this for initialization
	void Start ()
    {
        SetUncoloredBckType(BckTypes.BCK_PHYSICS);
    }

    public void SetUncoloredBckType(BckTypes type)
    {
        foreach (var itm in ColorizingBck)
            itm.gameObject.SetActive(false);
        ColorizingBck[(int)type].gameObject.SetActive(true);
        ColorizingBck[(int)type].SetUncolor();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
