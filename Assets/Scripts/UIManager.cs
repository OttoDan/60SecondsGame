using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    private Text scoreText;
    private Text timeText;
    private Canvas ComboUICanvas;
    private CanvasGroup ComboUICanvasGroup;
    private Text comboUItext;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("Two UIManagers in this scene!");
            Destroy(gameObject);
        }

        scoreText = transform.Find("StatsCanvas").transform.Find("ScoreText").GetComponent<Text>();
        timeText = transform.Find("StatsCanvas").transform.Find("TimeText").GetComponent<Text>();

        ComboUICanvas = transform.Find("ComboUICanvas").GetComponent<Canvas>();
        ComboUICanvasGroup = ComboUICanvas.GetComponent<CanvasGroup>();
        comboUItext = ComboUICanvas.transform.Find("Text").GetComponent<Text>();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + (int)GameManager.Instance.currentScore;
    }

    public void UpdateTime()
    {
        timeText.text = GameManager.Instance.seconds + " sec";
    }

    public void DisplayComboUI(Enemy enemy)
    {
        if (DisplayComboUICoroutine != null)
            StopCoroutine(DisplayComboUICoroutine);

        ComboUICanvas.gameObject.SetActive(true);
        DisplayComboUICoroutine = DisplayComboUIRoutine(enemy);
        StartCoroutine(DisplayComboUICoroutine);
    }

    float displayComboUIduration = 4.0f;

    IEnumerator DisplayComboUICoroutine;

    IEnumerator DisplayComboUIRoutine(Enemy enemy)
    {

        ComboUICanvas.gameObject.SetActive(true);
        if(enemy != null)
            comboUItext.text = enemy.name + " " + enemy.score;
        else
            comboUItext.text = "Messed up! NO0oB!";


        float fromAlpha = ComboUICanvasGroup.alpha;

        //fadein
        for (float t = 0; t < displayComboUIduration*0.25f; t+= Time.unscaledDeltaTime)
        {
            ComboUICanvasGroup.alpha = Mathf.Lerp(fromAlpha, 1, t / displayComboUIduration * 0.25f);
            yield return null;
        }
        //display
        for (float t = 0; t < displayComboUIduration * 0.5f; t += Time.unscaledDeltaTime * 0.5f)
            yield return null;
        //fadeout
        for (float t = 0; t < displayComboUIduration * 0.25f; t += Time.unscaledDeltaTime)
        {
            ComboUICanvasGroup.alpha = Mathf.Lerp(1, 0, t / displayComboUIduration * 0.25f);
            yield return null;
        }
        ComboUICanvas.gameObject.SetActive(false);
        DisplayComboUICoroutine = null;
    }

}
