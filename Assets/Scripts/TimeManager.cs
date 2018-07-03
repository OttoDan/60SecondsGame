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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There are two TimeManagers in this scene!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    void Update()
    {
        //Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

    }

    public void ToogleStopmotion()
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

}
