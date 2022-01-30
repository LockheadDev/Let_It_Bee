using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("Audio Controller")]
    [SerializeField]
    private GameObject PR_audioSource;
    [SerializeField]
    private int sourceQuantity = 5;
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip AC_click;
    [SerializeField]
    private AudioClip AC_background;
    [SerializeField]
    private AudioClip AC_backgroundHard;
    [SerializeField]
    private AudioClip AC_beeReachDestination;
    [SerializeField]
    private AudioClip AC_beeReachFlower;
    [SerializeField]
    private AudioClip AC_beeCrash;
    [SerializeField]
    private AudioClip AC_gameOver;

    [Header("Music Channel")]
    [SerializeField]
    private AudioSource CH_background2;

    [Header("UI Channel")]
    [SerializeField]
    private AudioSource CH_background3;


    [Header("Audio Source Pool")]
    public List<AudioSource> audioSources = new List<AudioSource>();

    void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < sourceQuantity; i++)
        {
            GameObject temp_go = Instantiate(PR_audioSource);
            temp_go.transform.parent = transform;
            audioSources.Add(temp_go.GetComponent<AudioSource>());
        }

    }
    public void PlayCLick()
    {
        PlayClipAdv(CH_background3, false, AudioClp.click);
    }
    public bool GetPlayingBackgroundHard()
    {
        if (CH_background2.isPlaying) return true;
        return false;
    }
    private void PlayClipAdv(AudioSource audsrc, bool isLoop,AudioClp clp)
    {
        AudioClip audio = GetAudio(clp);
        audsrc.clip = audio;
        audsrc.loop = isLoop;
        audsrc.Play();

    }
    public void PlayAudio(AudioClp clpEnum)
    {
        switch (clpEnum)
        {
            case AudioClp.crash:
                PlayClip(AC_beeCrash);
                break;
            case AudioClp.destination:
                PlayClip(AC_beeReachDestination);
                break;
            case AudioClp.gameOver:
                PlayClip(AC_gameOver);
                break;
            case AudioClp.background:
                PlayClip(AC_background);
                break;
            case AudioClp.backgroundHard:
                PlayClipAdv(CH_background2,false, AudioClp.backgroundHard) ;
                break;
            case AudioClp.pickFlower:
                PlayClip(AC_beeReachFlower);
                break;
            case AudioClp.click:
                PlayClip(AC_click);
                break;
            default:
                break;
        }
    }
    private AudioClip GetAudio(AudioClp clpEnum)
    {
        switch (clpEnum)
        {
            case AudioClp.crash:
                return AC_beeCrash;
            case AudioClp.destination:
                return AC_beeReachDestination;
            case AudioClp.gameOver:
                return AC_gameOver;
            case AudioClp.background:
                return AC_background;
            case AudioClp.backgroundHard:
                return AC_backgroundHard;
            case AudioClp.pickFlower:
                return AC_beeReachFlower;
            case AudioClp.click:
                return AC_click;

        }
        return AC_gameOver;
    }
    private void PlayClip(AudioClip ac)
    {
       
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.clip = ac;
        audioSource.Play();
        
    }
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource item in audioSources)
        {
            if (!item.isPlaying) return item;
        }
        return null;
    }
   
}
