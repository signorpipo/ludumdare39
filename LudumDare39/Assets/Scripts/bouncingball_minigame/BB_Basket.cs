using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BasketDone();

public class BB_Basket : MonoBehaviour {

    public event BasketDone OnBasketDone;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("BB_Ball"))
        {
            if(OnBasketDone != null)
            {
                OnBasketDone();
            }
        }
    }
}
