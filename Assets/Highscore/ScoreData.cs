using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{

    public string name = "";
    public float score;

    public ScoreData(string name, float score)
    {
        this.name = name;
        this.score = score;
    }
}