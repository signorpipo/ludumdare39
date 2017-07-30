using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnLetterPass(GameObject letterFall);

public class LetterFall : MonoBehaviour {
    [SerializeField]
    private float velocity = 0.0f;
    public float Velocity
    {
        get { return velocity; }
        set
        {
            velocity = value;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, (velocity * -1.0f), 0.0f);
        }
    }

    public OnLetterPass onLetterPass = null;

    public void FixedUpdate()
    {
        if (gameObject.transform.position.y < (Camera.main.orthographicSize*1.1f)*-1.0f)
        {
            if (onLetterPass != null)
            {
                onLetterPass(gameObject);
            }

        }
    }
}
