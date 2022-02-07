using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManger : MonoBehaviour
{
    public static MusicManger instance;
    [Header("Music Controller")]
    [SerializeField]
    private AudioSource backgroundMusic;
    [SerializeField]
    private AudioSource damageMusic;

    void Awake()
    {
        if (instance == null) { instance = this; }
    }
    private void Start()
    {
        if (backgroundMusic.clip != null)
        {
            backgroundMusic.Play();
        }
    }
    public void PlayDamageMusic()
    {
        if (!damageMusic.isPlaying && damageMusic.clip != null) damageMusic.Play();
    }
}
