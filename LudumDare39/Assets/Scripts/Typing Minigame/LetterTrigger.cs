using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnLetterHit(GameObject letterFall);

public class LetterTrigger : MonoBehaviour
{
    private bool inside= false;
    private GameObject objectTriggered;
    private KeyCode myKey;
    public KeyCode MyKey
    {
        get { return myKey; }
        set { myKey = value; }
    }

    public OnLetterHit onLetterHit = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inside = true;
        objectTriggered = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inside = false;
        objectTriggered = null;
    }

    private void FixedUpdate()
    {
        if(inside && (Input.GetKeyDown(myKey)) && onLetterHit != null)
        {
            onLetterHit(objectTriggered);
        }
    }
}
