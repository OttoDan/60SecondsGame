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
    private void Start()
    {
        AdjustCameraZoomByLevelBounds();
    }

    private void LateUpdate()
    {

        transform.parent.Rotate(Vector3.up, localRotation.x * Time.unscaledDeltaTime);
        transform.parent.Rotate(Vector3.right, -localRotation.y * Time.unscaledDeltaTime);
    }

    #endregion

    #region Methods

    public void InputRotation(float inputX, float inputY)
    {
        //transform.RotateAround(Vector3.zero, Vector3.up, inputX * 5 * Time.unscaledDeltaTime);
        //transform.RotateAround(Vector3.zero, Vector3.Cross(transform.position.normalized, transform.up), -inputY * 5 * Time.unscaledDeltaTime);

        localRotation.x = inputX * MouseSensitivity;
        localRotation.y = inputY * MouseSensitivity;
    }


    #endregion
    //TODO:Cleanup/fall back to structure
    public enum Zoom
    {
        In,
        Out,
        InOut
    }

    IEnumerator StopRotationCoroutine;
    float stopRotationDuration = 0.125f;
    public void StopRotation()
    {
        if(StopRotationCoroutine == null)
        {
            StopRotationCoroutine = StopRotationRoutine(stopRotationDuration);
            StartCoroutine(StopRotationCoroutine);
        }
    }
    public void AdjustCameraZoomByLevelBounds()
    {
        Debug.Log("MinDist"+LevelManager.Instance.MinCamDistance());
        float camDifference = (LevelManager.Instance.MinCamDistance()*2f - transform.localPosition.y);
        Debug.Log("CamDiff"+camDifference);
        if (camDifference < 0)
            Zooming(-camDifference, Zoom.In); 
        if (camDifference > 0)
            Zooming(camDifference, Zoom.Out);
        //transform.position = transform.position + transform.forward * camDifference;
    }
    public void InputDoubleTapZoom()
    {
        //if()
    }
    IEnumerator StopRotationRoutine(float duration)
    {
        float fromRotationX = localRotation.x;
        float fromRotationY = localRotation.y;
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            localRotation.x = Mathf.Lerp(fromRotationX, 0, t / duration);
            localRotation.y = Mathf.Lerp(fromRotationY, 0, t / duration);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("StopRotation");
        StopRotationCoroutine = null;
    }
    public void Zooming(float factor, Zoom zoom, float lerpDuration = 0.5f, float stayDuration = 0.25f)
    {
        if(ZoomCoroutine == null)
        {
            ZoomCoroutine = ZoomRoutine(factor, zoom, lerpDuration , stayDuration);
            StartCoroutine(ZoomCoroutine);
        }
    }
   
    public void ZoomAtPos(float factor, Zoom zoom, Vector3 pos, float lerpDuration = 0.5f, float stayDuration = 0.25f)
    {
        if (ZoomCoroutine != null)
            StopCoroutine(ZoomCoroutine);
        else
        {
            localPositionBeforeZoom = transform.localPosition;
            localRotationBeforeZoom = transform.localRotation;
            parentPositionBeforeZoom = transform.parent.position;

        }
        ZoomCoroutine = ZoomAtPosRoutine(factor, zoom, pos, lerpDuration, stayDuration);
        StartCoroutine(ZoomCoroutine);
    }
    IEnumerator ZoomCoroutine;
    IEnumerator ZoomRoutine(float factor, Zoom zoom, float lerpDuration = 0.5f, float stayDuration = 0.25f)
    {
        Vector3 fromPos = transform.localPosition;
        Vector3 toPos = new Vector3();

        //We are moving around the cube by transforming the parent, so we need to adjust our local y coordinate
        float fromY = transform.localPosition.y;
        float toY = 0;
        switch (zoom)
        {
            case Zoom.In:
                toPos = fromPos - Vector3.up * factor;
                break;
            case Zoom.Out:
                toPos = fromPos + Vector3.up * factor;
                break;
            case Zoom.InOut:
                toPos = fromPos - Vector3.up * factor;
                break;
        }
        
        for (float t = 0; t < lerpDuration; t += Time.unscaledDeltaTime)
        {
            transform.localPosition = Vector3.Lerp(fromPos, toPos, t / lerpDuration);
            //transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(fromY, toY, t / lerpDuration),transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }

        if (zoom == Zoom.InOut)
        {
            for (float t = 0; t < stayDuration; t += Time.unscaledDeltaTime)
                yield return null;
            for (float t = 0; t < lerpDuration; t += Time.unscaledDeltaTime)
            {
                transform.position = Vector3.Slerp(toPos, fromPos, t / lerpDuration);
                //transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(toY, fromY, t / lerpDuration), transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }
        }
            

        //}

        Debug.Log("ZoomEnd");
        ZoomCoroutine = null;
    }
    Vector3 localPositionBeforeZoom;
    Vector3 parentPositionBeforeZoom;
    Quaternion localRotationBeforeZoom;
    IEnumerator ZoomAtPosRoutine(float factor, Zoom zoom,Vector3 pos, float lerpDuration = 0.5f, float stayDuration = 0.25f)
    {
        Vector3 fromPos = transform.localPosition;
        Vector3 toPos = new Vector3();
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation;
        Vector3 parentFromPos = transform.parent.position;
        Vector3 parentToPos = transform.position + (pos - transform.position).normalized * (pos - transform.position).magnitude*0.95f;
        //We are moving around the cube by transforming the parent, so we need to adjust our local y coordinate
        float fromY = transform.localPosition.y;
        float toY = 0;
        switch (zoom)
        {
            case Zoom.In:
                toPos = fromPos - Vector3.up * factor;
                break;
            case Zoom.Out:
                toPos = fromPos + Vector3.up * factor;
                break;
            case Zoom.InOut:
                toPos = fromPos - Vector3.up * factor;
                break;
        }
        //toPos += (transform.InverseTransformPoint(pos) - transform.localPosition).normalized * (transform.InverseTransformPoint(pos) - transform.localPosition).magnitude * 0.125f;

        toRotation = Quaternion.LookRotation((transform.InverseTransformPoint(pos) - toPos).normalized,transform.up);

        for (float t = 0; t < lerpDuration; t += Time.unscaledDeltaTime)
        {
            transform.localPosition = Vector3.Lerp(fromPos, toPos, t / lerpDuration);
            transform.parent.position = Vector3.Lerp(parentFromPos, parentToPos, t / lerpDuration);
            //transform.localRotation = Quaternion.Lerp(fromRotation, toRotation, t / lerpDuration);
            //transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(fromY, toY, t / lerpDuration),transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }

        if (zoom == Zoom.InOut)
        {
            for (float t = 0; t < stayDuration; t += Time.deltaTime)
            {
                yield return new WaitForFixedUpdate();
            }
            for (float t = 0; t < lerpDuration; t += Time.unscaledDeltaTime)
            {
                transform.localPosition = Vector3.Lerp(toPos, localPositionBeforeZoom, t / lerpDuration);
                transform.parent.position = Vector3.Lerp(parentToPos, parentPositionBeforeZoom, t / lerpDuration);
                //transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(toY, fromY, t / lerpDuration), transform.localPosition.z);
                //transform.localRotation = Quaternion.Lerp(toRotation, localRotationBeforeZoom, t / lerpDuration);
                yield return new WaitForFixedUpdate();
            }
            transform.localPosition = localPositionBeforeZoom;
            transform.parent.position = parentPositionBeforeZoom;
            //transform.localRotation = localRotationBeforeZoom;
        }


        //}

        Debug.Log("ZoomEnd");
        ZoomCoroutine = null;
    }

    //void Zoom
}
