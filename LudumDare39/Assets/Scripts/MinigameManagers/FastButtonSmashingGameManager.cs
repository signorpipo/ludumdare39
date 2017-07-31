using UnityEngine;
using UnityEngine.UI;

public class FastButtonSmashingGameManager : MinigameManager
{
    public Slider successBarSlider;

    public KeyCode LeftKey = KeyCode.LeftArrow;
    public KeyCode RightKey = KeyCode.RightArrow;

    public float PositiveMult = 0.3f;
    public float NegativeMult = 0.1f;
    public float KeyTimer = .08f;

    private float mKeyTimer;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
    }

    // 1 facile, 0 difficile
    public override void StartMinigame
        (
        int i_Type,
        float i_TimeValue,
        float i_NumItemValue,
        float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        successBarSlider.value = 0;
        mKeyTimer = 0;
        mGameTotalTime = mGameTime = 6f + 10f * i_TimeValue;
        mCurrState = GameState.PLAY_GAME;
    }

    // Update is called once per frame
    public override void Update ()
    {
        if(mCurrState != GameState.NO_GAME)
        {
            bool keyLeft = Input.GetKey(LeftKey);
            bool keyRight = Input.GetKey(RightKey);

            if (mCurrState == GameState.PLAY_GAME_RELEASE)
            {
                if (!keyLeft && !keyRight)
                    mCurrState = 0;
                successBarSlider.value -= NegativeMult * Time.deltaTime;
            }
            else if (mCurrState == GameState.PLAY_GAME)
            {
                mKeyTimer = 0;
                if (keyLeft && !keyRight)
                    mCurrState = GameState.PLAY_GAME_LEFT;
                else if (keyRight && !keyLeft)
                    mCurrState = GameState.PLAY_GAME_RIGHT;
                successBarSlider.value -= NegativeMult * Time.deltaTime;
            }
            else if (mCurrState == GameState.PLAY_GAME_LEFT)
            {
                if (keyLeft && !keyRight && mKeyTimer < KeyTimer)
                {
                    mKeyTimer += Time.deltaTime;
                    successBarSlider.value += PositiveMult * Time.deltaTime;
                }
                else
                {
                    mCurrState = GameState.PLAY_GAME_RELEASE;
                }
            }
            else if (mCurrState == GameState.PLAY_GAME_RIGHT)
            {
                if (!keyLeft && keyRight && mKeyTimer < KeyTimer)
                {
                    mKeyTimer += Time.deltaTime;
                    successBarSlider.value += PositiveMult * Time.deltaTime;
                }
                else
                {
                    mCurrState = GameState.PLAY_GAME_RELEASE;
                }
            }
        }
        base.Update();
    }
}
