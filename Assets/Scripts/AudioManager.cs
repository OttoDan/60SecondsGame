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
    public AudioClip ObstacleHit;
    public AudioClip MainCharacterDash;
    public AudioClip DrawDashStart; 

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

    private AudioSource TimerAudio;
    private AudioSource SongAudio;

    private AudioSource DrawDashStartAudio;
    private AudioSource MainCharacterDashAudio;

    private int enemyHitAudioSourceCount = 8;
    private Queue<AudioSource> EnemyHitAudios;
    private int NumbersRiseAudiosCount = 2;
    private Queue<AudioSource> NumbersRiseAudios;

    private AudioSource obstacleHitAudio;


    private float enemyHitPitch = 1;

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

        //DrawDashStartAudio
        DrawDashStartAudio = gameObject.AddComponent<AudioSource>();
        DrawDashStartAudio.clip = DrawDashStart;

        //MainCharacterDash
        MainCharacterDashAudio = gameObject.AddComponent<AudioSource>();
        MainCharacterDashAudio.clip = MainCharacterDash;

        //Enemy Hit
        EnemyHitAudios = new Queue<AudioSource>();

        for (int i = 0; i < enemyHitAudioSourceCount; i++)

        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = EnemyHit;

            EnemyHitAudios.Enqueue(audioSource);
        }

        //Obstacle Hit
        obstacleHitAudio = gameObject.AddComponent<AudioSource>();
        obstacleHitAudio.clip = ObstacleHit;


        //Numbers Rise
        NumbersRiseAudios = new Queue<AudioSource>();
        for (int i = 0; i < NumbersRiseAudiosCount; i++)

        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = NumbersRise;

            NumbersRiseAudios.Enqueue(audioSource);
        }

    }

    #endregion

    #region Methods

    

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

    public void DrawDashAudio()
    {
        if (DrawDashStartAudio != null)
            DrawDashStartAudio.Play();
    }

    public void DashAudio()
    {
        if (MainCharacterDashAudio != null)
            MainCharacterDashAudio.Play();
    }
    public void EnemyHitResetAudio()
    {
        enemyHitPitch = 1;
    }
    public void EnemyHitAudio()
    {
        AudioSource audioSource = EnemyHitAudios.Dequeue();
        enemyHitPitch += 0.125f;
        if (enemyHitPitch >= 2)
            EnemyHitResetAudio();

        if (audioSource != null)
        {
            audioSource.pitch = enemyHitPitch;// Mathf.Lerp(-1,3,PlayerController.Instance.enemyHitsDuringDash / 10) ;
            audioSource.Play();
            EnemyHitAudios.Enqueue(audioSource);
        }
    }

    public void ObstacleHitAudio()
    {
        if (obstacleHitAudio != null)
            obstacleHitAudio.Play();
    }

    public void NumbersRiseAudio()
    {
        AudioSource audioSource = NumbersRiseAudios.Dequeue();
        if (audioSource != null)
        {
            audioSource.Play();
            NumbersRiseAudios.Enqueue(audioSource);
        }
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
