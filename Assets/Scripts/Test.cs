using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CKFrame;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveItem saveItem = SaveManager.CreateSaveItem();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.RemoveEventListener("Test",Test2);
            Debug.Log("a");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            EventManager.EventTrigger("Test");
            Debug.Log("d");
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            EventManager.Clear();
            Debug.Log("B");
        }
    }

    private void Test2()
    {
        Debug.Log("Test");
    }
}
