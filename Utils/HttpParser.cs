using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*
 * 
 * 将http解析类和http模型类放在同一个文件中，这样是不好的行为，但是方便
 * 
 * 
 * 
 */
namespace Com.Utility.Commons
{

    #region 应答行
    /// <summary>
    /// 应答行 格式如下：
    /// HTTP-Version Status-Code Reason-Phrase CRLF
    /// </summary>
    public class ResponseLine
    {
        /// <summary>
        /// 服务器HTTP协议版本
        /// </summary>
        public string Http_Version { get; private set; }

        /// <summary>
        /// 服务器发回的响应状态码
        /// </summary>
        public string Status_Code { get; private set; }

        /// <summary>
        /// 表示状态代码的文本描述
        /// </summary>
        public string Reason_Phrase { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="responseline">应答行</param>
        public ResponseLine(string responseline)
        {
            if(string.IsNullOrEmpty(responseline))
            {
                IsParseSuccess = false;
                ErrorMsg = "应答行为null或empty";
                return;
            }

            //分割请求行
            string[] array = responseline.Split(new char[] {' '}, 3, StringSplitOptions.RemoveEmptyEntries);
            //array == null is always false
            if(array.Length != 3)
            {
                IsParseSuccess = false;
                ErrorMsg = string.Format("应答行[{0}]格式不正确", responseline);
                return;
            }

            //获取http协议版本号、响应状态码、文本描述
            Http_Version = array[0];
            Status_Code = array[1];
            Reason_Phrase = array[2];
            IsParseSuccess = true;
        }

        /// <summary>
        /// 是否解析成功
        /// </summary>
        public bool IsParseSuccess { get; private set; }

        /// <summary>
        /// 是否状态码值为200
        /// </summary>
        public bool Is200StatusCode { get { return Status_Code == "200"; } }

        /// <summary>
        /// 在解析失败时，用户可以用来获取失败原因
        /// </summary>
        public string ErrorMsg { get; private set; }
    }
    #endregion

    #region 应答包模型
    /// <summary>
    /// 应答包模型
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 应答行
        /// </summary>
        public ResponseLine ResponseLine { get; private set; }

        /// <summary>
        /// 解析应答包
        /// </summary>
        /// <param name="response"></param>
        public ResponseModel(string response)
        {
            //字符串为空
            if (string.IsNullOrEmpty(response))
            {
                IsParseSuccess = false;
                ErrorMsg = "应答包为null或空";
                return;
            }
            //读取http请求
            using (StringReader reader = new StringReader(response))
            {
                //读取应答行
                string responseLine = reader.ReadLine();
                //解析应答行
                ResponseLine = new ResponseLine(responseLine);
                //判断应答行解析情况
                if(!ResponseLine.IsParseSuccess)
                {
                    IsParseSuccess = false;
                    ErrorMsg = ResponseLine.ErrorMsg;
                    return;
                }
                //判断应答行状态码
                if (!ResponseLine.Is200StatusCode)
                {
                    IsParseSuccess = false;
                    ErrorMsg = "应答行状态码不为200";
                    return;
                }

                //解析请求报头
                string line = null;
                Headers = new Dictionary<string, string>();
                bool isSuccess = false;
                while((line = reader.ReadLine())!= null)
                {
                    //应答头
                    if(line != string.Empty)
                    {
                        string[] array = line.Split(new char[] {':'}, 2);
                        
                        if(array .Length != 2 || string.IsNullOrEmpty(array[0]))
                        {
                            continue;
                        }
                        else
                        {
                            if(Headers.ContainsKey(array[0]))
                            {
                                //包头包含了两个相同的字段
                                continue;
                            }
                            else
                            {
                                Headers.Add(array[0],array[1]);
                            }
                        }
                    }
                    else
                    {
                        //应答包体
                        ResponseBody = reader.ReadToEnd();
                        isSuccess = true;
                    }
                }

                if(Headers == null || Headers.Count == 0)
                {
                    IsParseSuccess = false;
                    ErrorMsg = "没有应答包头";
                    return;
                }

                if(isSuccess)
                {
                    IsParseSuccess = true;
                }
                else
                {
                    IsParseSuccess = false;
                    ErrorMsg = "没有应答包体";
                }
            }
        }

        /// <summary>
        /// 请求头
        /// </summary>
        public Dictionary<String, String> Headers { get; private set; }

