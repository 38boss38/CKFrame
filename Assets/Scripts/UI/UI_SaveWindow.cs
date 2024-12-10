using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CKFrame;
using Unity.VisualScripting;

[UIElement(true,"UI/SaveWindow",1)]
public class UI_SaveWindow : UI_WindowBase
{
    [SerializeField] private Button Close_Button;
    [SerializeField] private Transform ItemParent;
    private List<UI_SaveItem> UI_SaveItemList = new List<UI_SaveItem>();
    
    private bool wantUpdate = true;

    public override void Init()
    {
        Close_Button.onClick.AddListener(OnCloseClick);
    }

    public override void OnShow()
    {
        base.OnShow();
        if (wantUpdate)
        {
            UpdateAllSaveItem();
        }
        
    }

    public override void Close()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button",UIManager.Instance);
        base.Close();
    }

    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
        EventManager.AddEventListener("UpdateSaveItem",UpdateSaveItemFlag);
    }

    private void UpdateSaveItemFlag()
    {
        wantUpdate = true;
    }

    /// <summary>
    /// 更新全部存档项
    /// </summary>
    private void UpdateAllSaveItem()
    {
        // 清空已有的
        for (int i = 0; i < UI_SaveItemList.Count; i++)
        {
            UI_SaveItemList[i].Destroy();
        }
        UI_SaveItemList.Clear();
        
        // 放置新的
        List<SaveItem> saveItems = SaveManager.GetAllSaveItemByUpdateTime();
        for (int i = 0; i < saveItems.Count; i++)
        {
            UI_SaveItem item = ResManager.Load<UI_SaveItem>("UI/SaveItem", ItemParent);
            item.Init(saveItems[i]);
            UI_SaveItemList.Add(item);
        }
        
        // 更新标志
        wantUpdate = false;
    }
}
