using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CKFrame;

public class GameManager : SingletonMono<GameManager>
{
    public SaveItem SaveItem { get;private set; }
    public UserData UserData { get;private set; }
    
    public UserSetting UserSetting { get;private set; }

    private void Start()
    {
        UserSetting = SaveManager.LoadStting<UserSetting>();
        // 如果没有获取到用户设置，意味着首次进入游戏或者玩家删除了设置文件
        if (UserSetting == null)
        {
            UserSetting = new UserSetting(1, 1, 1, LanguageType.Chinese);
            SaveManager.SaveSetting(UserSetting);
        }
        
        //根据用户设置，设置相关的内容
        AudioManager.Instance.GlobalVolume = UserSetting.GlobalVolume;
        AudioManager.Instance.EffectVolume = UserSetting.EffectVolume;
        AudioManager.Instance.BGVolume = UserSetting.BGVolume;
        LocalizationManager.Instance.CurrentLanguageType = UserSetting.LanguageType;
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    public void EnterGame(SaveItem saveItem,UserData userData)
    {
        // 全局保存当前的存档和用户数据
        this.SaveItem = saveItem;
        this.UserData = userData;
        
    }
}
