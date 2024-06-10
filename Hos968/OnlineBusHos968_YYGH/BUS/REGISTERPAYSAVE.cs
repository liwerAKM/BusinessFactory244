using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos968_YYGH.Model;
using Soft.Core;
using System.Xml;
using System.Data;
using DB.Core;

namespace OnlineBusHos968_YYGH.BUS
{
    class REGISTERPAYSAVE
    {
        public static string B_REGISTERPAYSAVE(string json_in)
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
                REGISTERPAYSAVE_M.REGISTERPAYSAVE_IN _in = JSONSerializer.Deserialize<REGISTERPAYSAVE_M.REGISTERPAYSAVE_IN>(json_in);
                REGISTERPAYSAVE_M.REGISTERPAYSAVE_OUT _out = new REGISTERPAYSAVE_M.REGISTERPAYSAVE_OUT();
                Dictionary<string, string> dic_filter = GlobalVar.Get_Filter(FormatHelper.GetStr(_in.FILTER));

                XmlDocument doc = QHXmlMode.GetBaseXml("REGISTERPAYSAVE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "SOURCE", "ZZJ");

                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", string.IsNullOrEmpty(_in.MOBILE_NO) ? "" : _in.MOBILE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", string.IsNullOrEmpty(_in.SEX) ? "" : _in.SEX.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIRTHDAY", string.IsNullOrEmpty(_in.BIRTHDAY) ? "" : _in.BIRTHDAY.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "ADDRESS", string.IsNullOrEmpty(_in.ADDRESS) ? "" : _in.ADDRESS.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_NAME", string.IsNullOrEmpty(_in.GUARDIAN_NAME) ? "" : _in.GUARDIAN_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_SFZ_NO", string.IsNullOrEmpty(_in.GUARDIAN_SFZ_NO) ? "" : _in.GUARDIAN_SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
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
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());

                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());

                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JE_ALL", string.IsNullOrEmpty(_in.JEALL) ? "" : _in.JEALL.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_STATES", string.IsNullOrEmpty(_in.DEAL_STATES) ? "" : _in.DEAL_STATES.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TIME", string.IsNullOrEmpty(_in.DEAL_TIME) ? "" : _in.DEAL_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FZH", "");
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                #region 医保用
                DataTable dtzqh = new DataTable();
                if (_in.YLCARD_TYPE == "2")
                {
                    dtzqh = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("ybywzqh", "usr_id='" + _in.USER_ID + "' AND  dj_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'", "yw_zqh");
                }
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBYWZQH", dtzqh.Rows.Count > 0 ? dtzqh.Rows[0]["yw_zqh"].ToString().Trim() : "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JS_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBDJH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MZNO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBYWZQH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", "");
                #endregion
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
                    _out.OPT_SN = dtrev.Columns.Contains("OPT_SN") ? dtrev.Rows[0]["OPT_SN"].ToString() : "";
                    _out.RCPT_NO = dtrev.Columns.Contains("RCPT_NO") ? dtrev.Rows[0]["RCPT_NO"].ToString() : "";

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                    #region 业务数据保存
                    Plat.Model.register_pay modelregister_pay = new Plat.Model.register_pay();
                    int pay_id = 0;//支付表主键
                    if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase_ZZJ("REGISTER_PAY", out pay_id))
                    {
                        string s = DateTime.Now.Ticks.ToString();
                        pay_id = Convert.ToInt32(s.Substring(s.Length - 6));
                    }
                    int reg_id = 0;//预约标识
                    if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase("REGISTER_APPT", out reg_id))
                    {
                        goto EndPoint;
                    }
                    int pat_id = 0;
                    DataTable dtappt = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("register_appt", "HOS_ID='" + _in.HOS_ID + "' and HOS_SN='" + _in.HOS_SN + "'", "REG_ID", "DIS_NAME", "PAT_ID", "DOC_NAME", "SFZ_NO", "GH_TYPE", "DEPT_NAME", "ZL_FEE", "GH_FEE", "HOS_SN_HIS");

                    if (dtappt.Rows.Count == 0)
                    {
                        DataTable dtpat = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "SFZ_NO='" + _in.SFZ_NO + "'", "PAT_ID");
                        if (dtpat != null && dtpat.Rows.Count > 0)
                        {
                            pat_id = int.Parse(dtpat.Rows[0]["PAT_ID"].ToString().Trim());
                        }
                    }
                    modelregister_pay.PAY_ID = pay_id;
                    modelregister_pay.REG_ID = dtappt.Rows.Count == 0 ? reg_id : int.Parse(dtappt.Rows[0]["REG_ID"].ToString().Trim());
                    modelregister_pay.HOS_ID = _in.HOS_ID;
                    modelregister_pay.PAT_ID = dtappt.Rows.Count == 0 ? 0 : int.Parse(dtappt.Rows[0]["PAT_ID"].ToString().Trim());
                    modelregister_pay.USER_ID = _in.USER_ID;
                    modelregister_pay.OPT_SN = _out.OPT_SN;
                    modelregister_pay.HOS_SN = _out.HOS_SN;
                    modelregister_pay.SFZ_NO = _in.SFZ_NO;// dtappt.Rows[0]["SFZ_NO"].ToString().Trim();
                    modelregister_pay.YLCARD_NO = _in.YLCARD_NO;
                    modelregister_pay.YLCARD_TYPE = int.Parse(_in.YLCARD_TYPE);
                    modelregister_pay.PAT_NAME = _in.PAT_NAME;
                    modelregister_pay.BIRTHDAY = _in.BIRTHDAY;
                    modelregister_pay.SEX = _in.SEX;
                    modelregister_pay.DEPT_CODE = _in.DEPT_CODE;
                    modelregister_pay.DOC_NO = _in.DOC_NO;

                    DataTable dtdept = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("dept_info", "dept_code='" + _in.DEPT_CODE + "' and HOS_ID='" + _in.HOS_ID + "'", "DEPT_NAME");
                    string DOC_NAME = "";
                    if (_in.SCH_TYPE == "2")
                    {
                        DOC_NAME = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("doc_info", "DOC_NO='" + _in.DOC_NO + "' and HOS_ID='" + _in.HOS_ID + "'", "DOC_NAME").Rows[0]["DOC_NAME"].ToString().Trim();
                    }
                    string DEPT_NAME = dtdept.Rows[0]["DEPT_NAME"].ToString().Trim();
                    string dept_name = DEPT_NAME;
                    string doc_name = DOC_NAME;
                    modelregister_pay.DEPT_NAME = dept_name;// dtappt.Rows[0]["DEPT_NAME"].ToString().Trim();
                    modelregister_pay.DOC_NAME = doc_name;// dtappt.Rows[0]["DOC_NAME"] == null ? "" : dtappt.Rows[0]["DOC_NAME"].ToString().Trim();
                    modelregister_pay.DIS_NAME = ""; //dtappt.Rows[0]["DIS_NAME"] == null ? "" : dtappt.Rows[0]["DIS_NAME"].ToString().Trim();
                    modelregister_pay.GH_TYPE = _in.SCH_TYPE;
                    modelregister_pay.HOS_GH_TYPE = _in.SCH_TYPE;
                    modelregister_pay.HOS_GH_NAME = "";//待定 院内挂号类别名
                    modelregister_pay.ZL_FEE = dtappt.Rows.Count == 0 ? 0 : decimal.Parse(dtappt.Rows[0]["ZL_FEE"].ToString().Trim());
                    modelregister_pay.GH_FEE = dtappt.Rows.Count == 0 ? 0 : decimal.Parse(dtappt.Rows[0]["ZL_FEE"].ToString().Trim());
                    modelregister_pay.CASH_JE = decimal.Parse(_out.APPT_PAY);//现金支付金额
                    modelregister_pay.JZ_CODE = "01";//待定
                    modelregister_pay.PAY_TYPE = 0;//待定
                    modelregister_pay.YB_SN = "";
                    modelregister_pay.YB_TYPE = "";
                    modelregister_pay.YB_FYLB = "";
                    modelregister_pay.YB_GH_ORDER = "";
                    modelregister_pay.YB_ZLFEE = 0;
                    modelregister_pay.YB_GHFEE = 0;
                    modelregister_pay.XJZF = 0;
                    modelregister_pay.ZHZF = 0;
                    modelregister_pay.ZHYE = 0;
                    modelregister_pay.XZM = "";
                    modelregister_pay.XZMCH = "";
                    modelregister_pay.YBBZM = "";
                    modelregister_pay.YBBZMC = "";
                    modelregister_pay.JE_ALL = decimal.Parse(_in.JEALL);// decimal.Parse(dtappt.Rows[0]["ZL_FEE"].ToString().Trim()) + decimal.Parse(dtappt.Rows[0]["GH_FEE"].ToString().Trim());//总金额
                    modelregister_pay.APPT_ORDER = "";//需要HIS返回
                    modelregister_pay.APPT_SN = "";
                    modelregister_pay.IS_DZ = false;
                    modelregister_pay.DJ_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy.MM.dd"));
                    modelregister_pay.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");
                    modelregister_pay.IS_TH = false;
                    modelregister_pay.SOURCE = "ZZJ";
                    modelregister_pay.lTERMINAL_SN = _in.LTERMINAL_SN;
                    //现金交易记录表
                    Plat.Model.pay_info modelpay_info = new Plat.Model.pay_info();
                    int pinfo_id = 0;
                    if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase_ZZJ("pay_info", out pinfo_id))
                    {
                        string s = DateTime.Now.Ticks.ToString();
                        pinfo_id = Convert.ToInt32(s.Substring(s.Length - 6));
                    }
                    //现金交易记录表
                    modelpay_info.PAT_ID = modelregister_pay.PAT_ID;
                    modelpay_info.PAY_ID = pinfo_id.ToString();
                    modelpay_info.HOS_ID = modelregister_pay.HOS_ID;
                    modelpay_info.USER_ID = modelregister_pay.USER_ID;
                    modelpay_info.BIZ_TYPE = 1;
                    modelpay_info.BIZ_SN = modelregister_pay.PAY_ID.ToString();
                    modelpay_info.CASH_JE = modelregister_pay.CASH_JE;
                    modelpay_info.SFZ_NO = modelregister_pay.SFZ_NO;
                    modelpay_info.DJ_DATE = _in.DEAL_TIME.Substring(0, 10);
                    modelpay_info.DJ_TIME = _in.DEAL_TIME.Substring(11, 8);
                    modelpay_info.DEAL_TYPE = _in.DEAL_TYPE;
                    modelpay_info.DEAL_STATES = _in.DEAL_STATES;
                    modelpay_info.lTERMINAL_SN = _in.LTERMINAL_SN;

                    string ORDERID = _in.QUERYID;
                    string DEAL_TYPE = _in.DEAL_TYPE;

                    //支付宝
                    Plat.Model.pay_info_zfb modelpay_info_zfb = null;
                    if (DEAL_TYPE == "2")
                    {
                        DataTable dtTran = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("alipay_tran", "comm_sn='" + ORDERID + "'", "seller_id");
                        modelpay_info_zfb = new Plat.Model.pay_info_zfb();
                        modelpay_info_zfb.PAY_ID = pinfo_id.ToString();
                        modelpay_info_zfb.SELLER_ID = dtTran.Rows.Count > 0 ? dtTran.Rows[0]["seller_id"].ToString().Trim() : "";
                        modelpay_info_zfb.BIZ_TYPE = 1;
                        modelpay_info_zfb.BIZ_SN = modelregister_pay.PAY_ID.ToString();
                        modelpay_info_zfb.JE = modelregister_pay.CASH_JE;
                        modelpay_info_zfb.DEAL_STATES = _in.DEAL_STATES;
                        modelpay_info_zfb.DEAL_TIME = DateTime.Parse(_in.DEAL_TIME);
                        modelpay_info_zfb.COMM_SN = ORDERID;
                        modelpay_info_zfb.lTERMINAL_SN = _in.LTERMINAL_SN;
                        modelpay_info_zfb.TXN_TYPE = "01";
                    }
                    //银联
                    Plat.Model.pay_info_upcap modelpay_info_upcap = null;
                    if (DEAL_TYPE == "3")
                    {
                        Plat.Model.unionpay_tran tran = new Plat.MySQLDAL.unionpay_tran().GetModel(ORDERID);
                        modelpay_info_upcap = new Plat.Model.pay_info_upcap();
                        modelpay_info_upcap.PAY_ID = pinfo_id.ToString();
                        modelpay_info_upcap.MERID = tran != null ? tran.MERID : "";
                        modelpay_info_upcap.ORDERID = ORDERID;
                        modelpay_info_upcap.QUERYID = ORDERID;
                        modelpay_info_upcap.REFCODE = tran != null ? tran.REFCODE : "";
                        modelpay_info_upcap.SFZ_NO = _in.SFZ_NO;
                        modelpay_info_upcap.TN = "";
                        modelpay_info_upcap.TXN_TYPE = "01";
                        modelpay_info_upcap.DJ_TIME = DateTime.Parse(_in.DEAL_TIME);
                        modelpay_info_upcap.BIZ_TYPE = 1;
                        modelpay_info_upcap.BDj_id = modelregister_pay.PAY_ID.ToString();
                        modelpay_info_upcap.JE = modelregister_pay.CASH_JE;
                        modelpay_info_upcap.TERMCODE = _in.LTERMINAL_SN;
                    }
                    //微信
                    Plat.Model.pay_info_wc wc = null;
                    if (DEAL_TYPE == "1" || DEAL_TYPE == "6")
                    {
                        /*PAY_ID,WECHAT,PAY_TYPE,BIZ_TYPE,BIZ_SN,COMM_SN,JE,COMM_NAME,DEAL_STATES,DEAL_TIME,DEAL_SN,lTERMINAL_SN,TXN_TYPE*/
                        Plat.Model.wechat_tran tran = new Plat.MySQLDAL.wechat_tran().GetModel(ORDERID);

                        wc = new Plat.Model.pay_info_wc();
                        wc.PAY_ID = pinfo_id.ToString();
                        wc.WECHAT = "";
                        wc.PAY_TYPE = "";
                        wc.BIZ_TYPE = 1;
                        wc.BIZ_SN = modelregister_pay.PAY_ID.ToString();
                        wc.COMM_SN = ORDERID;
                        wc.JE = modelregister_pay.CASH_JE;
                        wc.COMM_NAME = tran != null ? tran.appid : "";
                        wc.DEAL_STATES = "2";
                        wc.DEAL_TIME = DateTime.Now;
                        wc.DEAL_SN = tran != null ? tran.transaction_id : "";
                        wc.TNX_TYPE = "01";
                        wc.lTERMINAL_SN = modelregister_pay.lTERMINAL_SN;
                    }
                    //医保
                    #region
                    Plat.Model.pay_info_yb yb = null;
                    if (int.Parse(_in.YLCARD_TYPE) == 2)
                    {
                        yb = new Plat.Model.pay_info_yb();
                        yb.PAY_ID = pinfo_id;
                        yb.DJ_ID = modelregister_pay.HOS_SN;
                        //入参
                        if (dic_filter.ContainsKey("YB_JSINPUT"))
                        {
                            string[] js_input = dic_filter["YB_JSINPUT"].Split('|');
                            yb.TRADELSH = js_input[0].Substring(0, js_input[0].LastIndexOf('^'));
                            yb.YL_TYPE = js_input[2];
                            yb.JS_DATE = js_input[3];
                            yb.CY_DATE = js_input[4];
                            yb.CY_REASON = js_input[5];
                            yb.DIS_CODE = js_input[6];
                            yb.YJSTYPE = js_input[7];
                            yb.ZTYJSTYPE = js_input[8];
                            yb.USR_NAME = js_input[9];
                            yb.FM_DATE = js_input[10];
                            yb.CC = js_input[11];
                            yb.TAIER_AMOUNT = js_input[12];
                            yb.CARDID = js_input[13];
                            yb.ZYYBBH = js_input[14];
                            yb.DEPT_CODE = js_input[15];
                            yb.DOC_NO = js_input[16];
                            yb.IS_GH = js_input[17];
                            yb.ZSRCARDID = js_input[18];
                            yb.SS_ISSUCCESS = js_input[19];
                        }
                        //出参
                        string[] js_out = dic_filter["JS_OUT"].Split('|');
                        yb.MZLSH = js_out[0].Split('^')[0];
                        yb.DJLSH = js_out[0].Split('^')[1];
                        yb.BC_ZJE = js_out[0].Split('^')[2];
                        yb.BC_TCJE = js_out[1];
                        yb.BC_DBJZ = js_out[2];
                        yb.BC_DBBX = js_out[3];
                        yb.BC_MZBZ = js_out[4];
                        yb.BC_ZHZC = js_out[5];
                        yb.BC_XJZC = js_out[6];
                        yb.BC_ZHZCZF = js_out[7];
                        yb.BC_ZHZCZL = js_out[8];
                        yb.BC_XJZCZF = js_out[9];
                        yb.BC_XJZCZL = js_out[10];
                        yb.YBFWNJE = js_out[11];
                        yb.ZHYE = js_out[12];
                        yb.DBZ_CODE = js_out[13];
                        yb.INSTRUCTION = js_out[14];
                        yb.MED_JE = js_out[15];
                        yb.CHK_JE = js_out[16];
                        yb.BB_JE = js_out[17];
                        yb.BY6 = js_out[18];
                        yb.lTERMINAL_SN = modelregister_pay.lTERMINAL_SN;

                        yb.DEAL_TIME = DateTime.Now;
                        yb.YWZQH = dtzqh.Rows.Count > 0 ? dtzqh.Rows[0]["yw_zqh"].ToString().Trim() : "";
                    }
                    #endregion
                    //保存数据到后台
                    if (!new Plat.MySQLDAL.register_pay().AddByTran_ZZJ(modelregister_pay, modelpay_info, modelpay_info_zfb, wc, null, modelpay_info_upcap, null, yb))
                    {
                    }
                    #endregion

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
                REGISTERPAYSAVE_M.REGISTERPAYSAVE_IN _in = JSONSerializer.Deserialize<REGISTERPAYSAVE_M.REGISTERPAYSAVE_IN>(json_in);
                REGISTERPAYSAVE_M.REGISTERPAYSAVE_OUT _out = new REGISTERPAYSAVE_M.REGISTERPAYSAVE_OUT();
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
                }
                #endregion

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
                    _out.APPT_TIME = dtrev.Columns.Contains("REALTIMES") ? dtrev.Rows[0]["REALTIMES"].ToString() : "";
                    _out.APPT_PLACE = dtrev.Columns.Contains("ZS_NAME") ? dtrev.Rows[0]["ZS_NAME"].ToString() : "";
                    _out.OPT_SN = dtrev.Columns.Contains("OPT_SN") ? dtrev.Rows[0]["OPT_SN"].ToString() : "";
                    _out.RCPT_NO = dtrev.Columns.Contains("RCPT_NO") ? dtrev.Rows[0]["RCPT_NO"].ToString() : "";

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
