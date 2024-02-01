using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGun : Item
{
    public int GunIndex { get; set; }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Stats>() == stats)
        {
            other.GetComponent<Shooting>().Havegun[GunIndex] = true;
            Destroy(gameObject);
        }
    }
}

