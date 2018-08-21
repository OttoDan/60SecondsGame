using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreview : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Transform lvl in transform)
        {
            lvl.Rotate(Vector3.up + Vector3.right, 12 * Time.unscaledDeltaTime);
        }
    }
}
