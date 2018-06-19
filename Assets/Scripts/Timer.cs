using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text TimeText;
    public float time = 60;
    public bool OnGame = true;

    void Start()
    {
        OnGame = true;

        
    }

    void Update()
    {

        time -= Time.deltaTime;
        
        if (OnGame = true)
        {
            TimeText.text = ("" + time);
        }

        if (time < 0)
        {
            OnGame = false;
        }

    }





}
