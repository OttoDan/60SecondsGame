using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager Instance;

    public List<GameObject> enemies = new List<GameObject>();
    public int startEnemyAmount = 10;
    public bool randomColor = false;
    public IEnumerator spawnPhaseCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There are two EnemyManagers in the scene!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < startEnemyAmount; i++)
            SpawnEnemy();
    }

    
    private void Update()
    {
        CheckEnemiesGrounded();
        GroundEnemies();
        MoveEnemies();
        
    }

    public void SpawnEnemy()
    {
        if(enemies.Count > 0)
        {
            GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], transform);
            //enemy.transform.Find("Mesh").GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
            if (randomColor)
                enemy.transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>().material.color = Random.ColorHSV();

            enemy.transform.localScale *= Random.Range(0.75f, 1.75f);
            enemy.transform.position = Random.insideUnitSphere * 32;
            enemy.transform.LookAt(Vector3.zero);
            StartCoroutine(FallOnCube(enemy.transform));
        }
    }

    void MoveEnemies()
    {
        foreach(Transform enemy in transform)
        {
            enemy.Translate(enemy.forward * enemy.GetComponent<EnemyController>().enemy.maxSpeed * Time.deltaTime);
        }
    }

    void GroundEnemies()
    {
        RaycastHit centerHit;
        foreach (Transform enemy in transform)
        {
            if(enemy.GetComponent<EnemyController>().isGrounded == false)
                if (Physics.Raycast(enemy.position + enemy.up, -enemy.position.normalized, out centerHit, Mathf.Abs(enemy.position.magnitude) * 2, LayerMask.GetMask("Walkable")))
                {
                    enemy.position = centerHit.point + enemy.up * 0.5f;
                    enemy.up = centerHit.normal;
                    enemy.GetComponent<EnemyController>().isGrounded = true;
                }

        }
    }

    void CheckEnemiesGrounded()
    {
        RaycastHit centerHit;
        foreach (Transform enemy in transform)
        {
            if (enemy.GetComponent<EnemyController>().isGrounded == true)
                if (!Physics.Raycast(enemy.position + enemy.up, -enemy.up, out centerHit, 3, LayerMask.GetMask("Walkable")))
                    enemy.GetComponent<EnemyController>().isGrounded = false;
        }
    }

    IEnumerator FallOnCube(Transform enemy)
    {
        float duration = Mathf.Abs(enemy.position.magnitude) * 0.05f;

        
        RaycastHit cubeHit;
        if(Physics.Raycast(enemy.position, enemy.forward, out cubeHit, Mathf.Abs(enemy.position.magnitude), LayerMask.GetMask("Walkable")))
        {
            Vector3 fromPos = enemy.position;
            Vector3 toPos = cubeHit.point;
            Vector3 fromNormal = enemy.up;
            Vector3 toNormal = cubeHit.normal;
            //Debug.Log("oh");
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)//deltaTime)
            {
                if (enemy != null)
                {
                    enemy.position = Vector3.Lerp(fromPos, toPos, t / duration);
                    enemy.up = Vector3.Lerp(fromNormal, toNormal, t / duration);
                }
                else
                    break;

                yield return null;
            }


        }
        //enemy.GetComponent<EnemyController>().isGrounded = false;
        
    }

    IEnumerator SpawnPhase()
    {
        yield return null;
    }
}
