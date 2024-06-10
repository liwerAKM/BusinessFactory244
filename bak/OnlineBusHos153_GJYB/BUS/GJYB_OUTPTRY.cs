using System;
using System.Collections.Generic;
using System.Text;
using CommonModel;
using Soft.Core;

namespace OnlineBusHos153_GJYB.BUS
{
    class GJYB_OUTPTRY
    {
        public static string B_GJYB_OUTPTRY(string json_in)
        {
            DataReturn dataReturn = GlobalVar.business.OUTPTRY(json_in);
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        //    DataReturn dataReturn = new DataReturn();
        //    string json_out = "";
        //    try
        //    {
        //        Dictionary<string, object> dic = JSONSerializer.Deserialize<Dictionary<string, object>>(json_in);
        //        if (!dic.ContainsKey("HOS_ID") || FormatHelper.GetStr(dic["HOS_ID"]) == "")
        //        {
        //            dataReturn.Code = ConstData.CodeDefine.Parameter_Define_Out;
        //            dataReturn.Msg = "HOS_ID为必传且不能为空";
        //            goto EndPoint;
        //        }
        //        string out_data = GlobalVar.CallOtherBus(json_in, FormatHelper.GetStr(dic["HOS_ID"]), "OnlineBusHos153_GJYB", "0004").BusData;
        //        return out_data;
        //    }
        //    catch (Exception ex)
        //    {
        //        dataReturn.Code = 6;
        //        dataReturn.Msg = "程序处理异常";
        //        dataReturn.Param = ex.ToString();
        //    }
        //EndPoint:
        //    json_out = JSONSerializer.Serialize(dataReturn);
        //    return json_out;
        }
    }
}
