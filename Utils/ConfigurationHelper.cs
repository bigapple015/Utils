using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace IvapEngineTest
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 获取配置项的值
        /// </summary>
        /// <param name="configName">配置项名称</param>
        /// <param name="defaultValue">在读取配置项的值失败的情况下返回给调用层的默认值</param>
        /// <returns></returns>
        public static string GetConfigValue(string configName, string defaultValue)
        {
            try
            {
                if (ConfigurationManager.AppSettings[configName] == null)
                {
                    return defaultValue;
                }
                return ConfigurationManager.AppSettings[configName].ToString();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
