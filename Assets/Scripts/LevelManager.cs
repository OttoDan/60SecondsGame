using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    //public Level Level { get; private set; }
    public new string name;
    public AudioClip song;

    public static LevelManager Instance;
    Vector3 levelBoundsMin = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
    Vector3 levelBoundsMax = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);

    IEnumerator ScaleLevelCoroutine;
    float scaleDownFactor = -0.125f;
    int scaleDownEachSeconds = 20;
    float seconds = 0;
    #region Unity Messages
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Debug.Log(levelBoundsMax);
        GetLevelBounds();
        CenterLevelToZeroPosition();
        Debug.Log(levelBoundsMax);
    }

    
    
    private void Update()
    {
        //DebugDrawLevelBounds();
        //seconds += Time.unscaledDeltaTime;
        //if ((int)seconds % scaleDownEachSeconds == 0)
        //{
        //    ScaleLevel(scaleDownFactor, 0.5f);
        //    ObstacleManager.Instance.GroundAllObstacles();
        //    seconds = 1;
        //}
    }

    public void ScaleLevel(float factor, float duration)
    {
        if (ScaleLevelCoroutine == null)
        {
            ScaleLevelCoroutine = ScaleLevelRoutine(factor, duration);
            StartCoroutine(ScaleLevelCoroutine);
        }
    }
    IEnumerator ScaleLevelRoutine(float factor, float duration)
    {
        Vector3 fromScale = transform.localScale;
        Vector3 toScale = transform.localScale + transform.localScale * factor;
        //TODO: test with scaled and unscaled deltaTime
        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            transform.localScale = Vector3.Lerp(fromScale, toScale, t / duration);
            yield return new WaitForFixedUpdate();
        }
        GetLevelBounds();
        //CenterLevelToZeroPosition();
        CameraController.Instance.AdjustCameraZoomByLevelBounds();
        ScaleLevelCoroutine = null;
    }

    #endregion

    void GetLevelBounds()
    {

        levelBoundsMin = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        levelBoundsMax = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);
        foreach (Transform child in transform)
        {
            Bounds bounds = child.GetComponent<MeshRenderer>().bounds;
            if (bounds != null && child.gameObject.activeSelf)
            {
                if (bounds.min.x < levelBoundsMin.x)
                    levelBoundsMin.x = bounds.min.x;
                if (bounds.min.y < levelBoundsMin.y)
                    levelBoundsMin.y = bounds.min.y;
                if (bounds.min.z < levelBoundsMin.z)
                    levelBoundsMin.z = bounds.min.z;

                if (bounds.max.x > levelBoundsMax.x)
                    levelBoundsMax.x = bounds.max.x;
                if (bounds.max.y > levelBoundsMax.y)
                    levelBoundsMax.y = bounds.max.y;
                if (bounds.max.z > levelBoundsMax.z)
                    levelBoundsMax.z = bounds.max.z;
            }
        }
        
    }

    public float MinCamDistance()
    {
        return Mathf.Max(new float[]{ levelBoundsMax.x, levelBoundsMax.y, levelBoundsMax.z });
    }
    void CenterLevelToZeroPosition()
    {
        //transform.position = new Vector3(
        //    (levelBoundsMin.x + levelBoundsMax.x) * 0.5f,
        //    (levelBoundsMin.y + levelBoundsMax.y) * 0.5f,
        //    (levelBoundsMin.z + levelBoundsMax.z) * 0.5f);
    }

    private void DrawGrid()
    {

    }

    void DebugDrawLevelBounds()
    {
        //Bottom
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMin.z), Color.green);

        //Top
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);

        //Connections
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMin.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMin.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMax.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);
        Debug.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, levelBoundsMax.z), new Vector3(levelBoundsMin.x, levelBoundsMax.y, levelBoundsMax.z), Color.green);

        float y = Mathf.Round(PlayerController.Instance.transform.position.y);

        for (float x = Mathf.Round(levelBoundsMin.x); x < Mathf.Round(levelBoundsMax.x); x += 1)
        {
            Debug.DrawLine(new Vector3(x, y, levelBoundsMin.z), new Vector3(x, y, levelBoundsMax.z), Color.white);
        }
        for (float z = Mathf.Round(levelBoundsMin.z); z < Mathf.Round(levelBoundsMax.z); z += 1)
        {
            Debug.DrawLine(new Vector3(levelBoundsMin.x, y, z), new Vector3(levelBoundsMax.x, y, z), Color.white);
        }
    }
}
