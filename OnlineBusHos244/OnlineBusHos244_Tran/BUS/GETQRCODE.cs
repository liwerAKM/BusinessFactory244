using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
//using Alipay.AopSdk.F2FPay.Business;

namespace OnlineBusHos244_Tran.BUS
{
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
                    Service_name = "WXPAYPRECREATE";//对应getqrcode
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



                if (_in.DEAL_TYPE == "2")//支付宝
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
                    AlipayTradeCreate F2FPayResult = InDataRe.GetBusinessData<AlipayTradeCreate>(Key);
                    _out.HIS_RTNXML = F2FPayResult.response.Body;
                    if (F2FPayResult.response.Code == "10000")
                    {

                        _out.QRCODE = F2FPayResult.response.QrCode;
                        _out.QUERYID = out_trade_no;
                    }
                    else
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = F2FPayResult.response.Msg;
                        goto EndPoint;
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                else if (_in.DEAL_TYPE == "1")//微信
                {
                    string HOS_ID = _in.HOS_ID;
                    if (HOS_ID == "44")//微信使用44-1的商户
                    {
                        HOS_ID = "44-1";
                    }
                    Key = EncryptionKeyCore.KeyData.AESKEY(HOS_ID);//解密key重新赋值
                    WxPayTradePrecreateBuilder builder = new WxPayTradePrecreateBuilder();
                    builder.out_trade_no = out_trade_no;
                    builder.total_fee = FormatHelper.GetDecimal(_in.CASH_JE);
                    builder.in_body = TYPE_NAME;
                    builder.time_expire = System.DateTime.Now.AddMinutes(FormatHelper.GetStr(_in.TIME_EXPIRE) == "" ? 2 : int.Parse(FormatHelper.GetStr(_in.TIME_EXPIRE))).ToString("yyyyMMddHHmmss");
                    InteractiveData InDataRe = PubFunc.F2FPay(HOS_ID, builder, Service_name);
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
