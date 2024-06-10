using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using Log.Core.Model;
using DB.Core;

namespace OnlineBusHos172_OutHos.BUS
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
            {

                Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN _in = JSONSerializer.Deserialize<Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN>(json_in);
                Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT _out = new Model.OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT();
                Dictionary<string, string> dic_filter = PubFunc.Get_Filter(FormatHelper.GetStr(_in.FILTER));
                XmlDocument doc = QHXmlMode.GetBaseXml("OUTFEEPAYSAVE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "SOURCE", "ZZJ");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPT_SN", string.IsNullOrEmpty(_in.OPT_SN) ? "" : _in.OPT_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRE_NO", string.IsNullOrEmpty(_in.PRE_NO) ? "" : _in.PRE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAY_TYPE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JEALL", string.IsNullOrEmpty(_in.JEALL) ? "" : _in.JEALL.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_STATES", string.IsNullOrEmpty(_in.DEAL_STATES) ? "" : _in.DEAL_STATES.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TIME", string.IsNullOrEmpty(_in.DEAL_TIME) ? "" : _in.DEAL_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "", string.IsNullOrEmpty(_in.MB_ID) ? "" : _in.MB_ID.Trim());

                string insuplc_admdvs = "";
                string mdtrt_id = "";
                string setl_id = "";
                string chsOutput1101 = "";
                string chsInput2201 = "";
                string chsOutput2201 = "";
                string chsInput2203 = "";
                string chsInput2204 = "";
                string chsOutput2204 = "";
                string chsInput2206 = "";
                string chsOutput2206 = "";
                string chsInput2207 = "";
                string chsOutput2207 = "";
                string chsOutput5360 = "";
                #region 医保
                if (_in.YLCARD_TYPE == "2")
                {

                    string tran_id = _in.HOS_ID + "_02_" + _in.HOS_SN;
                    DataTable dtybjslog = DbHelperMySQLInsur.Query("select * from chs_tran where tran_id='" + tran_id + "'").Tables[0];
                    if (dtybjslog.Rows.Count == 0)
                    {
                        dataReturn.Code = 222;//未调用his 直接冲正
                        dataReturn.Msg = "未取到医保结算信息!";
                        goto EndPoint;
                    }

                    insuplc_admdvs = FormatHelper.GetStr(dtybjslog.Rows[0]["insuplc_admdvs"]);
                    mdtrt_id = FormatHelper.GetStr(dtybjslog.Rows[0]["mdtrt_id"]);
                    setl_id = FormatHelper.GetStr(dtybjslog.Rows[0]["setl_id"]);
                    chsOutput1101 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput1101"]);
                    chsInput2201 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsInput2201"]);
                    chsOutput2201 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2201"]);
                    chsInput2203 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsInput2203"]);
                    chsInput2204 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsInput2204"]);
                    chsOutput2204 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2204"]);
                    chsInput2206 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsInput2206"]);
                    chsOutput2206 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2206"]);
                    chsInput2207 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsInput2207"]);
                    chsOutput2207 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput2207"]);
                    chsOutput5360 = FormatHelper.GetStr(dtybjslog.Rows[0]["chsOutput5360"]);

                }
                #endregion


                //新增字段
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PATIENTTYPE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OUTPUTTC", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBDJH", setl_id);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JS_OUT", chsOutput2207);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YJS_IN", chsInput2206);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YJS_OUT", chsOutput2206);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_IN", chsInput2207);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JS_OUT_ADD", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", chsOutput1101);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DIS_NO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_TYPE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBYWZQH", insuplc_admdvs);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YYGHLX", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MZNO", mdtrt_id);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NB_MZNO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NBDJH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NB_JS_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NB_PAT_CARD_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NB_YJS_IN", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NB_YJS_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NBYWZQH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NB_IN", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NB_JS_OUT_ADD", "");
                #region 新增 国家医保新增字段
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsOutput1101", chsOutput1101);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsInput2201", chsInput2201);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsOutput2201", chsOutput2201);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsInput2203", chsInput2203);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsOutput2203", "");
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsInput2204", chsInput2204);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsOutput2204", chsOutput2204);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsInput2206", chsInput2206);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsOutput2206", chsOutput2206);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsInput2207", chsInput2207);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsOutput2207", chsOutput2207);
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "chsOutput5360", chsOutput5360);
                #endregion

                //string YB_MXINPUT = dic_filter.ContainsKey("YB_MXINPUT") ? dic_filter["YB_MXINPUT"] : "";
                //string YB_MXOUT = dic_filter.ContainsKey("YB_MXOUT") ? dic_filter["YB_MXOUT"] : "";
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
                    if (!dtrev.Columns.Contains("CLBZ"))//his出参不是标准格式则不充正，也不存数据
                    {
                        dataReturn.Code = 6;
                        dataReturn.Msg = "his出参非标准格式，不冲正";
                        goto EndPoint;
                    }
                    else if (dtrev.Rows[0]["CLBZ"].ToString() != "0")//his明确标识没有保存成功则直接冲正
                    {
                        dataReturn.Code = 222;
                        dataReturn.Msg = "his出参返回失败";
                        goto EndPoint;
                    }

                    
                    _out.HOS_PAY_SN = dtrev.Columns.Contains("HOS_PAY_SN") ? dtrev.Rows[0]["HOS_PAY_SN"].ToString() : "";
                    _out.HOS_REG_SN = dtrev.Columns.Contains("HOS_REG_SN") ? dtrev.Rows[0]["HOS_REG_SN"].ToString() : "";
                    _out.RCPT_NO = dtrev.Columns.Contains("RCPT_NO") ? dtrev.Rows[0]["RCPT_NO"].ToString() : "";
                    _out.OPT_SN = dtrev.Columns.Contains("OPT_SN") ? dtrev.Rows[0]["OPT_SN"].ToString() : "";
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
