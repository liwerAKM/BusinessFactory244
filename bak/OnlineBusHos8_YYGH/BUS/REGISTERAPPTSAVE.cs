using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos8_YYGH.Model;
using Soft.Core;
using System.Xml;
using System.Data;
using Log.Core.Model;

namespace OnlineBusHos8_YYGH.BUS
{
    class REGISTERAPPTSAVE
    {
        public static string B_REGISTERAPPTSAVE(string json_in)
        {
            if (GlobalVar.DoBussiness == "0")
            {
                return UnDoBusiness(json_in);
            }
            else
            {
                return DoBusiness(json_in);
            }
        }

        public static string DoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN _in = JSONSerializer.Deserialize<REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN>(json_in);
                REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT _out = new REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("REGISTERAPPTSAVE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "SOURCE", "ZZJ");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
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
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim());
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
                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
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
                    _out.JEALL = dtrev.Columns.Contains("JE_ALL") ? dtrev.Rows[0]["JE_ALL"].ToString() : "";

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                    #region 业务数据保存
                    int reg_id = 0;//预约标识
                    if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase("REGISTER_APPT", out reg_id))
                    {

                        goto EndPoint;
                    }
                    DataTable dtpat_info = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "SFZ_NO='" + _in.SFZ_NO + "'", "MOBILE_NO", "PAT_ID");
                    int pat_id = int.Parse(dtpat_info.Rows[0]["pat_id"].ToString().Trim());
                    Plat.Model.register_appt modelregister_appt = new Plat.Model.register_appt();
                    modelregister_appt.REG_ID = reg_id;
                    modelregister_appt.HOS_ID = _in.HOS_ID;
                    modelregister_appt.PAT_ID = pat_id;
                    modelregister_appt.SCH_DATE = _in.SCH_DATE;
                    modelregister_appt.SCH_TIME = _in.SCH_TIME;
                    modelregister_appt.DEPT_CODE = _in.DEPT_CODE;
                    modelregister_appt.DOC_NO = _in.DOC_NO;
                    modelregister_appt.USER_ID =_in.USER_ID;
                    modelregister_appt.TIME_BUCKET = "";
                    modelregister_appt.TIME_POINT = "";
                    modelregister_appt.SFZ_NO = _in.SFZ_NO;
                    modelregister_appt.PAT_NAME = _in.PAT_NAME;
                    modelregister_appt.BIRTHDAY = _in.BIRTHDAY;
                    modelregister_appt.SEX = _in.SEX;
                    modelregister_appt.YLCARD_NO = _in.YLCARD_NO;
                    modelregister_appt.YLCARD_TYPE = FormatHelper.GetInt(_in.YLCARD_TYPE);
                    DataTable dtdept = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("dept_info", "dept_code='" +_in.DEPT_CODE + "' and HOS_ID='" + _in.HOS_ID + "'", "DEPT_NAME");
                    string DOC_NAME = "";
                    if (_in.SCH_TYPE == "2")
                    {
                        DOC_NAME = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("doc_info", "DOC_NO='" + _in.DOC_NO + "' and HOS_ID='" + _in.HOS_ID + "'", "DOC_NAME").Rows[0]["DOC_NAME"].ToString().Trim();
                    }
                    string DEPT_NAME = dtdept.Rows[0]["DEPT_NAME"].ToString().Trim();
                    modelregister_appt.DEPT_NAME = DEPT_NAME;
                    modelregister_appt.DOC_NAME = DOC_NAME;
                    modelregister_appt.DIS_NAME = "";//待定
                    modelregister_appt.GH_TYPE = _in.SCH_TYPE;//挂号类别暂定为排版类型
                    modelregister_appt.HOS_SN = _out.HOS_SN;
                    modelregister_appt.APPT_SN = _out.HOS_SN;//待定
                    modelregister_appt.HOS_FH_TYPE = "";//待定
                    DataTable dsSchdule = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("schedule", "HOS_ID='" + _in.HOS_ID + "' and SCH_DATE='" + _in.SCH_DATE + "' and SCH_TIME='" + _in.SCH_TIME + "' and DEPT_CODE='" + _in.DEPT_CODE + "' and DOC_NO='" + _in.DOC_NO + "'", "GH_FEE", "ZL_FEE");
                    decimal GH_FEE = 0, ZL_FEE = 0;
                    if (dsSchdule.Rows.Count > 0)
                    {
                        GH_FEE = Convert.ToDecimal(dsSchdule.Rows[0]["GH_FEE"]);
                        ZL_FEE = Convert.ToDecimal(dsSchdule.Rows[0]["ZL_FEE"]);
                    }
                    modelregister_appt.ZL_FEE = ZL_FEE;
                    modelregister_appt.GH_FEE = GH_FEE;
                    modelregister_appt.APPT_TYPE = "0";//预约标记
                    modelregister_appt.APPT_ORDER = _out.APPT_ORDER;//待定
                    modelregister_appt.APPT_PAY =FormatHelper.GetDecimal(_out.APPT_PAY);
                    modelregister_appt.IS_FZ = 0;//待定
                    modelregister_appt.PAY_STATUS = "0";
                    modelregister_appt.APPT_TATE = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    modelregister_appt.APPT_TIME = DateTime.Now.ToString("HH:mm:ss");
                    //modelregister_appt.PAY_EXPIRATION =;//待定
                    //modelregister_appt.APPT_EXPIRATION = DateTime.Now;//待定
                    modelregister_appt.APPT_WAY = "";//待定 预约途径
                    modelregister_appt.lTERMINAL_SN =_in.LTERMINAL_SN;
                    modelregister_appt.PERIOD_START = _in.PERIOD_START;
                    modelregister_appt.HOS_SN_HIS = _out.HOS_SN;
                    #endregion
                    if (!new Plat.MySQLDAL.register_appt().AddByTran_ZZJ(modelregister_appt))
                    {
                        ModLogQHZZJ logzzj = new ModLogQHZZJ();
                        logzzj.BUS = "";
                        logzzj.BUS_NAME = "ZZJ_YYGH";
                        logzzj.SUB_BUSNAME = "REGISTERAPPTSAVE";
                        logzzj.InTime = DateTime.Now;
                        logzzj.InData = "预约挂号平台保存失败";
                        logzzj.OutTime = DateTime.Now;
                        logzzj.OutData = _out.HOS_SN;
                        new Log.Core.MySQLDAL.DalLogQHZZJ().Add(logzzj);
                    }
                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
        public static string UnDoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN _in = JSONSerializer.Deserialize<REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN>(json_in);
                REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT _out = new REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT();
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
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim());
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
                if (GlobalVar.DoBussiness == "0")
                {
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml;
                        goto EndPoint;
                    }
                }
                else if (GlobalVar.DoBussiness == "1")
                {
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml;
                        goto EndPoint;
                    }
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

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}
