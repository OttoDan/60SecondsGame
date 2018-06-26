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

    

    public bool gameFinished = true;
   
    #endregion

    #region UnityFunctions
    private void Start()
    {
        
        path = Application.persistentDataPath + "/" + filename;
        Debug.Log(path);
        JsonString = File.ReadAllText(path);
        ScoreData data = JsonUtility.FromJson<ScoreData>(JsonString);
        lastHighscore = data.highscore;
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            SaveData();
        }
        
        Debug.Log("old" + lastHighscore);
        newhighscore = Timer.instance.score;
    }
    #endregion

    void SaveData()
    {
        if (newhighscore > lastHighscore)
        
        {
            string contents = JsonUtility.ToJson(new ScoreData(SetName.instance.getname.text, newhighscore), true);
            System.IO.File.WriteAllText(path, contents);
        }
        
    }

   
}

