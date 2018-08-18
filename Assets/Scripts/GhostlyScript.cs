using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostlyScript : MonoBehaviour {

    #region private fields

    private SkinnedMeshRenderer tempMesh;
    private Color tempCol;
    private float timeVisible = 2.5f;
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
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    void StartFade()
    {
        if (FadeOutInRoutine == null)
            FadeOutInRoutine = FadeOutIn();
        else
            StopCoroutine(FadeOutInRoutine);
            StartCoroutine(FadeOutInRoutine);
    }
    
    IEnumerator FadeOutIn(float duration = 0.25f)
    {
        while (enabled)
        {
            //fadeOut
            for (float t = 0; t < duration; t += Time.fixedDeltaTime)
            {
                tempCol = tempMesh.material.color;
                tempCol.a = Mathf.Lerp(1, 0, t / (duration));
                tempMesh.material.color = tempCol;
                yield return null;
            }
            tempCol.a = 0;

            BoxColl.enabled = false;

            //wait

            yield return new WaitForSeconds(timeVisible*0.5f);


            //fadeIn
            for (float t = 0; t < duration; t += Time.fixedDeltaTime)
            {
                tempCol = tempMesh.material.color;
                tempCol.a = Mathf.Lerp(0, 1, t / (duration));
                tempMesh.material.color = tempCol;
                yield return null;
            }
            tempCol.a = 1;
            BoxColl.enabled = true;

            yield return new WaitForSeconds(timeVisible);
        }
        
        FadeOutInRoutine = null;
    }
}
