using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkScript : MonoBehaviour {

    Texture myTexture;
    MeshRenderer meshRenderer;

    [ContextMenu("Start")]
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(GetText());
        StartCoroutine(GetTextures());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://ottodan.github.io/highscoretest/test.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            Debug.Log(www.error);
        else
        {
            Debug.Log(www.downloadHandler.text);

            List<string> names = new List<string>();
            List<int> scores = new List<int>();

            string str = www.downloadHandler.text;// "\"Name1\" \"Name2\" \"Name3\"";

            for (int i = 0; i<str.Length; i++)
            {
                for(int j = i; i<str.Length; j++)
                {
                    if(str[j] == ',')
                    {
                        string newName = "";
                            
                        for(int k = i; k<j; k++)
                        {
                            newName += str[k];

                        }
                        names.Add(newName);
                        i = j + 1;

                        break;
                    }
                }

                for (int j = i; i < str.Length; j++)
                {
                    if (str[j] == ',')
                    {
                        string newNumber = "";

                        for (int k = i + 1; k < j; k++)
                        {
                            if(char.IsDigit(str[k]))
                                newNumber += str[k];

                        }

                        scores.Add(int.Parse(newNumber));
                        i = j + 1;

                        break;
                    }
                }
            }
            for(int i = 0; i < names.Count; i++)
            {
                Debug.Log(i+1 +" - " + names[i] + ": " + scores[i]);
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
