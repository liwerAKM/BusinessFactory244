using BusinessInterface;
using PasS.Base.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
    /// <summary>
    /// 电子票据
    /// add by wanglei 20190909
    /// </summary>
    public class EBPPAPI : ProcessingBusinessAsyncResult
    {
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            // Thread.Sleep(10000);
            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
            try
            {


                string str = JsonConvert.SerializeObject(InBusinessInfo);
                OutBusinessInfo = new EBPPAPIHelper().DealBuiness(InBusinessInfo);
                /*
                         //方法1：验证此API用户是否有权限调用API "我的API_Name",带入TID目的是为了区别是TCP调用还是HTTP调用
                 if (!base.APIACFCheck(InBusinessInfo.AUID, "我的API_Name", InBusinessInfo.TID))
                 {
                     //没有权限调用此API  继续其他操作
                     //  return true;
                 }
                 */

                //OutBusinessInfo.ReslutCode = 1;//返回结果标识 1成功
                //OutBusinessInfo.ResultMessage = "Sucess";//返回结果说明
                //  OutBusinessInfo.SetBusData(sDingMessageR);
                //  TestUsing.ClassTestUsing classTestUsing = new TestUsing.ClassTestUsing();
                return true;
            }
            catch (Exception ex)
            {
                OutBusinessInfo.ReslutCode = -1;//返回结果标识 1成功
                OutBusinessInfo.ResultMessage = string.Format("Process [{0}] fail:{1}", InBusinessInfo.BusID, ex.Message);//返回结果说明
                return true;
            }
        }

        public override byte[] DefErrotReturn(int Code, string ErrorMsage)
        {
            return new Byte[0];
        }

    }
}
