using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextZoomer : MonoBehaviour {
    IEnumerator routine;
    private void OnMouseDown()
    {
        if (routine == null)
        {
            StartCoroutine(routine = scaleUp());
        }
    }
    private void OnMouseUp()
    {
        if (routine!=null)
        {
            StopCoroutine(routine);
            StartCoroutine(routine = scaleDown());
        }


    }
    IEnumerator scaleUp() {
        float duration = 0.75f;
        Vector2 fromScale = transform.localScale;
        Vector2 toScale = fromScale * 1.25f;
        for(float t = 0; t<duration; t += Time.unscaledDeltaTime)
        {
            transform.localScale = Vector2.Lerp(fromScale, toScale, t / duration);
            yield return null;
        }
    }

    IEnumerator scaleDown()
    {
        float duration = 0.25f;
        Vector2 fromScale = transform.localScale;
        Vector2 toScale = Vector2.one;
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            transform.localScale = Vector2.Lerp(fromScale, toScale, t / duration);
            yield return null;
        }
        routine = null;
    }

}
