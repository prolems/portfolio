using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        //���ӽ����ص� ���ηκ�� �Ѿ
        if (GameManger.Instance == null)
        {
            SceneManager.LoadScene("Lobby");
        }
        //GameManager.instance.CreatePlayer();
        //UI.Instance.OnGameStart();
    }
}
