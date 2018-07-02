using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


//[XmlRoot("LetterContainer")]
public class LetterContainer  {

    
    public Font font;
    public string name;

   // [XmlArray("Letters"), XmlArrayItem("Letter")]
    public Letter[] Letters;

    public LetterContainer()
    {

    }

    public LetterContainer(Font font, Letter[] letters)
    {
        this.font = font;
        Letters = letters;
    }

    public LetterContainer(string name, Letter[] letters)
    {
        this.name = name;
        Letters = letters;
    }

    public void Save(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LetterContainer));

        using (StreamWriter stream = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static LetterContainer Load(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LetterContainer));

        using (StreamReader stream = new StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            LetterContainer data = serializer.Deserialize(stream) as LetterContainer;
            return data;
        }
    }

    public static LetterContainer LoadLetters(Font font)
    {
        if (!Directory.Exists(Application.dataPath + "/LetterContainers/" + font.name + "/"))
        {
            LetterContainer letterContainer = new LetterContainer(font, null);
            List<Letter> letters = new List<Letter>();
            foreach(string file in Directory.GetFiles(Application.dataPath + "/LetterContainers/" + font.name + "/"))
            {
                letters.Add(Letter.Load(Application.dataPath + "/LetterContainers/" + font.name + "/" + file));
                Debug.Log(file);
            }
            letterContainer.Letters = letters.ToArray();
            return letterContainer;
        }
        else
        {
            Debug.LogError("Letter folder not found");
            return null;
        }
    }

    public static LetterContainer LoadLetters(string folderPath)
    {
        Debug.Log(folderPath);
        if (!Directory.Exists(folderPath + "/"))
        {

            LetterContainer letterContainer = new LetterContainer(Path.GetDirectoryName(folderPath), null);
            List<Letter> letters = new List<Letter>();
            foreach (string file in Directory.GetFiles(folderPath))
            {
                letters.Add(Letter.Load(folderPath + file));
                Debug.Log(file);
            }
            letterContainer.Letters = letters.ToArray();
            return letterContainer;
        }
        else
        {
            Debug.LogError("Letter folder not found");
            return null;
        }
    }

    public static LetterContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(LetterContainer));
        return serializer.Deserialize(new StringReader(text)) as LetterContainer;
    }
}
