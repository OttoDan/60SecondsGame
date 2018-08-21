using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGameButton : MonoBehaviour {
    IEnumerator coroutine;
    public Transform lerpTo;
    public Transform LevelPreview;
    private void OnMouseDown()
    {
        if (coroutine == null)
        {

            coroutine = StartGameTransition();
            StartCoroutine(coroutine);
        }
        
    }
    IEnumerator StartGameTransition(float duration = 2.0f)
    {
        Camera.main.GetComponent<CameraController>().enabled = false;
        PlayerController.Instance.transform.parent = Camera.main.transform;
        Vector3 fromPos = Camera.main.transform.position;

        Quaternion fromRotation = Camera.main.transform.rotation;
        Quaternion toRotation = lerpTo.rotation;

        for (float t = 0; t < duration; t+= Time.unscaledDeltaTime)
        {

            Camera.main.transform.position = Vector3.Lerp(fromPos, lerpTo.position, t / duration);
            Camera.main.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t / duration);

            yield return null;
        }
        int levelID = 0;
        for (int i = 0; i<8; i++)
        {
            levelID = Random.Range(0, LevelPreview.childCount);
            fromPos = PlayerController.Instance.transform.position;
            Vector3 toPos = LevelPreview.GetChild(levelID).position + Vector3.back * 3;
            for (float t = 0; t < 0.5f; t += Time.unscaledDeltaTime)
            {
                PlayerController.Instance.transform.position = Vector3.Lerp(fromPos, toPos, t / 1);
                PlayerController.Instance.transform.LookAt(toPos);
                yield return null;
            }
        }
        PlayerController.Instance.transform.parent = null;
        Camera.main.transform.LookAt(LevelPreview.GetChild(levelID).position);
        fromPos = Camera.main.transform.position;
        for(float t = 0; t < duration * 0.95f; t+= Time.unscaledDeltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(fromPos, LevelPreview.GetChild(levelID).position-Camera.main.transform.forward * 3.25f , t / duration);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.Instance.SwitchToLevelState();
        SceneManager.LoadScene(2+levelID);
    }
}
