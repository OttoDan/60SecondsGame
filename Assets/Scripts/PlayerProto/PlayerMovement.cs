using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


    #region Private Fields

    List<Vector3> dashPoints;
    IEnumerator dashPathCoroutine;
    LineRenderer lineRenderer;
    private Phase phase = Phase.Dash;


    #endregion

    #region Public Fields

    public enum Phase
    {
        Planning,
        Dash
    }

    public GameObject dashPointPrefab;
    public Transform dashCalc;

    #endregion

    #region Planning Phase

    #endregion

    #region Dash Phase

    #endregion

    #region Unity Messages
    private void Awake()
    {
        dashPoints = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
    }
    Vector3 toPos = Vector3.zero;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("CubeGround"))
                {
                    DashRoute(hitInfo.point+hitInfo.normal * 0.5f);
                    transform.up = hitInfo.normal;
                    toPos = hitInfo.point + hitInfo.normal * 0.5f;
                    //lineRenderer.positionCount = dashPoints.Count;
                    //lineRenderer.SetPositions(dashPoints.ToArray());
                    //Debug.Log(hitInfo.point);
                    //Instantiate(dashPointPrefab, hitInfo.point, Quaternion.identity, transform);
                }
            }
        }

        Debug.DrawLine(transform.position, toPos);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitInfo;
        //    ////Raycast from Mouse pos on screen / Touch pos on screen towards the cube 
        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        Debug.Log(hitInfo.point);
        //        RaycastHit forwardHitInfo;

        //        if(dashPoints.Count==0)
        //            dashPoints.Add(
        //                Instantiate(dashPointPrefab, hitInfo.point, Quaternion.Euler(
        //                    (hitInfo.point - transform.position).normalized
        //                ), transform).transform
        //            );

        //        else
        //        {
        //            //Raycast from Last DashPos towards new pos
        //            if (!Physics.Raycast(dashPoints[dashPoints.Count - 1].position, hitInfo.point, out forwardHitInfo))
        //            {
        //                //nothing hit? Position is reachable and valid
        //                dashPoints.Add(
        //                    Instantiate(dashPointPrefab, hitInfo.point, Quaternion.Euler(
        //                        (hitInfo.point - dashPoints[dashPoints.Count-1].position).normalized
        //                    ), transform).transform
        //                );

                        

        //            }
        //            else
        //            {
        //                //something in the way, position is not reachable 

        //                int scanSteps = 1;

        //                RaycastHit downwardHitInfo;
        //                while (Physics.Raycast(forwardHitInfo.point, -transform.up + transform.forward * scanSteps, out downwardHitInfo))
        //                {
        //                    //shoot raycasts downwards as long as you are grounded
        //                    scanSteps++;

        //                }
        //                //reached a place with space underneath?
        //                //place a dashpoint
        //                dashPoints.Add(
        //                    Instantiate(dashPointPrefab, forwardHitInfo.point + transform.forward * scanSteps, Quaternion.Euler(
        //                        (hitInfo.point - dashPoints[dashPoints.Count-1].position).normalized
        //                    ), transform).transform
        //                );

        //                //(turn around by 90 degrees on the x)
        //                //shoot a ray in the direction of the click/touch point
        //                if (!Physics.Raycast(hitInfo.point, transform.forward * 1f, out forwardHitInfo))
        //                {
        //                    //nothing hit? Position is reachable and valid
        //                    dashPoints.Add(
        //                        Instantiate(dashPointPrefab, hitInfo.point, Quaternion.identity, transform).transform
        //                    );
        //                }
        //            }


                    
        //        }

                

        //    }
        //}


        
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);

        Debug.DrawLine(transform.position, transform.position - transform.up, Color.green);
    }
    #endregion

    #region Coroutines

    void DashRoute(Vector3 toPos)
    {
        // Down: Grounded
        // Forward: free
        //-- 1 Step Forward


        // Down: Free
        // Forward: Free
        //--Rotate 90 degrees forward

        // Down: Grounded
        // forward: Blocked
        //--Rotate -90 degrees forward 

        float dist = Vector3.Distance(transform.position, toPos);

        RaycastHit hit;

        if(Physics.Raycast(transform.position,(toPos-transform.position).normalized, out hit, dist))
        {
            Debug.Log("Something in the way");
        }
        Debug.Log("clear");
        transform.position = toPos;

    }

    #endregion
}

