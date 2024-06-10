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
    class GETORDERSTATUS
    {
        public static string B_GETORDERSTATUS(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETORDERSTATUS_M.GETORDERSTATUS_IN _in = JSONSerializer.Deserialize<Model.GETORDERSTATUS_M.GETORDERSTATUS_IN>(json_in);
                Model.GETORDERSTATUS_M.GETORDERSTATUS_OUT _out = new Model.GETORDERSTATUS_M.GETORDERSTATUS_OUT();

                //调用远端服务 卫宁支付
                XmlDocument doc = QHXmlMode.GetBaseXml("GETORDERSTATUS", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
             

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
                        _out.STATUS = "0";
                        dataReturn.Param = JSONSerializer.Serialize(_out);

                    }
                    else
                    {
                        dataReturn.Code = 0;
                        dataReturn.Msg = "SUCCESS";
                        _out.STATUS = dtrev.Rows[0]["STATUS"].ToString();
                        _out.QUERYID = dtrev.Columns.Contains("QUERYID")? dtrev.Rows[0]["QUERYID"].ToString():"";
                        _out.INNERORDERNO = dtrev.Columns.Contains("INNERORDERNO") ? dtrev.Rows[0]["INNERORDERNO"].ToString() : "";
                        _out.OUTERORDERNO = dtrev.Columns.Contains("OUTERORDERNO") ? dtrev.Rows[0]["OUTERORDERNO"].ToString() : "";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                    }
                }
                catch (Exception ex)
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
