using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    #region Private Fields

    public List<Vector3> dashPoints = new List<Vector3>();

    LineRenderer lineRenderer;

    IEnumerator DashCoroutine;

    #endregion

    #region Public Fields

    public static PlayerController Instance;

    public bool isPaused;

    public Transform lastDashPoint;

    public Canvas canvas;

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

                    List<Vector3> points = CalculateDashPath(hit.point);
                    if (points != null)
                    {
                        //int i = lineRenderer.positionCount;
                        //lineRenderer.positionCount = lineRenderer.positionCount + points.Count;

                        foreach (Vector3 point in points)
                        {
                            dashPoints.Add(point);
                            //i++;
                        }

                        lineRenderer.positionCount = dashPoints.Count + 1;
                        lineRenderer.SetPosition(0, transform.position);
                        for (int i = 1; i < lineRenderer.positionCount; i++)
                        {
                            lineRenderer.SetPosition(i, dashPoints[i - 1]);

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
    
    List<Vector3> CalculateDashPath(Vector3 toPosition)
    {
        List<Vector3> points;
        
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
            points = new List<Vector3>();
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

            points.Add(lastDashPoint.position + forward * directionMagnitude + forward * 0.5f);
            points.Add(toPosition + forward * 0.5f);

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

            return points;
        }
        else
        {
            points = new List<Vector3>();
            points.Add(toPosition);


            //get Normal Vector from to Position
            lastDashPoint.position = toPosition;
            Debug.Log("keine kante");
        }

        return points;
    }

    public void Dash()
    {
        if (DashCoroutine == null && dashPoints.Count > 0)
        {
            canvas.enabled = false;
            isPaused = false;
            Camera.main.transform.parent = transform;
            DashCoroutine = DashRoutine();
            StartCoroutine(DashCoroutine);
        }
    }
    IEnumerator DashRoutine()
    {
        lineRenderer.positionCount = 0;
        for (int i = 0; i < dashPoints.Count; i++)
        {
            float duration;
            if (i == 0)
                duration = Vector3.Distance(dashPoints[0], transform.position) * 0.125f;
            else
                duration = Vector3.Distance(dashPoints[i], dashPoints[i - 1]) * 0.125f;

            Vector3 fromPos = transform.position;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(fromPos, dashPoints[i], t / duration);
                yield return null;
            }
        }
        dashPoints.Clear();
        isPaused = true;
        canvas.enabled = true;
        DashCoroutine = null;


    }

    #endregion
}
