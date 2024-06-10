using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos36_InHos.Model
{
    class GETPATINFBYHOSNO_M
    {
        public class GETPATINFBYHOSNO_IN
        {
            public string HOS_ID { get; set; }
            public string USER_ID { get; set; }
            public string LTERMINAL_SN { get; set; }
            public string HOS_NO { get; set; }
            public string SFZ_NO { get; set; }
            public string PAT_NAME { get; set; }
            public string SOURCE { get; set; }
            public string FILTER { get; set; }
        }


        public class GETPATINFBYHOSNO_OUT 
        {
            public string HOS_NO { get; set; }
            public string HOSPATID { get; set; }
            public string PAT_NAME { get; set; }
            public string SEX { get; set; }
            public string HIN_TIME { get; set; }
            public string BED_NO { get; set; }
            public string SFZ_NO { get; set; }
            public string HIS_RTNXML { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string PARAMETERS { get; set; }
        }


    }
}
