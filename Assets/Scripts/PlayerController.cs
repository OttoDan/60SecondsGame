using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovingObject {

    public static PlayerController Instance;


    public Canvas dashButtonCanvas;

    #region Private Fields

    List<DashPoint> dashPoints = new List<DashPoint>();

    LineRenderer lineRenderer;

    IEnumerator DashCoroutine;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Two PlayerControllers in the scene!");
            Destroy(gameObject);
        }

        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        TimeManager.Instance.ActivateStopMotion();
    }
    private void Update()
    {
        DebugDrawDashPoints();
    }
    IEnumerator dashStopMotionCoroutine;
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collider");
        if(collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyController enemyController = collider.gameObject.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                GameManager.Instance.AddScore(enemyController.enemy.score);
                enemyController.HitEvent();
                TimeManager.Instance.EnemyHitStopMoution();
                //if (dashStopMotionCoroutine == null)
                //{
                //    dashStopMotionCoroutine = DashStopMotionRoutine();
                //    StartCoroutine(dashStopMotionCoroutine);
                //}

            }
            else
                Debug.LogError("Missing EnemyController on object on Enemy layer!");
        }
    }
    
    #endregion

    #region Methods

    public void PlaceDashpoint(Vector2 screenPos)
    {
        if (DashCoroutine != null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.magnitude*2, LayerMask.GetMask("Walkable")))
        {
            Vector3 fromPosition;
            Vector3 fromNormal;

            if (dashPoints.Count > 0)
            {
                fromNormal = dashPoints[dashPoints.Count - 1].normal;
                fromPosition = dashPoints[dashPoints.Count - 1].position + dashPoints[dashPoints.Count - 1].normal * 0.5f;
            }
            else
            {
                fromNormal = transform.up;
                fromPosition = Grid.Snap(transform.position) + fromNormal * 0.5f;
            }
            float distance = Vector3.Distance(fromPosition,hit.point);

            
            //if (distance <= 2)//1.45 eigentlich
            //{
                Vector3 direction = hit.point - fromPosition;
                Vector3 orthogonalDirection = Quaternion.AngleAxis(90, fromNormal) * Vector3.Cross(direction.normalized,fromNormal);

                RaycastHit dirHit;

            if (Physics.Raycast(fromPosition, orthogonalDirection, out dirHit, 2 /*direction.magnitude*/, LayerMask.GetMask("Walkable")))
            {
                AddDashPoint(dirHit.point, dirHit.normal);
                return;
            }
            else
            {
                RaycastHit groundHit;

                if (Physics.Raycast(fromPosition + orthogonalDirection, -fromNormal, out groundHit, 2, LayerMask.GetMask("Walkable")))
                {
                    AddDashPoint(groundHit.point, groundHit.normal);
                    return;
                }
                else
                {
                    RaycastHit cornerHit;
                    if (Physics.Raycast(fromPosition + orthogonalDirection - fromNormal * 2, -orthogonalDirection, out cornerHit, 2, LayerMask.GetMask("Walkable")))
                    {
                        Debug.Log("Corner");
                        AddDashPoint(cornerHit.point, orthogonalDirection);
                        return;
                    }
                    else
                    {
                        Debug.Log("No ray");
                        AddDashPoint(hit.point, hit.normal);
                        return;
                    }
                }
            }

            //}
            //else
            //{

            //}
        }
    }


    void AddDashPoint(Vector3 position, Vector3 normal)
    {
        //Prevent double points
        foreach (DashPoint point in dashPoints)
        {
            if (Vector3.Distance(point.position, position) < 0.5f)
                return;
        }
        DashPoint dashPoint = new DashPoint(position, normal);
        FocusParticles.Instance.MoveToPoint(dashPoint);
        dashPoints.Add(dashPoint);
        lineRenderer.positionCount = dashPoints.Count + 1;
        lineRenderer.SetPosition(0, transform.position);
        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, dashPoints[i - 1].position + dashPoints[i - 1].normal * 0.125f);
        }
        lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.1f, .5f), new Keyframe(.9f, .5f), new Keyframe(1, 0));

    }

    public void Dash()
    {
        if (DashCoroutine == null && dashPoints.Count > 0)
        {
            dashButtonCanvas.enabled = false;
            DashCoroutine = DashRoutine();
            StartCoroutine(DashCoroutine);
        }
    }

    IEnumerator DashRoutine()
    {
        TimeManager.Instance.DeactivateStopMotion();

        lineRenderer.positionCount = 0;

        for (int i = 0; i < dashPoints.Count; i++)
        {
            float duration=0.05f;
            //if (i == 0)
            //    duration = Vector3.Distance(dashPoints[0].position, transform.position) * 0.05f;
            //else
            //    duration = Vector3.Distance(dashPoints[i].position, dashPoints[i - 1].position) * 0.05f;

           
            Vector3 fromPos = transform.position;
            Vector3 fromUp = transform.up;
            Quaternion fromRotation = transform.rotation;

            Quaternion toRotation;
            toRotation = Quaternion.LookRotation((dashPoints[i].position-fromPos).normalized, dashPoints[i].normal);//Quaternion.AngleAxis(90, transform.forward) * Vector3.Cross((dashPoints[i].position - fromPos).normalized, transform.up);

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(fromPos, dashPoints[i].position,  t / duration);
                transform.rotation = Quaternion.Slerp(fromRotation, toRotation, t / duration);
                //transform.LookAt(dashPoints[i].position);

                yield return null;
            }
        }

        //set to absolute position
        transform.position = dashPoints[dashPoints.Count - 1].position;
        //transform.up = dashPoints[dashPoints.Count - 1].normal;

        dashPoints.Clear();


        //wait for Respawn Routine / respawn enemies
        int enemyCount = Random.Range(1, 10);// * anzahlDerBeimLetztenDashZerstörtenEnemies
        for(int i = 0; i < enemyCount; i++)
            EnemyManager.Instance.SpawnEnemy();


        TimeManager.Instance.ActivateStopMotion();
        dashButtonCanvas.enabled = true;
        DashCoroutine = null;


    }

    IEnumerator DashStopMotionRoutine()
    {
        TimeManager.Instance.ActivateStopMotion();
        yield return new WaitForSecondsRealtime(0.25f);
        TimeManager.Instance.DeactivateStopMotion();
        dashStopMotionCoroutine = null;
        yield return null;
    }
    #endregion

    #region Debug

    void DebugDrawDashPoints()
    {
        if (dashPoints.Count > 0)
        {
            Debug.DrawLine(dashPoints[0].position, transform.position, Color.green);
            Debug.DrawRay(dashPoints[0].position, dashPoints[0].normal, Color.cyan);

            for (int i = 0; i < dashPoints.Count - 1; i++)
            {

                Debug.DrawLine(dashPoints[i].position, dashPoints[i + 1].position, Color.green);
                Debug.DrawRay(dashPoints[i + 1].position, dashPoints[i + 1].normal, Color.cyan);
            }
        }

    }

    #endregion
}
