using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;

namespace iFlyTek.ECSS30.Demo.IvrNMGUnicom
{
    /// <summary>
    /// 获取应用程序配置的工具类
    /// </summary>
    public static class ConfigUtils
    {
        /// <summary>
        /// 获取配置字符串
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static string GetString(string key,string defaultValue)
        {
            if(string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                string value = WebConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch(Exception)
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
        public static int GetInteger(string key,int defaultValue)
        {
            if(string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                int value;
                if(int.TryParse(WebConfigurationManager.AppSettings[key],out value))
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
        /// 获取配置的double值
        /// </summary>
        /// <param name="key">配置文件的键</param>
        /// <param name="defaultValue">获取不到对应的键值时的默认返回值</param>
        /// <returns></returns>
        public static double GetDouble(string key,double defaultValue)
        {
            if(string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }

            try
            {
                double value;
                if(double.TryParse(WebConfigurationManager.AppSettings[key],out value))
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
    }
}