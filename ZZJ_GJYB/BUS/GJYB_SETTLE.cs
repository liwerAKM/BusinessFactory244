﻿using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZZJ_GJYB.BUS
{
    class GJYB_SETTLE
    {
        public static string B_GJYB_SETTLE(string json_in)
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
                string out_data = GlobalVar.CallOtherBus(json_in, FormatHelper.GetStr(dic["HOS_ID"]), "ZZJ_GJYB", "0004").BusData;
                return out_data;
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.ToString();
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}