using UnityEngine.Audio;
using System;
using UnityEngine;
//AudioManager is a Global script that contorls all audio in the game
//Based on a tutorial video by Brackeys
//Link: https://bit.ly/2GULEkd
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager _instance;

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
        //Sound is a custiom class that stores the following information
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        
        }
    }
    private void Start()
    //Start playing the background music on loop once the game is started
    {
        Play("Background");
    }
    //Public fuction for playing audio clips
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null) { 
            //Alert Unity if audio clip is not found
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
    }
        s.source.Play();
    }
    private void Update()
    {
        RestartGame();
    }
    //If the game is restarted then Destory the current AudioManager
    private void RestartGame()
    {
        GameManager gm = gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (gm == null)
        {
            Debug.LogError("Game Manager is Null (AudioManager)");
        }
        else if (gm != null && gm.gameRestarted)
        {
            Destroy(this.gameObject);
        }
    }
}
