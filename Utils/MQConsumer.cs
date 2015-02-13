using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Utils;
using 电报解析.Models;
using 电报解析.Utils;

namespace 电报解析.Utils
{
    public static class MQConsumer
    {
        //创建连接工厂
        private static IConnectionFactory factory = new ConnectionFactory(ConfigurationManager.AppSettings["MQUri"]);
        //连接
        private static IConnection connection = null;
        //会话
        private static ISession session = null;

        private static Boolean isAlive = false;

        /// <summary>
        /// 本地ICAO码
        /// </summary>
        private static String localIcao = ConfigurationManager.AppSettings["LocalIcao"];

        /// <summary>
        /// init 消费者
        /// </summary>
        public static bool InitConsumer()
        {
            try
            {
                if (connection != null)
                {
                    CloseConsumer();
                }
                NLogHelper.Info("开始初始化MQConsumer");
                //通过工厂构建链接
                connection = factory.CreateConnection();
                //设置连接的标识
                connection.ClientId = "HuiZhouDatagramPaser";
                connection.ExceptionListener += connection_ExceptionListener;
                connection.ConnectionInterruptedListener += connection_ConnectionInterruptedListener;
                //启动连接
                connection.Start();
                //通过连接创建一个对话
                session = connection.CreateSession();
                //通过会话创建一个消费者
                IMessageConsumer consumer =
                    session.CreateConsumer(new ActiveMQQueue(ConfigurationManager.AppSettings["MQName"]));
                //注册监听事件
                consumer.Listener += ConsumerOnListener;
                isAlive = true;
                NLogHelper.Info("初始化MQConsumer成功,等待接收报文... ...");
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Error("初始化MQ失败："+ex);
                return false;
            }
        }

        /// <summary>
        /// 连接断开
        /// </summary>
        private static void connection_ConnectionInterruptedListener()
        {
            isAlive = false;
            NLogHelper.Error("ConnectionInterruptedListener连接发生异常连接断开");
        }

        /// <summary>
        /// 连接异常
        /// </summary>
        /// <param name="exception"></param>
        private static void connection_ExceptionListener(Exception exception)
        {
            isAlive = false;
            NLogHelper.Error("connection_ExceptionListener连接发生异常："+exception);
        }

        /// <summary>
        /// 关闭消费者
        /// </summary>
        public static void CloseConsumer()
        {
            try
            {
                NLogHelper.Info("开始关闭MQ Consumer");
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
                NLogHelper.Error("关闭MQ连接失败："+ex);
            }
        }

        /// <summary>
        /// MQ是否或者存活
        /// </summary>
        /// <returns></returns>
        public static Boolean IsAlive()
        {
            return isAlive;
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="message"></param>
        private static void ConsumerOnListener(IMessage message)
        {
            try
            {
                isAlive = true;

            }
            catch (Exception ex)
            {
            }
        }

    }
}
