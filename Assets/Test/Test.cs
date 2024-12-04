using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TestSave
{
    public string Name;
}
public class Test : MonoBehaviour
{
    void Start()
    {
        SaveManager.SaveSetting(new TestSave(){Name = "123"});
        Debug.Log(SaveManager.LoadStting<TestSave>().Name);

    }
    
}

