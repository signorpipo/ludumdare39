using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_GridManager : MonoBehaviour {

    private LineRenderer m_PrefabGridLine;
    private SpriteRenderer m_PrefabCellBackground;

    private float m_CellSize;
    private Vector2 m_TopLeft;
    private F_Matrix m_Grid;
    private F_GrabManager m_GrabManager;

    private int m_Docked;

    public bool TryDock(F_Pluggable i_ToDock)
    {
        Rect gridRect = GetRect();
        Rect pluggableRect = i_ToDock.GetRect();
        pluggableRect = AlignToGrid(gridRect, pluggableRect);
        if (IsInside(gridRect, pluggableRect))
        {
            Vector2 topLeftInGrid = GetTopLeftInGrid(gridRect, pluggableRect);
            if(IsGridFree(topLeftInGrid, i_ToDock.GetGrid()))
            {
                FillGrid(topLeftInGrid, i_ToDock.GetGrid());
                i_ToDock.OnClickEvent += RemoveFromGrid;
                i_ToDock.Dock(new Vector2(pluggableRect.x, pluggableRect.y));
                ++m_Docked;
                return true;
            }
        }
        return false;
    }

    private Rect AlignToGrid(Rect i_GridRect, Rect i_PluggableRect)
    {

        for (int row = 0; row <= m_Grid.Rows(); ++row)
        {
            if (i_PluggableRect.y < i_GridRect.y - m_CellSize * (row - 0.5f) && i_PluggableRect.y > i_GridRect.y - m_CellSize * (row + 0.5f))
            {
                i_PluggableRect.y = i_GridRect.y - m_CellSize * row;
            }
        }

        for (int column = 0; column <= m_Grid.Columns(); ++column)
        {
            if (i_PluggableRect.x > i_GridRect.x + m_CellSize * (column - 0.5f) && i_PluggableRect.x < i_GridRect.x + m_CellSize * (column + 0.5f))
            {
                i_PluggableRect.x = i_GridRect.x + m_CellSize * column;
            }
        }

        return i_PluggableRect;
    }

    private bool IsInside(Rect i_Out, Rect i_In)
    {
        if (i_Out.x - m_CellSize/100.0f <= i_In.x &&
           i_Out.y + m_CellSize / 100.0f >= i_In.y &&
           i_Out.x + i_Out.width + m_CellSize / 100.0f >= i_In.x + i_In.width && 
           i_Out.y  - i_Out.height - m_CellSize / 100.0f <= i_In.y - i_In.height )
        {
            return true;
        }
        return false;
    }

    private Vector2 GetTopLeftInGrid(Rect i_GridRect, Rect i_PluggableRect)
    {
        Vector2 topLeft = new Vector2();

        for (int row = 0; row <= m_Grid.Rows(); ++row)
        {
            if (i_PluggableRect.y <= i_GridRect.y - m_CellSize * (row -  m_CellSize/100.0f ) && i_PluggableRect.y >= i_GridRect.y - m_CellSize * (row +  m_CellSize/100.0f ))
            {
                topLeft.x = row;
            }
        }

        for (int column = 0; column <= m_Grid.Columns(); ++column)
        {
            if (i_PluggableRect.x >= i_GridRect.x + m_CellSize * (column -  m_CellSize/100.0f ) && i_PluggableRect.x <= i_GridRect.x + m_CellSize * (column +  m_CellSize/100.0f ))
            {
                topLeft.y = column;
            }
        }

        return topLeft;

    }

    private bool IsGridFree(Vector2 i_TopLeftInGrid, F_Matrix i_PluggableMatrix)
    {
        bool isFree = true;

        for (int row = 0; row < i_PluggableMatrix.Rows(); ++row)
        {
            for (int column = 0; column < i_PluggableMatrix.Columns(); ++column)
            {
                if (i_PluggableMatrix.Get(row, column)>0 && m_Grid.Get(row + (int)i_TopLeftInGrid.x, column + (int)i_TopLeftInGrid.y) != 1)
                {
                    isFree = false;
                    break;
                }
            }

        }

        return isFree;
    }

    private void FillGrid(Vector2 i_TopLeftInGrid, F_Matrix i_PluggableMatrix)
    {
        for (int row = 0; row < i_PluggableMatrix.Rows(); ++row)
        {
            for (int column = 0; column < i_PluggableMatrix.Columns(); ++column)
            {
                if (i_PluggableMatrix.Get(row, column) > 0 && m_Grid.Get(row + (int)i_TopLeftInGrid.x, column + (int)i_TopLeftInGrid.y) == 1)
                {
                    m_Grid.Set(row + (int)i_TopLeftInGrid.x, column + (int)i_TopLeftInGrid.y, 2);
                }
            }

        }
    }

    private void FreeGrid(Vector2 i_TopLeftInGrid, F_Matrix i_PluggableMatrix)
    {
        for (int row = 0; row < i_PluggableMatrix.Rows(); ++row)
        {
            for (int column = 0; column < i_PluggableMatrix.Columns(); ++column)
            {
                if (i_PluggableMatrix.Get(row, column) > 0 && m_Grid.Get(row + (int)i_TopLeftInGrid.x, column + (int)i_TopLeftInGrid.y) == 2)
                {
                    m_Grid.Set(row + (int)i_TopLeftInGrid.x, column + (int)i_TopLeftInGrid.y, 1);
                }
            }

        }
    }

    public void Initialize(F_Matrix i_Grid, float i_CellSize, LineRenderer i_PrefabGridLine, SpriteRenderer i_PrefabCellBackground, F_GrabManager i_GrabManager)
    {
        m_Grid = i_Grid;
        m_CellSize = i_CellSize;
        m_PrefabGridLine = i_PrefabGridLine;
        m_Docked = 0;
        m_PrefabCellBackground = i_PrefabCellBackground;
        m_GrabManager = i_GrabManager;

        CreateGrid();
    }

    public void RemoveFromGrid(F_Pluggable i_ToRemove)
    {
        if (!m_GrabManager.IsGrabbing() || i_ToRemove == m_GrabManager.GetGrabbed())
        {
            i_ToRemove.OnClickEvent -= RemoveFromGrid;
            --m_Docked;
            Rect gridRect = GetRect();
            Rect pluggableRect = i_ToRemove.GetRect();
            Vector2 topLeftInGrid = GetTopLeftInGrid(gridRect, pluggableRect);
            FreeGrid(topLeftInGrid, i_ToRemove.GetGrid());
        }

    }

    public Rect GetRect()
    {
        Rect objectRect = new Rect(m_TopLeft.x, m_TopLeft.y, m_Grid.Columns() * m_CellSize, m_Grid.Rows() * m_CellSize);

        return objectRect;

    }

    public int GetDocked()
    {
        return m_Docked;
    }

    private void CreateGrid()
    {
        CalculateTopLeft();

        for (int row = 0; row < m_Grid.Rows(); ++row)
        {
            for (int column = 0; column < m_Grid.Columns(); ++column)
            {
                if(m_Grid.Get(row,column) == 1)
                {
                    CreateCell(row, column);
                }
            }

        }

    }

    private void CalculateTopLeft()
    {
        float screenHeight = Camera.main.orthographicSize * 2.0f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        float gridHeight = m_Grid.Rows() * m_CellSize;
        float gridWidth = m_Grid.Columns() * m_CellSize;

        m_TopLeft = new Vector2(((screenWidth/2.0f)-gridWidth)/2.0f - screenWidth/2.0f, screenHeight / 2.0f - (screenHeight - gridHeight) / 2.0f);
    }

    private void CreateCell(int row, int column)
    {

        GameObject background = Instantiate(m_PrefabCellBackground.gameObject, Vector3.zero, Quaternion.identity, this.gameObject.transform);
        background.transform.localScale = new Vector3(m_CellSize, m_CellSize, m_CellSize);
        background.transform.position = new Vector3(m_TopLeft.x + m_CellSize * column + m_CellSize/2.0f, m_TopLeft.y - m_CellSize * row - m_CellSize / 2.0f, 3);

        GameObject line = Instantiate(m_PrefabGridLine.gameObject, Vector3.zero, Quaternion.identity, this.gameObject.transform);

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 6;
        lineRenderer.SetPosition(0, new Vector3(m_TopLeft.x + m_CellSize * column, m_TopLeft.y - m_CellSize * row, 2));
        lineRenderer.SetPosition(1, new Vector3(m_TopLeft.x + m_CellSize * (column + 1), m_TopLeft.y - m_CellSize * row, 2));
        lineRenderer.SetPosition(2, new Vector3(m_TopLeft.x + m_CellSize * (column + 1), m_TopLeft.y - m_CellSize * (row + 1), 2));
        lineRenderer.SetPosition(3, new Vector3(m_TopLeft.x + m_CellSize * column, m_TopLeft.y - m_CellSize * (row + 1), 2));
        lineRenderer.SetPosition(4, new Vector3(m_TopLeft.x + m_CellSize * column, m_TopLeft.y - m_CellSize * row, 2));

        //L'ultimo serve per evitare che ci siano pixel vuoti nell'angolo dove si fa il loop
        lineRenderer.SetPosition(5, new Vector3(m_TopLeft.x + m_CellSize * (column + 1), m_TopLeft.y - m_CellSize * row, 2));
    }
}
