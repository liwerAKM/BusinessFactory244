using CommonModel;
using System;
using System.Collections.Generic;
using Soft.Core;
using System.Xml;
using System.Data;

namespace ZZJ_YYGH.BUS
{
    internal class GETINSUREGPARA
    {
        public static string B_GETINSUREGPARA(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Dictionary<string, object> dic = JSONSerializer.Deserialize<Dictionary<string, object>>(json_in);
                if (!dic.ContainsKey("HOS_ID") || FormatHelper.GetStr(dic["HOS_ID"]) == "")
                {
                    dataReturn.Code = ConstData.CodeDefine.Parameter_Define_Out;
                    dataReturn.Msg = "HOS_ID为必传且不能为空";
                    goto EndPoint;
                }
                string out_data = GlobalVar.CallOtherBus(json_in, FormatHelper.GetStr(dic["HOS_ID"]), "ZZJ_YYGH", "0009").BusData;
                return out_data;
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常："+ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}
