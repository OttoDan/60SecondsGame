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
    private Text ComboMultiplierText;
    public Material noiseUImat;


    Vector3 UpdateComboMultiplierTextOrigPos;

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
        ComboMultiplierText = transform.Find("StatsCanvas").transform.Find("ComboMultiplierText").GetComponent<Text>();

        ComboUICanvas = transform.Find("ComboUICanvas").GetComponent<Canvas>();
        ComboUICanvasGroup = ComboUICanvas.GetComponent<CanvasGroup>();
        comboUItext = ComboUICanvas.transform.Find("Text").GetComponent<Text>();
    }
    float timeTextFromFontSize;
    private void Start()
    {
        UpdateComboMultiplierTextOrigPos = ComboMultiplierText.rectTransform.position;
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
    public void UpdateComboMultiplierText()
    {
        if (UpdateComboMultiplierCoroutine != null)
            StopCoroutine(UpdateComboMultiplierCoroutine);
        UpdateComboMultiplierCoroutine = UpdateComboMultiplierRoutine();
        StartCoroutine(UpdateComboMultiplierCoroutine); 

    }
    IEnumerator UpdateComboMultiplierCoroutine;
    IEnumerator UpdateComboMultiplierRoutine()
    {
        Vector3 fromPos = ComboMultiplierText.rectTransform.position;
        Vector3 toPos = Vector3.right * Screen.width + Vector3.up * Screen.height;
        for(float t = 0; t<0.125f; t += Time.unscaledDeltaTime)
        {

            ComboMultiplierText.rectTransform.position = Vector3.Lerp(fromPos, toPos, t / 0.125f);
            yield return null;
        }
        ComboMultiplierText.text = PlayerController.Instance.comboMultiplier.ToString();

        fromPos = ComboMultiplierText.rectTransform.position;
        toPos = Vector3.right * Screen.width*0.475f + Vector3.up * Screen.height *0.475f;
        for (float t = 0; t < 0.75f; t += Time.unscaledDeltaTime)
        {

            ComboMultiplierText.rectTransform.position = Vector3.Lerp(fromPos, toPos, t / 0.75f);
            yield return null;
        }

        fromPos = ComboMultiplierText.rectTransform.position;
        for (float t = 0; t < 0.25f; t += Time.unscaledDeltaTime)
        {

            ComboMultiplierText.rectTransform.position = Vector3.Lerp(fromPos, UpdateComboMultiplierTextOrigPos, t / 0.25f);
            yield return null;
        }

        UpdateComboMultiplierCoroutine = null;
    }
    
        //List<Text> texts = new List<Text>( transform.GetComponentsInChildren<Text>());

        //foreach (Text text in texts)
        //{
        //    Debug.Log("text");

        //    text.ma = new Vector2(Random.Range(0, 128), Random.Range(0, 128));// text.material.mainTexture.width), Random.Range(0, text.material.mainTexture.height));
        //}
    

    public void DisplayComboUI(Enemy enemy)
    {
        if (DisplayComboUICoroutine != null)
            StopCoroutine(DisplayComboUICoroutine);

        ComboUICanvas.gameObject.SetActive(true);
        DisplayComboUICoroutine = DisplayComboUIRoutine(enemy);
        StartCoroutine(DisplayComboUICoroutine);
    }

    public void DisplayLevelStartUI()
    {
        //TODO: Add Display Start Text and show level name for a few seconds
    }

    public void TimeUIStart()
    {
        if (TimeCoroutine != null)
            StopCoroutine(TimeCoroutine);
        TimeCoroutine = TimeRoutine();
        StartCoroutine(TimeCoroutine);
    }

    float displayComboUIduration = 0.75f;

    IEnumerator DisplayComboUICoroutine;

    IEnumerator DisplayComboUIRoutine(Enemy enemy)
    {

        ComboUICanvas.gameObject.SetActive(true);
        if (enemy != null)
            comboUItext.text = enemy.name + "\n" + enemy.score + "\n x " + PlayerController.Instance.enemyHitsDuringDash;
        else
            comboUItext.text = "";


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

    IEnumerator TimeCoroutine;
    IEnumerator TimeRoutine()
    {
        Vector3 fromLocalPosition;
        Quaternion fromLocalRotation;
        float shakeDuration = 1f;
        float shakeMagnitude = 2.8f;
        float shakeNewMagnitude;

        while (GameManager.Instance.seconds > 1)
        {
            UpdateTime();
            //while ((int)GameManager.Instance.seconds % 2 != 0)
            //    yield return null;
            fromLocalPosition = timeText.transform.localPosition;
            fromLocalRotation = timeText.transform.localRotation;
            //currShake = (Shake(duration, magnitude));
            shakeNewMagnitude = Mathf.Pow(shakeMagnitude, 2); // so the screenshake is really intens at the start but gets weaker quickly


            float timeShaked = 0f;

            while (timeShaked < shakeDuration)
            {
                float x = Random.Range(-1f, 1f); //* (60 / GameManager.Instance.seconds) * 0.5f;/* * (((int)GameManager.Instance.seconds % 5) + 1);*/
                float y = Random.Range(-1f, 1f); //* (60 / GameManager.Instance.seconds) * 0.5f;/* * (((int)GameManager.Instance.seconds % 5) + 1);*/

                //if ((int)GameManager.Instance.seconds % 5 == 0)
                //{
                //    x *= 5;
                //    y *= 5;
                //}

                timeText.transform.localPosition += new Vector3(x * .1f, y * .1f, 0) * (shakeNewMagnitude / 2);
                timeText.transform.localRotation = Quaternion.Euler(fromLocalRotation.eulerAngles.x, y * shakeNewMagnitude, fromLocalRotation.eulerAngles.z);
                
                timeShaked += Time.unscaledDeltaTime;
                shakeNewMagnitude = Mathf.Sqrt(shakeNewMagnitude); // so the screenshake is really intens at the start but gets weaker quickly
                                                         //Debug.Log(_magnitude);

                yield return null;
            }
            //timeText.transform.localPosition = fromLocalPosition;
            //timeText.transform.localRotation = fromLocalRotation;
        }


        TimeCoroutine = null;
    }
}