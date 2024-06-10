using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos172_Tran.QHModel
{
    public class BaseRequest
    {
        public string CLIENT_ID { get; set; }
        public string HOS_ID { get; set; }
        public string PAY_TYPE { get; set; }
    }
    public class BaseResponse
    {
        public string CLBZ { get; set; }
        public string CLJG { get; set; }

        /// <summary>
        /// 支付机构返回原始数据
        /// </summary>
        public string ReData { get; set; }
    }
}
