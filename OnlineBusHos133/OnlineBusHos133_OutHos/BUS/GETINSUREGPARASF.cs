﻿using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos133_OutHos.BUS
{
    class GETINSUREGPARASF
    {
        public static string B_GETINSUREGPARASF(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETINSUREGPARASF_M.GETINSUREGPARASF_IN _in = JSONSerializer.Deserialize<Model.GETINSUREGPARASF_M.GETINSUREGPARASF_IN>(json_in);
                Model.GETINSUREGPARASF_M.GETINSUREGPARASF_OUT _out = new Model.GETINSUREGPARASF_M.GETINSUREGPARASF_OUT();
                Dictionary<string, string> dic_filter = PubFunc.Get_Filter(FormatHelper.GetStr(_in.FILTER));
                XmlDocument doc = QHXmlMode.GetBaseXml("GETINSUREGPARASF", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "SOURCE", "ZZJ");
                
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPT_SN", string.IsNullOrEmpty(_in.OPT_SN) ? "" : _in.OPT_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRE_NO", string.IsNullOrEmpty(_in.PRE_NO) ? "" : _in.PRE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAY_TYPE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JEALL", string.IsNullOrEmpty(_in.JEALL) ? "" : _in.JEALL.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_STATES", string.IsNullOrEmpty(_in.DEAL_STATES) ? "" : _in.DEAL_STATES.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TIME", string.IsNullOrEmpty(_in.DEAL_TIME) ? "" : _in.DEAL_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBDJH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JZ_CODE", _in.YLCARD_TYPE.Trim() == "4" ? "01" : "41");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JE_ALL", _in.JEALL.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PARAMETERS", string.IsNullOrEmpty(_in.PARAMETERS) ? "" : _in.PARAMETERS.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_CARD_TYPE", string.IsNullOrEmpty(_in.YB_CARD_TYPE) ? "" : _in.YB_CARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YY_LSH", string.IsNullOrEmpty(_in.YY_LSH) ? "" : _in.YY_LSH.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", "qhzzj001");
             
                string inxml = doc.InnerXml;
                string his_rtnxml = "";

                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                _out.HIS_RTNXML = his_rtnxml;
                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
                    DataTable dtrev = ds.Tables[0];

                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }

                    _out.InvoiceNo = dtrev.Columns.Contains("InvoiceNo") ? dtrev.Rows[0]["InvoiceNo"].ToString() : "";
                    _out.InvoiceAmt = dtrev.Columns.Contains("InvoiceAmt") ? dtrev.Rows[0]["InvoiceAmt"].ToString() : "";
                    _out.InsuFlag = dtrev.Columns.Contains("InsuFlag") ? dtrev.Rows[0]["InsuFlag"].ToString() : "";
                    _out.OutInsuInfo = dtrev.Columns.Contains("OutInsuInfo") ? dtrev.Rows[0]["OutInsuInfo"].ToString() : "";
                    _out.HISInsuInfo = dtrev.Columns.Contains("HISInsuInfo") ? dtrev.Rows[0]["HISInsuInfo"].ToString() : "";
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);


                    #region 平台数据保存
 /*                   try
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
                            sqlerror.TYPE = "GETINSUREGPARASF";
                            sqlerror.Exception = ex.Message;
                            sqlerror.DateTime = DateTime.Now;
                            LogHelper.SaveSqlerror(sqlerror);
                        }
                    }
                    catch (Exception ex)
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "GETINSUREGPARASF";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }*/
                    #endregion
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
                dataReturn.Msg = "程序处理异常:" + ex.ToString();
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}