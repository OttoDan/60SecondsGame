using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    #region Private Fields

    public List<DashPoint> dashPoints = new List<DashPoint>();

    public List<Vector3> dashPointsPosition = new List<Vector3>();
    public List<Vector3> dashPointsUp = new List<Vector3>();

    LineRenderer lineRenderer;

    IEnumerator DashCoroutine;

    #endregion

    #region Public Fields

    public static PlayerController Instance;

    public static float snapX = 0.5f;
    public static float snapY = 0.5f;
    public static float snapZ = 0.5f;

    public bool isPaused;

    public Transform lastDashPoint;

    public Canvas canvas;

    public float camTransitionDuration = 0.75f;

    #endregion

    #region Test Fields
    Vector3 lastClick;
    Vector3 lastNormal;
    Vector3 lastNormalPos;
    Vector3 lastForward;
    float lastDirMag;
    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        lineRenderer = GetComponent<LineRenderer>();
        if (lastDashPoint == null)
            Debug.LogError("Last Dash Point not set!");

        lastDashPoint.position = transform.position;
        lastDashPoint.rotation = transform.rotation;

        isPaused = true;
    }
    private void Update()
    {
        if (isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {

                    //if(lineRenderer.positionCount == 0)
                    //{
                    //    lineRenderer.positionCount = 1;
                    //    lineRenderer.SetPosition(0,transform.position);
                    //}

                    List<DashPoint> points = CalculateDashPath(hit.point);
                    if (points != null)
                    {
                        //int i = lineRenderer.positionCount;
                        //lineRenderer.positionCount = lineRenderer.positionCount + points.Count;

                        foreach (DashPoint point in points)
                        {
                            dashPointsPosition.Add(point.position);
                            dashPointsUp.Add(point.normal);
                            //i++;
                        }

                        lineRenderer.positionCount = dashPointsPosition.Count + 1;
                        lineRenderer.SetPosition(0, transform.position);
                        for (int i = 1; i < lineRenderer.positionCount; i++)
                        {
                            lineRenderer.SetPosition(i, dashPointsPosition[i - 1]);

                        }
                    }

                }
            }
        }
        

        Debug.DrawLine(lastDashPoint.position, lastClick,Color.cyan);
        Debug.DrawLine(lastDashPoint.position, lastDashPoint.position + lastForward * lastDirMag, Color.blue);

    }
    #endregion

    #region Methods
    
    List<DashPoint> CalculateDashPath(Vector3 toPosition)
    {
        List<DashPoint> dashPoints;
        List<Vector3> pointsPosition;
        List<Vector3> pointsForward;
        List<Vector3> pointsUp;
        
        lastClick = toPosition;
        Vector3 direction;
        float directionMagnitude;
        Vector3 normal;
        Vector3 forward;

        direction = (toPosition - lastDashPoint.position).normalized;
        Debug.LogFormat("ToPos: {0} - FromPos: {1} = Direction: {2}", toPosition, lastDashPoint.position,direction);

        RaycastHit hit;
        if (Physics.Raycast(lastDashPoint.position+lastDashPoint.up, direction, out hit))
        {
            pointsPosition = new List<Vector3>();
            pointsForward = new List<Vector3>();
            pointsUp = new List<Vector3>();

            normal = hit.normal;
            Debug.Log("Normal: " + normal + " Normalized normal: " + normal.normalized);

            //Check for opposite cube side case
            if(new Vector3(Mathf.Round(normal.x), Mathf.Round(normal.y), Mathf.Round(normal.z)) == lastDashPoint.up ||
               new Vector3(Mathf.Round(normal.x), Mathf.Round(normal.y), Mathf.Round(normal.z)) == -lastDashPoint.up)
            {
                Debug.Log("its tha opposite case");
            }

            forward = Vector3.Cross(direction, normal);
            //rotate forward by 90 degrees
            forward = Quaternion.AngleAxis(90, lastDashPoint.up) * forward;

            Debug.Log("Forward: " + forward + " Normalized forward: " + forward.normalized);
            lastForward = forward;

            directionMagnitude = (toPosition - lastDashPoint.position).magnitude;
            Debug.Log("Direction Magnitude: " + directionMagnitude);
            lastDirMag = directionMagnitude;

            pointsPosition.Add(lastDashPoint.position + forward * directionMagnitude + forward * 0.5f);
            pointsUp.Add(lastDashPoint.up);
            pointsPosition.Add(toPosition + forward * 0.5f);
            pointsUp.Add(forward);

            //get Normal Vector from to Position
            lastDashPoint.position = toPosition;
            lastDashPoint.up = forward;

            //RaycastHit normalHit;
            //if (Physics.Raycast(toPosition + direction, -direction, out normalHit))
            //{
            //    lastDashPoint.position = toPosition;
            //    lastDashPoint.up = normalHit.normal;
                
            //    Debug.Log(lastDashPoint.up);
            //}
            //else
            //    Debug.LogError("Could not hit surface of target cube side!");

            //TODO: Consider the cube side underneath the player. 
            
            
            //Snap points to grid:
            for(int i=0; i<pointsPosition.Count; i++)
            {
                pointsPosition[i] = new Vector3(
                    Mathf.Round(pointsPosition[i].x / snapX * snapX),
                    Mathf.Round(pointsPosition[i].y / snapY * snapY),
                    Mathf.Round(pointsPosition[i].z / snapZ * snapZ)
                );
            }

            if (pointsPosition.Count != pointsUp.Count)
                Debug.LogError("(pointsPosition.Count != pointsUp.Count)");
            else
            {
                dashPoints = new List<DashPoint>();
                for(int i=0; i<pointsPosition.Count; i++)
                {
                    dashPoints.Add(new DashPoint(pointsPosition[i], pointsUp[i]));
                }
                return dashPoints;
            }

        }
        else
        {
            dashPoints = new List<DashPoint>();

            dashPoints.Add(new DashPoint(toPosition, lastDashPoint.up));


            //get Normal Vector from to Position
            lastDashPoint.position = toPosition;
            Debug.Log("keine kante");
            return dashPoints;
        }
        return null;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="toPosition"></param>
    /// <returns></returns>
    List<DashPoint> CalculateDashPath2(Vector3 fromPosition, Vector3 toPosition)
    {
        return null;
    }

    //{
    //    Vector3 direction;
    //    float directionMagnitude;
    //    Vector3 normal;
    //    Vector3 forward;

    //    direction = (toPosition - lastDashPoint.position).normalized;

        
    //    Debug.Log("Direction: " + (toPosition - lastDashPoint.position) + " Normalized: " + direction + " Rounded: " + new Vector3(Mathf.Round(direction.x), Mathf.Round(direction.y), Mathf.Round(direction.z)));

    //    //get the normal of the cube and figure out something is in the way

    //    RaycastHit hit;

    //    if(Physics.Raycast(lastDashPoint.position, direction, out hit, Vector3.Distance(lastDashPoint.position,toPosition)))
    //    {
    //        Debug.Log("Hit.point: " + hit.point);
    //        Debug.Log("Hit.normal: " + hit.normal);
    //    }


       

    //    return null;

    //}

    public void Dash()
    {
        if (DashCoroutine == null && dashPointsPosition.Count > 0)
        {
            canvas.enabled = false;
            isPaused = false;
            DashCoroutine = DashRoutine();
            StartCoroutine(DashCoroutine);
        }
    }
    IEnumerator DashRoutine()
    {
        //Camera.main.transform.parent = transform;

        ////Transform cube = Camera.main.GetComponent<CameraController>().cube;

        ////rotate camera in order to get the player cube on centered and on top of the screen

        //Vector3 camFromPosition = Camera.main.transform.position;
        //Vector3 camToPostiton = (transform.position-Camera.main.transform.position).normalized * Vector3.Distance(Camera.main.transform.position, Vector3.zero);// camFromPosition + (cube.position - camFromPosition).normalized* 20f;//(transform.position - camFromPosition).magnitude;
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

        //for (float t =0; t < camTransitionDuration; t += Time.deltaTime)
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
        for (int i = 0; i < dashPointsPosition.Count; i++)
        {
            float duration;
            if (i == 0)
                duration = Vector3.Distance(dashPointsPosition[0], transform.position) * 0.025f;
            else
                duration = Vector3.Distance(dashPointsPosition[i], dashPointsPosition[i - 1]) * 0.025f;

            Vector3 fromPos = transform.position;
            Vector3 fromUp = transform.up;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(fromPos, dashPointsPosition[i], t / duration);
                transform.up = Vector3.Lerp(fromUp, dashPointsUp[i], t / duration);
                yield return null;
            }
        }
        dashPointsPosition.Clear();


        ////lerp back to old position 
        ////TODO: Change!!
        //camToPostiton = Camera.main.transform.position;
        //camToRotation = Camera.main.transform.rotation.eulerAngles;
        //for (float t = 0; t < camTransitionDuration; t += Time.deltaTime)
        //{
        //    Camera.main.transform.position = Vector3.Lerp(camToPostiton, camFromPosition,t / camTransitionDuration);
        //    Camera.main.transform.rotation = Quaternion.Lerp(
        //                                        Quaternion.Euler(camToRotation),
        //                                        Quaternion.Euler(camFromRotation),
        //                                        t / camTransitionDuration
        //                                        );

        //    yield return null;
        //}
        //Camera.main.transform.parent = null ;

        isPaused = true;
        canvas.enabled = true;
        DashCoroutine = null;


    }

    #endregion
}

public class DashPoint
{
    public Vector3 position;
    public Vector3 normal;

    public DashPoint(Vector3 position, Vector3 normal)
    {
        this.position = position;
        this.normal = normal;
    }
}