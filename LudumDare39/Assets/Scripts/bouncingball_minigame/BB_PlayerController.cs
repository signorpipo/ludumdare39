using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_PlayerController : MonoBehaviour {

    [SerializeField]
    private float m_HorizontalSpeed = 10.0f;

    [SerializeField]
    private float m_VerticalSpeed = 10.0f;


    private float m_CameraWidthSize = 0.0f;


    private float m_MyHalfWidthSize = 0.0f;

    private void Awake()
    {
        m_CameraWidthSize = Camera.main.orthographicSize * Camera.main.aspect;
        m_MyHalfWidthSize = (GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2) * transform.localScale.x;
        
    }

    // Update is called once per frame
    void Update () {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3(currentPosition.x + horizontalInput * m_HorizontalSpeed * Time.deltaTime, currentPosition.y, currentPosition.z);

        if ((Mathf.Abs(newPosition.x) + m_MyHalfWidthSize) <= m_CameraWidthSize)
        {
            transform.position = newPosition;
        }

        transform.Rotate(new Vector3(0, 0, verticalInput * m_VerticalSpeed * Time.deltaTime));

    }
}
