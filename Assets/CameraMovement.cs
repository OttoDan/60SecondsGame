using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Camera cam;
    private Transform camParent;

    private Vector3 localRotation;

    public float MouseSensitivity = 120f;

    public float OrbitDamping = .85f;

    private Ray ray;



    private void Awake()
    {
        cam = Camera.main;
        camParent = transform.parent;
    }

    
    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if ( !Physics.Raycast(ray))
            {
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                   localRotation.x = Input.GetAxis("Mouse X") * MouseSensitivity;
                   localRotation.y = Input.GetAxis("Mouse Y") * MouseSensitivity;
                }
            }
        }
       
        localRotation.x = localRotation.x * OrbitDamping;
        localRotation.y = localRotation.y * OrbitDamping;

        camParent.Rotate(Vector3.up, localRotation.x);
        camParent.Rotate(Vector3.right, -localRotation.y);
    }
}
