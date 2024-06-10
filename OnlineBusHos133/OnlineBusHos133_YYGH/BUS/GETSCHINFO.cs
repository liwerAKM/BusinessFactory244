using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
namespace OnlineBusHos133_YYGH.BUS
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
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtschdept = new DataTable();
                        DataTable dtschdoc = new DataTable();
                        try
                        {
                            dtschdept = ds.Tables["DEPT"];
                        }
                        catch { }

                        try
                        {
                            dtschdoc = ds.Tables["DOC"];
                        }
                        catch { }


                        _out.DEPTLIST = new List<Model.GETSCHINFO_M.DEPT>();
                        _out.DOCLIST = new List<Model.GETSCHINFO_M.DOC>();

                        if (dtschdept != null && dtschdept.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtschdept.Rows)
                            {

                                Model.GETSCHINFO_M.DEPT dept = new Model.GETSCHINFO_M.DEPT();
                                dept.DEPT_CODE = dtschdept.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                                dept.DEPT_NAME = dtschdept.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                                dept.DOC_NO = dtschdept.Columns.Contains("DOC_NO") ? dr["DOC_NO"].ToString() : "";
                                dept.DOC_NAME = dtschdept.Columns.Contains("DOC_NAME") ? dr["DOC_NAME"].ToString() : "";
                                dept.GH_FEE = dtschdept.Columns.Contains("GH_FEE") ? dr["GH_FEE"].ToString() : "";
                                dept.ZL_FEE = dtschdept.Columns.Contains("ZL_FEE") ? dr["ZL_FEE"].ToString() : "";
                                dept.SCH_TYPE = dtschdept.Columns.Contains("SCH_TYPE") ? dr["SCH_TYPE"].ToString() : "";
                                dept.SCH_DATE = dtschdept.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                                dept.SCH_TIME = dtschdept.Columns.Contains("SCH_TIME") ? dr["SCH_TIME"].ToString() : "";
                                dept.PERIOD_START = dtschdept.Columns.Contains("PERIOD_START") ? dr["PERIOD_START"].ToString() : "";
                                dept.PERIOD_END = dtschdept.Columns.Contains("PERIOD_END") ? dr["PERIOD_END"].ToString() : "";
                                dept.CAN_WAIT = dtschdept.Columns.Contains("CAN_WAIT") ? dr["CAN_WAIT"].ToString() : "";
                                dept.REGISTER_TYPE = dtschdept.Columns.Contains("REGISTER_TYPE") ? dr["REGISTER_TYPE"].ToString() : "";
                                dept.REGISTER_TYPE_NAME = dtschdept.Columns.Contains("REGISTER_TYPE_NAME") ? dr["REGISTER_TYPE_NAME"].ToString() : "";
                                dept.STATUS = dtschdept.Columns.Contains("STATUS") ? dr["STATUS"].ToString() : "";
                                dept.COUNT_REM = dtschdept.Columns.Contains("COUNT_REM") ? dr["COUNT_REM"].ToString() : "";
                                dept.YB_CODE = dtschdept.Columns.Contains("YB_CODE") ? dr["YB_CODE"].ToString() : "";
                                dept.PRO_TITLE = dtschdept.Columns.Contains("PRO_TITLE") ? dr["PRO_TITLE"].ToString() : "";

                                _out.DEPTLIST.Add(dept);
                            }
                        }

                        if (dtschdoc != null && dtschdoc.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtschdoc.Rows)
                            {
                                Model.GETSCHINFO_M.DOC doctor = new Model.GETSCHINFO_M.DOC();
                                doctor.DOC_NO = dtschdoc.Columns.Contains("DOC_NO") ? dr["DOC_NO"].ToString() : "";
                                doctor.DOC_NAME = dtschdoc.Columns.Contains("DOC_NAME") ? dr["DOC_NAME"].ToString() : "";
                                doctor.GH_FEE = dtschdoc.Columns.Contains("GH_FEE") ? dr["GH_FEE"].ToString() : "";
                                doctor.ZL_FEE = dtschdoc.Columns.Contains("ZL_FEE") ? dr["ZL_FEE"].ToString() : "";
                                doctor.SCH_TYPE = dtschdoc.Columns.Contains("SCH_TYPE") ? dr["SCH_TYPE"].ToString() : "";
                                doctor.SCH_DATE = dtschdoc.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                                doctor.SCH_TIME = dtschdoc.Columns.Contains("SCH_TIME") ? dr["SCH_TIME"].ToString() : "";
                                doctor.PERIOD_START = dtschdoc.Columns.Contains("PERIOD_START") ? dr["PERIOD_START"].ToString() : "";
                                doctor.PERIOD_END = dtschdoc.Columns.Contains("PERIOD_END") ? dr["PERIOD_END"].ToString() : "";
                                doctor.CAN_WAIT = dtschdoc.Columns.Contains("CAN_WAIT") ? dr["CAN_WAIT"].ToString() : "";

                                doctor.REGISTER_TYPE = dtschdoc.Columns.Contains("REGISTER_TYPE") ? dr["REGISTER_TYPE"].ToString() : "";
                                doctor.REGISTER_TYPE_NAME = dtschdoc.Columns.Contains("REGISTER_TYPE_NAME") ? dr["REGISTER_TYPE_NAME"].ToString() : "";
                                doctor.STATUS = dtschdoc.Columns.Contains("STATUS") ? dr["STATUS"].ToString() : "";
                                doctor.COUNT_REM = dtschdoc.Columns.Contains("COUNT_REM") ? dr["COUNT_REM"].ToString() : "";
                                doctor.YB_CODE = dtschdoc.Columns.Contains("YB_CODE") ? dr["YB_CODE"].ToString() : "";
                                doctor.PRO_TITLE = dtschdoc.Columns.Contains("PRO_TITLE") ? dr["PRO_TITLE"].ToString() : "";

                                _out.DOCLIST.Add(doctor);
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = ex.Message;
                        dataReturn.Param = JSONSerializer.Serialize(_out);
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
