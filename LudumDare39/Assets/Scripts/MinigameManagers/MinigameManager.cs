using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour {

    public Slider gameTimeBarSlider;

    public MessageManager MessageMgr;

    public float GameTotalTime = 5.0f;

    protected int mCurrState = 0;
    private float mGameTime = 0;

    // Use this for initialization
    public virtual void Start () {
        mGameTime = GameTotalTime;
    }

    // Update is called once per frame
    public virtual void Update () {
        mGameTime -= Time.deltaTime;
        if (mCurrState != -5 && mGameTime <= 0.0f)
        {
            MessageMgr.GameOver();
            mCurrState = -5;
        }
        else
        {
            gameTimeBarSlider.value = mGameTime / GameTotalTime;
        }
    }
}
