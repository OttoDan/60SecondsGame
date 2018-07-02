using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LetterFactory))]
public class LetterFactoryInspector : Editor {

    public override void OnInspectorGUI()
    {
        
        LetterFactory letterFactory = (LetterFactory)target;
        //base.OnInspectorGUI();

        EditorGUILayout.LabelField("Text");
        letterFactory.font = EditorGUILayout.ObjectField("Font:", letterFactory.font, typeof(Font), false) as Font;

        //if (GUILayout.Button("Create Letter"))
        //{
        //    letterFactory.wakeup();
        //    letterFactory.CreateLetters("Hello World");
        //}

        letterFactory.input =
                EditorGUILayout.TextField("Input Char: ", letterFactory.input);

        letterFactory.randomize = EditorGUILayout.Toggle("Randomize?", letterFactory.randomize);

        EditorGUI.BeginDisabledGroup(!letterFactory.randomize);
        letterFactory.randomRange = EditorGUILayout.Slider("RandomRange:", letterFactory.randomRange, 0, 1);
        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Create Cube Letter"))
        {
            letterFactory.wakeup();
            if(letterFactory.input.Length == 1 )
                letterFactory.CreateCubeLetter(letterFactory.CreateLetter(letterFactory.input[0],letterFactory.font));
            else if (letterFactory.input.ToString().Length>1)
            {
                GameObject stringParentObject = new GameObject(letterFactory.input);
                int step = 0;
                foreach(char chr in letterFactory.input)
                {
                    Transform newCubeLetter = letterFactory.CreateCubeLetter(letterFactory.CreateLetter(chr, letterFactory.font)).transform;
                    newCubeLetter.position = Vector3.right * 48 * step;
                    newCubeLetter.parent = stringParentObject.transform;
                    step++;
                }
                stringParentObject.transform.localScale = new Vector3(0.25f, 1.75f, 0.3f);
            }
        }

        if (GUILayout.Button("Create Letter Container"))
        {
            letterFactory.wakeup();
            letterFactory.CreateLetterContainer(letterFactory.font);
        }


        if (GUILayout.Button("Load LetterContainer"))
        {
            string path = EditorUtility.OpenFolderPanel("Open LetterContainer Folder", "Assets/LetterContainer/",""); //OpenFilePanel("Open file", "Assets/LetterContainers/", "xml");
            LetterContainer.LoadLetters(path);
            Debug.Log(path);
        }

        this.Repaint();

    }

}
