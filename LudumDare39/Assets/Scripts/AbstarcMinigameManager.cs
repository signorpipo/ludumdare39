using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnSceneEnded(float result);

public abstract class AbstarcMinigameManager : MonoBehaviour
{

    public event OnSceneEnded onSceneEnded = null;

    private F_Timer m_PrefabTimer;
    private F_Timer m_Timer;

    public abstract void StartMinigame(int i_Type, float i_TimeValue, float i_NumItemValue, float i_ItemVelocityValue); //i_type will be between 0 and 2

    protected void SceneEnded(float result) // You need to call this method at the end of the scene
    {
        if (onSceneEnded != null)
        {
            onSceneEnded(result);
        }
    }

    public void StartTimer(Transform i_TimerParent, F_TimesUpEvent i_OnGameStart, F_TimesUpEvent i_OnTimesUp, string i_StartTitle, string i_StartMessage, int i_GameStartSeconds, string i_TimesUpMessage, int i_TimerSeconds)
    {
        if(m_PrefabTimer == null)
        {
            m_PrefabTimer = Resources.Load<F_Timer>("Prefabs/F_Timer");
        }

        if(m_Timer != null)
        {
            Destroy(m_Timer);
        }
        m_Timer = Instantiate(m_PrefabTimer).GetComponent<F_Timer>();
        m_Timer.transform.SetParent(i_TimerParent.transform);
        m_Timer.StartTimer(i_OnGameStart, i_OnTimesUp, i_StartTitle, i_StartMessage, i_GameStartSeconds, i_TimesUpMessage, i_TimerSeconds);
    }

    public void StopTimer()
    {
        m_Timer.StopTimer();
    }

    public int GetSecondsLeft()
    {
        return m_Timer.GetSecondsLeft();
    }

}

