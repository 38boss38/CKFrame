using UnityEngine;
using UnityEngine.UI;

namespace CKFrame
{
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
            EventManager.AddEventListener<float>("LoadingSceneProgress",UpdateProgress);
            EventManager.AddEventListener("LoadSceneSucceed",OnLoadSceneSucceed);
        }
 
        protected override void CancelEventListener()
        {
            base.CancelEventListener();
            EventManager.RemoveEventListener<float>("LoadingSceneProgress",UpdateProgress);
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
            Debug.Log((int)progressValue * 100f + "%");
            progress_Text.text = (int)progressValue * 100f + "%";
            fill_Image.fillAmount = 0f;
        }
    }

}
