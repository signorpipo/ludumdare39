using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_GridManager : MonoBehaviour {

    public LineRenderer m_PrefabGridLine;
    public Vector2 m_TopLeft;
    public float m_CellSize;

    private F_Matrix m_Grid;

	void Start () {

        m_Grid = new F_Matrix(4, 4);
        m_Grid.Set(0, 0, 1);
        m_Grid.Set(1, 0, 0);
        m_Grid.Set(2, 0, 1);
        m_Grid.Set(3, 0, 1);
        m_Grid.Set(0, 1, 1);
        m_Grid.Set(1, 1, 1);
        m_Grid.Set(2, 1, 1);
        m_Grid.Set(3, 1, 0);
        m_Grid.Set(0, 2, 0);
        m_Grid.Set(1, 2, 1);
        m_Grid.Set(2, 2, 1);
        m_Grid.Set(3, 2, 0);
        m_Grid.Set(0, 3, 0);
        m_Grid.Set(1, 3, 1);
        m_Grid.Set(2, 3, 1);
        m_Grid.Set(3, 3, 1);
        CreateGrid();


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
        GameObject line = Instantiate(m_PrefabGridLine.gameObject, Vector3.zero, Quaternion.identity, this.gameObject.transform);

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
        lineRenderer.SetPosition(0, new Vector3(m_TopLeft.x + m_CellSize * column, m_TopLeft.y - m_CellSize * row, 2));
        lineRenderer.SetPosition(1, new Vector3(m_TopLeft.x + m_CellSize * (column + 1), m_TopLeft.y - m_CellSize * row, 2));
        lineRenderer.SetPosition(2, new Vector3(m_TopLeft.x + m_CellSize * (column + 1), m_TopLeft.y - m_CellSize * (row + 1), 2));
        lineRenderer.SetPosition(3, new Vector3(m_TopLeft.x + m_CellSize * column, m_TopLeft.y - m_CellSize * (row + 1), 2));
        lineRenderer.SetPosition(4, new Vector3(m_TopLeft.x + m_CellSize * column, m_TopLeft.y - m_CellSize * row + 0.035f, 2));
    }

    void Update () {
		
	}
}
