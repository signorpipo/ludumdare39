using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    public Slider successBarSlider;
    public Slider gameTimeBarSlider;

    public MessageManager MessageMgr;

    public KeyCode LeftKey = KeyCode.LeftArrow;
    public KeyCode RightKey = KeyCode.RightArrow;

    public float GameTotalTime = 5.0f;
    public float PositiveMult = 0.3f;
    public float NegativeMult = 0.1f;
    public float KeyTimer = .08f;

    private float mKeyTimer;
    private float mGameTime = 0;
    private int mCurrState = 0;


	// Use this for initialization
	void Start () {
        successBarSlider.value = 0;
        mGameTime = GameTotalTime;
        mKeyTimer = 0;

       
    }
	
	// Update is called once per frame
	void Update ()
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
