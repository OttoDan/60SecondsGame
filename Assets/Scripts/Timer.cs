using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public Text TimeText;
    public float time = 60;
    public bool OnGame = true;
    public Text HighScore;
    public int highscore;

    public GameObject ScoreMenuPrefab;
    

    void Start()
    {
        OnGame = true;

        
    }

    void Update()
    {

        time -= Time.deltaTime;

        
        
        if (Input.GetButton("Submit"))
        {
            highscore += 1;
        }
        

        if (OnGame = true)
        {
            TimeText.text = ("" + time);
            HighScore.text = ("HighScore " + highscore);
        }

        if (time < 0)
        {
            OnGame = false;
            
        }

        if (!OnGame)
        {
            ScoreMenuPrefab.SetActive(true);
        }
        else
        {
            ScoreMenuPrefab.SetActive(false);
        }

    }
}
