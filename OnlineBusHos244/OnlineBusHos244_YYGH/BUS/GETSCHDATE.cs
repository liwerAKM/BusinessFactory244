using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos244_YYGH.BUS
{
    class GETSCHDATE
    {
        public static string B_GETSCHDATE(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            string[] weekdays = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

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
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USE_TYPE", string.IsNullOrEmpty(_in.USE_TYPE) ? "" : _in.USE_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", "01");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", "");

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {

                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }

                _out.HIS_RTNXML = his_rtnxml;
                DataTable SCHLISTDEPT = new DataTable();
                DataTable SCHLISTDOC = new DataTable();
                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    try
                    {
                        SCHLISTDEPT = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/DEPTLIST").Tables[0];
                    
                    }
                    catch
                    { }
                    try
                    {
                        SCHLISTDOC = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/DOCLIST").Tables[0];
                    }
                    catch
                    {
                    }

                    _out.SCHDEPTLIST = new List<Model.GETSCHDATE_M.SCHLIST>();
                    _out.SCHDOCLIST = new List<Model.GETSCHDATE_M.SCHLIST>();

                    if (SCHLISTDEPT.Rows.Count > 0)
                    {
                        DataView dv = SCHLISTDEPT.DefaultView;
                        SCHLISTDEPT = dv.ToTable("SCHLISTDEPT", true, "SCH_DATE");
                    }
                    if (SCHLISTDOC.Rows.Count > 0)
                    {
                        DataView dv = SCHLISTDOC.DefaultView;
                        SCHLISTDOC = dv.ToTable("SCHLISTDOC", true, "SCH_DATE");
                    }
                    try
                    {
                        if (SCHLISTDEPT.Rows.Count > 0)
                        {
                            foreach (DataRow row in SCHLISTDEPT.Rows)
                            {
                                Model.GETSCHDATE_M.SCHLIST dept = new Model.GETSCHDATE_M.SCHLIST();
                                dept.SCH_DATE = row["SCH_DATE"].ToString();
                                dept.WEEK_DAY = weekdays[(int)Convert.ToDateTime(row["SCH_DATE"].ToString()).DayOfWeek];
                                if (_out.SCHDEPTLIST.FindIndex(x => x.SCH_DATE.Equals(dept.SCH_DATE)) < 0)
                                {
                                    _out.SCHDEPTLIST.Add(dept);
                                }
                            }
                        }
                        if (SCHLISTDOC.Rows.Count > 0)
                        {
                            foreach (DataRow row in SCHLISTDOC.Rows)
                            {
                                Model.GETSCHDATE_M.SCHLIST doct = new Model.GETSCHDATE_M.SCHLIST();
                                doct.SCH_DATE = row["SCH_DATE"].ToString();
                                doct.WEEK_DAY = weekdays[(int)Convert.ToDateTime(row["SCH_DATE"].ToString()).DayOfWeek];
                                if (_out.SCHDOCLIST.FindIndex(x => x.SCH_DATE.Equals(doct.SCH_DATE)) < 0)
                                {
                                    _out.SCHDOCLIST.Add(doct);
                                }
                            }
                        }
                    }
                    catch
                    { }

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
