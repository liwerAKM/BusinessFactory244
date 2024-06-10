using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZJEINVOICE.Class;
using CommonModel;
using ConstData;
using Soft.Core;

namespace ZZJEINVOICE.BUS
{
    class UpdatePrintStatus_B
    {
        public static string UpdatePrintStatus(string json_in)
        {
            UpdatePrintStatus_IN _in = JSONSerializer.Deserialize<UpdatePrintStatus_IN>(json_in);
            DataReturn dataReturn = new DataReturn();
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}
