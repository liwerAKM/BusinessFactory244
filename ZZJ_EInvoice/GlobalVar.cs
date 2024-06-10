﻿using BusinessInterface;
using CommonModel;
using DB.Core;
using Log.Core.Model;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace ZZJ_EInvoice
{
    class GlobalVar
    {
        public static SLBBusinessInfo CallOtherBus(string data, string HOS_ID, string SLB_ID, string subSubId)
        {
            try
            {
                DateTime nowIn = DateTime.Now;
                SLBBusinessInfo OutSLBBOtherBus = new SLBBusinessInfo();

                string key = HOS_ID + "_" + SLB_ID;
                DataTable dtconfig = DictionaryCacheHelper.GetCache(key, () => GetSLBBusinessInfo(SLB_ID, HOS_ID));
                if (dtconfig.Rows.Count == 0)
                {
                    dtconfig = DictionaryCacheHelper.UpdateCache(key, () => GetSLBBusinessInfo(SLB_ID, HOS_ID));
                    if (dtconfig.Rows.Count == 0)
                    {
                        DataReturn dataReturn = new CommonModel.DataReturn();
                        dataReturn.Code = ConstData.CodeDefine.BusError;
                        dataReturn.Msg = "未配置模块对应院端服务";
                        OutSLBBOtherBus.BusData = JSONSerializer.Serialize(dataReturn);
                        goto EndPoint;
                    }
                }
                else
                {
                    TimeSpan ts = new TimeSpan();
                    ts = DateTime.Now - DateTime.Parse(dtconfig.Rows[0]["CURRENT_TIMESTAMP"].ToString());
                    if (ts.Minutes > 5)
                    {
                        dtconfig = DictionaryCacheHelper.UpdateCache(key, () => GetSLBBusinessInfo(SLB_ID, HOS_ID));
                    }
                }

                SLBBusinessInfo SLBBOtherBus = new SLBBusinessInfo();
                SLBBOtherBus.BusID = Soft.Core.FormatHelper.GetStr(dtconfig.Rows[0]["BUS_ID"]);
                SLBBOtherBus.SubBusID = subSubId;
                SLBBOtherBus.BusData = data;

                bool result = BusServiceAdapter.Ipb_CallOtherBusiness(SLBBOtherBus, out OutSLBBOtherBus);


            EndPoint:
                Log.Core.Model.ModLogAPP modLogAPP = new Log.Core.Model.ModLogAPP();
                modLogAPP.inTime = nowIn;
                modLogAPP.outTime = DateTime.Now;
                modLogAPP.inXml = data;
                modLogAPP.outXml = OutSLBBOtherBus.BusData;
                Log.Core.LogHelper.Addlog(modLogAPP);
                return OutSLBBOtherBus;
            }
            catch (Exception ex)
            {
                Log.Core.Model.ModLogAPPError modLogAPPError = new Log.Core.Model.ModLogAPPError();
                modLogAPPError = new Log.Core.Model.ModLogAPPError();
                modLogAPPError.inTime = DateTime.Now;
                modLogAPPError.outTime = DateTime.Now;
                modLogAPPError.inXml = data;
                modLogAPPError.TYPE = "2020";
                modLogAPPError.outXml = ex.ToString();

                Log.Core.LogHelper.Addlog(modLogAPPError);
                return null;
            }
        }

        public static DataTable GetSLBBusinessInfo(string SLB_ID, string HOS_ID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select *,CURRENT_TIMESTAMP from baccountSlbToHos where Slb_ID=@SLB_ID and HOS_ID=@HOS_ID");
            MySqlParameter[] parameters =
            {
                    new MySqlParameter("@SLB_ID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20)
                };
            parameters[0].Value = SLB_ID;
            parameters[1].Value = HOS_ID;
            DataTable dtconfig = DbHelperPlatZzjSQL.Query(sb.ToString(), parameters).Tables[0];
            return dtconfig;
        }
    }

}
