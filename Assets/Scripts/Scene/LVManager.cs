using System;
using System.Collections;
using System.Collections.Generic;
using CKFrame;
using UnityEngine;

public class LVManager : LogicManagerBase<LVManager>
{
    private void Start()
    {
        Debug.Log("游戏开始了");
    }

    protected override void RegisterEventListener()
    {
       
    }

    protected override void CancelEventListener()
    {
        
    }
}
