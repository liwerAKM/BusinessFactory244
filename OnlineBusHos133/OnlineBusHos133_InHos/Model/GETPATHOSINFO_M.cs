using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos133_InHos.Model
{
    class GETPATHOSINFO_M
    {
        public class GETPATHOSINFO_IN 
        {
            public string HOS_ID { get; set; }
            public string USER_ID { get; set; }
            public string LTERMINAL_SN { get; set; }
            public string HOS_NO { get; set; }
            public string HOS_PAT_ID { get; set; }

            public string BARCODE { get; set; }
            public string SFZ_NO { get; set; }
            public string PAT_NAME { get; set; }
            public string SOURCE { get; set; }
            public string FILTER { get; set; }
        } 
        public class GETPATHOSINFO_OUT 
        {
            public string HOS_NO { get; set; }
            public string HOSPATID { get; set; }
            public string AdmID { get; set; }
            public string JE_PAY { get; set; }
            public string JE_YET { get; set; }
            public string JE_REMAIN { get; set; }
            public string CAN_PAY { get; set; }
            public string HIS_RTNXML { get; set; }

            public List<PAYLIST> payList { get; set; }
            public List<FEELIST> feeList { get; set; }
        }

        /// <summary>
        /// 缴费信息列表
        /// </summary>
        internal class PAYLIST 
        {
           public string HIN_TIME { get; set; }
           public string JE_NOTE { get; set; }
           public string JE { get; set; }

        }

        internal class FEELIST
        {
            public string FEE_NOTE { get; set; }

            public string JE { get; set; }
        }

    }
}
