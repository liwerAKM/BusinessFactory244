using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
namespace OnlineBusHos324_YYGH.BUS
{
    class GETSCHPERIOD
    {
        public static string B_GETSCHPERIOD(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETSCHPERIOD_M.GETSCHPERIOD_IN _in = JSONSerializer.Deserialize<Model.GETSCHPERIOD_M.GETSCHPERIOD_IN>(json_in);
                Model.GETSCHPERIOD_M.GETSCHPERIOD_OUT _out = new Model.GETSCHPERIOD_M.GETSCHPERIOD_OUT();
                //XmlDocument doc = QHXmlMode.GetBaseXml("GETSCHPERIOD", "1");
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", string.IsNullOrEmpty(_in.SCH_TYPE) ? "" : _in.SCH_TYPE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TIME", string.IsNullOrEmpty(_in.SCH_TIME) ? "" : _in.SCH_TIME.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERY_TYPE", "01");


                XmlDocument doc = QHXmlMode.GetBaseXml("GETSCHINFO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", string.IsNullOrEmpty(_in.SCH_TYPE) ? "" : _in.SCH_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", "");



                string inxml = doc.InnerXml;
                string his_rtnxml = "固定出参";
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


                        DataTable dtDept = new DataTable();
                        DataTable dtDoc = new DataTable();
                        string COUNT_REM = "0";
                        if (string.IsNullOrEmpty(_in.DOC_NO))//普通号
                        {
                            try
                            {
                                dtDept = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/DEPTLIST").Tables[0];

                            }
                            catch
                            {
                                dataReturn.Code = 5;
                                dataReturn.Msg = "解析HIS出参失败,未找到DEPTLIST节点,请检查HIS出参";
                                goto EndPoint;
                            }
                            DataRow[] tempRows = dtDept.Select("SCH_DATE='" + _in.SCH_DATE + "' AND SCH_TIME='"+_in.SCH_TIME+"' AND DEPT_CODE='"+_in.DEPT_CODE+"'");

                            COUNT_REM = tempRows[0]["COUNT_REM"].ToString().Trim();
                        }
                        else//专家号
                        {
                            try
                            {
                                dtDoc = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/DOCLIST").Tables[0];
                            }
                            catch
                            {
                                dataReturn.Code = 5;
                                dataReturn.Msg = "解析HIS出参失败,未找到DOCLIST节点,请检查HIS出参";
                                goto EndPoint;
                            }

                            DataRow[] tempRows = dtDoc.Select("SCH_DATE='" + _in.SCH_DATE + "' AND SCH_TIME='" + _in.SCH_TIME + "' AND DEPT_CODE='" + _in.DEPT_CODE + "' AND DOC_NO='"+ _in.DOC_NO+"'");

                            COUNT_REM = tempRows[0]["COUNT_REM"].ToString().Trim();

                        }

                        
                     
                       


                        //DataTable dtperiod = ds.Tables["PERIOD"];
                        DataTable dtperiod = new DataTable();
                        dtperiod.Columns.Add("PERIOD_START", typeof(string));
                        dtperiod.Columns.Add("PERIOD_END", typeof(string));
                        dtperiod.Columns.Add("COUNT_REM", typeof(string));
                        dtperiod.Columns.Add("REGISTER_TYPE", typeof(string));

                        DataRow dataRow = dtperiod.NewRow();
                        dataRow["PERIOD_START"] = "08:00";
                        dataRow["PERIOD_END"] = "17:00";
                        dataRow["COUNT_REM"] = COUNT_REM;
                        dataRow["REGISTER_TYPE"] =string.IsNullOrEmpty(_in.REGISTER_TYPE)?"":_in.REGISTER_TYPE;
                        dtperiod.Rows.Add(dataRow);
                        _out.PERIODLIST = new List<Model.GETSCHPERIOD_M.PERIOD>();
                        foreach (DataRow dr in dtperiod.Rows)
                        {
                            Model.GETSCHPERIOD_M.PERIOD period = new Model.GETSCHPERIOD_M.PERIOD();
                            period.PERIOD_START = dtperiod.Columns.Contains("PERIOD_START") ? dr["PERIOD_START"].ToString() : "";
                            period.PERIOD_END = dtperiod.Columns.Contains("PERIOD_END") ? dr["PERIOD_END"].ToString() : "";
                            period.COUNT_REM = dtperiod.Columns.Contains("COUNT_REM") ? dr["COUNT_REM"].ToString() : "";
                            //period.REGISTER_TYPE = dtperiod.Columns.Contains("REGISTERCODE") ? dr["REGISTERCODE"].ToString() : "";
                            period.REGISTER_TYPE = dtperiod.Columns.Contains("REGISTER_TYPE") ? dr["REGISTER_TYPE"].ToString() : "";
                            _out.PERIODLIST.Add(period);
                        }
                    }
                    catch(Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到SCHLIST节点,请检查HIS出参";
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
