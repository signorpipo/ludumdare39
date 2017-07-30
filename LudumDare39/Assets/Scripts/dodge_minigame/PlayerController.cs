using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void PlayerFailure();

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float m_HorizontalSpeed = 10.0f;

    [SerializeField]
    private GameObject m_ShitOnIce;

    private float m_CameraWidthSize = 0.0f;

 
    private float m_MyHalfWidthSize = 0.0f;

    public event PlayerFailure OnPlayerFailure;


    private void Awake()
    {
        m_CameraWidthSize = Camera.main.orthographicSize * Camera.main.aspect;
        m_MyHalfWidthSize = (GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2)*transform.localScale.x;
        m_ShitOnIce.SetActive(false);
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 currentPosition = transform.position;

        Vector3 newPosition = new Vector3( currentPosition.x + horizontalInput*m_HorizontalSpeed*Time.deltaTime , currentPosition.y, currentPosition.z);

        if( (Mathf.Abs(newPosition.x)+m_MyHalfWidthSize) <= m_CameraWidthSize )
        {
            transform.position = newPosition;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
        Destroy(collision.gameObject);
        m_ShitOnIce.SetActive(true); // and game failure emitter

        if(OnPlayerFailure != null)
            OnPlayerFailure();
    }
}
