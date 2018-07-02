using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public IEnumerator coroutine;
    public IEnumerator MoveTo(Vector3 destination)
    {

        float duration = 2f;
        Vector3 fromPos = transform.position;
        duration *= Random.Range(.5f, 2f);
        for (float t = 0; t < duration; t += Time.deltaTime)
        {

            transform.position = Vector3.Lerp(fromPos, destination, t / duration);

            yield return null;
        }

    }
}
