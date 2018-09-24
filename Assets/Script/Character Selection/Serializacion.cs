using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public static class Serializacion
{
    public static void SaveJsonToDisk<T>(this T classToSerialize, string fileName)
    {
        string json = JsonUtility.ToJson(classToSerialize);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }
    
    public static T LoadJsonFromDisk<T>(string fileName)
    {
        if(File.Exists(Application.persistentDataPath + "/" + fileName + ".json"))
        {
            TextAsset fileToLoad = Resources.Load(fileName) as TextAsset;
            T data = JsonUtility.FromJson<T>(File.ReadAllText(Application.persistentDataPath + "/" + fileName + ".json"));
            return data;
        }
        return default(T);
    }
}