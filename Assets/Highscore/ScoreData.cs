using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{

    public string name = "";
    public float highscore;

    public ScoreData(string name, float highscore)
    {
        this.name = name;
        this.highscore = highscore;
    }
}