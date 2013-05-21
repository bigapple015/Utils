using System;
using System.Data;
using System.Configuration;
using iFlyTek.SMSS10.Common.NLSMS;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace iFlyTek.ECSS30.WirelessCity.LotteryFlows
{
    /// <summary>
    /// 负责与trs的通信
    /// </summary>
    public static class TRSProxy
    {
        /// <summary>
        /// 获取识别结果
        /// </summary>
        /// <param name="toRouteContent">用户短信内容</param>
        /// <param name="config">TRS服务器的配置类 </param>
        /// <returns></returns>
        public static NLSMSResult_Route Route(string toRouteContent,TRSConfig config)
        {
            NLSMSResult_Route routeResult = null;
            try
            {
                routeResult = Get_RouteResult();

                if(toRouteContent==null || string.IsNullOrEmpty(toRouteContent.Trim()) || config == null || !config.IsValid)
                {
                    routeResult.ResultTime = DateTime.Now;
                    routeResult.ResultCode = NLSMSResultCode.InvalidParam;
                    //WriteLog_RecRoute(requestID, toRouteContent, routeResult);
                    return routeResult;
                }

                //发送请求给trs
                Route_Engine(ref routeResult,toRouteContent,config);
                return routeResult;
            }
            catch (Exception ex)
            {
                //ErrorProxy.AddError(ex.ToString());
                //LogWriter.WriteLogSystem(LogLevel.Error, "识别引擎异常:" + ex.ToString());
                //LogHelper.WriteSystemLog(LogLevel.Error, "识别引擎异常："+ex.ToString());
                if(routeResult != null)
                {
                    routeResult.ResultTime = DateTime.Now;
                    routeResult.ResultCode = NLSMSResultCode.Routing_RecDllFailed;
                    routeResult.RecType = NLRecType.RecFail;
                    routeResult.IsMultiRouteResult = false;
                    routeResult.IsSuccess = false;
                    routeResult.ResultDesc = "调用识别dll返回错误";
                    routeResult.RouteResultItems = new NLSMSResult_Route.RouteResultItem[0];
                }
                return routeResult;
            }

        }

        /// <summary>
        /// 发送请求gettrs
        /// </summary>
        /// <param name="routeResult"></param>
        private static void Route_Engine(ref NLSMSResult_Route routeResult,string usercontent,TRSConfig config)
        {
            routeResult = new NLSMSResult_Route();
            IPEndPoint endPoint = null;
            Socket socket = null;

            try
            {
                //Encoding encode = Encoding.Default;
                //默认是gb2312编码的
                endPoint = new IPEndPoint(IPAddress.Parse(config.TRSIP), config.TRSPort);
                //构建socket，并连接服务器端
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = config.SendTimeout;
                socket.ReceiveTimeout = config.ReceiveTimeout;
                socket.Connect(endPoint);

                //构建要发送的消息
                StringBuilder buff = new StringBuilder();
                //构建request-line
                buff.Append("POST ").Append("/trs_recognize").Append(" HTTP/1.0\r\n");

                //构建请求头

                Byte[] bodyBytes = new byte[0];
                //检查是否需要传递包体

                buff.Append(string.Format("content-length: {0}\r\n", 0));
                buff.Append(string.Format("usrname: {0}\r\n", config.TRSUserName));
                buff.Append(string.Format("password: {0}\r\n", config.TRSPassword));
                buff.Append(string.Format("usercontent: {0}\r\n", usercontent));

                //blank-line
                buff.Append("\r\n");

                //没有request-body
                //这里的编码方式取决于服务器端的编码方式//Encoding.GetEncoding("gb2312") 即 Encoding.Default
                Byte[] sendBytes = Encoding.Default.GetBytes(buff.ToString());
                socket.Send(sendBytes);
                //存放服务器端发回的字符
                byte[] recvBytes = new byte[1024];
                //实际接收到的字符数
                int ibytes;
                //接收到的字符串
                string recvStr = string.Empty;
                do
                {
                    ibytes = socket.Receive(recvBytes, recvBytes.Length, SocketFlags.None);
                    //这里的编码方式取决于服务器端的编码方式//Encoding.GetEncoding("gb2312")
                    recvStr += Encoding.Default.GetString(recvBytes, 0, ibytes);
                }
                while (ibytes != 0);
                //解析发回的字符串
                RecongnizeResult result = RecongnizeResult.ParseResponse(recvStr);
                routeResult.RecType = RecongnizeResult.GetRecType(result.HeadProcesstype);
                RecongnizeResult.ParseRoute(result, ref routeResult);

            }
            catch (Exception ex)
            {
                throw new Exception("调用trs引擎异常",ex);
            }
            finally
            {
                if (socket != null)
                {
                    //关闭连接
                    socket.Close();
                }
            }
        }

        /// <summary>
        /// 构建一个错误的结果
        /// </summary>
        /// <returns></returns>
        private static NLSMSResult_Route Get_RouteResult()
        {
            NLSMSResult_Route routeResult = new NLSMSResult_Route();
            routeResult.RecType = NLRecType.InvalidRecType;
            routeResult.IsMultiRouteResult = false;
            routeResult.IsSuccess = false;
            routeResult.ResultCode = NLSMSResultCode.Routing_Exception;
            routeResult.ResultTime = DateTime.Now;
            routeResult.EngineNodeName = string.Empty;

            return routeResult;
        }
    }
}
