using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace com.cmlu.commons
{
    public class WebClientHelper
    {
        /// <summary>
        /// 从指定地址下载文件到本地
        /// DownloadFile("http://www.baidu.com/", "result.txt");
        /// </summary>
        /// <param name="address"></param>
        /// <param name="fileName">本地存储文件的地址</param>
        public static void DownloadFile(string address,string fileName)
        {
            WebClient client = new WebClient();
            client.DownloadFile(address,fileName);
        }

        /// <summary>
        /// 读取网站内容
        /// </summary>
        /// <param name="address"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DownloadStringUseStream(string address,Encoding encoding)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(address);
            StreamReader reader = new StreamReader(stream,encoding);
            StringBuilder sb = new StringBuilder();
            string line = null;
            while((line = reader.ReadLine())!=null)
            {
                sb.AppendLine(line);
            }
            reader.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 读取网站内容
        /// </summary>
        /// <param name="address"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DownloadString(String address,Encoding encoding)
        {
            WebClient client = new WebClient();
            client.Encoding = encoding;
            return client.DownloadString(address);
        }

        /// <summary>
        /// 发送web请求
        /// WebRequest  WebResponse  HttpWebRequest  HttpWebResponse
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string SendWebRequest(string address)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Timeout = 10000;
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.Default);
            StringBuilder sb = new StringBuilder();
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                sb.AppendLine(line);
            }
            reader.Close();
            return sb.ToString();
        }

        /// <summary>
        /// 开启ie
        /// </summary>
        /// <param name="address"></param>
        public static void StartIE(string address)
        {
            Process process = new Process();
            process.StartInfo.FileName = "iexplore.exe";
            process.StartInfo.Arguments = address;
            process.Start();
        }
    }
}
