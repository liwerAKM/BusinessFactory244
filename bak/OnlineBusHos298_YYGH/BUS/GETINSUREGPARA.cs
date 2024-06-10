using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos298_YYGH.Model;
using Soft.Core;
using System.Xml;
using System.Data;
namespace OnlineBusHos298_YYGH.BUS
{
    class GETINSUREGPARA
    {
        public static string B_GETINSUREGPARA(string json_in)
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
                GETINSUREGPARA_M.GETINSUREGPARA_IN _in = JSONSerializer.Deserialize<GETINSUREGPARA_M.GETINSUREGPARA_IN>(json_in);
                GETINSUREGPARA_M.GETINSUREGPARA_OUT _out = new GETINSUREGPARA_M.GETINSUREGPARA_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETINSUREGPARA", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", string.IsNullOrEmpty(_in.BARCODE) ? "" : _in.BARCODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REGISTER_TYPE", string.IsNullOrEmpty(_in.REGISTER_TYPE) ? "" : _in.REGISTER_TYPE.Trim());

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
                    _out.USERID= dtrev.Columns.Contains("USERID") ? dtrev.Rows[0]["USERID"].ToString() : "";
                    _out.PAADMID= dtrev.Columns.Contains("PAADMID") ? dtrev.Rows[0]["PAADMID"].ToString() : "";
                    _out.EXPSTRING = dtrev.Columns.Contains("EXPSTRING") ? dtrev.Rows[0]["EXPSTRING"].ToString() : "";
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
                GETINSUREGPARA_M.GETINSUREGPARA_IN _in = JSONSerializer.Deserialize<GETINSUREGPARA_M.GETINSUREGPARA_IN>(json_in);
                GETINSUREGPARA_M.GETINSUREGPARA_OUT _out = new GETINSUREGPARA_M.GETINSUREGPARA_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETINSUREGPARA", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", string.IsNullOrEmpty(_in.BARCODE) ? "" : _in.BARCODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REGISTER_TYPE", string.IsNullOrEmpty(_in.REGISTER_TYPE) ? "" : _in.REGISTER_TYPE.Trim());

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
                    _out.USERID = dtrev.Columns.Contains("USERID") ? dtrev.Rows[0]["USERID"].ToString() : "";
                    _out.PAADMID = dtrev.Columns.Contains("PAADMID") ? dtrev.Rows[0]["PAADMID"].ToString() : "";
                    _out.EXPSTRING = dtrev.Columns.Contains("EXPSTRING") ? dtrev.Rows[0]["EXPSTRING"].ToString() : "";
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
