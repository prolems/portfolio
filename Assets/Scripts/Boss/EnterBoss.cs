using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBoss : MonoBehaviour
{
    [SerializeField] GameObject bossPanel;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject player;
    public void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            Debug.Log("������ ����");
            bossPanel.SetActive(true);
            wall.SetActive(true);
        }
    }
}
