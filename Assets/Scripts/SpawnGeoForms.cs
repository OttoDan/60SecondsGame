using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnGeoForms : MonoBehaviour {


	//attach to enemy
	// if collision   radius=5
	//                point = 5 or whatever

	private LineRenderer lineRenderer;

	[Range(3,30)]
	int points;
	float radius = 1;

	Vector3[] positions;
	// Use this for initialization

	private void Awake()
	{
        //radius = 5;
        //points = Random.Range(3, 10);
        //CreateGeoForm();


        //lr = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        lineRenderer = GetComponent<LineRenderer>();

	}

    private void Start()
    {
        CreateGeoForm();
    }

    private void Update()
	{
		if (radius >= 0)
		{
            //transform.LookAt(Camera.main.transform);

		    positions = new Vector3[points + 2];

		    for (int i = 0; i < positions.Length; i++)
		    {
			    float x = Mathf.Cos((i / (float) points) * 2 * Mathf.PI);
			    float y = Mathf.Sin((i / (float) points) * 2 * Mathf.PI);
			    positions[i] = new Vector3(x, y, 0) * radius;
		    }
		    positions[positions.Length - 2] = positions[0];
		    positions[positions.Length - 1] = positions[1];

		    lineRenderer.positionCount = positions.Length;
		    lineRenderer.SetPositions(positions);

            lineRenderer.widthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.1f, .5f), new Keyframe(.9f, .5f), new Keyframe(1, 0));

        }



		if (radius > 0)
		{
			radius -= Random.Range(8,16) * Time.unscaledDeltaTime;


		}

		 if(radius < 0)
			radius = 0;
	}

	public void CreateGeoForm(float _radius = 5,int _points = 4)
	{
        Debug.Log("radius: " + _radius + " points: " + _points);
		radius = PlayerController.Instance.enemyHitsDuringDash + 2;
		points = (int)Random.Range(3,6)/*_points*/;

	}


}
