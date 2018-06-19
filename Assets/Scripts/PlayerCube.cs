using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCube : MonoBehaviour {
    public static PlayerCube Instance;

    LineRenderer lineRenderer;

    List<Vector3> dashPositions = new List<Vector3>();
    bool isPaused;
    IEnumerator DashCoroutine;

    public Canvas canvas;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        lineRenderer = GetComponent<LineRenderer>();
        isPaused = true;
    }

    void Update()
    {
        if (isPaused)
        {
            Debug.Log(dashPositions.Count);
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (dashPositions.Count == 0)
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, transform.position);
                }
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        lineRenderer.positionCount = lineRenderer.positionCount + 1;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitInfo.point);
                    }
                }
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitInfo.point);
                }
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    dashPositions.Add(hitInfo.point);
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitInfo.point);
                }
            }
        }


        Debug.DrawLine(Vector3.zero, transform.position, Color.red);
    }

    public void Dash()
    {
        if (DashCoroutine == null && dashPositions.Count > 0)
        {
            canvas.enabled = false;
            isPaused = false;
            DashCoroutine = DashRoutine();
            StartCoroutine(DashCoroutine);
        }
    }

    IEnumerator DashRoutine()
    {
        lineRenderer.positionCount = 0;
        for (int i = 0; i < dashPositions.Count; i++)
        {
            float duration;
            if (i == 0)
                duration = Vector3.Distance(dashPositions[0], transform.position) * 0.125f;
            else
                duration = Vector3.Distance(dashPositions[i], dashPositions[i - 1]) * 0.125f;

            Vector3 fromPos = transform.position;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(fromPos, dashPositions[i], t / duration);
                yield return null;
            }
        }
        dashPositions.Clear();
        isPaused = true;
        canvas.enabled = true;
        DashCoroutine = null;


    }
}
