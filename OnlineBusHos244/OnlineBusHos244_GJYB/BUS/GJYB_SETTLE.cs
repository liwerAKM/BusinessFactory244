using System;
using System.Collections.Generic;
using System.Text;
using CommonModel;
using Soft.Core;
namespace OnlineBusHos244_GJYB.BUS
{
    class GJYB_SETTLE
    {
        public static string B_GJYB_SETTLE(string json_in)
        {
            DataReturn dataReturn = GlobalVar.business.SETTLE(json_in);
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}
