using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using System.IO;

namespace WebClientHelper
{
    /// <summary>
    /// 注默认的webclient超时时间1分40秒，且不可以设置
    /// </summary>
    public class WebClientProxy
    {
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private WebClientProxy()
        {
        }

        /// <summary>
        /// 将指定的名称/值集合上载到指定的url
        /// </summary>
        /// <param name="address"></param>
        /// <param name="queryData"> </param>
        /// <param name="data">可以通过Request[key]，来获取；但不能通过Request.QueryString[]来获取；也不能通过控件（这里指asp.net的控件，但纯asp和jsp可以）来获取，</param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string UploadValues(string address,NameValueCollection queryData,NameValueCollection data,Encoding encode)
        {
            using(WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                client.QueryString.Add(queryData);
                byte[] array = client.UploadValues(address, data);
                return encode.GetString(array);
            }
        }

        /// <summary>
        /// 不提交任何额外的请求参数，直接请求页面
        /// </summary>
        /// <param name="address"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string DownloadString(string address,Encoding encode)
        {
            using(WebClient client = new WebClient())
            {
                //client.UseDefaultCredentials = true;
                client.Encoding = encode;
                Uri uri = new Uri(address,UriKind.Absolute);
                return client.DownloadString(uri);
            }
        }

        /// <summary>
        /// 使用默认的编码方式请求页面数据
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string DownloadString(string address)
        {
            return DownloadString(address, Encoding.Default);
        }


        /*
          尝试给asp.net的网站的textbox赋值，并解析返回的字符串
         */
        public static string JustTest(string address)
        {
            using (WebClient client = new WebClient())
            {
                //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //NameValueCollection collection = new NameValueCollection();
                string postData = "TextBox1={0}&TextBox2={1}&__VIEWSTATE={2}&__EVENTVALIDATION={3}";
                string tb1 = HttpUtility.UrlEncode("合肥");
                string tb2 = HttpUtility.UrlEncode("中文TextBox2");
                //collection.Add("TextBox1", tb1);
                //collection.Add("TextBox2", "中文TextBox2");
                string viewstate =
                    HttpUtility.UrlEncode("/wEPDwUKLTg0OTIzODQwNWRka0tEz3imhPBwOiGQitZvV1i4YnU=");
                string eventvalidation =
                    HttpUtility.UrlEncode("/wEWBALilPKSDwLs0bLrBgLs0fbZDAKM54rGBvarFlsAFIuybnFjYObWIZYDQw0o");
                postData = string.Format(postData, tb1, tb2, viewstate, eventvalidation);
                //collection.Add("__VIEWSTATE", viewstate);
                //collection.Add("__EVENTVALIDATION", eventvalidation);
                byte[] array = Encoding.ASCII.GetBytes(postData);
                //return Encoding.UTF8.GetString(client.UploadValues(address, "POST", collection));
                return Encoding.UTF8.GetString(client.UploadData(address,"POST",array));
            }
            #region 废弃
            /*
            HttpWebRequest req = (HttpWebRequest) HttpWebRequest.Create(address);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
            req.ContentLength = array.Length;
            using(Stream stream = req.GetRequestStream())
            {
                stream.Write(array,0,array.Length);
            }
            string str = string.Empty;
            using(WebResponse wr = req.GetResponse())
            {
                using(Stream stream = wr.GetResponseStream())
                {
                    byte[] bs = new byte[4096];
                    int i;
                    
                    while ((i = stream.Read(bs,0,bs.Length))!= 0)
                    {
                        str += Encoding.UTF8.GetString(bs, 0, i);
                    }
                    
                }
            }
            return str;*/
            #endregion
        }
    }
}
