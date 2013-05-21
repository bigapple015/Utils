using System;
using iFlyTek.SMSS10.Common.NLSMS;
using System.Collections.Generic;

namespace iFlyTek.ECSS30.WirelessCity.LotteryFlows
{
    /// <summary>
    /// trs配置类
    /// </summary>
    public sealed class TRSConfig
    {
        /// <summary>
        /// trs服务器的端口号
        /// </summary>
        public int TRSPort;

        /// <summary>
        /// TRS服务器的IP地址
        /// </summary>
        public string TRSIP;

        /// <summary>
        /// TRS服务器的登陆用户名
        /// </summary>
        public string TRSUserName;

        /// <summary>
        /// TRS的登陆密码
        /// </summary>
        public string TRSPassword;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TRSIP">TRS服务器的IP地址</param>
        /// <param name="TRSPort">trs服务器的端口号</param>
        /// <param name="TRSUserName">TRS服务器的登陆用户名</param>
        /// <param name="TRSPassword">TRS的登陆密码</param>
        public TRSConfig(string TRSIP,int TRSPort,string TRSUserName,string TRSPassword)
        {
            this.TRSIP = TRSIP;
            this.TRSPort = TRSPort;
            this.TRSUserName = TRSUserName;
            this.TRSPassword = TRSPassword;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TRSConfig()
        {
            
        }

        /// <summary>
        /// socket连接trs时，send方法的超时时间(单位ms)，默认值3000ms，不建议修改
        /// </summary>
        public int SendTimeout = 3000;

        /// <summary>
        /// socket连接trs时，receive方法的超时时间(单位ms)，默认值3000ms，不建议修改
        /// </summary>
        public int ReceiveTimeout = 3000;

        /// <summary>
        /// 配置是否有效
        /// </summary>
        internal bool IsValid
        {
            get
            {
                return (TRSPort > 500 && TRSPort <= 65535) &&
                       !string.IsNullOrEmpty(TRSIP) &&
                       !string.IsNullOrEmpty(TRSUserName) &&
                       !string.IsNullOrEmpty(TRSPassword);
            }
        }
    }

    /// <summary>
    /// 参数项
    /// </summary>
    internal class ParamItem
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string ParamName;
        /// <summary>
        /// 参数值
        /// </summary>
        public string ParamValue;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ParamItem()
        {
            ParamName = string.Empty;
            ParamValue = string.Empty;
        }
    }

    /// <summary>
    /// 解析TRS发回的字符串
    /// </summary>
    internal class RecongnizeResult
    {
                /***********************************
         * 应答包格式
         Request-Line:     http/1.0  200  OK CRLF   
         Request-Header:   response: /recognizeText CRLF
	     content-length: xxx CRLF
		 retcode: xxx CRLF
		 sessionid: xxx CRLF
	     processtype: xxx CRLFCRLF
		 type1<制表符>value1<制表符>type2<制表符>value2…typeN<制表符>valueN
         *********************************************************/
        #region Request-Line
        private string requestLine;
        /// <summary>
        /// 应答包的request_line
        /// </summary>
        public string RequestLine
        {
            get { return requestLine; }
            set { requestLine = value; }
        }
        #endregion

        #region 请求头

        private string headResponse;
        public string HeadResponse
        {
            get { return headResponse; }
            set { headResponse = value; }
        }

        private string headSessionid;
        public string HeadSessionid
        {
            get { return headSessionid; }
            set { headSessionid = value; }
        }

        private string headRetcode;
        public string HeadRetcode
        {
            get { return headRetcode; }
            set { headRetcode = value; }
        }

        private string headContentlength;
        public string HeadContentlength
        {
            get { return headContentlength; }
            set { headContentlength = value; }
        }

        private string headProcesstype;
        public string HeadProcesstype
        {
            get { return headProcesstype; }
            set { headProcesstype = value; }
        }
        #endregion

        #region 请求体

        private string responseBody;
        /// <summary>
        /// 应答包体
        /// </summary>
        public string ResponseBody
        {
            get { return responseBody; }
            set { responseBody = value; }
        }

        private string[,] responseItems;

        public string[,] ResponseItems
        {
            get { return responseItems; }
            set { responseItems = value; }
        }

        #endregion

