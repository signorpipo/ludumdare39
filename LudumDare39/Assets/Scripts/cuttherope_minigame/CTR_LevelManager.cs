using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTR_LevelManager : MonoBehaviour {
    [SerializeField]
    public RobeDraw m_Rope;

    [SerializeField]
    public GameObject m_Ball;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButton(0))
        {
            m_Ball.GetComponent<FixedJoint2D>().enabled = false;
            m_Rope.RemoveLastJount();

        }
	}
}
