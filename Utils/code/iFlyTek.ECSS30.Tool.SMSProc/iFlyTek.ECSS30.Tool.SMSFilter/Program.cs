using System;
using System.Collections.Generic;
using System.Text;
using iFlyTek.ECSS30.Tool.SMSProc;

namespace iFlyTek.ECSS30.Tool.SMSFilter
{
    /// <summary>
    /// 短信过滤工具
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            #region 获取配置文件

            if (ConfigProxy.IsValid)
            {
                Console.WriteLine("获取配置文件成功");
            }
            else
            {
                Console.WriteLine("获取配置文件失败，请检查配置文件");
                return;
            }

            #endregion

            #region 加载资源文件

            //初始化预处理工具
            try
            {
                SMSPreProc.Init(ConfigProxy.Trad2Simp);
                Console.WriteLine("加载简繁体表文件成功");
            }
            catch(Exception ex)
            {
                Console.WriteLine("加载繁简体表失败："+ex.ToString());
                return;
            }

            if( HanziProxy.LoadHanzi()&&
                SMSProxy.LoadSMS()&&
                TalkingProxy.LoadTalking()&&
                SMSRefuseProxy.LoadSMSRefuse()
                )
            {
                Console.WriteLine("加载资源文件成功");
            }
            else
            {
                Console.WriteLine("加载资源文件失败，请检查资源文件");
                return;
            }

            #endregion

            #region 进行数据处理
            Console.WriteLine("开始读取数据源");
            if (ConfigProxy.DataSource == "0")
            {
                if (ExcelFilter.ReadExcel())
                {
                    Console.WriteLine("读取文件成功");
                    //ExcelFilter.SaveAsExcel();
                }
                else
                {
                    Console.WriteLine("读取数据源Excel失败");
                    return;
                }
            }
            else if(ConfigProxy.DataSource == "1")
            {
                if (ExcelFilter.ReadFromDb())
                {
                    Console.WriteLine("读取文件成功");
                    //ExcelFilter.SaveAsExcel();
                }
                else
                {
                    Console.WriteLine("读取数据源db失败");
                    return;
                }
            }
            Console.WriteLine("开始保存处理结果");

            if (ConfigProxy.IsOutPutToExcel())
            {
                ExcelFilter.SaveAsExcel();
            }
            else
            {
                ExcelFilter.SaveAsTxt();
            }

            Console.WriteLine("处理结束");
            Console.ReadKey();
            #endregion
        }
    }
}
