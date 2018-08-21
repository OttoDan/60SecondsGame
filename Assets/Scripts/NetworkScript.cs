using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkScript : MonoBehaviour {
    public static NetworkScript Instance;
    private string access_token = "a86ugar386a3ghaf38jqh8o3fq8";
    public static System.Action<HighscoreData> OnHighscoreDataReceived;
    public string user = "Hans";
    public int score = 1000;
    public string uri = "http://www.laienoper.de/scores.php";
    public Text Scoretext;

    [ContextMenu("Test")]
    void Start() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        //string uriSend = uri + "?user=" + user + "&score=" + score + "&token=" + access_token;
        //Debug.Log(uriSend);
        StartCoroutine(GetText());
        DontDestroyOnLoad(gameObject);
    }
    public void SendScore(string name, int scoreValue)
    {
        StartCoroutine(uri + "?user=" + name + "&score=" + scoreValue + "&token=" + access_token);
    }
    IEnumerator SetText (string uriSend)
    {
        UnityWebRequest www = UnityWebRequest.Get(uriSend);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogFormat("isNet: {0} ; isHttp: {1}; uri: {2}", www.isNetworkError, www.isHttpError, uri);
            Debug.LogError(www.error);
        }
        else
        {
            // successful
            Debug.Log(www.downloadHandler.text);

            // parse json text
            HighscoreData data = JsonUtility.FromJson<HighscoreData>(www.downloadHandler.text);
            Debug.Log(data + ": Data!");
            //Debug.Log(data.entries);
            //for (int i = 0; i < data.entries.Count; i++)
            //{
            //    Debug.Log(data.entries[i].user + ": " + data.entries[i].score);
            //}

            if (OnHighscoreDataReceived != null)
            {
                OnHighscoreDataReceived(data);
            }
        }
    }


IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogFormat("isNet: {0} ; isHttp: {1}; uri: {2}", www.isNetworkError , www.isHttpError, uri);
            Debug.LogError(www.error);
        } else
        {
            // successful
            Debug.Log(www.downloadHandler.text);

            // parse json text
            HighscoreData data = JsonUtility.FromJson<HighscoreData>(www.downloadHandler.text);
            Debug.Log(data + ": Data!");
            //Debug.Log(data.entries);
            //for (int i = 0; i < data.entries.Count; i++)
            //{
            //    Debug.Log(data.entries[i].user + ": " + data.entries[i].score);
            //}
            if(Scoretext!= null)
            {
                Scoretext.text = "";
                for (int i = 0; i < data.entries.Count && i < 11; i++)
                {
                    Scoretext.text += i + 1 + ". " + data.entries[i].user + ": " + data.entries[i].score + "\n";
                }
            }
            
            //if (OnHighscoreDataReceived != null)
            //{
            //    OnHighscoreDataReceived(data);

            //}
        }
    }
}


//sendScores for loop webrequest "laienoper.de/scores.php?name="+name+"&score="+score"