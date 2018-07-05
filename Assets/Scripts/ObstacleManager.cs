using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
    public static ObstacleManager Instance;

    Transform[] obstacles;
    Vector3[] fromPositions;
    Vector3[] toPositions;

    float fallDuration = 0.5f;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        //TODO: dont forget to adapt this when there are obstacles created on the fly
    }

    public void GroundAllObstacles()
    {
        Debug.Log("how often");
        

        if (ObstacleFallCoroutine != null)
            StopCoroutine(ObstacleFallCoroutine);
        
        ObstacleFallCoroutine = ObstacleFallRoutine(fallDuration);
        StartCoroutine(ObstacleFallCoroutine);

        
    }
    
    IEnumerator ObstacleFallCoroutine;
    IEnumerator ObstacleFallRoutine(float duration)
    {
        obstacles = new Transform[transform.childCount];
        fromPositions = new Vector3[transform.childCount];
        toPositions = new Vector3[transform.childCount];
        RaycastHit hit;
        Transform obstacle;

        yield return new WaitForSecondsRealtime(0.75f);

        for(int i = 0; i < transform.childCount; i++)
        {
            obstacle = transform.GetChild(i);
            if (Physics.Raycast(obstacle.position, -obstacle.up, out hit))//, obstacle.position.magnitude*2, LayerMask.NameToLayer("Walkable")))
            {
                obstacles[i] = obstacle;
                fromPositions[i] = obstacle.position;
                toPositions[i] = hit.point;
                //obstacle.gameObject.AddComponent<Rigidbody>();

            }
            else
            {
                Debug.Log("no raycasthit?!");
                obstacles[i] = obstacle;
                fromPositions[i] = obstacle.position;
                toPositions[i] = obstacle.position-obstacle.up*obstacle.position.magnitude*0.5f;
            }
        }
        for (float t=0; t<duration; t+= Time.deltaTime)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].position = Vector3.Lerp(fromPositions[i], toPositions[i], t / duration);
            }

            yield return new WaitForFixedUpdate();
        }
        
        ObstacleFallCoroutine = null;
    }
}
