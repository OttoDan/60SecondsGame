using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostlyScript : MonoBehaviour {

    #region private fields

    private SkinnedMeshRenderer tempMesh;
    private Color tempCol;
    private float timeVisible = 10f;
    private BoxCollider BoxColl;

    IEnumerator FadeOutInRoutine;

    #endregion

    #region public fields
    
    
    #endregion
    
    // Use this for initialization
    void Start () {
        tempMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        if (GetComponent<BoxCollider>() != null)
            BoxColl = GetComponent<BoxCollider>();
        else
            Debug.LogFormat("shit");
        StartFade();
    }

    private void Update()
    {
        //if (FadeInOutCoroutine == null)
        //    StartFade();
    }

    void StartFade()
    {
        if (FadeOutInRoutine == null)
            FadeOutInRoutine = FadeOutIn();
        else
            StopCoroutine(FadeOutInRoutine);
            StartCoroutine(FadeOutInRoutine);
    }
    
    IEnumerator FadeOutIn(float duration = 1f)
    {
        //fadeOut
        for(float t=0; t < duration; t+= Time.fixedDeltaTime)
        {
            tempCol = tempMesh.material.color;
            tempCol.a =  Mathf.Lerp(1, 0, t / (duration));
            tempMesh.material.color = tempCol;
            yield return null;
        }

        BoxColl.enabled = false;

        //wait
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;
        }

        BoxColl.enabled = false;

        //fadeIn
        for (float t = 0; t < duration; t += Time.fixedDeltaTime)
        {
            tempCol = tempMesh.material.color;
            tempCol.a = Mathf.Lerp(0, 1, t / (duration));
            tempMesh.material.color = tempCol;
            yield return null;
        }

        yield return new WaitForSeconds(timeVisible);
        FadeOutInRoutine = null;
    }
}
