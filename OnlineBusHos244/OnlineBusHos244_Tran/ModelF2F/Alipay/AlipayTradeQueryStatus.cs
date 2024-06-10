using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos244_Tran
{
    internal class AlipayTradeQueryStatus
    {
        public RESPONSE response { get; set; }
        public class RESPONSE
        {
            public string TradeStatus { get; set; }
            public string Code { get; set; }
            public string Msg { get; set; }
            public string Body { get; set; }


        }
    }
}
