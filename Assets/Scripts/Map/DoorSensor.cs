using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    [SerializeField] Door door;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            //¿­¸²
            door.IsOpen = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player")
        {
            //´ÝÈû
            door.IsOpen = 2;
        }
    }
}
