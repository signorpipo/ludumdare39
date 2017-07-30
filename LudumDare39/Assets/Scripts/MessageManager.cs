using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour {
    private Animator mAnimator = null;

    // Use this for initialization
    void Start ()
    {
        ((RectTransform)transform).localScale = Vector3.zero;
        mAnimator = GetComponentInParent<Animator>();
    }
	
    public void GameOver()
    {
        mAnimator.SetTrigger("GameOver");
    }

	// Update is called once per frame
	void Update () {
        ((RectTransform)transform).localScale = Vector3.zero;

    }
}
