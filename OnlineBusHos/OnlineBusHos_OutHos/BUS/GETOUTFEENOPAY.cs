using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos_OutHos.BUS
{
    class GETOUTFEENOPAY
    {
        public static string B_GETOUTFEENOPAY(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_IN _in = JSONSerializer.Deserialize<Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_IN>(json_in);
                Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_OUT _out = new Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETOUTFEENOPAY", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());

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
                        DataTable dtpre = ds.Tables["PRE"];
                        _out.PRELIST = new List<Model.GETOUTFEENOPAY_M.PRE>();
                        foreach (DataRow dr in dtpre.Rows)
                        {
                            Model.GETOUTFEENOPAY_M.PRE pre = new Model.GETOUTFEENOPAY_M.PRE();
                            pre.OPT_SN = dtpre.Columns.Contains("OPT_SN") ? dr["OPT_SN"].ToString() : "";
                            pre.PRE_NO = dtpre.Columns.Contains("PRE_NO") ? dr["PRE_NO"].ToString() : "";
                            pre.HOS_SN = dtpre.Columns.Contains("HOS_SN") ? dr["HOS_SN"].ToString() : "";
                            pre.DEPT_CODE = dtpre.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                            pre.DEPT_NAME = dtpre.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                            pre.DOC_NO  = dtpre.Columns.Contains("DOC_NO") ? dr["DOC_NO"].ToString() : "";
                            pre.DOC_NAME = dtpre.Columns.Contains("DOC_NAME") ? dr["DOC_NAME"].ToString() : "";
                            pre.JEALL = dtpre.Columns.Contains("JEALL") ? dr["JEALL"].ToString() : "";
                            pre.CASH_JE = dtpre.Columns.Contains("CASH_JE") ? dr["CASH_JE"].ToString() : "";
                            pre.YB_PAY =dtpre.Columns.Contains("YB_PAY") ?FormatHelper.GetInt(dr["YB_PAY"]).ToString() : "1";
                            pre.YB_NOPAY_REASON = dtpre.Columns.Contains("YB_NOPAY_REASON") ? dr["YB_NOPAY_REASON"].ToString() : pre.YB_PAY=="0"?"该处方只支持自费缴费":"";

                            pre.YLLB = dtpre.Columns.Contains("YLLB") ? dr["YLLB"].ToString() : "";
                            pre.DIS_CODE = dtpre.Columns.Contains("DIS_CODE") ? dr["DIS_CODE"].ToString() : "";
                            pre.DIS_TYPE = dtpre.Columns.Contains("DIS_TYPE") ? dr["DIS_TYPE"].ToString() : "";

                            _out.PRELIST.Add(pre);
                        }
                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到ITEMLIST节点,请检查HIS出参";
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
