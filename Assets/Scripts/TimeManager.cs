using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f;

    public TimeManager timeManager;

    private bool slow;

    CursorLockMode wantedMode;

    void Update ()
	{
        //Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        
    }

    public void DoSlowmotion ()
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
