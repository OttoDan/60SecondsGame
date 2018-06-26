using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerCont))]
public class PlayerContEditor : Editor
{

    public override void OnInspectorGUI()
    {
        PlayerCont script = (PlayerCont)target;
        base.OnInspectorGUI();

        foreach (DashPoint point in script.dashPoints)
        {
            EditorGUILayout.TextArea(point.position.ToString());
        }

    }
}
