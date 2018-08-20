using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WIPSwitchScene : MonoBehaviour {
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void SwitchToLevel()
    {
        SceneManager.LoadScene(1);
    }
    void PlayIntroSound()
    {
        audioSource.Play();
    }
}
