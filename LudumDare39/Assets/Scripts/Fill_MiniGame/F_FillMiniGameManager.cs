using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class F_FillMiniGameManager : AbstarcMinigameManager {

    [System.Serializable]
    public struct NamedPluggable
    {
        public string m_Name;
        public F_Pluggable m_Pluggable;
    }

    [SerializeField]
    [TextArea(3, 20)]
    private string m_StartMessage = "Start";

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
    private bool m_Restart;
    [SerializeField]
    private float[] m_StartInput;
    [SerializeField]
    private int m_Level = -1;

    private Dictionary<string, F_Pluggable>[] m_PrefabPluggablesDictionary;
    private int m_StartSeconds = 4;
    private int m_EndSeconds = 2;
    private int m_Seconds = 2;
    private int m_ToDock;
    private float m_CellSize = 1;

    private GameObject m_LoadedLevel;
    private F_GridManager m_GridManager;
    private F_GrabManager m_GrabManager;

    private float m_PhysicsValue;
    private float m_MoneyValue;
    private float m_SocialValue;

    private int m_Kind;

    private AudioSource m_AudioSource;
    private AudioClip m_DockSound;
    private AudioClip m_WinSound;

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

        m_AudioSource = gameObject.AddComponent<AudioSource>();
        m_DockSound = Resources.Load<AudioClip>("Audio/SFX/F_Dock");
        m_WinSound = Resources.Load<AudioClip>("Audio/SFX/F_Win");

    }

    private void Update()
    {
        if (m_Restart)
        {
            m_WinCanvas.gameObject.SetActive(false);
            StartMinigame((int)m_StartInput[0], m_StartInput[1], m_StartInput[2], m_StartInput[3]);
        }
    }

    public override void StartMinigame(int i_Kind, float i_PhysicsValue, float i_MoneyValue, float i_SocialValue)
    {
        m_Restart = false;

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
                    m_Seconds = 10;
                }
                else if (m_PhysicsValue < 0.7f)
                {
                    m_Seconds = 13;
                }
                else
                {
                    m_Seconds = 16;
                }
                break;
            case 1:
                if (m_MoneyValue < 0.35f)
                {
                    m_Seconds = 10;
                }
                else if (m_MoneyValue < 0.7f)
                {
                    m_Seconds = 13;
                }
                else
                {
                    m_Seconds = 16;
                }
                break;
            case 2:
                if (m_SocialValue < 0.35f)
                {
                    m_Seconds = 10;
                }
                else if (m_SocialValue < 0.7f)
                {
                    m_Seconds = 13;
                }
                else
                {
                    m_Seconds = 16;
                }
                break;
        }

        StartTimer(m_LoadedLevel.transform, OnStartTimerEnd, OnFailure, "Fill the bag!", m_StartMessage, m_StartSeconds, "Times Up!!", m_Seconds);

    }

    private void DeleteOldLevel()
    {
        if(m_LoadedLevel != null)
        {
            Destroy(m_LoadedLevel);
        }
        m_LoadedLevel = null;
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
                m_AudioSource.PlayOneShot(m_WinSound, 0.7F);
                m_GrabManager.DisableInput();
                m_WinCanvas.gameObject.SetActive(true);
                EngGame(true);
            }
            else
            {
                m_AudioSource.PlayOneShot(m_DockSound, 0.7F);
            }
        }else
        {
            m_GrabManager.ReleaseGrabbed(true);
        }
    }

    private void LoadLevel()
    {
        GameObject background = new GameObject("Background");
        background.transform.SetParent(m_LoadedLevel.transform);
        SpriteRenderer spriteRenderer = background.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = m_Backgrounds[m_Kind];
        spriteRenderer.color = new Color(1, 1, 1, 170/255.0f);
        background.transform.position = new Vector3(background.transform.position.x, background.transform.position.y, 5);

        GameObject grid = new GameObject("Grid");
        grid.transform.SetParent(m_LoadedLevel.transform);
        m_GridManager = grid.AddComponent<F_GridManager>();

        GameObject grabbables = new GameObject("Grabbables");
        grabbables.transform.SetParent(m_LoadedLevel.transform);
        m_GrabManager = grabbables.AddComponent<F_GrabManager>();
        m_GrabManager.OnTryDockEvent += TryDock;
        m_GrabManager.DisableInput();

        if (m_Level >= 0)
        {
            switch (m_Level)
            {
                case 0:
                    LoadPhysics1();
                    break;
                case 1:
                    LoadPhysics2();
                    break;
                case 2:
                    LoadPhysics3();
                    break;
                case 3:
                    LoadMoney1();
                    break;
                case 4:
                    LoadMoney2();
                    break;
                case 5:
                    LoadMoney3();
                    break;
                case 6:
                    LoadSocial1();
                    break;
                case 7:
                    LoadSocial2();
                    break;
                case 8:
                    LoadSocial3();
                    break;
            }

        }
        else
        {

            switch (m_Kind)
            {
                case 0:
                    if (m_SocialValue < 0.35f)
                    {
                        Load3();
                    }
                    else if (m_SocialValue < 0.7f)
                    {
                        Load2();
                    }
                    else
                    {
                        Load1();
                    }
                    break;
                case 1:
                    if (m_PhysicsValue < 0.35f)
                    {
                        Load3();
                    }
                    else if (m_PhysicsValue < 0.7f)
                    {
                        Load2();
                    }
                    else
                    {
                        Load1();
                    }
                    break;
                case 2:
                    if (m_MoneyValue < 0.35f)
                    {
                        Load3();
                    }
                    else if (m_MoneyValue < 0.7f)
                    {
                        Load2();
                    }
                    else
                    {
                        Load1();
                    }
                    break;
            }

        }
    }

    IEnumerator LoadNextScene(bool i_Success)
    {
        yield return new WaitForSeconds(m_EndSeconds);

        SceneEnded((i_Success) ? 1 : 0);
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
        StopTimer();
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
        F_Matrix grid = new F_Matrix(5, 6);

        for (int row = 0; row < grid.Rows(); ++row)
        {
            for (int column = 1; column < grid.Columns()-1; ++column)
            {
                grid.Set(row, column, 1);
            }

        }

        grid.Set(1, 0, 1);
        grid.Set(2, 0, 1);
        grid.Set(3, 0, 1);

        grid.Set(1, 5, 1);
        grid.Set(2, 5, 1);
        grid.Set(3, 5, 1);

        return grid;

    }

    private F_Matrix Physics2Grid()
    {
        F_Matrix grid = new F_Matrix(6, 5);

        for (int row = 0; row < 3; ++row)
        {
            for (int column = 0; column < 3; ++column)
            {
                grid.Set(row, column, 1);
            }

        }

        for (int row = 3; row < 6; ++row)
        {
            for (int column = 2; column < 5; ++column)
            {
                grid.Set(row, column, 1);
            }

        }

        return grid;
    }

    private F_Matrix Physics3Grid()
    {
        F_Matrix grid = new F_Matrix(8, 6);

        grid.Set(0,2, 1);

        grid.Set(1, 2, 1);
        grid.Set(1, 3, 1);
        grid.Set(1, 4, 1);

        grid.Set(2, 0, 1);

        grid.Set(3, 0, 1);
        grid.Set(3, 3, 1);
        grid.Set(3, 4, 1);

        grid.Set(4, 0, 1);
        grid.Set(4, 1, 1);
        grid.Set(4, 4, 1);
        grid.Set(4, 5, 1);

        grid.Set(6, 2, 1);
        grid.Set(6, 3, 1);
        grid.Set(6, 4, 1);

        grid.Set(7, 3, 1);


        return grid;
    }

    private F_Matrix Money1Grid()
    {
        F_Matrix grid = new F_Matrix(5, 5);

        for (int row = 0; row < grid.Rows(); ++row)
        {
            for (int column = 0; column < grid.Columns(); ++column)
            {
                grid.Set(row, column, 1);
            }

        }

        grid.Set(grid.Rows() - 1, 0, 0);
        grid.Set(0, grid.Columns()-1, 0);

        return grid;
    }

    private F_Matrix Money2Grid()
    {
        F_Matrix grid = new F_Matrix(6, 6);

        for (int row = 0; row < 6; ++row)
        {
            grid.Set(row, 2, 1);
            grid.Set(row, 3, 1);

        }

        for (int column = 0; column < 6; ++column)
        {
            grid.Set(2, column, 1);
            grid.Set(3, column, 1);

        }

        return grid;
    }

    private F_Matrix Money3Grid()
    {
        F_Matrix grid = new F_Matrix(7, 7);

        grid.Set(0, 4, 1);

        grid.Set(1, 2, 1);
        grid.Set(1, 3, 1);
        grid.Set(1, 4, 1);

        grid.Set(2, 0, 1);
        grid.Set(2, 1, 1);
        grid.Set(2, 5, 1);

        grid.Set(3, 1, 1);
        grid.Set(3, 5, 1);

        grid.Set(4, 1, 1);
        grid.Set(4, 5, 1);
        grid.Set(4, 6, 1);

        grid.Set(5, 2, 1);
        grid.Set(5, 3, 1);
        grid.Set(5, 4, 1);

        grid.Set(6, 2, 1);


        return grid;
    }

    private F_Matrix Social1Grid()
    {
        F_Matrix grid = new F_Matrix(5, 5);

        for (int row = 0; row < grid.Rows(); ++row)
        {
            for (int column = 0; column < grid.Columns(); ++column)
            {
                grid.Set(row, column, 1);
            }

        }

        grid.Set(2, 2, 0);

        return grid;
    }

    private F_Matrix Social2Grid()
    {
        F_Matrix grid = new F_Matrix(5, 6);

        for (int row = 0; row < 5; ++row)
        {
            grid.Set(row, 2, 1);
            grid.Set(row, 3, 1);

        }

        for (int column = 0; column < 6; ++column)
        {
            grid.Set(0, column, 1);
            grid.Set(1, column, 1);

        }

        return grid;
    }

    private F_Matrix Social3Grid()
    {
        F_Matrix grid = new F_Matrix(4,7);

        grid.Set(0, 0, 1);
        grid.Set(0, 3, 1);
        grid.Set(0, 6, 1);

        grid.Set(1, 0, 1);
        grid.Set(1, 2, 1);
        grid.Set(1, 3, 1);
        grid.Set(1, 4, 1);
        grid.Set(1, 6, 1);

        grid.Set(2, 0, 1);
        grid.Set(2, 1, 1);
        grid.Set(2, 2, 1);
        grid.Set(2, 3, 1);
        grid.Set(2, 4, 1);
        grid.Set(2, 5, 1);
        grid.Set(2, 6, 1);

        grid.Set(3, 3, 1);

        return grid;
    }


    private void Physics1Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[3];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_TPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, -2, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(7.2f, 0, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, 2, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Physics2Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[3];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(6, 2, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(5, -1, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, 2, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Physics3Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[4];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_TPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.5f, -1.5f, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(7.5f, 0, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(1.5f, 0, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.5f, 1.5f, 1);
        pluggables[3] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Money1Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[3];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, 2, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(7f, 1.5f, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_TPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.5f, -1, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Money2Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[5];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(6.5f, 2.5f, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, 2.5f, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(5, 0, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(6.5f, -2.5f, 1);
        pluggables[3] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, -2.5f, 1);
        pluggables[4] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Money3Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[4];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, 2, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, -2, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(6.5f, 2, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(6.5f, -2, 1);
        pluggables[3] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Social1Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[3];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, 1.5f, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, -1.5f, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(6f, 1, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Social2Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[3];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_OPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(3.5f, 2, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(7f, 1.5f, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_ZPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.5f, -1, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }

    private void Social3Grab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[4];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_TPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.5f, 2, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_TPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(4.5f, -2, 1);
        pluggables[1] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(1.5f, 0, 1);
        pluggables[2] = pluggableObject.GetComponent<F_Pluggable>();

        pluggableObject = Instantiate(m_PrefabPluggablesDictionary[m_Kind]["F_LPieceInverse"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(7.5f, 0, 1);
        pluggables[3] = pluggableObject.GetComponent<F_Pluggable>();

        m_GrabManager.Initialize(pluggables);
        m_ToDock = pluggables.GetLength(0);
    }
}
