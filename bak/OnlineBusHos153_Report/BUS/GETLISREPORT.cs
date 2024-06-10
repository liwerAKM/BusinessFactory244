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
                XmlDocument doc = QHXmlMode.GetBaseXml("GETLISREPORTSUQIAN", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", string.IsNullOrEmpty(_in.HOSPATID) ? _in.YLCARD_NO.Trim() : _in.HOSPATID.Trim());

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                //his_rtnxml = string.Format(@"<ROOT><HEADER><TYPE>GETLISREPORTSUQIAN</TYPE><CZLX>4</CZLX></HEADER><BODY><CLBZ>0</CLBZ><CLJG>SUCCESS</CLJG><LISREPORTLIST><LISREPORT><BASE64PDF /><FILEPATH>\\135.1.2.36\PDFReport\send2file\exportpic\6219696.pdf</FILEPATH><REPORT_ID>6219696</REPORT_ID><AUDIT_FLAG>1</AUDIT_FLAG><PRINT_FLAG>0</PRINT_FLAG><PRINT_COUNT>0</PRINT_COUNT></LISREPORT><LISREPORT><BASE64PDF /><FILEPATH>\\135.1.2.36\PDFReport\send2file\exportpic\6219608.pdf</FILEPATH><REPORT_ID>6219608</REPORT_ID><AUDIT_FLAG>1</AUDIT_FLAG><PRINT_FLAG>0</PRINT_FLAG><PRINT_COUNT>0</PRINT_COUNT></LISREPORT></LISREPORTLIST></BODY></ROOT>");
                if (GlobalVar.DoBussiness == "0")
                {
                    if (!GlobalVar.CallService_Core(_in.HOS_ID, inxml, ref his_rtnxml))
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
                    DataTable dtlisreport= new DataTable();
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
                        lis.REPORT_SN = dtlisreport.Columns.Contains("REPORT_ID") ? FormatHelper.GetStr(dr["REPORT_ID"]) : "";
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
                        lis.DATA_TYPE = "4";
                        lis.REPORTDATA =dtlisreport.Columns.Contains("FILEPATH") ? FormatHelper.GetStr(dr["FILEPATH"]) : "";
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
