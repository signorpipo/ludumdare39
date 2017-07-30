using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ClickEvent(F_Pluggable i_Clicked);
public class F_Pluggable : MonoBehaviour
{

    [System.Serializable]
    public struct IntArray
    {
        public int[] m_Array;
    }


    public ClickEvent OnClickEvent;

    public IntArray[] m_MatrixGrid;
    public int m_CellSize;

    private F_Direction m_Direction;
    private F_Matrix m_Grid;
    private Vector3 m_InitialPosition;

    public void Start()
    {
        m_Direction = F_Direction.UP;
        m_InitialPosition = gameObject.transform.position;
        m_Grid = new F_Matrix(m_MatrixGrid.GetLength(0), m_MatrixGrid[0].m_Array.GetLength(0));
        for (int row = 0; row < m_Grid.Rows(); ++row)
        {
            for (int column = 0; column < m_Grid.Columns(); ++column)
            {
                m_Grid.Set(row, column, (m_MatrixGrid[row].m_Array[column] >= 1) ? 1 : 0);
            }

        }
    }

    public void Turn(F_Direction i_Direction)
    {

        m_Direction = F_DirectionExtensions.Turn(m_Direction, i_Direction);

        gameObject.transform.rotation = Quaternion.identity;
        switch (m_Direction)
        {
            case F_Direction.RIGHT:
                gameObject.transform.Rotate(new Vector3(0, 0, -90));
                break;
            case F_Direction.LEFT:
                gameObject.transform.Rotate(new Vector3(0, 0, 90));
                break;
            case F_Direction.DOWN:
                gameObject.transform.Rotate(new Vector3(0, 0, 180));
                break;
        }

    }

    public void Dock(Vector2 i_TopLeft)
    {
        float xOffset = (m_Grid.Columns() * m_CellSize) / 2.0f;
        float yOffset = (m_Grid.Rows() * m_CellSize) / 2.0f;

        Vector3 newPosition = new Vector3(i_TopLeft.x, i_TopLeft.y, 1);

        if (m_Direction == F_Direction.UP || m_Direction == F_Direction.DOWN)
        {
            newPosition.x += xOffset;
            newPosition.y -= yOffset;
        }
        else
        {
            newPosition.x += yOffset;
            newPosition.y -= xOffset;
        }

        gameObject.transform.position = newPosition;
    }

    public void GoToInitial()
    {
        gameObject.transform.position = m_InitialPosition;
        gameObject.transform.rotation = Quaternion.identity;
        m_Direction = F_Direction.UP;

    }

    public Rect GetRect()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        float xOffset = (m_Grid.Columns() * m_CellSize) / 2.0f;
        float yOffset = (m_Grid.Rows() * m_CellSize) / 2.0f;

        Rect objectRect = new Rect();

        if (m_Direction == F_Direction.UP || m_Direction == F_Direction.DOWN)
        {
            position.x -= xOffset;
            position.y += yOffset;

            objectRect.width = m_Grid.Columns() * m_CellSize;
            objectRect.height = m_Grid.Rows() * m_CellSize;
        }
        else
        {
            position.x -= yOffset;
            position.y += xOffset;

            objectRect.height = m_Grid.Columns() * m_CellSize;
            objectRect.width = m_Grid.Rows() * m_CellSize;
        }

        objectRect.x = position.x;
        objectRect.y = position.y;

        return objectRect;
    }

    public F_Matrix GetGrid()
    {
        return m_Grid.GetRotated(m_Direction);
    }

    private void OnMouseDown()
    {
        if(OnClickEvent != null)
        {
            OnClickEvent(this);
        }
    }

}
