using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTextLerp : MonoBehaviour
{
    float range = 16;
    List<Vector3> from = new List<Vector3>();
    List<Vector3> to = new List<Vector3>();



    // Use this for initialization
    private void Start()
    {


        StartCoroutine(LerpIn());
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    IEnumerator LerpIn(float duration = 3.0f)
    {
        foreach (Transform cube in transform)
        {
            to.Add(cube.position);
            ///from.Add(Camera.main.transform.position + Camera.main.transform.forward * 0.21f + Camera.main.transform.right * Random.Range(-0.33f, 0.33f) + Camera.main.transform.up * Random.Range(-0.3f, 0.3f));
            from.Add(Random.onUnitSphere * 32);
            cube.position = from[from.Count - 1];
        }
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position = Vector3.Lerp(from[i], to[i], t / duration);
            }
            yield return null;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = to[i];
        }


    }
}
