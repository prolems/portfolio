using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour,IDamageable
{
    [SerializeField] ParticleSystem fxBreak;
    [SerializeField] MeshRenderer mesh;
    float wallHp = 400;
    public void Damage(float dmg)
    {
        if (wallHp > 0)
        {
            wallHp -= dmg;
            if (wallHp <= 0)
            {
                wallHp = 0;
                // break method
                Break();
            }
        }
    }
    void Break()
    {
        fxBreak.Play();
        mesh.enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
