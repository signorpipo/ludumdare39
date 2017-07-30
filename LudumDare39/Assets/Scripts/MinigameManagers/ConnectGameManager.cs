using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectGameManager : MinigameManager
{
    public RectTransform LeftPanel;
    public RectTransform RightPanel;

    public List<Sprite> GameItems;

    public int NumGameItems = 0;

    public MonoBehaviour ItemPrefab;

    // Use this for initialization
    public override void Start () {
        float deltaHeight = LeftPanel.rect.height / NumGameItems;
        for (int i =0; i<NumGameItems; ++i)
        {
            int spriteIdx = (int)(Random.value * GameItems.Count) % GameItems.Count;
            float spriteScale = LeftPanel.rect.width / GameItems[0].rect.width;
            MonoBehaviour lItm = Instantiate(ItemPrefab, LeftPanel);
            lItm.GetComponent<Image>().sprite = GameItems[spriteIdx];
            lItm.transform.localPosition = Vector3.up * (deltaHeight * i - LeftPanel.rect.height * 0.5f);
            lItm.transform.localScale = Vector3.one * spriteScale;

            MonoBehaviour rItm = Instantiate(ItemPrefab, RightPanel);
            rItm.GetComponent<Image>().sprite = GameItems[spriteIdx];
            rItm.transform.localPosition = Vector3.up * (deltaHeight * i - LeftPanel.rect.height * 0.5f);
            rItm.transform.localScale = Vector3.one * spriteScale;
        }

        base.Start();
	}

    // Update is called once per frame
    public override void Update () {
        base.Update();
	}
}
