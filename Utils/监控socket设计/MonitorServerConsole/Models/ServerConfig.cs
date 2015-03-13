using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Utils;
using MonitorServerConsole.Utils;

namespace MonitorServerConsole.Models
{
    /// <summary>
    /// 配置信息
    /// </summary>
    [Serializable]
    public class ServerConfig
    {
        public static ServerConfig I;

        #region Private member

        /// <summary>
        /// 服务器端口号
        /// </summary>
        private int serverPort;

        /// <summary>
        /// 服务器名
        /// </summary>
        public string serverName;

        /// <summary>
        /// 最多接收的连接数
        /// </summary>
        private int backlog;

        /// <summary>
        /// 是否是主服务器，如果为主服务器负责负载均衡
        /// </summary>
        private bool isMainServer;

        /// <summary>
        /// 扫描周期，单位为秒
        /// </summary>
        private int scannerPeriod;

        #endregion

        #region Public Properties

        /// <summary>
        /// 服务器端口号
        /// </summary>
        public int ServerPort{get{return serverPort;}set { serverPort = value; }}

        /// <summary>
        /// 服务器名
        /// </summary>
        public String ServerName { get { return serverName; }set { serverName = value; }}

        /// <summary>
        /// 最多接收的连接数
        /// </summary>
        public int Backlog { get { return backlog; } set { backlog = value; } }

        /// <summary>
        /// 是否是主服务器
        /// </summary>
        public bool IsMainServer { get { return isMainServer; } set { isMainServer = value; } }

        /// <summary>
        /// 设置扫描周期
        /// </summary>
        public int ScannerPeriod { get { return scannerPeriod; } set { scannerPeriod = value; } }
        
        #endregion

        #region Public Member

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="file">要加载的配置文件</param>
        /// <returns></returns>
        public static ServerConfig Load(String file)
        {
            if (!File.Exists(file))
            {
                LogUtils.Error(String.Format("找不到配置文件：{0}",file));
                return null;
            }
            ServerConfig serverConfig = null;
            try
            {
                serverConfig = ObjectXmlSerializer.ToObject<ServerConfig>(file);
                LogUtils.Info(String.Format("加载服务器配置文件{0}成功",file));
            }
            catch (Exception ex)
            {
                LogUtils.Error(String.Format("加载服务器配置文件{0}失败，异常信息为：{1}",file,ex));
            }
            
            return serverConfig;
        }

        /// <summary>
        /// 将配置信息序列化到文件中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool Save(string file)
        {
            try
            {
                ObjectXmlSerializer.ToFile<ServerConfig>(this,file);
                LogUtils.Info(string.Format("保存服务器配置文件{0}成功",file));
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Error(string.Format("保存服务器配置文件{0}失败：{1}",file,ex));
                return false;
            }
        }

        #endregion

    }
}
