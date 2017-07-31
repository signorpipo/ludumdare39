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

    private const float MIN_GAME_TIME = 6f;
    private const float MAX_GAME_TIME = 14f;

    private const float MIN_NUM_ITEMS = 4f;
    private const float MAX_NUM_ITEMS = 8f;

    private const float DELTA_NONMATCHED_ITEMS = 3;

    private int mNumGameItems = 0;
    private int mMatchedItem = 0;

    // 1 facile, 0 difficile
    public override void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue) //i_type will be between 0 and 2
    {
        ColorizeBckManager.BckTypes type = ColorizeBckManager.BckTypes.BCK_PLANNING;
        switch (i_Type)
        {
            case 0: type = ColorizeBckManager.BckTypes.BCK_PHYSICS; break;
            case 1: type = ColorizeBckManager.BckTypes.BCK_MONEY; break;
            case 2: type = ColorizeBckManager.BckTypes.BCK_SOCIAL; break;
        }
        FindObjectOfType<ColorizeBckManager>().SetUncoloredBckType(type);

        mNumGameItems = (int)(MAX_NUM_ITEMS - (MIN_NUM_ITEMS * i_NumItemValue));
        mGameTotalTime = mGameTime = MIN_GAME_TIME + (MAX_GAME_TIME - MIN_GAME_TIME) * i_TimeValue;
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
            Color color = (colorIdx == 0 ? new Color(1f,0.5f,0.5f) : colorIdx == 1 ? new Color(0.5f, 1f, 0.5f) : new Color(0.5f, 0.5f, 1.0f));
            float itemScale = Math.Min(LeftPanel.rect.width * 0.95f / DEFAULT_IMAGE_WIDTH, deltaHeight / DEFAULT_IMAGE_WIDTH);

            MonoBehaviour lItm = Instantiate(ItemPrefab, LeftPanel);
            lItm.GetComponent<Image>().sprite = GameItems[spriteIdx];
            lItm.GetComponent<Image>().color = color;
            lItm.GetComponent<DropHandler>().onDroppedItem += ConnectGameManager_onDroppedItem;
            lItm.transform.localPosition = Vector3.up * (deltaHeight * i - (LeftPanel.rect.height * 0.95f - DEFAULT_IMAGE_WIDTH * itemScale) * 0.5f);
            lItm.transform.localScale = Vector3.one * itemScale;
            leftitemsPos.Add(lItm.transform.localPosition);

            MonoBehaviour rItm = Instantiate(ItemPrefab, RightPanel);
            rItm.GetComponent<Image>().sprite = GameItems[spriteIdx];
            rItm.GetComponent<Image>().color = color;
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
    public override void Start ()
    {
        base.Start();
	}

    private void ConnectGameManager_onDroppedItem(bool matched)
    {
        if (matched)
        {
            mAudioSource.PlayOneShot(mWinSound, 0.5F);
            mMatchedItem++;
            mScore = (float)mMatchedItem / (float)mNumGameItems * 0.85f;
            if (mMatchedItem == mNumGameItems)
            {
                // Bonus se si finisce prima dello scadere
                mScore += 0.15f * Mathf.Clamp(mGameTime / (MIN_GAME_TIME * 0.5f),0f,1f);
                SceneEnded(1.0f);
            }
        }
        else
        {
            // Trig infame
            mNumGameItems--;
            mAudioSource.PlayOneShot(mTimesUpSound, 0.5F);
        }
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
	}
}
