using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    public Text TimeText;
    public float time = 60;
    public bool OnGame = true;

    public GameObject ScoreMenuPrefab;
    public GameObject Game;

    void Start()
    {
        OnGame = true;

        
    }

    void Update()
    {

        time -= Time.deltaTime;
        
        if (OnGame = true)
        {
            TimeText.text = ("" + time);
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
