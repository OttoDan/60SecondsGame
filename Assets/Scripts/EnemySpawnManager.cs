using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    public GameObject mesh;

    public GameObject[] prefabs;


    private Vector3 circRan;

    private Ray ray;

    GameObject enemy;


    private void Start()
    {


    }


    void Spawn()
    {
        RaycastHit hit;
        circRan = Random.onUnitSphere * 15;
        Debug.Log("2");

        if (Physics.Raycast(circRan, -circRan.normalized, out hit, 128))//, LayerMask.NameToLayer("walkableCube"))) 
        {

            //if (hit.transform.tag == "Cube")
            //{
                int enemyType = Random.Range(0, 1);

                Vector3 spawnPoint = hit.point;

                enemy = Instantiate(prefabs[enemyType], circRan, prefabs[enemyType].transform.rotation);

                Debug.Log(hit.point);
                Debug.Log(circRan);
                Enemy myScript = enemy.GetComponent<Enemy>();
                myScript.coroutine = myScript.MoveTo(hit.point);
                StartCoroutine(myScript.coroutine);

            //}
        }
        else
        {
            Debug.Log("7");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Spawn();
        }
    }

}
