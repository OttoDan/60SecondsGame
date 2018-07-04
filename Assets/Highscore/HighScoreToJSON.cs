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
    public float lastHighscore;
    public static HighScoreToJSON Instance;
    public string highscorename;
    


    public bool gameFinished = true;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        path = Application.persistentDataPath + "/" + filename;
        Debug.Log(path);
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
    }

    private void Start()
    {
        
        
        
    }


    // Update is called once per frame
    void Update()
    {


        
    }
    #endregion

    public void SaveData()
    {
        Debug.Log("Trying to save: Current Score:" + GameManager.Instance.currentScore + " Last HighScore:" + lastHighscore);
        if (GameManager.Instance.currentScore > lastHighscore)
        {
            string contents = JsonUtility.ToJson(new ScoreData(GameManager.Instance.playerName, GameManager.Instance.currentScore), true);
            System.IO.File.WriteAllText(path, contents);
            Debug.Log("Score Saved at: " + path);
        }
        
    }

   
}

