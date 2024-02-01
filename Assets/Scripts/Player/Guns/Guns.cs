using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Transform[] firePos;
    [SerializeField] protected Shooting shooting;
    [SerializeField] protected Stats stats;

    public float Damage_0 { get; set; }
    public float Damage_1 { get; set; }
   // public int Ammo { get; set; }
    //public int OriginAmmo { get; set; }
   // public int MaxAmmo { get; set; }
    //public int AmmoCost_0 { get; set; }
    //public int AmmoCost_1 { get; set; }
    public float ShootDelay_0 { get; set; }
    public float ShootDelay_1 { get; set; }
    public float ReloadSpeed { get; set; }
    public float Distance { get; set; }

}
