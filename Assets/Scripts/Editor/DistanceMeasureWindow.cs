using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DistanceMeasureWindow : EditorWindow
{
    Transform from;
    Transform to;
    [MenuItem("InsaniTools/DistanceMeasure")]
    static void Init()
    {
        DistanceMeasureWindow window = (DistanceMeasureWindow)EditorWindow.GetWindow(typeof(DistanceMeasureWindow));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Distance");
        EditorGUILayout.LabelField("From:");
        from = (Transform)EditorGUILayout.ObjectField(from, typeof(Transform), true);
        EditorGUILayout.LabelField("To:");
        to = (Transform)EditorGUILayout.ObjectField(to, typeof(Transform), true);
        if(from!= null && to != null)
        {
            EditorGUILayout.LabelField(Vector3.Distance(from.position,to.position).ToString());
            Debug.DrawLine(from.position, to.position, Color.black);

        }

        //Repaint();
    }
}