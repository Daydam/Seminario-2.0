using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public static class Serializacion
{
    public static void SaveDataToDisk<T>(this T classToSerialize, string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        
        bf.Serialize(file, classToSerialize);
        
        file.Close();
    }
    
    public static T LoadDataFromDisk<T>(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            
            T newClass = (T)bf.Deserialize(file);
            
            file.Close();
            return newClass;
        }
        else return default(T);
    }
}