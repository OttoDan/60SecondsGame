using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OrientationSwitchButton : MonoBehaviour {

    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        text.text = "orientation = " + Screen.orientation;
    }

    private void OnMouseDown()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft||Screen.orientation == ScreenOrientation.LandscapeRight)
            Screen.orientation = ScreenOrientation.Portrait;
        else if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) 
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        text.text = "orientation = " + Screen.orientation;
        Debug.Log(Screen.orientation);

    }
}
