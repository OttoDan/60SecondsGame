using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawGeoFormTouch : MonoBehaviour
{

    LineRenderer lineRenderer;
    [Range(3, 30)]
    int points;
    public float radius = 1;

    public Vector3[] positions;

    IEnumerator coroutine;

    Color fromColor;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>() as LineRenderer;
    }

    public void Draw()
    {
        points = 4;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = (MoveGeoForms());
        StartCoroutine(coroutine);

    }
    public void Stop()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        if(radius > 0)
        {
            coroutine = StopGeoForm();
                StartCoroutine(coroutine);
        }
    }

        private void UpdateLineRenderer()
    {
        if (radius >= 0.1f)
        {
            transform.LookAt(Camera.main.transform);
            positions = new Vector3[points + 2];

            for (int i = 0; i < positions.Length; i++)
            {
                float x = Mathf.Cos((i / (float)points) * 2 * Mathf.PI);
                float y = Mathf.Sin((i / (float)points) * 2 * Mathf.PI);
                positions[i] = new Vector3(x, y, 0) * radius;
            }
            positions[positions.Length - 2] = positions[0];
            positions[positions.Length - 1] = positions[1];

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

           // lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.1f, .5f), new Keyframe(.9f, .5f), new Keyframe(1, 0));

        }

        if (radius < 0)
        {
            lineRenderer.positionCount = 0;
            radius = 0;
        }
    }

    IEnumerator MoveGeoForms(float duration = 0.75f, float _radius = 4)
    {
        radius = 0;

        for(float t = 0; t < duration*0.75f; t+= Time.unscaledDeltaTime)
        {
            transform.Rotate(0, 0, -25 * Time.unscaledDeltaTime);
            radius = Mathf.Lerp(0, _radius, t / (duration*0.75f));
            UpdateLineRenderer();

            yield return null;
        }
        float index = 0;
        while (PlayerController.Instance.dashPoints.Count < 2)
        {
            index += Time.unscaledDeltaTime;
            radius = _radius + Mathf.Sin(index * 4.5f) * _radius*0.125f;
            UpdateLineRenderer();


            yield return null;
        }

        for (float t = 0; t < duration*0.25f; t += Time.unscaledDeltaTime)
        {
            transform.Rotate(0, 0, -25 * Time.unscaledDeltaTime);
            radius = Mathf.Lerp( _radius,0, t / (duration*0.25f));
            UpdateLineRenderer();

            yield return null;
        }
        radius = 0;
        points = 0;
        UpdateLineRenderer();
        coroutine = null;
    }

    IEnumerator StopGeoForm(float duration = 0.25f)
    {
        float fromRadius = radius;
        UpdateLineRenderer();
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            transform.Rotate(0, 0, -25 * Time.unscaledDeltaTime);
            radius = Mathf.Lerp(fromRadius, 0, t / duration );
            UpdateLineRenderer();

            yield return null;
        }
        radius = 0;
        points = 0;
        UpdateLineRenderer();
        coroutine = null;
    }


    //public void CreateGeoForm(float _radius = 5, int _points = 4)
    //{
    //    radius = PlayerController.Instance.enemyHitsDuringDash + 2;
    //    points = (int)Random.Range(3, 6)/*_points*/;
    //}
}
