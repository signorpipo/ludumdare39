using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{

	// Use this for initialization
	void Start () {
		
	}

    public void OnDrop(PointerEventData eventData)
    {
        DragHandler.itemDragged.transform.SetParent(transform);
        DragHandler.itemDragged.transform.position = transform.position;

        gameObject.SetActive(false);
    }
}
