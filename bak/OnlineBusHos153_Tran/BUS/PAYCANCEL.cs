using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using OnlineBusHos153_Tran.Model;
using Com.Alipay.Domain;
using Com.Alipay.Business;

namespace OnlineBusHos153_Tran.BUS
{
    class PAYCANCEL
    {
        public static string B_PAYCANCEL(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                PAYCANCEL_M.PAYCANCEL_IN _in = JSONSerializer.Deserialize<PAYCANCEL_M.PAYCANCEL_IN>(json_in);
                PAYCANCEL_M.PAYCANCEL_OUT _out = new PAYCANCEL_M.PAYCANCEL_OUT();
                string his_rtnxml = "";
                if (GlobalVar.DoBussiness == "0")
                {
                    XmlDocument doc = QHXmlMode.GetBaseXml("PAYCANCEL", "1");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());
                    string inxml = doc.InnerXml;

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
                        DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                        if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                            goto EndPoint;
                        }
                        _out.STATUS = dtrev.Columns.Contains("STATUS") ? FormatHelper.GetStr(dtrev.Rows[0]["STATUS"]) : "";
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
                else if (GlobalVar.DoBussiness == "1")
                {
                    string Key = EncryptionKeyCore.KeyData.AESKEY(_in.HOS_ID);
                    string Service_name = _in.DEAL_TYPE == "1" ? "WXPAYCLOSEORDER" : "ALIPAYTRADECLOSE";
                    if (GlobalVar.Linux == "0")
                    {
                        if (_in.DEAL_TYPE == "2")//支付宝
                        {

                            _out.STATUS = "1";      
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                        else if (_in.DEAL_TYPE == "1")//微信
                        {
                            Dictionary<string, string> builder = new Dictionary<string, string>();
                            builder.Add("out_trade_no", _in.QUERYID);
                            InteractiveData InDataRe = GlobalVar.F2FPay(_in.HOS_ID, builder, Service_name);
                            if (InDataRe.Code < 0)
                            {
                                if (InDataRe.Code == -2)
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg = InDataRe.Msg;
                                    goto EndPoint;
                                }
                                else
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg = AESExample.Decrypt(InDataRe.Body, Key); ;
                                    goto EndPoint;
                                }
                            }
                            F2FPAY.Windows.WxPayCloseOrder F2FPayResult = InDataRe.GetBusinessData<F2FPAY.Windows.WxPayCloseOrder>(Key);
                            _out.HIS_RTNXML = "";
                            if (F2FPayResult.result_code == "SUCCESS")
                            {
                                _out.STATUS = "1";
                            }
                            else
                            {
                                _out.STATUS = "0";
                            }
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                    }
                    else
                    {
                        if (_in.DEAL_TYPE == "2")//支付宝
                        {
                            Dictionary<string, string> builder = new Dictionary<string, string>();
                            builder.Add("out_trade_no", _in.QUERYID);
                            InteractiveData InDataRe = GlobalVar.F2FPay_Linux(_in.HOS_ID, builder, Service_name);
                            if (InDataRe.Code < 0)
                            {
                                if (InDataRe.Code == -2)
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg = InDataRe.Msg;
                                    goto EndPoint;
                                }
                                else
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg = AESExample.Decrypt(InDataRe.Body, Key); ;
                                    goto EndPoint;
                                }
                            }
                            F2FPAY.Linux.AlipayTradeCloseResponse F2FRefundResult = InDataRe.GetBusinessData<F2FPAY.Linux.AlipayTradeCloseResponse>(Key);
                            _out.HIS_RTNXML = F2FRefundResult.Body;
                            if (F2FRefundResult.Code == "10000")
                            {
                                _out.STATUS = "1";
                            }
                            else
                            {
                                _out.STATUS = "0";
                            }
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                        else if (_in.DEAL_TYPE == "1")//微信
                        {
                            Dictionary<string, string> builder = new Dictionary<string, string>();
                            builder.Add("out_trade_no", _in.QUERYID);
                            InteractiveData InDataRe = GlobalVar.F2FPay_Linux(_in.HOS_ID, builder, Service_name);
                            if (InDataRe.Code < 0)
                            {
                                if (InDataRe.Code == -2)
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg = InDataRe.Msg;
                                    goto EndPoint;
                                }
                                else
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg =  AESExample.Decrypt(InDataRe.Body, Key); ;
                                    goto EndPoint;
                                }
                            }
                            F2FPAY.Linux.WxPayCloseOrder F2FPayResult = InDataRe.GetBusinessData<F2FPAY.Linux.WxPayCloseOrder>(Key);
                            _out.HIS_RTNXML = "";
                            if (F2FPayResult.result_code == "SUCCESS")
                            {
                                _out.STATUS = "1";
                            }
                            else
                            {
                                _out.STATUS = "0";
                            }
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                    }
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