        /// <summary>
        /// 应答包体
        /// </summary>
        public string ResponseBody { get; private set; }

        /// <summary>
        /// 是否解析成功
        /// </summary>
        public bool IsParseSuccess { get; private set; }

        /// <summary>
        /// 在解析失败时，用户可以用来获取失败原因
        /// </summary>
        public string ErrorMsg { get; private set; }
    }
    #endregion

    #region 请求行

    /// <summary>
    /// 请求行
    /// </summary>
    public class RequestLine
    {
        /// <summary>
        /// 请求方法
        /// </summary>
        public string Method;
        /// <summary>
        /// 统一资源标识符
        /// </summary>
        public string Request_Uri;
        /// <summary>
        /// 请求的http版本协议
        /// </summary>
        public string Http_Version;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="request"></param>
        public RequestLine(string request)
        {
            if(string.IsNullOrEmpty(request))
            {
                IsParseSuccess = false;
                ErrorMsg = "请求行为null或empty";
                return;
            }

            //分割请求行
            string[] array = request.Split(new char[] {' '}, 3, StringSplitOptions.RemoveEmptyEntries);
            if(array.Length != 3)
            {
                IsParseSuccess = false;
                ErrorMsg = string.Format("请求行{0}格式错误",request);
                return;
            }

            IsParseSuccess = true;
            Method = array[0];
            Request_Uri = array[1];
            Http_Version = array[2];
            return;
        }


        /// <summary>
        /// 是否解析成功
        /// </summary>
        public bool IsParseSuccess { get; private set; }

        /// <summary>
        /// 在解析失败时，用户可以用来获取失败原因
        /// </summary>
        public string ErrorMsg { get; private set; }
    }

    #endregion


    #region 请求包

    /// <summary>
    /// 请求包模型
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// 应答行
        /// </summary>
        public RequestLine RequestLine { get; private set; }

        /// <summary>
        /// 解析应答包
        /// </summary>
        /// <param name="request"></param>
        public RequestModel(string request)
        {
            //字符串为空
            if (string.IsNullOrEmpty(request))
            {
                IsParseSuccess = false;
                ErrorMsg = "请求包为null或空";
                return;
            }
            //读取http请求
            using (StringReader reader = new StringReader(request))
            {
                //读取应答行
                string requestLine = reader.ReadLine();
                //解析应答行
                RequestLine = new RequestLine(requestLine);
                //判断应答行解析情况
                if(!RequestLine.IsParseSuccess)
                {
                    IsParseSuccess = false;
                    ErrorMsg = RequestLine.ErrorMsg;
                    return;
                }
                

                //解析请求报头
                string line = null;
                Headers = new Dictionary<string, string>();
                bool isSuccess = false;
                while((line = reader.ReadLine())!= null)
                {
                    //应答头
                    if(line != string.Empty)
                    {
                        string[] array = line.Split(new char[] {':'}, 2);
                        
                        if(array .Length != 2 || string.IsNullOrEmpty(array[0]))
                        {
                            continue;
                        }
                        else
                        {
                            if(Headers.ContainsKey(array[0]))
                            {
                                //包头包含了两个相同的字段
                                continue;
                            }
                            else
                            {
                                Headers.Add(array[0],array[1]);
                            }
                        }
                    }
                    else
                    {
                        //应答包体
                        RequestBody = reader.ReadToEnd();
                        isSuccess = true;
                    }
                }

                if(Headers == null || Headers.Count == 0)
                {
                    IsParseSuccess = false;
                    ErrorMsg = "没有请求包头";
                    return;
                }

                if(isSuccess)
                {
                    IsParseSuccess = true;
                }
                else
                {
                    IsParseSuccess = false;
                    ErrorMsg = "没有请求包体";
                }
            }
        }

        /// <summary>
        /// 请求头
        /// </summary>
        public Dictionary<String, String> Headers { get; private set; }

        /// <summary>
        /// 应答包体
        /// </summary>
        public string RequestBody { get; private set; }

        /// <summary>
        /// 是否解析成功
        /// </summary>
        public bool IsParseSuccess { get; private set; }

        /// <summary>
        /// 在解析失败时，用户可以用来获取失败原因
        /// </summary>
        public string ErrorMsg { get; private set; }
    }

    #endregion
}