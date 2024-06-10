using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos298_YYGH.Model
{
    class GETINSUREGPARA_M
    {
        public class GETINSUREGPARA_IN
        {
            public string HOS_ID { get; set; }
            /// <summary>
            /// 患者唯一号
            /// </summary>
            public string BARCODE { get; set; }
            /// <summary>
            /// 排班类型
            /// </summary>
            public string REGISTER_TYPE { get; set; }


        }

        public class GETINSUREGPARA_OUT
        {
            /// <summary>
            /// 红冲需要
            /// </summary>
            public string USERID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string PAADMID { get; set; }
            /// <summary>
            /// 医保入参
            /// </summary>
            public string EXPSTRING { get; set; }

            public string HIS_RTNXML { get; set; }
        }
    }
}
