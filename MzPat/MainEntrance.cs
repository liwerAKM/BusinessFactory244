using BusinessInterface;
using CommonModel;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZJMZ
{
    class ZZJMZAPI : ProcessingBusinessAsyncResultByte
    {
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS sLBInfoHeadBusS, byte[] In, out byte[] Out)
        {

            string OutBusinessInfo = "";
            try
            {
                WriteLog("ZZJMZHead", "ReceiveData", JSONSerializer.Serialize(sLBInfoHeadBusS));
                string InBusinessInfo = base.GetStrData(In);
                InBusinessInfo = System.Web.HttpUtility.UrlDecode(InBusinessInfo);
                WriteLog("ZZJMZAPI", "ReceiveData", InBusinessInfo);
                switch (CCN.ToString().Substring(CCN.ToString().Length - 4))
                {
                    case "0001"://获取待处方缴费列表
                        OutBusinessInfo = BUS.GETOUTFEENOPAY.B_GETOUTFEENOPAY(InBusinessInfo);
                        break;
                    case "0002"://获取费用明细
                        OutBusinessInfo = BUS.GETOUTFEENOPAYMX.B_GETOUTFEENOPAYMX(InBusinessInfo);
                        break;
                    case "0003"://诊间支付锁定
                        OutBusinessInfo = BUS.OUTFEEPAYLOCK.B_OUTFEEPAYLOCK(InBusinessInfo);
                        break;
                    case "0004"://诊间支付保存
                        OutBusinessInfo = BUS.OUTFEEPAYSAVE.B_OUTFEEPAYSAVE(InBusinessInfo);
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
            WriteLog("ZZJMZAPI", "outData", OutBusinessInfo);
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
