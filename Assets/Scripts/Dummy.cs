using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Dummy : MonoBehaviour
{
    private int mHealth;
    private int mMeowCount;
    private string mName;
    private Vector3 position;

    public Dummy()
    {
        mHealth = 10;
        mMeowCount = 3;
        mName = "NewDummy";
        position = Vector3.zero;
    }

    public byte[] Serialize()
    {
        var size = sizeof(int) * 2 + sizeof(float) * 3;
        byte[] mByte = new byte[size];

        var healthBytes = BitConverter.GetBytes(mHealth);

        for (int i = 0; i < sizeof(int); i++)
        {
            mByte[i] = healthBytes[i];
        }

        var meowBytes = BitConverter.GetBytes(mMeowCount);

        for (int i = 0; i < sizeof(int); i++)
        {
            mByte[i + sizeof(int)] = meowBytes[i];
        }

        var xByte = BitConverter.GetBytes(position.x);

        for (int i = 0; i < sizeof(float); i++)
        {
            mByte[i + sizeof(int) * 2] = xByte[i];
        }

        var yByte = BitConverter.GetBytes(position.y);

        for (int i = 0; i < sizeof(float); i++)
        {
            mByte[i + sizeof(int) * 2 + sizeof(float)] = yByte[i];
        }

        var zByte = BitConverter.GetBytes(position.z);

        for (int i = 0; i < sizeof(float); i++)
        {
            mByte[i + sizeof(int) * 2 + sizeof(float) * 2] = zByte[i];
        }

        return mByte;

    }
}
