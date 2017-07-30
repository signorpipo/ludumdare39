using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject letterFallPref;

    [SerializeField]
    private float fallingVelocity=5.0f;
    [SerializeField]
    private float timeSpawnLetter =1.0f;
    [SerializeField]
    private float timeGame;
    private float timer=0.0f;

    [SerializeField]
    private GameObject letterTriggerPref;
    [SerializeField]
    private int numLet = 5;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private List<KeyCode> typeCode;
    private Canvas newCanvas;
    [SerializeField]
    private Queue<GameObject> letterFallPool;

    [SerializeField]
    private List<GameObject> letterTriggerList;

    private int points = 0;
    void Start()
    {
        Camera camera = FindObjectOfType<Camera>();
        newCanvas = Instantiate(canvas);
        newCanvas.worldCamera = camera;

        letterTriggerList = new List<GameObject>();

        float left = camera.orthographicSize * camera.aspect * -1;
        float space = (camera.orthographicSize * camera.aspect * 2) / (numLet + 1);
        for (int index = 1; index <= numLet; index++)
        {
            GameObject letterTrigger = Instantiate(letterTriggerPref, new Vector3(left + (space * index), (camera.orthographicSize * 0.85f) * -1, -1), Quaternion.identity, newCanvas.transform);//0.9f= 90% of camera hight
            letterTriggerList.Add(letterTrigger);
            letterTrigger.name = typeCode[index - 1].ToString();
            Transform text=letterTrigger.transform.Find("Text");
           
            text.GetComponent<Text>().text= typeCode[index - 1].ToString();
            LetterTrigger letterListener = letterTrigger.GetComponent<LetterTrigger>();
            letterListener.MyKey = typeCode[index - 1];
            letterListener.onLetterHit += OnLetterHit;
            letterListener.onLetterMiss += OnLetterMiss;
        }
        letterFallPool = new Queue<GameObject>();
        for(int i =0; i < 10; i++)
        {
            GameObject letterFalling = Instantiate(letterFallPref);
            
            LetterFall letterFallListener = letterFalling.GetComponent<LetterFall>();
            letterFallListener.onLetterPass += OnLetterPass;
            letterFalling.SetActive(false);
            letterFallPool.Enqueue(letterFalling);
        }
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        timeGame -= Time.deltaTime;
        if (timer < 0.0f && timeGame>0.0f)
        {
            timer = timeSpawnLetter;
            GameObject letterFallSpawn= letterFallPool.Dequeue();
            int random = Random.Range(0, letterTriggerList.Count);
            float positionX=letterTriggerList[random].transform.position.x;
            
            letterFallSpawn.transform.position = new Vector3(positionX, (Camera.main.orthographicSize * 1.2f), 0.0f);
            letterFallSpawn.SetActive(true);
            letterFallSpawn.GetComponent<LetterFall>().Velocity =fallingVelocity;
        }
    }

    public void OnLetterHit(GameObject letterFall)
    {
        letterFall.SetActive(false);
        letterFallPool.Enqueue(letterFall);
        points++;
        newCanvas.GetComponent<Animator>().SetTrigger("onResizeHit");
    }

    public void OnLetterPass(GameObject letterFall)
    {
        letterFall.SetActive(false);
        letterFallPool.Enqueue(letterFall);
        newCanvas.GetComponent<Animator>().SetTrigger("onResizeMiss");
        points--;
    }

    public void OnLetterMiss()
    {
        newCanvas.GetComponent<Animator>().SetTrigger("onResizeMiss");
    }
}
