using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Test : MonoBehaviour {
    
	void Start () {
        //TestMatrix();
    }

    private void TestMatrix()
    {
        F_Matrix matrix = new F_Matrix(5, 3);
        for(int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                matrix.Set(i, j, i + j);
            }

        }

        matrix.debugDisplay("Plain");
        F_Matrix rotated = matrix.GetRotated(F_Direction.UP);
        rotated.debugDisplay("UP");
        rotated = matrix.GetRotated(F_Direction.RIGHT);
        rotated.debugDisplay("RIGHT");
        rotated = matrix.GetRotated(F_Direction.DOWN);
        rotated.debugDisplay("DOWN");
        rotated = matrix.GetRotated(F_Direction.LEFT);
        rotated.debugDisplay("LEFT");
    }
}
