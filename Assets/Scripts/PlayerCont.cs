using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour {

    IEnumerator DashCoroutine;
    public Canvas canvas;
    public bool isPaused = true;
    public float camTransitionDuration = 1.5f;

    public List<DashPoint> dashPoints = new List<DashPoint>();
    public static float snap = 0.5f;
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(SnapToGrid(hit.point), SnapToGrid(hit.point)+hit.normal,Color.magenta);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                CalculateDashPath(hit.point, hit.normal);//CalculateDashPath(SnapToGrid(hit.point), hit.normal);
            }
        }

        if (dashPoints.Count > 0)
        {
            Debug.DrawLine(dashPoints[0].position, transform.position, Color.green);
            Debug.DrawRay(dashPoints[0].position, dashPoints[0].normal, Color.cyan);

        }
        for(int i = 0; i < dashPoints.Count-1; i++)
        {
            
            Debug.DrawLine(dashPoints[i].position, dashPoints[i+1].position, Color.green);
            Debug.DrawRay(dashPoints[i+1].position, dashPoints[i+1].normal, Color.cyan);
        }

    }
    public Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(Mathf.Round(position.x),Mathf.Round(position.y),Mathf.Round(position.z));
        //return new Vector3(Mathf.Round(position.x/snap)*snap,Mathf.Round(position.y/snap)*snap,Mathf.Round(position.z/snap)*snap);
    }

    void CalculateDash(Vector3 point, Vector3 normal)
    {
        /* Current
        Vector3 fromPosition;
        Vector3 fromNormal;
        Vector3 direction;
        Vector3 orthogonalDirection;

        float directionMagnitude;

        if (dashPoints.Count > 0)
        {
            fromPosition = dashPoints[dashPoints.Count - 1].position;
            fromNormal = dashPoints[dashPoints.Count - 1].normal;
        }
        else
        {
            fromPosition = transform.position;
            fromNormal = transform.up;
        }

        direction = (point - fromPosition).normalized;
        directionMagnitude = (point - fromPosition).magnitude;

        orthogonalDirection = Quaternion.AngleAxis(90, fromNormal) * Vector3.Cross(direction, fromNormal);



        //We need to consider validation of dash points

        //Rules:
        // We can only overpass obstacles which lay within the direct path. 
        // We can only dash to points which are reachable by moving just forward (and overpassing obstacles in between) 

        //VALIDATE DASHPOINT

        /* check if the path is reachable directly / if the entire path is grounded
         * 
         *

        //First Check for the amount of obstacles in the way

        RaycastHit orthoHit;

        if (Physics.Raycast(fromPosition, orthogonalDirection, out orthoHit, directionMagnitude))
        {
            
        }
        //no obstacle in the orthogonal
        else
        {

            //fromPos and point share orientation
            if(normal == fromNormal)
            {
                float dist = Vector3.Distance(fromPosition, fromPosition + orthogonalDirection * directionMagnitude);
                bool grounded;
                int groundedSteps=0;
                int nonGroundedSteps=0;
                for(float s = 0; s < dist; s+= snap)
                {
                    RaycastHit groundHit;
                    if(Physics.Raycast(fromPosition + orthogonalDirection * s, -fromNormal, out groundHit, snap * 2))
                    {
                        grounded = true;
                        groundedSteps++;
                    }
                    else
                    {
                        grounded = false;
                        //here we continue looping to validate our target pos
                        if (Physics.Raycast(fromPosition + orthogonalDirection * s, -fromNormal, out groundHit, snap * 2))
                            nonGroundedSteps++;
                    }
                }


            }
            //different orientation
            else
            {

            }
        }


        */













        Debug.Log("calculate Dash");
        Vector3 fromPosition;
        Vector3 fromNormal;
        Vector3 direction;
        Vector3 orthogonalDirection;

        float heightDiff;
        float directionMagnitude;

        if (dashPoints.Count > 0)
        {
            fromPosition = dashPoints[dashPoints.Count - 1].position;
            fromNormal = dashPoints[dashPoints.Count - 1].normal;
        }
        else
        {
            fromPosition = transform.position;
            fromNormal = transform.up;
        }

        direction = (point - fromPosition).normalized;
        directionMagnitude = (point - fromPosition).magnitude;

        orthogonalDirection = Quaternion.AngleAxis(90, fromNormal) * Vector3.Cross(direction, fromNormal);

        RaycastHit hit;
        if (Physics.Raycast(fromPosition, orthogonalDirection, out hit, directionMagnitude))
        {
            float dist = Vector3.Distance(fromPosition, hit.point);
            RaycastHit groundHit;

            bool grounded = true;
            Vector3 lastGroundedPos = fromPosition;
            Vector3 nonGroundedNormal = fromNormal;

            for(float s=0; s<dist; s+= snap)
            {
                if (Physics.Raycast(fromPosition + orthogonalDirection * s, -fromNormal, out groundHit, snap * 2))
                {
                    lastGroundedPos = groundHit.point + groundHit.normal * snap;
                    grounded = true;
                }
                else
                {
                    RaycastHit nonGroundedHit;
                    if (Physics.Raycast(fromPosition + orthogonalDirection * s - fromNormal * snap * 2, -orthogonalDirection, out nonGroundedHit, snap * 2))
                    {
                        nonGroundedNormal = nonGroundedHit.normal;
                    }
                    grounded = false;
                    break;
                }
            }

            //All the way towards the obstacle is walkable
            if (grounded)
            {
                dashPoints.Add(new DashPoint(hit.point - orthogonalDirection * snap, hit.normal));
                CalculateDash(point, normal);
                return;
            }
            //there is a gap in between fromPos and the obstacle
            else
            {
                dashPoints.Add(new DashPoint(lastGroundedPos + orthogonalDirection * snap, nonGroundedNormal));
                CalculateDash(point, normal);
                return;
            }
        }
        //No obstacle in the way
        else
        {
            //This could be either obstacle free way to the point or different orientations!
            if (normal == fromNormal)
            {
                float dist = Vector3.Distance(fromPosition, point);
                RaycastHit groundHit;

                bool grounded = true;
                Vector3 lastGroundedPos = fromPosition;
                Vector3 nonGroundedNormal = fromNormal;

                for (float s = 0; s < dist; s += snap)
                {
                    if (Physics.Raycast(fromPosition + orthogonalDirection * s, -fromNormal, out groundHit, snap * 2))
                    {
                        lastGroundedPos = groundHit.point + fromNormal * snap;
                        grounded = true;
                    }
                    //reached non walkable space
                    //!(attention: this could be an indice for an invalid dash point!)
                    else
                    {
                        RaycastHit nonGroundedHit;
                        if (Physics.Raycast(fromPosition + orthogonalDirection * s - fromNormal * snap * 2, -orthogonalDirection, out nonGroundedHit, snap * 2))
                        {
                            nonGroundedNormal = nonGroundedHit.normal;
                        }
                        grounded = false;
                        break;
                    }
                }
                //All the way towards the orthogonal border point is walkable
                if (grounded)
                {
                    dashPoints.Add(new DashPoint(point + fromNormal * snap, fromNormal));
                    //if(Vector3.Distance(point + fromUp * snap, point)<snap) 
                    return;
                }
                //there is a gap in between fromPos and the point
                //!(attention: this could be an indice for an invalid dash point!)
                else
                {
                    dashPoints.Add(new DashPoint(lastGroundedPos + orthogonalDirection * snap, nonGroundedNormal));
                    CalculateDash(point, normal);
                    return;
                }
            }
            //Different Orientations!
            else
            {
                float dist = Vector3.Distance(fromPosition, fromPosition+orthogonalDirection*directionMagnitude);
                RaycastHit groundHit;

                bool grounded = true;
                Vector3 lastGroundedPos = fromPosition;
                Vector3 nonGroundedNormal = fromNormal;

                for (float s = 0; s < dist; s += snap)
                {
                    if (Physics.Raycast(fromPosition + orthogonalDirection * s, -fromNormal, out groundHit, snap * 2))
                    {
                        lastGroundedPos = groundHit.point + fromNormal * snap;
                        grounded = true;
                    }
                    //reached non walkable space
                    //!(attention: this could be an indice for an invalid dash point!)
                    else
                    {
                        RaycastHit nonGroundedHit;
                        if (Physics.Raycast(fromPosition + orthogonalDirection * s - fromNormal * snap * 2, -orthogonalDirection, out nonGroundedHit, snap * 2))
                        {
                            nonGroundedNormal = nonGroundedHit.normal;
                        }
                        grounded = false;
                        break;
                    }
                }
                //All the way towards the orthogonal point is walkable
                if (grounded)
                {
                    dashPoints.Add(new DashPoint(point + fromNormal * snap, fromNormal));
                    //if(Vector3.Distance(point + fromUp * snap, point)<snap)
                    if (fromNormal != normal)
                        CalculateDash(point, normal);
                    return;
                }
                //there is a gap in between fromPos and the point
                //!(attention: this could be an indice for an invalid dash point!)
                else
                {
                    dashPoints.Add(new DashPoint(lastGroundedPos + orthogonalDirection * snap, nonGroundedNormal));
                    CalculateDash(point, normal);
                    return;
                }
            }
        }


        ///OLD
        /*
        dist = Vector3.Distance(fromPosition, point);

        RaycastHit forwardHit;

        //check if there is something in the way (using the crossproduct rotated by 90 degrees not direction)

        if (!Physics.Raycast(fromPosition, orthogonalDirection, out forwardHit, dist))
        {
            RaycastHit downHit;
            bool grounded = false;
            //check for every step if we are grounded 
            for (float s = 0; s < dist; s += snap)
            {
                if (Physics.Raycast(fromPosition + direction * s, -fromUp, out downHit, snap * 2))
                    grounded = true;
                else
                {
                    grounded = false;
                    break;
                }
            }

            //we can directly walk to our point without falling - yeh!
            if (grounded)
            {
                dashPoints.Add(new DashPoint(point + normal * snap, normal));
                Debug.Log("dashPoints.Count: " + dashPoints.Count);
                return;
            }
            else
                Debug.LogError("Cant reach point as tile in front of it is not grounded");
        }
        //there is something in the way, climb it!
        */

    }
    void CalculateDashPath(Vector3 point, Vector3 normal)
    {
        Vector3 fromPosition;
        Vector3 fromUp;
        Vector3 direction;

        float heightDiff;
        float dist;

        if (dashPoints.Count > 0)
        {
            fromPosition = dashPoints[dashPoints.Count - 1].position;
            fromUp = dashPoints[dashPoints.Count - 1].normal;
        }
        else
        {
            fromPosition = transform.position;
            fromUp = transform.up;
        }

        direction = (point - fromPosition).normalized;
        dist = Vector3.Distance(fromPosition, point);

        //First find out in which axis the normal of our point is pointing and compare it with our fromPos
        if (normal == fromUp)
        {
            //ok we share orientation, now find out if theres any height difference
            heightDiff = CalculateHeightDifference(fromPosition, point, normal);
            Debug.Log(heightDiff);

            //Check if we are on the same height(with snapping deviation)
            if(Mathf.Abs(heightDiff) <= snap)
            {
                bool repeat = true;
                while (repeat)
                {
                    RaycastHit forwardHit;

                    //check if there is something in the way (using the crossproduct rotated by 90 degrees not direction)
                    Vector3 orthogonalDirection = Vector3.Cross(direction, normal);
                    orthogonalDirection = Quaternion.AngleAxis(90, normal) * orthogonalDirection;

                    if (!Physics.Raycast(fromPosition, orthogonalDirection, out forwardHit, dist))
                    {
                        RaycastHit downHit;
                        bool grounded = false;
                        //check for every step if we are grounded 
                        for(float s = 0; s < dist; s += snap)
                        {
                            if (Physics.Raycast(fromPosition + direction * s, -fromUp, out downHit, snap * 2))
                                grounded = true;
                            else
                            {
                                grounded = false;
                                break;
                            }
                        }

                        //we can directly walk to our point without falling - yeh!
                        if (grounded)
                        {
                            dashPoints.Add(new DashPoint(point +normal*snap, normal));
                            Debug.Log("dashPoints.Count: " + dashPoints.Count);
                            return;
                        }
                        else
                            Debug.LogError("Cant reach point as tile in front of it is not grounded");
                    }
                    //there is something in the way, climb it!
                    else
                    {
                        float obstDist = Vector3.Distance(fromPosition, forwardHit.point);
                        //check for every step till the obstacle if we are grounded
                        RaycastHit downHit;
                        Vector3 downHitPos = Vector3.zero;
                        Vector3 downHitNormal = Vector3.zero;

                        bool grounded = false;
                        //check for every step if we are grounded 
                        for (float s = 0; s < obstDist; s += snap)
                        {
                            if (Physics.Raycast(fromPosition + direction * s, -fromUp, out downHit, snap * 2))
                            {
                                grounded = true;
                                downHitPos = downHit.point;
                                downHitNormal = downHit.normal;
                            }
                            else
                            {
                                grounded = false;
                                break;
                            }
                        }

                        //we can directly walk to our obstacle hitpoint without falling - yeh!
                        //use the last downward Raycast hit point as a new Dashpoint
                        if (grounded)
                        {
                            dashPoints.Add(new DashPoint(downHitPos + downHitNormal * snap - direction * snap, normal));
                            Debug.Log("dashPoints.Count: " + dashPoints.Count);
                        }
                        else
                            Debug.LogError("Cant climb obstacle as there is no ground in front of it");

                        //now lets get to the highest point of the obstacle to place another dash point and climb it
                        bool forwardObstacle = true;
                        float steps = 0;
                        RaycastHit obstacleHit;
                        while (forwardObstacle)
                        {
                            steps++;
                            if (Physics.Raycast(downHitPos + downHitNormal * steps * snap, orthogonalDirection, out obstacleHit, snap * 2))
                                forwardObstacle = true;
                            else
                                forwardObstacle = false;

                        }

                        Vector3 obstacleTopStartPos = downHitPos + downHitNormal * steps * snap;

                        dashPoints.Add(new DashPoint(obstacleTopStartPos, downHitNormal));
                        Debug.Log("dashPoints.Count: " + dashPoints.Count);

                        //Now walk forward on top of the obstacle until we can go down again
                        bool downObstacle = true;
                        RaycastHit obstacleTopDownHit;
                        steps = 0; 
                        while (downObstacle)
                        {
                            steps++;
                            if (Physics.Raycast(obstacleTopStartPos + orthogonalDirection * steps * snap, -downHitNormal, out obstacleTopDownHit, snap * 2))
                                downObstacle = true;
                            else
                                downObstacle = false;

                        }

                        //Ok we found the spot to go down again
                        Vector3 obstacleTopEndPos = obstacleTopStartPos + orthogonalDirection * steps * snap;
                        dashPoints.Add(new DashPoint(obstacleTopEndPos, downHitNormal));
                        Debug.Log("dashPoints.Count: " + dashPoints.Count);

                        //now lets get back to the ground 
                        bool downwardsObstacle = false;
                        steps = 0;
                        RaycastHit obstacleBackwardHit;
                        while (!downwardsObstacle)
                        {
                            steps++;
                            if (Physics.Raycast(obstacleTopEndPos - downHitNormal * steps * snap, -downHitNormal, out obstacleBackwardHit, snap * 2))
                                downwardsObstacle = true;
                            else
                                downwardsObstacle = false;
                        }

                        //Ok we found the grounded spot after our obstacle
                        Vector3 obstacleLowEndPos = obstacleTopEndPos - downHitNormal * steps * snap;
                        dashPoints.Add(new DashPoint(obstacleLowEndPos, downHitNormal));
                        Debug.Log("dashPoints.Count: " + dashPoints.Count);

                        //Now Check if there is another obstacle 
                        RaycastHit lastHit;
                        if(!Physics.Raycast(obstacleLowEndPos, orthogonalDirection, out lastHit, Vector3.Distance(obstacleLowEndPos, point)))
                        {
                            dashPoints.Add(new DashPoint(point + downHitNormal*snap, downHitNormal));
                            Debug.Log("dashPoints.Count: " + dashPoints.Count);
                            return;
                        }
                        else
                        {
                            //TODO: is this safe? or will it cause crash and endless while loop?
                            CalculateDashPath(point + downHitNormal * snap, downHitNormal);
                            return;
                        }
                        
                    }
                }
            }
            //We are not on the same height with our point.
            //are we above the point? [(with snapping deviation)]
            else if (heightDiff > snap)
            {
                //lets find out where we need to walk forward before we can climb up 
                RaycastHit forwardHit;

                //check if there is something in the way (using the crossproduct rotated by 90 degrees not direction)
                Vector3 orthogonalDirection = Vector3.Cross(direction, normal);
                orthogonalDirection = Quaternion.AngleAxis(90, normal) * orthogonalDirection;

                if (Physics.Raycast(fromPosition, orthogonalDirection, out forwardHit, dist))
                {
                    float obstDist = Vector3.Distance(fromPosition, forwardHit.point);
                    //check for every step till the obstacle if we are grounded
                    RaycastHit downHit;
                    Vector3 downHitPos = Vector3.zero;
                    Vector3 downHitNormal = Vector3.zero;

                    bool grounded = false;
                    //check for every step if we are grounded 
                    for (float s = 0; s < obstDist; s += snap)
                    {
                        if (Physics.Raycast(fromPosition + orthogonalDirection * s, -fromUp, out downHit, snap * 2))
                        {
                            grounded = true;
                            downHitPos = downHit.point;
                            downHitNormal = downHit.normal;
                        }
                        else
                        {
                            grounded = false;
                            break;
                        }
                    }

                    //we can directly walk to our obstacle hitpoint without falling - yeh!
                    //use the last downward Raycast hit point as a new Dashpoint
                    if (grounded)
                    {
                        dashPoints.Add(new DashPoint(downHitPos + downHitNormal * snap - direction * snap, normal));
                        Debug.Log("dashPoints.Count: " + dashPoints.Count);
                    }
                    else
                        Debug.LogError("Cant climb obstacle as there is no ground in front of it");

                    //now lets get to the highest point of the obstacle to place another dash point and climb it
                    bool forwardObstacle = true;
                    float steps = 0;
                    RaycastHit obstacleHit;
                    while (forwardObstacle)
                    {
                        steps++;
                        if (Physics.Raycast(downHitPos + downHitNormal * steps * snap, orthogonalDirection, out obstacleHit, snap * 2))
                            forwardObstacle = true;
                        else
                            forwardObstacle = false;

                    }

                    Vector3 obstacleTopStartPos = downHitPos + downHitNormal * steps * snap;

                    dashPoints.Add(new DashPoint(obstacleTopStartPos, downHitNormal));
                    Debug.Log("dashPoints.Count: " + dashPoints.Count);

                    //Now walk forward on top of the obstacle until we can reach the point 
                    //TODO: need to have logic looped here to avoid the cases of going down / or up inbetween this point and the target point

                    float remainingDist = Vector3.Distance(obstacleTopStartPos, point);
                    RaycastHit hopefullyNothingInbetweenHit;
                    if(!Physics.Raycast(obstacleTopStartPos, orthogonalDirection, out hopefullyNothingInbetweenHit, remainingDist))
                    {
                        //Ok the way is clear to reach our target

                        //Are we now on the same height as our point?
                        if(Mathf.Abs(CalculateHeightDifference(obstacleTopStartPos,point, downHitNormal)) <= snap)
                        {
                            Vector3 obstacleTopEndPos = obstacleTopStartPos + orthogonalDirection * steps * snap;
                            dashPoints.Add(new DashPoint(obstacleTopEndPos, downHitNormal));
                            Debug.Log("dashPoints.Count: " + dashPoints.Count);
                            return;
                        }
                        
                    }
                    
                    
                }
            }
            //are we below the point? [(with snapping deviation)]
            else if (heightDiff < snap)
            {

            }

        }
        else
        {

        }



        /*

        RaycastHit hit;

        Vector3 fromPosition;
        Vector3 fromUp;
        Vector3 direction;

        if (dashPoints.Count > 0)
        {
            fromPosition = dashPoints[dashPoints.Count - 1].position;
            fromUp = dashPoints[dashPoints.Count - 1].up;
        }
        else
        {
            fromPosition = transform.position;
            fromUp = transform.up;
        }

        direction = (point - fromPosition).normalized;
        float steps = Vector3.Distance(fromPosition, point) / snap;

        //Go snapwise step by step and check if every step is valid and grounded!
        for(float s = 0; s < steps; s += snap)
        {
            RaycastHit forwardHit;
            //forward check
            if(Physics.Raycast(fromPosition, direction, out forwardHit, s * snap))
            {
                //direction change
                break;
            }

            RaycastHit downHit;

            //ground check
            if (Physics.Raycast(fromPosition + direction * s, -fromUp, out downHit, snap))
            {

            }
            //include step downwards
            else
            {

            }
        }


        /*

        //First: try to reach point on direct way 
        if (Physics.Raycast(fromPosition, direction, out hit))
        {
            //check if hit is close to our point
            if(Vector3.Distance(hit.point, point) < 1.0f)
            {
                dashPoints.Add(new DashPoint(hit.point, hit.normal));
            }


            //get normal from surface (up vector)
            RaycastHit normalHit;
            

            return;
        }
        else
        {

        }
        */
    }

    float CalculateHeightDifference(Vector3 fromPos, Vector3 toPos, Vector3 up)
    {
        //Since c#4 does not support switch case statements for Vector3s

        //X-Axis
        if (new Vector3(Mathf.Abs(up.x), up.y, up.z) == new Vector3(1, 0, 0))
        {
            return toPos.x - fromPos.x;
        }

        //Y-Axis
        if(new Vector3(up.x, Mathf.Abs(up.y), up.z) == new Vector3(0, 1, 0))
        {
            return toPos.y - fromPos.y;
        }

        //Z-Axis
        if (new Vector3(up.x, up.y, Mathf.Abs(up.z)) == new Vector3(0, 0, 1))
        {
            return toPos.z - fromPos.z;
        }


        Debug.LogError("Couldnt calculate height difference!");

        return 0;
    }
    
    public void Dash()
    {
        if (DashCoroutine == null && dashPoints.Count > 0)
        {
            canvas.enabled = false;
            isPaused = false;
            DashCoroutine = DashRoutine();
            StartCoroutine(DashCoroutine);
        }
    }
    IEnumerator DashRoutine()
    {
        Camera.main.transform.parent = transform;

        //Transform cube = Camera.main.GetComponent<CameraController>().cube;

        //rotate camera in order to get the player cube on centered and on top of the screen

        Vector3 camFromPosition = Camera.main.transform.position;
        Vector3 camToPostiton = (transform.position - Camera.main.transform.position).normalized * Vector3.Distance(Camera.main.transform.position, Vector3.zero);// camFromPosition + (cube.position - camFromPosition).normalized* 20f;//(transform.position - camFromPosition).magnitude;
        //transform.position.normalized + 

        Vector3 camFromRotation = Camera.main.transform.rotation.eulerAngles;

        // move camera to destination and look at player to get rotation angle
        Camera.main.transform.position = camToPostiton;
        Camera.main.transform.LookAt(transform.position);
        //Camera.main.transform.Rotate(transform.forward, 90);
        Vector3 camToRotation = Camera.main.transform.rotation.eulerAngles;

        //return to fromPosition
        Camera.main.transform.position = camFromPosition;
        Camera.main.transform.rotation = Quaternion.Euler(camFromRotation);

        for (float t = 0; t < camTransitionDuration; t += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(camFromPosition, camToPostiton, t / camTransitionDuration);
            Camera.main.transform.rotation = Quaternion.Lerp(
                                                Quaternion.Euler(camFromRotation),
                                                Quaternion.Euler(camToRotation),
                                                t / camTransitionDuration
                                                );

            yield return null;
        }


        for (int i = 0; i < dashPoints.Count; i++)
        {
            float duration;
            if (i == 0)
                duration = Vector3.Distance(dashPoints[0].position, transform.position) * 0.125f;
            else
                duration = Vector3.Distance(dashPoints[i].position, dashPoints[i - 1].position) * 0.125f;

            float turnRotation = 0.25f;
            Quaternion fromRotation = transform.rotation;
            Quaternion toRotation = Quaternion.LookRotation(Quaternion.AngleAxis(90, transform.forward) * Vector3.Cross((dashPoints[i].position-transform.position).normalized, transform.position));
            for (float t = 0; t < turnRotation; t += Time.deltaTime)
            {
                transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t / turnRotation);
                yield return null;
            }

            Vector3 fromPos = transform.position;
            Vector3 fromUp = transform.up;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(fromPos, dashPoints[i].position, t / duration);
                transform.up = Vector3.Lerp(fromUp, dashPoints[i].normal, t / duration);
                yield return null;
            }
        }

        //set to absolut position
        transform.position = dashPoints[dashPoints.Count - 1].position;
        transform.up = dashPoints[dashPoints.Count - 1].normal;

        dashPoints.Clear();


        //lerp back to old position 
        //TODO: Change!!
        camToPostiton = Camera.main.transform.position;
        camToRotation = Camera.main.transform.rotation.eulerAngles;
        for (float t = 0; t < camTransitionDuration; t += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(camToPostiton, camFromPosition, t / camTransitionDuration);
            Camera.main.transform.rotation = Quaternion.Lerp(
                                                Quaternion.Euler(camToRotation),
                                                Quaternion.Euler(camFromRotation),
                                                t / camTransitionDuration
                                                );

            yield return null;
        }
        Camera.main.transform.parent = null;

        isPaused = true;
        canvas.enabled = true;
        DashCoroutine = null;


    }

}
