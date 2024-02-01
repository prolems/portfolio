using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        //게임실행해도 메인로비로 넘어감
        if (GameManger.Instance == null)
        {
            SceneManager.LoadScene("Lobby");
        }
        //GameManager.instance.CreatePlayer();
        //UI.Instance.OnGameStart();
    }
}
