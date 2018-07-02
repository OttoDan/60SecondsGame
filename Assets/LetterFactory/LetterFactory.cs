using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class LetterFactory : MonoBehaviour {

    #region Public Fields

    public static LetterFactory Instance;

    public string letterContainerFilePath = "/Assets/LetterContainers/";

    public Font font;
    public string input;
    public Dictionary<int, char> alphabetIndex = new Dictionary<int, char>()
    {
        { 0, ' ' },
        { 1, 'A' },
        { 2, 'B' },
        { 3, 'C' },
        { 4, 'D' },
        { 5, 'E' },
        { 6, 'F' },
        { 7, 'G' },
        { 8, 'H' },
        { 9, 'I' },
        {10, 'J' },
        {11, 'K' },
        {12, 'L' },
        {13, 'M' },
        {14, 'N' },
        {15, 'O' },
        {16, 'P' },
        {17, 'Q' },
        {18, 'R' },
        {19, 'S' },
        {20, 'T' },
        {21, 'U' },
        {22, 'V' },
        {23, 'W' },
        {24, 'X' },
        {25, 'Y' },
        {26, 'Z' }
    };

    public RenderTexture renderTexture;

    public bool randomize = false;

    [Range(0,1)]
    public float randomRange = 1;

    #endregion

    #region Private Fields

    private Camera camera;

    private Canvas canvas;

    private Text text;


    #endregion
    #region Unity Messages

    private void Awake()
    {
        if (Instance != null)
            DestroyImmediate(gameObject);
        else
            Instance = this;

        letterContainerFilePath = Application.dataPath + "/LetterContainers/";
    }
    public void wakeup()
    { 
        camera = GetComponentInChildren<Camera>();
        canvas = GetComponentInChildren<Canvas>();
        text = GetComponentInChildren<Text>();
        
    }

    #endregion

    #region Public Methods
    #endregion

    #region Public Methods

    public void CreateLetterContainer(Font font)
    {
        text.font = font;

        List<Letter> letters = new List<Letter>();

        //foreach (var entry in alphabetIndex)
        //{
        //    letters.Add(CreateLetter(entry.Value, font));
        //}

        letters = CreateLetters("ABCDEFGHIJKLMNOPQRSTUVWXYZ");

        LetterContainer letterContainer = new LetterContainer(font, letters.ToArray());
        foreach(Letter letter in letterContainer.Letters)
        {
            //CreateCubeLetter(letter);
            letter.Save();
        }
       // letterContainer.Save(letterContainerFilePath + font.name + ".xml");
    }

    public List<Letter> CreateLetters(string characters)
    {
        List<Letter> letters = new List<Letter>();

        foreach (char chr in characters)
            letters.Add(CreateLetter(chr));

        return letters;
    }

    public Letter CreateLetter(char character)
    {
        bool[,] pixels;

        // apply the character to our canvas text
        text.text = (string)character.ToString();
        Canvas.ForceUpdateCanvases();

        //render our canvas to texture
        Texture2D texture2D = RTImage(camera);
        
        //wip
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/test.png", bytes);

        //end wip
        pixels = new bool[texture2D.width, texture2D.height];
        //Debug.Log("Width: " + texture2D.width + " Height: " + texture2D.height);

        for (int x = 0; x < texture2D.width; x++)
        {
            for (int y = 0; y < texture2D.height; y++)
            {
                Color color = texture2D.GetPixel(x, y);

                if (color.a == 1)
                    pixels[x, y] = true;
                else
                    pixels[x, y] = false;
            }
        }

        //TODO: ADD XML STUFF HERE

        return new Letter(character, pixels, font);
    }

    public Letter CreateLetter(char character, Font font)
    {
        text.font = font;
        return CreateLetter(character);
    }


    public GameObject CreateCubeLetter(Letter letter)
    {
        GameObject cubeLetter = new GameObject("CubeLetter: " + letter.Character);
        //Debug.Log("Length(0): " + letter.Pixels.GetLength(0) + " Length(1): " + letter.Pixels.GetLength(0));
        Material newMat = new Material(Shader.Find("LightweightPipeline/Standard (Simple Lighting)"));
        newMat.color = Random.ColorHSV();
        for (int x = 0; x < letter.Pixels.GetLength(0); x++)
        {
            for (int y = 0; y < letter.Pixels.GetLength(1); y++)
            {
                if (letter.Pixels[x,y])
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = randomize == true ?
                        new Vector3(x, 0, y) + new Vector3(Random.Range(-0.25f, 0.25f) * randomRange, 0, Random.Range(-0.25f, 0.25f) * randomRange) :
                        new Vector3(x, 0, y);
                    if (randomize)
                    {
                        cube.transform.rotation = Quaternion.Euler(Vector3.one * (Random.Range(0.75f,1.25f) * randomRange));
                        cube.transform.localScale *= Random.Range(0.75f, 1.5f) * randomRange;

                    }
                    cube.transform.parent = cubeLetter.transform;
                    cube.GetComponent<MeshRenderer>().material = newMat;
                    //Instantiate(cube, cube.transform.position, Quaternion.identity, null);

                }
            }
        }

        //foreach(Transform cube in cubeLetter.transform)
        //{

        //    Rigidbody rb = cube.gameObject.AddComponent<Rigidbody>();
        //    rb.AddExplosionForce(1024f, cube.transform.position, 128f,64f);
        //}
        return cubeLetter;
    }



    #endregion

    #region Helper

    Texture2D RTImage(Camera cam)
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;
        return image;
    }

    #endregion

}
