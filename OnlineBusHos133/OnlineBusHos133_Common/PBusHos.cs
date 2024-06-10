﻿using BusinessInterface;
using CommonModel;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OnlineBusHos133_Common
{
    class PBusHos133_Common : ProcessingBusinessAsyncResult
    {
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
            try
            {

                string name = InBusinessInfo.SubBusID;
                switch (name)//CCN.ToString().Substring(CCN.ToString().Length - 4)
                {
                    case "0001"://获取病人基础信息
                        OutBusinessInfo.BusData = BUS.GETPATINFO.B_GETPATINFO(InBusinessInfo.BusData);
                        break;
                    case "0002"://保存病人建档信息
                        OutBusinessInfo.BusData = BUS.GETPATRECORD.B_GETPATRECORD(InBusinessInfo.BusData);
                        break;
                    case "0005"://凭条打印
                        OutBusinessInfo.BusData = BUS.TICKETREPRINT.B_TICKETREPRINT(InBusinessInfo.BusData);
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
            //WriteLog("OnlineBusHos968_OutHosAPI", "inData",Newtonsoft.Json.JsonConvert.SerializeObject(InBusinessInfo));
            //WriteLog("OnlineBusHos968_OutHosAPI", "outData",  OutBusinessInfo.BusData);
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
    }
}
