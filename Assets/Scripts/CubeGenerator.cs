using UnityEngine;
using UnityEditor;

public class CubeGenerator : EditorWindow {

    #region Private Fields



    #endregion

    #region Public Fields

    public int width, height;

    public float cubeWidth, cubeHeight;

    public GameObject cubePrefab;


    #endregion

    #region Window Functions

    [MenuItem("CubeTools/CubeGenerator")]
    static void Init()
    {
        CubeGenerator window = (CubeGenerator)EditorWindow.GetWindow(typeof(CubeGenerator));
        window.Show();
    }

    private void OnGUI()
    {
        if (cubePrefab == null)
        {
            EditorGUILayout.HelpBox("Please assign a cube prefab reference.", MessageType.Info);
            // GUILayout.Label("Please assign a prefab reference.", GUI.tooltip);
        }


        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);

        cubePrefab = EditorGUILayout.ObjectField("Prefab", cubePrefab, typeof(GameObject), false) as GameObject;

        cubeWidth = EditorGUILayout.FloatField("Cube Width", cubeWidth);
        cubeHeight = EditorGUILayout.FloatField("Cube Height", cubeHeight);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUI.BeginDisabledGroup(cubePrefab == null);
            {
                if (GUILayout.Button("Generate Cube"))
                {
                    GenerateCube(width, height, cubeWidth, cubeHeight);
                }
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region Methods

    public void GenerateCube(int width=0, int height=0, float cubeWidth = 1, float cubeHeight = 1)
    {
        Transform parent = new GameObject("ThaCube").transform;
        if(width == 0)
            width = this.width;
        if (height == 0)
            height = this.height;

        //Debug.LogFormat("Width: {0}\nHeight: {1}", width, height);

        Vector3 right = Vector3.right;
        Vector3 forward = Vector3.forward;

        Transform currentSide;

        for(int i = 0; i < 6; i++)
        {
            currentSide = new GameObject("Side" + i).transform;
            currentSide.parent = parent;

            
            

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject copy = Instantiate(cubePrefab, (-right * width * 0.5f) + x * right + (-forward * height * 0.5f) + y * forward, Quaternion.identity, currentSide);
                    copy.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
                    //copy.transform.localScale = new Vector3(cubeWidth, cubeHeight, 1);
                }
            }

            switch (i)
            {
                case 0:
                    currentSide.localPosition = Vector3.up * height * 0.5f;
                    break;

                case 1:
                    currentSide.localPosition = Vector3.forward * height * 0.5f;
                    currentSide.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
                    break;

                case 2:
                    currentSide.localPosition = -Vector3.up * height * 0.5f;
                    currentSide.localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
                    break;

                case 3:
                    currentSide.localPosition = -Vector3.forward * height * 0.5f;
                    currentSide.localRotation = Quaternion.Euler(new Vector3(270, 0, 0));
                    break;

                case 4:
                    currentSide.localPosition = Vector3.right * height * 0.5f;
                    currentSide.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    break;

                case 5:
                    currentSide.localPosition = -Vector3.right * height * 0.5f;
                    currentSide.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    break;

            }


        }

        Camera.main.transform.position = Vector3.right * width - Vector3.forward * height + Vector3.up * height;
        Camera.main.transform.LookAt(parent);

        //GameObject.FindGameObjectWithTag("Player").transform.position = -Vector3.forward * height +  Vector3.up * height;
    }

    #endregion
}
