using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusParticles : MonoBehaviour {
    public static FocusParticles Instance;

    public float transitionDuration = 0.125f;

    IEnumerator coroutine;

    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Two FocusParticles in this scene!");
            Destroy(gameObject);
        }
        //GetComponent<ParticleSystem>().Simulate(Time.unscaledDeltaTime, true, false);
    }
    #endregion

    public void MoveToPoint(DashPoint dashPoint)
    {
        StopAllCoroutines();
        coroutine = MoveRoutine(dashPoint);
        StartCoroutine(coroutine);
    }

    IEnumerator MoveRoutine(DashPoint dashPoint)
    {
        Vector3 fromPosition = transform.position;
        Vector3 fromNormal = transform.up;
        for(float t=0; t < transitionDuration; t+= Time.unscaledDeltaTime)
        {
            transform.position = Vector3.Lerp(fromPosition, dashPoint.position + dashPoint.normal*0.5f, t / transitionDuration);
            //transform.up = Vector3.Lerp(fromNormal, dashPoint.normal, t / transitionDuration);
            yield return null;
        }
    }
}
