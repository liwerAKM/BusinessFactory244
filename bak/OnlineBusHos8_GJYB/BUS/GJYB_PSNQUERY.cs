using CommonModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBusHos8_GJYB.Model;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OnlineBusHos8_GJYB.BUS
{
    class GJYB_PSNQUERY
    {
        public static string B_GJYB_PSNQUERY(string json_in)
        {
            DataReturn dataReturn=GlobalVar.business.PSNQUERY(json_in);
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
            //    string json_out = "";
            //    try
            //    {
            //        GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN _in = JSONSerializer.Deserialize<GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN>(json_in);
            //        GJYB_PSNQUERY_M.GJYB_PSNQUERY_OUT _out = new GJYB_PSNQUERY_M.GJYB_PSNQUERY_OUT();

            //        Dictionary<string, string> dic_sign = new Dictionary<string, string>();
            //        dic_sign.Add("mac", _in.MAC);
            //        dic_sign.Add("ip", _in.IP);
            //        dic_sign.Add("opter_no", _in.USER_ID);

            //        Models.T1101.Data t = new Models.T1101.Data();
            //        string[] cardinfo = _in.PCARDINFO.Split('|');

            //        if (FormatHelper.GetStr(_in.MDTRT_CERT_TYPE) == "01")
            //        {
            //            t.mdtrt_cert_type = "01";
            //            t.mdtrt_cert_no = cardinfo[2].ToString() + "|" +FormatHelper.GetStr(_in.PBUSICARDINFO);
            //        }
            //        else if (FormatHelper.GetStr(_in.MDTRT_CERT_TYPE) == "02")
            //        {
            //            t.mdtrt_cert_type = "02";
            //            //t.mdtrt_cert_no = cardinfo[0].ToString(); //+ "|" + pBusiCardInfo;
            //            t.mdtrt_cert_no = "320524196905012963";
            //            t.card_sn = "";
            //            t.psn_name = ""; //cardinfo[1].ToString();//人员姓名
            //            t.certno = "";// cardinfo[0].ToString();//证件号码
            //            t.begntime = "";
            //            t.psn_cert_type = "01";
            //        }
            //        else if (FormatHelper.GetStr(_in.MDTRT_CERT_TYPE) == "04")
            //        {
            //            t.mdtrt_cert_type = "01";
            //            t.mdtrt_cert_no = cardinfo[2].ToString() + "|" + FormatHelper.GetStr(_in.PBUSICARDINFO);
            //        }
            //        else //社会保障卡
            //        {
            //            t.mdtrt_cert_type = "03";
            //            t.mdtrt_cert_no = cardinfo[2].ToString() + "|" + FormatHelper.GetStr(_in.PBUSICARDINFO);
            //            t.card_sn = cardinfo[3].ToString();//卡识别码
            //            t.psn_name = cardinfo[4].ToString();//人员姓名
            //            t.certno = cardinfo[1].ToString();//证件号码
            //            t.begntime = "";
            //            t.psn_cert_type = "90";
            //        }
            //        Models.T1101.Root root = new Models.T1101.Root()
            //        {
            //            data = t
            //        };
            //        Models.OutputRoot outputRoot = new Models.OutputRoot();
            //        string errormessage = "";
            //        if (GlobalVar.DoBusiness("1101", FormatHelper.GetStr(_in.HOS_ID), "", root, dic_sign, ref outputRoot, ref errormessage))
            //        {
            //            Models.RT1101.Root rT1101 = outputRoot.GetOutput<Models.RT1101.Root>();
            //            JObject card = GlobalVar.GetVaildCard(Newtonsoft.Json.JsonConvert.SerializeObject(outputRoot.output));
            //            _out.PAT_CARD_OUT = JsonConvert.SerializeObject(outputRoot.output);
            //            _out.PAT_NAME = rT1101.baseinfo.psn_name;
            //            _out.PSN_NO = rT1101.baseinfo.psn_no;
            //            _out.SEX = rT1101.baseinfo.gend == "1" ? "男" : "女";
            //            _out.NATION = rT1101.baseinfo.naty;
            //            _out.BIRTHDAY = rT1101.baseinfo.brdy;
            //            _out.AGE = rT1101.baseinfo.age;
            //            _out.SFZ_NO = rT1101.baseinfo.certno;
            //            _out.PAT_NAME = rT1101.baseinfo.psn_name;
            //            if (card != null)
            //            {
            //                _out.BALANCE = FormatHelper.GetStr(card["balc"]);
            //                _out.UNIT_NAME = FormatHelper.GetStr(card["emp_name"]);
            //            }
            //            else
            //            {
            //                _out.BALANCE = "";
            //                _out.UNIT_NAME = "";
            //            }
            //            _out.MDTRT_CERT_TYPE = t.mdtrt_cert_type;
            //            _out.MDTRT_CERT_NO = t.mdtrt_cert_no;
            //            dataReturn.Code = 0;
            //            dataReturn.Msg = "SUCCESS";
            //            dataReturn.Param = JSONSerializer.Serialize(_out);
            //        }
            //        else
            //        {
            //            dataReturn.Code = 6;
            //            dataReturn.Msg = errormessage;

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        dataReturn.Code = 6;
            //        dataReturn.Msg = "程序处理异常";
            //        dataReturn.Param = ex.ToString();
            //    }
            //EndPoint:
            //    json_out = JSONSerializer.Serialize(dataReturn);
            //    return json_out;
        }
    }
}
