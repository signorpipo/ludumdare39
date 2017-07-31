using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectGameManager : MinigameManager
{
    public RectTransform LeftPanel;
    public RectTransform RightPanel;

    public List<Sprite> GameItems;

    public MonoBehaviour ItemPrefab;

    private const float DEFAULT_IMAGE_WIDTH = 100f;

    private int mNumGameItems = 0;
    private int mMatchedItem = 0;

    // 1 facile, 0 difficile
    public override void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        mNumGameItems = 8 - (int)(4f * i_NumItemValue);
        mGameTotalTime = mGameTime = 6f + 8f * i_TimeValue;
        mCurrState = GameState.PLAY_GAME;
        mScore = 0;

        // Pulisco le due liste
        int childs = LeftPanel.transform.childCount;
        for (int i = childs - 1; i > 0; i--)
        {
            GameObject.Destroy(LeftPanel.transform.GetChild(i).gameObject);
            GameObject.Destroy(RightPanel.transform.GetChild(i).gameObject);
        }

        float deltaHeight = LeftPanel.rect.height * 0.95f / mNumGameItems;
        List<Vector3> leftitemsPos = new List<Vector3>();
        // Inizializzazione item
        for (int i = 0; i < mNumGameItems; ++i)
        {
            int spriteIdx = (int)(UnityEngine.Random.value * GameItems.Count) % GameItems.Count;
            int colorIdx = (int)(UnityEngine.Random.value * 2f);
            float itemScale = Math.Min(LeftPanel.rect.width * 0.95f / DEFAULT_IMAGE_WIDTH, deltaHeight / DEFAULT_IMAGE_WIDTH);

            MonoBehaviour lItm = Instantiate(ItemPrefab, LeftPanel);
            lItm.GetComponent<Image>().sprite = GameItems[spriteIdx];
            lItm.GetComponent<Image>().color = ColorizeBck.yellowColors[colorIdx];
            lItm.GetComponent<DropHandler>().onDroppedItem += ConnectGameManager_onDroppedItem;
            lItm.transform.localPosition = Vector3.up * (deltaHeight * i - (LeftPanel.rect.height * 0.95f - DEFAULT_IMAGE_WIDTH * itemScale) * 0.5f);
            lItm.transform.localScale = Vector3.one * itemScale;
            leftitemsPos.Add(lItm.transform.localPosition);

            MonoBehaviour rItm = Instantiate(ItemPrefab, RightPanel);
            rItm.GetComponent<Image>().sprite = GameItems[spriteIdx];
            rItm.GetComponent<Image>().color = ColorizeBck.yellowColors[colorIdx];
            rItm.GetComponent<DropHandler>().onDroppedItem += ConnectGameManager_onDroppedItem;
            rItm.transform.localScale = Vector3.one * itemScale;
        }

        // secondo ciclo di rimescolamento item di destra sulla base delle posizioni di sinistra
        for (int i = 0; i < mNumGameItems; ++i)
        {
            int posIdx = (int)(UnityEngine.Random.value * leftitemsPos.Count) % leftitemsPos.Count;
            RightPanel.transform.GetChild(i).transform.localPosition = leftitemsPos[posIdx];
            leftitemsPos.RemoveAt(posIdx);
        }
    }

    // Use this for initialization
    public override void Start () {
        StartMinigame(0, 1f, 1f, 0f);
        base.Start();
	}

    private void ConnectGameManager_onDroppedItem(bool matched)
    {
        if (matched)
        {
            mMatchedItem++;
            mScore = (float)mMatchedItem / (float)mNumGameItems * 0.85f;
            if (mMatchedItem == mNumGameItems)
                SceneEnded(1.0f);
        }
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
	}
}
