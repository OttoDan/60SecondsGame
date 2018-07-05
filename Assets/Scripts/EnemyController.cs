using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovingObject {

    public Enemy enemy;

    int cubesPerRow = 3;

    #region Unity Messages

    private void Start()
    {
    }
    RaycastHit centerHit;

    Vector3 velocity;
    //float speed = 0;

    private void Update()
    {
        
        if(Physics.Raycast(transform.position, -transform.position.normalized, out centerHit, Mathf.Abs(transform.position.magnitude)*2, LayerMask.GetMask("Walkable")))
        {
            transform.up = Vector3.Lerp(transform.up, centerHit.normal, Time.deltaTime);
            //velocity = (transform.forward + (centerHit.point - transform.position).normalized) * Time.deltaTime;
            transform.Translate((transform.forward * enemy.maxSpeed + (centerHit.point - transform.position).normalized) * Time.deltaTime);
        }
    }


    #endregion

    #region Methods

    public void HitEvent()
    {
        /* go into the queque following the player
         * or explode
         * or fall from the cube
         * or leave the scene
         */

        //Explosion with cubes (Works only if meshrenderer is under "Mesh" gameobject)
        Bounds bounds = transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().bounds;
        float xStep = (bounds.max.x - bounds.min.x) / cubesPerRow;
        float yStep = (bounds.max.y - bounds.min.y) / cubesPerRow;
        float zStep = (bounds.max.z - bounds.min.z) / cubesPerRow;
        Vector3 direction = (transform.position - PlayerController.Instance.transform.position).normalized;
        

        for (float x = bounds.min.x; x < bounds.max.x; x+= xStep)
        {

            for (float y = bounds.min.y; y < bounds.max.y; y += yStep)
            {
                for (float z = bounds.min.z; z < bounds.max.z; z += zStep)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = new Vector3(xStep, yStep, zStep);
                    cube.transform.position = Vector3.right * x + Vector3.up * y + Vector3.forward * z;

                    cube.AddComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 0.25f + direction * 0.5f, ForceMode.Impulse);
                    cube.AddComponent<DelayedDestroy>();
                    cube.GetComponent<MeshRenderer>().material.color = transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color;
                }
            }
        }
        
        EnemyManager.Instance.SpawnEnemy();
        Destroy(gameObject);
    }


    void QuickAndDirtyStopMotionEnde()
    {

        TimeManager.Instance.DeactivateSlowMotion();
    }

    #endregion

}
