using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManger : MonoBehaviour
{
    public static GameManger Instance;
    [SerializeField] Texture2D[] cursorTextures;
    public static Button startButton;
    Vector2 cursorHotspot;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            //startButton = FindAnyObjectByType<Button>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < cursorTextures.Length; i++)
        {
            if (arg0.name == cursorTextures[i].name)
            {
                SetCursor(cursorTextures[i]);
                Debug.Log(cursorTextures[i]);
            }
        }
    }
    public void SetCursor(Texture2D cursor)
    {
        if(cursor == cursorTextures[0])
        {
            cursorHotspot = new Vector2(0, 1);
        }
        else
        {
            cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);
        }
        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
    }

    public static void OnclickStart()
    {
        SceneManager.LoadScene("Scene_01");
    }
    public static void OnClickQuit()
    {
        Application.Quit();
    }
}
