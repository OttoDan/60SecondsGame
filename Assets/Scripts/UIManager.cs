using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    private Text scoreText;
    private Text timeText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Two UIManagers in this scene!");
            Destroy(gameObject);
        }

        scoreText = transform.Find("StatsCanvas").transform.Find("ScoreText").GetComponent<Text>();
        timeText = transform.Find("StatsCanvas").transform.Find("TimeText").GetComponent<Text>();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + (int)GameManager.Instance.currentScore;
    }

    public void UpdateTime()
    {
        timeText.text = GameManager.Instance.seconds + " sec";
    }
}
