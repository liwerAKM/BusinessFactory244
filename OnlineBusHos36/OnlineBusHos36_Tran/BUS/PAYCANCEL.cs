using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data;

namespace OnlineBusHos36_Tran.BUS
{
    class PAYCANCEL
    {
        public static string B_PAYCANCEL(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.PAYCANCEL_M.PAYCANCEL_IN _in = JSONSerializer.Deserialize<Model.PAYCANCEL_M.PAYCANCEL_IN>(json_in);
                Model.PAYCANCEL_M.PAYCANCEL_OUT _out = new Model.PAYCANCEL_M.PAYCANCEL_OUT();
                string Key = EncryptionKeyCore.KeyData.AESKEY(_in.HOS_ID);
                string Service_name = _in.DEAL_TYPE == "1" ? "WXPAYCLOSEORDER" : "ALIPAYTRADECANCEL";
                if (_in.DEAL_TYPE == "2")//支付宝
                {

                    System.Collections.Generic.Dictionary<string, string> inData = new Dictionary<string, string>();
                    inData.Add("out_trade_no", _in.QUERYID);


                    InteractiveData InDataRe = PubFunc.F2FPay(_in.HOS_ID,inData, "ALIPAYTRADECANCEL");// ALIPAYTRADECLOSE

                    AlipayTradeCloseResponse F2FQueryResult = InDataRe.GetBusinessData<AlipayTradeCloseResponse>(Key);


                    if (F2FQueryResult.Code == "10000")
                    {
                        _out.STATUS = "1";
                    }
                    else
                    {
                        _out.STATUS = "0";
                       
                    }

                    _out.STATUS = "1";
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
                else if (_in.DEAL_TYPE == "1")//微信
                {
                    Dictionary<string, string> builder = new Dictionary<string, string>();
                    builder.Add("out_trade_no", _in.QUERYID);
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
                    WxPayCloseOrder F2FPayResult = InDataRe.GetBusinessData<WxPayCloseOrder>(Key);
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

                else
                {

                    POSC2BUnionPay.Models.QHModel.PAYCANCEL_IN posin = new POSC2BUnionPay.Models.QHModel.PAYCANCEL_IN();
                    posin.HOS_ID = _in.HOS_ID;
                    posin.QUERYID = _in.QUERYID;
                    string TID = "";
                    //获取终端号对应信息
                    var db = new DbMySQLZZJ().Client;
                    SqlSugarModel.Baccountposc2btid model = db.Queryable<SqlSugarModel.Baccountposc2btid>().Where(t => t.HOS_ID == _in.HOS_ID && t.LTERMINAL_SN == _in.LTERMINAL_SN).First();
                    if (model != null)
                    {
                        TID = model.tid;
                    }
                    posin.TID = TID;
                    POSC2BUnionPay.Models.QHModel.PAYCANCEL_OUT posout = new POSC2BUnionPay.Models.QHModel.PAYCANCEL_OUT();
                    string errmsg = "";
                    bool flag = POSC2BUnionPay.POSC2BunionPayHelper.CallService("PAYCANCEL", posin, ref posout, ref errmsg);
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
