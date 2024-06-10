using BusinessInterface;
using CommonModel;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZZJZY
{
    /// <summary>
    /// 测试两数相加
    /// </summary>
    public class ZZJZYAPI: ProcessingBusinessAsyncResultByte
    {
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS sLBInfoHeadBusS, byte[] In, out byte[] Out)
        {

            string OutBusinessInfo = "";
            try
            {
                WriteLog("ZZJZYHead", "ReceiveData", JSONSerializer.Serialize(sLBInfoHeadBusS));
                string InBusinessInfo = base.GetStrData(In);
                InBusinessInfo = System.Web.HttpUtility.UrlDecode(InBusinessInfo);
                WriteLog("ZZJZYAPI", "ReceiveData", InBusinessInfo);
                switch (CCN.ToString().Substring(CCN.ToString().Length - 4))
                {
                    case "0001"://获取病人住院号
                        OutBusinessInfo = BUS.GETPATHOSNO.B_GETPATHOSNO(InBusinessInfo);
                        break;
                    case "0002":// 通过住院获取病人信息 
                        OutBusinessInfo = BUS.GETPATHOSINFO.B_GETHOSPATINFO(InBusinessInfo);
                        break;
                    case "0003":// 通过住院号获取病人基本信息 
                        OutBusinessInfo = BUS.GETPATINFBYHOSNO.B_GETPATINFBYHOSNO(InBusinessInfo);
                        break;
                    case "0004"://缴纳预交金
                        OutBusinessInfo = BUS.SAVEINPATYJJ.B_SAVEINPATYJJ(InBusinessInfo);
                        break;
                    case "0005"://获取住院病人每日清单
                        OutBusinessInfo = BUS.GETHOSDAILY.B_GETHOSDAILY(InBusinessInfo);
                        break;
                    case "0006"://判断病人是否可以出院
                        OutBusinessInfo = BUS.CANPATOUTJS.B_CANPATOUTJS(InBusinessInfo);
                        break;
                    case "0007"://出院预结算
                        OutBusinessInfo = BUS.JZHOUTYJS.B_JZHOUTYJS(InBusinessInfo);
                        break;
                    case "0008"://出院结算
                        OutBusinessInfo = BUS.JZHOUTJS.B_JZHOUTJS(InBusinessInfo);
                        break;
                    case "0009"://获取病人住院登记标识
                        OutBusinessInfo = BUS.GETPATZYDJSTATE.B_GETPATZYDJSTATE(InBusinessInfo);
                        break;
                    case "0010"://获取病人住院登记数据
                        OutBusinessInfo = BUS.GETPATZYDJDATA.B_GETPATZYDJDATA(InBusinessInfo);
                        break;
                    case "0011"://住院登记保存
                        OutBusinessInfo = BUS.ZYDJSAVE.B_ZYDJSAVE(InBusinessInfo);
                        break;
                    case "0012"://腕带打印回传
                        OutBusinessInfo = BUS.WDPRINT.B_WDPRINT(InBusinessInfo);
                        break;
                    default:
                        DataReturn dataReturn = new DataReturn();
                        dataReturn.Code =1;
                        dataReturn.Msg = "未匹配到此业务类型";
                        OutBusinessInfo= JSONSerializer.Serialize(dataReturn);
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
            WriteLog("ZZJZYAPI", "outData", OutBusinessInfo);
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
