using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class F_FillMiniGameManager : MonoBehaviour {

    [System.Serializable]
    public struct NamedPluggable
    {
        public string m_Name;
        public F_Pluggable m_Pluggable;
    }

    [SerializeField]
    private NamedPluggable[] m_PrefabPluggables;

    [SerializeField]
    private Canvas m_WinCanvas;

    [SerializeField]
    private LineRenderer m_PrefabGridLine;
    [SerializeField]
    private SpriteRenderer m_PrefabCellBackground;

    private F_GridManager m_GridManager;
    private F_GrabManager m_GrabManager;
    private Dictionary<string, F_Pluggable> m_PrefabPluggablesDictionary;
    private int m_ToDock;

    [SerializeField]
    private CM_Timer m_PrefabTimer;
    [SerializeField]
    private GameObject m_LevelTimer;
    [SerializeField]
    private GameObject m_MiddleMessage;
    private CM_Timer m_StartTimer;
    private CM_Timer m_RunningTimer;

    public void Awake () {

        m_PrefabPluggablesDictionary = new Dictionary<string, F_Pluggable>();
        foreach (NamedPluggable namedPluggable in m_PrefabPluggables)
        {
            m_PrefabPluggablesDictionary.Add(namedPluggable.m_Name, namedPluggable.m_Pluggable);
        }

        GameObject grid = new GameObject("Grid");
        grid.transform.parent = this.transform;
        m_GridManager = grid.AddComponent<F_GridManager>();

        GameObject grabbables = new GameObject("Grabbables");
        grabbables.transform.parent = this.transform;
        m_GrabManager = grabbables.AddComponent<F_GrabManager>();
        m_GrabManager.OnTryDockEvent += TryDock;
        m_GrabManager.DisableInput();

        ChooseLevel();

        if (m_PrefabTimer != null)
        {
            m_StartTimer = Instantiate(m_PrefabTimer).GetComponent<CM_Timer>();
            m_RunningTimer = Instantiate(m_PrefabTimer).GetComponent<CM_Timer>();
        }

    }

    private void Start()
    {
        m_MiddleMessage.SetActive(true);
        m_MiddleMessage.GetComponentInChildren<Text>().text = "Fill the bag!";
        m_StartTimer.OnTimesUp += OnStartTimerEnd;
        m_StartTimer.StartTimer(2, "Fill the bag!", "", null);
    }

    private void TryDock(F_Pluggable i_ToDock)
    {
        if (m_GridManager.TryDock(i_ToDock))
        {
            m_GrabManager.ReleaseGrabbed(false);
            if(m_GridManager.GetDocked() == m_ToDock)
            {
                m_GrabManager.DisableInput();
                m_WinCanvas.gameObject.SetActive(true);
                EngGame(true);
            }
        }
    }

    private void ChooseLevel()
    {
        LoadGrid();
        LoadGrab();
    }

    private void LoadGrid()
    {
        m_GridManager.Initialize(FakeGrid(), 1, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
    }

    private void LoadGrab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[3];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(7.2f, 0, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary["F_PPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.6f, 0, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);

        m_ToDock = pluggables.GetLength(0);
    }

    private F_Matrix FakeGrid()
    {

        F_Matrix fakeGrid = new F_Matrix(4, 8);
        fakeGrid.Set(0, 0, 1);
        fakeGrid.Set(1, 0, 0);
        fakeGrid.Set(2, 0, 1);
        fakeGrid.Set(3, 0, 1);
        fakeGrid.Set(0, 1, 1);
        fakeGrid.Set(1, 1, 1);
        fakeGrid.Set(2, 1, 1);
        fakeGrid.Set(3, 1, 0);
        fakeGrid.Set(0, 2, 1);
        fakeGrid.Set(1, 2, 1);
        fakeGrid.Set(2, 2, 1);
        fakeGrid.Set(3, 2, 0);
        fakeGrid.Set(0, 3, 1);
        fakeGrid.Set(1, 3, 1);
        fakeGrid.Set(2, 3, 1);
        fakeGrid.Set(3, 3, 1);
        fakeGrid.Set(0, 4, 1);
        fakeGrid.Set(1, 4, 1);
        fakeGrid.Set(2, 4, 1);
        fakeGrid.Set(3, 4, 1);
        fakeGrid.Set(0, 5, 1);
        fakeGrid.Set(1, 5, 0);
        fakeGrid.Set(2, 5, 1);
        fakeGrid.Set(3, 5, 1);
        fakeGrid.Set(0, 6, 1);
        fakeGrid.Set(1, 6, 1);
        fakeGrid.Set(2, 6, 1);
        fakeGrid.Set(3, 6, 1);
        fakeGrid.Set(0, 7, 0);
        fakeGrid.Set(1, 7, 0);
        fakeGrid.Set(2, 7, 0);
        fakeGrid.Set(3, 7, 1);

        return fakeGrid;
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3);

        m_WinCanvas.gameObject.SetActive(false);
    }

    private void OnStartTimerEnd()
    {
        m_MiddleMessage.SetActive(false);
        m_LevelTimer.SetActive(true);
        m_RunningTimer.OnTimesUp += OnFailure;
        m_RunningTimer.StartTimer(10, "Time Left: ", "", m_LevelTimer.GetComponentInChildren<Text>());
        m_GrabManager.EnableInput();
    }

    private void OnFailure()
    {
        m_MiddleMessage.GetComponentInChildren<Text>().text = "You Lose";
        m_MiddleMessage.SetActive(true);
        EngGame(false);
    }

    private void EngGame(bool i_Success)
    {
        m_GrabManager.DisableInput();
        m_RunningTimer.StopTimer();
        if (i_Success)
        {

        }
        else
        {

        }
        StartCoroutine(LoadNextScene());
    }


}
