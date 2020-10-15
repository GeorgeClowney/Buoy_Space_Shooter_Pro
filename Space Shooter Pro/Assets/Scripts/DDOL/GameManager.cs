using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//GameManager is a global script that controls Scene Management and turning off the game 
public class GameManager : MonoBehaviour
{
    [SerializeField]
    public bool isGameOver;
    public bool gameRestarted;
    private GameManager _instance;

    //Make the GameManager a Singleton
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
    private void Awake()
    {
        MakeSingleton();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isGameOver)
        {
            RestartGame();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }  
    private void RestartGame()
    {
        gameRestarted = true;
        SceneManager.LoadScene(0);
        Destroy(gameObject);

    }
    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("quiting game...");
    } 
}
