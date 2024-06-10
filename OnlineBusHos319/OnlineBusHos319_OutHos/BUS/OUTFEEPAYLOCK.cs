using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using OnlineBusHos319_OutHos.Model;
using Google.Protobuf.WellKnownTypes;
using System.Reflection.Emit;
using Newtonsoft.Json.Linq;

namespace OnlineBusHos319_OutHos.BUS
{
    class OUTFEEPAYLOCK
    {
        public static string B_OUTFEEPAYLOCK(string json_in)
        {
            return Business(json_in);

        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN _in = JSONSerializer.Deserialize<Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN>(json_in);
                Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT _out = new Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT();
                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string HOSPATID = _in.HOSPATID;
                string MDTRT_CERT_TYPE = string.IsNullOrEmpty(_in.MDTRT_CERT_TYPE) ? "" : _in.MDTRT_CERT_TYPE.Trim();
                string MDTRT_CERT_NO = string.IsNullOrEmpty(_in.MDTRT_CERT_NO) ? "" : _in.MDTRT_CERT_NO.Trim();
                string CARD_SN = string.IsNullOrEmpty(_in.CARD_SN) ? "" : _in.CARD_SN.Trim();
                string BEGNTIME = string.IsNullOrEmpty(_in.BEGNTIME) ? "" : _in.BEGNTIME.Trim();
                string PSN_CERT_TYPE = string.IsNullOrEmpty(_in.PSN_CERT_TYPE) ? "" : _in.PSN_CERT_TYPE.Trim();
                string CERTNO = string.IsNullOrEmpty(_in.CERTNO) ? "" : _in.CERTNO.Trim();
                string PSN_NAME = string.IsNullOrEmpty(_in.PSN_NAME) ? "" : _in.PSN_NAME.Trim();
                string YLCARD_TYPE = string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim();

                DataTable dtpre = new DataTable();
                dtpre.Columns.Add("HOS_ID", typeof(string));
                dtpre.Columns.Add("lTERMINAL_SN", typeof(string));
                dtpre.Columns.Add("USER_ID", typeof(string));
                dtpre.Columns.Add("OPT_SN", typeof(string));
                dtpre.Columns.Add("PRE_NO", typeof(string));
                dtpre.Columns.Add("HOS_SN", typeof(string));
                dtpre.Columns.Add("SFZ_NO", typeof(string));
                dtpre.Columns.Add("MB_ID", typeof(string));

                DataTable dtMed = new DataTable();
                DataTable dtChkt = new DataTable();
                #region 药品定义
                dtMed.Columns.Add("PRENO", typeof(string));
                dtMed.Columns.Add("DATIME", typeof(string));
                dtMed.Columns.Add("DAID", typeof(string));
                dtMed.Columns.Add("MED_ID", typeof(string));
                dtMed.Columns.Add("MED_NAME", typeof(string));
                dtMed.Columns.Add("MED_GG", typeof(string));
                dtMed.Columns.Add("GROUPID", typeof(string));
                dtMed.Columns.Add("USAGE", typeof(string));
                dtMed.Columns.Add("AUT_NAME", typeof(string));
                dtMed.Columns.Add("CAMT", typeof(string));
                dtMed.Columns.Add("AUT_NAMEALL", typeof(string));
                dtMed.Columns.Add("CAMTALL", typeof(string));
                dtMed.Columns.Add("TIMES", typeof(string));
                dtMed.Columns.Add("PRICE", typeof(string));
                dtMed.Columns.Add("AMOUNT", typeof(string));
                dtMed.Columns.Add("YB_CODE", typeof(string));
                dtMed.Columns.Add("MINAUT_FLAG", typeof(string));
                #endregion
                #region 项目定义
                dtChkt.Columns.Add("DATIME", typeof(string));
                dtChkt.Columns.Add("DAID", typeof(string));
                dtChkt.Columns.Add("CHKIT_ID", typeof(string));
                dtChkt.Columns.Add("CHKIT_NAME", typeof(string));
                dtChkt.Columns.Add("AUT_NAME", typeof(string));
                dtChkt.Columns.Add("CAMTALL", typeof(string));
                dtChkt.Columns.Add("PRICE", typeof(string));
                dtChkt.Columns.Add("AMOUNT", typeof(string));
                dtChkt.Columns.Add("YB_CODE", typeof(string));
                dtChkt.Columns.Add("MINAUT_FLAG", typeof(string));
                #endregion

                DataTable dtPre = new DataTable();
                dtPre.Columns.Add("HOS_ID", typeof(string));
                dtPre.Columns.Add("OPT_SN", typeof(string));
                dtPre.Columns.Add("PRE_NO", typeof(string));
                dtPre.Columns.Add("HOS_SN", typeof(string));
                dtPre.Columns.Add("JEALL", typeof(string));
                dtPre.Columns.Add("CASH_JE", typeof(string));
                dtPre.Columns.Add("DJ_DATE", typeof(string));
                dtPre.Columns.Add("DJ_TIME", typeof(string));
                foreach (Model.OUTFEEPAYLOCK_M.PRE pre in _in.PRELIST)
                {

                    HISModels.BasesiInfo basesiInfo = new HISModels.BasesiInfo()
                    {
                        mdtrt_cert_type = MDTRT_CERT_TYPE,
                        mdtrt_cert_no = MDTRT_CERT_NO,
                        card_sn = CARD_SN,
                        begntime = BEGNTIME,
                        psn_cert_type = PSN_CERT_TYPE,
                        certno = CERTNO,
                        psn_name = PSN_NAME
                    };

                    HISModels.A302.A302Request a302Request = new HISModels.A302.A302Request()
                    {
                        clinicNo = pre.OPT_SN,
                        operCode = _operCode,
                        operName = _operName,
                        hospCode = _hospCode,
                        operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        selfPay = YLCARD_TYPE == "2" ? "0" : "1",
                        siInfo = basesiInfo
                    };

                    a302Request.recipeNos = new List<string>();
                    foreach (string recipeNo in pre.PRE_NO.Split('|'))
                    {
                        a302Request.recipeNos.Add(recipeNo);
                    }

                    string inputjson = JSONSerializer.Serialize(a302Request);
                    //System.IO.StreamReader stream = new System.IO.StreamReader("D:/test.txt");
                    //string his_rtnxml = stream.ReadToEnd();
                    string his_rtnjson = "";
                    if (!PubFunc.CallHISService(HOS_ID, inputjson, "A302", ref his_rtnjson))
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

                        HISModels.A302.A302Response a302Response = baseRsponse.GetInput<HISModels.A302.A302Response>();


                        foreach (HISModels.A302.recipeDetails recipeDetails in a302Response.recipeDetail)
                        {
                            if (recipeDetails.drugFlag == "1")//药品
                            {
                                DataRow drmed = dtMed.NewRow();
                                drmed["PRENO"] = pre.PRE_NO;
                                drmed["DATIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                drmed["DAID"] = recipeDetails.recipeSeq;
                                drmed["MED_ID"] = recipeDetails.itemCode;
                                drmed["MED_NAME"] = recipeDetails.itemName;
                                drmed["MED_GG"] = recipeDetails.specs;
                                drmed["GROUPID"] = "0";
                                drmed["USAGE"] = recipeDetails.usageName;
                                drmed["AUT_NAME"] = recipeDetails.doseUnit;
                                drmed["CAMT"] = recipeDetails.doseOnce;
                                drmed["AUT_NAMEALL"] = recipeDetails.priceUnit;
                                drmed["CAMTALL"] = recipeDetails.qty.ToString();
                                drmed["TIMES"] = recipeDetails.frequencyCode;
                                drmed["PRICE"] = recipeDetails.unitPrice.ToString();
                                drmed["AMOUNT"] = recipeDetails.totCost.ToString();
                                //(decimal.Parse(recipeDetails.qty.ToString()) * decimal.Parse(recipeDetails.unitPrice)).ToString();
                                drmed["YB_CODE"] = "";
                                drmed["MINAUT_FLAG"] = "";
                                dtMed.Rows.Add(drmed);
                            }
                            else
                            {
                                DataRow drchkt = dtChkt.NewRow();
                                drchkt["DATIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                drchkt["DAID"] = recipeDetails.recipeSeq;
                                drchkt["CHKIT_ID"] = recipeDetails.itemCode;
                                drchkt["CHKIT_NAME"] = recipeDetails.itemName;
                                drchkt["AUT_NAME"] = recipeDetails.priceUnit;
                                drchkt["CAMTALL"] = recipeDetails.qty.ToString();
                                drchkt["PRICE"] = recipeDetails.unitPrice.ToString();
                                drchkt["AMOUNT"] = recipeDetails.totCost.ToString();
                                //(decimal.Parse(recipeDetails.qty.ToString()) * decimal.Parse(recipeDetails.unitPrice)).ToString();
                                drchkt["YB_CODE"] = "";
                                drchkt["MINAUT_FLAG"] = "";
                                dtChkt.Rows.Add(drchkt);
                            }
                        }


                        DataRow drdtp = dtPre.NewRow();
                        drdtp["HOS_ID"] = _in.HOS_ID;
                        drdtp["OPT_SN"] = pre.OPT_SN;
                        drdtp["PRE_NO"] = pre.PRE_NO;
                        drdtp["HOS_SN"] = pre.HOS_SN;
                        drdtp["JEALL"] = a302Response.totCost.ToString();
                        drdtp["CASH_JE"] = a302Response.ownCost.ToString();
                        drdtp["DJ_DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
                        drdtp["DJ_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                        dtPre.Rows.Add(drdtp);

                        _out.JEALL = a302Response.totCost.ToString();
                        _out.CASH_JE = a302Response.ownCost.ToString();
                        _out.HOS_SN = pre.HOS_SN;
                        _out.OPT_SN = pre.OPT_SN;
                        _out.PRE_NO = pre.PRE_NO;
                        _out.INVOICE_NO = a302Response.invoiceNo;
                        _out.CHSOUTPUT2206 = "";
                        _out.ecoCost = a302Response.ecoCost;
                        _out.PSN_CASH_PAY = a302Response.ownCost;
                        _out.MEDFEE_SUMAMT = a302Response.totCost.ToString();
                        if (YLCARD_TYPE == "2")
                        {
                            string siOutput = string.IsNullOrEmpty(a302Response.siOutput.ToString()) ? "" : a302Response.siOutput.ToString();
                            JObject Keys = JObject.Parse(siOutput);
                            _out.ACCT_PAY = Keys["setlinfo"]["acct_pay"].ToString();
                            _out.BALC = Keys["setlinfo"]["balc"].ToString();
                            _out.FUND_PAY_SUMAMT = Keys["setlinfo"]["fund_pay_sumamt"].ToString();
                        }
                        else
                        {
                            _out.ACCT_PAY = "0";
                            _out.BALC = "0";
                            _out.FUND_PAY_SUMAMT = a302Response.pubCost;
                        }
                    }
                    catch (Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                        dataReturn.Param = ex.Message;
                    }

                }
                string PAY_ID = "";
                #region 平台数据保存
                try
                {
                    var db = new DbMySQLZZJ().Client;

                    SqlSugarModel.OptPayLock Modelopt_pay_lock = new SqlSugarModel.OptPayLock();

                    int payid = 0;//
                    if (!PubFunc.GetSysID("opt_pay_lock", out payid))
                    {
                        goto EndPoint;
                    }
                    int pat_id = 0;
                    var pat_info = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.SFZ_NO == _in.SFZ_NO).First();
                    if (pat_info != null)
                    {
                        pat_id = pat_info.PAT_ID;
                    }

                    string PRE_NO = "";
                    decimal CASH_JE = 0;
                    decimal JE_ALL = 0;
                    string HOS_SN = "";
                    foreach (DataRow dr in dtPre.Rows)
                    {
                        PRE_NO += dr["PRE_NO"].ToString().Trim() + "|";
                        CASH_JE = decimal.Parse(dr["CASH_JE"].ToString().Trim());
                        JE_ALL = decimal.Parse(dr["JEALL"].ToString().Trim());
                        HOS_SN += dr["HOS_SN"].ToString().Trim() + "|";
                    }
                    PAY_ID = payid.ToString().PadLeft(10, '0');
                    Modelopt_pay_lock.PAY_ID = PAY_ID;
                    Modelopt_pay_lock.HOS_ID = _in.HOS_ID;
                    Modelopt_pay_lock.PAT_ID = pat_id;
                    Modelopt_pay_lock.PAT_NAME = string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim(); //为空存表报错
                    Modelopt_pay_lock.SFZ_NO = _in.SFZ_NO;
                    Modelopt_pay_lock.YLCARD_TYPE = FormatHelper.GetInt(_in.YLCARD_TYPE);
                    Modelopt_pay_lock.YLCARD_NO = _in.YLCARD_NO;
                    Modelopt_pay_lock.HOSPATID = _in.HOSPATID;

                    Modelopt_pay_lock.DEPT_CODE = "";
                    Modelopt_pay_lock.DEPT_NAME = "";
                    Modelopt_pay_lock.DOC_NO = "";
                    Modelopt_pay_lock.DOC_NAME = "";
                    Modelopt_pay_lock.CHARGE_TYPE = "";
                    Modelopt_pay_lock.QUERYID = "";
                    Modelopt_pay_lock.DEAL_TYPE = "";
                    Modelopt_pay_lock.SUM_JE = JE_ALL;
                    Modelopt_pay_lock.CASH_JE = CASH_JE;
                    Modelopt_pay_lock.ACCT_JE = 0;
                    Modelopt_pay_lock.FUND_JE = 0;
                    Modelopt_pay_lock.OTHER_JE = 0;
                    Modelopt_pay_lock.HOS_REG_SN = "";
                    Modelopt_pay_lock.HOS_SN = HOS_SN.TrimEnd('|');
                    Modelopt_pay_lock.OPT_SN = dtPre.Rows[0]["OPT_SN"].ToString().Trim();
                    Modelopt_pay_lock.PRE_NO = PRE_NO.TrimEnd('|');
                    Modelopt_pay_lock.RCPT_NO = "";
                    Modelopt_pay_lock.HOS_PAY_SN = "";
                    Modelopt_pay_lock.DJ_DATE = DateTime.Now.Date;
                    Modelopt_pay_lock.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");

                    Modelopt_pay_lock.USER_ID = _in.USER_ID;
                    Modelopt_pay_lock.lTERMINAL_SN = _in.LTERMINAL_SN;
                    Modelopt_pay_lock.SOURCE = "ZZJ";


                    List<SqlSugarModel.OptPayMxLock> Modelopt_pay_mx_locks = new List<SqlSugarModel.OptPayMxLock>();
                    int item_no = 0;
                    if (dtMed != null && dtMed.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtMed.Rows.Count; i++)
                        {
                            item_no++;
                            SqlSugarModel.OptPayMxLock Modelopt_pay_mx_lock = new SqlSugarModel.OptPayMxLock();

                            Modelopt_pay_mx_lock.PAY_ID = Modelopt_pay_lock.PAY_ID;
                            Modelopt_pay_mx_lock.ITEM_NO = item_no;
                            Modelopt_pay_mx_lock.DAID = dtMed.Rows[i]["DAID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_TYPE = "0";
                            Modelopt_pay_mx_lock.ITEM_CODE = dtMed.Rows[i]["MED_ID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_NAME = dtMed.Rows[i]["MED_NAME"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_SPEC = dtMed.Rows[i]["MED_GG"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_UNITS = dtMed.Rows[i]["AUT_NAMEALL"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_PRICE = FormatHelper.GetDecimal(dtMed.Rows[i]["PRICE"].ToString().Trim());
                            Modelopt_pay_mx_lock.AMOUNT = FormatHelper.GetDecimal(dtMed.Rows[i]["CAMTALL"].ToString().Trim());
                            Modelopt_pay_mx_lock.COSTS = FormatHelper.GetDecimal(dtMed.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.CHARGES = FormatHelper.GetDecimal(dtMed.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.ZFBL = null;

                            Modelopt_pay_mx_locks.Add(Modelopt_pay_mx_lock);
                        }

                    }
                    if (dtChkt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtChkt.Rows.Count; i++)
                        {
                            item_no++;
                            SqlSugarModel.OptPayMxLock Modelopt_pay_mx_lock = new SqlSugarModel.OptPayMxLock();

                            Modelopt_pay_mx_lock.PAY_ID = Modelopt_pay_lock.PAY_ID;
                            Modelopt_pay_mx_lock.ITEM_NO = item_no;
                            Modelopt_pay_mx_lock.DAID = dtChkt.Rows[i]["DAID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_TYPE = "1";
                            Modelopt_pay_mx_lock.ITEM_CODE = dtChkt.Rows[i]["CHKIT_ID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_NAME = dtChkt.Rows[i]["CHKIT_NAME"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_SPEC = "";
                            Modelopt_pay_mx_lock.ITEM_UNITS = dtChkt.Rows[i]["AUT_NAME"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_PRICE = FormatHelper.GetDecimal(dtChkt.Rows[i]["PRICE"].ToString().Trim());
                            Modelopt_pay_mx_lock.AMOUNT = FormatHelper.GetDecimal(dtChkt.Rows[i]["CAMTALL"].ToString().Trim());
                            Modelopt_pay_mx_lock.COSTS = FormatHelper.GetDecimal(dtChkt.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.CHARGES = FormatHelper.GetDecimal(dtChkt.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.ZFBL = null;

                            Modelopt_pay_mx_locks.Add(Modelopt_pay_mx_lock);
                        }
                    }

                    try
                    {
                        db.BeginTran();

                        db.Insertable(Modelopt_pay_lock).ExecuteCommand();
                        db.Insertable(Modelopt_pay_mx_locks).ExecuteCommand();
                        db.CommitTran();

                    }
                    catch (Exception ex)
                    {
                        db.RollbackTran();

                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "OUTFEEPAYLOCK";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }
                }
                catch (Exception ex)
                {
                    SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                    sqlerror.TYPE = "OUTFEEPAYLOCK";
                    sqlerror.Exception = ex.Message;
                    sqlerror.DateTime = DateTime.Now;
                    LogHelper.SaveSqlerror(sqlerror);
                }
                #endregion

                _out.PAY_ID = PAY_ID;
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);
                goto EndPoint;
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:" + ex.Message;
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}
