using System.Collections;
using UnityEngine;
using System;

namespace CKFrame
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public static class SceneManager 
    {
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="callback">回调函数</param>
        public static void LoadScene(string sceneName, Action callback = null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            callback?.Invoke();
        }
    
        /// <summary>
        /// 异步加载场景
        /// 会自动分发进度到事件中心，事件名称"LoadingSceneProgress"
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="callback">回调函数</param>
        public static void LoadSceneAsync(string sceneName, Action callback = null)
        {
            MonoManager.Instance.StartCoroutine(DoLoadSceneAsync(sceneName, callback));
        }

        private static IEnumerator DoLoadSceneAsync(string sceneName, Action callback = null)
        {
            AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            while (ao.isDone == false)
            {
                //把加载进度分发到事件中心
                EventManager.EventTrigger("LoadingSceneProgress",ao.progress);
                yield return ao.progress;
            }
            EventManager.EventTrigger<float>("LoadingSceneProgress",1f);
            callback?.Invoke();
        }
    }
}

