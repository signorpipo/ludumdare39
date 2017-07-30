using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnSceneEnded(float result);

public abstract class AbstarcMinigameManager : MonoBehaviour {

    public event OnSceneEnded onSceneEnded = null;

    public abstract void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue); //i_type will be between 0 and 2

    protected void SceneEnded(float result) // You need to call this method at the end of the scene
    {
        if(onSceneEnded !=null)
        {
            onSceneEnded(result);
        }
    }

}

