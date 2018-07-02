using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetName : MonoBehaviour {

    public static SetName instance;
    public string playername;
    public Text nameField;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}

    public void setName()
    {
        nameField.text = playername;

        Debug.Log("Name = " + nameField.text);
    }

    // Update is called once per frame
    void Update () {
        

    }

    
}
