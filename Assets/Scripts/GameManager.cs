using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

    #region Public Fields

    public static GameManager Instance;
    public string playerName = "Dude";
    public enum State
    {
        Intro,
        MainMenu,
        Level,
        Transition,
        GameWon,
        GameLost
    }

    public State state = State.MainMenu;

    public int currentScore { get; private set; }

    public float seconds { get; private set; }
    float timer;
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
            Debug.LogWarning("Two GameManagers in scene!");
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            //state = State.MainMenu;
            SwitchToLevelState();
        }
        else
        {
        }
        
    }
    private void Update()
    {
        switch (state)
        {
            case State.Level:
                //seconds += Time.deltaTime;//Time.unscaledDeltaTime;//Time.unscaledTime;
                seconds -= Time.unscaledDeltaTime;
                

                if (seconds < 0)
                {
                    GameEnded();
                }
                break;
        }
    }

    #endregion

    #region Methods

    public void SwitchToLevelState()
    {
        seconds = 60;
        StartLevel();
        this.state = State.Level;
    }

    public void StartLevel()
    { 
        //UI
        UIManager.Instance.DisplayLevelStartUI();
        UIManager.Instance.TimeUIStart();

        //Audio
        AudioManager.Instance.SetSongAudio(LevelManager.Instance.song);
        AudioManager.Instance.TimerStartAudio();
        AudioManager.Instance.SongStartAudio();
    }

    public void AddScore(int score)
    {
        //IEnumerator coroutine = AddScoreRoutine(score);
        //StartCoroutine(coroutine);
        AudioManager.Instance.NumbersRiseAudio();
        currentScore += score;
        UIManager.Instance.UpdateScore();

        //Debug.Log("scored: " + score + "\ncurrentScore:" + currentScore);
    }

    void GameEnded()
    {
        HighScoreToJSON.Instance.SaveData();
        //TODO: Add more than one entry to the JSON highscore list
        state = State.GameWon;
        SwitchToLevelState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game Ended");
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
