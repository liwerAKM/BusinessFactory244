using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSC2BUnionPay.Models
{
    public class QHModel
    {
        public class BODY_OUT
        {
            public string CLBZ { get; set; }
            public string CLJG { get; set; }
        }

        public class BACCOUNTCONFIG_IN
        {
            public string OPERATE { get; set; }
            public string HOS_ID { get; set; }
            public string msgSrc { get; set; }
            public string msgSrcId { get; set; }
            public string md5Key { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string mid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tid { get; set; }
        }


        public class GETQRCODE_IN
        {
            public string HOS_ID { get; set; }
            public decimal CASH_JE { get; set; }
            public string PAT_NAME { get; set; }
            public string SFZ_NO { get; set; }
            /// <summary>
            /// 商品描述
            /// </summary>
            public string BILLDESC { get; set; }
            public string TID { get; set; }
            
        }


        public class GETQRCODE_OUT:BODY_OUT
        {
            public string QUERYID { get; set; }
            public string QRCODE { get; set; }
        }

        public class GETORDERSTATUS_IN
        {
            public string HOS_ID { get; set; }
            public string QUERYID { get; set; }
            public string TID { get; set; }
        }
        public class GETORDERSTATUS_OUT : BODY_OUT
        {
            public string STATUS { get; set; }
        }

        public class PAYREFUND_IN
        {
            public string HOS_ID { get; set; }
            public string QUERYID { get; set; }
            public decimal CASH_JE { get; set; }
            public string TID { get; set; }
        }
        public class PAYREFUND_OUT : BODY_OUT
        {
        }

        public class PAYCANCEL_IN
        {
            public string HOS_ID { get; set; }
            public string QUERYID { get; set; }
            public string TID { get; set; }
        }
        public class PAYCANCEL_OUT : BODY_OUT
        {
        }
        public class NOTIFY_OUT : BODY_OUT
        {
        }

        public class HEADER
        {
            /// <summary>
            /// 
            /// </summary>
            public string MODULE { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string CZLX { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string TYPE { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string source { get; set; }

        }

        public class ROOT
        {
            /// <summary>
            /// 
            /// </summary>
            public HEADER HEADER { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public object BODY { get; set; }

        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public ROOT ROOT { get; set; }
        }
    }
}