        #region 构造函数
        public RecongnizeResult()
        {
            requestLine = string.Empty;
            headResponse = string.Empty;
            headSessionid = string.Empty;
            headRetcode = string.Empty;
            headContentlength = string.Empty;
            headProcesstype = string.Empty;
            responseBody = string.Empty;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取识别的类型
        /// </summary>
        public static NLRecType GetRecType(string processType)
        {
            switch (processType)
            {
                case "0":
                    //前置据识
                    return NLRecType.PreReject;
                case "1":
                    //文法识别成功
                    return NLRecType.Grammar;
                case "2":
                    //硬匹配识别成功
                    return NLRecType.HardMatch;
                case "3":
                    //词模识别成功
                    return NLRecType.SVM;
                case "4":
                    //识别失败
                    return NLRecType.RecFail;
                default:
                    //无效的recType
                    return NLRecType.InvalidRecType;

            }
        }


        /// <summary>
        /// 将识别结果按照原样解析
        /// </summary>
        /// <param name="source">待解析的字符串</param>
        /// <returns>解析后的对象</returns>
        public static RecongnizeResult ParseResponse(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            RecongnizeResult recognizeResult = new RecongnizeResult();
            string[] splitStr = new string[] { "\r\n" };
            string[] targetArray = source.Split(splitStr, StringSplitOptions.RemoveEmptyEntries);
            if (targetArray != null && targetArray.Length > 0)
            {
                recognizeResult.requestLine = targetArray[0];

                #region 解析包头
                foreach (string str in targetArray)
                {
                    string[] strTarget = str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strTarget.Length == 2)
                    {
                        switch (strTarget[0].ToLower().Trim())
                        {
                            case "response":
                                recognizeResult.headResponse = strTarget[1].Trim();
                                break;
                            case "content-length":
                                recognizeResult.headContentlength = strTarget[1].Trim();
                                break;
                            case "retcode":
                                recognizeResult.headRetcode = strTarget[1].Trim();
                                break;
                            case "sessionid":
                                recognizeResult.headSessionid = strTarget[1].Trim();
                                break;
                            case "processtype":
                                recognizeResult.headProcesstype = strTarget[1].Trim();
                                break;
                        }
                    }
                }
                #endregion

                #region 解析包体

                if (!string.IsNullOrEmpty(recognizeResult.headRetcode) && recognizeResult.headRetcode.Equals("0"))
                {
                    recognizeResult.responseBody = targetArray[targetArray.Length - 1];
                    //解析包体
                    string[] strTarget = targetArray[targetArray.Length - 1].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strTarget != null && strTarget.Length % 2 == 0)
                    {
                        recognizeResult.responseItems = new string[(strTarget.Length / 2), 2];
                        for (int index = 0; index < strTarget.Length; index += 2)
                        {
                            recognizeResult.responseItems[index / 2, 0] = strTarget[index].Trim();
                            recognizeResult.responseItems[index / 2, 1] = strTarget[index + 1].Trim();
                        }
                    }
                }

                #endregion
            }

            return recognizeResult;
        }

        #endregion

