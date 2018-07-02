using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLettersButton : MonoBehaviour {

    Collider collider;
    List<Transform> Letters = new List<Transform>();
    IEnumerator coroutine;
    int pixelCount = 0;
	void Start () {
        collider = GetComponent<Collider>();
        foreach(Transform letter in transform)
        {
            Letters.Add(letter);
            pixelCount += letter.childCount;
        }

        coroutine = IntroTransition();
        StartCoroutine(coroutine);
	}
	
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
    }

    IEnumerator IntroTransition()
    {
        Vector3[] cubeFromPos = new Vector3[pixelCount];
        Vector3[] cubeToPos = new Vector3[pixelCount];
        int i = 0;
        Debug.Log("test");
        foreach (Transform letter in Letters)
        {
            foreach(Transform cube in letter)
            {
                cubeToPos[i] = cube.position;
                cubeFromPos[i] = cube.position + Random.onUnitSphere * 64;
            }
        }

        float duration = 2;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            i = 0;
            foreach (Transform letter in Letters)
            {
                foreach (Transform cube in letter)
                {
                    cube.position = Vector3.Lerp(cubeFromPos[i], cubeToPos[i], t / duration);
                    i++;
                }
            }

            yield return null;
        }
    }
}
