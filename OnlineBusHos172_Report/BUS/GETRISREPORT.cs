using CommonModel;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using OnlineBusHos172_Report.Model;

namespace OnlineBusHos172_Report.BUS
{
    class GETRISREPORT
    {
        public static string B_GETRISREPORT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETRISREPORT_M.GETRISREPORT_IN _in = JSONSerializer.Deserialize<GETRISREPORT_M.GETRISREPORT_IN>(json_in);
                GETRISREPORT_M.GETRISREPORT_OUT _out = new GETRISREPORT_M.GETRISREPORT_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETRISREPORT", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.HOSPATID) ? _in.YLCARD_NO.Trim() : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BEGIN_DATE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "END_DATE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGEINDEX", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGESIZE","30");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? _in.YLCARD_NO.Trim() : _in.HOSPATID.Trim());

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                //his_rtnxml = string.Format(@"<ROOT><HEADER><TYPE>GETRISREPORTLIST</TYPE><CZLX></CZLX></HEADER><BODY><CLBZ>INF210</CLBZ><CLJG>没有找到对应的报告记录,查询号:621594273246</CLJG><PAGE_COUNT>10</PAGE_COUNT><RISREPORTLIST><RISREPORT><HOS_SN></HOS_SN><REPORT_SN></REPORT_SN><REPORT_TYPE>心电</REPORT_TYPE><REPORT_NAME>胸部</REPORT_NAME><CHECK_DATE></CHECK_DATE><REPORT_DATE></REPORT_DATE><AUDIT_DATE></AUDIT_DATE><PDFURL></PDFURL></RISREPORT></RISREPORTLIST></BODY></ROOT>");
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
                    if (!GlobalVar.CallService_Core(_in.HOS_ID, inxml, ref his_rtnxml))
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
                        dataReturn.Msg = dtrev.Columns.Contains("CLJG") ? dtrev.Rows[0]["CLJG"].ToString() : "";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    DataTable dtrisreport = new DataTable();
                    try
                    {
                        dtrisreport = ds.Tables["RISREPORT"];
                        if (dtrisreport.Rows.Count == 0)
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "未查询到检查报告列表";
                            goto EndPoint;
                        }
                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到RISREPORT节点,请检查出参";
                        goto EndPoint;
                    }
                    _out.REPORT_ALL_NUM = dtrisreport.Rows.Count.ToString();
                    _out.REPORT_PRINT_NUM = dtrisreport.Rows.Count.ToString();
                    if (dtrisreport.Columns.Contains("AUDIT_FLAG"))
                    {
                        _out.REPORT_AUDIT_NUM = dtrisreport.Select("AUDIT_FLAG=0").Length.ToString();
                    }
                    _out.RISREPORT = new List<GETRISREPORT_M.RISREPORT>();
                    foreach (DataRow dr in dtrisreport.Rows)
                    {
                        GETRISREPORT_M.RISREPORT lis = new GETRISREPORT_M.RISREPORT();
                        lis.REPORT_SN = dtrisreport.Columns.Contains("REPORT_SN") ? FormatHelper.GetStr(dr["REPORT_SN"]) : "";
                        lis.REPORT_TYPE = dtrisreport.Columns.Contains("REPORT_TYPE") ? FormatHelper.GetStr(dr["REPORT_TYPE"]) : "";
                        lis.REPORT_DATE = dtrisreport.Columns.Contains("REPORT_DATE") ? FormatHelper.GetStr(dr["REPORT_DATE"]) : "";
                        lis.PRINT_FLAG = dtrisreport.Columns.Contains("PRINT_FLAG") ? FormatHelper.GetStr(dr["PRINT_FLAG"]) : lis.PRINT_FLAG;
                        lis.PRINT_TIME = dtrisreport.Columns.Contains("PRINT_TIME") ? FormatHelper.GetStr(dr["PRINT_TIME"]) : "";
                        lis.CHECK_DATE = dtrisreport.Columns.Contains("CHECK_DATE") ? FormatHelper.GetStr(dr["CHECK_DATE"]) : "";
                        lis.CHECK_DOC_NAME = dtrisreport.Columns.Contains("CHECK_DOC_NAME") ? FormatHelper.GetStr(dr["CHECK_DOC_NAME"]) : "";
                        lis.CHECK_DEPT_NAME = dtrisreport.Columns.Contains("CHECK_DEPT_NAME") ? FormatHelper.GetStr(dr["CHECK_DEPT_NAME"]) : "";
                        lis.APPLY_DATE = dtrisreport.Columns.Contains("APPLY_DATE") ? FormatHelper.GetStr(dr["APPLY_DATE"]) : "";
                        lis.APPLY_DEPT_NAME = dtrisreport.Columns.Contains("APPLY_DEPT_NAME") ? FormatHelper.GetStr(dr["APPLY_DEPT_NAME"]) : "";
                        lis.APPLY_DOC_NAME = dtrisreport.Columns.Contains("APPLY_DOC_NAME") ? FormatHelper.GetStr(dr["APPLY_DOC_NAME"]) : "";
                        lis.AUDIT_DATE = dtrisreport.Columns.Contains("AUDIT_DATE") ? FormatHelper.GetStr(dr["AUDIT_DATE"]) : "";
                        lis.AUDIT_DOC_NAME = dtrisreport.Columns.Contains("AUDIT_DOC_NAME") ? FormatHelper.GetStr(dr["AUDIT_DOC_NAME"]) : "";
                        lis.AUDIT_FLAG = dtrisreport.Columns.Contains("AUDIT_FLAG") ? FormatHelper.GetStr(dr["AUDIT_FLAG"]) : lis.AUDIT_FLAG;
                        lis.RESULT = dtrisreport.Columns.Contains("RESULT") ? FormatHelper.GetStr(dr["RESULT"]) : "";
                        lis.FINAL_REPORT = dtrisreport.Columns.Contains("FINAL_REPORT") ? FormatHelper.GetStr(dr["FINAL_REPORT"]) : "";
                        lis.NOTE = dtrisreport.Columns.Contains("NOTE") ? FormatHelper.GetStr(dr["NOTE"]) : "";
                        lis.DATA_TYPE = "4";
                        lis.REPORTDATA =dtrisreport.Columns.Contains("PDFURL") ? FormatHelper.GetStr(dr["PDFURL"]) : "";
                        _out.RISREPORT.Add(lis);
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
