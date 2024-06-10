using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
//using Com.Alipay.Business;
using Alipay.AopSdk.F2FPay.Business;

namespace OnlineBusHos36_Tran.BUS
{
    class GETPASSIVEPAY
    {
        public static string B_GETPASSIVEPAY(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETPASSIVEPAY_M.GETPASSIVEPAY_IN _in = JSONSerializer.Deserialize<Model.GETPASSIVEPAY_M.GETPASSIVEPAY_IN>(json_in);
                Model.GETPASSIVEPAY_M.GETPASSIVEPAY_OUT _out = new Model.GETPASSIVEPAY_M.GETPASSIVEPAY_OUT();

                string Key = EncryptionKeyCore.KeyData.AESKEY(_in.HOS_ID);
                string Service_name = _in.DEAL_TYPE == "1" ? "WXPAYTRADEPAY" : "ALIPAYTRADEPAY";
                string TYPE_NAME = "";
                if (_in.TYPE == "0")
                {
                    TYPE_NAME = "Z预约挂号费" + "|" + _in.SFZ_NO + "|" + _in.LTERMINAL_SN + "&1";
                }
                else if (_in.TYPE == "1")
                {
                    TYPE_NAME = "Z门诊收费" + "|" + _in.SFZ_NO + "|" + _in.LTERMINAL_SN + "&2";
                }
                else if (_in.TYPE == "4")
                {
                    TYPE_NAME = "Z预交金收费" + "|" + _in.SFZ_NO + "|" + _in.LTERMINAL_SN + "&3";
                }
                else if (_in.TYPE == "5")
                {
                    TYPE_NAME = "Z出院结算" + "|" + _in.SFZ_NO + "|" + _in.LTERMINAL_SN + "&4";
                }
                else if (_in.TYPE == "2")
                {
                    TYPE_NAME = "Z病历本" + "|" + _in.SFZ_NO + "|" + _in.LTERMINAL_SN + "&5";
                }
                string out_trade_no = Soft.Core.NewIdHelper.NewOrderId20 + "-" + _in.HOS_ID;
                if (_in.DEAL_TYPE == "2")//支付宝
                {
                    AlipayTradePayContentBuilder builder = new AlipayTradePayContentBuilder();
                    builder.out_trade_no = out_trade_no;
                    builder.scene = "bar_code";
                    builder.auth_code = _in.AUTH_CODE;
                    builder.total_amount = FormatHelper.GetDecimal(_in.CASH_JE).ToString("0.00");
                    builder.discountable_amount = FormatHelper.GetDecimal(_in.CASH_JE).ToString("0.00");
                    builder.undiscountable_amount = "0";
                    builder.operator_id = "F2F";//商户操作员编号
                    builder.store_id = "F2F";//商户门店编号
                    builder.subject = TYPE_NAME;
                    builder.body = TYPE_NAME;
                    builder.store_id = "test store id";    //商户门店编号
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
                    AlipayF2FPayResult F2FPayResult = InDataRe.GetBusinessData<AlipayF2FPayResult>(Key);
                    _out.HIS_RTNXML = F2FPayResult.response.Body;
                    if (F2FPayResult.Status.ToString() == "SUCCESS")
                    {
                        _out.QUERYID = out_trade_no;
                    }
                    else
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = F2FPayResult.Status.ToString();
                        goto EndPoint;
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                else if (_in.DEAL_TYPE == "1")//微信
                {
                    WxPayTradePayBuilder builder = new WxPayTradePayBuilder();
                    builder.out_trade_no = out_trade_no;
                    builder.total_fee = FormatHelper.GetDecimal(_in.CASH_JE);
                    builder.in_body = TYPE_NAME;
                    builder.in_auth_code = _in.AUTH_CODE;
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
                    WxPayTradePayBuilder F2FPayResult = InDataRe.GetBusinessData<WxPayTradePayBuilder>(Key);
                    _out.HIS_RTNXML = F2FPayResult.DateRe;
                    if (F2FPayResult.result_code.ToString() == "0" || F2FPayResult.result_code.ToString() == "SUCCESS")
                    {
                        _out.QUERYID = out_trade_no;
                    }
                    else
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = F2FPayResult.err_code_des;
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
