using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos324_InHos.Model
{
    class SAVEINPATYJJ_M
    {
        public class SAVEINPATYJJ_IN 
        {
            public string HOS_ID { get; set; }
            public string USER_ID { get; set; }
            public string LTERMINAL_SN { get; set; }
            public string HOS_NO { get; set; }
            public string HOSPATID { get; set; }
            public string CASH_JE { get; set; }
            public string DEAL_TYPE { get; set; }
            public string DEAL_STATES { get; set; }
            public string DEAL_TIME { get; set; }
            public string QUERYID { get; set; }
            public string SFZ_NO { get; set; }
            public string SOURCE { get; set; }
            public string FILTER { get; set; }
        }
        public class SAVEINPATYJJ_OUT
        {
            public string HOSPATID { get; set; }
            public string JE_PAY { get; set; }
            public string CASH_JE { get; set; }
            public string JE_REMAIN { get; set; }
            public string HOS_PAY_SN { get; set; }
            public string PARAMETERS { get; set; }
            public string HIS_RTNXML { get; set; }
        }


    }
}
