using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameTool : EditorWindow {


    [MenuItem("GameTool/GameTool")]
    public static void SchowWindow()
    {
        EditorWindow.GetWindow<GameTool>("GameTool");
    }


    private void OnGUI()
    {
        
    }

}
