﻿using BusinessInterface;
using CommonModel;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace OnlineBusHos153_Report
{
    class PBusHos153_Report: ProcessingBusinessAsyncResult
    {
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);

            try
            {
                string name = InBusinessInfo.SubBusID;
                switch (name)//CCN.ToString().Substring(CCN.ToString().Length - 4)
                {
                    case "0001"://获取患者检验报告列表
                        OutBusinessInfo.BusData = BUS.GETLISREPORT.B_GETLISREPORT(InBusinessInfo.BusData);
                        break;
                    case "0002":// 查询检验报告明细
                        OutBusinessInfo.BusData = BUS.GETLISRESULT.B_GETLISRESULT(InBusinessInfo.BusData);
                        break;
                    case "0003":// 检验报告打印回传
                        OutBusinessInfo.BusData = BUS.ZZJLISPRNBACK.B_ZZJLISPRNBACK(InBusinessInfo.BusData);
                        break;
                    case "0004"://获取患者检查报告列表
                        OutBusinessInfo.BusData = BUS.GETRISREPORT.B_GETRISREPORT(InBusinessInfo.BusData);
                        break;
                    case "0005":// 查询检查报告明细
                        OutBusinessInfo.BusData = BUS.GETRISRESULT.B_GETRISRESULT(InBusinessInfo.BusData);
                        break;
                    case "0006":// 检查报告打印回传
                        OutBusinessInfo.BusData = BUS.ZZJRISPRNBACK.B_ZZJRISPRNBACK(InBusinessInfo.BusData);
                        break;
                    case "0007"://获取患者病理报告列表
                        OutBusinessInfo.BusData = BUS.GETPATHOLOGYREPORT.B_GETPATHOLOGYEPORT(InBusinessInfo.BusData);
                        break;
                    case "0008":// 查询病理报告明细
                        OutBusinessInfo.BusData = BUS.GETPATHOLOGYRESULT.B_GETPATHOLOGYRESULT(InBusinessInfo.BusData);
                        break;
                    case "0009":// 病理报告打印回传
                        OutBusinessInfo.BusData = BUS.ZZJPATHOLOGYPRNBACK.B_ZZJPATHOLOGYPRNBACK(InBusinessInfo.BusData);
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
            WriteLog("OnlineBusHos8_Tran", "outData", OutBusinessInfo.BusData);
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