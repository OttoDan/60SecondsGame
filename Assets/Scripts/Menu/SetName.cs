using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetName : MonoBehaviour {

<<<<<<< HEAD
    public static SetName Instance;
=======
    public static SetName instance;
>>>>>>> MainMenu
    public string playername;
    public Text nameField;

    private void Awake()
    {
<<<<<<< HEAD
        if (Instance == null)
            Instance = this;
=======
        if (instance == null)
            instance = this;
>>>>>>> MainMenu
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
