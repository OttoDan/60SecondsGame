using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScoreToJSON : MonoBehaviour
{
    #region Public Fields
    string filename = "data.json";
    string path;
    string JsonString;
<<<<<<< HEAD
    public float lastHighscore;
    public static HighScoreToJSON Instance;
=======
    public float newhighscore;
    public float lastHighscore;
    public static HighScoreToJSON instanceJson;
>>>>>>> MainMenu
    public string highscorename;
    


    public bool gameFinished = true;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
<<<<<<< HEAD
        if (Instance == null)
            Instance = this;
=======
        if (instanceJson == null)
            instanceJson = this;
>>>>>>> MainMenu
        else
            Destroy(gameObject);

        path = Application.persistentDataPath + "/" + filename;
        Debug.Log(path);
<<<<<<< HEAD
        if(File.Exists(path))
        {
            JsonString = File.ReadAllText(path);
            ScoreData data = JsonUtility.FromJson<ScoreData>(JsonString);
            lastHighscore = data.highscore;
            highscorename = data.name;
        }
        else
        {

            lastHighscore = -1;
            highscorename = "never played the game";
        }
=======
        JsonString = File.ReadAllText(path);
        ScoreData data = JsonUtility.FromJson<ScoreData>(JsonString);
        lastHighscore = data.highscore;
        highscorename = data.name;
>>>>>>> MainMenu
    }

    private void Start()
    {
        
        
        
    }


    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD

=======
        if (MainMenu.instanceMenu.OnMenu == false)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {

                SaveData();
            }

            Debug.Log("old" + lastHighscore);
            newhighscore = Timer.instanceTimer.score;
        }
>>>>>>> MainMenu

        
    }
    #endregion

<<<<<<< HEAD
    public void SaveData()
    {
        Debug.Log("Trying to save: Current Score:" + GameManager.Instance.currentScore + " Last HighScore:" + lastHighscore);
        if (GameManager.Instance.currentScore > lastHighscore)
        {
            string contents = JsonUtility.ToJson(new ScoreData(GameManager.Instance.playerName, GameManager.Instance.currentScore), true);
            System.IO.File.WriteAllText(path, contents);
            Debug.Log("Score Saved at: " + path);
=======
    void SaveData()
    {
        if (newhighscore > lastHighscore)
        
        {
            string contents = JsonUtility.ToJson(new ScoreData(SetName.instance.nameField.text, newhighscore), true);
            System.IO.File.WriteAllText(path, contents);
>>>>>>> MainMenu
        }
        
    }

   
}

