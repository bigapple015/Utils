using System;
using System.Collections.Generic;
using System.Text;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    ///// <summary>
    ///// 数据库数据过滤器
    ///// </summary>
    //public class DBFilter
    //{
    //    /// <summary>
    //    /// 所有短信的条数
    //    /// </summary>
    //    public static int SumCount = 0;

    //    /// <summary>
    //    /// 识别短信总数
    //    /// </summary>
    //    public static int RecCount = 0;
    //    /// <summary>
    //    /// 拒识短信总数
    //    /// </summary>
    //    public static int RefuseCount = 0;

    //    #region 应该识别而没有识别的短信
    //    /// <summary>
    //    /// 应该识别而没有识别的短信数目
    //    /// </summary>
    //    public static int SMSCount = 0;

    //    /// <summary>
    //    /// string代表预处理之后短信内容 int代表短信出现的次数
    //    /// </summary>
    //    public static Dictionary<string, int> SMSDic = new Dictionary<string, int>();
    //    /// <summary>
    //    /// 代表预处理之前的短信
    //    /// </summary>
    //    public static List<string> SMSList = new List<string>();
    //    #endregion

    //    #region 单字
    //    /// <summary>
    //    /// 单字的数目
    //    /// </summary>
    //    public static int DanziCount = 0;
    //    /// <summary>
    //    /// string代表预处理之后短信内容 int代表短信出现的次数
    //    /// </summary>
    //    public static Dictionary<string, int> DanziDic = new Dictionary<string, int>();
    //    /// <summary>
    //    /// 预处理之前的短信
    //    /// </summary>
    //    public static List<string> DanziList = new List<string>();
    //    #endregion

    //    #region Empty短信
    //    /// <summary>
    //    /// 空短信的数目
    //    /// </summary>
    //    public static int EmptyCount = 0;

    //    /// <summary>
    //    /// 预处理之前的短信
    //    /// </summary>
    //    public static List<string> EmptyList = new List<string>();

    //    #endregion

    //    #region 超长短信

    //    /// <summary>
    //    /// 超长短信数目
    //    /// </summary>
    //    public static int LongSMSCount = 0;

    //    /// <summary>
    //    /// 长短信内容，保存预处理之前的内容
    //    /// </summary>
    //    public static List<string> LongSMSList = new List<string>();
    //    #endregion

    //    #region 纯数字短信

    //    /// <summary>
    //    /// 纯数字的短信的数目
    //    /// </summary>
    //    public static int PureNumSMSCount = 0;

    //    /// <summary>
    //    /// string 预处理之后的短信 int短信的数目
    //    /// </summary>
    //    public static Dictionary<string, int> PureNumDic = new Dictionary<string, int>();

    //    /// <summary>
    //    /// 预处理之前的短信
    //    /// </summary>
    //    public static List<string> PureNumList = new List<string>();
    //    #endregion

    //    #region 完全乱码
    //    /// <summary>
    //    /// 完全乱码的短信数目
    //    /// </summary>
    //    public static int FullGarbledCount = 0;

    //    /// <summary>
    //    /// string 预处理之后的的短信 int 短信的数目
    //    /// </summary>
    //    public static Dictionary<string, int> FullGarbledDic = new Dictionary<string, int>();

    //    /// <summary>
    //    /// 预处理之前的短信
    //    /// </summary>
    //    public static List<string> FullGarbledList = new List<string>();
    //    #endregion

    //    #region 出现在拒识表中的短信
    //    /// <summary>
    //    /// 出现在拒识表中的短信的数目
    //    /// </summary>
    //    public static int SMSRefuseCount = 0;

    //    /// <summary>
    //    /// string 预处理之后的短信 int 短信的数目
    //    /// </summary>
    //    public static Dictionary<string, int> SMSRefuseDic = new Dictionary<string, int>();

    //    /// <summary>
    //    /// string 预处理之前的短信
    //    /// </summary>
    //    public static List<string> SMSRefuseList = new List<string>();
    //    #endregion

    //    #region 出现在talking.txt表中的短信
    //    /// <summary>
    //    /// 出现在talking.txt表中的短信
    //    /// </summary>
    //    public static int SMSTalkingCount = 0;

    //    /// <summary>
    //    /// string 预处理之后的短信 int 短信的数目
    //    /// </summary>
    //    public static Dictionary<string, int> SMSTalkingDic = new Dictionary<string, int>();

    //    /// <summary>
    //    /// 预处理之前的短信
    //    /// </summary>
    //    public static List<string> SMSTalkingList = new List<string>();
    //    #endregion

    //    #region 作为单条处理的短信

    //    /// <summary>
    //    /// 其他类型的短信的数目
    //    /// </summary>
    //    public static int OtherCount = 0;

    //    /// <summary>
    //    /// string 预处理之后的短信 int 短信的数目
    //    /// </summary>
    //    public static Dictionary<string, int> OtherDic = new Dictionary<string, int>();

    //    /// <summary>
    //    /// 预处理之前的短信
    //    /// </summary>
    //    public static List<string> OtherList = new List<string>();

    //    #endregion

    //    /// <summary>
    //    /// 从数据库中获取数据
    //    /// </summary>
    //    /// <returns></returns>
    //    public static bool ReadDB()
    //    {
    //        ConfigProxy.
    //    }
    //}
}
