using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetName : MonoBehaviour {

    public static SetName instance;
    public string playername;
    public Text getname;

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
        getname.text = playername;

        Debug.Log("Name = " + getname.text);
    }

    // Update is called once per frame
    void Update () {
        

    }

    
}
