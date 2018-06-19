using UnityEngine;
using UnityEditor;

public class Popup : EditorWindow
{
    

    [MenuItem("Info/Info")]
    static void Init()
    {



        Popup window = ScriptableObject.CreateInstance<Popup>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 350);
        window.ShowPopup();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Oben In den Reitern GameTool/GameTool um das Hauptmenü zu skippen und andere Sachen", EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        if (GUILayout.Button("klick mich")) this.Close();
    }

    
}