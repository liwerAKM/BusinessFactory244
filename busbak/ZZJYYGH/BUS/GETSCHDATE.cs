using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZJYYGH.Model;
using Soft.Core;
using System.Xml;
using System.Data;

namespace ZZJYYGH.BUS
{
    class GETSCHDATE
    {
        public static string B_GETSCHDATE(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETSCHDATE_M.GETSCHDATE_IN _in = JSONSerializer.Deserialize<GETSCHDATE_M.GETSCHDATE_IN>(json_in);
                GETSCHDATE_M.GETSCHDATE_OUT _out = new GETSCHDATE_M.GETSCHDATE_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETSCHDATE", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USE_TYPE", string.IsNullOrEmpty(_in.USE_TYPE) ? "" : _in.USE_TYPE.Trim());
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
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
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
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtdept = ds.Tables["SCHDEPT"];
                        _out.DEPTLIST = new List<GETSCHDATE_M.SCHLIST>();
                        foreach (DataRow dr in dtdept.Rows)
                        {
                            GETSCHDATE_M.SCHLIST dept = new GETSCHDATE_M.SCHLIST();
                            dept.SCH_DATE = dtdept.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                            _out.DEPTLIST.Add(dept);
                        }
                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到SCHDEPT节点,请检查HIS出参";
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtdoc = ds.Tables["SCHDOC"];
                        _out.DOCLIST = new List<GETSCHDATE_M.SCHLIST>();
                        foreach (DataRow dr in dtdoc.Rows)
                        {
                            GETSCHDATE_M.SCHLIST doct = new GETSCHDATE_M.SCHLIST();
                            doct.SCH_DATE = dtdoc.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                            _out.DOCLIST.Add(doct);
                        }
                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到SCHDOC节点,请检查HIS出参";
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
