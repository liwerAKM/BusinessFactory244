using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using static OnlineBusHos172_OutHos.Model.GETOUTFEENOPAYMX_M;
using OnlineBusHos172_OutHos.Model;

namespace OnlineBusHos172_OutHos.BUS
{
    class GETOUTFEENOPAYMX
    {
        public static string B_GETOUTFEENOPAYMX(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_IN _in = JSONSerializer.Deserialize<Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_IN>(json_in);
                Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_OUT _out = new Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETOUTFEENOPAYMX", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPT_SN", string.IsNullOrEmpty(_in.OPT_SN) ? "" : _in.OPT_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRE_NO", string.IsNullOrEmpty(_in.PRE_NO) ? "" : _in.PRE_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MB_ID", string.IsNullOrEmpty(_in.MB_ID) ? "" : _in.MB_ID.Trim());


                string inxml = doc.InnerXml;
                //System.IO.StreamReader stream = new System.IO.StreamReader("D:/test.txt");
                //string his_rtnxml = stream.ReadToEnd();
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
                    _out.JEALL = dtrev.Columns.Contains("JEALL") ?FormatHelper.GetStr(dtrev.Rows[0]["JEALL"]) : "";
                    
                    try
                    {
                        DataTable dtmed = ds.Tables["DAMED"];
                        _out.DAMEDLIST = new List<Model.GETOUTFEENOPAYMX_M.MED>();

                        if (dtmed !=null &&dtmed.Rows.Count>0)
                        {
                            #region 中药明细合并
                            var zydtmed = from items in dtmed.AsEnumerable()
                                          where items.Field<string>("IS_ZY").Equals("1")
                                          group items by items.Field<string>("PRENO") into g
                                          select new MED
                                          {
                                              PRENO = FormatHelper.GetStr(g.FirstOrDefault()["PRENO"]),
                                              DATIME = FormatHelper.GetStr(g.FirstOrDefault()["DATIME"]),
                                              DAID = FormatHelper.GetStr(g.FirstOrDefault()["DAID"]),
                                              MED_ID = FormatHelper.GetStr(g.FirstOrDefault()["MED_ID"]),
                                              MED_NAME = "中药费",
                                              MED_GG = FormatHelper.GetStr(g.FirstOrDefault()["MED_GG"]),
                                              GROUPID = FormatHelper.GetStr(g.FirstOrDefault()["GROUPID"]),
                                              USAGE = FormatHelper.GetStr(g.FirstOrDefault()["USAGE"]),
                                              AUT_NAME = FormatHelper.GetStr(g.FirstOrDefault()["AUT_NAME"]),
                                              CAMT = "1",
                                              AUT_NAMEALL = FormatHelper.GetStr(g.FirstOrDefault()["AUT_NAMEALL"]),
                                              CAMTALL = "1",
                                              TIMES = FormatHelper.GetStr(g.FirstOrDefault()["TIMES"]),
                                              PRICE = g.Sum(itm => Convert.ToDecimal(itm["AMOUNT"])).ToString(),
                                              AMOUNT = g.Sum(itm => Convert.ToDecimal(itm["AMOUNT"])).ToString(),
                                              YB_CODE = FormatHelper.GetStr(g.FirstOrDefault()["YB_CODE"]),
                                              YB_CODE_GJM = FormatHelper.GetStr(g.FirstOrDefault()["YB_CODE_GJM"]),
                                              IS_QX = "0",
                                              MINAUT_FLAG = "",
                                          };
                            _out.DAMEDLIST = zydtmed.ToList();
                            #endregion
                            //去掉合并的部分
                            DataRow[] fzydrs = dtmed.Select("IS_ZY <>'1'");
                            foreach (DataRow dr in fzydrs)
                            {
                                Model.GETOUTFEENOPAYMX_M.MED med = new Model.GETOUTFEENOPAYMX_M.MED();
                                med.PRENO = dtmed.Columns.Contains("PRENO") ? dr["PRENO"].ToString() : "";
                                med.DATIME = dtmed.Columns.Contains("DATIME") ? dr["DATIME"].ToString() : "";
                                med.DAID = dtmed.Columns.Contains("DAID") ? dr["DAID"].ToString() : "";
                                med.MED_ID = dtmed.Columns.Contains("MED_ID") ? dr["MED_ID"].ToString() : "";
                                med.MED_NAME = dtmed.Columns.Contains("MED_NAME") ? dr["MED_NAME"].ToString() : "";
                                med.MED_GG = dtmed.Columns.Contains("MED_GG") ? dr["MED_GG"].ToString() : "";
                                med.GROUPID = dtmed.Columns.Contains("GROUPID") ? dr["GROUPID"].ToString() : "";
                                med.USAGE = dtmed.Columns.Contains("USAGE") ? dr["USAGE"].ToString() : "";
                                med.AUT_NAME = dtmed.Columns.Contains("AUT_NAME") ? dr["AUT_NAME"].ToString() : "";
                                med.CAMT = dtmed.Columns.Contains("CAMT") ? dr["CAMT"].ToString() : "";
                                med.AUT_NAMEALL = dtmed.Columns.Contains("AUT_NAMEALL") ? dr["AUT_NAMEALL"].ToString() : "";
                                med.CAMTALL = dtmed.Columns.Contains("CAMTALL") ? dr["CAMTALL"].ToString() : "";
                                med.TIMES = dtmed.Columns.Contains("TIMES") ? dr["TIMES"].ToString() : "";
                                med.PRICE = dtmed.Columns.Contains("PRICE") ? dr["PRICE"].ToString() : "";
                                med.AMOUNT = dtmed.Columns.Contains("AMOUNT") ? dr["AMOUNT"].ToString() : "";
                                med.YB_CODE = dtmed.Columns.Contains("YB_CODE") ? dr["YB_CODE"].ToString() : "";
                                med.YB_CODE_GJM = dtmed.Columns.Contains("YB_CODE_GJM") ? dr["YB_CODE_GJM"].ToString() : "";
                                med.IS_QX = dtmed.Columns.Contains("IS_QX") ? dr["IS_QX"].ToString() : "";
                                med.MINAUT_FLAG = dtmed.Columns.Contains("MINAUT_FLAG") ? dr["MINAUT_FLAG"].ToString() : "";
                                _out.DAMEDLIST.Add(med);
                            }

                        }

                    }
                    catch(Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析处方明细出现异常";
                        dataReturn.Param = ex.Message;
                        goto EndPoint;
                    }
                    try
                    {
                        DataTable dtchkt = ds.Tables["DACHKT"];
                        _out.DACHKTLIST = new List<Model.GETOUTFEENOPAYMX_M.CHKT>();
                        if (dtchkt !=null&&dtchkt.Rows.Count>0)
                        {
                            #region 合并中药相关检查项目
                            var zydtchkt = from items in dtchkt.AsEnumerable()
                                          where !string.IsNullOrEmpty(items.Field<string>("CHICK_NAME_ZY"))
                                          group items by items.Field<string>("CHICK_NAME_ZY") into g
                                          select new CHKT
                                          {
                                              DATIME = FormatHelper.GetStr(g.FirstOrDefault()["DATIME"]),
                                              DAID = FormatHelper.GetStr(g.FirstOrDefault()["DAID"]),
                                              CHKIT_ID = FormatHelper.GetStr(g.FirstOrDefault()["CHKIT_ID"]),
                                              CHKIT_NAME = FormatHelper.GetStr(g.FirstOrDefault()["CHICK_NAME_ZY"]),
                                              AUT_NAME = FormatHelper.GetStr(g.FirstOrDefault()["AUT_NAME"]),
                                              CAMTALL = "1",
                                              PRICE = g.Sum(itm => Convert.ToDecimal(itm["AMOUNT"])).ToString(),
                                              AMOUNT = g.Sum(itm => Convert.ToDecimal(itm["AMOUNT"])).ToString(),
                                              YB_CODE = FormatHelper.GetStr(g.FirstOrDefault()["YB_CODE"]),
                                              YB_CODE_GJM = FormatHelper.GetStr(g.FirstOrDefault()["YB_CODE_GJM"]),
                                              IS_QX = "0",
                                              MINAUT_FLAG = "",
                                              FEE_TYPE = "",
                                          };
                            _out.DACHKTLIST= zydtchkt.ToList();
                            #endregion
                            //去掉合并的部分
                            DataRow[] fzydrs = dtchkt.Select("CHICK_NAME_ZY =''");
                            foreach (DataRow dr in fzydrs)
                            {
                                Model.GETOUTFEENOPAYMX_M.CHKT chkt = new Model.GETOUTFEENOPAYMX_M.CHKT();
                                chkt.DATIME = dtchkt.Columns.Contains("DATIME") ? dr["DATIME"].ToString() : "";
                                chkt.DAID = dtchkt.Columns.Contains("DAID") ? dr["DAID"].ToString() : "";
                                chkt.CHKIT_ID = dtchkt.Columns.Contains("CHKIT_ID") ? dr["CHKIT_ID"].ToString() : "";
                                chkt.CHKIT_NAME = dtchkt.Columns.Contains("CHKIT_NAME") ? dr["CHKIT_NAME"].ToString() : "";
                                chkt.AUT_NAME = dtchkt.Columns.Contains("AUT_NAME") ? dr["AUT_NAME"].ToString() : "";
                                chkt.CAMTALL = dtchkt.Columns.Contains("CAMTALL") ? dr["CAMTALL"].ToString() : "";
                                chkt.PRICE = dtchkt.Columns.Contains("PRICE") ? dr["PRICE"].ToString() : "";
                                chkt.AMOUNT = dtchkt.Columns.Contains("AMOUNT") ? dr["AMOUNT"].ToString() : "";
                                chkt.YB_CODE = dtchkt.Columns.Contains("YB_CODE") ? dr["YB_CODE"].ToString() : "";
                                chkt.YB_CODE_GJM = dtchkt.Columns.Contains("YB_CODE_GJM") ? dr["YB_CODE_GJM"].ToString() : "";
                                chkt.IS_QX = dtchkt.Columns.Contains("IS_QX") ? dr["IS_QX"].ToString() : "";
                                chkt.MINAUT_FLAG = dtchkt.Columns.Contains("MINAUT_FLAG") ? dr["MINAUT_FLAG"].ToString() : "";
                                chkt.FEE_TYPE = dtchkt.Columns.Contains("FEE_TYPE") ? dr["FEE_TYPE"].ToString() : "";
                                _out.DACHKTLIST.Add(chkt);
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析检查明细出现异常";
                        dataReturn.Param = ex.Message;
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
