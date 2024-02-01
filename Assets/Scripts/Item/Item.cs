using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected Stats stats;

    public abstract void OnTriggerEnter(Collider other);
    private void Update()
    {
        transform.Rotate(Vector3.up*Time.deltaTime*30f);
    }
}
