using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos172_Tran.QHModel
{
    public class CREATEH5PAYORDER
    {
        public class Request : BaseRequest
        {
            public decimal JE { get; set; }
            /// <summary>
            /// 商品描述
            /// </summary>
            public string ORDER_DESC { get; set; }

            /// <summary>
            /// 前台通知地址，支付成功时前台跳转使用
            /// </summary>
            public string FRONT_NOTIFY_URL { get; set; }

            public string EXPIRE_MINUTES { get; set; }

            public string BIZ_TYPE { get; set; }
            public string APPT_SN { get; set; }
            public string SOURCE { get; set; }
        }
        public class Response: BaseResponse
        {
            public string COMM_SN { get; set; }
            public string COMM_UNIT { get; set; }
            public string H5PAYURL { get; set; }
        }
    }
}
