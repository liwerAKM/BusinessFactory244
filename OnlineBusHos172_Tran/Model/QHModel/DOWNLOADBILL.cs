using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos172_Tran.QHModel
{
    public class DOWNLOADBILL
    {
        public class Request :BaseRequest
        {
            /// <summary>
            /// TRADEDATE
            /// </summary>
            public string TRADEDATE { get; set; }
        }
        public class Response:BaseResponse
        {
            public List<StatementDetail> statements { get; set; }
        }
        public class StatementDetail
        {
            /// <summary>
            /// HOS_ID
            /// </summary>
            public string HOS_ID { get; set; }

            /// <summary>
            /// ZF_TYPE
            /// </summary>
            public string ZF_TYPE { get; set; }

            /// <summary>
            /// DATAORIGIN
            /// </summary>
            public string DATAORIGIN { get; set; }

            /// <summary>
            /// COMM_MAIN
            /// </summary>
            public string COMM_MAIN { get; set; }

            /// <summary>
            /// COMM_SN
            /// </summary>
            public string COMM_SN { get; set; }

            /// <summary>
            /// TXN_TYPE
            /// </summary>
            public string TXN_TYPE { get; set; }

            /// <summary>
            /// TRADEDATE
            /// </summary>
            public string TRADEDATE { get; set; }

            /// <summary>
            /// TRADETIME
            /// </summary>
            public string TRADETIME { get; set; }
            /// <summary>
            /// TRADESTATUS 
            /// </summary>
            public string TRADESTATUS { get; set; }

            /// <summary>
            /// JEALL
            /// </summary>
            public decimal JEALL { get; set; }

            /// <summary>
            /// YBCASH
            /// </summary>
            public decimal? YBCASH { get; set; }

            /// <summary>
            /// XJCASH
            /// </summary>
            public decimal XJCASH { get; set; }
            /// <summary>
            /// FEE
            /// </summary>
            public decimal? FEE { get; set; }
            /// <summary>
            /// FEEDESC
            /// </summary>
            public string FEEDESC { get; set; }

            /// <summary>
            /// TRACETYPE
            /// </summary>
            public string TRACETYPE { get; set; }

            /// <summary>
            /// ACCOUNT
            /// </summary>
            public string ACCOUNT { get; set; }

            /// <summary>
            /// ORDERNO
            /// </summary>
            public string ORDERNO { get; set; }

            /// <summary>
            /// varchar(1000)
            /// </summary>
            public string EXPCONTENT { get; set; }

            /// <summary>
            /// varchar(20)
            /// </summary>
            public string BY1 { get; set; }

            /// <summary>
            /// varchar(50)
            /// </summary>
            public string BY2 { get; set; }

            /// <summary>
            /// decimal
            /// </summary>
            public decimal? BY3 { get; set; }

        }
    }
}
