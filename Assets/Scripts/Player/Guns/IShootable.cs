using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    void Shoot_0();
    void Shoot_1();
    IEnumerator Reload();
}
