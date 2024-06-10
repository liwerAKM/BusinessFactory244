using PasS.Base.Lib;
using System;

namespace BusinessInterface
{
    public abstract class IBusiness : ProcessingBusinessBase
    {
        public override bool ProcessingBusiness(string strJson)
        {
            return true;
        }
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo)
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
    }
}
