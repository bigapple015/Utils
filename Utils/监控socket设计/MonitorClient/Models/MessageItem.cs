using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorClient.Models
{
    public class MessageItem
    {
        #region Private Member
        private string message;

        #endregion

        #region Public Method
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        public string GetMessage()
        {
            return message;
        }
        #endregion

        public MessageItem(string message)
        {
            // TODO: Complete member initialization
            this.message = message;
        }

        /// <summary>
        /// 解析处理数据
        /// </summary>
        public void Parse()
        {
        }
    }
}
