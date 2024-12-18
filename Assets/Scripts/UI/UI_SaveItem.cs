using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using CKFrame;

[Pool]
public class UI_SaveItem : MonoBehaviour
{
    private SaveItem saveItem;
    private UserData userData;
    
    [SerializeField] private Image BG;
    [SerializeField] private Text UserName_Text;
    [SerializeField] private Text Time_Text;
    [SerializeField] private Text Score_Text;
    [SerializeField] private Text Del_Button_Text;
    [SerializeField] private Button Del_Button;
    
    private static Color normalColor = new Color(0f, 0f, 0f, 0.6f);
    private static Color selectedColor = new Color(0.4f, 0.8f, 0.9f, 0.8f);
    
    private void Start()
    {
        Del_Button.onClick.AddListener(DelButtonClick);
        this.OnMouseEnter(MouseEnter);
        this.OnMouseExit(MouseExit);
        this.OnClick(Click);
    }

    private void OnEnable()
    {
        Del_Button_Text.CKLocaSet("UI_SaveWindow","SaveItem_DelButtonText");
    }

    public void Init(SaveItem saveItem)
    {
        this.saveItem = saveItem;
        
        Time_Text.text = saveItem.lastSaveTime.ToString("g");
        // 用户数据
        userData = SaveManager.LoadObject<UserData>(saveItem);
        UserName_Text.text = userData.UserName;
        Score_Text.text = userData.Score.ToString();
    }

    public void Destroy()
    {
        userData = null;
        saveItem = null;
        this.CKGameObjectPushPool();
    }

    public void DelButtonClick()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button",UIManager.Instance);
        SaveManager.DeleteSaveItem(saveItem);
        EventManager.EventTrigger("UpdateSaveItem");
        EventManager.EventTrigger("UpdateRankItem");
    }

    private void MouseEnter(PointerEventData eventData,params object[] args)
    {
        BG.color = selectedColor;
    }

    private void MouseExit(PointerEventData eventData,params object[] args)
    {
        BG.color = normalColor;
    }

    private void Click(PointerEventData eventData, params object[] args)
    {
        // 进入游戏
        EventManager.EventTrigger<SaveItem,UserData>("EnterGame", saveItem,userData);
    }
}
