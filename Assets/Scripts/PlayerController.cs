using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovingObject {

    public static PlayerController Instance;

    #region Private Fields

    List<DashPoint> dashPoints = new List<DashPoint>();


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
    }

    private void Update()
    {
        DebugDrawDashPoints();
    }

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
            }
            else
                Debug.LogError("Missing EnemyController on object on Enemy layer!");
        }
    }

    #endregion

    #region Methods

    public void PlaceDashpoint(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.magnitude, LayerMask.GetMask("Walkable")))
        {
            Vector3 fromPosition;
            Vector3 fromNormal;

            if (dashPoints.Count > 0)
            {
                fromNormal = dashPoints[dashPoints.Count - 1].normal;
                fromPosition = dashPoints[dashPoints.Count - 1].position + fromNormal * 0.5f;
            }
            else
            {
                fromNormal = transform.up;
                fromPosition = Grid.Snap(transform.position + fromNormal * 0.5f);
            }
            float dist = Vector3.Distance(fromPosition, Grid.Snap(hit.point));

            //if(dist >= 1)
            //{

            //}
            //else
            if (dist > 2)
            {
                Vector3 direction = Quaternion.AngleAxis(90, fromNormal) * Vector3.Cross((Grid.Snap(hit.point) - fromPosition).normalized,fromNormal);

                Debug.Log(direction);
                RaycastHit dirHit;

                if (Physics.Raycast(fromPosition, direction, out dirHit, 24, LayerMask.GetMask("Walkable")))
                {
                    dashPoints.Add(new DashPoint(dirHit.point, dirHit.normal));
                    return;
                }
                else
                {
                    RaycastHit groundHit;

                    if (Physics.Raycast(fromPosition + direction, -fromNormal, out groundHit, 24, LayerMask.GetMask("Walkable")))
                    {
                        dashPoints.Add(new DashPoint(groundHit.point, groundHit.normal));
                        return;
                    }
                    else
                    {
                        //dashPoints.Add(new DashPoint(Grid.Snap(hit.point), hit.normal));
                        return;
                    }
                }

            }
        }
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
