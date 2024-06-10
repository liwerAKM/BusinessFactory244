using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos172_Tran.QHModel
{
    public class ORDERREFUND
    {
        public class Request : BaseRequest
        {
            public string COMM_SN { get; set; }
            public decimal JE { get; set; }
        }
        public class Response:BaseResponse
        {
            public string COMM_SN { get; set; }
            public string COMM_UNIT { get; set; }
        }
    }
}
