using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos44_Tran
{
    internal class AlipayTradeCreate
    {
        public RESPONSE response { get; set; }
        public class RESPONSE
        {
            public string OutTradeNo { get; set; }
            public string QrCode { get; set; }
            public string Code { get; set; }
            public string Msg { get; set; }
            public string Body { get; set; }


        }

    }
}
