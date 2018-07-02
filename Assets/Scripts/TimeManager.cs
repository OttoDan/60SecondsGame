using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        //Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        //Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

}