using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    #region Public Fields

    public static CameraController Instance;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Two CameraControllers in scene!");
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        
    }

    #endregion

    #region Methods

    public void InputRotation(float inputX, float inputY)
    {
        transform.RotateAround(Vector3.zero, Vector3.up, inputX * 5 * Time.unscaledDeltaTime);
        transform.RotateAround(Vector3.zero, Vector3.Cross(transform.position.normalized, transform.up), -inputY * 5 * Time.unscaledDeltaTime);
    }


    #endregion
}
