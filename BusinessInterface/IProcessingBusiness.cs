using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasS.Base.Lib;

namespace BusinessInterface
{
    public delegate bool CallOtherBusinessEventHandler<T>(T In, out T Out);

    /// <summary>
    ///真正执行工作的类继承此接口
    /// </summary>
    public interface IProcessingBusiness
    {
        // event CallOtherBusinessEventHandler<SLBBusinessInfo> CallOtherBusinessEvent;
        // bool CallOtherBusiness(SLBBusinessInfo In, out SLBBusinessInfo Out);
        //bool  HasBandCallOtherBusiness{get; }
        /// <summary>
        /// 不需要返回、交互、订阅的需要实现此接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        bool ProcessingBusiness(string strJson);

        /// <summary>
        ///工作任务种类<see cref="BusType.AsyncResult"/>结果的需要实现此接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        bool ProcessingBusiness(string strJson, out string strJsonResult);


        bool ProcessingBusiness(SLBBusinessInfo In, out SLBBusinessInfo Out);
        bool ProcessingBusiness(SLBBusinessInfo In);

        bool ProcessingBusiness(int CCN, byte [] In, out byte[] Out);

        bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In, out byte[] Out);

        
        bool ProcessingBusiness(int CCN, byte[] In);
        bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In);
        /// <summary>
        /// 工作任务种类<see cref="BusType.AllowPublish"/>为True的需要实现此接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <param name="Allow"></param>
        /// <param name="RoutingKey"></param>
        /// <returns></returns>
        bool ProcessingBusiness(string strJson, out string strJsonResult, ref bool Allow, ref string RoutingKey);

        Byte [] DefErrotReturn(int  Code,string ErrorMsage);

        OutputRoot Trans(InputRoot inputRoot);
    }

    

    /// <summary>
    ///真正执行工作的基类，他们继承此类 
    /// </summary>
    public abstract class ProcessingBusinessBase : IProcessingBusiness
    {
        //   public event CallOtherBusinessEventHandler<SLBBusinessInfo> CallOtherBusinessEvent;
        public string GetStrData(byte[] bInfo )
        {
            return Encoding.UTF8.GetString(bInfo);
        }
        /// <summary>
        /// 转换为二进制
        /// </summary>
        /// <param name="sInfo"></param>
        /// <returns></returns>
        public byte[] GetByte(string sInfo)
        {
            return Encoding.UTF8.GetBytes(sInfo);
        }
        /// <summary>
        /// 转换为二进制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public byte[] GetByte<T>(T t)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(t));
        }
        protected bool APIACFCheck(string AUID, string API_Name)
        {
            //return BusServiceAdapter.APIACFCheck(AUID, API_Name);
          return true;  
        }

        protected bool APIACFCheck(string AUID, string API_Name, string TID)
        {
            return true;
          //  return BusServiceAdapter.APIACFCheck(AUID, API_Name, TID);
        }
        protected bool APIACFCheck(string AUID, string API_Name, string TID, ref SLBBusinessInfo OutBusinessInfo)
        {
            return true;
            //bool pass = BusServiceAdapter.APIACFCheck(AUID, API_Name, TID);
            //if (!pass)
            //{
            //    OutBusinessInfo.ReslutCode = -1;
            //    OutBusinessInfo.ResultMessage = "You don't have permission to access the API[" + API_Name + "]";
            //}
            //return pass;
        }

        protected bool CallOtherBusiness(SLBBusinessInfo In, out SLBBusinessInfo Out)
        {
            return BusServiceAdapter.Ipb_CallOtherBusiness(In, out Out);
            //return CallOtherBusinessEvent(In, out Out);
        }
        
        protected bool CallOtherBusiness(InputRoot In, out OutputRoot Out)
        {
            return BusServiceAdapter.Ipb_CallOtherBusiness(In, out Out);
            //return CallOtherBusinessEvent(In, out Out);
        }
        protected bool CallOtherBusiness<T>(string BusID, string SubBusID, string BusData, out T t, out string ResultMessage, out int ReslutCode)
        {
            ReslutCode = CallOtherBusiness<T>(BusID, SubBusID, BusData, out t, out ResultMessage);
            return ReslutCode == 1;
        }

        protected int CallOtherBusiness<T>(string BusID, string SubBusID, string BusData, out T t, out string ResultMessage)
        {
            SLBBusinessInfo In = new SLBBusinessInfo();
            In.BusID = BusID;
            In.SubBusID = SubBusID;
            In.BusData = BusData;
            In.TID = BusID + Guid.NewGuid().GetHashCode().ToString();
            ResultMessage = "";
            t = default(T);
            SLBBusinessInfo OutSLBBOtherBus;
            int retint = 0;
            bool ret = BusServiceAdapter.Ipb_CallOtherBusiness(In, out OutSLBBOtherBus);
            if (ret)
            {
                if (OutSLBBOtherBus != null)
                {
                    if (OutSLBBOtherBus.ReslutCode == 1)//返回结果标识 1成功
                    {

                        try
                        {
                            t = JsonConvert.DeserializeObject<T>(OutSLBBOtherBus.BusData);
                            retint = 1;
                        }
                        catch (Exception ex)
                        {
                            ResultMessage = "Call[" + In.BusID + "] Fail :Convert BusData Fail ：" + OutSLBBOtherBus.BusData;
                            retint = 0;
                        }
                    }
                    else
                    {
                        retint = OutSLBBOtherBus.ReslutCode;
                        ResultMessage = OutSLBBOtherBus.ResultMessage;
                    }
                }
                else
                {
                    retint = -1;
                    ResultMessage = "Call[" + In.BusID + "] Fail ";
                    if (OutSLBBOtherBus != null)
                        ResultMessage += OutSLBBOtherBus.ResultMessage;
                }
            }
            return retint;
        }
        protected bool CallOtherBusiness(string BusID, string SubBusID, string BusData, out SLBBusinessInfo Out)
        {
            SLBBusinessInfo In = new SLBBusinessInfo();
            In.BusID = BusID;
            In.SubBusID = SubBusID;
            In.BusData = BusData;
            In.TID = BusID + Guid.NewGuid().GetHashCode().ToString();
            return BusServiceAdapter.Ipb_CallOtherBusiness(In, out Out);
            //return CallOtherBusinessEvent(In, out Out);
        }
        protected bool CallOtherBusiness(string BusID, string SubBusID, string BusData, string CTag, out SLBBusinessInfo Out)
        {
            SLBBusinessInfo In = new SLBBusinessInfo();
            In.BusID = BusID;
            In.SubBusID = SubBusID;
            In.BusData = BusData;
            In.CTag = CTag;
            In.TID = BusID + Guid.NewGuid().GetHashCode().ToString();
            return BusServiceAdapter.Ipb_CallOtherBusiness(In, out Out);
            //return CallOtherBusinessEvent(In, out Out);
        }

        //public bool HasBandCallOtherBusiness
        //{
        //    get
        //    {

        //        return CallOtherBusinessEvent != null;
        //    }
        //}
        /// <summary>
        /// 不需要返回、交互、订阅的需要实现此接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public abstract bool ProcessingBusiness(string strJson);

        /// <summary>
        ///工作任务种类<see cref="BusType.AsyncResult"/>结果的需要实现此接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        public abstract bool ProcessingBusiness(string strJson, out string strJsonResult);

        public abstract bool ProcessingBusiness(SLBBusinessInfo In);

        public abstract bool ProcessingBusiness(SLBBusinessInfo In, out SLBBusinessInfo Out);

        public abstract bool ProcessingBusiness(int CCN, byte [] In);

        public abstract bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out);

        public abstract bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In, out byte[] Out);
        public abstract bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In);

        /// <summary>
        /// 工作任务种类<see cref="TasksInfo.AllowPublish"/>为True的需要实现此接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <param name="Allow"></param>
        /// <param name="RoutingKey"></param>
        /// <returns></returns>
        public abstract bool ProcessingBusiness(string strJson, out string strJsonResult, ref bool Allow, ref string RoutingKey);

        /// <summary>
        /// 默认错误处理
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="ErrorMsage"></param>
        /// <returns></returns>
        public abstract Byte[] DefErrotReturn(int Code, string ErrorMsage);
        public abstract OutputRoot Trans(InputRoot inputRoot);
    }
    /// <summary>
    /// 不需要返回、交互、订阅的，可直接继承此类实现对应接口
    /// </summary>
    public abstract class ProcessingBusinessNormal : ProcessingBusinessBase
    {


        public override bool ProcessingBusiness(string strJson)
        {
            return true;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson, out string strJsonResult)
        {

            strJsonResult = "";
            return true;
        }
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = InBusinessInfo;
            return true;
        }
        public override bool ProcessingBusiness(string strJson, out string strJsonResult, ref bool AllowPublish, ref string RoutingKey)
        {
            strJsonResult = "";
            return true;
        }
        public override bool ProcessingBusiness(int CCN,byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
    }
    /// <summary>
    /// 不需要返回、交互、订阅的，可直接继承此类实现对应接口<see cref="BusType.ImmediateByte"/>
    /// </summary>
    public abstract class ProcessingBusinessNormalByte : ProcessingBusinessBase
    {

       

        public override bool ProcessingBusiness(string strJson)
        {
            return true;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson, out string strJsonResult)
        {

            strJsonResult = "";
            return true;
        }
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo )
        {
           
            return true;
        }
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = InBusinessInfo;
            return true;
        }
        public override bool ProcessingBusiness(string strJson, out string strJsonResult, ref bool AllowPublish, ref string RoutingKey)
        {
            strJsonResult = "";
            return true;
        }
        

        public override bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }

    }

    /// <summary>
    /// 工作任务种类<see cref="BusType.AsyncResult "/>，可直接继承此类实现对应接口
    /// </summary>
    public abstract class ProcessingBusinessAsyncResult : ProcessingBusinessBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson)

        {
            return true;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson, out string strJsonResult)
        {
            strJsonResult = "";
            return true;
        }

        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo)
        {

            return true;
        }

        public override bool ProcessingBusiness(string strJson, out string strJsonResult, ref bool AllowPublish, ref string RoutingKey)
        {
            strJsonResult = "";
            return true;
        }
        public override bool ProcessingBusiness(int CCN, byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
      
        public override OutputRoot Trans(InputRoot inputRoot)
        {   return  new OutputRoot();
        }
    }
    /// <summary>
    /// 工作任务种类<see cref="BusType.AsyncResultByte "/>，可直接继承此类实现对应接口
    /// </summary>
    public abstract class ProcessingBusinessAsyncResultByte : ProcessingBusinessBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson)

        {
            return true;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson, out string strJsonResult)
        {
            strJsonResult = "";
            return true;
        }

        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo)
        {

            return true;
        }
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = InBusinessInfo;
            return true;
        }

        public override bool ProcessingBusiness(string strJson, out string strJsonResult, ref bool AllowPublish, ref string RoutingKey)
        {
            strJsonResult = "";
            return true;
        }
        public override bool ProcessingBusiness(int CCN, byte[] In)
        {
            return true;
        }
        public override bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In)
        {
            return true;
        }

        public override OutputRoot Trans(InputRoot inputRoot)
        {
            return new OutputRoot();
        }

    }

    public abstract class ProcessingBusinessPublish : ProcessingBusinessBase
    {

        public override bool ProcessingBusiness(string strJson)
        {
            return true;
        }

        /// <summary>
        /// 需要异步返回结果的需要实现此接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson, out string strJsonResult)
        {
            strJsonResult = "";
            return true;
        }

        public override bool ProcessingBusiness(SLBBusinessInfo sLBBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = sLBBusinessInfo;
            return true;
        }
        public override bool ProcessingBusiness(int CCN, byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
        public override OutputRoot Trans(InputRoot inputRoot)
        {
            return new OutputRoot();
        }
    }

}
