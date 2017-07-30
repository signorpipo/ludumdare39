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
        successBarSlider.value = 0;
        mKeyTimer = 0;
        base.Start();
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        bool keyLeft = Input.GetKey(LeftKey);
        bool keyRight = Input.GetKey(RightKey);

        if (mCurrState == -3)
        {
            if (!keyLeft && !keyRight)
                mCurrState = 0;
            successBarSlider.value -= NegativeMult * Time.deltaTime;
        }
        else if (mCurrState == 0)
        {
            mKeyTimer = 0;
            if (keyLeft && ! keyRight)
                mCurrState = -1;
            else if (keyRight && !keyLeft)
                mCurrState = 1;
            successBarSlider.value -= NegativeMult * Time.deltaTime;
        }
        else if (mCurrState == -1)
        {
            if (keyLeft && !keyRight && mKeyTimer < KeyTimer)
            {
                mKeyTimer += Time.deltaTime;
                successBarSlider.value += PositiveMult * Time.deltaTime;
            }
            else
            {
                mCurrState = -3;
            }
        }
        else if (mCurrState == 1)
        {
            if (!keyLeft && keyRight && mKeyTimer < KeyTimer)
            {
                mKeyTimer += Time.deltaTime;
                successBarSlider.value += PositiveMult * Time.deltaTime;
            }
            else
            {
                mCurrState = -3;
            }
        }

        base.Update();
    }
}
