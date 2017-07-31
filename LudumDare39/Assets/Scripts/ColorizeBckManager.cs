using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorizeBckManager : MonoBehaviour
{
    public enum BckTypes
    {
        BCK_NONE,
        BCK_SOCIAL = 0,
        BCK_PLANNING,
        BCK_PHYSICS,
        BCK_MONEY
    }

    public BckTypes ForceBackground = BckTypes.BCK_NONE;

    public List<ColorizeBck> ColorizingBck;

    public Transform MainCharacter;

	// Use this for initialization
	void Start ()
    {
        if (ForceBackground != BckTypes.BCK_NONE)
            SetUncoloredBckType(ForceBackground);
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
