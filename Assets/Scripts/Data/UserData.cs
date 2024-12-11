using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UserData
{
    public string UserName;
    public int Score;


    public UserData(string userName)
    {
        UserName = userName;
    }
}
