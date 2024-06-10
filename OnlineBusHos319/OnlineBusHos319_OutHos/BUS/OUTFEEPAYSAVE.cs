using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using DB.Core;
using Google.Protobuf.WellKnownTypes;
using System.Reflection.Emit;
using OnlineBusHos319_OutHos.Model;
using Newtonsoft.Json.Linq;

namespace OnlineBusHos319_OutHos.BUS
{
    class OUTFEEPAYSAVE
    {
        public static string B_OUTFEEPAYSAVE(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {;

                Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN _in = JSONSerializer.Deserialize<Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN>(json_in);
                Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT _out = new Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT();

                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string HOSPATID = _in.HOSPATID;
                string DEAL_TYPE = string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim();
                string HOS_SN = string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim();
                DateTime DEAL_TIME = Convert.ToDateTime(_in.DEAL_TIME);
                string JE_ALL = string.IsNullOrEmpty(_in.JEALL) ? "" : _in.JEALL.Trim();
                string QUERYID = string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim();
                string PRE_NO = string.IsNullOrEmpty(_in.PRE_NO) ? "" : _in.PRE_NO.Trim();
                string YLCARD_TYPE = string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim();
                string OPT_SN = string.IsNullOrEmpty(_in.OPT_SN) ? "" : _in.OPT_SN.Trim();
                string MDTRT_CERT_TYPE = string.IsNullOrEmpty(_in.MDTRT_CERT_TYPE) ? "" : _in.MDTRT_CERT_TYPE.Trim();
                string MDTRT_CERT_NO = string.IsNullOrEmpty(_in.MDTRT_CERT_NO) ? "" : _in.MDTRT_CERT_NO.Trim();
                string BEGNTIME = string.IsNullOrEmpty(_in.BEGNTIME) ? "" : _in.BEGNTIME.Trim();
                string CARD_SN = string.IsNullOrEmpty(_in.CARD_SN) ? "" : _in.CARD_SN.Trim();
                string CERTNO = string.IsNullOrEmpty(_in.CERTNO) ? "" : _in.CERTNO.Trim();
                string PSN_CERT_TYPE = string.IsNullOrEmpty(_in.PSN_CERT_TYPE) ? "" : _in.PSN_CERT_TYPE.Trim();
                string PSN_NAME = string.IsNullOrEmpty(_in.PSN_NAME) ? "" : _in.PSN_NAME.Trim();
                string INVOICE_NO = string.IsNullOrEmpty(_in.INVOICE_NO) ? "" : _in.INVOICE_NO.Trim();

                string DEAL_TYPE_NAME = "FREE";
                switch (DEAL_TYPE)
                {
                    case "L":
                        DEAL_TYPE_NAME = "GH";
                        break;
                    case "1":
                        DEAL_TYPE_NAME = "WX";
                        break;
                    case "2":
                        DEAL_TYPE_NAME = "ZFB";
                        break;
                }



                HISModels.A303.siInfo siInfo = new HISModels.A303.siInfo()
                {
                    mdtrt_cert_type = MDTRT_CERT_TYPE,
                    mdtrt_cert_no = MDTRT_CERT_NO,
                    begntime = BEGNTIME,
                    card_sn = CARD_SN,
                    certno = CERTNO,
                    psn_cert_type = PSN_CERT_TYPE,
                    psn_name = PSN_NAME,
                    invoiceNo = INVOICE_NO,
                    totCost = JE_ALL.ToString(),
                    finType = "2"
                };
                HISModels.A303.A303Request a303Request = new HISModels.A303.A303Request()
                {
                    amonCost = JE_ALL.ToString(),
                    bankTradeNo = string.IsNullOrEmpty(QUERYID)?"0": QUERYID,
                    clinicNo = OPT_SN,
                    hospCode = _hospCode,
                    invoiceNo =INVOICE_NO,
                    openTradeNo = string.IsNullOrEmpty(QUERYID) ? "0" : QUERYID,
                    operCode = _operCode,
                    operName = _operName,
                    operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    payTime = DEAL_TIME.ToString("yyyy-MM-dd HH:mm:ss"),
                    payTypeId = DEAL_TYPE_NAME,
                    selfPay = YLCARD_TYPE == "2" ? "0" : "1",
                    siInfo = siInfo
                };
                a303Request.recipeNos = new List<string>();
                foreach (string recipeNo in PRE_NO.Split('|'))
                {
                    a303Request.recipeNos.Add(recipeNo);
                }

                string inputjson = JSONSerializer.Serialize(a303Request);

                //string YB_MXINPUT = dic_filter.ContainsKey("YB_MXINPUT") ? dic_filter["YB_MXINPUT"] : "";
                //string YB_MXOUT = dic_filter.ContainsKey("YB_MXOUT") ? dic_filter["YB_MXOUT"] : "";
                //string inxml = doc.InnerXml;
                string his_rtnjson = "";

                if (!PubFunc.CallHISService(HOS_ID, inputjson, "A303", ref his_rtnjson))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnjson;
                    goto EndPoint;
                }
                _out.HIS_RTNXML = "";
                try
                {

                    HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);


                    if (baseRsponse.code != "200")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = baseRsponse.message;
                        goto EndPoint;
                    }
                    JObject Jkeys = baseRsponse.GetInput<JObject>();
                
