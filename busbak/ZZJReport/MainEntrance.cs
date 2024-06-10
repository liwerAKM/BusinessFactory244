using BusinessInterface;
using CommonModel;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZZJReport
{
    class ZZJReportAPI : ProcessingBusinessAsyncResultByte
    {
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS sLBInfoHeadBusS, byte[] In, out byte[] Out)
        {

            string OutBusinessInfo = "";
            try
            {
                WriteLog("ZZJReportHead", "ReceiveData", JSONSerializer.Serialize(sLBInfoHeadBusS));
                string InBusinessInfo = base.GetStrData(In);
                InBusinessInfo = System.Web.HttpUtility.UrlDecode(InBusinessInfo);
                WriteLog("ZZJReportAPI", "ReceiveData", InBusinessInfo);
                switch (CCN.ToString().Substring(CCN.ToString().Length - 4))
                {
                    case "0001"://获取患者检验报告列表
                        OutBusinessInfo = BUS.GETLISREPORT.B_GETLISREPORT(InBusinessInfo);
                        break;
                    case "0002":// 通过患者检验报告明细
                        OutBusinessInfo = BUS.GETLISRESULT.B_GETLISRESULT(InBusinessInfo);
                        break;
                    case "0003":// 检验报告打印回传
                        OutBusinessInfo = BUS.ZZJLISPRNBACK.B_ZZJLISPRNBACK(InBusinessInfo);
                        break;
                    default:
                        DataReturn dataReturn = new DataReturn();
                        dataReturn.Code = 1;
                        dataReturn.Msg = "未匹配到此业务类型";
                        OutBusinessInfo = JSONSerializer.Serialize(dataReturn);
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonModel.DataReturn dataReturn = new CommonModel.DataReturn();
                dataReturn.Code = ConstData.CodeDefine.BusError;
                dataReturn.Msg = ex.Message;
                OutBusinessInfo = JSONSerializer.Serialize(dataReturn);
            }
            WriteLog("ZZJReportAPI", "outData", OutBusinessInfo);
            //OutBusinessInfo = System.Web.HttpUtility.UrlEncode(OutBusinessInfo);
            Out = base.GetByte(OutBusinessInfo);
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
                path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ZzjLog");
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
