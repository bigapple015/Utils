using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cares.Fids.Monitor.Models.Utils;
using MonitorServerConsole.Utils.Sockets;

namespace MonitorServerConsole.Models
{
    public class MessageItem
    {
        #region Private Member

        /// <summary>
        /// 要发送的消息
        /// </summary>
        private String message;

        /// <summary>
        /// 发送消息使用的socket
        /// 或者接受消息对应的socket
        /// </summary>
        private ClientEx clientEx;

        /// <summary>
        /// 重试次数
        /// </summary>
        private volatile int retryCount;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientEx"></param>
        public MessageItem(String message, ClientEx clientEx)
        {
            this.message = message;
            this.clientEx = clientEx;
        }

        #endregion

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// 获取客户端代理
        /// </summary>
        public ClientEx Client
        {
            get { return clientEx; }
        }

        /// <summary>
        /// 开始处理消息
        /// </summary>
        public void Parse()
        {
            try
            {
                LogUtils.Info("开始处理消息："+message);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
