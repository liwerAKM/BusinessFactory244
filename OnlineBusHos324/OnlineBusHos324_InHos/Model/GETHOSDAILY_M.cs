using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos324_InHos.Model
{
    class GETHOSDAILY_M
    {
        public class GETHOSDAILY_IN
        {
            public string HOS_ID { get; set; }
            public string USER_ID { get; set; }
            public string LTERMINAL_SN { get; set; }
            public string HOS_NO { get; set; }
            public string HOSPATID { get; set; }
            public string BEGIN_DATE { get; set; }
            public string END_DATE { get; set; }
            public string SFZ_NO { get; set; }
            public string SOURCE { get; set; }
            public string FILTER { get; set; }
        }

        public class GETHOSDAILY_OUT
        {
            public string HOSPATID { get; set; }
            public string JE_PAY { get; set; }
            public string JE_YET { get; set; }
            public string JE_REMAIN { get; set; }
            public string JE_TODAY { get; set; }
            public string HIS_RTNXML { get; set; }

            public List<BIGITEM> BIGITEMLIST { get; set; }



        }

        public class BIGITEM
        {
            public string ITEM_NAME { get; set; }
            public string JE_ALL { get; set; }

            public List<ITEM> ITEMLIST { get; set; }

            public List<GITEM> GITEMLIST { get; set; }
        }

        public class ITEM
        {
            public string NAME { get; set; }
            public string GG { get; set; }
            public string AMOUNT { get; set; }
            public string CAMT { get; set; }
            public string JE { get; set; }
            public string JE_ALL { get; set; }
            public string DJ_DATE { get; set; }
            public string PARAMETERS { get; set; }
        }

        public class GITEM
        {
            public string BIG_NAME { get; set; }
            public string NAME { get; set; }
            public string GG { get; set; }
            public string AMOUNT { get; set; }
            public string CAMT { get; set; }
            public string JE { get; set; }
            public string JE_ALL { get; set; }
        }



    }
}
