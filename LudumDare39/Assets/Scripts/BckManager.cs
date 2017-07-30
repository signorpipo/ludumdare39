using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BckManager : MonoBehaviour {

    public List<Sprite> SpriteList;

	// Use this for initialization
	void Start () {
        GetComponentInParent<SpriteRenderer>().sprite = SpriteList[(int)(Random.value * SpriteList.Count)% SpriteList.Count];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
