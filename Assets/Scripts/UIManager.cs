using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

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
    float timeTextFromFontSize;
    private void Start()
    {
        timeTextFromFontSize = timeText.fontSize;
    }
    public void UpdateScore()
    {
        scoreText.text = "Score: " + (int)GameManager.Instance.currentScore;
    }

    public void UpdateTime()
    {
        timeText.text = (int)GameManager.Instance.seconds + "";
        timeText.fontSize = (int)Mathf.Lerp(timeTextFromFontSize, 250, Mathf.Abs(GameManager.Instance.seconds - 60) / 60);
        //timeText.rectTransform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.up * timeText.rectTransform.rect.height + Vector3.right * timeText.rectTransform.rect.width, Mathf.Abs(GameManager.Instance.seconds - 60) / 60);
    }

    public void DisplayComboUI(Enemy enemy)
    {
        if (DisplayComboUICoroutine != null)
            StopCoroutine(DisplayComboUICoroutine);

        ComboUICanvas.gameObject.SetActive(true);
        DisplayComboUICoroutine = DisplayComboUIRoutine(enemy);
        StartCoroutine(DisplayComboUICoroutine);
    }

    float displayComboUIduration = 0.75f;

    IEnumerator DisplayComboUICoroutine;

    IEnumerator DisplayComboUIRoutine(Enemy enemy)
    {

        ComboUICanvas.gameObject.SetActive(true);
        if (enemy != null)
            comboUItext.text = enemy.name + "\n" + enemy.score + "\n x " + PlayerController.Instance.enemyHitsDuringDash;
        else
            comboUItext.text = "Messed up!\nNO0oB!";


        float fromAlpha = ComboUICanvasGroup.alpha;

        //fadein
        for (float t = 0; t < displayComboUIduration * 0.25f; t += Time.unscaledDeltaTime)
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