        public static void ParseRoute(RecongnizeResult recResult, ref NLSMSResult_Route routeResult)
        {
            if (recResult == null || recResult.responseItems == null || recResult.responseItems.GetLength(0) < 1)
            {
                //没有识别结果
                routeResult.ResultCode = NLSMSResultCode.RouteResult_Items_IsNull;
                routeResult.ResultTime = DateTime.Now;
                routeResult.RecType = NLRecType.RecFail;
                routeResult.ResultDesc = "路由结果项错误-为空";
                routeResult.IsSuccess = false;
                return;
            }

            #region begin

            //二次解析识别结果
            List<int> bizOrBizCodeIndex = new List<int>();
            for (int index = 0; index < recResult.responseItems.GetLength(0); index++)
            {
                if (recResult.responseItems[index, 0].ToLower().Equals("biz") || recResult.responseItems[index, 0].ToLower().Equals("bizcode"))
                {
                    bizOrBizCodeIndex.Add(index);
                }
            }

            //判断是否有识别结果
            if (bizOrBizCodeIndex.Count == 0)
            {
                //没有识别结果
                routeResult.ResultCode = NLSMSResultCode.RouteResult_Items_IsNull;
                routeResult.ResultTime = DateTime.Now;
                routeResult.RecType = NLRecType.RecFail;
                routeResult.ResultDesc = "路由结果项错误-为空";
                routeResult.IsSuccess = false;
                return;
            }
            else
            {
                //设置全局参数
                routeResult.ResultCode = NLSMSResultCode.RoutingEngine_OK;
                routeResult.ResultTime = DateTime.Now;
                routeResult.ResultDesc = "识别成功";
                routeResult.IsSuccess = true;
            }

            //判断是否是多个识别结果
            if (bizOrBizCodeIndex.Count > 1)
            {
                routeResult.IsMultiRouteResult = true;
            }

            //获取每个识别结果
            routeResult.RouteResultItems = new NLSMSResult_Route.RouteResultItem[bizOrBizCodeIndex.Count];
            for (int index = 0; index < bizOrBizCodeIndex.Count; index++)
            {

                if (index == bizOrBizCodeIndex.Count - 1)
                {
                    //最后一项
                    List<ParamItem> dicParams = new List<ParamItem>();
                    routeResult.RouteResultItems[index] = new NLSMSResult_Route.RouteResultItem();
                    for (int begin = bizOrBizCodeIndex[index]; begin < recResult.responseItems.GetLength(0); begin++)
                    {

                        switch (recResult.responseItems[begin, 0].ToLower())
                        {
                            case "biz":
                            case "bizcode":
                                routeResult.RouteResultItems[index].BizName = recResult.responseItems[begin, 1];
                                break;
                            case "opera":
                                routeResult.RouteResultItems[index].OperationName = recResult.responseItems[begin, 1];
                                break;
                            case "type":
                                //type暂时未使用
                                break;
                            default:
                                ParamItem item = new ParamItem();
                                item.ParamName = recResult.responseItems[begin, 0];
                                item.ParamValue = recResult.responseItems[begin, 1];
                                dicParams.Add(item);
                               
                                break;
                        }
                    }
                    routeResult.RouteResultItems[index].ParamItems = new NLSMSResult_Route.RouteResultItem.ParamItem[dicParams.Count];
                    int paramIndex = 0;
                    foreach (ParamItem paramItem in dicParams)
                    {
                        routeResult.RouteResultItems[index].ParamItems[paramIndex] = new NLSMSResult_Route.RouteResultItem.ParamItem();
                        routeResult.RouteResultItems[index].ParamItems[paramIndex].ParamName = paramItem.ParamName;
                        routeResult.RouteResultItems[index].ParamItems[paramIndex].ParamValue = paramItem.ParamValue;
                        paramIndex++;
                    }
                    //设置参数的个数和得分
                    routeResult.RouteResultItems[index].Score = 1.0;
                    routeResult.RouteResultItems[index].PramCount = dicParams.Count;
                    //这一句设置无效，主要因为只有bizcode，没有opera的情况下，该项无效
                    //routeResult.RouteResultItems[index].IsValid = true;
                }
                else
                {

                    //参数的个数
                    List<ParamItem> dicParams = new List<ParamItem>();
                    //初始化
                    routeResult.RouteResultItems[index] = new NLSMSResult_Route.RouteResultItem();

                    for (int begin = bizOrBizCodeIndex[index]; begin < bizOrBizCodeIndex[index + 1]; begin++)
                    {

                        switch (recResult.responseItems[begin, 0].ToLower())
                        {
                            case "biz":
                            case "bizcode":
                                routeResult.RouteResultItems[index].BizName = recResult.responseItems[begin, 1];
                                break;
                            case "opera":
                                routeResult.RouteResultItems[index].OperationName = recResult.responseItems[begin, 1];
                                break;
                            case "type":
                                //type暂时未使用
                                break;
                            default:
                                ParamItem item = new ParamItem();
                                item.ParamName = recResult.responseItems[begin, 0];
                                item.ParamValue = recResult.responseItems[begin, 1];
                                dicParams.Add(item);
                                break;
                        }
                    }
                    routeResult.RouteResultItems[index].ParamItems = new NLSMSResult_Route.RouteResultItem.ParamItem[dicParams.Count];
                    int paramIndex = 0;
                    foreach (ParamItem paramItem in dicParams)
                    {
                        routeResult.RouteResultItems[index].ParamItems[paramIndex] = new NLSMSResult_Route.RouteResultItem.ParamItem();
                        routeResult.RouteResultItems[index].ParamItems[paramIndex].ParamName = paramItem.ParamName;
                        routeResult.RouteResultItems[index].ParamItems[paramIndex].ParamValue = paramItem.ParamValue;
                        paramIndex++;
                    }
                    //设置参数的个数和得分
                    routeResult.RouteResultItems[index].Score = 1.0;
                    routeResult.RouteResultItems[index].PramCount = dicParams.Count;
                }

            }

            #endregion
        }
    }
}
