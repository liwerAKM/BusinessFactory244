using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;
using Alipay.AopSdk.F2FPay.Business;

namespace OnlineBusHos44_Tran.BUS
{
    class GETORDERSTATUS
    {
        public static string B_GETORDERSTATUS(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETORDERSTATUS_M.GETORDERSTATUS_IN _in = JSONSerializer.Deserialize<Model.GETORDERSTATUS_M.GETORDERSTATUS_IN>(json_in);
                Model.GETORDERSTATUS_M.GETORDERSTATUS_OUT _out = new Model.GETORDERSTATUS_M.GETORDERSTATUS_OUT();

                string Key = EncryptionKeyCore.KeyData.AESKEY(_in.HOS_ID);
                string Service_name = _in.DEAL_TYPE == "1" ? "WXPAYQUERY" : "ALIPAYTradeQuery";

                if (_in.DEAL_TYPE == "2")//支付宝
                {
                    Dictionary<string, string> builder = new Dictionary<string, string>();
                    builder.Add("out_trade_no", _in.QUERYID);
                    builder.Add("COMM_HIS", "");
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
                    AlipayTradeQueryStatus F2FQueryResult = InDataRe.GetBusinessData<AlipayTradeQueryStatus>(Key);
                    _out.HIS_RTNXML = F2FQueryResult.response.Body;
                    if (F2FQueryResult.response.Code == "10000" && (F2FQueryResult.response.TradeStatus.Equals("TRADE_SUCCESS") || F2FQueryResult.response.TradeStatus.Equals("TRADE_FINISHED")))
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
                    string HOS_ID = _in.HOS_ID;
                    if (HOS_ID == "44")//微信使用44-1的商户
                    {
                        HOS_ID = "44-1";
                    }
                    Key = EncryptionKeyCore.KeyData.AESKEY(HOS_ID);//解密key重新赋值
                    Dictionary<string, string> builder = new Dictionary<string, string>();
                    builder.Add("out_trade_no", _in.QUERYID);
                    builder.Add("COMM_HIS", "");
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
                   WxPayTradeQueryBuilder F2FQueryResult = InDataRe.GetBusinessData<WxPayTradeQueryBuilder>(Key);
                    if (F2FQueryResult.result_code.ToString() == "SUCCESS" && F2FQueryResult.trade_state == "SUCCESS")
                    {
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

                    POSC2BUnionPay.Models.QHModel.GETORDERSTATUS_IN posin = new POSC2BUnionPay.Models.QHModel.GETORDERSTATUS_IN();
                    posin.HOS_ID = _in.HOS_ID;
                    posin.QUERYID = _in.QUERYID;
                    string TID = "";
                    //获取终端号对应信息
                    var db = new DbMySQLZZJ().Client;
                    SqlSugarModel.Baccountposc2btid model = db.Queryable<SqlSugarModel.Baccountposc2btid>().Where(t => t.HOS_ID == _in.HOS_ID && t.LTERMINAL_SN == _in.LTERMINAL_SN).First();

                    if (model!=null)
                    {
                        TID = model.tid ;
                    }
                    posin.TID = TID;
                    POSC2BUnionPay.Models.QHModel.GETORDERSTATUS_OUT posout = new POSC2BUnionPay.Models.QHModel.GETORDERSTATUS_OUT();
                    string errmsg = "";
                    bool flag = POSC2BUnionPay.POSC2BunionPayHelper.CallService("GETORDERSTATUS", posin, ref posout, ref errmsg);
                    if (!flag)
                    {
                        dataReturn.Code = -1;
                        dataReturn.Msg = errmsg;
                        goto EndPoint;
                    }
                    if (posout.CLBZ == "0")
                    {
                        _out.STATUS = posout.STATUS;
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
