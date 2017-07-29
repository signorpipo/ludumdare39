using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnLetterHit();

public class LetterTrigger : MonoBehaviour
{
    public OnLetterHit onLetterHit = null;
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(gameObject.name + " collide with: " + other.name);
        if (onLetterHit != null)
        {
            onLetterHit();
        }
        
    }
}
