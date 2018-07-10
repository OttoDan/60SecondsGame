using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class AudioManager : MonoBehaviour {


    #region Public Fields

    public static AudioManager Instance;

    public float volume = 1;


    public AudioClip CubeRotation;

    public AudioClip EnemyHit;
    public AudioClip MainCharacterDash;
    public AudioClip DashDrawingStart; 

    public AudioClip NumbersRise; //Score +1 

    public AudioClip MenuHover;
    public AudioClip MenuSelection;

    [Range(0,255)]
    public int timerPriority = 0;
    public AudioClip Timer_Ticking1;
    public AudioClip Timer_Ticking2;
    public AudioClip Timer_Ticking3;
    public AudioClip Timer_Ticking4;


    public AudioClip SongMainTheme;

    #endregion

    #region Private Fields

    //private AudioSource TimerAS;

    private Queue<AudioSource> EnemyHitAudios;
    private AudioSource TimerAudio;
    private AudioSource SongAudio;

    private int enemyHitAudioSourceCount = 8;

    private IEnumerator TimerCoroutine;

    #endregion

    #region Unity Messages

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //Timer
        TimerAudio = gameObject.AddComponent<AudioSource>();
        TimerAudio.volume = 0.25f;
        TimerAudio.priority = timerPriority;

        //Song
        SongAudio = gameObject.AddComponent<AudioSource>();

        //Enemy Hit
        EnemyHitAudios = new Queue<AudioSource>();

        for (int i = 0; i < enemyHitAudioSourceCount; i++)

        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = EnemyHit;

            EnemyHitAudios.Enqueue(audioSource);
        }


    }

    #endregion

    #region Methods

    public void EnemyHitAudio()
    {
        AudioSource audioSource = EnemyHitAudios.Dequeue();
        if (audioSource != null)
        {
            audioSource.Play();
            EnemyHitAudios.Enqueue(audioSource);
        }
    }

    public void TimerStartAudio()
    {
        if (TimerCoroutine != null)
            StopCoroutine(TimerCoroutine);

        TimerCoroutine = TimerRoutine();
        StartCoroutine(TimerCoroutine);        
    }


    public void SetSongAudio(AudioClip audioClip)
    {
        SongAudio.clip = audioClip;
    }

    public void SongStartAudio()
    {
        if (SongAudio != null)
            SongAudio.Play();
    }
    #endregion

    #region Coroutines
    
    IEnumerator TimerRoutine()
    {
        TimerAudio.loop = true;
        TimerAudio.clip = Timer_Ticking1;
        TimerAudio.Play();

        while (GameManager.Instance.seconds > 30)
            yield return null;

        TimerAudio.clip = Timer_Ticking2;
        TimerAudio.Play();

        while (GameManager.Instance.seconds > 40)
            yield return null;

        TimerAudio.clip = Timer_Ticking3;
        TimerAudio.Play();

        while (GameManager.Instance.seconds > 1)
            yield return null;

        TimerAudio.loop = false;
        TimerAudio.clip = Timer_Ticking4;
        TimerAudio.Play();


        TimerCoroutine = null;
    }

    #endregion

}
