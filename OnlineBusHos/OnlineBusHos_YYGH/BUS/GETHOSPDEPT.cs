using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
namespace OnlineBusHos_YYGH.BUS
{
    class GETHOSPDEPT
    {
        public static string B_GETHOSPDEPT(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETHOSPDEPT_M.GETHOSPDEPT_IN _in = JSONSerializer.Deserialize<Model.GETHOSPDEPT_M.GETHOSPDEPT_IN>(json_in);
                Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT _out = new Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETHOSPDEPT", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USE_TYPE", string.IsNullOrEmpty(_in.USE_TYPE) ? "" : _in.USE_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_TYPE", string.IsNullOrEmpty(_in.FILT_TYPE) ? "" : _in.FILT_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_VALUE", string.IsNullOrEmpty(_in.FILT_VALUE) ? "" : _in.FILT_VALUE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "RETURN_TYPE", string.IsNullOrEmpty(_in.RETURN_TYPE) ? "" : _in.RETURN_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGEINDEX", string.IsNullOrEmpty(_in.PAGEINDEX) ? "" : _in.PAGEINDEX.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGESIZE", string.IsNullOrEmpty(_in.PAGESIZE) ? "" : _in.PAGESIZE.Trim());

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
                        DataTable dtdept = ds.Tables["DEPT"];
                        _out.DEPTLIST = new List<Model.GETHOSPDEPT_M.DEPT>();
                        foreach (DataRow dr in dtdept.Rows)
                        {
                            Model.GETHOSPDEPT_M.DEPT dept = new Model.GETHOSPDEPT_M.DEPT();
        
                            dept.DEPT_CODE = dtdept.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                            dept.DEPT_NAME = dtdept.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                            dept.DEPT_INTRO = dtdept.Columns.Contains("DEPT_INTRO") ? dr["DEPT_INTRO"].ToString() : "";
                            dept.DEPT_URL = dtdept.Columns.Contains("DEPT_URL") ? dr["DEPT_URL"].ToString() : "";
                            dept.DEPT_ORDER = dtdept.Columns.Contains("DEPT_ORDER") ? dr["DEPT_ORDER"].ToString() : "";
                            dept.DEPT_TYPE = dtdept.Columns.Contains("DEPT_TYPE") ? dr["DEPT_TYPE"].ToString() : "";
                            dept.DEPT_ADDRESS = dtdept.Columns.Contains("DEPT_ADDRESS") ? dr["DEPT_ADDRESS"].ToString() : "";
                            _out.DEPTLIST.Add(dept);
                        }
                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到DEPTLIST节点,请检查HIS出参";
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
                    dataReturn.Param = ex.Message;
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}
