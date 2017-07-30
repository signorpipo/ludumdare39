using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MissEvent();

public class Walls : MonoBehaviour {
    public MissEvent onMissEvent;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (onMissEvent != null)
        {
            onMissEvent();
        }
    }
}
