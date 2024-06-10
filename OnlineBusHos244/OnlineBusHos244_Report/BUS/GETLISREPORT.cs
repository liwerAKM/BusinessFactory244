using CommonModel;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using OnlineBusHos244_Report.Model;

namespace OnlineBusHos244_Report.BUS
{
    class GETLISREPORT
    {
        public static string B_GETLISREPORT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                

                GETLISREPORT_M.GETLISREPORT_IN _in = JSONSerializer.Deserialize<GETLISREPORT_M.GETLISREPORT_IN>(json_in);
                GETLISREPORT_M.GETLISREPORT_OUT _out = new GETLISREPORT_M.GETLISREPORT_OUT();

                //string YLCARD_TYPE = GlobalVar.GETHISYLCARDTYPE(_in.YLCARD_TYPE.Trim());

                XmlDocument doc = QHXmlMode.GetBaseXml("GETLISREPORT", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "LTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGEINDEX", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGESIZE", "100");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "LOCAL_ID_TYPE", "4");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "LOCAL_ID", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "VER_DATETIME_START", DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "VER_DATETIME_END", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
                    DataTable dtrev = ds.Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Columns.Contains("CLJG") ? dtrev.Rows[0]["CLJG"].ToString() : "";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    DataTable dtlisreport = new DataTable();
                    try
                    {
                        dtlisreport = ds.Tables["LISREPORT"];
                        if (dtlisreport.Rows.Count == 0)
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "未查询到检验报告列表";
                            goto EndPoint;
                        }
                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到LISREPORT节点,请检查出参";
                        goto EndPoint;
                    }
                    _out.REPORT_ALL_NUM = dtlisreport.Rows.Count.ToString();
                    if (dtlisreport.Columns.Contains("AUDIT_FLAG"))
                    {
                        _out.REPORT_AUDIT_NUM = dtlisreport.Select("AUDIT_FLAG=0").Length.ToString();
                        _out.REPORT_PRINT_NUM = dtlisreport.Select("PRINT_FLAG=0 and AUDIT_FLAG=1").Length.ToString();
                    }
                    else
                    {
                        _out.REPORT_PRINT_NUM = dtlisreport.Select("PRINT_FLAG=0").Length.ToString();
                    }


                    _out.LISREPORT = new List<GETLISREPORT_M.LisReport>();
                    foreach (DataRow dr in dtlisreport.Rows)
                    {
                        GETLISREPORT_M.LisReport lis = new GETLISREPORT_M.LisReport();
                        lis.REPORT_SN = dtlisreport.Columns.Contains("REPORT_SN") ? FormatHelper.GetStr(dr["REPORT_SN"]) : "";
                        lis.REPORT_TYPE = dtlisreport.Columns.Contains("REPORT_TYPE") ? FormatHelper.GetStr(dr["REPORT_TYPE"]) : "";
                        lis.REPORT_DATE = dtlisreport.Columns.Contains("REPORT_DATE") ? FormatHelper.GetStr(dr["REPORT_DATE"]) : "";
                        lis.PRINT_FLAG = dtlisreport.Columns.Contains("PRINT_FLAG") ? FormatHelper.GetStr(dr["PRINT_FLAG"]) : lis.PRINT_FLAG;
                        lis.PRINT_TIME = dtlisreport.Columns.Contains("PRINT_TIME") ? FormatHelper.GetStr(dr["PRINT_TIME"]) : "";
                        lis.TEST_DATE = dtlisreport.Columns.Contains("TEST_DATE") ? FormatHelper.GetStr(dr["TEST_DATE"]) : "";
                        lis.TEST_DOC_NAME = dtlisreport.Columns.Contains("TEST_DOC_NAME") ? FormatHelper.GetStr(dr["TEST_DOC_NAME"]) : "";
                        lis.TEST_DEPT_NAME = dtlisreport.Columns.Contains("TEST_DEPT_NAME") ? FormatHelper.GetStr(dr["TEST_DEPT_NAME"]) : "";
                        lis.AUDIT_DATE = dtlisreport.Columns.Contains("AUDIT_DATE") ? FormatHelper.GetStr(dr["AUDIT_DATE"]) : "";
                        lis.AUDIT_DOC_NAME = dtlisreport.Columns.Contains("AUDIT_DOC_NAME") ? FormatHelper.GetStr(dr["AUDIT_DOC_NAME"]) : "";
                        lis.AUDIT_FLAG = dtlisreport.Columns.Contains("AUDIT_FLAG") ? FormatHelper.GetStr(dr["AUDIT_FLAG"]) : lis.AUDIT_FLAG;
                        lis.DATA_TYPE = "2";
                        lis.REPORTDATA = dtlisreport.Columns.Contains("URL") ? FormatHelper.GetStr(dr["URL"]) : "";
                        lis.SIGNER_URL = dtlisreport.Columns.Contains("SIGNER_URL") ? FormatHelper.GetStr(dr["SIGNER_URL"]) : "";
                        _out.LISREPORT.Add(lis);
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
                dataReturn.Param = ex.ToString();
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}
