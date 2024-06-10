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
using OnlineBusHos319_YYGH.Model;
using System.Reflection.Emit;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace OnlineBusHos319_YYGH.BUS
{
    class REGISTERPAYSAVE
    {
        public static string B_REGISTERPAYSAVE(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.REGISTERPAYSAVE_M.REGISTERPAYSAVE_IN _in = JSONSerializer.Deserialize<Model.REGISTERPAYSAVE_M.REGISTERPAYSAVE_IN>(json_in);
                Model.REGISTERPAYSAVE_M.REGISTERPAYSAVE_OUT _out = new Model.REGISTERPAYSAVE_M.REGISTERPAYSAVE_OUT();

                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string HOSPATID = _in.HOSPATID;
                string DEAL_TYPE = string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim();
                string HOS_SN = string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim();
                string SCH_DATE = string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim();
                string DEAL_TIME = _in.DEAL_TIME;
                string JE_ALL = string.IsNullOrEmpty(_in.JEALL) ? "" : _in.JEALL.Trim();
                string CASH_JE = string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim();
                string QUERYID = string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim();
                string YLCARD_TYPE = string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim();
                string PERIOD_START = string.IsNullOrEmpty(_in.PERIOD_START) ? "" : _in.PERIOD_START.Trim();
                string PERIOD_END = string.IsNullOrEmpty(_in.PERIOD_END) ? "" : _in.PERIOD_END.Trim();
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
                    case "1":
                        DEAL_TYPE_NAME = "WX";
                        break;
                    case "2":
                        DEAL_TYPE_NAME = "ZFB";
                        break;
                }
                string inputjson = "";
                string type = "";
                if (SCH_DATE == DateTime.Now.ToString("yyyy-MM-dd"))//当日结算
                {
                    HISModels.A203.siInfo basesiInfo = new HISModels.A203.siInfo()
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
                    HISModels.A203.A203Request a203Request = new HISModels.A203.A203Request()
                    {
                        amonCost = Convert.ToDecimal(JE_ALL),
                        bankTradeNo = QUERYID == "" ? "0" : QUERYID,
                        clinicNo = HOS_SN,
                        hospCode = _hospCode,
                        openTradeNo = QUERYID == "" ? "0" : QUERYID,
                        operCode = _operCode,
                        operName = _operName,
                        operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        payTime = DEAL_TIME.ToString(),
                        payTypeId = DEAL_TYPE_NAME,
                        beginTime = PERIOD_START,
                        endTime = PERIOD_END,
                        selfPay = YLCARD_TYPE == "2" ? "0" : "1",
                        siInfo = basesiInfo
                    };
                    type = "A203";
                     inputjson = JSONSerializer.Serialize(a203Request);
                }
                else
                {
                    HISModels.A208.siInfo basesiInfo = new HISModels.A208.siInfo()
                    {
                        mdtrt_cert_type = MDTRT_CERT_TYPE,
                        mdtrt_cert_no = MDTRT_CERT_NO,
                        card_sn = CARD_SN,
                        begntime = BEGNTIME,
                        certno = CERTNO,
                        psn_cert_type = PSN_CERT_TYPE,
                        psn_name = PSN_NAME,
                        totCost = JE_ALL.ToString(),
                        invoiceNo = INVOICE_NO,
                        finType = "1"
                    };
                    HISModels.A208.A208Request a208Request = new HISModels.A208.A208Request()
                    {
                        amonCost = Convert.ToDecimal(JE_ALL),
                        bankTradeNo = QUERYID == "" ? "0" : QUERYID,
                        clinicNo = HOS_SN,
                        hospCode = _hospCode,
                        openTradeNo = QUERYID == "" ? "0" : QUERYID,
                        operCode = _operCode,
                        operName = _operName,
                        operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        payTime = DEAL_TIME,
                        payTypeId = DEAL_TYPE_NAME,
                        beginTime = "",
                        endTime = "",
                        selfPay = YLCARD_TYPE == "2" ? "0" : "1",
                        siInfo = basesiInfo
                    };

                    type = "A208";
                    inputjson = JSONSerializer.Serialize(a208Request);
                }

                string his_rtnjson = "";

                if (!PubFunc.CallHISService(HOS_ID, inputjson, type, ref his_rtnjson))
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
                    JObject Jkey = baseRsponse.GetInput<JObject>();


                    _out.HOS_SN = Jkey["clinicNo"].ToString();
                    _out.APPT_PAY = CASH_JE.ToString();
                    _out.APPT_ORDER =  "";
                    _out.APPT_TIME = "";
                    _out.APPT_PLACE = Jkey["visitPosition"].ToString(); 
                    _out.OPT_SN = HOSPATID;
                    _out.ZS_NAME = Jkey["visitPosition"].ToString();
                    _out.RCPT_NO = Jkey["invoiceNo"].ToString();
                    #region  平台数据保存
                    try
                    {
                        var db = new DbMySQLZZJ().Client;

                        SqlSugarModel.RegisterPay modelregister_pay = new SqlSugarModel.RegisterPay();
                        SqlSugarModel.RegisterAppt modelregister_appt = db.Queryable<SqlSugarModel.RegisterAppt>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOS_SN == _in.HOS_SN).First();
                        int pay_id = 0;//
                        if (!PubFunc.GetSysID("REGISTER_PAY", out pay_id))
                        {
                            goto EndPoint;
                        }

                        modelregister_pay.PAY_ID = pay_id;
                        modelregister_pay.REG_ID = modelregister_appt.REG_ID;
                        modelregister_pay.HOS_ID = _in.HOS_ID;
                        modelregister_pay.PAT_ID = modelregister_appt.PAT_ID;
                        modelregister_pay.CHARGE_TYPE = "";
                        modelregister_pay.QUERYID = _in.QUERYID;
                        modelregister_pay.DEAL_TYPE = _in.DEAL_TYPE;
                        modelregister_pay.SUM_JE =FormatHelper.GetDecimal(_in.JEALL);
                        modelregister_pay.CASH_JE= FormatHelper.GetDecimal(_in.CASH_JE);
                        modelregister_pay.ACCT_JE = 0;
                        modelregister_pay.FUND_JE = modelregister_pay.SUM_JE - modelregister_pay.CASH_JE;
                        modelregister_pay.OTHER_JE = 0;

                        modelregister_pay.HOS_SN = _out.HOS_SN;
                        modelregister_pay.OPT_SN = _out.OPT_SN;
                        modelregister_pay.PRE_NO = "";
                        modelregister_pay.RCPT_NO = _out.RCPT_NO;

                        modelregister_pay.DJ_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy.MM.dd"));
                        modelregister_pay.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");
                        modelregister_pay.USER_ID =_in.USER_ID;
                        modelregister_pay.SOURCE = "ZZJ";
                        modelregister_pay.lTERMINAL_SN = _in.LTERMINAL_SN;

                        modelregister_pay.IS_TH = false;

                        modelregister_appt.APPT_TYPE = "1";
                        var row1 = db.Updateable(modelregister_appt).ExecuteCommand();
                        var row = db.Insertable(modelregister_pay).ExecuteCommand();
                    }
                    catch (Exception ex)
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "REGISTERPAYSAVE";
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
                    dataReturn.Param = ex.ToString();

                }
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
