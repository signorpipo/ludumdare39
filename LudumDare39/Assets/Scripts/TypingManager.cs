using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject point;
    [SerializeField]
    private int numLet = 5;

    [SerializeField]
    private Canvas canvas;

    // Use this for initialization
    void Start()
    {
        Camera camera = FindObjectOfType<Camera>();
        Canvas newCanvas = Instantiate(canvas);
        newCanvas.worldCamera = camera;

       

        float left = camera.orthographicSize * camera.aspect * -1;
        float space = (camera.orthographicSize * camera.aspect * 2) / (numLet + 1);
        for (int i = 1; i <= numLet; i++)
        {
            GameObject bru = Instantiate(point, new Vector3(left + (space * i), (camera.orthographicSize * 0.9f) * -1, 0), Quaternion.identity, newCanvas.transform);//0.9f= 90% of camera hight

            bru.name = point.name + i;
            LetterTrigger letterListener = bru.AddComponent<LetterTrigger>();
            letterListener.onLetterHit += OnLetterHit;
        }

    }

    public void OnLetterHit()
    {
        Debug.Log("bruuuuu");
    }
}
