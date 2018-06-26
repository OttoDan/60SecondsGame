using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePathManager : MonoBehaviour {

    Vector3 boundsMax = Vector3.negativeInfinity;
    Vector3 boundsMin = Vector3.positiveInfinity;

    Vector3 clickPoint;
    Vector3 clickNormal;

    List<Vector3> clickPoints = new List<Vector3>(); 
    List<Vector3> clickNormals = new List<Vector3>();

    List<Vector3> boundsPoints = new List<Vector3>();

    #region Unity Messages

    private void Start()
    {
        foreach(Transform cube in transform)
        {
            GetPossiblePositions(cube);
        }

        Debug.Log("BoundsMax:" + boundsMax + " BoundsMin:" + boundsMin);

        // look from above
        for (float x = boundsMin.x - 0.5f; x < boundsMax.x + 0.5f; x += 0.5f)
        {
            for (float z = boundsMin.z - 0.5f; z < boundsMax.z + 0.5f; z += 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, boundsMax.y + 0.5f, z), Vector3.down, out hit))
                {
                    boundsPoints.Add(hit.point); 
                }
            }
        }

        // look from above
        for (float x = boundsMin.x - 0.5f; x < boundsMax.x + 0.5f; x += 0.5f)
        {
            for (float y = boundsMin.y - 0.5f; y < boundsMax.y + 0.5f; y += 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, y, boundsMax.z + 0.5f), Vector3.back, out hit))
                {
                    boundsPoints.Add(hit.point);
                }
            }
        }
    }

    private void Update()
    {
        foreach (Vector3 point in boundsPoints)
        {
            Debug.DrawLine(point + Vector3.up, point);
        }

        if (clickPoint != null)
        {
            Debug.DrawLine(clickPoint, Vector3.zero, Color.red);
            Debug.DrawLine(clickPoint, clickPoint + clickNormal, Color.red);
        }
        for (int i = 1; i < clickPoints.Count; i++)
        {
            Debug.DrawLine(clickPoints[i], clickPoints[i-1], Color.white);
            Debug.DrawLine(clickPoints[i], clickPoints[i] + clickNormals[i], Color.red);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                clickPoint = hit.point;//transform.GetComponent<MeshRenderer>().bounds.ClosestPoint(hit.point);
                clickNormal = hit.normal;

                if (clickPoints.Count > 1)
                {
                    Vector3 direction = clickPoint - clickPoints[clickPoints.Count - 1];
                    

                    if (clickNormals[clickNormals.Count - 1] != clickNormal)//not on the same side
                    {
                        Vector3 cross = Vector3.Cross(clickNormal, direction.normalized);
                        clickPoints.Add(cross);
                        clickNormals.Add(clickNormal);
                        clickPoints.Add(clickPoint);
                        clickNormals.Add(clickNormal);
                    }
                    else //probably on the same side
                    {
                        //need to check for height differences
                        clickPoints.Add(clickPoint);
                        clickNormals.Add(clickNormal);
                    }
                }
                else
                {
                    clickPoints.Add(clickPoint);
                    clickNormals.Add(clickNormal);
                }
                
                //foreach(Transform cube in transform)
                //{
                //    Bounds bounds = cube.GetComponent<MeshRenderer>().bounds;

                //}
            }
        }
    }
    #endregion

    #region Methods
    void GetPossiblePositions(Transform cube)
    {
        Bounds bounds = cube.GetComponent<MeshRenderer>().bounds;


        

        if (bounds.max.x > boundsMax.x)
            boundsMax = new Vector3(bounds.max.x, boundsMax.y, boundsMax.z);
        if (bounds.max.y > boundsMax.y)
            boundsMax = new Vector3(boundsMax.x, bounds.max.y, boundsMax.z);
        if (bounds.max.z > boundsMax.z)
            boundsMax = new Vector3(boundsMax.x, boundsMax.y, bounds.max.z);

        if (bounds.min.x < boundsMin.x)
            boundsMin = new Vector3(bounds.min.x, boundsMin.y, boundsMin.z);
        if (bounds.min.y < boundsMin.y)
            boundsMin = new Vector3(boundsMin.x, bounds.min.y, boundsMin.z);
        if (bounds.min.z < boundsMin.z)
            boundsMin = new Vector3(boundsMin.x, boundsMin.y, bounds.min.z);

        

    }

    void DebugDrawPositions(Transform cube)
    {
        foreach(Transform child in cube)
        {
            Debug.DrawLine(child.position, cube.position);
        }
    }
    #endregion
}
