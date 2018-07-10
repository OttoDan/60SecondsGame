using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour {
    public static ScreenShake Instance;

	private float newMagnitude;
	//public IEnumerator currShake=null;  // makes sure that there is always just 1 coroutine running at once

    float duration = .5f;
    float magnitude = .8f;
    private IEnumerator ShakeCoroutine;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Screenshake changing rotation and position
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    public IEnumerator Shake(float duration, float magnitude)
	{
        Vector3 fromLocalPosition = transform.localPosition;
        Quaternion fromLocalRotation = transform.localRotation;
		//currShake = (Shake(duration, magnitude));

		newMagnitude = Mathf.Pow(magnitude, 2); // so the screenshake is really intens at the start but gets weaker quickly


		float timeShaked = 0f;

		while (timeShaked<duration)
		{
			float x = Random.Range(-1f, 1f);
			float y = Random.Range(-1f, 1f);

			transform.localPosition += new Vector3 (x*.1f,y*.1f,0)*(newMagnitude/2);
			transform.localRotation = Quaternion.Euler(fromLocalRotation.eulerAngles.x, y*newMagnitude, fromLocalRotation.eulerAngles.z );

			timeShaked +=  Time.unscaledDeltaTime;
			newMagnitude = Mathf.Sqrt(newMagnitude); // so the screenshake is really intens at the start but gets weaker quickly
			//Debug.Log(_magnitude);
												
			yield return new WaitForFixedUpdate();
		}
        Debug.Log("nonsence");
        //transform.localRotation = fromLocalRotation;//Quaternion.Euler(0, 0, 0);
        //transform.localPosition = fromLocalPosition;//new Vector3(0,0,transform.localPosition.z);
        //currShake = null;
    }
    public void DoShake(float _duration= 0.5f, float _magnitude = 0.8f)
    {
        //if (ShakeCoroutine != null)
        //    StopCoroutine(ShakeCoroutine);
        //if(ShakeCoroutine == null)
        //{
            ShakeCoroutine = Shake(_duration, _magnitude);
            StartCoroutine(ShakeCoroutine);
        //}
    }
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        DoShake(duration, magnitude);
    //    }
    //}

}
