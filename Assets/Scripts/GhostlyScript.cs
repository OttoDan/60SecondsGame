using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostlyScript : MonoBehaviour {

    #region private fields

    private SkinnedMeshRenderer tempMesh;
    private Color tempCol;
    private float timeVisible = 10f;

    IEnumerator FadeInOutCoroutine;

    #endregion

    #region public fields
    
    
    #endregion
    
    // Use this for initialization
    void Start () {
        tempMesh = GetComponent<SkinnedMeshRenderer>();
        StartFade();
    }

    private void Update()
    {
        //if (FadeInOutCoroutine == null)
        //    StartFade();
    }

    void StartFade()
    {
        if (FadeInOutCoroutine == null)
            FadeInOutCoroutine = FadeInOut();
        else
            StopCoroutine(FadeInOutCoroutine);
            StartCoroutine(FadeInOutCoroutine);
    }
    
IEnumerator FadeInOut(float duration = 1f)
    {
        //fadein
        for(float t=0; t < duration; t+= Time.fixedDeltaTime)
        {
            tempCol = tempMesh.material.color;
            tempCol.a =  Mathf.Lerp(1, 0, t / (duration));
            tempMesh.material.color = tempCol;
            yield return null;
        }

        //wait
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
        }
            //fadeout
            for (float t = 0; t < duration; t += Time.fixedDeltaTime)
        {
            tempCol = tempMesh.material.color;
            tempCol.a = Mathf.Lerp(0, 1, t / (duration));
            tempMesh.material.color = tempCol;
            yield return null;
        }

        yield return new WaitForSeconds(timeVisible);
        FadeInOutCoroutine = null;
    }
}
