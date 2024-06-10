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

namespace OnlineBusHos_YYGH.BUS
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
                XmlDocument doc = QHXmlMode.GetBaseXml("REGISTERPAYSAVE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", string.IsNullOrEmpty(_in.MOBILE_NO) ? "" : _in.MOBILE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", string.IsNullOrEmpty(_in.SEX) ? "" : _in.SEX.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIRTHDAY", string.IsNullOrEmpty(_in.BIRTHDAY) ? "" : _in.BIRTHDAY.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "ADDRESS", string.IsNullOrEmpty(_in.ADDRESS) ? "" : _in.ADDRESS.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_NAME", string.IsNullOrEmpty(_in.GUARDIAN_NAME) ? "" : _in.GUARDIAN_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_SFZ_NO", string.IsNullOrEmpty(_in.GUARDIAN_SFZ_NO) ? "" : _in.GUARDIAN_SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TIME", string.IsNullOrEmpty(_in.SCH_TIME) ? "" : _in.SCH_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", string.IsNullOrEmpty(_in.SCH_TYPE) ? "" : _in.SCH_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PERIOD_START", string.IsNullOrEmpty(_in.PERIOD_START) ? "" : _in.PERIOD_START.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PERIOD_END", string.IsNullOrEmpty(_in.PERIOD_END) ? "" : _in.PERIOD_END.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRO_TITLE", string.IsNullOrEmpty(_in.PRO_TITLE) ? "" : _in.PRO_TITLE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "WAIT_ID", string.IsNullOrEmpty(_in.WAIT_ID) ? "" : _in.WAIT_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REGISTER_TYPE", string.IsNullOrEmpty(_in.REGISTER_TYPE) ? "" : _in.REGISTER_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JE_ALL", string.IsNullOrEmpty(_in.JEALL) ? "" : _in.JEALL.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TIME", string.IsNullOrEmpty(_in.DEAL_TIME) ? "" : _in.DEAL_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_STATES", string.IsNullOrEmpty(_in.DEAL_STATES) ? "" : _in.DEAL_STATES.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());

                #region 医保
                if (_in.YLCARD_TYPE == "2")
                {
                    /*
                    string tran_id = _in.HOS_ID + "_01_" + _in.HOS_SN;
                    DataTable dtybjslog = DbHelperMySQLInsur.Query("select * from chs_tran where tran_id='" + tran_id + "'").Tables[0];
                    if (dtybjslog.Rows.Count == 0)
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = "未取到医保结算信息!";
                        goto EndPoint;
                    }
                    DataTable dtywzqh = DbHelperMySQLInsur.Query("select * from hos_opter where opter_no='" + _in.USER_ID + "'").Tables[0];

                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YYGHLX", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBYWZQH", FormatHelper.GetStr(dtybjslog.Rows[0]["insuplc_admdvs"])); 
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MZNO",FormatHelper.GetStr(dtybjslog.Rows[0]["mdtrt_id"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBDJH", FormatHelper.GetStr(dtybjslog.Rows[0]["setl_id"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JS_OUT", FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2207"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YJS_IN", FormatHelper.GetStr(dtybjslog.Rows[0]["chsInput2206"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YJS_OUT", FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2201"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_IN", FormatHelper.GetStr(dtybjslog.Rows[0]["chsInput2207"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JS_OUT_ADD", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput1101"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_TYPE", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DIS_NO", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBYJS_OUT", FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2204"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DJ_IN", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DJ_OUT", FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2206"]));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QHSOFT", "1");
                    */
                }
                #endregion

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
                    _out.HOS_SN = dtrev.Columns.Contains("HOS_SN") ? dtrev.Rows[0]["HOS_SN"].ToString() : "";
                    _out.APPT_PAY = dtrev.Columns.Contains("APPT_PAY") ? dtrev.Rows[0]["APPT_PAY"].ToString() : "";
                    _out.APPT_ORDER = dtrev.Columns.Contains("APPT_ORDER") ? dtrev.Rows[0]["APPT_ORDER"].ToString() : "";
                    _out.APPT_TIME = dtrev.Columns.Contains("REALTIMES") ? dtrev.Rows[0]["REALTIMES"].ToString() : "";
                    _out.APPT_PLACE = dtrev.Columns.Contains("ZS_NAME") ? dtrev.Rows[0]["ZS_NAME"].ToString() : "";
                    _out.OPT_SN = dtrev.Columns.Contains("OPT_SN") ? dtrev.Rows[0]["OPT_SN"].ToString() : "";
                    _out.RCPT_NO = dtrev.Columns.Contains("RCPT_NO") ? dtrev.Rows[0]["RCPT_NO"].ToString() : "";
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
