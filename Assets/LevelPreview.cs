using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreview : MonoBehaviour {
    Gyroscope gyroscope;
    Vector3 velocity = Vector3.zero;
    Vector3 oldEuler;
	// Use this for initialization
	void Awake () {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
        oldEuler = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Transform lvl in transform)
        {
            lvl.Rotate(Vector3.up + Vector3.right, 12 * Time.unscaledDeltaTime);
        }
    }
    void LateUpdate()
    {
        if (gyroscope.enabled)
        {
            if(oldEuler.magnitude!= transform.eulerAngles.magnitude)
            {
                velocity = oldEuler-transform.eulerAngles;
            }
            //transform.Rotate(gyroscope.rotationRate, gyroscope.updateInterval * Time.unscaledDeltaTime);
           // transform.Rotate(gyroscope.rotationRate * 4 * Time.unscaledDeltaTime);
            //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + velocity, 0.125f* Time.unscaledDeltaTime);
            //skyBox.transform.Rotate(gyroscope.rotationRate * 16* Time.unscaledDeltaTime);
            RenderSettings.skybox.SetFloat("_Rotation", gyroscope.rotationRate.magnitude * 180 * Time.unscaledDeltaTime);
        }
    }
}
