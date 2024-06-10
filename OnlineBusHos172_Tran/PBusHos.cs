using BusinessInterface;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OnlineBusHos172_Tran
{
    /// <summary>
    /// 
    /// </summary>
    public class PBusHos172_Tran : ProcessingBusinessAsyncResult 
    {
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
            try
            {
                string name =InBusinessInfo.SubBusID;
                switch (name)
                {
                    case "0001"://获取支付二维码
                        OutBusinessInfo.BusData = BUS.GETQRCODE.B_GETQRCODE(InBusinessInfo.BusData);
                        break;
                    case "0002"://查询订单状态
                        OutBusinessInfo.BusData = BUS.GETORDERSTATUS.B_GETORDERSTATUS(InBusinessInfo.BusData);
                        break;
                    case "0003"://微信支付宝退费
                        OutBusinessInfo.BusData= BUS.WXZFBTF.B_WXZFBTF(InBusinessInfo.BusData);
                        break;
                    case "0004"://扫码被动支付
                        OutBusinessInfo.BusData = BUS.GETPASSIVEPAY.B_GETPASSIVEPAY(InBusinessInfo.BusData);
                        break;
                    case "0005"://订单取消
                        OutBusinessInfo.BusData = BUS.PAYCANCEL.B_PAYCANCEL(InBusinessInfo.BusData);
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonModel.DataReturn dataReturn = new CommonModel.DataReturn();
                dataReturn.Code = ConstData.CodeDefine.BusError;
                dataReturn.Msg = ex.Message;
                OutBusinessInfo.BusData = JSONSerializer.Serialize(dataReturn);
            }
            WriteLog("OnlineBusHos968_Tran", "outData", OutBusinessInfo.BusData);
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

        public static void WriteLog(string type, string className, string content)
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
                string filename =Path.Combine( path ,DateTime.Now.ToString("yyyyMMdd") + type.Replace('|', ':') + ".log");//用日期对日志文件命名
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
