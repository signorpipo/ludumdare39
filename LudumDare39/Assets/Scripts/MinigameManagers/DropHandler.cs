using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropHandler : MonoBehaviour, IDropHandler
{
    public delegate void OnDroppedItem(bool matched);
    public event OnDroppedItem onDroppedItem = null;

    private Image imageComp = null;
    public void Start()
    {
        imageComp = gameObject.GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragHandler.itemDragged.transform.SetParent(transform);
        DragHandler.itemDragged.transform.position = transform.position;

        gameObject.SetActive(false);

        if (onDroppedItem != null)
        {
            Image imageDragged = DragHandler.itemDragged.GetComponent<Image>();
            onDroppedItem(imageComp.sprite.Equals(imageDragged.sprite) && imageComp.color.Equals(imageDragged.color));
        }
    }
}
