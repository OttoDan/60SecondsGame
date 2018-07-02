using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

    Vector3 levelBoundsMin = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    Vector3 levelBoundsMax = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);

    #region Unity Messages

    private void Start()
    {
        GetLevelBounds();
    }


    private void Update()
    {
        DrawLevelBounds();
    }

    #endregion

    void GetLevelBounds()
    {
        foreach(Transform child in transform)
        {
            Bounds bounds = child.GetComponent<MeshRenderer>().bounds;
            if(bounds != null)
            {
                if (bounds.min.x < levelBoundsMin.x)
                    levelBoundsMin.x = bounds.min.x;
                if (bounds.min.y < levelBoundsMin.y)
                    levelBoundsMin.y = bounds.min.y;
                if (bounds.min.z < levelBoundsMin.z)
                    levelBoundsMin.z = bounds.min.z;

                if (bounds.max.x > levelBoundsMax.x)
                    levelBoundsMax.x = bounds.max.x;
                if (bounds.max.y > levelBoundsMax.y)
                    levelBoundsMax.y = bounds.max.y;
                if (bounds.max.z > levelBoundsMax.z)
                    levelBoundsMax.z = bounds.max.z;
            }
        }
    }

    private void DrawGrid()
    {
       
    }

    void DrawLevelBounds()
    {
        //Bottom
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMin.z), Color.green);

        //Top
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);

        //Connections
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);

        float y = Mathf.Round(PlayerController3.Instance.transform.position.y);

        for (float x = Mathf.Round(levelBoundsMin.x); x < Mathf.Round(levelBoundsMax.x); x += 1)
        {
            Debug.DrawLine(new Vector3(x, y, levelBoundsMin.z), new Vector3(x, y, levelBoundsMax.z), Color.white);
        }
        for (float z = Mathf.Round(levelBoundsMin.z); z < Mathf.Round(levelBoundsMax.z); z += 1)
        {
            Debug.DrawLine(new Vector3(levelBoundsMin.x, y, z), new Vector3(levelBoundsMax.x, y, z), Color.white);
        }
    }
}
