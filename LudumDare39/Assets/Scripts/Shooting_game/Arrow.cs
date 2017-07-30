using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    private Vector3 m_rotation = new Vector3(0,0, 0);
    public Vector3 unitRotation = new Vector3(0, 0, 1);
    private bool invert = false;
    public void UpdateDirection()
    {
        if (transform.rotation.eulerAngles.z >= 90)
        {
            invert = true;
        } else if (transform.rotation.eulerAngles.z <= 1)
        {
            invert = false;
        }

        if (invert)
        {
            transform.Rotate(-unitRotation);
        } else
        {
            transform.Rotate(unitRotation);
        }
        
    }

    public Vector2 GetCurrentDirection()
    {
        return transform.right;
    
    }
}
