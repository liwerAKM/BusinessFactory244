using CommonModel;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using OnlineBusHos36_Report.Model;

namespace OnlineBusHos36_Report.BUS
{
    class GETLISRESULT
    {
        public static string B_GETLISRESULT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETLISRESULT_M.GETLISRESULT_IN _in = JSONSerializer.Deserialize<GETLISRESULT_M.GETLISRESULT_IN>(json_in);
                GETLISRESULT_M.GETLISRESULT_OUT _out = new GETLISRESULT_M.GETLISRESULT_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETLISRESULT_PDF", "1");
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", "218");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REPORT_SN", string.IsNullOrEmpty(_in.REPORT_SN) ? "" : _in.REPORT_SN.Trim());


                string inxml = doc.InnerXml;
                string his_rtnxml = "";

                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }


                //_out.HIS_RTNXML = his_rtnxml;
                _out.HIS_RTNXML = "";
                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
                    DataTable dtrev = ds.Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Columns.Contains("CLJG") ? dtrev.Rows[0]["CLJG"].ToString() : "";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtlisreport = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/LISRESULT").Tables[0] ;
                        _out.DATA_TYPE = dtlisreport.Columns.Contains("DATA_TYPE") ? dtlisreport.Rows[0]["DATA_TYPE"].ToString() : "1";
                        _out.REPORTDATA = dtlisreport.Columns.Contains("PDF") ? dtlisreport.Rows[0]["PDF"].ToString() : "";

                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到LISRESULT节点,请检查出参";
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
