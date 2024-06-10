using BusinessInterface;
using PasS.Base.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSC2BUnionPay
{
    /// <summary>
    /// POS通插件C扫B业务
    /// </summary>
    public class PBusPOSC2BUnionPay : ProcessingBusinessAsyncResult
    {
        public override byte[] DefErrotReturn(int Code, string ErrorMsage)
        {
            throw new NotImplementedException();
        }

        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
            try
            {
                string context = InBusinessInfo.BusData;
                string SubBusID = InBusinessInfo.SubBusID;
                if (SubBusID == "NOTIFY")
                {
                    OutBusinessInfo.BusData = Entrance.NOTIFY(context);
                }
                else if(SubBusID == "V1NETPAY")
                {
                    OutBusinessInfo.BusData = EntranceV1NETPAY.DoBusiness(context);
                }
                else
                {
                    OutBusinessInfo.BusData = Entrance.DoBusiness(context);
                }
                
                OutBusinessInfo.ReslutCode = 1;//返回结果标识 1成功
                OutBusinessInfo.ResultMessage = "Success";//返回结果说明
                return true;
            }
            catch (Exception ex)
            {
                OutBusinessInfo.ReslutCode = 0;//返回结果标识 1成功
                OutBusinessInfo.ResultMessage = string.Format("Process [{0}] fail:{1}", InBusinessInfo.BusID, ex.Message);//返回结果说明
                return true;
            }
        }
    }
}
