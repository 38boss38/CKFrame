using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class TestMono
{
    private Coroutine c;
    public TestMono()
    {
        this.AddUpdateListener(OnUpdate);
        c = this.StartCoroutine(DoAction());
        
    }
    
    private void OnUpdate()
    {
        Debug.Log("OnUpdate");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.RemoveUpdateListener(OnUpdate);
            this.StopCoroutine(c);
        }
    }

    IEnumerator DoAction()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("DoAction");
        }
    }
}
public class Test : MonoBehaviour
{
    private TestMono t;
    void Start()
    {
       //t = new TestMono();
       ResManager.LoadGameObjectAsync<CubeController>("Cube",Action,null);
    }

    void Action(CubeController cubeController)
    {
        Debug.Log(cubeController.transform.name);
    }
    
}

