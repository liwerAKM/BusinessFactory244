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
    class GETSCHTIME
    {
        public static string B_GETSCHTIME(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETSCHDATE_M.GETSCHDATE_IN _in = JSONSerializer.Deserialize<Model.GETSCHDATE_M.GETSCHDATE_IN>(json_in);
                Model.GETSCHDATE_M.GETSCHDATE_OUT _out = new Model.GETSCHDATE_M.GETSCHDATE_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETSCHINFO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());

                if (_in.USE_TYPE == "09")
                {
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", "");//DateTime.Now.ToString("yyyy-MM-dd")
                }
                else
                {
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", "");
                }
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", "4");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", "");
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
                        _out.SCHDEPTLIST = new List<Model.GETSCHDATE_M.SCHLIST>();
                        foreach (DataRow dr in dtdept.Rows)
                        {
                            Model.GETSCHDATE_M.SCHLIST dept = new Model.GETSCHDATE_M.SCHLIST();
                            dept.SCH_DATE = dtdept.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                            if (_out.SCHDEPTLIST.FindIndex(x => x.SCH_DATE.Equals(dept.SCH_DATE)) < 0)
                            {
                                _out.SCHDEPTLIST.Add(dept);
                            }
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        DataTable dtdoc = ds.Tables["DOC"];
                        _out.SCHDOCLIST = new List<Model.GETSCHDATE_M.SCHLIST>();
                        foreach (DataRow dr in dtdoc.Rows)
                        {
                            Model.GETSCHDATE_M.SCHLIST doct = new Model.GETSCHDATE_M.SCHLIST();
                            doct.SCH_DATE = dtdoc.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                            if (_out.SCHDOCLIST.FindIndex(x => x.SCH_DATE.Equals(doct.SCH_DATE)) < 0)
                            {
                                _out.SCHDOCLIST.Add(doct);
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
