using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class MyJSON : MonoBehaviour
{
    public static MyJSON data;

    public string playerName;
    public int lives;
    public float health;

    public void Start()
    {
        Debug.Log(SaveToString());
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    public void Save(string data)
    {
        try
        {
            File.AppendText(Application.persistentDataPath + "/gamedata.json");
        }
        catch(Exception e)
        {
            Debug.Log("Save data exception: " + e.Message);
        }

    }

    public void Load(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            data = JsonUtility.FromJson<MyJSON>(json);
        }
        catch(Exception e)
        {
            data = new MyJSON();
            Debug.Log("Load data exception: " + e.Message);
        }

    }
    
}
