using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {
    public static PlayerController2 Instance;

    public List<DashPoint> dashPoints = new List<DashPoint>();
    public List<DashPoint> plannedPoints = new List<DashPoint>();
    public Phase phase = Phase.WaitingForInput;

    Vector3 lastMousePosition;

    public enum Phase
    {
        WaitingForInput,
        Planning,
        Dashing
    }


    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, 1024, LayerMask.NameToLayer("WalkableCube")))
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(Camera.main.transform.position, (hit.point-Camera.main.transform.position), Color.grey);
                Vector3 line = Vector3.Cross((hit.point - transform.position).normalized,transform.up);
                Debug.DrawRay(transform.position,line, Color.green);
                line = Quaternion.AngleAxis(90, transform.up) * line;
                RaycastHit hitInfo2;
                if(Physics.Raycast(transform.position, line,out hitInfo2))
                {
                    float dist = Vector3.Distance(hitInfo2.point, transform.position);
                    Debug.DrawRay(transform.position, line * dist, Color.green, 2f, true);
                    for(float d = 0; d < dist; d += 0.5f)
                    {
                        RaycastHit hit3;
                        if(!Physics.Raycast(transform.position + line * d,-transform.up, out hit3,1))
                        {
                            Debug.DrawRay(transform.position + line * d,  -transform.up, Color.magenta, 1.5f);
                            break;
                        }
                    }
                }
               // Debug.DrawLine(transform.position, Vector3.Cross((hit.point - transform.position).normalized, transform.up), Color.cyan,2f, true);
            }
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    //if (Physics.Raycast(ray, out hit, 1024, LayerMask.NameToLayer("WalkableCube")))
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        dashPoints = DashPath(transform.position, transform.up, hit.transform.GetComponent<MeshRenderer>().bounds.ClosestPoint(hit.point), hit.normal);
        //    }
        //}
        /*switch (phase)
        {
            //game is waiting for player input, player can place dash points by clicking and orbit with the camera
            case Phase.WaitingForInput:
                //Touch.Began
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    //if (Physics.Raycast(ray, out hit, 1024, LayerMask.NameToLayer("WalkableCube")))
                    if(Physics.Raycast(ray,out hit))
                    {
                        //is this the initial dash point?
                        if (dashPoints.Count == 0)
                        {
                            plannedPoints = DashPath(transform.position, transform.up, hit.point, hit.normal);
                        }
                        else
                        {
                            plannedPoints = DashPath(dashPoints[dashPoints.Count - 1].position, dashPoints[dashPoints.Count - 1].up, hit.point, hit.normal);
                        }

                        lastMousePosition = Input.mousePosition;

                        phase = Phase.Planning;
                    }

                }
                break;

            //Planning means the initial dash point has already been placed
            case Phase.Planning:
                //Touch.Moved 
                if (Input.GetMouseButton(0) && Input.mousePosition != lastMousePosition)
                {
                    lastMousePosition = Input.mousePosition;
                    Debug.Log("Mouse Moved");
                    //update the current dash position the user wants to place
                }
                //Touch.Ended / Released
                if (Input.GetMouseButtonUp(0))
                {
                    //if (dashPoints.Count == 0)
                    //{
                    //    dashPoints = plannedPoints;
                    //}
                    //else
                    //{
                        foreach (DashPoint plannedPoint in plannedPoints)
                            dashPoints.Add(plannedPoint);
                    //}
                }
                break;

            case Phase.Dashing:

                break;
        }*/



    }

    #endregion



    List<DashPoint> DashPath(Vector3 fromPos, Vector3 fromUp, Vector3 toPos, Vector3 toUp)
    {


        ////Check if FromPos and ToPos are grounded on the same area
        //RaycastHit fromHit;
        //RaycastHit toHit;
        //if(Physics.Raycast(fromPos+fromUp*0.5f, -fromUp, out fromHit))
        //{
        //    if(Physics.Raycast(toPos+toUp*0.5f, -toUp, out toHit))
        //    {
        //        if (fromHit.normal == toHit.normal)
        //        {
        //            Debug.Log("fromHit.transform == toHit.transform");
        //        }
        //        else
        //            Debug.Log("fromHit.transform != toHit.transform");
        //        CompareHeight(fromPos, fromUp, toPos, toUp);
        //    }
        //}
        


        return null;
    }

    bool CompareHeight(Vector3 fromPosition, Vector3 fromUp, Vector3 toPosition, Vector3 toUp)
    {
        Debug.Log(Vector3.Dot(fromUp, toUp));


        return false;
    }
}
