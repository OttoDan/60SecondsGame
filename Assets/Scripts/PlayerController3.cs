using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3 : MonoBehaviour {

    #region Public Fields
    public static PlayerController3 Instance;

    IEnumerator DashCoroutine;
    public Canvas canvas;
    public bool isPaused = true;
    public float camTransitionDuration = 1.5f;

    #endregion

    #region Private Fields

    private bool dashPaint = false;
    private bool camRotate = false;
    private int paintUpdateFrames = 0;
    private int paintUpdateFrameFrequence = 2;
    List<DashPoint> dashPoints = new List<DashPoint>();

    int dashPaintFingerID = -1;
    int camRotateFingerID = -1;

    LineRenderer lineRenderer;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        TimeManager.Instance.DoSlowmotion();
    }
    void Update()
    {

        
        for(int i = 0; i<Input.touchCount && i<2; i++)
        {
            Touch currentTouch = Input.GetTouch(i);
            switch (currentTouch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = Camera.main.ScreenPointToRay(currentTouch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray))//TODO: restrict to the walkable layer / ignore all other layers
                    {
                        dashPaintFingerID = currentTouch.fingerId;
                        dashPaint = true;
                        Debug.Log("dashPaint Index: " + i + " FingerID" + currentTouch.fingerId);
                    }
                    else
                    {
                        camRotateFingerID = currentTouch.fingerId;
                        camRotate = true;
                        Debug.Log("CamRotate Index: " + i + " FingerID" + currentTouch.fingerId);
                    }
                    break;

                case TouchPhase.Moved:
                    if (dashPaintFingerID == currentTouch.fingerId)
                    {
                        paintUpdateFrames++;

                        if (paintUpdateFrames % paintUpdateFrameFrequence == 0)
                        {
                            paintUpdateFrames = 0;
                            PaintDashPoints(currentTouch);
                        }

                    }
                    else if (camRotateFingerID == currentTouch.fingerId)
                    {
                        Vector2 touchDeltaPosition = Input.GetTouch(i).deltaPosition;
                        float inputX = touchDeltaPosition.x;
                        float inputY = touchDeltaPosition.y;
                        Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, inputX * 5 * Time.deltaTime);
                        Camera.main.transform.RotateAround(Vector3.zero, Vector3.Cross(Camera.main.transform.position.normalized, Camera.main.transform.up), inputY * 5 * Time.deltaTime);
                    }
                    break;

                case TouchPhase.Ended:
                    Debug.Log("Ended: " + i);
                    if (dashPaintFingerID == i)
                        dashPaintFingerID = -1;
                    if (camRotateFingerID == i)
                        camRotateFingerID = -1;
                    break;
                case TouchPhase.Canceled:
                    Debug.Log("Canceled: " + i);
                    if (dashPaintFingerID == i)
                        dashPaintFingerID = -1;
                    if (camRotateFingerID == i)
                        camRotateFingerID = -1;
                    break;
            }
            
        }


        /*
         * OLD mouse / touch hybrid
         */

        ////Touch.Phase.Begin
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray))
        //    {
        //        dashPaint = true;
        //    }
        //    else
        //    {
        //        camRotate = true;
        //    }
        //} 
        //if (dashPaint)
        //{
        //    paintUpdateFrames++;

        //    if (paintUpdateFrames % paintUpdateFrameFrequence == 0)
        //    {
        //        paintUpdateFrames = 0;

        //        //Touch.Phase.Moved
        //        if (Input.GetMouseButton(0))
        //        {
        //            PaintDashPoints();
        //        }
        //    }

        //    //Touc.Phase.Ended
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        dashPaint = false;
        //    }

        //}

        //if (camRotate && Input.touchCount > 0)
        //{
        //    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //    float inputX = touchDeltaPosition.x;
        //    float inputY = touchDeltaPosition.y;
        //    Camera.main.transform.RotateAround(Vector3.zero, transform.up, inputX * 5 * Time.deltaTime);
        //    Camera.main.transform.RotateAround(Vector3.zero, transform.forward, inputY * 5 * Time.deltaTime);
        //}
        DebugDrawDashPoints();

        DebugDrawSurroundingTiles();
    }

    #endregion

    #region Methods

    void PaintDashPoints(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (dashPoints.Count > 0)
            {
                float dist = Vector3.Distance(dashPoints[dashPoints.Count - 1].position, Snap(hit.point));
                //prevent points that are too close to each other
                if (dist > 1)
                {
                    //Prevent unvalid paths
                    RaycastHit validationHit;
                    Vector3 fromPos = dashPoints[dashPoints.Count - 1].position  + dashPoints[dashPoints.Count - 1].normal * 0.5f;
                    Vector3 direction = Snap(hit.point) - fromPos;

                    if (Physics.Raycast(fromPos, direction.normalized, out validationHit, dist))
                    {
                        dashPoints.Add(new DashPoint(Snap(validationHit.point), validationHit.normal));
                        lineRenderer.positionCount = dashPoints.Count + 1;
                        lineRenderer.SetPosition(0, transform.position);
                        for (int i = 1; i < lineRenderer.positionCount; i++)
                        {
                            lineRenderer.SetPosition(i, dashPoints[i - 1].position);

                        }
                    }
                    else
                    {
                        //Debug.Log(Snap(hit.point));
                        dashPoints.Add(new DashPoint(Snap(hit.point), hit.normal));
                        lineRenderer.positionCount = dashPoints.Count + 1;
                        lineRenderer.SetPosition(0, transform.position);
                        for (int i = 1; i < lineRenderer.positionCount; i++)
                        {
                            lineRenderer.SetPosition(i, dashPoints[i - 1].position);

                        }
                    }
                    
                }
            }
            else
            {
                //Prevent unvalid paths
                RaycastHit validationHit;
                Vector3 fromPos = transform.position + transform.up * 0.5f;// + dashPoints[dashPoints.Count - 1].normal * 0.5f;
                float dist = Vector3.Distance(transform.position, hit.point);
                Vector3 direction = Snap(hit.point) - fromPos;

                if (Physics.Raycast(fromPos, direction.normalized, out validationHit, dist))
                {
                    dashPoints.Add(new DashPoint(Snap(validationHit.point), validationHit.normal));
                    lineRenderer.positionCount = dashPoints.Count + 1;
                    lineRenderer.SetPosition(0, transform.position);
                    for (int i = 1; i < lineRenderer.positionCount; i++)
                    {
                        lineRenderer.SetPosition(i, dashPoints[i - 1].position);

                    }
                }
                else
                {

                    dashPoints.Add(new DashPoint(Snap(hit.point), hit.normal));
                    lineRenderer.positionCount = dashPoints.Count + 1;
                    lineRenderer.SetPosition(0, transform.position);
                    for (int i = 1; i < lineRenderer.positionCount; i++)
                    {
                        lineRenderer.SetPosition(i, dashPoints[i - 1].position);

                    }
                }
            }
        }
    }

    void DebugDrawDashPoints()
    {
        if (dashPoints.Count > 0)
        {
            Debug.DrawLine(dashPoints[0].position, transform.position, Color.green);
            Debug.DrawRay(dashPoints[0].position, dashPoints[0].normal, Color.cyan);

            for (int i = 0; i < dashPoints.Count - 1; i++)
            {

                Debug.DrawLine(dashPoints[i].position, dashPoints[i + 1].position, Color.green);
                Debug.DrawRay(dashPoints[i + 1].position, dashPoints[i + 1].normal, Color.cyan);
            }
        }
        
    }

    void DebugDrawSurroundingTiles()
    {
        Debug.DrawLine(Snap(transform.position + transform.forward + transform.right * 0.5f), Snap(transform.position + transform.forward - transform.right * 0.5f), Color.magenta);
        Debug.DrawLine(Snap(transform.position + transform.forward - transform.right * 0.5f), Snap(transform.position - transform.forward - transform.right * 0.5f), Color.magenta);
        Debug.DrawLine(Snap(transform.position - transform.forward - transform.right * 0.5f), Snap(transform.position - transform.forward + transform.right * 0.5f), Color.magenta);
        Debug.DrawLine(Snap(transform.position - transform.forward + transform.right * 0.5f), Snap(transform.position + transform.forward + transform.right * 0.5f), Color.magenta);
    }

    public void Dash()
    {
        if (DashCoroutine == null && dashPoints.Count > 0)
        {
            TimeManager.Instance.NormalSpeed();
            canvas.enabled = false;
            isPaused = false;
            DashCoroutine = DashRoutine();
            StartCoroutine(DashCoroutine);
        }
    }

    public Vector3 Snap(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            Mathf.Round(position.y),
            Mathf.Round(position.z)
            //Mathf.Round(position.x / 0.5f) * 0.5f,
            //Mathf.Round(position.y / 0.5f) * 0.5f,
            //Mathf.Round(position.z / 0.5f) * 0.5f

            );
    }

    IEnumerator DashRoutine()
    {
        //Camera.main.transform.parent = transform;

        //Transform cube = Camera.main.GetComponent<CameraController>().cube;

        //rotate camera in order to get the player cube on centered and on top of the screen

        //Vector3 camFromPosition = Camera.main.transform.position;
        //Vector3 camToPostiton = (transform.position - Camera.main.transform.position).normalized * Vector3.Distance(Camera.main.transform.position, Vector3.zero);// camFromPosition + (cube.position - camFromPosition).normalized* 20f;//(transform.position - camFromPosition).magnitude;
        ////transform.position.normalized + 

        //Vector3 camFromRotation = Camera.main.transform.rotation.eulerAngles;

        //// move camera to destination and look at player to get rotation angle
        //Camera.main.transform.position = camToPostiton;
        //Camera.main.transform.LookAt(transform.position);
        ////Camera.main.transform.Rotate(transform.forward, 90);
        //Vector3 camToRotation = Camera.main.transform.rotation.eulerAngles;

        ////return to fromPosition
        //Camera.main.transform.position = camFromPosition;
        //Camera.main.transform.rotation = Quaternion.Euler(camFromRotation);

        //for (float t = 0; t < camTransitionDuration; t += Time.deltaTime)
        //{
        //    Camera.main.transform.position = Vector3.Lerp(camFromPosition, camToPostiton, t / camTransitionDuration);
        //    Camera.main.transform.rotation = Quaternion.Lerp(
        //                                        Quaternion.Euler(camFromRotation),
        //                                        Quaternion.Euler(camToRotation),
        //                                        t / camTransitionDuration
        //                                        );

        //    yield return null;
        //}
        lineRenderer.positionCount = 0;

        for (int i = 0; i < dashPoints.Count; i++)
        {
            float duration;
            if (i == 0)
                duration = Vector3.Distance(dashPoints[0].position, transform.position) * 0.025f;
            else
                duration = Vector3.Distance(dashPoints[i].position, dashPoints[i - 1].position) * 0.025f;

            //float turnRotation = 0.25f;
            //Quaternion fromRotation = transform.rotation;
            //Quaternion toRotation = Quaternion.LookRotation(Quaternion.AngleAxis(90, transform.forward) * Vector3.Cross((dashPoints[i].position - transform.position).normalized, transform.position));
            //for (float t = 0; t < turnRotation; t += Time.deltaTime)
            //{
            //    transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t / turnRotation);
            //    yield return null;
            //}
            Vector3 fromPos = transform.position;
            Vector3 fromUp = transform.up;
            //Vector3 fromRotation = transform.eulerAngles;
            //Vector3 toRotation = (dashPoints[i].position-transform.position);

            //Quaternion fromRotation = transform.rotation;
            //Quaternion toRotation = Quaternion.Angle(transform.rotation, )


            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(fromPos, dashPoints[i].position, t / duration);
                //transform.up = Vector3.Lerp(fromUp, dashPoints[i].normal, t / duration);
                //transform.eulerAngles = Vector3.Lerp(fromRotation, toRotation, t / duration);
                //transform.rotation = Quaternion.Lerp( t / duration);
                transform.LookAt(dashPoints[i].position);

                yield return null;
            }
        }

        //set to absolut position
        transform.position = dashPoints[dashPoints.Count - 1].position;
        transform.up = dashPoints[dashPoints.Count - 1].normal;

        dashPoints.Clear();


        ////lerp back to old position 
        ////TODO: Change!!
        //camToPostiton = Camera.main.transform.position;
        //camToRotation = Camera.main.transform.rotation.eulerAngles;
        //for (float t = 0; t < camTransitionDuration; t += Time.deltaTime)
        //{
        //    Camera.main.transform.position = Vector3.Lerp(camToPostiton, camFromPosition, t / camTransitionDuration);
        //    Camera.main.transform.rotation = Quaternion.Lerp(
        //                                        Quaternion.Euler(camToRotation),
        //                                        Quaternion.Euler(camFromRotation),
        //                                        t / camTransitionDuration
        //                                        );

        //    yield return null;
        //}


        //Camera.main.transform.parent = null;

        TimeManager.Instance.DoSlowmotion();
        isPaused = true;
        canvas.enabled = true;
        DashCoroutine = null;


    }

    #endregion
}
