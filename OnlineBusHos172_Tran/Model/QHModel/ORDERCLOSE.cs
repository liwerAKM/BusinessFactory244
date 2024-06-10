using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos172_Tran.QHModel
{
    public class ORDERCLOSE
    {
        public class Request : BaseRequest
        {
            public string COMM_SN { get; set; }
        }
        public class Response:BaseResponse
        {
        }
    }
}
