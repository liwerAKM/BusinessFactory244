using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos172_Tran.QHModel
{
    public class GETORDERSTATUS
    {
        public class Request : BaseRequest
        {
            public string COMM_SN { get; set; }
        }
        public class Response : BaseResponse
        {
            public string STATUS { get; set; }
            public string COMM_SN { get; set; }
            public string COMM_UNIT { get; set; }
        }
    }
}
