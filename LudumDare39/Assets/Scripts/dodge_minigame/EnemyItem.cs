using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItem : MonoBehaviour {

    [SerializeField]
    private float m_VerticalSpeed = -5.0f;

    public float VerticalSpeed
    {
        set
        {
            m_VerticalSpeed = value;
        }
        get
        {
            return m_VerticalSpeed;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 currentPosition = transform.position;

        transform.position = new Vector3(currentPosition.x, currentPosition.y+m_VerticalSpeed*Time.deltaTime, currentPosition.z);
		
	}
}
