using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("TestPool OnEnable");
        Invoke("Dead",3);
    }

    void Dead()
    {
        PoolManager.Instance.PushGameObject(gameObject);
    }
}
