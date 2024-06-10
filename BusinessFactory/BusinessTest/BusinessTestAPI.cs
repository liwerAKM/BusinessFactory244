using BusinessInterface;
using PasS.Base.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTest
{
    /// <summary>
    /// 电子票据
    /// add by wanglei 20190909
    /// </summary>
    public class BusinessTestAPI : IBusiness
    {
        public override  OutputRoot Trans(InputRoot input)
        {
            // Thread.Sleep(10000);
            OutputRoot  outroot = new OutputRoot(input);
            try
            {
                outroot.OutData = input.InData;


                outroot.RspCode = "0000";
                outroot.RspMsg = "sucess";
                if (input.infno == "0001")
                {
                    SLBBusinessInfo sLBBusinessin = new SLBBusinessInfo();
                    sLBBusinessin.BusID = "EBPP";
                    sLBBusinessin.BusData =(string) input.InData;
                    SLBBusinessInfo sLBBusinessout = new SLBBusinessInfo();
                    base.CallOtherBusiness(sLBBusinessin, out sLBBusinessout);
                    outroot.OutData = sLBBusinessout.BusData;
                    outroot.RspCode = sLBBusinessout.ReslutCode.ToString();
                    outroot.RspMsg = sLBBusinessout.ResultMessage;
                }

            
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
                return outroot;
            }
            catch (Exception ex)
            {
                outroot.RspCode ="9996";//返回结果标识  
                outroot.RspMsg = string.Format("Process [{0}] fail:{1}", input.BusID, ex.Message);//返回结果说明
                return outroot;
            }
        }

        public override byte[] DefErrotReturn(int Code, string ErrorMsage)
        {
            return new Byte[0];
        }

    }
}
