using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    #region Public Fields

    public static CameraController Instance;


    // Transform -> Zoom Spring -> ShakeSpring 
    public Transform ZoomSpring;
    public Transform ShakeSpring;
    public Transform RotationSpring;



    #endregion


    #region CameraMovement.cs Adaption



    private Vector3 inputRotation;

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
        ShakeSpring = new GameObject("ShakeSpring").transform;
        ShakeSpring.localPosition = Vector3.zero;
        ShakeSpring.rotation = transform.rotation;
        ShakeSpring.parent = transform.parent;
        ZoomSpring = new GameObject("ZoomSpring").transform;
        ZoomSpring.parent = transform.parent;
        ZoomSpring.position = transform.position;
        ZoomSpring.rotation = transform.rotation;
        RotationSpring = new GameObject("RotationSpring").transform;

    }
    private void Start()
    {
        FlyBack();
    }

    private void LateUpdate()
    {
        inputRotation.x = inputRotation.x * OrbitDamping;
        inputRotation.y = inputRotation.y * OrbitDamping;


       

        if(FlyBackCoroutine == null)
        {
            transform.parent.Rotate(Vector3.forward, inputRotation.x * Time.unscaledDeltaTime);
            transform.parent.Rotate(Vector3.right, -inputRotation.y * Time.unscaledDeltaTime);

            transform.localPosition = ZoomSpring.localPosition + ShakeSpring.localPosition;
            transform.rotation = ShakeSpring.rotation;//Quaternion.Lerp(ZoomSpring.rotation, ShakeSpring.rotation, 0.85f);
        }
    }

    #endregion

    #region Methods
    void ResetSprings()
    {

        ZoomSpring.position = transform.position;
        ZoomSpring.rotation = transform.rotation;
        ShakeSpring.localPosition = Vector3.zero;
        ShakeSpring.rotation = transform.rotation;
    }
    public void InputRotation(float inputX, float inputY)
    {
        //transform.RotateAround(Vector3.zero, Vector3.up, inputX * 5 * Time.unscaledDeltaTime);
        //transform.RotateAround(Vector3.zero, Vector3.Cross(transform.position.normalized, transform.up), -inputY * 5 * Time.unscaledDeltaTime);

        inputRotation.x = inputX * MouseSensitivity;
        inputRotation.y = inputY * MouseSensitivity;
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
        float camDifference = (LevelManager.Instance.MinCamDistance()*3f - transform.localPosition.y);
        Debug.Log("CamDiff"+camDifference);
        if (camDifference < 0)
            Zooming(-camDifference, Zoom.In); 
        if (camDifference > 0)
            Zooming(camDifference, Zoom.Out);
        //transform.position = transform.position + transform.forward * camDifference;
    }
    //to be replaced with
    public void AdjustCameraZoomByPosition()
    {
        RaycastHit raycastHit;
        if(Physics.Raycast(transform.position, -transform.position.normalized, out raycastHit, Mathf.Abs(transform.position.magnitude), LayerMask.GetMask("Walkable")))
        {

            float camDifference = Vector3.Distance(transform.position, raycastHit.point + transform.position.normalized * LevelManager.Instance.MinCamDistance());
            if (camDifference < 0)
                Zooming(-camDifference, Zoom.In);
            if (camDifference > 0)
                Zooming(camDifference, Zoom.Out);
        }
    }
    public void FlyBack()
    {
        if (FlyBackCoroutine != null)
            StopCoroutine(FlyBackCoroutine);
        FlyBackCoroutine = FlyBackRoutine();
        StartCoroutine(FlyBackCoroutine);
    }
    IEnumerator FlyBackCoroutine;

    IEnumerator FlyBackRoutine()
    {
        if(ZoomCoroutine != null)
            StopCoroutine(ZoomCoroutine);

        Quaternion fromDirection = transform.parent.rotation;//Quaternion.LookRotation(-transform.position.normalized,transform.up);
        Vector3 fromUp = transform.parent.up;
        Vector3 fromPivotPosition = transform.parent.position;
        //Vector3 toDirection = -PlayerController.Instance.transform.position.normalized;//get desired position above player
        //temporarily set up as camera pivots up missleadingly is our forward
        transform.parent.up = Vector3Int.CeilToInt(PlayerController.Instance.transform.position);
        Quaternion toDirection = transform.parent.rotation;
        transform.parent.up = fromUp;
        transform.parent.rotation = fromDirection;
        //Quaternion toDirection = Quaternion.AngleAxis(90, Vector3.up) * Quaternion.Euler(Vector3.Cross(-PlayerController.Instance.transform.position.normalized, Vector3.Cross((PlayerController.Instance.transform.position-transform.position).normalized,Vector3.up)));//Quaternion.LookRotation(PlayerController.Instance.transform.position.normalized);//, PlayerController.Instance.transform.up);

        Vector3 fromPosition = transform.localPosition;
        Vector3 toPosition = new Vector3(0, LevelManager.Instance.MinCamDistance()*3, 0);

        float duration = Mathf.Clamp(Vector3.Angle(fromDirection.eulerAngles, toDirection.eulerAngles) / 360 * 2/*+ LevelManager.Instance.MinCamDistance() / (toPosition.y - fromPosition.y)*/, 0.25f, 0.75f);



        for(float t=0; t<duration; t+= Time.unscaledDeltaTime)
        {
            //inputRotation.x = transform.rig;
            transform.parent.position = Vector3.Lerp(fromPivotPosition, Vector3.zero, t / duration);
            transform.parent.rotation = Quaternion.Lerp(fromDirection, toDirection/*Quaternion.Euler(toDirection)*/, t / duration);
            transform.localPosition = Vector3.Lerp(fromPosition, toPosition, t / duration);
            yield return new WaitForEndOfFrame();
        }

        transform.parent.position = Vector3.zero;
        transform.parent.rotation = toDirection;
        transform.localPosition = toPosition;

        ResetSprings();
        FlyBackCoroutine = null;
        //AdjustCameraZoomByPosition();

    }

    IEnumerator StopRotationRoutine(float duration)
    {
        //float fromRotationX = localRotation.x;
        //float fromRotationY = localRotation.y;
        //for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        //{
        //    localRotation.x = Mathf.Lerp(fromRotationX, 0, t / duration);
        //    localRotation.y = Mathf.Lerp(fromRotationY, 0, t / duration);
        //    yield return new WaitForFixedUpdate();
        //}
        //Debug.Log("StopRotation");
        //StopRotationCoroutine = null;
        yield return null;
    }
    public void Zooming(float factor, Zoom zoom, float lerpDuration = 0.5f, float stayDuration = 0.25f)
    {
        if (ZoomCoroutine == null && FlyBackCoroutine == null)
        {
            ZoomCoroutine = ZoomRoutine(factor, zoom, lerpDuration , stayDuration);
            StartCoroutine(ZoomCoroutine);
        }
    }
   
    public void ZoomAtPos(float factor, Zoom zoom, Vector3 pos, float lerpDuration = 0.5f, float stayDuration = 0.25f)
    {
        if (FlyBackCoroutine == null)
        {
            if (ZoomCoroutine != null)
                StopCoroutine(ZoomCoroutine);
            else
            {
                localPositionBeforeZoom = ZoomSpring.localPosition;
                localRotationBeforeZoom = ZoomSpring.localRotation;
                parentPositionBeforeZoom = transform.parent.position;

            }

            ZoomCoroutine = ZoomAtPosRoutine(factor, zoom, pos, lerpDuration, stayDuration);
            StartCoroutine(ZoomCoroutine);
        }
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
            ZoomSpring.localPosition = Vector3.Lerp(fromPos, toPos, t / lerpDuration);
            //transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(fromY, toY, t / lerpDuration),transform.localPosition.z);
            yield return new WaitForFixedUpdate();
        }

        if (zoom == Zoom.InOut)
        {
            for (float t = 0; t < stayDuration; t += Time.unscaledDeltaTime)
                yield return null;
            for (float t = 0; t < lerpDuration; t += Time.unscaledDeltaTime)
            {
                ZoomSpring.position = Vector3.Lerp(toPos, fromPos, t / lerpDuration);
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
        Vector3 fromPos = ZoomSpring.localPosition;
        Vector3 toPos = new Vector3();
        Quaternion fromRotation = ZoomSpring.localRotation;
        Quaternion toRotation;
        Vector3 parentFromPos = transform.parent.position;
        Vector3 parentToPos = ZoomSpring.position + (pos - ZoomSpring.position).normalized * (pos - ZoomSpring.position).magnitude*0.95f;
        //We are moving around the cube by transforming the parent, so we need to adjust our local y coordinate
        float fromY = ZoomSpring.localPosition.y;
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

        toRotation = Quaternion.LookRotation((ZoomSpring.InverseTransformPoint(pos) - toPos).normalized, ZoomSpring.up);

        for (float t = 0; t < lerpDuration; t += Time.unscaledDeltaTime)
        {
            ZoomSpring.localPosition = Vector3.Lerp(fromPos, toPos, t / lerpDuration);
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
                ZoomSpring.localPosition = Vector3.Lerp(toPos, localPositionBeforeZoom, t / lerpDuration);
                transform.parent.position = Vector3.Lerp(parentToPos, parentPositionBeforeZoom, t / lerpDuration);
                //transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(toY, fromY, t / lerpDuration), transform.localPosition.z);
                //transform.localRotation = Quaternion.Lerp(toRotation, localRotationBeforeZoom, t / lerpDuration);
                yield return new WaitForFixedUpdate();
            }
            ZoomSpring.localPosition = localPositionBeforeZoom;
            transform.parent.position = parentPositionBeforeZoom;
            //transform.localRotation = localRotationBeforeZoom;
        }


        //}

        Debug.Log("ZoomEnd");
        ZoomCoroutine = null;
    }

    //void Zoom
}
