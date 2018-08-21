using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreData {

    [System.Serializable]
    public class HighscoreDataEntry
    {
        public string user;
        public int score;

        public HighscoreDataEntry()
        {
            this.user = "";
            this.score = 0;
        }
    }

    public List<HighscoreDataEntry> entries;

    public HighscoreData()
    {
        entries = new List<HighscoreDataEntry>();
    }
}
