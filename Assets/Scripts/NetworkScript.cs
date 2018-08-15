using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkScript : MonoBehaviour {

    //Das Event wo sich die anderen Klassen dranhaengen mussen um immer den aktuellsten Datensatz zu bekommen
    public static System.Action<HighscoreData> OnHighscoreDataReceived;



    Texture myTexture;
    MeshRenderer meshRenderer;

    [ContextMenu("Start")]
    public void Start()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(GetText());
        //StartCoroutine(GetTextures());
    }

    IEnumerator GetText()
    {
        //UnityWebRequest www = UnityWebRequest.Get("https://ottodan.github.io/highscoretest/test.txt");
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/test/scores.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
        {
            Debug.Log(www.downloadHandler.text);

            //PARSE JSON
            HighscoreData data = JsonUtility.FromJson<HighscoreData>(www.downloadHandler.text);
            Debug.Log(data.entries);
            for (int i = 0; i < data.entries.Count; i++)
            {
                Debug.Log(data.entries[i].user + " : " + data.entries[i].score);
            }

            if (OnHighscoreDataReceived != null)
            {
                OnHighscoreDataReceived(data);
            }
        }
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://picsum.photos/200/300/?random");
//"http://www.casadelkerls.de/Images/test02.png");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);

        else
        {
            myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Debug.Log(myTexture.name);
        }

        meshRenderer.material.SetTexture("_MainTex", myTexture);
    }

    IEnumerator GetTextures()
    {
        for(int i = 0; i < 100; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://picsum.photos/200/300/?random");
            //"http://www.casadelkerls.de/Images/test02.png");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);

            else
            {
                myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Debug.Log(myTexture.name);
            }
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.AddComponent<Rigidbody>();
            cube.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", myTexture);
            cube.transform.position = Random.insideUnitSphere * 16;
        }
        
    }
}
