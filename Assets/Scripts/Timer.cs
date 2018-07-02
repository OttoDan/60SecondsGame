using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {


    public static Timer instanceTimer;
    public Text TimeText;
    public Text ScoreText;
    public float time = 60;
    public bool OnGame = true;
    public float score;
    public GameObject ScoreMenuPrefab;
    
    private void Awake()
    {
        if (instanceTimer == null)
            instanceTimer = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        OnGame = true;

        
    }

    void Update()
    {
        if (time > -1)
        {
            time -= Time.deltaTime;
        }
        
        
        if (Input.GetButton("Submit"))
        {
            score += 1;
        }


        if (OnGame == true)
        {
            TimeText.text = ("" + time);
            ScoreText.text = ("HighScore " + score);
            ScoreMenuPrefab.SetActive(false);
        }
        
        if (OnGame == false)
        {
            ScoreMenuPrefab.SetActive(true);
        }
        

        if (time < 0)
        {
            OnGame = false;
            time = -1;

        }
    }
}
