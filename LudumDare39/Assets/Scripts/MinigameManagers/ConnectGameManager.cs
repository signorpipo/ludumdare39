using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectGameManager : MinigameManager
{
    public RectTransform LeftPanel;
    public RectTransform Rightpanel;

    public List<Sprite> GameItems;

    public int NumGameItems = 0;

    public MonoBehaviour ItemPrefab;

    // Use this for initialization
    public override void Start () {
        float deltaHeight = LeftPanel.GetComponent<RectTransform>().sizeDelta.y;
        for (int i =0; i<NumGameItems; ++i)
        {
            MonoBehaviour itm = Instantiate(ItemPrefab, LeftPanel);
            itm.GetComponent<Image>().sprite = GameItems[(int)(Random.value * GameItems.Count) % GameItems.Count];
            //itm.GetComponent<RectTransform>().SetPositionAndRotation(Vector3.up * deltaHeight, Quaternion.identity);
        }

        base.Start();
	}

    // Update is called once per frame
    public override void Update () {
        base.Update();
	}
}
