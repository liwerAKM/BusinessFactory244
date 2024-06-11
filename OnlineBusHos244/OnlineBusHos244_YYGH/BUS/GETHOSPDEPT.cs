using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using Kdbndp.KingbaseTypes;
//using System.IO;

namespace OnlineBusHos244_YYGH.BUS
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
          

            string[] nousrdept = PubFunc.GetConfigClass("nousedept").Value.ToString().Split("|"); ;
            try
            {
                Model.GETHOSPDEPT_M.GETHOSPDEPT_IN _in = JSONSerializer.Deserialize<Model.GETHOSPDEPT_M.GETHOSPDEPT_IN>(json_in);
                Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT _out = new Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("SENDHOSPDEPT", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USE_TYPE", _in.USE_TYPE);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_TYPE", string.IsNullOrEmpty(_in.FILT_TYPE) ? "" : _in.FILT_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_VALUE", string.IsNullOrEmpty(_in.FILT_VALUE) ? "" : _in.FILT_VALUE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "RETURN_TYPE", string.IsNullOrEmpty(_in.RETURN_TYPE) ? "" : _in.RETURN_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGEINDEX", string.IsNullOrEmpty(_in.PAGEINDEX) ? "" : _in.PAGEINDEX.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGESIZE", string.IsNullOrEmpty(_in.PAGESIZE) ? "" : _in.PAGESIZE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                //StreamReader stream = new StreamReader("D:/test.txt");
                //string his_rtnxml = stream.ReadToEnd();

                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {

                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                //his_rtnxml = "<ROOT><HEADER><TYPE>SENDHOSPDEPT</TYPE><CZLX>0</CZLX></HEADER><BODY><DEPTLIST><DEPT><DEPT_CODE>00026</DEPT_CODE><DEPT_NAME>生殖健康科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00050</DEPT_CODE><DEPT_NAME>两癌筛查</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00027</DEPT_CODE><DEPT_NAME>计划生育科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00049</DEPT_CODE><DEPT_NAME>麻醉手术科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00044</DEPT_CODE><DEPT_NAME>儿童生长发育门诊</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00009</DEPT_CODE><DEPT_NAME>儿童保健</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00052</DEPT_CODE><DEPT_NAME>外科门诊</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00010</DEPT_CODE><DEPT_NAME>口腔科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00043</DEPT_CODE><DEPT_NAME>产科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00064</DEPT_CODE><DEPT_NAME>泌尿外科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00101</DEPT_CODE><DEPT_NAME>预防保健科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00041</DEPT_CODE><DEPT_NAME>内科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00040</DEPT_CODE><DEPT_NAME>中医科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00067</DEPT_CODE><DEPT_NAME>不孕不育门诊</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00020</DEPT_CODE><DEPT_NAME>妇女保健科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00011</DEPT_CODE><DEPT_NAME>眼保健科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00023</DEPT_CODE><DEPT_NAME>孕产保健科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00092</DEPT_CODE><DEPT_NAME>VCT门诊</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00037</DEPT_CODE><DEPT_NAME>儿科</DEPT_NAME><YB_CODE /></DEPT><DEPT><DEPT_CODE>00058</DEPT_CODE><DEPT_NAME>妇科专家门诊</DEPT_NAME><YB_CODE /></DEPT></DEPTLIST></BODY></ROOT>".Replace(" ", "");
                _out.HIS_RTNXML = his_rtnxml;


                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
                    DataTable dtrev = ds.Tables[0];
                    //if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    //{
                    //    dataReturn.Code = 1;
                    //    dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                    //    dataReturn.Param = JSONSerializer.Serialize(_out);
                    //    goto EndPoint;
                    //}
                    if(dtrev.Rows.Count == 0)
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtdept = ds.Tables["DEPT"];
                        dtdept.Columns.Add("DEPT_ORDER", typeof(string));
                        if (dtdept.Rows.Count > 0)
                        {
                            dtdept.DefaultView.Sort = "DEPT_NAME";
                            dtdept = dtdept.DefaultView.ToTable();

                            foreach (DataRow dr in dtdept.Rows)
                            {
                                if (string.IsNullOrEmpty(dr["DEPT_ORDER"].ToString()))
                                {
                                    dr["DEPT_ORDER"] = "9999";
                                }
                            }

                            dtdept = System.Data.DataTableExtensions.CopyToDataTable(dtdept.Rows.Cast<DataRow>().OrderBy(r => Convert.ToDecimal(r["DEPT_ORDER"])));

                        }

                        _out.DEPTLIST = new List<Model.GETHOSPDEPT_M.DEPT>();
                        foreach (DataRow dr in dtdept.Rows)
                        {
                            Model.GETHOSPDEPT_M.DEPT dept = new Model.GETHOSPDEPT_M.DEPT();


                            #region  过滤无排班科室

                            XmlDocument doc2 = QHXmlMode.GetBaseXml("GETSCHINFO", "1");
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "HOS_ID", "244");
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "DEPT_CODE", dr["DEPT_CODE"].ToString());
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "DOC_NO", "");
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "SCH_TYPE", "2");
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "SCH_DATE", DateTime.Now.ToString("yyyy-MM-dd"));
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "YLCARD_TYPE", "4");
                            XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "YLCARD_NO", "");

                            string inxml2 = doc2.InnerXml;
                            //System.IO.StreamReader stream = new System.IO.StreamReader("D:/test.txt");
                            //string his_rtnxml = stream.ReadToEnd();
                            string his_rtnxml2 = "";
                            if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml2, ref his_rtnxml2))
                            {

                                dataReturn.Code = 1;
                                dataReturn.Msg = his_rtnxml2;
                                goto EndPoint;
                            }
                            //his_rtnxml2 = "<ROOT><HEADER><TYPE>GETSCHINFO</TYPE><CZLX>0</CZLX></HEADER><BODY><CLBZ>0</CLBZ><CLJG>(TYPE:GETSCHINFO)成功</CLJG><DEPTLIST><DEPT><DEPT_CODE>00092</DEPT_CODE><DEPT_NAME>VCT门诊</DEPT_NAME><SCH_TIME>上午</SCH_TIME><DOC_NO /><DOC_NAME /><SCH_TYPE>1</SCH_TYPE><REGISTER_TYPE>008</REGISTER_TYPE><STATUS /><COUNT_REM>50</COUNT_REM><GH_FEE>0.0</GH_FEE><ZL_FEE>0</ZL_FEE><START_TIME>00:00:00</START_TIME><END_TIME>13:00:00</END_TIME><CAN_WAIT>0</CAN_WAIT><DEPT_ORDER>00092</DEPT_ORDER><YB_CODE>110200006-a-4%0%</YB_CODE><DOC_ORDER /><DOC_NO_GJM /></DEPT><DEPT><DEPT_CODE>00092</DEPT_CODE><DEPT_NAME>VCT门诊</DEPT_NAME><SCH_TIME>下午</SCH_TIME><DOC_NO /><DOC_NAME /><SCH_TYPE>1</SCH_TYPE><REGISTER_TYPE>008</REGISTER_TYPE><STATUS /><COUNT_REM>50</COUNT_REM><GH_FEE>0.0</GH_FEE><ZL_FEE>0</ZL_FEE><START_TIME>13:00:00</START_TIME><END_TIME>23:59:59</END_TIME><CAN_WAIT>0</CAN_WAIT><DEPT_ORDER>00092</DEPT_ORDER><YB_CODE>110200006-a-4%0%</YB_CODE><DOC_ORDER /><DOC_NO_GJM /></DEPT></DEPTLIST><DOCLIST /></BODY></ROOT>";
                            try
                            {

                                XmlDocument xmldoc2 = XMLHelper.X_GetXmlDocument(his_rtnxml2);
                                DataSet ds2 = XMLHelper.X_GetXmlData(xmldoc2, "ROOT/BODY");
                                DataSet ds3 = XMLHelper.X_GetXmlData(xmldoc2, "ROOT/BODY/DEPTLIST");
                                DataSet ds4 = XMLHelper.X_GetXmlData(xmldoc2, "ROOT/BODY/DOCLIST");




                               

                                if (ds3.Tables.Count == 0 && ds4.Tables.Count == 0)
                                {
                                    continue;
                                }

                                //#region 只要有一个非零元号源科室就是保留，其余情况 跳过
                                //bool f1 = false;
                                //foreach(DataRow tr in ds3.Tables[0].Rows)
                                //{
                                //    string aab = tr["GH_FEE"].ToString();
                                //    if (tr["GH_FEE"].ToString() != "0.0"|| tr["ZL_FEE"].ToString() != "0") { 
                                        
                                //      f1 = true;  }

                                //}
                                //if (!f1) {
                                //    continue;
                                //}
                                //#endregion



                                DataTable dtrev2 = ds2.Tables[0];

                                if (dtrev2.Rows[0]["CLBZ"].ToString() != "0")
                                {
                                    continue;

                                }
                            }
                            catch { continue; }
                            #endregion

                            dept.DEPT_CODE = dtdept.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                            dept.DEPT_NAME = dtdept.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                            dept.DEPT_INTRO = dtdept.Columns.Contains("DEPT_INTRO") ? dr["DEPT_INTRO"].ToString() : "";
                            dept.DEPT_URL = dtdept.Columns.Contains("DEPT_URL") ? dr["DEPT_URL"].ToString() : "";
                            dept.DEPT_ORDER = dtdept.Columns.Contains("DEPT_ORDER") ? dr["DEPT_ORDER"].ToString() : "";
                            dept.DEPT_TYPE = dtdept.Columns.Contains("DEPT_TYPE") ? dr["DEPT_TYPE"].ToString() : "";
                            dept.DEPT_ADDRESS = dtdept.Columns.Contains("DEPT_ADDRESS") ? dr["DEPT_ADDRESS"].ToString() : "";
                            dept.DEPT_USE = dtdept.Columns.Contains("DEPT_USE") ? dr["DEPT_USE"].ToString() : "";
                            _out.DEPTLIST.Add(dept);

                        }
                    }
                    catch
                    {
                        //    dataReturn.Code = 5;
                        //    dataReturn.Msg = "解析HIS出参失败,未找到DEPTLIST节点,请检查HIS出参";
                        dataReturn.Code = 0;
                        dataReturn.Msg = "SUCCESS";
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
