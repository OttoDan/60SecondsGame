using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnGeoForms : MonoBehaviour
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

    private void Start()
    {
        points = PlayerController.Instance.enemyHitsDuringDash + 2;
        coroutine = (MoveGeoForms(2f, 1));
        StartCoroutine(coroutine);

    }

    private void Update()
    {
        if (radius >= 0)
        {

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
            radius = 0;
    }

    IEnumerator MoveGeoForms(float duration = 1, float _radius = 5)
    {
        float movedTime = 0f;
        radius = _radius;

        while (movedTime < (duration / 2))
        {
            transform.Rotate(0, 0, 25);
            // Debug.Log("smaller");
            radius -= .1f;
            movedTime += Time.unscaledDeltaTime;



            yield return null;
        }

        Color toColor = lineRenderer.material.color;
        fromColor = lineRenderer.material.color;
        toColor.a = 0f;

        while (movedTime > (duration / 2) && movedTime < duration)
        {
            // Debug.Log("bigger");
            radius += .17f;
            transform.Rotate(0, 0, -25);
            lineRenderer.material.color = Color.Lerp(fromColor, toColor, (movedTime - duration / 2) / (duration / 2));

            movedTime += Time.unscaledDeltaTime;

            //  Debug.Log(movedTime);

            yield return null;
        }
        lineRenderer.material.color = fromColor;
        radius = 0;
        Debug.Log("end");
        coroutine = null;
    }


    //public void CreateGeoForm(float _radius = 5, int _points = 4)
    //{
    //    radius = PlayerController.Instance.enemyHitsDuringDash + 2;
    //    points = (int)Random.Range(3, 6)/*_points*/;
    //}
}
