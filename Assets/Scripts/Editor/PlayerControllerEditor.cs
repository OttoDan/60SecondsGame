using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        PlayerController script = (PlayerController)target;
        base.OnInspectorGUI();

        foreach (Vector3 point in script.dashPointsPosition)
        {

            EditorGUILayout.TextArea(point.ToString());
        }
        
    }
}
