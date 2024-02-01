using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MovingWall
{
    void Start()
    {
        verticalMin = 46;
        verticalMax = 50;
        horizontalMin = -33.44f;
        horizontalMax = -33.44f;
        speed = 1;
    }

    
}
