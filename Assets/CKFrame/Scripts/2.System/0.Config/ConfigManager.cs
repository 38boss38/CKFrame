using UnityEngine;

namespace CKFrame
{
    public class ConfigManager : ManagerBase<ConfigManager>
    {
        [SerializeField]
        private ConfigSetting configSetting;

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="id">id</param>
        /// <typeparam name="T">具体的配置类型</typeparam>
        /// <returns></returns>
        public T GetConfig<T>(string configTypeName, int id = 0) where T : ConfigBase
        {
            return configSetting.GetConfig<T>(configTypeName, id);
        }
    }

}
