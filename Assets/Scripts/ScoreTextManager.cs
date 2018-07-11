using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextManager : MonoBehaviour {

    public static ScoreTextManager Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScoreText(int score)
    {

    }

}

