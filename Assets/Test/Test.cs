using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour,IStateMachineOwner
{
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UIManager.Instance.Show<Test_Window>();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            UIManager.Instance.Close<Test_Window>();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            UIManager.Instance.Show<Test_Window2>();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            UIManager.Instance.Close<Test_Window2>();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.CloseAll();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            UIManager.Instance.Show<Test_Window>(4);
        }
    }
}

