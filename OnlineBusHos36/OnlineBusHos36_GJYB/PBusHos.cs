﻿using BusinessInterface;
using CommonModel;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBusHos36_GJYB
{
    class PBusHos36_GJYB : ProcessingBusinessAsyncResult
    {
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
            GlobalVar.Init();//初始化静态变量
            string type = "";
            try
            {
                string name = InBusinessInfo.SubBusID;
                
                switch (name)
                {
                    case "0001"://获取人员信息
                        type = "PSNQUERY";
                        WriteLog("OnlineBusHos36_GJYBAPI", type + " 入参", OutBusinessInfo.BusData);
                        OutBusinessInfo.BusData = BUS.GJYB_PSNQUERY.B_GJYB_PSNQUERY(InBusinessInfo.BusData);
                        
                        break;
                    case "0002"://挂号预结算 
                        type = "REGTRY";
                        WriteLog("OnlineBusHos36_GJYBAPI", type + " 入参", OutBusinessInfo.BusData);
                        OutBusinessInfo.BusData = BUS.GJYB_REGTRY.B_GJYB_REGTRY(InBusinessInfo.BusData);
                        
                        break;
                    case "0003":// 门诊缴费预结算 
                        type = "OUTPTRY";
                        WriteLog("OnlineBusHos36_GJYBAPI", type + " 入参", OutBusinessInfo.BusData);
                        OutBusinessInfo.BusData = BUS.GJYB_OUTPTRY.B_GJYB_OUTPTRY(InBusinessInfo.BusData);
                        
                        break;
                    case "0004"://结算 
                        type = "SETTLE";
                        WriteLog("OnlineBusHos36_GJYBAPI", type + " 入参", OutBusinessInfo.BusData);
                        OutBusinessInfo.BusData = BUS.GJYB_SETTLE.B_GJYB_SETTLE(InBusinessInfo.BusData);
                       
                        break;
                    case "0005"://退款
                        type = "REFUND";
                        WriteLog("OnlineBusHos36_GJYBAPI", type + " 入参", OutBusinessInfo.BusData);
                        OutBusinessInfo.BusData = BUS.GJYB_REFUND.B_GJYB_REFUND(InBusinessInfo.BusData);
                        
                        break;
                    case "0006":
                        type = "PSNINFOQUERY";
                        WriteLog("OnlineBusHos36_GJYBAPI", type + " 入参", OutBusinessInfo.BusData);
                        OutBusinessInfo.BusData = BUS.GJYB_PSNINFOQUERY.B_GJYB_PSNINFOQUERY(InBusinessInfo.BusData);
                        
                        break;
                    default:
                        DataReturn dataReturn = new DataReturn();
                        dataReturn.Code = 1;
                        dataReturn.Msg = "未匹配到此业务类型";
                        OutBusinessInfo.BusData = JSONSerializer.Serialize(dataReturn);
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
            finally
            {
                WriteLog("OnlineBusHos36_GJYBAPI", type+" 出参", OutBusinessInfo.BusData);              
            }
            //WriteLog("OnlineBusHos153_GJYBAPI", "outData", OutBusinessInfo.BusData);
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