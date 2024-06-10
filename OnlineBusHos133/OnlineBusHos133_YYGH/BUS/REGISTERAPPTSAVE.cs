using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos133_YYGH.BUS
{
    class REGISTERAPPTSAVE
    {
        public static string B_REGISTERAPPTSAVE(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN _in = JSONSerializer.Deserialize<Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN>(json_in);
                Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT _out = new Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("REGISTERAPPTSAVE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
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
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? DateTime.Today.ToString("yyyy-MM-dd") : _in.SCH_DATE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TIME", string.IsNullOrEmpty(_in.SCH_TIME) ? "" : _in.SCH_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", string.IsNullOrEmpty(_in.SCH_TYPE) ? "" : _in.SCH_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PERIOD_START", string.IsNullOrEmpty(_in.PERIOD_START) ? "" : _in.PERIOD_START.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PERIOD_END", string.IsNullOrEmpty(_in.PERIOD_END) ? "" : _in.PERIOD_END.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRO_TITLE", string.IsNullOrEmpty(_in.PRO_TITLE) ? "" : _in.PRO_TITLE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "WAIT_ID", string.IsNullOrEmpty(_in.WAIT_ID) ? "" : _in.WAIT_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REGISTER_TYPE", string.IsNullOrEmpty(_in.REGISTER_TYPE) ? "" : _in.REGISTER_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
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
                    _out.APPT_TIME = dtrev.Columns.Contains("APPT_TIME") ? dtrev.Rows[0]["APPT_TIME"].ToString() : "";
                    _out.APPT_PLACE = dtrev.Columns.Contains("APPT_PLACE") ? dtrev.Rows[0]["APPT_PLACE"].ToString() : "";
                    _out.JEALL = dtrev.Columns.Contains("JE_ALL") ? dtrev.Rows[0]["JE_ALL"].ToString() : _out.APPT_PAY;

                    #region 平台数据保存
                    try
                    {
                        var db = new DbMySQLZZJ().Client;

                        SqlSugarModel.RegisterAppt modelregister_appt = new SqlSugarModel.RegisterAppt();

                        int reg_id = 0;//预约标识
                        if (!PubFunc.GetSysID("REGISTER_APPT", out reg_id))
                        {
                            goto EndPoint;
                        }
                        int pat_id = 0;
                        var pat_info=db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.SFZ_NO == _in.SFZ_NO).First();
                        if (pat_info != null) 
                        {
                            pat_id = pat_info.PAT_ID;
                        }

                        modelregister_appt.REG_ID = reg_id;
                        modelregister_appt.HOS_ID = _in.HOS_ID;
                        modelregister_appt.PAT_ID = pat_id;

                        modelregister_appt.SFZ_NO = _in.SFZ_NO;
                        modelregister_appt.PAT_NAME = _in.PAT_NAME;
                        modelregister_appt.BIRTHDAY = _in.BIRTHDAY;
                        modelregister_appt.SEX = _in.SEX;
                        modelregister_appt.ADDRESS = _in.ADDRESS;
                        modelregister_appt.MOBILE_NO = _in.MOBILE_NO;

                        modelregister_appt.YLCARD_NO = _in.YLCARD_NO;
                        modelregister_appt.YLCARD_TYPE = FormatHelper.GetInt(_in.YLCARD_TYPE);

                        modelregister_appt.DEPT_CODE = _in.DEPT_CODE;
                        modelregister_appt.DOC_NO = _in.DOC_NO;
                        modelregister_appt.DEPT_NAME = "";
                        modelregister_appt.DOC_NAME = "";

                        modelregister_appt.SCH_DATE = _in.SCH_DATE;
                        modelregister_appt.SCH_TIME = _in.SCH_TIME;
                        modelregister_appt.SCH_TYPE = _in.SCH_TYPE;
                        modelregister_appt.PERIOD_START = _in.PERIOD_START;
                        modelregister_appt.PERIOD_END = _in.PERIOD_END;
                        modelregister_appt.TIME_POINT = "";
                        modelregister_appt.REGISTER_TYPE = _in.REGISTER_TYPE;
                        modelregister_appt.IS_FZ = false;//初复诊


                        modelregister_appt.ZL_FEE = FormatHelper.GetDecimal(_out.APPT_PAY);
                        modelregister_appt.GH_FEE = 0;
                        modelregister_appt.APPT_PAY = FormatHelper.GetDecimal(_out.APPT_PAY);

                        modelregister_appt.APPT_DATE = DateTime.Now.Date;
                        modelregister_appt.APPT_TIME = DateTime.Now.ToString("HH:mm:ss");

                        modelregister_appt.HOS_SN = _out.HOS_SN;
                        modelregister_appt.APPT_SN = _out.HOS_SN;//待定
                        modelregister_appt.APPT_ORDER = _out.APPT_ORDER;//待定
                        modelregister_appt.ZS_NAME = _out.APPT_PLACE;//预约标记
                        modelregister_appt.APPT_TYPE = "0";//预约标记
                        modelregister_appt.HOSPATID = _in.HOSPATID;//预约标记

                        modelregister_appt.USER_ID = _in.USER_ID;
                        modelregister_appt.lTERMINAL_SN = _in.LTERMINAL_SN;
                        modelregister_appt.SOURCE = "ZZJ";
                      
                        var row = db.Insertable(modelregister_appt).ExecuteCommand();
                    }
                    catch (Exception ex) 
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "REGISTERAPPTSAVE";
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
