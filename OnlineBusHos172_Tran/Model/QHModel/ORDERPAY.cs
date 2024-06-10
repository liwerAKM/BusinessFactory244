using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos172_Tran.QHModel
{
    public class ORDERPAY
    {
        public class Request : BaseRequest
        {
            public decimal JE { get; set; }
            /// <summary>
            /// 商品描述
            /// </summary>
            public string ORDER_DESC { get; set; }
            public string QRCODE { get; set; }
            public string COMM_HIS { get; set; }
        }
        public class Response:BaseResponse
        {
            public string COMM_SN { get; set; }
            public string COMM_UNIT { get; set; }
            
        }
    }
}
