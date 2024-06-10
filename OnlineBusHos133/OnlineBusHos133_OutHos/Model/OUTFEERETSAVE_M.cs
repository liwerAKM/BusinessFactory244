using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos133_OutHos.Model
{
    internal class OUTFEERETSAVE_M
    {

        public class OUTFEERETSAVE_IN
        {
            /// <summary>
            /// 医院ID
            /// </summary>
            public string HOS_ID { get; set; }
            /// <summary>
            /// 操作员ID
            /// </summary>
            public string USER_ID { get; set; }
            /// <summary>
            /// 自助机终端号
            /// </summary>
            public string LTERMINAL_SN { get; set; }
            public string lTERMINAL_SN { get; set; }


            /// <summary>
            /// 患者院内唯一索引
            /// </summary>
            public string HOSPATID { get; set; }
            public string HOS_SN { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string YY_LSH { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string InvoiceNo { get; set; }
            /// <summary>
            /// 数据来源
            /// </summary>
            public string SOURCE { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string FILTER { get; set; }
            /// <summary>
            /// 核酸标识
            /// </summary>
            public string MB_ID { get; set; }
        }

        public class OUTFEERETSAVE_OUT
        {
            /// <summary>
            /// 平台记录唯一标识
            /// </summary>
            public string STATUS { get; set; }
            public string HIS_RTNXML { get; set; }
            public string PARAMETERS { get; set; }

        }

    }
}
