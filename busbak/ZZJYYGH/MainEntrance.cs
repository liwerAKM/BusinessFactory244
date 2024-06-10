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
namespace ZZJYYGH
{
    class ZZJYYGHAPI : ProcessingBusinessAsyncResultByte
    {
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS sLBInfoHeadBusS, byte[] In, out byte[] Out)
        {

            string OutBusinessInfo = "";
            try
            {
                WriteLog("ZZJYYGHHead", "ReceiveData", JSONSerializer.Serialize(sLBInfoHeadBusS));
                string InBusinessInfo = base.GetStrData(In);
                InBusinessInfo = System.Web.HttpUtility.UrlDecode(InBusinessInfo);
                WriteLog("ZZJYYGHAPI", "ReceiveData", InBusinessInfo);
                switch (CCN.ToString().Substring(CCN.ToString().Length - 4))
                {
                    case "0001"://获取医院科室信息列表
                        OutBusinessInfo = BUS.GETHOSPDEPT.B_GETHOSPDEPT(InBusinessInfo);
                        break;
                    case "0002"://获取医生科室排班数据
                        OutBusinessInfo = BUS.GETSCHINFO.B_GETSCHINFO(InBusinessInfo);
                        break;
                    case "0003"://获取指定医院科室(专家)日期排班时间段
                        OutBusinessInfo = BUS.GETSCHPERIOD.B_GETSCHPERIOD(InBusinessInfo);
                        break;
                    case "0004"://预约挂号保存
                        OutBusinessInfo = BUS.REGISTERAPPTSAVE.B_REGISTERAPPTSAVE(InBusinessInfo);
                        break;
                    case "0005"://预约(实时)挂号支付
                        OutBusinessInfo = BUS.REGISTERPAYSAVE.B_REGISTERPAYSAVE(InBusinessInfo);
                        break;
                    case "0006"://预约取消(含支付)
                        OutBusinessInfo = BUS.REGISTERPAYCANCEL.B_REGISTERPAYCANCEL(InBusinessInfo);
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
            WriteLog("ZZJYYGHAPI", "outData", OutBusinessInfo);
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
