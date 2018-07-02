using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Letter")]
public class Letter : GenericXmlSerializableClass<Letter> {

    #region Properties
    [XmlElement("Character")]
    public char Character;// { get; private set; }

    [XmlIgnore]
    public bool[,] Pixels;// { get; private set; }

    //[XmlArray("Pixels"), XmlArrayItem("Pixel")]
    [XmlArray("Pixels"), XmlArrayItem(typeof(bool), ElementName = "Pixel")]
    public List<bool> XmlPixels;

    [XmlIgnore]
    public Font Font;// { get; private set; }
    
    #endregion

    #region Constructor

    //TODO: Add LetterLib class and check in constructor if there has been a letter of this font created to prevent duplicates

    public Letter ()
    {
        Character = ' ';
        Font = LetterFactory.Instance.font;
        
    }
    public Letter(char character, Font font)
    {
        Character = character;
        Font = font;
    }
    public Letter(char character, bool[,] pixels, Font font)
    {
        Character = character;
        Pixels = pixels;
        Font = font;
    }

    #endregion

    #region Public Methods

    public void SetFont(Font font)
    {
        Font = font;
    }

    public void SetPixels(bool[,] pixels)
    {
        Pixels = pixels;
    }

    #endregion

    #region XML

    public void Save()
    {
        XmlPixels = new List<bool>();
        for (int x = 0; x < Pixels.GetLength(0); x++)
        {
            for(int y = 0; y < Pixels.GetLength(1); y++)
            {
                XmlPixels.Add(Pixels[x, y]);
            }
        }
        if (!Directory.Exists(Application.dataPath + "/LetterContainers/" + Font.name + "/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/LetterContainers/" + Font.name + "/");
            Debug.Log("Directory Created");
        }
        string filename = Application.dataPath + "/LetterContainers/" + Font.name + "/" + Character + ".xml";
        Save(filename);
    }

    #endregion

}
