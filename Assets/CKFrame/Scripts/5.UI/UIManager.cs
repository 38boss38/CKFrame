using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : ManagerBase<UIManager>
{
    #region 内部类
    [Serializable]
    private class UILayer
    {
        public Transform root;
        public Image maskImage;
        private int count = 0;

        public void OnShow()
        {
            count += 1;
            Update();
        }

        public void OnClose()
        {
            count -= 1;
            Update();
        }

        private void Update()
        {
            maskImage.raycastTarget = count != 0;
            int posIndex = root.childCount - 2;
            maskImage.transform.SetSiblingIndex(posIndex < 0 ? 0 : posIndex);
        }
    }
    
    #endregion
    /// <summary>
    /// 元素资源库
    /// </summary>
    public Dictionary<Type, UIElement> UIElementsDic
    {
        get { return GameRoot.Instance.GameSetting.UIElementsDic; }
    }
    
    [SerializeField]
    private UILayer[] UILayers;
    
    // 提示窗
    [SerializeField]
    private UITips UITips;

    public void AddTips(string info)
    {
        UITips.AddTips(info);
    }

    /// <summary>
    /// 显示窗口
    /// </summary>
    /// <param name="layer">层级 -1等于不设置</param>
    /// <typeparam name="T">窗口类型</typeparam>
    public T Show<T>(int layer = -1) where T : UI_WindowBase
    {
        return Show(typeof(T), layer) as T;
    }
    
    /// <summary>
    /// 显示窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <param name="layer">层级 -1等于不设置</param>
    public UI_WindowBase Show(Type type,int layer = -1)
    {
        if (UIElementsDic.ContainsKey(type))
        {
            UIElement info = UIElementsDic[type];
            int layerNum = layer == -1?info.layerNum:layer;
            
            // 实例化或者获取到实例，保证窗口实例存在
            if (info.objInstance != null)
            {
                info.objInstance.gameObject.SetActive(true);
                info.objInstance.transform.SetParent(UILayers[layerNum].root);
                info.objInstance.transform.SetAsLastSibling();
                info.objInstance.OnShow();
            }
            else
            {
                UI_WindowBase window = ResManager.InstantiateForPrefab(info.prefab, UILayers[layerNum].root).GetComponent<UI_WindowBase>();
                info.objInstance = window;
                window.Init();
                window.OnShow();
            }
            
            info.layerNum = layerNum;
            UILayers[layerNum].OnShow();
            return info.objInstance ;
        }
        // 资源库中没有意味着不允许显示
        return null;
    }
    
    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <typeparam name="T">窗口类型</typeparam>
    public void Close<T>()
    {
        Close(typeof(T));
    }
    
    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    public void Close(Type type)
    {
        if (UIElementsDic.ContainsKey(type))
        {
            UIElement info = UIElementsDic[type];
            if (info.objInstance == null) return;
            
            // 缓存则隐藏
            if (info.isCache)
            {
                info.objInstance.transform.SetAsFirstSibling();
                info.objInstance.gameObject.SetActive(false);
            }
            // 不缓存则销毁
            else
            {
                Destroy(info.objInstance.gameObject);
                info.objInstance = null;
            }
            UILayers[info.layerNum].OnClose();
        }
    }

    /// <summary>
    /// 关闭全部窗口
    /// </summary>
    public void CloseAll()
    {
        //处理缓存中所有状态的逻辑
        var enumerator = UIElementsDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.objInstance.Close();
        }
    }
}
