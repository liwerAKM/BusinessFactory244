using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
namespace OnlineBusHos172_YYGH.BUS
{
    class GETSCHINFO
    {
        public static string B_GETSCHINFO(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETSCHINFO_M.GETSCHINFO_IN _in = JSONSerializer.Deserialize<Model.GETSCHINFO_M.GETSCHINFO_IN>(json_in);
                Model.GETSCHINFO_M.GETSCHINFO_OUT _out = new Model.GETSCHINFO_M.GETSCHINFO_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETSCHINFO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", string.IsNullOrEmpty(_in.SCH_TYPE) ? "" : _in.SCH_TYPE.Trim());

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
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtsch = ds.Tables["SCH"];
                        bool flag = true;
                        if (DateTime.Now > Convert.ToDateTime("17:00:00"))//晚上
                        {
                            flag = false;
                        }
                        if (dtsch.Rows.Count > 0)
                        {
                            if (flag)
                            {
                                dtsch = System.Data.DataTableExtensions.CopyToDataTable(dtsch.Select("SCH_TIME <>'前夜' AND SCH_TIME <>'后夜'"));
                            }
                            else
                            {
                                dtsch = System.Data.DataTableExtensions.CopyToDataTable(dtsch.Select("SCH_TIME <>'上午' AND SCH_TIME <>'下午' AND SCH_TIME <>'白天' "));
                            }
                        }
                        _out.DEPTLIST = new List<Model.GETSCHINFO_M.DEPT>();
                        _out.DOCLIST = new List<Model.GETSCHINFO_M.DOC>();
                        if (!string.IsNullOrEmpty(_in.SCH_DATE)&& dtsch!=null && dtsch.Rows.Count>0)//当传日期时只取当日
                        {
                            dtsch = DataTableExtensions.CopyToDataTable(dtsch.Select("SCH_DATE='" + _in.SCH_DATE + "'"));
                        }

                        foreach (DataRow dr in dtsch.Rows)
                        {
                            if (dr["SCH_TYPE"].ToString().Trim() == "1")
                            {
                                Model.GETSCHINFO_M.DEPT dept = new Model.GETSCHINFO_M.DEPT();
                                dept.DEPT_CODE = dtsch.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                                dept.DEPT_NAME = dtsch.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                                dept.DOC_NO = dtsch.Columns.Contains("DOC_NO") ? dr["DOC_NO"].ToString() : "";
                                dept.DOC_NAME = dtsch.Columns.Contains("DOC_NAME") ? dr["DOC_NAME"].ToString() : "";
                                dept.GH_FEE = dtsch.Columns.Contains("GH_FEE") ? dr["GH_FEE"].ToString() : "";
                                dept.ZL_FEE = dtsch.Columns.Contains("ZL_FEE") ? dr["ZL_FEE"].ToString() : "";
                                dept.SCH_TYPE = dtsch.Columns.Contains("SCH_TYPE") ? dr["SCH_TYPE"].ToString() : "";
                                dept.SCH_DATE = dtsch.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                                dept.SCH_TIME = dtsch.Columns.Contains("SCH_TIME") ? dr["SCH_TIME"].ToString() : "";
                                dept.PERIOD_START = dtsch.Columns.Contains("PERIOD_START") ? dr["PERIOD_START"].ToString() : "";
                                dept.PERIOD_END = dtsch.Columns.Contains("PERIOD_END") ? dr["PERIOD_END"].ToString() : "";
                                dept.CAN_WAIT = dtsch.Columns.Contains("CAN_WAIT") ? dr["CAN_WAIT"].ToString() : "";
                                dept.REGISTER_TYPE = dtsch.Columns.Contains("REGISTER_TYPE") ? dr["REGISTER_TYPE"].ToString() : "";
                                dept.REGISTER_TYPE_NAME = dtsch.Columns.Contains("REGISTER_TYPE_NAME") ? dr["REGISTER_TYPE_NAME"].ToString() : "";
                                dept.STATUS = dtsch.Columns.Contains("STATUS") ? dr["STATUS"].ToString() : "";
                                dept.COUNT_REM = dtsch.Columns.Contains("COUNT_REM") ? dr["COUNT_REM"].ToString() : "";
                                dept.YB_CODE = dtsch.Columns.Contains("YB_CODE") ? dr["YB_CODE"].ToString() : "";
                                dept.PRO_TITLE = dtsch.Columns.Contains("PRO_TITLE") ? dr["PRO_TITLE"].ToString() : "";

                                _out.DEPTLIST.Add(dept);
                            }
                            else
                            {
                                Model.GETSCHINFO_M.DOC doctor = new Model.GETSCHINFO_M.DOC();
                                doctor.DOC_NO = dtsch.Columns.Contains("DOC_NO") ? dr["DOC_NO"].ToString() : "";
                                doctor.DOC_NAME = dtsch.Columns.Contains("DOC_NAME") ? dr["DOC_NAME"].ToString() : "";
                                doctor.GH_FEE = dtsch.Columns.Contains("GH_FEE") ? dr["GH_FEE"].ToString() : "";
                                doctor.ZL_FEE = dtsch.Columns.Contains("ZL_FEE") ? dr["ZL_FEE"].ToString() : "";
                                doctor.SCH_TYPE = dtsch.Columns.Contains("SCH_TYPE") ? dr["SCH_TYPE"].ToString() : "";
                                doctor.SCH_DATE = dtsch.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                                doctor.SCH_TIME = dtsch.Columns.Contains("SCH_TIME") ? dr["SCH_TIME"].ToString() : "";
                                doctor.PERIOD_START = dtsch.Columns.Contains("PERIOD_START") ? dr["PERIOD_START"].ToString() : "";
                                doctor.PERIOD_END = dtsch.Columns.Contains("PERIOD_END") ? dr["PERIOD_END"].ToString() : "";
                                doctor.CAN_WAIT = dtsch.Columns.Contains("CAN_WAIT") ? dr["CAN_WAIT"].ToString() : "";

                                doctor.REGISTER_TYPE = dtsch.Columns.Contains("REGISTER_TYPE") ? dr["REGISTER_TYPE"].ToString() : "";
                                doctor.REGISTER_TYPE_NAME = dtsch.Columns.Contains("REGISTER_TYPE_NAME") ? dr["REGISTER_TYPE_NAME"].ToString() : "";
                                doctor.STATUS = dtsch.Columns.Contains("STATUS") ? dr["STATUS"].ToString() : "";
                                doctor.COUNT_REM = dtsch.Columns.Contains("COUNT_REM") ? dr["COUNT_REM"].ToString() : "";
                                doctor.YB_CODE = dtsch.Columns.Contains("YB_CODE") ? dr["YB_CODE"].ToString() : "";
                                doctor.PRO_TITLE = dtsch.Columns.Contains("PRO_TITLE") ? dr["PRO_TITLE"].ToString() : "";

                                _out.DOCLIST.Add(doctor);
                            }
                        }
                    }
                    catch
                    {
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
