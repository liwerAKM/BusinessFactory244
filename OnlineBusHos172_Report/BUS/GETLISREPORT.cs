using CommonModel;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using OnlineBusHos172_Report.Model;
using System.Linq;

namespace OnlineBusHos172_Report.BUS
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
                XmlDocument doc = QHXmlMode.GetBaseXml("GETLISREPORT", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REPORT_TYPE", string.IsNullOrEmpty(_in.REPORT_TYPE) ? "" : _in.REPORT_TYPE.Trim());

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                //System.IO.StreamReader stream = new System.IO.StreamReader("D:/test.txt");
                //string his_rtnxml = stream.ReadToEnd();
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
                    //_out.REPORT_ALL_NUM = dtlisreport.Rows.Count.ToString();
        

                    _out.LISREPORT = new List<GETLISREPORT_M.LisReport>();
                    foreach (DataRow dr in dtlisreport.Rows)
                    {
                        if (Convert.ToDateTime(dr["APPLY_DATE"])<DateTime.Today.AddDays(-7))//20230725 lq提需求 pyl修改
                        {
                            continue;
                        }


                        GETLISREPORT_M.LisReport lis = new GETLISREPORT_M.LisReport();
                        lis.REPORT_SN = dtlisreport.Columns.Contains("REPORT_SN") ? FormatHelper.GetStr(dr["REPORT_SN"]) : "";
                        lis.REPORT_TYPE = dtlisreport.Columns.Contains("REPORT_TYPE") ? FormatHelper.GetStr(dr["REPORT_TYPE"]) : "";
                        lis.REPORT_DATE = dtlisreport.Columns.Contains("REPORT_DATE") ? FormatHelper.GetStr(dr["REPORT_DATE"]) : "";
                        //2023.4.17新增检查中间表 增加打印标识校验
                        if (DB.Core.DbHelperMySQLZZJ.Exists(@"SELECT 1 FROM reportmx WHERE HOS_SN='" + lis.REPORT_SN + "'"))
                        {
                            lis.PRINT_FLAG = "1";
                            lis.PRINT_TIME = "1";
                        }
                        else
                        {
                            lis.PRINT_FLAG = dtlisreport.Columns.Contains("PRINT_FLAG") ? FormatHelper.GetStr(dr["PRINT_FLAG"]) : "0";
                            lis.PRINT_TIME = dtlisreport.Columns.Contains("PRINT_TIME") ? FormatHelper.GetStr(dr["PRINT_TIME"]) : "0";
                        }

                        
                        lis.TEST_DATE = dtlisreport.Columns.Contains("TEST_DATE") ? FormatHelper.GetStr(dr["TEST_DATE"]) : "";
                        lis.TEST_DOC_NAME = dtlisreport.Columns.Contains("TEST_DOC_NAME") ? FormatHelper.GetStr(dr["TEST_DOC_NAME"]) : "";
                        lis.TEST_DEPT_NAME = dtlisreport.Columns.Contains("TEST_DEPT_NAME") ? FormatHelper.GetStr(dr["TEST_DEPT_NAME"]) : "";
                        lis.AUDIT_DATE = dtlisreport.Columns.Contains("AUDIT_DATE") ? FormatHelper.GetStr(dr["AUDIT_DATE"]) : "";
                        lis.AUDIT_DOC_NAME = dtlisreport.Columns.Contains("AUDIT_DOC_NAME") ? FormatHelper.GetStr(dr["AUDIT_DOC_NAME"]) : "";
                        lis.AUDIT_FLAG = dtlisreport.Columns.Contains("AUDIT_FLAG") ? FormatHelper.GetStr(dr["AUDIT_FLAG"]) : lis.AUDIT_FLAG;
                        lis.DATA_TYPE = "4";
                        lis.REPORTDATA = dtlisreport.Columns.Contains("FILEPATH") ? FormatHelper.GetStr(dr["FILEPATH"]) : "";
                        _out.LISREPORT.Add(lis);
                    }
                    _out.REPORT_ALL_NUM = _out.LISREPORT.Count.ToString();
                    if (dtlisreport.Columns.Contains("AUDIT_FLAG"))
                    {
                        //_out.REPORT_AUDIT_NUM = dtlisreport.Select("AUDIT_FLAG=0").Length.ToString();
                        //_out.REPORT_PRINT_NUM = dtlisreport.Select("PRINT_FLAG=0 and AUDIT_FLAG=1").Length.ToString();

                        _out.REPORT_AUDIT_NUM= _out.LISREPORT.Where(a => a.AUDIT_FLAG.Equals("0")).Count().ToString();
                        _out.REPORT_PRINT_NUM = _out.LISREPORT.Where(a => a.AUDIT_FLAG.Equals("0")||a.AUDIT_FLAG.Equals("1")).Count().ToString();
                    }
                    else
                    {
                        _out.REPORT_PRINT_NUM = _out.LISREPORT.Where(a=>a.PRINT_FLAG.Equals("0")).Count().ToString();
                    }

                    if (_out.LISREPORT.Count == 0)
                    {
                        dataReturn.Code = -1;
                        dataReturn.Msg = "当前没有可打印报告";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                    }
                    else
                    {
                        dataReturn.Code = 0;
                        dataReturn.Msg = "SUCCESS";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                    }


                    

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

        /// <summary>
        /// 调用
        /// </summary>
        /// <param name="json_in"></param>
        /// <returns></returns>
        public static string A_GETLISREPORT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETLISREPORT_M.GETLISREPORT_IN _in = JSONSerializer.Deserialize<GETLISREPORT_M.GETLISREPORT_IN>(json_in);
                GETLISREPORT_M.GETLISREPORT_OUT _out = new GETLISREPORT_M.GETLISREPORT_OUT();
                string his_rtnxml = "";

                Hashtable hashtable = new Hashtable
                {
                    { "PatientID", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim() },
                    {"BeginDate",DateTime.Today.AddDays(-7).ToString() },
                    {"ClientCompanies","QH" }
                };
                string inxml = "PatientID:" + (string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim()) + "BeginDate:" + DateTime.Today.AddDays(-7).ToString() + "ClientCompanies:QH";
                string MethodName = "QueryLisSampleListBySelfPrint";
                if (!GlobalVar.CALLSERVICE_LIS( inxml, hashtable, MethodName,ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                _out.HIS_RTNXML = his_rtnxml;
                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ResponseContainerOfPatientObject/ResponseContentLisSample");
                    if(ds==null || ds.Tables.Count == 0)
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg ="调用lis接口异常";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    DataTable dtlisreport = new DataTable();
                    try
                    {
                        dtlisreport = ds.Tables["LisSample"];
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
                        dataReturn.Msg = "解析LIS出参失败,未找到LisSample节点,请检查出参";
                        goto EndPoint;
                    }
                    //_out.REPORT_ALL_NUM = dtlisreport.Rows.Count.ToString();


                    _out.LISREPORT = new List<GETLISREPORT_M.LisReport>();
                    foreach (DataRow dr in dtlisreport.Rows)
                    {

                        GETLISREPORT_M.LisReport lis = new GETLISREPORT_M.LisReport();
                        lis.REPORT_SN = dtlisreport.Columns.Contains("ALL_BARCODES") ? FormatHelper.GetStr(dr["ALL_BARCODES"]) : "";
                        lis.REPORT_TYPE ="";
                        lis.REPORT_DATE = dtlisreport.Columns.Contains("REPORT_DATE") ? FormatHelper.GetStr(dr["REPORT_DATE"]) : "";
                        lis.PRINT_FLAG = dtlisreport.Columns.Contains("PRINT_FLAG") ? FormatHelper.GetStr(dr["PRINT_FLAG"]) : "0";
                        lis.PRINT_TIME = "";
                       
                        lis.TEST_DATE = dtlisreport.Columns.Contains("TEST_DATE") ? FormatHelper.GetStr(dr["TEST_DATE"]) : "";
                        lis.TEST_DOC_NAME = dtlisreport.Columns.Contains("TEST_DOCTOR") ? FormatHelper.GetStr(dr["TEST_DOCTOR"]) : "";
                        lis.TEST_DEPT_NAME = "";
                        lis.AUDIT_DATE = dtlisreport.Columns.Contains("ORDER_DATE") ? FormatHelper.GetStr(dr["ORDER_DATE"]) : "";
                        lis.AUDIT_DOC_NAME = dtlisreport.Columns.Contains("VALIDATE_DOCTOR") ? FormatHelper.GetStr(dr["VALIDATE_DOCTOR"]) : "";
                        lis.AUDIT_FLAG = dtlisreport.Columns.Contains("AUDIT_FLAG") ? FormatHelper.GetStr(dr["AUDIT_FLAG"]) : lis.AUDIT_FLAG;
                        lis.DATA_TYPE = "4";
                        lis.REPORTDATA = dtlisreport.Columns.Contains("FILEPATH") ? FormatHelper.GetStr(dr["FILEPATH"]) : "";
                        _out.LISREPORT.Add(lis);
                    }
                    _out.REPORT_ALL_NUM = _out.LISREPORT.Count.ToString();
                    if (dtlisreport.Columns.Contains("AUDIT_FLAG"))
                    {
                        //_out.REPORT_AUDIT_NUM = dtlisreport.Select("AUDIT_FLAG=0").Length.ToString();
                        //_out.REPORT_PRINT_NUM = dtlisreport.Select("PRINT_FLAG=0 and AUDIT_FLAG=1").Length.ToString();

                        _out.REPORT_AUDIT_NUM = _out.LISREPORT.Where(a => a.AUDIT_FLAG.Equals("0")).Count().ToString();
                        _out.REPORT_PRINT_NUM = _out.LISREPORT.Where(a => a.AUDIT_FLAG.Equals("0") || a.AUDIT_FLAG.Equals("1")).Count().ToString();
                    }
                    else
                    {
                        _out.REPORT_PRINT_NUM = _out.LISREPORT.Where(a => a.PRINT_FLAG.Equals("0")).Count().ToString();
                    }

                    if (_out.LISREPORT.Count == 0)
                    {
                        dataReturn.Code = -1;
                        dataReturn.Msg = "当前没有可打印报告";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                    }
                    else
                    {
                        dataReturn.Code = 0;
                        dataReturn.Msg = "SUCCESS";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                    }




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
