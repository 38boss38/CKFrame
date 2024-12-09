using UnityEditor;
using UnityEngine;

namespace CKFrame
{
    public class GameRoot : SingletonMono<GameRoot>
    {
        /// <summary>
        /// 框架设置
        /// </summary>
        [SerializeField] private GameSetting gameSetting;
    
        public GameSetting GameSetting
        {
            get { return gameSetting; }
        }
    
        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
    
            base.Awake();
            DontDestroyOnLoad(gameObject);
    
            // 初始化所有管理器
            InitManagers();
        }
    
        private void InitManagers()
        {
            ManagerBase[] managers = GetComponents<ManagerBase>();
            for (int i = 0; i < managers.Length; i++)
            {
                managers[i].Init();
            }
        }
        
        
    #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void InitForEditor()
        {
            // 当前是否要进行播放或准备播放中
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            if (Instance == null && GameObject.Find("GameRoot")!=null)
            {
                Instance = GameObject.Find("GameRoot").GetComponent<GameRoot>();
                // 清空事件
                EventManager.Clear();
                Instance.InitManagers();
                Instance.GameSetting.InitForEditor();
            }
        }
    #endif
    }
}
