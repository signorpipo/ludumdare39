using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    public Slider successBarSlider;

    public float PositiveMult = 0.3f;
    public float NegativeMult = 0.1f;
    public float KeyTimer = .15f;

    private float mKeyTimer;
    private int mCurrState = 0;

	// Use this for initialization
	void Start () {
        successBarSlider.value = 0;
        mKeyTimer = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool keyLeft = Input.GetKey(KeyCode.RightArrow);
        bool keyRight = Input.GetKey(KeyCode.LeftArrow);

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
    }
}
