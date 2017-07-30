using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public delegate void OnNewSon();
public delegate void OnTrashedSon();

public class Slot : MonoBehaviour, IDropHandler
{
    public event OnNewSon onNewSon = null;
    public event OnTrashedSon onTrashedSon = null;
    private bool hasChild = false;

    void Update()
    {
        if (hasChild && transform.childCount <= 0)
        {
            onTrashedSon.Invoke();
        }
    }

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragHandler.itemDragged.transform.SetParent(transform);
            DragHandler.itemDragged.transform.position = transform.position;
            hasChild = true;
            onNewSon.Invoke();
        }
    }
}