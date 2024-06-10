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

namespace OnlineBusHos324_InHos
{
    class PBusHos324_InHos : ProcessingBusinessAsyncResult
    {
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {

            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
            try
            {
                string name = InBusinessInfo.SubBusID;
                switch (name)//CCN.ToString().Substring(CCN.ToString().Length - 4)
                {
                    case "0001"://获取患者住院号
                        OutBusinessInfo.BusData = BUS.GETPATHOSNO.B_GETPATHOSNO(InBusinessInfo.BusData);
                        break;
                    case "0002"://获取医生科室排班数据
                        OutBusinessInfo.BusData = BUS.GETPATHOSINFO.B_GETPATHOSINFO(InBusinessInfo.BusData);
                        break;
                    case "0003"://根据住院号获取住院信息
                        OutBusinessInfo.BusData = BUS.GETPATINFBYHOSNO.B_GETPATINFBYHOSNO(InBusinessInfo.BusData);
                        break;
                    case "0004"://缴纳住院预交金
                        OutBusinessInfo.BusData = BUS.SAVEINPATYJJ.B_SAVEINPATYJJ(InBusinessInfo.BusData);
                        break;
                    case "0005"://预约取消(含支付)
                        OutBusinessInfo.BusData = BUS.GETHOSDAILY.B_GETHOSDAILY(InBusinessInfo.BusData);
                        break;
                    //case "0006"://获取指定医院科室(专家)日期排班时间段
                    //    OutBusinessInfo.BusData = BUS.GETSCHPERIOD.B_GETSCHPERIOD(InBusinessInfo.BusData);
                    //    break;
                    //case "0007"://获取可预约日期
                    //    OutBusinessInfo.BusData = BUS.GETSCHDATE.B_GETSCHDATE(InBusinessInfo.BusData);
                    //    break;
                    //case "0008"://获取日期上下午时段排班
                    //    OutBusinessInfo.BusData = BUS.GETSCHTIME.B_GETSCHTIME(InBusinessInfo.BusData);
                    //    break;
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
