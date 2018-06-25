using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreToJSON : MonoBehaviour
{
    #region Public Fields
    string filename = "data.json";
    string path;

    

    

    public bool gameFinished = true;
    //public string nickName = "Hansi";
    //public float highscore = Random.Range(1f, 100000000000f);
    #endregion

    #region UnityFunctions
    private void Start()
    {
        //
        path = Application.persistentDataPath + "/" + filename;
        Debug.Log(path);

        //highscore = GetComponent<>();
        //nickName = GetComponent<>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {

            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ReadData();
        }
    }
    #endregion

    void SaveData()
    {
        string contents = JsonUtility.ToJson(new ScoreData(SetName.instance.getname.text, Timer.instance.score), true);
        System.IO.File.WriteAllText(path, contents);
    }

    void ReadData()
    {

    }
}