                    _out.HOS_PAY_SN = Jkeys["invoiceNo"].ToString();
                    _out.HOS_REG_SN = HOS_SN;
                    _out.RCPT_NO = Jkeys["invoiceNo"].ToString();
                    _out.OPT_SN = HOSPATID;
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);


                    #region 平台数据保存
                    try
                    {
                        var db = new DbMySQLZZJ().Client;


                        SqlSugarModel.OptPayLock modelPayLock = db.Queryable<SqlSugarModel.OptPayLock>().Where(t => t.PAY_ID == _in.PAY_ID).Single();
                        SqlSugarModel.OptPay Modelopt_pay = new SqlSugarModel.OptPay();


                        Modelopt_pay.PAY_ID = _in.PAY_ID;
                        Modelopt_pay.HOS_ID = _in.HOS_ID;
                        Modelopt_pay.PAT_ID = modelPayLock.PAT_ID;
                        Modelopt_pay.PAT_NAME = modelPayLock.PAT_NAME;
                        Modelopt_pay.SFZ_NO = modelPayLock.SFZ_NO;
                        Modelopt_pay.YLCARD_TYPE = FormatHelper.GetInt(modelPayLock.YLCARD_TYPE);
                        Modelopt_pay.YLCARD_NO = modelPayLock.YLCARD_NO;
                        Modelopt_pay.HOSPATID = _in.HOSPATID;

                        Modelopt_pay.DEPT_CODE = modelPayLock.DEPT_CODE;
                        Modelopt_pay.DEPT_NAME = modelPayLock.DEPT_NAME;
                        Modelopt_pay.DOC_NO = modelPayLock.DOC_NO;
                        Modelopt_pay.DOC_NAME = modelPayLock.DOC_NAME;
                        Modelopt_pay.CHARGE_TYPE = "";
                        Modelopt_pay.QUERYID = _in.QUERYID;
                        Modelopt_pay.DEAL_TYPE = _in.DEAL_TYPE;
                        Modelopt_pay.SUM_JE = modelPayLock.SUM_JE;
                        Modelopt_pay.CASH_JE = FormatHelper.GetDecimal(_in.CASH_JE);
                        Modelopt_pay.ACCT_JE = 0;
                        Modelopt_pay.FUND_JE = Modelopt_pay.SUM_JE - Modelopt_pay.CASH_JE;
                        Modelopt_pay.OTHER_JE = 0;
                        Modelopt_pay.HOS_REG_SN = _out.HOS_PAY_SN;
                        Modelopt_pay.HOS_SN = _in.HOS_SN;
                        Modelopt_pay.OPT_SN = _out.OPT_SN;
                        Modelopt_pay.PRE_NO = _in.PRE_NO;
                        Modelopt_pay.RCPT_NO = _out.RCPT_NO;
                        Modelopt_pay.HOS_PAY_SN = _out.HOS_PAY_SN;
                        Modelopt_pay.DJ_DATE = DateTime.Now.Date;
                        Modelopt_pay.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");

                        Modelopt_pay.USER_ID = _in.USER_ID;
                        Modelopt_pay.lTERMINAL_SN = _in.LTERMINAL_SN;
                        Modelopt_pay.SOURCE = "ZZJ";

                        try
                        {
                            db.BeginTran();
                            List<SqlSugarModel.OptPayMx> Modelopt_pay_mxs = db.Queryable<SqlSugarModel.OptPayMx>().AS("opt_pay_mx_lock").Where(t => t.PAY_ID == _in.PAY_ID).ToList();

                            db.Insertable(Modelopt_pay).ExecuteCommand();
                            db.Insertable<SqlSugarModel.OptPayMx>(Modelopt_pay_mxs).ExecuteCommand();
                            db.CommitTran();

                        }
                        catch (Exception ex)
                        {
                            db.RollbackTran();

                            SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                            sqlerror.TYPE = "OUTFEEPAYSAVE";
                            sqlerror.Exception = ex.Message;
                            sqlerror.DateTime = DateTime.Now;
                            LogHelper.SaveSqlerror(sqlerror);
                        }
                    }
                    catch (Exception ex)
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "OUTFEEPAYSAVE";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "数据解析异常，请至窗口检查是否缴费成功";
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
