using UnityEngine;
using System.Collections;
/*
 * Philip
 *
 */

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    public TimeManager timeManager;

    private bool slow;

    CursorLockMode wantedMode;

    IEnumerator enemyHitCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogWarning("There are two TimeManagers in this scene!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    void Update()
    {
        if(GameManager.Instance.state == GameManager.State.Level)
        //Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

    }

    public void ToggleStopMotion()
    {
        if (slow == false)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            slow = true;
        }
        else
        {
            Time.timeScale = 1;
            slow = false;
        }
    }

    public void ActivateSlowMotion()
    {
        if (slow == false)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            slow = true;
        }
    }

    public void DeactivateSlowMotion()
    {
        if (slow == true)
        {
            Time.timeScale = 1;
            slow = false;
        }

    }

    public void EnemyHitStopMoution()
    {
        if(enemyHitCoroutine == null)
        {
            enemyHitCoroutine = EnemyHitRoutine();
            StartCoroutine(enemyHitCoroutine);
        }
    }

    IEnumerator EnemyHitRoutine()
    {
        float duration = 0.75f;
        float timeFrom = Time.timeScale;
        float timeTo = slowdownFactor;
        //for (float t = 0; t < duration*0.125f; t+= Time.unscaledDeltaTime)
        //{
        //    Time.timeScale = Mathf.Lerp(timeFrom, timeTo, t / duration);
        //    yield return new WaitForEndOfFrame();
        //}
        Time.timeScale = timeTo;
        for (float t = 0; t < duration * 0.75f; t += Time.unscaledDeltaTime)
        {
            yield return new WaitForEndOfFrame();
        }
        for (float t = 0; t < duration * 0.25f; t += Time.unscaledDeltaTime)
        {
            Time.timeScale = Mathf.Lerp(timeTo, timeFrom, t / duration);
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = timeFrom;
        enemyHitCoroutine = null;
    }

}
