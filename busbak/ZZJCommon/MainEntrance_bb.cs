using BusinessInterface;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZZJCommon
{
    /// <summary>
    /// 测试两数相加
    /// </summary>
    public class ZZJCommonAPI_bb : ProcessingBusinessAsyncResult
    {
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);

            try
            {
                string ss = "0002";
                switch (ss)//CCN.ToString().Substring(CCN.ToString().Length - 4)
                {
                    case "0001"://获取病人基础信息
                        OutBusinessInfo.BusData = BUS.GETPATINFO.B_GETPATINFO(InBusinessInfo.BusData);
                        break;
                    case "0002"://保存病人建档信息
                        OutBusinessInfo.BusData = BUS.GETPATRECORD.B_GETPATRECORD(InBusinessInfo.BusData);
                        break;
                    case "0005"://凭条打印
                        OutBusinessInfo.BusData = BUS.TicketReprint.B_TicketReprint(InBusinessInfo.BusData);
                        break;
                        //case "0006"://预交金凭条打印内容
                        //    OutBusinessInfo = BUS.UpdatePrintStatus_B.SAVEINPATYJJ(InBusinessInfo);
                        //    break;
                        //case "0007"://保存病人住院登记信息
                        //    OutBusinessInfo = BUS.UpdatePrintStatus_B.SAVEINPATYJJ(InBusinessInfo);
                        //    break;
                }
            }
            catch (Exception ex)
            {
                CommonModel.DataReturn dataReturn = new CommonModel.DataReturn();
                dataReturn.Code = ConstData.CodeDefine.BusError;
                dataReturn.Msg = ex.Message;
                OutBusinessInfo.BusData = JSONSerializer.Serialize(dataReturn);
            }
            WriteLog("ZyPatAPI", "outData", OutBusinessInfo.BusData);
            //OutBusinessInfo = System.Web.HttpUtility.UrlEncode(OutBusinessInfo);
            return true;
        }

        public override byte[] DefErrotReturn(int Code, string ErrorMsage)
        {
            CommonModel.DataReturn dataReturn = new CommonModel.DataReturn();
            dataReturn.Code = Code;
            dataReturn.Msg = ErrorMsage;
            return base.GetByte(dataReturn);

        }

        protected static void WriteLog(string type, string className, string content)
        {
            string path = "";
            try
            {
                // path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\MySpringlog";
                path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "PasSLog", "ZzjLog");
            }
            catch (Exception ex)
            {
                //   path = HttpContent.Current.Server.MapPath("MySpringlog");
            }

            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
                string filename = path + "/" + DateTime.Now.ToString("yyyyMMdd") + type.Replace('|', ':') + ".log";//用日期对日志文件命名
                //创建或打开日志文件，向日志文件末尾追加记录
                StreamWriter mySw = File.AppendText(filename);

                //向日志文件写入内容
                string write_content = className + ": " + content;
                mySw.WriteLine(time + " " + type);
                mySw.WriteLine(write_content);
                mySw.WriteLine("");
                //关闭日志文件
                mySw.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
