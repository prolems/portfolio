using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DmgMap : MonoBehaviour
{
    protected float dmg;
    bool isAttackable = true;
    protected void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null && isAttackable)
        {
            damageable.Damage(dmg);
            StartCoroutine(AttackTimer());
        }
    }
    float attackTime = 1f;
    protected void OnTriggerStay(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && isAttackable)
        {
            damageable.Damage(dmg);
            StartCoroutine(AttackTimer());
        }
    }
    IEnumerator AttackTimer()
    {
        isAttackable = false;
        yield return new WaitForSeconds(attackTime);
        isAttackable = true;
    }

}
