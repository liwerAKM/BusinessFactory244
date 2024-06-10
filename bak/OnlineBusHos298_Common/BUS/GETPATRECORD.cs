using CommonModel;
using DB.Core;
using OnlineBusHos298_Common.Model;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos298_Common.BUS
{
    class GETPATRECORD
    {
        public static string B_GETPATRECORD(string json_in)
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
            int pat_id = 0;
            try
            {
                GETPATRECORD_M.GETPATRECORD_IN _in = JSONSerializer.Deserialize<GETPATRECORD_M.GETPATRECORD_IN>(json_in);
                GETPATRECORD_M.GETPATRECORD_OUT _out = new GETPATRECORD_M.GETPATRECORD_OUT();
                string HOS_ID = FormatHelper.GetStr(_in.HOS_ID);
                string SFZ_NO = FormatHelper.GetStr(_in.SFZ_NO);
                string PAT_NAME= FormatHelper.GetStr(_in.PAT_NAME);
                string SEX = FormatHelper.GetStr(_in.SEX);
                string ADDRESS = FormatHelper.GetStr(_in.ADDRESS);
                string BIRTHDAY = FormatHelper.GetStr(_in.BIR_DATE);
                string GUARDIAN_NAME = FormatHelper.GetStr(_in.GUARDIAN_NAME);
                string GUARDIAN_SFZ_NO = FormatHelper.GetStr(_in.GUARDIAN_SFZ_NO);
                string MOBILE_NO = FormatHelper.GetStr(_in.MOBILE_NO);
                string YLCARD_TYPE = FormatHelper.GetStr(_in.YLCARD_TYPE);
                string YLCARD_NO = FormatHelper.GetStr(_in.YLCARD_NO);
                string USER_ID = FormatHelper.GetStr(_in.USER_ID);
                string PAT_CARD_OUT = FormatHelper.GetStr(_in.PAT_CARD_OUT);

                DataTable dtpat_info = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "SFZ_NO='" + SFZ_NO + "'", "MOBILE_NO", "PAT_ID", "GUARDIAN_NAME");
                if (dtpat_info.Rows.Count == 0)
                {
                    if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase("pat_info", out pat_id))
                    {
                        dataReturn.Code = 7;
                        dataReturn.Msg = "获取pat_info的pat_id失败";
                        goto EndPoint;
                    }

                    Plat.Model.pat_info pat_info = new Plat.Model.pat_info();
                    pat_info.PAT_ID = pat_id;
                    pat_info.PAT_NAME = PAT_NAME;
                    pat_info.SEX = SEX;
                    pat_info.SFZ_NO = SFZ_NO;
                    pat_info.ADDRESS = ADDRESS;
                    pat_info.BIRTHDAY = BIRTHDAY;
                    pat_info.GUARDIAN_NAME = GUARDIAN_NAME;
                    pat_info.GUARDIAN_SFZ_NO = GUARDIAN_SFZ_NO;
                    pat_info.MOBILE_NO = MOBILE_NO;
                    pat_info.OPER_TIME = DateTime.Now;
                    pat_info.CREATE_TIME = DateTime.Now;
                    pat_info.MARK_DEL = false;
                    pat_info.NOTE = "";
                    pat_info.YB_CARDNO = YLCARD_TYPE == "2" ? YLCARD_NO : "";
                    pat_info.SMK_CARDNO = YLCARD_TYPE == "3" ? YLCARD_NO : "";
                }
                XmlDocument doc = new XmlDocument();
                doc = QHXmlMode.GetBaseXml("SENDCARDINFO", "0");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARTD_TYPE", YLCARD_TYPE == "2" ? YLCARD_TYPE : "4");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", YLCARD_NO.Trim() == "2" ? YLCARD_NO : SFZ_NO);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", PAT_NAME);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", SEX);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIRTHDAY", BIRTHDAY);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_SFZ_NO", GUARDIAN_SFZ_NO);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_NAME", GUARDIAN_NAME);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", SFZ_NO);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", MOBILE_NO);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "ADDRESS", ADDRESS);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPERATOR", USER_ID);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_OUT", PAT_CARD_OUT);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FZH", "");

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
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        goto EndPoint;
                    }
                    if (dtrev.Columns.Contains("HOSPATID"))
                    {
                        _out.HOSPATID = dtrev.Columns.Contains("HOSPATID") ? FormatHelper.GetStr(dtrev.Rows[0]["HOSPATID"]) : "";
                    }
                    else
                    {
                        _out.HOSPATID = dtrev.Columns.Contains("BARCODE") ? FormatHelper.GetStr(dtrev.Rows[0]["BARCODE"]) : "";
                    }
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
        public static string UnDoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETPATRECORD_M.GETPATRECORD_IN _in = JSONSerializer.Deserialize<GETPATRECORD_M.GETPATRECORD_IN>(json_in);
                GETPATRECORD_M.GETPATRECORD_OUT _out = new GETPATRECORD_M.GETPATRECORD_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETPATRECORD", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", string.IsNullOrEmpty(_in.SEX) ? "" : _in.SEX.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "AGE", string.IsNullOrEmpty(_in.AGE) ? "" : _in.AGE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", string.IsNullOrEmpty(_in.MOBILE_NO) ? "" : _in.MOBILE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "ADDRESS", string.IsNullOrEmpty(_in.ADDRESS) ? "" : _in.ADDRESS.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIR_DATE", string.IsNullOrEmpty(_in.BIR_DATE) ? "" : _in.BIR_DATE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NATION", string.IsNullOrEmpty(_in.NATION) ? "" : _in.NATION.Trim());
                string PAT_CARD_OUT = "";
                if (_in.YLCARD_TYPE == "2")//医保卡
                {
                    DataTable dtchs_psn = DbHelperMySQLInsur.Query("select * from chs_psn where psn_no='" +_in.YLCARD_NO + "'").Tables[0];
                    if (dtchs_psn.Rows.Count > 0)
                    {
                        PAT_CARD_OUT = FormatHelper.GetStr(dtchs_psn.Rows[0]["chsOutput1101"]);
                    }
                }
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", PAT_CARD_OUT);

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
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        goto EndPoint;
                    }
                    _out.HOSPATID = dtrev.Columns.Contains("OPT_SN") ? FormatHelper.GetStr(dtrev.Rows[0]["OPT_SN"]) : "";
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
