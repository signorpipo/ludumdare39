using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterFall : MonoBehaviour {
    [SerializeField]
    private float velocity = 1.0f;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, (velocity * -1.0f), 0);
	}

}
