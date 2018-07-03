using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MovingObject {

    public Enemy enemy;

    int cubesPerRow = 2;

    #region Unity Messages

    private void Start()
    {
    }

    private void Update()
    {
        transform.Translate(transform.forward * 2 * Time.deltaTime);
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
        Bounds bounds = transform.Find("Mesh").GetComponent<MeshRenderer>().bounds;
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
                }
            }
        }

        EnemyManager.Instance.SpawnEnemy();
        Destroy(gameObject);
    }

    #endregion

}
