using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using ZZJZY.Model;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;

namespace ZZJZY.BUS
{
    class WDPRINT
    {
        public static string B_WDPRINT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                WDPRINT_M.WDPRINT_IN _in = JSONSerializer.Deserialize<WDPRINT_M.WDPRINT_IN>(json_in);
                WDPRINT_M.WDPRINT_OUT _out = new WDPRINT_M.WDPRINT_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("WDPRINT", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.HOS_PAT_ID) ? "" : _in.HOS_PAT_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.HOS_NO) ? "" : _in.HOS_NO.Trim());
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
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
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
    }
}
