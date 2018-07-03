using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid  {

    public static  Vector3 Snap(Vector3 position)
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
}
