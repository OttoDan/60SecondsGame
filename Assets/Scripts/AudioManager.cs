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
    public AudioClip MenuHover;
    public AudioClip MenuSelection;
    public AudioClip Timer_Ticking1;
    public AudioClip Timer_Ticking2;
    public AudioClip Timer_Ticking3;
    public AudioClip Timer_Ticking4;

    #endregion

    #region Private Fields

    //private AudioSource TimerAS;

    private Queue<AudioSource> EnemyHitAudios;

    private int enemyHitAudioSourceCount = 8;

    #endregion

    #region Unity Messages

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        for(int i = 0; i < enemyHitAudioSourceCount; i++)

        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            //audioSource.clip = EnemyHit;
            
            //EnemyHitAudios.Enqueue(audioSource);
        }
    }

    #endregion

    #region Methods

    public void EnemyHitAudio()
    {
        //AudioSource audioSource = EnemyHitAudios.Dequeue();
        //if(audioSource != null)
        //{
        //    audioSource.Play();
        //    EnemyHitAudios.Enqueue(audioSource);

        //}
    }

    #endregion

}
