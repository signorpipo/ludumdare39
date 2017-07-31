using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BasketDoneEvent();

public class CTR_Trash : MonoBehaviour {

    public event BasketDoneEvent OnBasketDone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(OnBasketDone != null)
        {
            OnBasketDone();
        }
    }
}
