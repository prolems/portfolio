using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube1 : MovingWall
{
    void Start()
    {
        verticalMin = 46;
        verticalMax = 50;
        horizontalMin = -35.44f;
        horizontalMax = -35.44f;
        speed = 1;
    }

}
