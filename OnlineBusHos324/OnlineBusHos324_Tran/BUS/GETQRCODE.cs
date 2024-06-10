using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using Com.Alipay.Business;

namespace OnlineBusHos324_Tran.BUS
{
    class GETQRCODE
    {
        public static string B_GETQRCODE(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETQRCODE_M.GETQRCODE_IN _in = JSONSerializer.Deserialize<Model.GETQRCODE_M.GETQRCODE_IN>(json_in);
                Model.GETQRCODE_M.GETQRCODE_OUT _out = new Model.GETQRCODE_M.GETQRCODE_OUT();

                //调用远端服务 卫宁支付
                XmlDocument doc = QHXmlMode.GetBaseXml("GETQRCODE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "TYPE", string.IsNullOrEmpty(_in.TYPE) ? "" : _in.TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", _in.DEAL_TYPE+_in.TYPE+ _in.SFZ_NO.Substring(_in.SFZ_NO.Length-4));

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
                        dataReturn.Code = -1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);

                    }
                    else
                    {
                        dataReturn.Code =0;
                        dataReturn.Msg = "SUCCESS";
                        _out.QRCODE = dtrev.Rows[0]["QRCODE"].ToString();
                        _out.QUERYID = dtrev.Rows[0]["QUERYID"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                    }
                }
                catch(Exception ex) 
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析出参异常";
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
