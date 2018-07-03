using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    #region Public Fields

    public static InputManager Instance;

    /// <summary>
    /// the finger id of the Touch inside the cube
    /// </summary>
    public int cubeFingerID { get; private set; }

    /// <summary>
    /// the finger id of the Touch outside the cube
    /// </summary>
    public int cameraFingerID { get; private set; }

    #endregion



    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogError("Two InputManagers in scene!");
            Destroy(gameObject);
        }

        switch(Application.platform)
        {
            case RuntimePlatform.WindowsEditor:

                break;

            case RuntimePlatform.Android:

            break;

            case RuntimePlatform.WindowsPlayer:

                break;
        }
        cubeFingerID    = -1;
        cameraFingerID  = -1;
    }


    private void Update()
    {
        TouchInput();
    }

    #endregion

    #region Methods

    void TouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch currentTouch = Input.GetTouch(i);

            switch (currentTouch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = Camera.main.ScreenPointToRay(currentTouch.position);
                    if (Physics.Raycast(ray,Camera.main.transform.position.magnitude, LayerMask.GetMask("Walkable")))//TODO: restrict to the walkable layer / ignore all other layers
                    {
                        cubeFingerID = currentTouch.fingerId;
                        //dashPaint = true;
                        Debug.Log("cubeFingerID: " + currentTouch.fingerId);
                    }
                    else
                    {
                        cameraFingerID = currentTouch.fingerId;
                        //camRotate = true;
                        Debug.Log("cameraFingerID: " + currentTouch.fingerId);
                    }
                    break;

                case TouchPhase.Moved:
                    if (cubeFingerID == currentTouch.fingerId)
                    {
                        //paintUpdateFrames++;

                        //if (paintUpdateFrames % paintUpdateFrameFrequence == 0)
                        //{
                        PlayerController.Instance.PlaceDashpoint(currentTouch.position);
                        //    paintUpdateFrames = 0;
                        //}

                    }
                    else if (cameraFingerID == currentTouch.fingerId)
                    {
                        Vector2 touchDeltaPosition = Input.GetTouch(i).deltaPosition;
                        CameraController.Instance.InputRotation(touchDeltaPosition.x, touchDeltaPosition.y);
                    }
                    break;

                case TouchPhase.Ended:
                    Debug.Log("Ended: " + i);
                    if (cubeFingerID == i)
                        cubeFingerID = -1;
                    if (cameraFingerID == i)
                        cameraFingerID = -1;
                    break;
                case TouchPhase.Canceled:
                    Debug.Log("Canceled: " + i);
                    if (cubeFingerID == i)
                        cubeFingerID = -1;
                    if (cameraFingerID == i)
                        cameraFingerID = -1;
                    break;
            }

        }
    }

    #endregion
}
