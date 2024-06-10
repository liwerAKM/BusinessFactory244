using EBPP.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
    public class EBPPConfig
    {



        /// <summary>
        /// 签名文件存储路径
        /// </summary>
        public static string CADESFilePath = ConfigurationManager.AppSettings["EBPPFilePath"];

        /// <summary>
        /// EBP PApply成功是否保存日志 
        /// </summary>
          static int eBPPApplySucessLog = -1;
        /// <summary>
        /// EBP PApply成功是否保存日志 
        /// </summary>
        public static bool  EBPPApplySucessLog
        {
            get
            {
                if (eBPPApplySucessLog == -1)
                {
                    string el = ConfigurationManager.AppSettings["EBPPApplySucessLog"];
                    if (el != null && el.ToUpper() == "TRUE")
                    {
                        eBPPApplySucessLog = 1;
                    }
                    else
                    {
                        eBPPApplySucessLog = 0;
                    }
                }
                return eBPPApplySucessLog == 1;
            }
        }

        /// <summary>
        /// EBPPApply是否严格验证数据
        /// </summary>
        static int eBPPApplyCloseCheck = -1;
        /// <summary>
        /// EBPPApply是否严格验证数据 
        /// </summary>
        public static bool EBPPApplyCloseCheck
        {
            get
            {
                if (eBPPApplyCloseCheck == -1)
                {
                    string el = ConfigurationManager.AppSettings["EBPPApplyCloseCheck"];
                    if (el != null && el.ToUpper() == "TRUE")
                    {
                        eBPPApplyCloseCheck = 1;
                    }
                    else
                    {
                        eBPPApplyCloseCheck = 0;
                    }
                }
                return eBPPApplyCloseCheck == 1;
            }
        }


        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="mebppmain"></param>
        /// <returns></returns>
        public static string GetFileName(Mebppmain mebppmain)
        {
            return Path.Combine(EBPPConfig.CADESFilePath, mebppmain.openTime.ToString("yyyyMMdd")+mebppmain.EBPPID.ToString() + ".pdf");
        }
    }
}
