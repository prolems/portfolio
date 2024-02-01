using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX_Jump : MonoBehaviour
{
    float endTimer=1;
    float curTime;
    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > endTimer)
        {
            FxJumpEnd();
            curTime = 0;
        }
    }
    public void FxJumpEnd()
    {
        FX_Pool.instance.FxJumpEnd(this);
    }
}
