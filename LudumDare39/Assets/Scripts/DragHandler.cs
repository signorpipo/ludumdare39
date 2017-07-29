using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DragHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static GameObject itemDragged;

    private Canvas mainCanvas;
    private Vector3 startPosition;
    private Transform startParent;

    public void Start()
    {
        startParent = transform.parent;
        mainCanvas = transform.root.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemDragged = gameObject;
        startPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetParent(mainCanvas.transform);
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(transform.parent == mainCanvas.transform)
        {
            transform.SetParent(startParent);
        }
    }


}
