using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : SingletonMono<GameRoot>
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();
        DontDestroyOnLoad(gameObject);
        
        // 初始化所有管理器
        InitMangers();
    }

    private void InitMangers()
    {
        ManagerBase[] managers = GetComponents<ManagerBase>();
        for (int i = 0; i < managers.Length; i++)
        {
            managers[i].Init();
        }
    }
}