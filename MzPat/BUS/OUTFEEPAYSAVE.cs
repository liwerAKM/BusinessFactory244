using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZJMZ.Model;
using Soft.Core;
using System.Xml;
using System.Data;

namespace ZZJMZ.BUS
{
    class OUTFEEPAYSAVE
    {
        public static string B_OUTFEEPAYSAVE(string json_in)
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
    }
}
