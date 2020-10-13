using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyJSON : MonoBehaviour
{
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

    
}
