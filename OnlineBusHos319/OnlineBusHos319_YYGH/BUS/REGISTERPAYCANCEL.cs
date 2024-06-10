using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using System.Reflection.Emit;
using OnlineBusHos319_YYGH.Model;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace OnlineBusHos319_YYGH.BUS
{
    class REGISTERPAYCANCEL
    {
        public static string B_REGISTERPAYCANCEL(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_IN _in = JSONSerializer.Deserialize<Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_IN>(json_in);
                Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_OUT _out = new Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_OUT();

                string QUERYID = string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim();
                string HOS_SN = string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim();
                string SCH_DATE = string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim();
                string HOS_SNAPPT = string.IsNullOrEmpty(_in.HOS_SNAPPT) ? "" : _in.HOS_SNAPPT.Trim();
                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;


                try
                {
                    if (!string.IsNullOrEmpty(QUERYID))
                    {
                        HISModels.A204.A204Request a204Request = new HISModels.A204.A204Request()
                        {
                            clinicNo = HOS_SN,
                            operCode = _operCode,
                            operName = _operName,
                            operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            hospCode = _hospCode,
                            tradeNo = QUERYID
                        };

                        string inputjson = JSONSerializer.Serialize(a204Request);
                        string his_rtnjson = "";

                        if (!PubFunc.CallHISService(HOS_ID, inputjson, "A204", ref his_rtnjson))
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = his_rtnjson;
                            goto EndPoint;
                        }
                        HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);

                        if (baseRsponse.code != "200")
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = baseRsponse.message;
                            goto EndPoint;
                        }

                    }
                    else
                    {
                        if (SCH_DATE != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            JObject Jkeys = new JObject();
                            Jkeys.Add("hospCode", _hospCode);
                            Jkeys.Add("operCode", _operCode);
                            Jkeys.Add("operName", _operName);
                            Jkeys.Add("operTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            Jkeys.Add("bookingNo", HOS_SNAPPT);

                            string inputjson = JSONSerializer.Serialize(Jkeys);
                            string his_rtnjson = "";
                            if (!PubFunc.CallHISService(HOS_ID, inputjson, "A206", ref his_rtnjson))
                            {
                                dataReturn.Code = 1;
                                dataReturn.Msg = his_rtnjson;
                                goto EndPoint;
                            }
                            HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);

                            if (baseRsponse.code != "200")
                            {
                                dataReturn.Code = 1;
                                dataReturn.Msg = baseRsponse.message;
                                goto EndPoint;

                            }
                        }

                    }



                    #region  平台数据保存
                    try
                    {
                        var db = new DbMySQLZZJ().Client;

                        SqlSugarModel.RegisterPay modelregister_pay = db.Queryable<SqlSugarModel.RegisterPay>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOS_SN == _in.HOS_SN).First();
                        SqlSugarModel.RegisterAppt modelregister_appt = db.Queryable<SqlSugarModel.RegisterAppt>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOS_SN == _in.HOS_SN).First();
                        if (modelregister_pay != null && modelregister_appt == null)
                        {
                            modelregister_appt = db.Queryable<SqlSugarModel.RegisterAppt>().Where(t => t.REG_ID == modelregister_pay.REG_ID).First();
                        }
                        //退费
                        if (modelregister_pay != null)
                        {
                            modelregister_pay.IS_TH = true;
                            modelregister_pay.TH_DATE = DateTime.Now.Date;
                            modelregister_pay.TH_TIME = DateTime.Now.ToString("HH:mm:ss");
                            modelregister_pay.TH_SOURCE = "ZZJ";
                            modelregister_pay.TH_USER_ID = _in.USER_ID;
                            modelregister_pay.TH_lTERMINAL_SN = _in.LTERMINAL_SN;
                            db.Updateable(modelregister_pay).ExecuteCommand();

                            modelregister_appt.APPT_TYPE = "3";
                            db.Updateable(modelregister_appt).ExecuteCommand();
                        }
                        else if (modelregister_appt != null)//取消预约挂号
                        {
                            modelregister_appt.APPT_TYPE = "5";
                            modelregister_appt.CANCEL_DATE = DateTime.Now.Date;
                            modelregister_appt.CANCEL_TIME = DateTime.Now.ToString("HH:mm:ss");
                            db.Updateable(modelregister_appt).ExecuteCommand();
                        }



                    }
                    catch (Exception ex)
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "REGISTERPAYCANCEL";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }
                    #endregion

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                    dataReturn.Param = ex.Message;
                }

            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}
