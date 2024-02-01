using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDmg : MonoBehaviour , IDamageable
{
   public enum DmgPart{ head, body, legs, tail}
    [SerializeField] DmgPart part;
    [SerializeField] Boss boss;

    public void Damage(float dmg)
    {
        switch (part)
        {
            //머리 0.1배율 바디 1배율 다리 0.5배율 꼬리약점 2배율
            case DmgPart.head:
                dmg /= 10;
                break;
            case DmgPart.legs:
                dmg /= 2;
                break;
            case DmgPart.tail:  
                dmg *= 2;
                break;
        }
        boss.HP -= dmg;
        if(boss.HP <= 0 ) 
        {
            boss.Dead();
        }
        Debug.Log($"{boss.HP}/{boss.maxHp}");
    }
}
