using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    #region Public Fields

    public static GameManager Instance;

    public enum State
    {
        Intro,
        MainMenu,
        Level,
        Transition
    }

    public State state = State.Level;

    public int currentScore { get; private set; }

    public float seconds = 0;

    public Canvas scoreCanvas;

    public const float addScoreDurationMultiplier = 0.25f;

    #endregion

    #region Private Fields


    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogError("Two GameManagers in scene!");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        seconds += Time.deltaTime;//Time.unscaledDeltaTime;//Time.unscaledTime;
        UIManager.Instance.UpdateTime();
    }

    #endregion

    #region Methods

    public void AddScore(int score)
    {
        //IEnumerator coroutine = AddScoreRoutine(score);
        //StartCoroutine(coroutine);
        currentScore += score;
        UIManager.Instance.UpdateScore();

        Debug.Log("scored: " + score + "\ncurrentScore:" + currentScore);
    }

    #endregion

    IEnumerator AddScoreRoutine(int score)
    {
        float duration = score * addScoreDurationMultiplier;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {

            yield return null;
        }
    }
}
