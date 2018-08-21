using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleSwitchButton : MonoBehaviour {

    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        bool obst = GameManager.Instance.GetObstacles();
        text.text = "0bstacles = " + obst;
    }

    private void OnMouseDown()
    {
        bool obst = GameManager.Instance.SwitchObstacles();
        text.text = "0bstacles = " + obst;

    }
}
