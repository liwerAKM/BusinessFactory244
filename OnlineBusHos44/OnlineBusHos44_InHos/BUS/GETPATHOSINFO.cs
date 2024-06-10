using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos44_InHos.BUS
{
    class GETPATHOSINFO
    {
        public static string B_GETPATHOSINFO(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";

            try
            {
                Model.GETPATHOSINFO_M.GETPATHOSINFO_IN _in = JSONSerializer.Deserialize<Model.GETPATHOSINFO_M.GETPATHOSINFO_IN>(json_in);
                Model.GETPATHOSINFO_M.GETPATHOSINFO_OUT _out = new Model.GETPATHOSINFO_M.GETPATHOSINFO_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETPATHOSINFO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "USERNAME", "QHZZJ");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REGPAT_ID", "0");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_ID", "0");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "LTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_NO", string.IsNullOrEmpty(_in.HOS_NO) ? "" : _in.HOS_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_PAT_ID", string.IsNullOrEmpty(_in.HOS_PAT_ID) ? "" : _in.HOS_PAT_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());

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
                        if (dtrev.Rows[0]["CLBZ"].ToString().Equals("INF100"))//his明确标识失败
                        {
                            dataReturn.Code = 0;
                            dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                            _out.HOS_NO = _in.HOS_NO;
                            _out.HOSPATID = _in.HOS_PAT_ID;
                            _out.JE_PAY = "0.00";
                            _out.JE_YET = "0.00";
                            _out.JE_REMAIN = "0.00";
                            _out.CAN_PAY = "1";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                            goto EndPoint;
                        }

                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }

                    
                    _out.HOS_NO = _in.HOS_NO;
                    _out.HOSPATID = dtrev.Rows[0]["HOS_PAT_ID"].ToString();
                    _out.JE_PAY = dtrev.Rows[0]["JE_PAY"].ToString();
                    _out.JE_YET = dtrev.Rows[0]["JE_YET"].ToString();
                    _out.JE_REMAIN = dtrev.Rows[0]["JE_REMAIN"].ToString();
                    _out.CAN_PAY = dtrev.Rows[0]["CAN_PAY"].ToString();


                    //获取缴费明细列表
                    DataTable dtPay = new DataTable();
                    try
                    {
                        dtPay = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/PAYLIST").Tables[0];
                    }
                    catch { }
                    if (dtPay.Rows.Count > 0)
                    {
                        DataView view = dtPay.DefaultView;
                        view.Sort = "HIN_TIME desc";
                        dtPay = view.ToTable();
                    }

                    _out.payList = new List<Model.GETPATHOSINFO_M.PAYLIST>();
                    _out.payList = PubFunc.ToList<Model.GETPATHOSINFO_M.PAYLIST>(dtPay);

                    DataTable dtFee = new DataTable();
                    try
                    {
                        dtFee = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/FEELIST").Tables[0];
                    }
                    catch { }

                    //费用明细
                    _out.feeList = new List<Model.GETPATHOSINFO_M.FEELIST>();
                    _out.feeList = PubFunc.ToList<Model.GETPATHOSINFO_M.FEELIST>(dtFee);

                }
                catch
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                    goto EndPoint;
                }
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常" + ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}
