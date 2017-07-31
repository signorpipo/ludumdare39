using UnityEngine;
using UnityEngine.UI;

public class FastButtonSmashingGameManager : MinigameManager
{
    public Slider successBarSlider;

    public Image LeftImage;
    public Image RightImage;

    private const float MIN_GAME_TIME = 6f;
    private const float MAX_GAME_TIME = 16f;

    public float PositiveMult = 0.3f;
    public float NegativeMult = 0.1f;
    public float KeyTimer = .08f;

    private float mKeyTimer;

    private string mLeftKey = "a";
    private string mRightKey = "l";

    private bool lastIsLeft = false;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
    }

    // 1 facile, 0 difficile
    public override void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        ColorizeBckManager.BckTypes type = ColorizeBckManager.BckTypes.BCK_PLANNING;
        switch (i_Type)
        {
            case 0: type = ColorizeBckManager.BckTypes.BCK_PHYSICS; break;
            case 1: type = ColorizeBckManager.BckTypes.BCK_MONEY; break;
            case 2: type = ColorizeBckManager.BckTypes.BCK_SOCIAL; break;
        }
        FindObjectOfType<ColorizeBckManager>().SetUncoloredBckType(type);

        successBarSlider.value = 0;
        mKeyTimer = 0;
        mScore = 0;
        mGameTotalTime = mGameTime = MIN_GAME_TIME + MAX_GAME_TIME * i_TimeValue;
        PositiveMult = 0.2f + 0.2f * i_NumItemValue;
        mCurrState = GameState.PLAY_GAME;

        string refKeys = "qazwsxedcrfvtgbyhnujmiklop";
        if (i_ItemVelocityValue > 0.5f)
            refKeys = "qazxswedcvfr";
        int leftIdx = ((int)(Random.value * refKeys.Length)) % refKeys.Length;
        mLeftKey = refKeys[leftIdx].ToString();

        if (i_ItemVelocityValue > 0.5f)
            refKeys = "tgbnhyujmkilop";
        int rightIdx = ((int)(Random.value * refKeys.Length)) % refKeys.Length;
        if (rightIdx == leftIdx)
            rightIdx = (rightIdx + 1) % refKeys.Length;
        mRightKey = refKeys[rightIdx].ToString();
        LeftImage.GetComponentInChildren<Text>().text = mLeftKey.ToUpper();
        RightImage.GetComponentInChildren<Text>().text = mRightKey.ToUpper();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (mCurrState != GameState.NO_GAME)
        {
            bool keyLeft = Input.GetKey(mLeftKey);
            bool keyRight = Input.GetKey(mRightKey);

            if (mCurrState == GameState.PLAY_GAME_RELEASE_LEFT && !keyLeft)
            {
                mCurrState = GameState.PLAY_GAME;
                successBarSlider.value -= NegativeMult * Time.deltaTime;
            }
            else if (mCurrState == GameState.PLAY_GAME_RELEASE_RIGHT && !keyRight)
            {
                mCurrState = GameState.PLAY_GAME;
                successBarSlider.value -= NegativeMult * Time.deltaTime;
            }
            else if(mCurrState == GameState.PLAY_GAME)
            {
                mKeyTimer = 0;
                if (keyLeft && !keyRight && !lastIsLeft)
                {
                    mCurrState = GameState.PLAY_GAME_LEFT;
                    mAudioSource.PlayOneShot(mTimesUpSound, 0.5F);
                    lastIsLeft = true;
                }
                else if (keyRight && !keyLeft && lastIsLeft)
                {
                    mCurrState = GameState.PLAY_GAME_RIGHT;
                    mAudioSource.PlayOneShot(mTimesUpSound, 0.5F);
                    lastIsLeft = !lastIsLeft;
                }
                else if(keyRight || keyLeft)
                {
                    mAudioSource.PlayOneShot(mDockSound, 0.5F);
                }
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
                    mCurrState = GameState.PLAY_GAME_RELEASE_LEFT;
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
                    mCurrState = GameState.PLAY_GAME_RELEASE_RIGHT;
                }
            }

            LeftImage.color = (mCurrState == GameState.PLAY_GAME_LEFT ? Color.red : Color.white);
            RightImage.color = (mCurrState == GameState.PLAY_GAME_RIGHT ? Color.red : Color.white);

            mScore = successBarSlider.value;
        }
        base.Update();
    }
}
