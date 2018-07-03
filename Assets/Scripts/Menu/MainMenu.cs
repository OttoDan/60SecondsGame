using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Text HighscoreText;
    public Text HighscoreNameText;
    public float highscoretext;
    public string highscoreNameText;
    public bool OnMenu = true;
    public static MainMenu instanceMenu;
    

    public void Awake()
    {
        if (instanceMenu == null)
            instanceMenu = this;
        else
            Destroy(gameObject);
    }

    /*void Start()
    {
        highscoretext = HighScoreToJSON.instanceJson.lastHighscore;
        highscoreNameText = HighScoreToJSON.instanceJson.highscorename;
    }*/

    void Update()
    {
        highscoretext = HighScoreToJSON.instanceJson.lastHighscore;
        highscoreNameText = HighScoreToJSON.instanceJson.highscorename;
        HighscoreNameText.text = ("" + highscoreNameText +("  ") + highscoretext);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Go");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


    public void EndGame()
    {
        Application.Quit();
        Debug.Log("Ente");
    }

    /*public void Text()
    {
        //HighscoreText.text = ("" + highscoretext);
        HighscoreNameText.text = ("" + highscoreNameText + highscoretext);
    }*/

   
}
