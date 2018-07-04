using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    #region Public Fields

    public static CameraController Instance;

    #endregion

    #region CameraMovement.cs Adaption



    private Vector3 localRotation;

    public float MouseSensitivity = 12;

    public float OrbitDamping = .92f;

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


    private void LateUpdate()
    {


        localRotation.x = localRotation.x * OrbitDamping;
        localRotation.y = localRotation.y * OrbitDamping;

        transform.parent.Rotate(Vector3.up, localRotation.x * 16 * Time.unscaledDeltaTime);
        transform.parent.Rotate(Vector3.right, -localRotation.y * 16 * Time.unscaledDeltaTime);
    }

    #endregion

    #region Methods

    public void InputRotation(float inputX, float inputY)
    {
        //transform.RotateAround(Vector3.zero, Vector3.up, inputX * 5 * Time.unscaledDeltaTime);
        //transform.RotateAround(Vector3.zero, Vector3.Cross(transform.position.normalized, transform.up), -inputY * 5 * Time.unscaledDeltaTime);

        localRotation.x = inputX;
        localRotation.y = inputY;
    }


    #endregion
}
