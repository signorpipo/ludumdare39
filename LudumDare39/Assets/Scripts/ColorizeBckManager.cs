using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorizeBckManager : MonoBehaviour {

    public List<ColorizeBck> ColorizingBck;

	// Use this for initialization
	void Start ()
    {
        foreach (var itm in ColorizingBck)
            itm.gameObject.SetActive(false);
        int bckIdx = (int)(Random.value * ColorizingBck.Count) % ColorizingBck.Count;
        ColorizingBck[bckIdx].gameObject.SetActive(true);
        ColorizingBck[bckIdx].SetColor(1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
