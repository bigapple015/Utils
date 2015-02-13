using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Utils;

namespace 电报解析.Utils
{
    /// <summary>
    /// MQ产生者
    /// </summary>
    public class MQProducer
    {
        //创建连接工厂
        private static IConnectionFactory factory = new ConnectionFactory(ConfigurationManager.AppSettings["FidsMQUri"]);
        //连接
        private static IConnection connection = null;
        //会话
        private static ISession session = null;

        private static IMessageProducer responseProducer = null;

        /// <summary>
        /// 发布启用·
        /// </summary>
        public static void ProducerStart()
        {
            NLogHelper.Info("开始启动Producer");
            connection = factory.CreateConnection();
            session = connection.CreateSession();
            responseProducer =
                session.CreateProducer(
                    session.GetDestination(ConfigurationManager.AppSettings["ActivemqDestination"].Trim(),
                        DestinationType.Topic));
            if (connection != null && !connection.IsStarted)
            {
                connection.Start();
                NLogHelper.Info("启动Producer成功");
            }
        }

        /// <summary>
        /// 发布关闭
        /// </summary>
        public static void ProducerClose()
        {
            NLogHelper.Info("开始关闭Producer");
            try
            {
                if (session != null)
                {
                    session.Dispose();
                    session.Close();
                }
                session = null;
            }
            catch (Exception ex)
            {
                NLogHelper.Error("关闭MQ Session失败：" + ex);
            }
            try
            {
                if (connection != null)
                {
                    connection.Stop();
                    connection.Dispose();
                    connection.Close();
                }
                connection = null;
            }
            catch (Exception ex)
            {
                NLogHelper.Error("关闭MQ连接失败：" + ex);
            }
        }

        /// <summary>
        /// 发送字符串消息
        /// </summary>
        /// <param name="message"></param>
        public static void SendMessage(String message)
        {
            try
            {
                NLogHelper.Info("开始给航显发送消息：" + message);
                ProducerStart();
                if (responseProducer != null && session != null)
                {
                    responseProducer.Send(session.CreateTextMessage(message));
                }
                NLogHelper.Info("发送消息成功");
            }
            catch (Exception ex)
            {
                NLogHelper.Error("发送给航显消息失败：" + ex);
            }
            finally
            {
                ProducerClose();
            }
        }
    }
}
