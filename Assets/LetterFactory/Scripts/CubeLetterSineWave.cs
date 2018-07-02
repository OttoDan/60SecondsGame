using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLetterSineWave : MonoBehaviour {
    IEnumerator WaveRoutine;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (WaveRoutine != null)
                StopCoroutine(WaveRoutine);
            WaveRoutine = WaveTheCubes(4f);
            StartCoroutine(WaveRoutine);
        }
    }

    IEnumerator WaveTheCubes(float duration)
    {
        float maxZ = Mathf.NegativeInfinity;
        float minZ = Mathf.Infinity; 

        foreach(Transform child in transform)
        {
            if (child.position.z > maxZ)
                maxZ = child.position.z;
            if (child.position.z < minZ)
                minZ = child.position.z;
        }

        float currentZ = minZ;

        for(float t = 0; t < duration; t+= Time.deltaTime)
        {
            currentZ = Mathf.Lerp(minZ, maxZ, t / duration);

            foreach (Transform child in transform)
            {
                //child.localScale = Vector3.one * Mathf.Sin(Mathf.Lerp(-1,1,t / duration));
                child.localScale = Vector3.one * Mathf.Sin(Mathf.Lerp(-1, 1,currentZ / child.position.z)) + Vector3.one;
            }

            yield return null;

        }

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            currentZ = Mathf.Lerp(maxZ, minZ, t / duration);

            foreach (Transform child in transform)
            {
                //child.localScale = Vector3.one * Mathf.Sin(Mathf.Lerp(-1,1,t / duration));
                child.localScale = Vector3.one * Mathf.Sin(Mathf.Lerp(-1, 1, currentZ / child.position.z)) + Vector3.one;
            }

            yield return null;

        }
        WaveRoutine = null;
    }

}
