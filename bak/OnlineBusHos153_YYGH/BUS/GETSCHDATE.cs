using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos153_YYGH.Model;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos153_YYGH.BUS
{
    class GETSCHDATE
    {
        public static string B_GETSCHDATE(string json_in)
        {
            if (GlobalVar.DoBussiness == "0")
            {
                return UnDoBusiness(json_in);
            }
            else
            {
                return DoBusiness(json_in);
            }
        }
        public static string DoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETSCHDATE_M.GETSCHDATE_IN _in = JSONSerializer.Deserialize<GETSCHDATE_M.GETSCHDATE_IN>(json_in);
                GETSCHDATE_M.GETSCHDATE_OUT _out = new GETSCHDATE_M.GETSCHDATE_OUT();

                XmlDocument doc = new XmlDocument();
                string his_rtnxml = "";
                if (_in.USE_TYPE == "01")//获取科室普通号可预约日期
                {
                    doc = QHXmlMode.GetBaseXml("GETSCHDEPT", "0");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", _in.DEPT_CODE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", "01");
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, doc.InnerXml, ref his_rtnxml))
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
                            DataTable dtdept = ds.Tables["SCH"];
                            _out.SCHDEPTLIST = new List<GETSCHDATE_M.SCHLIST>();
                            foreach (DataRow dr in dtdept.Rows)
                            {
                                if (_out.SCHDEPTLIST.FindIndex(x => x.SCH_DATE.Equals(FormatHelper.GetStr(dr["SCH_DATE"]))) < 0)
                                {
                                    GETSCHDATE_M.SCHLIST sch = new GETSCHDATE_M.SCHLIST();
                                    sch.SCH_DATE = FormatHelper.GetStr(dr["SCH_DATE"]);
                                    _out.SCHDEPTLIST.Add(sch);
                                }
                            }
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                        catch (Exception ex)
                        {
                            dataReturn.Code = 5;
                            dataReturn.Msg = "解析HIS出参失败,未找到SCHDEPT节点,请检查HIS出参";
                            dataReturn.Param = ex.ToString();
                            goto EndPoint;
                        }
                    }
                    catch (Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "程序处理异常";
                        dataReturn.Param = ex.ToString();
                        goto EndPoint;
                    }
                }
                else if (_in.USE_TYPE == "05")//获取科室专家号可预约日期
                {
                    doc = QHXmlMode.GetBaseXml("GETSCHDOC", "0");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", _in.DEPT_CODE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", _in.DOC_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", "01");
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, doc.InnerXml, ref his_rtnxml))
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
                            DataTable dtdoc = ds.Tables["SCH"];
                            _out.SCHDOCLIST = new List<GETSCHDATE_M.SCHLIST>();
                            foreach (DataRow dr in dtdoc.Rows)
                            {
                                if (_out.SCHDOCLIST.FindIndex(x => x.SCH_DATE.Equals(FormatHelper.GetStr(dr["SCH_DATE"]))) < 0)
                                {
                                    GETSCHDATE_M.SCHLIST sch = new GETSCHDATE_M.SCHLIST();
                                    sch.SCH_DATE = FormatHelper.GetStr(dr["SCH_DATE"]);
                                    _out.SCHDOCLIST.Add(sch);
                                }
                            }
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                        catch (Exception ex)
                        {
                            dataReturn.Code = 5;
                            dataReturn.Msg = "解析HIS出参失败,未找到SCHDEPT节点,请检查HIS出参";
                            dataReturn.Param = ex.ToString();
                            goto EndPoint;
                        }
                    }
                    catch (Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "程序处理异常";
                        dataReturn.Param = ex.ToString();
                        goto EndPoint;
                    }

                }
                else if (_in.USE_TYPE == "08")//普通及专家
                {

                }
                else if (_in.USE_TYPE == "09")//用于选完科室后就获取科室和专家可预约日期
                {
                    #region 普通
                    doc = QHXmlMode.GetBaseXml("GETSCHDEPT", "0");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", _in.DEPT_CODE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", "01");
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, doc.InnerXml, ref his_rtnxml))
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
                            DataTable dtdept = ds.Tables["SCH"];
                            _out.SCHDEPTLIST = new List<GETSCHDATE_M.SCHLIST>();
                            foreach (DataRow dr in dtdept.Rows)
                            {
                                if (_out.SCHDEPTLIST.FindIndex(x => x.SCH_DATE.Equals(FormatHelper.GetStr(dr["SCH_DATE"]))) < 0)
                                {
                                    GETSCHDATE_M.SCHLIST sch = new GETSCHDATE_M.SCHLIST();
                                    sch.SCH_DATE = FormatHelper.GetStr(dr["SCH_DATE"]);
                                    _out.SCHDEPTLIST.Add(sch);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                    #region 专家
                    doc = QHXmlMode.GetBaseXml("GETSCHDOC", "0");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", _in.DEPT_CODE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", _in.DOC_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", "01");
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, doc.InnerXml, ref his_rtnxml))
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
                            DataTable dtdoc = ds.Tables["SCH"];
                            _out.SCHDOCLIST = new List<GETSCHDATE_M.SCHLIST>();
                            foreach (DataRow dr in dtdoc.Rows)
                            {
                                if (_out.SCHDOCLIST.FindIndex(x => x.SCH_DATE.Equals(FormatHelper.GetStr(dr["SCH_DATE"]))) < 0)
                                {
                                    GETSCHDATE_M.SCHLIST sch = new GETSCHDATE_M.SCHLIST();
                                    sch.SCH_DATE = FormatHelper.GetStr(dr["SCH_DATE"]);
                                    _out.SCHDOCLIST.Add(sch);
                                }
                            }
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                    foreach (GETSCHDATE_M.SCHLIST sch in _out.SCHDOCLIST)
                    {
                        if (_out.SCHDEPTLIST.FindIndex(x => x.SCH_DATE.Equals(FormatHelper.GetStr(sch.SCH_DATE))) < 0)
                        {
                            _out.SCHDEPTLIST.Add(sch);
                        }
                    }
                    _out.SCHDEPTLIST.Sort();
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
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
        public static string UnDoBusiness(string json_in)
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

                DataTable dt_dept = new Plat.MySQLDAL.BaseFunction().Query("select * from dept_info where dept_code='"+_in.DEPT_CODE+"' and HOS_ID='153'");
                if (dt_dept.Rows.Count > 0)
                {
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NOTE", dt_dept.Rows[0]["NOTE"].ToString());

                }
                else
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "未获取到大科名称";
                    goto EndPoint;
                }

                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", "4");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", "");
                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
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
                        DataTable dtdept = ds.Tables["SCHDEPT"];
                        _out.SCHDEPTLIST = new List<GETSCHDATE_M.SCHLIST>();
                        foreach (DataRow dr in dtdept.Rows)
                        {
                            GETSCHDATE_M.SCHLIST dept = new GETSCHDATE_M.SCHLIST();
                            dept.SCH_DATE = dtdept.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                            dept.WEEK_DAY= dtdept.Columns.Contains("WEEK") ? dr["WEEK"].ToString() : "";
                            if (_out.SCHDEPTLIST.FindIndex(x => x.SCH_DATE.Equals(dept.SCH_DATE))<0)
                            {
                                _out.SCHDEPTLIST.Add(dept);
                            }
                        }
                    }
                    catch
                    {
                        //dataReturn.Code = 5;
                        //dataReturn.Msg = "解析HIS出参失败,未找到SCHDEPT节点,请检查HIS出参";
                        //goto EndPoint;
                    }
                    try
                    {
                        DataTable dtdoc = ds.Tables["SCHDOC"];
                        _out.SCHDOCLIST = new List<GETSCHDATE_M.SCHLIST>();
                        foreach (DataRow dr in dtdoc.Rows)
                        {
                            GETSCHDATE_M.SCHLIST doct = new GETSCHDATE_M.SCHLIST();
                            doct.SCH_DATE = dtdoc.Columns.Contains("SCH_DATE") ? dr["SCH_DATE"].ToString() : "";
                            doct.WEEK_DAY = dtdoc.Columns.Contains("WEEK") ? dr["WEEK"].ToString() : "";
                            if (_out.SCHDOCLIST.FindIndex(x => x.SCH_DATE.Equals(doct.SCH_DATE)) < 0)
                            {
                                _out.SCHDOCLIST.Add(doct);
                            }
                        }
                    }
                    catch
                    {
                        //dataReturn.Code = 5;
                        //dataReturn.Msg = "解析HIS出参失败,未找到SCHDOC节点,请检查HIS出参";
                        //goto EndPoint;
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
