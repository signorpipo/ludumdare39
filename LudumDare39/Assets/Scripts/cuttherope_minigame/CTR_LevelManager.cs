using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CTR_LevelManager : AbstarcMinigameManager
{

    [SerializeField]
    private GameObject m_Basket;


    private bool m_MouseAlreadyPressed = true;

    [SerializeField]
    private bool m_ForceStart = false;

    [SerializeField]
    private GameObject m_RopeAndBalPrefab = null;


    private GameObject m_CurrentRopeAndBallInstance = null;

    [SerializeField]
    private Text m_Life;


    private int m_GravityScale = 1;

    
    public int m_NumAttemptes = 2;


    public int Time = 10;

  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !m_MouseAlreadyPressed )
        {

            GameObject ball = GameObject.Find("BB_Ball");
           GameObject basket =  GameObject.Find("HingePoint");
            ball.GetComponent<FixedJoint2D>().enabled = false;

            m_MouseAlreadyPressed = true;

            basket.GetComponent<RobeDraw>().RemoveLastJount();

        }
    }


    public override void StartMinigame
    (
        int i_Type,
        float i_TimeValue,
        float i_NumItemValue,
        float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        m_NumAttemptes = (int)((3 - 1) * i_NumItemValue + 1);
        m_GravityScale = (int)((5 - 1) * (1- i_ItemVelocityValue) + 1);
        DoStart();
    }

    private void DoStart()
    {
        m_MouseAlreadyPressed = true;
        StartTimer(
            transform,
            TimerStart,
            TimerEnd,
            "Clean Your Office",
            "Left click of the mouse to cut the rope. Try to put the paper in the trash bin.",
            3,
            "Times Up!",
            Time
        );
    }

    private void Awake()
    {
        m_Life.text = "Life: " + m_NumAttemptes;    
    }

    private void Start()
    {
        
        m_Basket.GetComponent<CTR_Trash>().OnBasketDone += Success;

        if (m_ForceStart)
        {
            DoStart();
        }
    }

    public void TimerStart()
    {
        m_MouseAlreadyPressed = false;
        m_CurrentRopeAndBallInstance = Instantiate(m_RopeAndBalPrefab, transform.parent);
        GameObject ball = GameObject.Find("BB_Ball");
        ball.GetComponent<Rigidbody2D>().gravityScale = m_GravityScale;
    }

    public void TimerEnd()
    {
        m_MouseAlreadyPressed = true;
        Destroy(m_CurrentRopeAndBallInstance);
        StopTimer();
        SceneEnded(0.0f);

    }

    private void Success()
    {
        StopTimer();
        SceneEnded(1.0f);
    }

    private void Failure()
    {
        if (--m_NumAttemptes > 0)
        {
           
            Destroy(m_CurrentRopeAndBallInstance);
            m_MouseAlreadyPressed = false;
            m_CurrentRopeAndBallInstance = Instantiate(m_RopeAndBalPrefab, transform.parent);
            GameObject ball = GameObject.Find("BB_Ball");
            ball.GetComponent<Rigidbody2D>().gravityScale = m_GravityScale;

        }
        else
        {
            StopTimer();
            SceneEnded(0.0f);
        }

        m_Life.text = "Life: " + m_NumAttemptes;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Failure();
    }
}
