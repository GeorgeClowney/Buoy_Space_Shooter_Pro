using UnityEngine.Audio;
using UnityEngine;
//The Sound class contains basic info for Audio Clips
//is used in the AudioManager script
//Based on a tutorial video by Brackeys
//Link: https://bit.ly/2GULEkd
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
