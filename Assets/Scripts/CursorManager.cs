using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D[] cursorTextures;
    public static CursorManager instance;
    Vector2 cursorHotspot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        cursorHotspot = new Vector2(cursorTextures[0].width / 2, cursorTextures[0].height / 2);
        Cursor.SetCursor(cursorTextures[0], cursorHotspot, CursorMode.Auto);
    }
}
