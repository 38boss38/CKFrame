using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TestMono
{
    
}
public class Test : MonoBehaviour
{
    void Start()
    {
        this.OnUpdate(Action);
        this.OnFixedUpdate(Action);
        this.OnLateUpdate(Action);
        
        this.RemoveUpdate(Action);
        this.RemoveLateUpdate(Action);
        this.RemoveFixedUpdate(Action);
    }

    void Action()
    {
        Debug.Log("Action");
    }
}

