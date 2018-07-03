using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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