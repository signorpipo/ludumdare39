using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LoseEvent();
public class Walls : MonoBehaviour {
    public LoseEvent onLoseEvent;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (onLoseEvent != null)
        {
            onLoseEvent();
        }
    }
}
