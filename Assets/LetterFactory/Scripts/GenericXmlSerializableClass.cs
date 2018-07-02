using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public abstract class GenericXmlSerializableClass<T> where T : class
{

    public void Save(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        
        using (StreamWriter stream = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static T Load(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (StreamReader stream = new StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            T data = serializer.Deserialize(stream) as T;
            return data;
        }
    }

}
