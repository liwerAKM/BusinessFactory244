using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos8_OutHos.Model;
using Soft.Core;
using System.Xml;
using System.Data;
using Log.Core.Model;

namespace OnlineBusHos8_OutHos.BUS
{
    class OUTFEEPAYSAVE
    {
        public static string B_OUTFEEPAYSAVE(string json_in)
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
        public static string UnDoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN _in = JSONSerializer.Deserialize<OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN>(json_in);
                OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT _out = new OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("OUTFEEPAYSAVE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRE_NO", string.IsNullOrEmpty(_in.PRE_NO) ? "" : _in.PRE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPT_SN", string.IsNullOrEmpty(_in.OPT_SN) ? "" : _in.OPT_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JE_ALL", string.IsNullOrEmpty(_in.JEALL) ? "" : _in.JEALL.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_STATES", string.IsNullOrEmpty(_in.DEAL_STATES) ? "" : _in.DEAL_STATES.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TIME", string.IsNullOrEmpty(_in.DEAL_TIME) ? "" : _in.DEAL_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAY_ID", string.IsNullOrEmpty(_in.PAY_ID) ? "" : _in.PAY_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBDJH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BANK_TELLERID", "");             
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
                    _out.HOS_PAY_SN = dtrev.Columns.Contains("HOS_PAY_SN") ? dtrev.Rows[0]["HOS_PAY_SN"].ToString() : "";
                    _out.HOS_REG_SN = dtrev.Columns.Contains("HOS_REG_SN") ? dtrev.Rows[0]["HOS_REG_SN"].ToString() : "";
                    _out.RCPT_NO = dtrev.Columns.Contains("RCPT_NO") ? dtrev.Rows[0]["RCPT_NO"].ToString() : "";
                    _out.OPT_SN = dtrev.Columns.Contains("OPT_SN") ? dtrev.Rows[0]["OPT_SN"].ToString() : "";
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

        public static string DoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {

                OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN _in = JSONSerializer.Deserialize<OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_IN>(json_in);
                OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT _out = new OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_OUT();
                Dictionary<string, string> dic_filter = GlobalVar.Get_Filter(FormatHelper.GetStr(_in.FILTER));
                XmlDocument doc = QHXmlMode.GetBaseXml("OUTFEEPAYSAVE", "1");
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
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YBDJH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JZ_CODE", _in.YLCARD_TYPE.Trim()=="4"?"01":"41");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JE_ALL", _in.JEALL.Trim() );
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MZNO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "JS_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YJS_IN", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YJS_OUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_TYPE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DIS_NO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BANK_JSINPUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BANK_JSOUT", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MB_ID", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", "");
                string BANK = dic_filter.ContainsKey("BANK") ? dic_filter["BANK"] : "0";
                string DEAL_TYPE = "";
                if (_in.DEAL_TYPE == "3" && BANK == "0")
                {
                    DEAL_TYPE = "A";
                }
                else if (_in.DEAL_TYPE == "3" && BANK == "1")
                {
                    DEAL_TYPE = "H";
                }
                else
                {
                    DEAL_TYPE = _in.DEAL_TYPE;
                }
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BANK", BANK);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", DEAL_TYPE);
                DataTable dtzqh = new DataTable();
                if (_in.YLCARD_TYPE == "2")
                {
                    dtzqh = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("ybywzqh", "usr_id='" + _in.USER_ID + "' AND  dj_date like '" + DateTime.Now.ToString("yyyy-MM-dd") + "%'", "yw_zqh");
                }
                else
                {
                }

