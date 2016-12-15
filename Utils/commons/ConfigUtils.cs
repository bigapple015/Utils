using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Scmis.Plc.Utils
{
    /// <summary>
    /// 获取应用程序配置的工具类
    /// 添加引用：System.Configuration.dll
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        /// 获取配置字符串
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static string GetString(string key, string defaultValue = "")
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                return value == null ? defaultValue : value.Trim();
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        
        /// <summary>
        /// 获取配置的整数值
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static int GetInteger(string key, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                int value;
                if (int.TryParse(ConfigurationManager.AppSettings[key], out value))
                {
                    return value;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetConnString(string key, string defaultValue = "")
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }

}
