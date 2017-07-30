using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_FillMiniGameManager : MonoBehaviour {

    [System.Serializable]
    public struct NamedPluggable
    {
        public string m_Name;
        public F_Pluggable m_Pluggable;
    }

    public NamedPluggable[] m_PrefabPluggables;

    public LineRenderer m_PrefabGridLine;

    private F_GridManager m_GridManager;
    private F_GrabManager m_GrabManager;
    private Dictionary<string, F_Pluggable> m_PrefabPluggablesDictionary;
    private int m_ToDock;
    
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

        ChooseLevel();

    }

    private void TryDock(F_Pluggable i_ToDock)
    {
        if (m_GridManager.TryDock(i_ToDock))
        {
            m_GrabManager.ReleaseGrabbed();
            if(m_GridManager.GetDocked() == m_ToDock)
            {
                //WIN
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
        m_GridManager.Initialize(FakeGrid(), 1, m_PrefabGridLine);
    }

    private void LoadGrab()
    {
        F_Pluggable[] pluggables = new F_Pluggable[1];

        GameObject pluggableObject = Instantiate(m_PrefabPluggablesDictionary["ZPiece"].gameObject, Vector3.zero, Quaternion.identity, m_GrabManager.gameObject.transform);
        pluggableObject.transform.position = new Vector3(5.5f, 0, 1);
        pluggables[0] = pluggableObject.GetComponent<F_Pluggable>();
        m_GrabManager.Initialize(pluggables);

        m_ToDock = pluggables.GetLength(0);
    }

    private F_Matrix FakeGrid()
    {

        F_Matrix fakeGrid = new F_Matrix(4, 4);
        fakeGrid.Set(0, 0, 1);
        fakeGrid.Set(1, 0, 0);
        fakeGrid.Set(2, 0, 1);
        fakeGrid.Set(3, 0, 1);
        fakeGrid.Set(0, 1, 1);
        fakeGrid.Set(1, 1, 1);
        fakeGrid.Set(2, 1, 1);
        fakeGrid.Set(3, 1, 0);
        fakeGrid.Set(0, 2, 0);
        fakeGrid.Set(1, 2, 1);
        fakeGrid.Set(2, 2, 1);
        fakeGrid.Set(3, 2, 0);
        fakeGrid.Set(0, 3, 0);
        fakeGrid.Set(1, 3, 1);
        fakeGrid.Set(2, 3, 1);
        fakeGrid.Set(3, 3, 1);

        return fakeGrid;
    }

}
