using UnityEngine;
using UnityEngine.UI;
using CKFrame;
using UnityEngine.Serialization;

[UIElement(true,"UI/SettingWindow",1)]
public class UI_SettingWindow : UI_WindowBase
{
    [SerializeField] private Button Close_Button;
    [SerializeField] private Text GlobalVolume_Text;
    [SerializeField] private Text BGVolume_Text;
    [SerializeField] private Text EffectVolume_Text;
    [SerializeField] private Text LanguageType_Text;
    
    [SerializeField] private Slider GlobalVolume_Slider;
    [SerializeField] private Slider BGVolume_Slider;
    [SerializeField] private Slider EffectVolume_Slider;
    [SerializeField] private Dropdown LanguageType_Dropdown;

    private const string LocalSetPackName = "UI_SettingWindow";
    public override void Init()
    {
        Close_Button.onClick.AddListener(Close);
        GlobalVolume_Slider.onValueChanged.AddListener(GlobalVolume_Slider_Valuechanged);
        BGVolume_Slider.onValueChanged.AddListener(BGVolume_Slider_Valuechanged);
        EffectVolume_Slider.onValueChanged.AddListener(EffectVolume_Slider_Valuechanged);
        LanguageType_Dropdown.onValueChanged.AddListener(LanguageType_Dropdown_Valuechanged);
    }

    public override void OnShow()
    {
        base.OnShow();
        GlobalVolume_Slider.value = GameManager.Instance.UserSetting.GlobalVolume;
        BGVolume_Slider.value = GameManager.Instance.UserSetting.BGVolume;
        EffectVolume_Slider.value = GameManager.Instance.UserSetting.EffectVolume;
        LanguageType_Dropdown.value = (int)GameManager.Instance.UserSetting.LanguageType;
    }

    public override void OnClose()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button",UIManager.Instance);
        base.OnClose();
    }

    protected override void OnUpdateLanguage()
    {
        GlobalVolume_Text.CKLocaSet(LocalSetPackName,"GlobalVolume");
        BGVolume_Text.CKLocaSet(LocalSetPackName,"BGVolume");
        EffectVolume_Text.CKLocaSet(LocalSetPackName,"EffectVolume");
        LanguageType_Text.CKLocaSet(LocalSetPackName,"LanguageType");
    }
    
    private void GlobalVolume_Slider_Valuechanged(float value)
    {
        GameManager.Instance.UserSetting.GlobalVolume = value;
        AudioManager.Instance.GlobalVolume = value;
        SaveManager.SaveSetting(GameManager.Instance.UserSetting);
    }
    private void BGVolume_Slider_Valuechanged(float value)
    {
        GameManager.Instance.UserSetting.BGVolume = value;
        AudioManager.Instance.BGVolume = value;
        SaveManager.SaveSetting(GameManager.Instance.UserSetting);
    }
    private void EffectVolume_Slider_Valuechanged(float value)
    {
        GameManager.Instance.UserSetting.EffectVolume = value;
        AudioManager.Instance.EffectVolume = value;
        SaveManager.SaveSetting(GameManager.Instance.UserSetting);
    }
    private void LanguageType_Dropdown_Valuechanged(int value)
    {
        LanguageType language = (LanguageType)value;
        GameManager.Instance.UserSetting.LanguageType = language;
        LocalizationManager.Instance.CurrentLanguageType = language;
        SaveManager.SaveSetting(GameManager.Instance.UserSetting);
    }
}
