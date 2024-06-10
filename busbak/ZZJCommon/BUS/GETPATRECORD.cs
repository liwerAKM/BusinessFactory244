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
    class GETPATRECORD
    {
        public static string B_GETPATRECORD(string json_in)
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
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", string.IsNullOrEmpty(_in.SEX) ? "" : _in.SEX.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "AGE", string.IsNullOrEmpty(_in.AGE) ? "" : _in.AGE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", string.IsNullOrEmpty(_in.MOBILE_NO) ? "" : _in.MOBILE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "ADDRESS", string.IsNullOrEmpty(_in.ADDRESS) ? "" : _in.ADDRESS.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIR_DATE", string.IsNullOrEmpty(_in.BIR_DATE) ? "" : _in.BIR_DATE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NATION", string.IsNullOrEmpty(_in.NATION) ? "" : _in.NATION.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_CARD_OUT", string.IsNullOrEmpty(_in.PAT_CARD_OUT) ? "" : _in.PAT_CARD_OUT.Trim());

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
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        goto EndPoint;
                    }
                    _out.HOSPATID= dtrev.Columns.Contains("OPT_SN") ? FormatHelper.GetStr(dtrev.Rows[0]["OPT_SN"]) : "";
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
