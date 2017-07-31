using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private float m_SpawnRateInSeconds = 1.0f;


    private float m_CameraWidthSize = 0.0f;

    [SerializeField]
    private GameObject m_EnemyItemPrefab;

    private float m_ElapsedTime = 0.0f;

    private float m_EnemyVerticalSpeed = 0.0f;

    public float EnemyVerticalSPeed
    {
        set
        {
            m_EnemyVerticalSpeed = value;
        }
        get
        {
            return m_EnemyVerticalSpeed;
        }
    }


    public float SpawnRate
    {
        get
        {
            return m_SpawnRateInSeconds;
        }
        set
        {
            m_SpawnRateInSeconds = value;
        }
    }

    private void Awake()
    {
        m_CameraWidthSize = Camera.main.orthographicSize * Camera.main.aspect;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_ElapsedTime += Time.deltaTime;

        if(m_ElapsedTime >= m_SpawnRateInSeconds)
        {
            GameObject enemy = Instantiate(m_EnemyItemPrefab, new Vector3(Random.Range(-m_CameraWidthSize, m_CameraWidthSize), Camera.main.orthographicSize, 0), Quaternion.identity);
            enemy.GetComponent<EnemyItem>().VerticalSpeed = m_EnemyVerticalSpeed;

            m_ElapsedTime = 0.0f;
        }

    }
}
