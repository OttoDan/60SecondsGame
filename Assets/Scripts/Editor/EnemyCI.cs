using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class EnemyCI : Editor {
    public override void OnInspectorGUI()
    {
        Enemy script = (Enemy)target;

        base.OnInspectorGUI();

        EditorGUILayout.MinMaxSlider(ref script.minSpeed, ref script.maxSpeed, 0, 100);

    }
}
