﻿using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using Com.Alipay.Business;
using Com.Alipay.Domain;

namespace OnlineBusHos172_Tran.BUS
{

    class DataReturn_201
    {
        public class ROOT
        {
            /// <summary>
            /// 消息码
            /// </summary>
            public int Code { get; set; }
            /// <summary>
            /// 消息说明
            /// </summary>
            public string Msg { get; set; }

            /// <summary>
            /// 参数
            /// </summary>
            public Param Param { get; set; }
        }

        public class Param
        {
            /// <summary>
            /// 二维码地址
            /// </summary>
            public string QRCODE { get; set; }
            /// <summary>
            /// 交易流水号
            /// </summary>
            public string QUERYID { get; set; }

            /// <summary>
            /// HIS交易出参
            /// </summary>
            public string HIS_RTNXML { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string PARAMETERS { get; set; }
        }

    }

    class GETQRCODE
    {
        public static string B_GETQRCODE(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETQRCODE_M.GETQRCODE_IN _in = JSONSerializer.Deserialize<Model.GETQRCODE_M.GETQRCODE_IN>(json_in);
                Model.GETQRCODE_M.GETQRCODE_OUT _out = new Model.GETQRCODE_M.GETQRCODE_OUT();

                string Key = EncryptionKeyCore.KeyData.AESKEY(_in.HOS_ID);
                string Service_name = "";
                if (_in.DEAL_TYPE == "1")
                {
                    Service_name = "WXPAYPRECREATE";
                }
                else if (_in.DEAL_TYPE == "2")
                {
                    Service_name = "ALIPAYPRECREATE";
                }
                string TYPE_NAME = "";
                if (_in.TYPE == "0")
                {
                    TYPE_NAME = "Z预约挂号费" + "|" + _in.HOSPATID + "|" + _in.LTERMINAL_SN + "&1";
                }
                else if (_in.TYPE == "1")
                {
                    TYPE_NAME = "Z门诊收费" + "|" + _in.HOSPATID + "|" + _in.LTERMINAL_SN + "&2";
                }
                else if (_in.TYPE == "4")
                {
                    TYPE_NAME = "Z预交金收费" + "|" + _in.HOSPATID + "|" + _in.LTERMINAL_SN + "&3";
                }
                else if (_in.TYPE == "5")
                {
                    TYPE_NAME = "Z出院结算" + "|" + _in.HOSPATID + "|" + _in.LTERMINAL_SN + "&4";
                }
                else if (_in.TYPE == "2")
                {
                    TYPE_NAME = "Z病历本" + "|" + _in.HOSPATID + "|" + _in.LTERMINAL_SN + "&5";
                }
                string out_trade_no = Soft.Core.NewIdHelper.NewOrderId20 + "-" + _in.HOS_ID;

                if(_in.DEAL_TYPE == "98")//数字人民币
                {
                    Service_name = "GETQRCODE";
                    //DataReturn_201.ROOT dataReturn_201 = new DataReturn_201.ROOT();

                    QHModel.GETQRCODE.Request request = new QHModel.GETQRCODE.Request();
                    request.HOS_ID = _in.HOS_ID;
                    request.CLIENT_ID = "17203";
                    request.PAY_TYPE = "201";
                    request.JE = FormatHelper.GetDecimal(_in.CASH_JE);
                    request.ORDER_DESC = TYPE_NAME.Replace("&","");
                    request.COMM_HIS = out_trade_no;

                    InteractiveData InDataRe = PubFunc.F2FPay_201(_in.HOS_ID, request, Service_name);

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
                            dataReturn.Msg = AESExample.Decrypt(InDataRe.Body, Key);
                            goto EndPoint;
                        

                         }
                    }


