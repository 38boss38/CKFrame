using System;
using System.Collections;
using System.Collections.Generic;
using CKFrame;
using UnityEngine;

public class LVManager : LogicManagerBase<LVManager>
{
    private void Start()
    {
        // 打开游戏主窗口
        UIManager.Instance.Show<UI_GameMainWindow>();
        // 初始化玩家
        Player_Controller.Instance.Init(ConfigManager.Instance.GetConfig<Player_Config>("Player"));
    }

    protected override void RegisterEventListener()
    {
       
    }

    protected override void CancelEventListener()
    {
        
    }
}
