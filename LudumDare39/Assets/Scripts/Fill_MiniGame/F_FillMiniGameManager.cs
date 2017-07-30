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
    private NamedPluggable[] m_PrefabPhysicsPluggables;

    [SerializeField]
    private NamedPluggable[] m_PrefabMoneyPluggables;

    [SerializeField]
    private NamedPluggable[] m_PrefabSocialPluggables;

    [SerializeField]
    private Sprite[] m_Backgrounds;

    [SerializeField]
    private Canvas m_WinCanvas;

    [SerializeField]
    private LineRenderer m_PrefabGridLine;
    [SerializeField]
    private SpriteRenderer m_PrefabCellBackground;

    [SerializeField]
    private F_Timer m_PrefabTimer;

    [SerializeField]
    private bool m_Restart;

    private Dictionary<string, F_Pluggable>[] m_PrefabPluggablesDictionary;
    private int m_StartEndSeconds = 3;
    private int m_ToDock;
    private float m_CellSize = 1;

    private GameObject m_LoadedLevel;
    private F_Timer m_Timer;
    private F_GridManager m_GridManager;
    private F_GrabManager m_GrabManager;


    private float m_PhysicsValue;
    private float m_MoneyValue;
    private float m_SocialValue;

    private int m_Kind;

    public void Awake () {

        m_PrefabPluggablesDictionary = new Dictionary<string, F_Pluggable>[3];
        m_PrefabPluggablesDictionary[0] = new Dictionary<string, F_Pluggable>();
        m_PrefabPluggablesDictionary[1] = new Dictionary<string, F_Pluggable>();
        m_PrefabPluggablesDictionary[2] = new Dictionary<string, F_Pluggable>();
        foreach (NamedPluggable namedPluggable in m_PrefabPhysicsPluggables)
        {
            m_PrefabPluggablesDictionary[0].Add(namedPluggable.m_Name, namedPluggable.m_Pluggable);
        }
        foreach (NamedPluggable namedPluggable in m_PrefabMoneyPluggables)
        {
            m_PrefabPluggablesDictionary[1].Add(namedPluggable.m_Name, namedPluggable.m_Pluggable);
        }
        foreach (NamedPluggable namedPluggable in m_PrefabSocialPluggables)
        {
            m_PrefabPluggablesDictionary[2].Add(namedPluggable.m_Name, namedPluggable.m_Pluggable);
        }

    }

    private void Update()
    {
        if (m_Restart)
        {
            m_Restart = false;
            StartGame(0,0,0,0);
        }
    }




    public void StartGame(float i_PhysicsValue, float i_MoneyValue, float i_SocialValue, int i_Kind)
    {
        m_PhysicsValue = i_PhysicsValue;
        m_MoneyValue = i_MoneyValue;
        m_SocialValue = i_SocialValue;
        m_Kind = i_Kind;

        DeleteOldLevel();

        m_LoadedLevel = new GameObject("Level");
        m_LoadedLevel.transform.SetParent(this.transform);

        LoadLevel();

        switch (m_Kind)
        {
            case 0:
                if (m_PhysicsValue < 0.35f)
                {
                    m_StartEndSeconds = 7;
                }
                else if (m_PhysicsValue < 0.7f)
                {
                    m_StartEndSeconds = 13;
                }
                else
                {
                    m_StartEndSeconds = 17;
                }
                break;
            case 1:
                if (m_PhysicsValue < 0.35f)
                {
                    m_StartEndSeconds = 7;
                }
                else if (m_PhysicsValue < 0.7f)
                {
                    m_StartEndSeconds = 13;
                }
                else
                {
                    m_StartEndSeconds = 17;
                }
                break;
            case 2:
                if (m_PhysicsValue < 0.35f)
                {
                    m_StartEndSeconds = 7;
                }
                else if (m_PhysicsValue < 0.7f)
                {
                    m_StartEndSeconds = 13;
                }
                else
                {
                    m_StartEndSeconds = 17;
                }
                break;
        }
        m_Timer.StartTimer(OnStartTimerEnd, OnFailure, "Fill the bag!", m_StartEndSeconds, "Times Up!!", 15);

    }

    private void DeleteOldLevel()
    {
        if(m_LoadedLevel != null)
        {
            Destroy(m_LoadedLevel);
        }
        m_LoadedLevel = null;
        m_Timer = null;
        m_GridManager = null;
        m_GrabManager = null;

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

    private void LoadLevel()
    {
        GameObject background = new GameObject("Background");
        background.transform.SetParent(m_LoadedLevel.transform);
        SpriteRenderer spriteRenderer = background.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = m_Backgrounds[m_Kind];
        spriteRenderer.color = new Color(1, 1, 1, 190/255.0f);
        background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, 5);

        GameObject grid = new GameObject("Grid");
        grid.transform.SetParent(m_LoadedLevel.transform);
        m_GridManager = grid.AddComponent<F_GridManager>();

        GameObject grabbables = new GameObject("Grabbables");
        grabbables.transform.SetParent(m_LoadedLevel.transform);
        m_GrabManager = grabbables.AddComponent<F_GrabManager>();
        m_GrabManager.OnTryDockEvent += TryDock;
        m_GrabManager.DisableInput();

        if (m_PrefabTimer != null)
        {
            m_Timer = Instantiate(m_PrefabTimer).GetComponent<F_Timer>();
            m_Timer.transform.SetParent(m_LoadedLevel.transform);
        }

        switch (m_Kind)
        {
            case 0:
                if (m_MoneyValue < 0.35f)
                {
                    Load1();
                }
                else if (m_MoneyValue < 0.7f)
                {
                    Load2();
                }
                else
                {
                    Load3();
                }
                break;
            case 1:
                if (m_MoneyValue < 0.35f)
                {
                    Load1();
                }
                else if (m_MoneyValue < 0.7f)
                {
                    Load2();
                }
                else
                {
                    Load3();
                }
                break;
            case 2:
                if (m_MoneyValue < 0.35f)
                {
                    Load1();
                }
                else if (m_MoneyValue < 0.7f)
                {
                    Load2();
                }
                else
                {
                    Load3();
                }
                break;
        }
    }

    IEnumerator LoadNextScene(bool i_Success)
    {
        yield return new WaitForSeconds(m_StartEndSeconds);

        SceneEnd((i_Success) ? 1 : 0);
    }

    private void OnStartTimerEnd()
    {
        m_GrabManager.EnableInput();
    }

    private void OnFailure()
    {
        EngGame(false);
    }

    private void EngGame(bool i_Success)
    {
        m_GrabManager.DisableInput();
        m_Timer.StopTimer();
        StartCoroutine(LoadNextScene(i_Success));
    }

    private void Load1()
    {
        int toLoad = UnityEngine.Random.Range(0, 3);
        switch (toLoad)
        {
            case 0:
                LoadPhysics1();
                break;
            case 1:
                LoadMoney1();
                break;
            case 2:
                LoadSocial1();
                break;
        }
    }
    private void Load2()
    {
        int toLoad = UnityEngine.Random.Range(0, 3);
        switch (toLoad)
        {
            case 0:
                LoadPhysics2();
                break;
            case 1:
                LoadMoney2();
                break;
            case 2:
                LoadSocial2();
                break;
        }
    }
    private void Load3()
    {
        int toLoad = UnityEngine.Random.Range(0, 3);
        switch (toLoad)
        {
            case 0:
                LoadPhysics3();
                break;
            case 1:
                LoadMoney3();
                break;
            case 2:
                LoadSocial3();
                break;
        }
    }

    private void LoadPhysics1()
    {
        m_GridManager.Initialize(Physics1Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Physics1Grab();

    }
    private void LoadPhysics2()
    {
        m_GridManager.Initialize(Physics2Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Physics2Grab();
    }
    private void LoadPhysics3()
    {
        m_GridManager.Initialize(Physics3Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Physics3Grab();
    }

    private void LoadMoney1()
    {
        m_GridManager.Initialize(Money1Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Money1Grab();
    }
    private void LoadMoney2()
    {
        m_GridManager.Initialize(Money2Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Money2Grab();
    }
    private void LoadMoney3()
    {
        m_GridManager.Initialize(Money3Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Money3Grab();
    }

    private void LoadSocial1()
    {
        m_GridManager.Initialize(Social1Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Social1Grab();
    }
    private void LoadSocial2()
    {
        m_GridManager.Initialize(Social2Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Social2Grab();
    }
    private void LoadSocial3()
    {
        m_GridManager.Initialize(Social3Grid(), m_CellSize, m_PrefabGridLine, m_PrefabCellBackground, m_GrabManager);
        Social3Grab();
    }

    private F_Matrix Physics1Grid()
    {
        return FakeGrid();
    }

    private F_Matrix Physics2Grid()
    {
        return new F_Matrix(4, 8);
    }

    private F_Matrix Physics3Grid()
    {
        return new F_Matrix(4, 8);
    }

    private F_Matrix Money1Grid()
    {
        return new F_Matrix(4, 8);
    }

    private F_Matrix Money2Grid()
    {
        return new F_Matrix(4, 8);
    }

    private F_Matrix Money3Grid()
    {
        return new F_Matrix(4, 8);
    }

    private F_Matrix Social1Grid()
    {
        return new F_Matrix(4, 8);
    }

    private F_Matrix Social2Grid()
    {
        return new F_Matrix(4, 8);
    }

    private F_Matrix Social3Grid()
    {
        return new F_Matrix(4, 8);
    }


    private void Physics1Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[3];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(7.2f, 0, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_PPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.6f, 0, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Physics2Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Physics3Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Money1Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Money2Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Money3Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Social1Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Social2Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Social3Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

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


}
