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
    public float newhighscore;
    public float lastHighscore;
    public static HighScoreToJSON instanceJson;
    public string highscorename;
    


    public bool gameFinished = true;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        if (instanceJson == null)
            instanceJson = this;
        else
            Destroy(gameObject);

        path = Application.persistentDataPath + "/" + filename;
        Debug.Log(path);
        JsonString = File.ReadAllText(path);
        ScoreData data = JsonUtility.FromJson<ScoreData>(JsonString);
        lastHighscore = data.highscore;
        highscorename = data.name;
    }

    private void Start()
    {
        
        
        
    }


    // Update is called once per frame
    void Update()
    {
        if (MainMenu.instanceMenu.OnMenu == false)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {

                SaveData();
            }

            Debug.Log("old" + lastHighscore);
            newhighscore = Timer.instanceTimer.score;
        }

        
    }
    #endregion

    void SaveData()
    {
        if (newhighscore > lastHighscore)
        
        {
            string contents = JsonUtility.ToJson(new ScoreData(SetName.instance.nameField.text, newhighscore), true);
            System.IO.File.WriteAllText(path, contents);
        }
        
    }

   
}

