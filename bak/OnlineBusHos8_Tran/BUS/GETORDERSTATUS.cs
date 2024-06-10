using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using OnlineBusHos8_Tran.Model;
using Com.Alipay.Business;

namespace OnlineBusHos8_Tran.BUS
{
    class GETORDERSTATUS
    {
        public static string B_GETORDERSTATUS(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETORDERSTATUS_M.GETORDERSTATUS_IN _in = JSONSerializer.Deserialize<GETORDERSTATUS_M.GETORDERSTATUS_IN>(json_in);
                GETORDERSTATUS_M.GETORDERSTATUS_OUT _out = new GETORDERSTATUS_M.GETORDERSTATUS_OUT();

                string his_rtnxml = "";
                if (GlobalVar.DoBussiness == "0")
                {
                    XmlDocument doc = QHXmlMode.GetBaseXml("GETQRCODE", "1");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
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
                    string Service_name = _in.DEAL_TYPE == "1" ? "WXPAYQUERY" : "ALIPAYTradeQuery";

                    if (GlobalVar.Linux == "0")
                    {
                        if (_in.DEAL_TYPE == "2")//支付宝
                        {
                            Dictionary<string, string> builder = new Dictionary<string, string>();
                            builder.Add("out_trade_no", _in.QUERYID);
                            builder.Add("COMM_HIS", "");
                            InteractiveData InDataRe = GlobalVar.F2FPay(_in.HOS_ID, builder, Service_name);
                            if (InDataRe.Code < 0)
                            {
                                if (InDataRe.Code == -2)
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg =InDataRe.Msg;
                                    goto EndPoint;
                                }
                                else
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg =  AESExample.Decrypt(InDataRe.Body, Key); 
                                    goto EndPoint;
                                }
                            }
                            AlipayF2FQueryResult F2FQueryResult = InDataRe.GetBusinessData<AlipayF2FQueryResult>(Key);
                            _out.HIS_RTNXML = F2FQueryResult.response.Body;
                            if (F2FQueryResult.Status.ToString() == "SUCCESS")
                            {
                                #region 保存alipay_tran数据
                                Plat.Model.alipay_tran tran = new Plat.MySQLDAL.alipay_tran().GetModel(_in.QUERYID);
                                if (tran == null)
                                {
                                    tran.batch_no = "";
                                    tran.body = "";
                                    tran.buyer_email = "";
                                    tran.buyer_id = "";
                                    tran.COMM_SN = _in.QUERYID;
                                    tran.COMM_MAIN = _in.QUERYID;
                                    tran.error_code = "";
                                    tran.error_message = "";
                                    tran.gmt_create = DateTime.Now;
                                    tran.gmt_payment = DateTime.Now;
                                    tran.gmt_refund = DateTime.Now;
                                    tran.JE = 0;
                                    tran.notify_id = "";
                                    tran.notify_time = DateTime.Now;
                                    tran.notify_type = "";
                                    tran.payment_type = "3";
                                    tran.refund_status = "";
                                    tran.seller_email = "";
                                    tran.seller_id = "";
                                    tran.subject = "";
                                    tran.trade_code = "01";
                                    tran.trade_message = "";
                                    tran.TRADE_NO = "";
                                    tran.TRADE_STATUS = "";
                                    tran.TXN_TYPE = "01";
                                    tran.USER_ID = _in.USER_ID;
                                    tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    tran.PAT_NAME = _in.PAT_NAME;
                                    tran.SFZ_NO = _in.SFZ_NO;
                                    tran.HOSPATID = _in.HOSPATID;
                                }
                                else
                                {
                                    tran.TXN_TYPE = "01";
                                    tran.COMM_MAIN = _in.QUERYID;
                                    tran.USER_ID =_in.USER_ID;
                                    tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    tran.PAT_NAME = _in.PAT_NAME;
                                    tran.SFZ_NO = _in.SFZ_NO;
                                    tran.HOSPATID = _in.HOSPATID;
                                }
                                new Plat.MySQLDAL.alipay_tran().Add(tran);
                                #endregion
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
                            builder.Add("COMM_HIS", "");
                            InteractiveData InDataRe = GlobalVar.F2FPay(_in.HOS_ID, builder, Service_name);
                            if (InDataRe.Code < 0)
                            {
                                if (InDataRe.Code == -2)
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg =  InDataRe.Msg;
                                    goto EndPoint;
                                }
                                else
                                {

                                    dataReturn.Code = 1;
                                    dataReturn.Msg =  AESExample.Decrypt(InDataRe.Body, Key); 
                                    goto EndPoint;
                                }
                            }
                            F2FPAY.Windows.WxPayTradeQueryBuilder F2FQueryResult = InDataRe.GetBusinessData<F2FPAY.Windows.WxPayTradeQueryBuilder>(Key);
                            if (F2FQueryResult.result_code.ToString() == "SUCCESS" && F2FQueryResult.trade_state == "SUCCESS")
                            {
                                #region 保存wechat_tran表数据
                                Plat.Model.wechat_tran wet_tran = new Plat.MySQLDAL.wechat_tran().GetModel(_in.QUERYID);
                                if (wet_tran == null)
                                {
                                    wet_tran.appid = "";
                                    wet_tran.AT_result_code = "";
                                    wet_tran.AT_TIME = "";
                                    wet_tran.body = "";
                                    wet_tran.COMM_MAIN = _in.QUERYID;
                                    wet_tran.COMM_SN = _in.QUERYID;
                                    wet_tran.currency_type = "";
                                    wet_tran.device_info = "";
                                    wet_tran.error_code = "";
                                    wet_tran.error_message = "";
                                    wet_tran.JE = 0;
                                    wet_tran.mch_id = "";
                                    wet_tran.nonce_str = "";
                                    wet_tran.openid = "";
                                    wet_tran.prepay_id = "";
                                    wet_tran.refund_channe = "";
                                    wet_tran.refund_recv_accout = "";
                                    wet_tran.request_back_time = DateTime.Now;
                                    wet_tran.return_code = "";
                                    wet_tran.spbill_create_ip = "";
                                    wet_tran.time_end = DateTime.Now;
                                    wet_tran.time_start = DateTime.Now;
                                    wet_tran.trade_code = "01";
                                    wet_tran.trade_message = "";
                                    wet_tran.trade_type = "";
                                    wet_tran.transaction_id = "";
                                    wet_tran.TXN_TYPE = "01";
                                    wet_tran.USER_ID = _in.USER_ID;
                                    wet_tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    wet_tran.PAT_NAME = _in.PAT_NAME;
                                    wet_tran.SFZ_NO = _in.SFZ_NO;
                                    wet_tran.HOSPATID = _in.HOSPATID;
                                }
                                else
                                {
                                    wet_tran.TXN_TYPE = "01";
                                    wet_tran.COMM_MAIN = _in.QUERYID;
                                    wet_tran.USER_ID = _in.USER_ID;
                                    wet_tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    wet_tran.PAT_NAME = _in.PAT_NAME;
                                    wet_tran.SFZ_NO = _in.SFZ_NO;
                                    wet_tran.HOSPATID = _in.HOSPATID;
                                }
                                new Plat.MySQLDAL.wechat_tran().Add(wet_tran);
                                #endregion
                                _out.STATUS = "1";
                            }
                            else
                            {
                                _out.STATUS = "0";
                            }
                            _out.HIS_RTNXML = F2FQueryResult.DateRe;
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                        }
                        else
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "无效的支付方式";
                        }
                    }
                    else
                    {
                        if (_in.DEAL_TYPE == "2")//支付宝
                        {
                            Dictionary<string, string> builder = new Dictionary<string, string>();
                            builder.Add("out_trade_no", _in.QUERYID);
                            builder.Add("COMM_HIS", "");
                            InteractiveData InDataRe = GlobalVar.F2FPay_Linux(_in.HOS_ID, builder, Service_name);
                            if (InDataRe.Code < 0)
                            {
                                if (InDataRe.Code == -2)
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg =  InDataRe.Msg;
                                    goto EndPoint;
                                }
                                else
                                {
                                    dataReturn.Code = 1;
                                    dataReturn.Msg =  AESExample.Decrypt(InDataRe.Body, Key); ;
                                    goto EndPoint;
                                }
                            }
                            F2FPAY.Linux.AlipayTradeQuery F2FQueryResult = InDataRe.GetBusinessData<F2FPAY.Linux.AlipayTradeQuery>(Key);
                            _out.HIS_RTNXML = F2FQueryResult.Body;
                            if (F2FQueryResult.Code == "10000" && F2FQueryResult.TradeStatus == "TRADE_SUCCESS")
                            {
                                #region 保存alipay_tran数据
                                Plat.Model.alipay_tran tran = new Plat.MySQLDAL.alipay_tran().GetModel(_in.QUERYID);
                                if (tran == null)
                                {
                                    tran.batch_no = "";
                                    tran.body = "";
                                    tran.buyer_email = "";
                                    tran.buyer_id = "";
                                    tran.COMM_SN = _in.QUERYID;
                                    tran.COMM_MAIN = _in.QUERYID;
                                    tran.error_code = "";
                                    tran.error_message = "";
                                    tran.gmt_create = DateTime.Now;
                                    tran.gmt_payment = DateTime.Now;
                                    tran.gmt_refund = DateTime.Now;
                                    tran.JE = 0;
                                    tran.notify_id = "";
                                    tran.notify_time = DateTime.Now;
                                    tran.notify_type = "";
                                    tran.payment_type = "3";
                                    tran.refund_status = "";
                                    tran.seller_email = "";
                                    tran.seller_id = "";
                                    tran.subject = "";
                                    tran.trade_code = "01";
                                    tran.trade_message = "";
                                    tran.TRADE_NO = "";
                                    tran.TRADE_STATUS = "";
                                    tran.TXN_TYPE = "01";
                                    tran.USER_ID = _in.USER_ID;
                                    tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    tran.PAT_NAME = _in.PAT_NAME;
                                    tran.SFZ_NO = _in.SFZ_NO;
                                    tran.HOSPATID = _in.HOSPATID;
                                }
                                else
                                {
                                    tran.TXN_TYPE = "01";
                                    tran.COMM_MAIN = _in.QUERYID;
                                    tran.USER_ID = _in.USER_ID;
                                    tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    tran.PAT_NAME = _in.PAT_NAME;
                                    tran.SFZ_NO = _in.SFZ_NO;
                                    tran.HOSPATID = _in.HOSPATID;
                                }
                                new Plat.MySQLDAL.alipay_tran().Add(tran);
                                #endregion
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
                            builder.Add("COMM_HIS", "");
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
                                    dataReturn.Msg =  AESExample.Decrypt(InDataRe.Body, Key); 
                                    goto EndPoint;
                                }
                            }
                            F2FPAY.Linux.WxPayTradeQueryBuilder F2FQueryResult = InDataRe.GetBusinessData<F2FPAY.Linux.WxPayTradeQueryBuilder>(Key);
                            _out.HIS_RTNXML = F2FQueryResult.DateRe;
                            if (F2FQueryResult.result_code.ToString() == "SUCCESS" && F2FQueryResult.trade_state == "SUCCESS")
                            {
                                #region 保存wechat_tran表数据
                                Plat.Model.wechat_tran wet_tran = new Plat.MySQLDAL.wechat_tran().GetModel(_in.QUERYID);
                                if (wet_tran == null)
                                {
                                    wet_tran.appid = "";
                                    wet_tran.AT_result_code = "";
                                    wet_tran.AT_TIME = "";
                                    wet_tran.body = "";
                                    wet_tran.COMM_MAIN = _in.QUERYID;
                                    wet_tran.COMM_SN = _in.QUERYID;
                                    wet_tran.currency_type = "";
                                    wet_tran.device_info = "";
                                    wet_tran.error_code = "";
                                    wet_tran.error_message = "";
                                    wet_tran.JE = 0;
                                    wet_tran.mch_id = "";
                                    wet_tran.nonce_str = "";
                                    wet_tran.openid = "";
                                    wet_tran.prepay_id = "";
                                    wet_tran.refund_channe = "";
                                    wet_tran.refund_recv_accout = "";
                                    wet_tran.request_back_time = DateTime.Now;
                                    wet_tran.return_code = "";
                                    wet_tran.spbill_create_ip = "";
                                    wet_tran.time_end = DateTime.Now;
                                    wet_tran.time_start = DateTime.Now;
                                    wet_tran.trade_code = "01";
                                    wet_tran.trade_message = "";
                                    wet_tran.trade_type = "";
                                    wet_tran.transaction_id = "";
                                    wet_tran.TXN_TYPE = "01";
                                    wet_tran.USER_ID = _in.USER_ID;
                                    wet_tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    wet_tran.PAT_NAME = _in.PAT_NAME;
                                    wet_tran.SFZ_NO = _in.SFZ_NO;
                                    wet_tran.HOSPATID = _in.HOSPATID;
                                }
                                else
                                {
                                    wet_tran.TXN_TYPE = "01";
                                    wet_tran.COMM_MAIN = _in.QUERYID;
                                    wet_tran.USER_ID = _in.USER_ID;
                                    wet_tran.lTERMINAL_SN = _in.LTERMINAL_SN;
                                    wet_tran.PAT_NAME = _in.PAT_NAME;
                                    wet_tran.SFZ_NO = _in.SFZ_NO;
                                    wet_tran.HOSPATID = _in.HOSPATID;
                                }
                                new Plat.MySQLDAL.wechat_tran().Add(wet_tran);
                                #endregion
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
                        else
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "无效的支付方式";
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
