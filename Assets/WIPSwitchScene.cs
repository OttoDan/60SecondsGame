using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WIPSwitchScene : MonoBehaviour {
    AudioSource audioSource;
    public AudioClip titleScreen;
    Gyroscope gyroscope;
    public Skybox skyBox;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
    }
    void LateUpdate()
    {
        if(gyroscope.enabled)
        {

            //transform.Rotate(gyroscope.rotationRate, gyroscope.updateInterval * Time.unscaledDeltaTime);
            transform.Rotate(gyroscope.rotationRate * 16* Time.unscaledDeltaTime);
            //skyBox.transform.Rotate(gyroscope.rotationRate * 16* Time.unscaledDeltaTime);
            RenderSettings.skybox.SetFloat("_Rotation", gyroscope.rotationRate.magnitude * 360 * Time.unscaledDeltaTime); 
        }
    }
    void SwitchToLevel()
    {
        SceneManager.LoadScene(1);
    }
    void PlayIntroSound()
    {
        audioSource.Play();
    }
    void PlayTitleScreenTheme()
    {
        audioSource.clip = titleScreen;
        audioSource.Play();
    }

    void ActivateCharPoseClip()
    {
        Animator animator = transform.Find("Char").GetComponentInChildren<Animator>();
        animator.SetBool("LogoPose",true);
        //animator.Play(
        //    "Intro_CharLogoPose");
    }
}
