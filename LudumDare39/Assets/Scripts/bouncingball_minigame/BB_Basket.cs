using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_Basket : MonoBehaviour {


    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("BB_Ball"))
        {
            Debug.Log("Canestro!!" +
                "");
        }
    }
}
