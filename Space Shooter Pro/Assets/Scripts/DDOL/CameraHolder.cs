using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//The CameraHolder script Makes the game's camera a Singleton
//CameraHolder is a empty gameobject that holds the Camera
//Based on a tutorial video by Brackeys
//Link: https://bit.ly/37c5j9Y
public class CameraHolder : MonoBehaviour
{
    public static CameraHolder _instance;

    void MakeSingleton()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Awake()
    {
        MakeSingleton();
    }
    private void Update()
    {
        RestartGame();
    }
    //If the game is restarted then Destory the current CameraHolder
    private void RestartGame()
    {
        GameManager gm = gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (gm == null)
        {
            Debug.LogError("Game Manager is Null (CameraHolder)");
        }
        else if (gm != null && gm.gameRestarted)
        {
            Destroy(this.gameObject);
        }
    }
}
