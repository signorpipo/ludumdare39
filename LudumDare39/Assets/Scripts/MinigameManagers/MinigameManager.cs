using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : AbstarcMinigameManager
{
    protected enum GameState
    {
        NO_GAME,
        PLAY_GAME,
        PLAY_GAME_LEFT,
        PLAY_GAME_RIGHT,
        PLAY_GAME_RELEASE,
        END_GAME
    }

    public Text TimeLeft;

    protected GameState mCurrState = GameState.NO_GAME;
    protected float mGameTotalTime = 0;
    protected float mGameTime = 0;
    protected float mScore = 0;

    // 1 facile, 0 difficile
    public override void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    public virtual void Start () {
    }

    // Update is called once per frame
    public virtual void Update () {
        if(mCurrState != GameState.NO_GAME)
        {
            mGameTime -= Time.deltaTime;
            if (mCurrState != GameState.END_GAME && mGameTime <= 0.0f)
            {
                SceneEnded(mScore);
                mCurrState = GameState.END_GAME;
            }
            else
            {
                TimeLeft.text = string.Format("TIME LEFT : {0:n0}", mGameTime);
            }
        }
    }
}
