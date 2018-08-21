using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTextLerp : MonoBehaviour {
    float range = 16;
    List<Vector3> from = new List<Vector3>();
    List<Vector3> to = new List<Vector3>();
    List<float> scale = new List<float>();
    Gyroscope gyroscope;
    

    public static CubeTextLerp Instance;

    private void Awake()
    {
        gyroscope = Input.gyro;
        gyroscope.enabled = true;
    }

    // Use this for initialization
    private void OnEnable()
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
            from.Add(Random.onUnitSphere * range + cube.position);
            cube.position = from[from.Count - 1];
            scale.Add(Random.Range(1, 1.25f));
        }
        for (float t = 0; t < duration; t+= Time.unscaledDeltaTime)
        {
            
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position = Vector3.Lerp(from[i], to[i], t / duration);
                transform.GetChild(i).localScale = Vector3.one * Mathf.Lerp(1, scale[i], t / duration);
            }
            yield return null;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = to[i];
        }

        //while (true)
        //{
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        transform.GetChild(i).localScale = Mathf.Sin(32/Time.deltaTime) * Vector3.one;
        //    }
        //    yield return null;
        //}
        while (Input.touchCount == 0)
        {

            transform.Rotate(gyroscope.rotationRate * Time.unscaledDeltaTime);

            yield return null;
        }
        //Time.timeScale = 1;
        for (int i = 0; i < transform.childCount; i++)
        {
            Rigidbody rb = transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
        }
        yield return new WaitForSecondsRealtime(0.75f);
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject.GetComponent<Rigidbody>());
            from[i] = transform.GetChild(i).position;
            to[i] = Camera.main.transform.position + Camera.main.transform.forward * 0.21f + Camera.main.transform.right * Random.Range(-0.33f,0.33f) + Camera.main.transform.up * Random.Range(-0.3f,0.3f);
        }
        for (float t = 0; t < duration*0.25f; t += Time.unscaledDeltaTime)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).position = Vector3.Lerp(from[i], to[i], t / (duration*0.25f));
            }
            yield return null;
        }
        if(Instance == null)
        {
            Instance = this;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        

    }
}
