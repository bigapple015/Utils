using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace Utility
{
    public static class LogWriter
    {
        /// <summary>
        /// 在数据库中插入一条新的日志信息
        /// </summary>
        /// <param name="logType">日志的类型</param>
        /// <param name="logMessage">日志的信息，长度不超过500</param>
        /// <returns>返回受影响的行数</returns>
        public static int AddLog(LogType logType, string logMessage)
        {
            string sql = "INSERT INTO [Log]([LogTime],[LogType],[LogMessage]) VALUES(@LogTime,@LogType,@LogMessage)";
            //初始化sql语句中的参数
            SqlParameter[] paras = new SqlParameter[3];
            paras[0] = new SqlParameter("@LogTime", SqlDbType.DateTime);
            paras[0].Value = DateTime.Now;
            paras[1] = new SqlParameter("@LogType",SqlDbType.NVarChar);
            paras[1].Value = logType.ToString();
            paras[2] = new SqlParameter("@LogMessage",SqlDbType.NVarChar);
            paras[2].Value = logMessage;
            if (!SqlHelper.GetInstance().IsConnectionActive())
            {
                return -1;
            }
            return SqlHelper.GetInstance().ExecuteNonQuery(sql, CommandType.Text, paras);
        }

        /// <summary>
        /// 在发生错误时发送邮件
        /// </summary>
        /// <param name="title">邮件的标题</param>
        /// <param name="msg">邮件的内容</param>
        /// <returns>发送成功，返回true;失败，返回false。</returns>
        public static bool SendMail(string title, string msg)
        {
            try
            {
                //邮件服务器，比如这个就是126邮箱的发件服务器
                SmtpClient client = new SmtpClient(ConfigReader.GetValueByKey("smtpClient"));
                client.UseDefaultCredentials = false;
                //设置发送邮箱的用户名，发送邮箱的密码
                client.Credentials = new System.Net.NetworkCredential(ConfigReader.GetValueByKey("mailFrom"), DESHelper.MD5Decrypt(ConfigReader.GetValueByKey("password")));
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage();
                //发件人地址
                mail.From = new MailAddress(ConfigReader.GetValueByKey("mailFrom"));
                //设置收件人集合
                mail.To.Add(ConfigReader.GetValueByKey("mailTo"));
                //设置抄送人集合
                //mail.CC.Add();
                //邮件的标题
                mail.Subject = title;
                mail.BodyEncoding = System.Text.Encoding.Default;
                //邮件的内容
                mail.Body = msg;
                //邮件的附件
                //if (list != null)
                //{
                //    foreach (Attachment item in list)
                //    {
                //        mail.Attachments.Add(item);
                //    }
                //}
                mail.IsBodyHtml = true;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                AddLog(LogType.Warning, "不能发送邮件,异常信息为："+ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 获取代码行号的函数
        /// </summary>
        /// <returns></returns>
        public static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }

        /// <summary>
        /// 获得文件名函数
        /// </summary>
        /// <returns></returns>
        public static string GetFileName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileName();
        }

        /// <summary>
        /// 获得当前函数名
        /// </summary>
        /// <returns></returns>
        public static string GetFuncName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetMethod().ToString();
        }

    }

    /// <summary>
    /// 用来表示日志的类型，日志的重要性从Verbose到Error依次增加
    /// </summary>
    public enum LogType
    {
        Verbose,
        Info,
        Debug,
        Warning,
        Error,
        SQL
    }
}
