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
            //�Ӹ� 0.1���� �ٵ� 1���� �ٸ� 0.5���� �������� 2����
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