                string YB_MXINPUT = dic_filter.ContainsKey("YB_MXINPUT") ? dic_filter["YB_MXINPUT"] : "";
                string YB_MXOUT = dic_filter.ContainsKey("YB_MXOUT") ? dic_filter["YB_MXOUT"] : "";
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
                        if (_in.DEAL_TYPE == "1" || _in.DEAL_TYPE == "2")
                        {
                            WXZFBTF_M.WXZFBTF_IN wxzfbtf = new WXZFBTF_M.WXZFBTF_IN();
                            wxzfbtf.USER_ID = _in.USER_ID;
                            wxzfbtf.HOS_ID = _in.HOS_ID;
                            wxzfbtf.LTERMINAL_SN = _in.LTERMINAL_SN;
                            wxzfbtf.QUERYID = _in.QUERYID;
                            wxzfbtf.DEAL_TYPE = _in.DEAL_TYPE;
                            wxzfbtf.CASH_JE = _in.CASH_JE;
                            wxzfbtf.REASON = "自动冲正";
                            wxzfbtf.SFZ_NO = _in.SFZ_NO;
                            wxzfbtf.PAT_NAME = "";
                            wxzfbtf.HOSPATID = _in.HOSPATID;
                            wxzfbtf.SOURCE = "ZZJ";
                            wxzfbtf.FILTER = "";

                            string Namespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace.Split('.')[0].Split('_')[0].Replace("OnlineBusHos", "");
                            string out_data = GlobalVar.CallOtherBus(JSONSerializer.Serialize(wxzfbtf),"PBusHos"+Namespace+"_Tran", "0003").BusData;
                            DataReturn drtn = JSONSerializer.Deserialize<DataReturn>(out_data);
                            OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_ERROR err = new OUTFEEPAYSAVE_M.OUTFEEPAYSAVE_ERROR();
                            if (drtn.Code != 0)
                            {
                                err.TF_RESULT = "0";
                            }
                            else
                            {
                                WXZFBTF_M.WXZFBTF_OUT wxzfb_out = JSONSerializer.Deserialize<WXZFBTF_M.WXZFBTF_OUT>(drtn.Param);
                                if (wxzfb_out.STATUS == "0")
                                {
                                    err.TF_RESULT = "0";
                                }
                                else
                                {
                                    err.TF_RESULT = "1";
                                }
                            }
                            dataReturn.Code = ConstData.CodeDefine.CodeWXZFBTF;
                            dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                            dataReturn.Param = JSONSerializer.Serialize(err);
                            goto EndPoint;
                        }
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        
                        goto EndPoint;
                    }
                    _out.HOS_PAY_SN = dtrev.Columns.Contains("HOS_PAY_SN") ? dtrev.Rows[0]["HOS_PAY_SN"].ToString() : "";
                    _out.HOS_REG_SN = dtrev.Columns.Contains("HOS_REG_SN") ? dtrev.Rows[0]["HOS_REG_SN"].ToString() : "";
                    _out.RCPT_NO = dtrev.Columns.Contains("RCPT_NO") ? dtrev.Rows[0]["RCPT_NO"].ToString() : "";
                    _out.OPT_SN = dtrev.Columns.Contains("OPT_SN") ? dtrev.Rows[0]["OPT_SN"].ToString() : "";
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                    #region 业务数据保存
                    try
                    {
                        Plat.Model.opt_pay_lock modelPayLock = new Plat.Model.opt_pay_lock();
                        modelPayLock = new Plat.MySQLDAL.opt_pay_lock().GetModel_ZZJ(_in.PAY_ID);
                        Plat.Model.opt_pay Modelopt_pay = new Plat.Model.opt_pay();
                        Modelopt_pay.PAY_ID = _in.PAY_ID;
                        Modelopt_pay.HOS_ID = _in.HOS_ID;
                        Modelopt_pay.PAT_ID = modelPayLock.PAT_ID;
                        Modelopt_pay.USER_ID = _in.USER_ID;
                        Modelopt_pay.HOS_SN = _in.HOS_SN;
                        Modelopt_pay.OPT_SN = _out.OPT_SN;
                        Modelopt_pay.HOS_REG_SN = _out.HOS_REG_SN;
                        Modelopt_pay.PRE_NO = modelPayLock.PRE_NO;
                        Modelopt_pay.RCPT_NO = _out.RCPT_NO;
                        //病人基本信息
                        Modelopt_pay.SFZ_NO = modelPayLock.SFZ_NO;
                        Modelopt_pay.YLCARD_NO = modelPayLock.YLCARD_NO;
                        Modelopt_pay.YLCARD_TYPE = modelPayLock.YLCARD_TYPE;
                        Modelopt_pay.PAT_NAME = modelPayLock.PAT_NAME;
                        //从预约挂号表获取数据
                        Modelopt_pay.DEPT_CODE = modelPayLock.DEPT_CODE;
                        Modelopt_pay.DEPT_NAME = modelPayLock.DEPT_NAME;
                        Modelopt_pay.DOC_NO = modelPayLock.DOC_NO == null ? "" : modelPayLock.DOC_NO;
                        Modelopt_pay.DOC_NAME = modelPayLock.DOC_NAME == null ? "" : modelPayLock.DOC_NAME;
                        Modelopt_pay.DIS_NAME = modelPayLock.DIS_NAME == null ? "" : modelPayLock.DIS_NAME;
                        //待定数据
                        Modelopt_pay.lTERMINAL_SN = _in.LTERMINAL_SN;
                        Modelopt_pay.HOS_PAY_SN = _out.HOS_PAY_SN;
                        Modelopt_pay.CASH_JE = FormatHelper.GetDecimal(_in.CASH_JE);
                        Modelopt_pay.PAY_TYPE = modelPayLock.PAY_TYPE;
                        Modelopt_pay.JEALL = modelPayLock.JEALL;
                        Modelopt_pay.JZ_CODE = modelPayLock.JZ_CODE;
                        Modelopt_pay.ybDJH = modelPayLock.ybDJH;
                        Modelopt_pay.GRZF = 0;
                        Modelopt_pay.GRZL = 0;
                        Modelopt_pay.TCZF = 0;
                        Modelopt_pay.DBZF = 0;
                        Modelopt_pay.XJZF = 0;
                        Modelopt_pay.ZHZF = 0;
                        Modelopt_pay.HM = 0;
                        Modelopt_pay.CS = 0;
                        Modelopt_pay.ZFY = 0;
                        Modelopt_pay.XMFY = 0;
                        Modelopt_pay.YF = 0;
                        Modelopt_pay.LCL = 0;
                        Modelopt_pay.ZHYE = 0;
                        Modelopt_pay.XZM = "";
                        Modelopt_pay.XZMCH = "";
                        Modelopt_pay.man_type = "";
                        Modelopt_pay.BZFYY = "";
                        Modelopt_pay.YBBZM = "";
                        Modelopt_pay.FYLB = "";
                        Modelopt_pay.YBBZMC = "";
                        Modelopt_pay.DJ_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                        Modelopt_pay.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");
                        Modelopt_pay.IS_TZ = false;
                        //Modelopt_pay_lock.TZ_DATE
                        //Modelopt_pay_lock.TZ_TIME
                        //Modelopt_pay_lock.PAY_ID_IN
                        Modelopt_pay.lTERMINAL_SN = _in.LTERMINAL_SN == null ? "" : _in.LTERMINAL_SN;
                        Modelopt_pay.PAY_lTERMINAL_SN = _in.LTERMINAL_SN == null ? "" : _in.LTERMINAL_SN;
                        Modelopt_pay.LockPAY_ID = _in.PAY_ID;
                        Plat.MySQLDAL.opt_pay_fl_lock bllpt_pay_fl_lock = new Plat.MySQLDAL.opt_pay_fl_lock();
                        DataTable dtFl_lock = bllpt_pay_fl_lock.GetList_ZZJ("PAY_ID='" + _in.PAY_ID + "'").Tables[0];

                        Plat.Model.opt_pay_fl[] Modelopt_pay_fls = new Plat.Model.opt_pay_fl[dtFl_lock.Rows.Count];
                        Plat.Model.opt_pay_fl_lock Modelopt_pay_fl_lock = new Plat.Model.opt_pay_fl_lock();

                        for (int i = 0; i < dtFl_lock.Rows.Count; i++)
                        {
                            Modelopt_pay_fls[i] = new Plat.Model.opt_pay_fl();
                            Modelopt_pay_fls[i].PAY_ID = _in.PAY_ID;
                            Modelopt_pay_fls[i].DEPT_CODE = dtFl_lock.Rows[i]["DEPT_CODE"].ToString().Trim();
                            Modelopt_pay_fls[i].DEPT_NAME = dtFl_lock.Rows[i]["DEPT_NAME"].ToString().Trim();
                            Modelopt_pay_fls[i].FL_JE = Convert.ToDecimal(dtFl_lock.Rows[i]["FL_JE"].ToString().Trim());
                            Modelopt_pay_fls[i].FL_NAME = dtFl_lock.Rows[i]["FL_NAME"].ToString().Trim();
                            if (dtFl_lock.Rows[i]["FL_NO"].ToString().Trim().Length > 255)
                            {
                                Modelopt_pay_fls[i].FL_NO = dtFl_lock.Rows[i]["FL_NO"].ToString().Trim().Substring(0, 255);
                            }
                            else
                            {
                                Modelopt_pay_fls[i].FL_NO = dtFl_lock.Rows[i]["FL_NO"].ToString().Trim();
                            }
                            Modelopt_pay_fls[i].FL_ORDER = dtFl_lock.Rows[i]["FL_ORDER"].ToString().Trim();
                        }
                        DataTable dtMx_lock = new Plat.MySQLDAL.opt_pay_mx_lock().GetList_ZZJ("PAY_ID='" + _in.PAY_ID + "'").Tables[0];

                        Plat.Model.opt_pay_mx[] Modelopt_pay_mxs = new Plat.Model.opt_pay_mx[dtMx_lock.Rows.Count];
                        Plat.Model.opt_pay_mx_lock Modelopt_pay_mx_lock = new Plat.Model.opt_pay_mx_lock();
                        for (int i = 0; i < dtMx_lock.Rows.Count; i++)
                        {
                            Modelopt_pay_mxs[i] = new Plat.Model.opt_pay_mx();
                            Modelopt_pay_mxs[i].COST = Convert.ToDecimal(dtMx_lock.Rows[i]["COST"].ToString().Trim());
                            Modelopt_pay_mxs[i].COUNT = dtMx_lock.Rows[i]["COUNT"].ToString().Trim();
                            if (dtMx_lock.Rows[i]["FL_NO"].ToString().Trim().Length > 255)
                            {
                                Modelopt_pay_mxs[i].FL_NO = dtMx_lock.Rows[i]["FL_NO"].ToString().Trim().Substring(0, 255);
                            }
                            else
                            {
                                Modelopt_pay_mxs[i].FL_NO = dtMx_lock.Rows[i]["FL_NO"].ToString().Trim();
                            }
                            Modelopt_pay_mxs[i].FL_NO = dtMx_lock.Rows[i]["FL_NO"].ToString().Trim();
                            Modelopt_pay_mxs[i].ITEM_GG = dtMx_lock.Rows[i]["ITEM_GG"].ToString().Trim();
                            Modelopt_pay_mxs[i].ITEM_ID = dtMx_lock.Rows[i]["ITEM_ID"].ToString().Trim();
                            Modelopt_pay_mxs[i].ITEM_NAME = dtMx_lock.Rows[i]["ITEM_NAME"].ToString().Trim();
                            Modelopt_pay_mxs[i].ITEM_TYPE = dtMx_lock.Rows[i]["ITEM_TYPE"].ToString().Trim();
                            Modelopt_pay_mxs[i].ITEM_UNIT = dtMx_lock.Rows[i]["ITEM_UNIT"].ToString().Trim();

                            Modelopt_pay_mxs[i].je = Convert.ToDecimal(dtMx_lock.Rows[i]["je"].ToString().Trim());
                            Modelopt_pay_mxs[i].PAY_ID = _in.PAY_ID;
                        }

                        Plat.Model.opt_pay_log log = new Plat.Model.opt_pay_log();
                        log.PAY_ID = Modelopt_pay.PAY_ID;
                        log.STATES = "2";
                        log.HOS_ID = _in.HOS_ID;
                        log.PAT_ID = modelPayLock.PAT_ID;
                        log.JEALL = FormatHelper.GetDecimal(_in.CASH_JE);
                        log.CASH_JE = FormatHelper.GetDecimal(_in.CASH_JE);
                        log.DJ_DATE = DateTime.Now;
                        log.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");
                        log.lTERMINAL_SN = _in.LTERMINAL_SN;
                        log.HSP_SN = Modelopt_pay.HOS_SN;


                        //现金交易记录表
                        Plat.Model.pay_info modelpay_info = new Plat.Model.pay_info();

                        int pinfo_id = 0;//预约标识
                        if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase("pay_info", out pinfo_id))
                        {
                            string s = DateTime.Now.Ticks.ToString();
                            pinfo_id = Convert.ToInt32(s.Substring(s.Length - 6));
                        }

                        //现金交易记录表
                        modelpay_info.PAT_ID = modelPayLock.PAT_ID;
                        modelpay_info.PAY_ID = pinfo_id.ToString();
                        modelpay_info.HOS_ID = _in.HOS_ID;
                        modelpay_info.USER_ID = _in.USER_ID;
                        modelpay_info.BIZ_TYPE = 2;
                        modelpay_info.BIZ_SN = _in.PAY_ID;
                        modelpay_info.CASH_JE = FormatHelper.GetDecimal(_in.CASH_JE);
                        modelpay_info.SFZ_NO = modelPayLock.SFZ_NO;
                        modelpay_info.DJ_DATE = _in.DEAL_TIME.Substring(0, 10);
                        modelpay_info.DJ_TIME = _in.DEAL_TIME.Substring(11, 8);
                        modelpay_info.DEAL_TYPE = DEAL_TYPE;
                        modelpay_info.DEAL_STATES = _in.DEAL_STATES;
                        modelpay_info.lTERMINAL_SN = _in.LTERMINAL_SN;
                        //支付宝
                        Plat.Model.pay_info_zfb modelpay_info_zfb = null;
                        if (DEAL_TYPE == "2")
                        {
                            DataTable dtTran = new Plat.MySQLDAL.BaseFunction().GetList("alipay_tran", "comm_sn='" + _in.QUERYID + "'", "seller_id");
                            modelpay_info_zfb = new Plat.Model.pay_info_zfb();
                            modelpay_info_zfb.PAY_ID = pinfo_id.ToString();
                            modelpay_info_zfb.SELLER_ID = dtTran.Rows.Count == 0 ? "" : dtTran.Rows[0]["seller_id"].ToString().Trim();
                            modelpay_info_zfb.BIZ_TYPE = 2;
                            modelpay_info_zfb.BIZ_SN = _in.PAY_ID;
                            modelpay_info_zfb.JE = FormatHelper.GetDecimal(_in.CASH_JE);
                            modelpay_info_zfb.DEAL_STATES = _in.DEAL_STATES;
                            modelpay_info_zfb.DEAL_TIME = DateTime.Parse(_in.DEAL_TIME);
                            modelpay_info_zfb.COMM_SN = _in.QUERYID;
                            modelpay_info_zfb.lTERMINAL_SN = _in.LTERMINAL_SN;
                            modelpay_info_zfb.TXN_TYPE = "01";
                        }
                        //微信
                        Plat.Model.pay_info_wc wc = null;
                        if (DEAL_TYPE == "1")
                        {
                            DataTable dtTran = new Plat.MySQLDAL.BaseFunction().GetList("wechat_tran", "comm_sn='" + _in.QUERYID + "'", "appid", "transaction_id");
                            wc = new Plat.Model.pay_info_wc();
                            wc.PAY_ID = pinfo_id.ToString();
                            wc.WECHAT = "";
                            wc.PAY_TYPE = "";
                            wc.BIZ_TYPE = 2;
                            wc.BIZ_SN = _in.PAY_ID;
                            wc.COMM_SN = _in.QUERYID;
                            wc.JE = FormatHelper.GetDecimal(_in.CASH_JE);
                            wc.COMM_NAME = dtTran.Rows.Count == 0 ? "" : dtTran.Rows[0]["appid"].ToString().Trim();
                            wc.DEAL_STATES = "2";
                            wc.DEAL_TIME = DateTime.Now;
                            wc.DEAL_SN = dtTran.Rows.Count == 0 ? "" : dtTran.Rows[0]["transaction_id"].ToString().Trim();
                            wc.TNX_TYPE = "01";
                            wc.lTERMINAL_SN = _in.LTERMINAL_SN;
                        }
                        //医保
                        #region add by 姚逸晖 2018.07.04
                        Plat.Model.pay_info_yb yb = null;
                        if (_in.YLCARD_TYPE == "2")
                        {
                            yb = new Plat.Model.pay_info_yb();
                            yb.PAY_ID = pinfo_id;
                            yb.DJ_ID = Modelopt_pay.HOS_PAY_SN;
                            //入参
                            if (dic_filter.ContainsKey("YB_JSINPUT"))
                            {
                                string[] js_input = dic_filter["YB_JSINPUT"].ToString().Trim().Split('|');
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
                            string[] js_out = dic_filter["JS_OUT"].ToString().Trim().Split('|');
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
                            yb.lTERMINAL_SN = Modelopt_pay.lTERMINAL_SN;

                            yb.DEAL_TIME = DateTime.Now;
                            yb.YWZQH = dtzqh.Rows.Count > 0 ? dtzqh.Rows[0]["yw_zqh"].ToString().Trim() : "";
                        }
                        #endregion
                        //保存数据到后台
                        if (new Plat.MySQLDAL.opt_pay().AddByTran_ZZJ(Modelopt_pay, Modelopt_pay_fls, Modelopt_pay_mxs, modelpay_info, modelpay_info_zfb, wc, null, log, null, null, yb, YB_MXINPUT, YB_MXOUT))
                        {
                            try
                            {
                                Dictionary<int, string> dic = new Dictionary<int, string>();
                                dic[0] = "门诊缴费";
                                dic[1] = Modelopt_pay.PAT_ID.ToString();
                                dic[2] = Modelopt_pay.SFZ_NO;
                                dic[3] = _in.YLCARD_NO;
                                dic[4] = Modelopt_pay.PAY_ID.ToString();//平台流水号
                                dic[5] = Modelopt_pay.HOS_PAY_SN;//医院流水号
                                dic[6] = "";//预约平台流水号
                                dic[7] = _in.QUERYID;//银行流水号
                                dic[8] = "1";//是否有效
                                dic[9] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                dic[10] = DEAL_TYPE;//支付方式
                                dic[11] = _in.LTERMINAL_SN.Contains("WZZ") ? "WZZ" : "ZZJ";
                                dic[12] = FormatHelper.GetDecimal(_in.CASH_JE).ToString();
                                dic[13] = _in.USER_ID;
                                dic[14] = _in.LTERMINAL_SN;
                                dic[15] = _in.YLCARD_TYPE == "4" ? "自费" : (_in.YLCARD_TYPE == "2" ? "医保" : "其他");
                                dic[16] = _in.HOS_ID;
                                dic[17] = "1";
                                dic[18] = _in.JEALL.ToString();
                                string sqlword = string.Format(@"insert into platmx values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}',
                 '{12}','{13}','{14}','{15}','{16}','{17}','{18}')",
                    dic[0], dic[1], dic[2], dic[3], dic[4], dic[5], dic[6], dic[7], int.Parse(dic[8]), DateTime.Now, dic[10], dic[11], dic[12], dic[13], dic[14], dic[15], dic[16], dic[17], dic[18]);

                                new Plat.MySQLDAL.BaseFunction().ExecSql(sqlword);

                            }
                            catch
                            { }
                        }
                    }
                    catch(Exception ex)
                    {
                        ModSqlError modSqlError = new ModSqlError();
                        modSqlError.TYPE = "诊间支付保存";
                        modSqlError.time = DateTime.Now;
                        modSqlError.EXCEPTION = ex.ToString().Replace("'", "\"");
                        new Log.Core.MySQLDAL.DalSqlERRROR().Add(modSqlError);
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
    }
}