                    QHModel.GETQRCODE.Response F2FPayResult = InDataRe.GetBusinessData<QHModel.GETQRCODE.Response>(Key);
                    //dataReturn_201.Param = new DataReturn_201.Param();
                    if (F2FPayResult.CLBZ == "0")
                    {
                        _out.QRCODE = F2FPayResult.QRCODE;
                        _out.QUERYID = F2FPayResult.COMM_SN;
                    }
                    else
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = F2FPayResult.CLJG;
                        goto EndPoint;

                    }

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                    
                }
                else if (_in.DEAL_TYPE == "2")//支付宝
                {
                    AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
                    builder.out_trade_no = out_trade_no;
                    builder.total_amount = FormatHelper.GetDecimal(_in.CASH_JE).ToString("0.00");
                    builder.discountable_amount = FormatHelper.GetDecimal(_in.CASH_JE).ToString("0.00");
                    builder.undiscountable_amount = "0";
                    builder.operator_id = "F2F";//商户操作员编号
                    builder.store_id = "F2F";//商户门店编号
                    builder.subject = TYPE_NAME;
                    builder.time_expire = System.DateTime.Now.AddMinutes(FormatHelper.GetStr(_in.TIME_EXPIRE) == "" ? 2 : int.Parse(FormatHelper.GetStr(_in.TIME_EXPIRE))).ToString("yyyy-MM-dd HH:mm:ss");
                    builder.body = TYPE_NAME;
                    builder.store_id = "test store id";    //商户门店编号
                    InteractiveData InDataRe =PubFunc.F2FPay(_in.HOS_ID, builder, Service_name);
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
                            dataReturn.Msg = AESExample.Decrypt(InDataRe.Body, Key);
                            goto EndPoint;
                        }
                    }
                    AlipayF2FPrecreateResult F2FPayResult = InDataRe.GetBusinessData<AlipayF2FPrecreateResult>(Key);
                    _out.HIS_RTNXML = F2FPayResult.response.Body;
                    if (F2FPayResult.Status.ToString() == "SUCCESS")
                    {
                        _out.QRCODE = F2FPayResult.response.QrCode;
                        _out.QUERYID = out_trade_no;
                    }
                    else
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = F2FPayResult.response.SubMsg;
                        goto EndPoint;
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                else if (_in.DEAL_TYPE == "1")//微信
                {
                    WxPayTradePrecreateBuilder builder = new WxPayTradePrecreateBuilder();
                    builder.out_trade_no = out_trade_no;
                    builder.total_fee = FormatHelper.GetDecimal(_in.CASH_JE);
                    builder.in_body = TYPE_NAME;
                    builder.time_expire = System.DateTime.Now.AddMinutes(FormatHelper.GetStr(_in.TIME_EXPIRE) == "" ? 2 : int.Parse(FormatHelper.GetStr(_in.TIME_EXPIRE))).ToString("yyyyMMddHHmmss");
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
                            dataReturn.Msg = AESExample.Decrypt(InDataRe.Body, Key);
                            goto EndPoint;
                        }
                    }
                   WxPayTradePrecreateBuilder F2FPayResult = InDataRe.GetBusinessData<WxPayTradePrecreateBuilder>(Key);
                    _out.HIS_RTNXML = F2FPayResult.DateRe;
                    if (F2FPayResult.result_code.ToString() == "SUCCESS")
                    {
                        _out.QRCODE = F2FPayResult.code_url;
                        _out.QUERYID = out_trade_no;
                    }
                    else
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = F2FPayResult.return_msg;
                        goto EndPoint;
                    }

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
                else
                {

                    POSC2BUnionPay.Models.QHModel.GETQRCODE_IN posin = new POSC2BUnionPay.Models.QHModel.GETQRCODE_IN();
                    posin.HOS_ID = _in.HOS_ID;
                    posin.CASH_JE = decimal.Parse(_in.CASH_JE);
                    posin.BILLDESC = TYPE_NAME;
                    string TID = "";
                    //获取终端号对应信息
                    var db = new DbMySQLZZJ().Client;
                    SqlSugarModel.Baccountposc2btid model = db.Queryable<SqlSugarModel.Baccountposc2btid>().Where(t => t.HOS_ID == _in.HOS_ID && t.LTERMINAL_SN == _in.LTERMINAL_SN).First();
                    if (model != null)
                    {
                        TID = model.tid;
                    }
                   
                    posin.TID = TID;
                    POSC2BUnionPay.Models.QHModel.GETQRCODE_OUT posout = new POSC2BUnionPay.Models.QHModel.GETQRCODE_OUT();
                    string errmsg = "";
                    bool flag = POSC2BUnionPay.POSC2BunionPayHelper.CallService("GETQRCODE", posin, ref posout, ref errmsg);
                    if (!flag)
                    {
                        dataReturn.Code = -1;
                        dataReturn.Msg = errmsg;
                        goto EndPoint;
                    }
                    if (posout.CLBZ == "0")
                    {
                        _out.QRCODE = posout.QRCODE;
                        _out.QUERYID = posout.QUERYID;
                    }
                    else
                    {
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
                dataReturn.Param = ex.ToString();
            }
            EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}
