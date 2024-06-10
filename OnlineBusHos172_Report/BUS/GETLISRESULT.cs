using CommonModel;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using OnlineBusHos172_Report.Model;
using System.Reflection.Emit;
using System.Reflection;

namespace OnlineBusHos172_Report.BUS
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



        public static string A_GETLISRESULT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETLISRESULT_M.GETLISRESULT_IN _in = JSONSerializer.Deserialize<GETLISRESULT_M.GETLISRESULT_IN>(json_in);
                GETLISRESULT_M.GETLISRESULT_OUT _out = new GETLISRESULT_M.GETLISRESULT_OUT();
                if (string.IsNullOrEmpty(_in.REPORT_SN))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "REPORT_SN不能为空";
                    goto EndPoint;
                }

                XmlDocument inXmlDoc = XMLHelper.X_CreateXmlDocument("Request");
                XMLHelper.X_XmlInsertNode(inXmlDoc, "Request", "MessageHead");//消息头
                XMLHelper.X_XmlInsertNode_NOCHANGE(inXmlDoc, "Request/MessageHead", "MessageID", Guid.NewGuid().ToString("N"));//32位guid 
                XMLHelper.X_XmlInsertNode_NOCHANGE(inXmlDoc, "Request/MessageHead", "MessageSender", "QH");//高淳中lis注册的厂商号
                XMLHelper.X_XmlInsertNode(inXmlDoc, "Request", "MessageBody");//消息体
                DataTable reportDt = new DataTable();
                reportDt.Columns.Add("ReportCode", typeof(string));//传该字段下面参数可以为空
                reportDt.Columns.Add("PATIENT_ID", typeof(string));
                reportDt.Columns.Add("BeginDate", typeof(string));
                reportDt.Columns.Add("EndDate", typeof(string));

                DataRow dataRow = reportDt.NewRow();
                dataRow["ReportCode"] = _in.REPORT_SN;
                dataRow["PATIENT_ID"] = "";
                dataRow["BeginDate"] = "";
                dataRow["EndDate"] = "";
                reportDt.Rows.Add(dataRow);
                XMLHelper.X_XmlInsertTable_NOCHANGE(inXmlDoc, "Request/MessageBody", reportDt, "Table");

                string inxml = inXmlDoc.InnerXml;
                string his_rtnxml = "";

                Hashtable hashtable = new Hashtable
                {
                    { "xmldata",inxml }
                };
                string MethodName = "GetReport";
                if (!GlobalVar.CALLSERVICE_LIS(inxml, hashtable, MethodName, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }

      
                _out.HIS_RTNXML = "";
                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "Response/MessageHead");
                    DataTable dtrev = ds.Tables[0];
                    if (dtrev.Rows[0]["MessageResultValue"].ToString() != "1")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Columns.Contains("MessageReusltInfo") ? dtrev.Rows[0]["MessageReusltInfo"].ToString() : "";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtlisreport = XMLHelper.X_GetXmlData(xmldoc, "Response/MessageBody/Table/Report").Tables[0];
                        _out.DATA_TYPE = "1";
                        _out.REPORTDATA = dtlisreport.Columns.Contains("ReportPDF") ? dtlisreport.Rows[0]["ReportPDF"].ToString() : "";

                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析LIS出参失败,请检查出参";
                        goto EndPoint;
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析LIS出参失败,请检查LIS出参是否正确";

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
