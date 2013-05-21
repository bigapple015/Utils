using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;

//添加到项目中时，修改命名空间
//本帮助类基于一个假设：配置文件中的key是唯一的。

namespace com.lib.config
{
    public static class ConfigHelper
    {
        /// <summary>
        /// 通过键来获取值，如果没有找到相应的键，则返回null
        /// </summary>
        /// <param name="key">要查找的键</param>
        /// <returns>查找到的值</returns>
        public static string GetKeyByValue(string key)
        {
            //键不能为null或string.Empty
            Debug.Assert(!string.IsNullOrEmpty(key));

            return ConfigurationManager.AppSettings.Get(key);
        }

        /// <summary>
        /// 用来检测配置文件中的键是否存在。
        /// </summary>
        /// <param name="key">要检测的键</param>
        /// <returns>返回true，则键存在，返回false，则键不存在。</returns>
        public static bool IsKeyExist(string key)
        {
            //键不能为null或string.Empty
            Debug.Assert(!string.IsNullOrEmpty(key));

            if (ConfigurationManager.AppSettings.Get(key) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
