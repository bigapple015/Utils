using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Utils;

namespace MonitorClient.Models
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ClientConfig
    {
        public static ClientConfig I;

        #region Private Member

        /// <summary>
        /// 主服务器Ip
        /// </summary>
        private string serverIp;

        /// <summary>
        /// 主服务器端口
        /// </summary>
        private int serverPort;

        /// <summary>
        /// 扫描周期,默认是30秒
        /// </summary>
        private int singalPeriod = 30;

        /// <summary>
        /// 是监控客户端，还是用户监控助手
        /// 暂时忽略
        /// false,为监控客户端，true为监控页面
        /// </summary>
        private bool clientOrMonitor;

        #endregion

        #region Public Member

        /// <summary>
        /// 主服务器Ip
        /// </summary>
        public string ServerIp { get { return serverIp; } set { serverIp = value; } }

        /// <summary>
        /// 主服务器端口
        /// </summary>
        public int ServerPort { get { return serverPort; } set { serverPort = value; } }

        /// <summary>
        /// 信号周期
        /// </summary>
        public int SingalPeriod { get { return singalPeriod; } set { singalPeriod = value; } }

        /// <summary>
        /// 是监控客户端还是用户监控助手
        /// 
        /// false,为监控客户端，true为监控页面
        /// </summary>
        public bool ClientOrMonitor { get { return clientOrMonitor; } set { clientOrMonitor = value; } }

        #endregion


        #region Public Member

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="file">要加载的配置文件</param>
        /// <returns></returns>
        public static ClientConfig Load(String file)
        {
            if (!File.Exists(file))
            {
                LogUtils.Error(String.Format("找不到配置文件：{0}", file));
                return null;
            }
            ClientConfig clientConfig = null;
            try
            {
                clientConfig = ObjectXmlSerializer.ToObject<ClientConfig>(file);
                LogUtils.Info(String.Format("加载服务器配置文件{0}成功", file));
            }
            catch (Exception ex)
            {
                LogUtils.Error(String.Format("加载服务器配置文件{0}失败，异常信息为：{1}", file, ex));
            }

            return clientConfig;
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
                ObjectXmlSerializer.ToFile<ClientConfig>(this, file);
                LogUtils.Info(string.Format("保存服务器配置文件{0}成功", file));
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Error(string.Format("保存服务器配置文件{0}失败：{1}", file, ex));
                return false;
            }
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return singalPeriod > 5;
        }

        #endregion

    }
}
