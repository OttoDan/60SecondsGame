using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {


    public static Timer instance;
    public Text TimeText;
    public float time = 60;
    public bool OnGame = true;
    public Text Score;
    public float score;

    public GameObject ScoreMenuPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        OnGame = true;

        
    }

    void Update()
    {

        time -= Time.deltaTime;

        
        
        if (Input.GetButton("Submit"))
        {
            score += 1;
        }


        if (OnGame == true)
        {
            TimeText.text = ("" + time);
            Score.text = ("HighScore " + score);
            ScoreMenuPrefab.SetActive(false);
        }
        else
        {
            ScoreMenuPrefab.SetActive(true);
        }
        

        if (time < 0)
        {
            OnGame = false;
            
        }
    }
}
