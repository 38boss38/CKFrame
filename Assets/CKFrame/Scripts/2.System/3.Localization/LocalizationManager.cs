using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 本地化管理器
/// 持有本地化配置
/// 提供本地化内容获取函数
/// </summary>
public class LocalizationManager : ManagerBase<LocalizationManager>
{
    // 本地化配置资源
    public LocalizationSetting localizationSetting;
    
    [SerializeField]
    [OnValueChanged("UpdataLanguage")]
    private LanguageType currentLanguageType;

    public LanguageType CurrentLanguageType
    {
        get => currentLanguageType;
        set
        {
            currentLanguageType = value;
            UpdataLanguage();
        }
    }

    /// <summary>
    /// 获取当前语言设置下的内容
    /// </summary>
    /// <param name="bagName">包名称</param>
    /// <param name="contenKey">内容名称</param>
    /// <typeparam name="T">具体类型</typeparam>
    /// <returns></returns>
    public T GetContent<T>(string bagName, string contenKey) where T : class, ILanguage_Content
    {
        return localizationSetting.GetContent<T>(bagName, contenKey, currentLanguageType);
    }
    
    /// <summary>
    /// 更新语言
    /// </summary>
    private void UpdataLanguage()
    {
#if UNITY_EDITOR
        GameRoot.InitForEditor();
        // TODO:UI框架相关事项
#endif
        //触发更新语言事件
        EventManager.EventTrigger("UpdateLanguage");
    }
}
