using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[UIElement(true,"UI/UI_LoadingWindow",4)]
public class UI_LoadingWindow : UI_WindowBase
{
    [SerializeField]
    private Text progress_Text;
    [SerializeField]
    private Image fill_Image;

    public override void OnShow()
    {
        base.OnShow();
        UpdateProgress(0);
    }

    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
        EventManager.AddEventListener<float>("LoadingScene",UpdateProgress);
        EventManager.AddEventListener("LoadSceneSucceed",OnLoadSceneSucceed);
    }

    protected override void CancelEventListener()
    {
        base.CancelEventListener();
        EventManager.RemoveEventListener<float>("LoadingScene",UpdateProgress);
        EventManager.RemoveEventListener("LoadSceneSucceed",OnLoadSceneSucceed);
    }

    private void OnLoadSceneSucceed()
    {
        Close();
    }

    /// <summary>
    /// 更新进度
    /// </summary>
    private void UpdateProgress(float progressValue)
    {
        progress_Text.text = progressValue.ToString("0.0") + "%";
        fill_Image.fillAmount = 0f;
    }
}
