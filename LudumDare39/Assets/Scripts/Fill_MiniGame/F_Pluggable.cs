using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ClickEvent(F_Pluggable i_Clicked);

public class F_Pluggable : MonoBehaviour {

    public ClickEvent OnClickEvent;

    private Vector3 m_InitialPosition;
    private F_Matrix m_Grid;
    private int m_CellSize;
    private F_Direction m_Direction;

    public void Start()
    {
        m_InitialPosition = gameObject.transform.position;
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
        int xOffset = (m_Grid.Columns() * m_CellSize) / 2;
        int yOffset = (m_Grid.Rows() * m_CellSize) / 2;

        Vector3 newPosition = new Vector3(i_TopLeft.x, i_TopLeft.y, 0);

        if (m_Direction == F_Direction.UP || m_Direction == F_Direction.DOWN)
        {
            newPosition.x += xOffset;
            newPosition.y += yOffset;
        }
        else
        {
            newPosition.x += yOffset;
            newPosition.y += xOffset;
        }

        gameObject.transform.position = newPosition;
    }

    public void GoToInitial()
    {
        gameObject.transform.position = m_InitialPosition;
        gameObject.transform.rotation = Quaternion.identity;
        m_Direction = F_Direction.UP;

    }

    public Vector2 GetTopLeft()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        int xOffset = (m_Grid.Columns() * m_CellSize) / 2;
        int yOffset = (m_Grid.Rows() * m_CellSize) / 2;

        if (m_Direction == F_Direction.UP || m_Direction == F_Direction.DOWN)
        {
            position.x -= xOffset;
            position.y -= yOffset;
        }
        else
        {
            position.x -= yOffset;
            position.y -= xOffset;
        }

        return position;
    }

    public F_Matrix GetGrid()
    {
        return m_Grid.GetRotated(m_Direction);
    }

    public void initialize(Vector3 i_Position, F_Matrix i_Grid, int i_CellSize, Sprite i_Sprite)
    {
        m_InitialPosition = i_Position;
        m_Grid = i_Grid;
        m_CellSize = i_CellSize;
        m_Direction = F_Direction.UP;

        SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = i_Sprite;
    }

    private void OnMouseDown()
    {
        if(OnClickEvent != null)
        {
            OnClickEvent(this);
        }
    }

}
