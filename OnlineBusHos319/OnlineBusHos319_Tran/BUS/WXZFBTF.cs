using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using Alipay.AopSdk.F2FPay.Domain;
using Alipay.AopSdk.F2FPay.Business;
//using Com.Alipay.Business;
//using Com.Alipay.Domain;

namespace OnlineBusHos319_Tran.BUS
{
    class WXZFBTF
    {
        public static string B_WXZFBTF(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.WXZFBTF_M.WXZFBTF_IN _in = JSONSerializer.Deserialize<Model.WXZFBTF_M.WXZFBTF_IN>(json_in);
                Model.WXZFBTF_M.WXZFBTF_OUT _out = new Model.WXZFBTF_M.WXZFBTF_OUT();
                string Key = EncryptionKeyCore.KeyData.AESKEY(_in.HOS_ID);
                string Service_name = _in.DEAL_TYPE == "1" ? "WXPAYREFUND" : "ALIPAYTradeRefund";
                if (_in.DEAL_TYPE == "2")//支付宝
                {
                    AlipayTradeRefundContentBuilder builder = new AlipayTradeRefundContentBuilder();
                    builder.out_trade_no = _in.QUERYID;
                    //退款请求单号保持唯一性。
                    builder.out_request_no = NewIdHelper.NewOrderId20 + "-" + _in.HOS_ID;
                    //out_request_no = builder.out_request_no;
                    builder.refund_amount = FormatHelper.GetDecimal(_in.CASH_JE).ToString("0.00");
                    builder.refund_reason = FormatHelper.GetStr(_in.REASON) == "" ? "refund reason" : FormatHelper.GetStr(_in.REASON);
                    InteractiveData InDataRe = PubFunc.F2FPay(_in.HOS_ID, builder, Service_name);
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
                    AlipayF2FRefundResult F2FRefundResult = InDataRe.GetBusinessData<AlipayF2FRefundResult>(Key);
                    _out.HIS_RTNXML = F2FRefundResult.response.Body;
                    if (F2FRefundResult.Status.ToString() == "SUCCESS")
                    {
                        _out.STATUS = "1";
                        _out.OUT_REQUEST_NO = builder.out_request_no;
                    }
                    else
                    {
                        _out.STATUS = "0";
                        _out.OUT_REQUEST_NO = builder.out_request_no;
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
                else if (_in.DEAL_TYPE == "1")//微信
                {
                    WxPayTradeRefundBuilder builder = new WxPayTradeRefundBuilder();
                    DateTime time_start = DateTime.Now;
                    builder.out_trade_no = _in.QUERYID;
                    builder.out_refund_no = NewIdHelper.NewOrderId20 + "-" + _in.HOS_ID;
                    builder.refund_fee = FormatHelper.GetDecimal(_in.CASH_JE);
                    InteractiveData InDataRe = PubFunc.F2FPay(_in.HOS_ID, builder, Service_name);
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
                    WxPayTradeRefundBuilder F2FPayResult = InDataRe.GetBusinessData<WxPayTradeRefundBuilder>(Key);
                    _out.HIS_RTNXML = F2FPayResult.DateRe;
                    if (F2FPayResult.result_code == "SUCCESS")
                    {
                        _out.STATUS = "1";
                        _out.OUT_REQUEST_NO = builder.out_refund_no;
                    }
                    else
                    {
                        _out.STATUS = "0";
                        _out.OUT_REQUEST_NO = builder.out_refund_no;
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }

                else
                {

                    POSC2BUnionPay.Models.QHModel.PAYREFUND_IN posin = new POSC2BUnionPay.Models.QHModel.PAYREFUND_IN();
                    posin.HOS_ID = _in.HOS_ID;
                    posin.QUERYID = _in.QUERYID;
                    posin.CASH_JE = decimal.Parse(_in.CASH_JE);
                    string TID = "";
                    //获取终端号对应信息
                    var db = new DbMySQLZZJ().Client;
                    SqlSugarModel.Baccountposc2btid model = db.Queryable<SqlSugarModel.Baccountposc2btid>().Where(t => t.HOS_ID == _in.HOS_ID && t.LTERMINAL_SN == _in.LTERMINAL_SN).First();
                    if (model != null)
                    {
                        TID = model.tid;
                    }
                    posin.TID = TID;
                    POSC2BUnionPay.Models.QHModel.PAYREFUND_OUT posout = new POSC2BUnionPay.Models.QHModel.PAYREFUND_OUT();
                    string errmsg = "";
                    bool flag = POSC2BUnionPay.POSC2BunionPayHelper.CallService("PAYREFUND", posin, ref posout, ref errmsg);
                    if (!flag)
                    {
                        dataReturn.Code = -1;
                        dataReturn.Msg = errmsg;
                        goto EndPoint;
                    }
                    if (posout.CLBZ == "0")
                    {
                        _out.STATUS = "1";
                    }
                    else
                    {
                        _out.STATUS = "0";
                        dataReturn.Code = 6;
                        dataReturn.Msg = posout.CLJG;

                        goto EndPoint;
                    }
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
    }
}
