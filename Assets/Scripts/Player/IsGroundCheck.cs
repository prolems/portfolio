using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGroundCheck : MonoBehaviour,IDamageable
{
    [SerializeField] Movement movement;
    [SerializeField] Stats stats;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IsGround"))
        {
            movement.IsGround = true;
            movement.JumpCount = 1;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("IsGround"))
        {
            movement.IsGround = true;
            movement.JumpCount = 1;
        }
    }
  private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IsGround"))
        {
            if (!movement.isJumping)
            {
                movement.JumpCount = 2;
            }
            movement.IsGround = false;
            
        }
    }
    public void Damage(float dmg)
    {
        stats.Damage(dmg);
    }
}
