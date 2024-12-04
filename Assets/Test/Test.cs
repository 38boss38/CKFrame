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
        //SaveManager.SaveObject(new TestSave(){Name = "张三"});
        Debug.Log(SaveManager.LoadObject<TestSave>().Name);
        Debug.Log(SaveManager.LoadObject<TestSave>().Name);
    }
    
}

