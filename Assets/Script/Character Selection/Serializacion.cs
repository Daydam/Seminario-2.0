using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

public static class Serializacion
{
    public static void SaveJsonToDisk<T>(this T classToSerialize, string path)
    {
        string json = JsonUtility.ToJson(classToSerialize);
        File.WriteAllText(Application.persistentDataPath + "/" + path + ".json", json);
    }
    
    public static T LoadJsonFromDisk<T>(string path)
    {
        if(File.Exists(Application.persistentDataPath + "/" + path + ".json"))
        {
            TextAsset fileToLoad = Resources.Load(path) as TextAsset;
            T data = JsonUtility.FromJson<T>(File.ReadAllText(Application.persistentDataPath + "/" + path + ".json"));
            return data;
        }
        return default(T);
    }
}