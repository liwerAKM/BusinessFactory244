using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos36_InHos.Model
{
    class GETPATHOSNO_M
    {
       

        public class GETPATHOSNO_IN
        {
            public string HOS_ID { get; set; }
            public string USER_ID { get; set; }
            public string LTERMINAL_SN { get; set; }
            public string YLCARD_TYPE { get; set; }
            public string YLCARD_NO { get; set; }
            public string HOSPATID { get; set; }
            public string PAT_NAME { get; set; }
            public string SFZ_NO { get; set; }
            public string SOURCE { get; set; }
            public string FILTER { get; set; }
        }

        public class GETPATHOSNO_OUT
        {
            /// <summary>
            /// 住院号
            /// </summary>
            public string HOS_NO { get; set; }

            /// <summary>
            /// 住院病人唯一索引
            /// </summary>
            public string HOSPATID { get; set; }
            public string HIS_RTNXML { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string PARAMETERS { get; set; }
        }

    }
}
