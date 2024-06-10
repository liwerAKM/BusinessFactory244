using CommonModel;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using OnlineBusHos153_Report.Model;

namespace OnlineBusHos153_Report.BUS
{
    class GETPATHOLOGYREPORT
    {
        public static string B_GETPATHOLOGYEPORT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETPATHOLOGYREPORT_M.GETPATHOLOGYREPORT_IN _in = JSONSerializer.Deserialize<GETPATHOLOGYREPORT_M.GETPATHOLOGYREPORT_IN>(json_in);
                GETPATHOLOGYREPORT_M.GETPATHOLOGYREPORT_OUT _out = new GETPATHOLOGYREPORT_M.GETPATHOLOGYREPORT_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GetPisReportInfo", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CodeValue", _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CodeType", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "StartDate", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "EndDate", "");


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
                    if (!GlobalVar.CallService_Core(_in.HOS_ID, inxml, ref his_rtnxml))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml;
                        goto EndPoint;
                    }
                }
                //his_rtnxml = "<ROOT><HEADER><TYPE>GetPisReportInfo</TYPE><CZLX>1</CZLX></HEADER><BODY><CLBZ>0</CLBZ><CLJG>SUCCESS</CLJG><RISREPORTLIST><RISREPORT><REPORTNO>T202200590</REPORTNO><PATIENTNAME>张永侠</PATIENTNAME><REPORTURL></REPORTURL><REPORTTIME>2022-01-25 00:00:00</REPORTTIME><REPORTSTATUS>已审核</REPORTSTATUS><ISALLOWPRINT>0</ISALLOWPRINT><UNPRINTABLEREASON>此报告已经被打印过，不能再次打印</UNPRINTABLEREASON></RISREPORT><RISREPORT><REPORTNO>V20220511</REPORTNO><PATIENTNAME>张永侠</PATIENTNAME><REPORTURL>http://135.1.2.37/pathimages/pdfbg/2022-01/V20220511CG120220124165648.pdf</REPORTURL><REPORTTIME>2022-01-24 00:00:00</REPORTTIME><REPORTSTATUS>已审核</REPORTSTATUS><ISALLOWPRINT>1</ISALLOWPRINT><UNPRINTABLEREASON></UNPRINTABLEREASON></RISREPORT></RISREPORTLIST></BODY></ROOT>";
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

                    DataTable dtpatholotyreport = new DataTable();
                    try
                    {
                        dtpatholotyreport = ds.Tables["RISREPORT"];
                        if (dtpatholotyreport.Rows.Count == 0)
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "未查询到检验报告列表";
                            goto EndPoint;
                        }
                        else if (dtpatholotyreport.Rows.Count == 1)
                        {
                            if (dtpatholotyreport.Rows[0]["IsAllowPrint"].ToString() != "1")
                            {
                                dataReturn.Code = 1;
                                dataReturn.Msg = dtpatholotyreport.Rows[0]["UNPRINTABLEREASON"].ToString();
                                dataReturn.Param = JSONSerializer.Serialize(_out);
                                goto EndPoint;
                            }
                        }
                        DataView dv = dtpatholotyreport.DefaultView;
                        dv.RowFilter = "IsAllowPrint=1";
                        dtpatholotyreport = dv.ToTable();
                        dtpatholotyreport.Columns["ReportNo"].ColumnName = "REPORT_SN";
                        dtpatholotyreport.Columns["ReportTime"].ColumnName = "REPORT_DATE";
                        dtpatholotyreport.Columns["ReportUrl"].ColumnName = "REPORTDATA";
                        dtpatholotyreport.Columns["ReportStatus"].ColumnName = "AUDIT_FLAG";

                        dtpatholotyreport.Columns.Add("DATA_TYPE", typeof(string));
                        foreach (DataRow dr in dtpatholotyreport.Rows)
                        {
                            dr["AUDIT_FLAG"] = FormatHelper.GetStr(dr["AUDIT_FLAG"]) == "已审核" ? "1" : "0";
                            dr["DATA_TYPE"] = FormatHelper.GetStr("2");
                        }

                    }
                    catch(Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到RISREPORT节点,请检查出参";
                        dataReturn.Param = ex.ToString();

                        goto EndPoint;
                    }
                    _out.REPORT_ALL_NUM = dtpatholotyreport.Rows.Count.ToString();
                    _out.REPORT_PRINT_NUM = dtpatholotyreport.Select("IsAllowPrint=1").Length.ToString();
                    if (dtpatholotyreport.Columns.Contains("AUDIT_FLAG"))
                    {
                        _out.REPORT_AUDIT_NUM = dtpatholotyreport.Select("AUDIT_FLAG=0").Length.ToString();
                    }
                    _out.PATHOLOGYREPORT = new List<GETPATHOLOGYREPORT_M.PATHOLOGYREPORT>() ;
                    foreach (DataRow dr in dtpatholotyreport.Rows)
                    {
                        GETPATHOLOGYREPORT_M.PATHOLOGYREPORT pathology = new GETPATHOLOGYREPORT_M.PATHOLOGYREPORT();
                        pathology.REPORT_SN = dtpatholotyreport.Columns.Contains("REPORT_SN") ? FormatHelper.GetStr(dr["REPORT_SN"]) : "";
                        pathology.REPORT_TYPE = dtpatholotyreport.Columns.Contains("REPORT_TYPE") ? FormatHelper.GetStr(dr["REPORT_TYPE"]) : "";
                        pathology.REPORT_DATE = dtpatholotyreport.Columns.Contains("REPORT_DATE") ? FormatHelper.GetStr(dr["REPORT_DATE"]) : "";
                        pathology.PRINT_FLAG = dtpatholotyreport.Columns.Contains("PRINT_FLAG") ? FormatHelper.GetStr(dr["PRINT_FLAG"]) : pathology.PRINT_FLAG;
                        pathology.PRINT_TIME = dtpatholotyreport.Columns.Contains("PRINT_TIME") ? FormatHelper.GetStr(dr["PRINT_TIME"]) : "";
                        pathology.CHECK_DATE = dtpatholotyreport.Columns.Contains("CHECK_DATE") ? FormatHelper.GetStr(dr["CHECK_DATE"]) : "";
                        pathology.CHECK_DOC_NAME = dtpatholotyreport.Columns.Contains("CHECK_DOC_NAME") ? FormatHelper.GetStr(dr["CHECK_DOC_NAME"]) : "";
                        pathology.CHECK_DEPT_NAME = dtpatholotyreport.Columns.Contains("CHECK_DEPT_NAME") ? FormatHelper.GetStr(dr["CHECK_DEPT_NAME"]) : "";
                        pathology.APPLY_DATE = dtpatholotyreport.Columns.Contains("APPLY_DATE") ? FormatHelper.GetStr(dr["APPLY_DATE"]) : "";
                        pathology.APPLY_DEPT_NAME = dtpatholotyreport.Columns.Contains("APPLY_DEPT_NAME") ? FormatHelper.GetStr(dr["APPLY_DEPT_NAME"]) : "";
                        pathology.APPLY_DOC_NAME = dtpatholotyreport.Columns.Contains("APPLY_DOC_NAME") ? FormatHelper.GetStr(dr["APPLY_DOC_NAME"]) : "";
                        pathology.AUDIT_DATE = dtpatholotyreport.Columns.Contains("AUDIT_DATE") ? FormatHelper.GetStr(dr["AUDIT_DATE"]) : "";
                        pathology.AUDIT_DOC_NAME = dtpatholotyreport.Columns.Contains("AUDIT_DOC_NAME") ? FormatHelper.GetStr(dr["AUDIT_DOC_NAME"]) : "";
                        pathology.AUDIT_FLAG = dtpatholotyreport.Columns.Contains("AUDIT_FLAG") ? FormatHelper.GetStr(dr["AUDIT_FLAG"]) : pathology.AUDIT_FLAG;
                        pathology.RESULT = dtpatholotyreport.Columns.Contains("RESULT") ? FormatHelper.GetStr(dr["RESULT"]) : "";
                        pathology.FINAL_REPORT = dtpatholotyreport.Columns.Contains("FINAL_REPORT") ? FormatHelper.GetStr(dr["FINAL_REPORT"]) : "";
                        pathology.NOTE = dtpatholotyreport.Columns.Contains("NOTE") ? FormatHelper.GetStr(dr["NOTE"]) : "";
                        pathology.DATA_TYPE = dtpatholotyreport.Columns.Contains("DATA_TYPE") ? FormatHelper.GetStr(dr["DATA_TYPE"]) : "1";
                        pathology.REPORTDATA = dtpatholotyreport.Columns.Contains("REPORTDATA") ? FormatHelper.GetStr(dr["REPORTDATA"]) : "";
                        _out.PATHOLOGYREPORT.Add(pathology);
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                    dataReturn.Param = ex.ToString();

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
