using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using ZZJCommon.Model;

namespace ZZJCommon.BUS
{
    class GETPATINFO
    {
        public static string B_GETPATINFO(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETPATINFO_M.GETPATINFO_IN _in = JSONSerializer.Deserialize<GETPATINFO_M.GETPATINFO_IN>(json_in);
                GETPATINFO_M.GETPATINFO_OUT _out = new GETPATINFO_M.GETPATINFO_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETPATINFO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
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
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                    
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        _out.IS_EXIST = "0";
                        goto EndPoint1;
                    }
                    _out.IS_EXIST = "1";
                    _out.PAT_NAME = dtrev.Columns.Contains("PAT_NAME") ? FormatHelper.GetStr(dtrev.Rows[0]["PAT_NAME"]) : "";
                    _out.SEX = dtrev.Columns.Contains("SEX") ? FormatHelper.GetStr(dtrev.Rows[0]["SEX"]) : "";
                    _out.AGE = dtrev.Columns.Contains("AGE") ? FormatHelper.GetStr(dtrev.Rows[0]["AGE"]) : "";
                    _out.MOBILE_NO = dtrev.Columns.Contains("TEL_NO") ? FormatHelper.GetStr(dtrev.Rows[0]["TEL_NO"]) : "";
                    _out.ADDRESS = dtrev.Columns.Contains("ADDRESS") ? FormatHelper.GetStr(dtrev.Rows[0]["ADDRESS"]) : "";
                    _out.SFZ_NO = dtrev.Columns.Contains("SFZ_NO") ? FormatHelper.GetStr(dtrev.Rows[0]["SFZ_NO"]) : "";
                    _out.HOSPATID = dtrev.Columns.Contains("OPT_SN") ? FormatHelper.GetStr(dtrev.Rows[0]["OPT_SN"]) : "";
                    _out.BIR_DATE = dtrev.Columns.Contains("BIR_DATE") ? FormatHelper.GetStr(dtrev.Rows[0]["BIR_DATE"]) : "";
                    _out.GUARDIAN_NAME = dtrev.Columns.Contains("GUARDIAN_NAME") ? FormatHelper.GetStr(dtrev.Rows[0]["GUARDIAN_NAME"]) : "";
                 EndPoint1:
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
                dataReturn.Msg = "程序处理异常:"+ex+";";
            }
            EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}
