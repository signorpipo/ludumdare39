using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnLetterHit(GameObject letterFall);
public delegate void OnLetterMiss();

public class LetterTrigger : MonoBehaviour
{
    private bool inside= false;
    private KeyCode myKey;
    public KeyCode MyKey
    {
        get { return myKey; }
        set { myKey = value; }
    }

    public OnLetterHit onLetterHit = null;
    public OnLetterMiss onLetterMiss = null;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(myKey)&&onLetterHit != null)
        {
            onLetterHit(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inside = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myKey))
        {
            GetComponent<Animator>().SetTrigger("goToResize");
            if (!inside && onLetterMiss != null)
            {
                onLetterMiss();              
            }
        }
    }
}
