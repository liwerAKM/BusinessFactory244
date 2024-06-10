using CommonModel;
using ConstData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos172_EInvoice.Model;
using Soft.Core;
using Newtonsoft.Json.Linq;

namespace OnlineBusHos172_EInvoice.BUS
{
    class GetHisIssueBySfzno
    {
        public static string B_GetHisIssueBySfzno(string json_in)
        {
            DataReturn dataReturn = new DataReturn();

            try
            {

                GetHisIssueBySfzno_IN _in = JSONSerializer.Deserialize<GetHisIssueBySfzno_IN>(json_in);
                GetHisIssueBySfzno_OUT _out = new GetHisIssueBySfzno_OUT();

                Root root = new Root();
                ROOT ROOT = new ROOT();
                HEADER head = new HEADER();
                head.MODULE = "";
                head.TYPE = "GETHISISSUEBYSFZNO";
                head.CZLX = "";
                head.SOURCE = "ZZJ";
                ROOT.HEADER = head;
                root.ROOT = ROOT;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("HOS_ID", FormatHelper.GetStr(_in.HOS_ID));
                dic.Add("sfz_no", FormatHelper.GetStr(_in.SFZ_NO));
                //如果T开头，为bizcode，否则用patient_id解锁
                string Bizcode = "";
                string patient_id = "";
                //if (_in.YLCARD_TYPE == "1"|| _in.YLCARD_TYPE == "12"|| _in.YLCARD_TYPE == "13")
                //{
                //    if (_in.YLCARD_NO.ToString().Substring(0, 1).Equals("T"))
                //    {
                //        Bizcode = _in.YLCARD_NO.ToString();
                //    }
                //    else
                //    {
                //        patient_id = _in.YLCARD_NO.ToString();
                //    }
                //}

                dic.Add("Bizcode", _in.YLCARD_TYPE == "13" ? FormatHelper.GetStr(_in.YLCARD_NO) : "");//发票号
                dic.Add("patient_id", (_in.YLCARD_TYPE == "1" || _in.YLCARD_TYPE == "12") ? FormatHelper.GetStr(_in.YLCARD_NO) : "");//院内卡号或门诊号
                //dic.Add("Bizcode", Bizcode);
                //dic.Add("patient_id", patient_id);
                dic.Add("begin_date", FormatHelper.GetStr(_in.BEGIN_DATE) == "" ? DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"): FormatHelper.GetStr(_in.BEGIN_DATE));
                dic.Add("end_date", FormatHelper.GetStr(_in.END_DATE) == "" ? DateTime.Now.ToString("yyyy-MM-dd") : FormatHelper.GetStr(_in.END_DATE));
                root.BODY = dic;
                string injson = JSONSerializer.Serialize(root);
                string his_rtnxml = "";

                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, injson, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                Root_rtn root_rtn = JSONSerializer.Deserialize<Root_rtn>(his_rtnxml);

                string json=JSONSerializer.Serialize(root_rtn.ROOT.BODY);
                BODY body = JSONSerializer.Deserialize<BODY>(json);
                if (body.CLBZ != "0")
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = body.CLJG;
                    goto EndPoint;
                }
                if (body.hisissuelist.Count == 0)
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "未查询到发票";
                    goto EndPoint;
                }
                _out.HISISSUELISTS = new List<GetHisIssueBySfzno_OUT.Hisissuelist>();
                foreach (HisissuelistItem item in body.hisissuelist)
                {
                    if (FormatHelper.GetDecimal(item.isprint)== 0)
                    {
                        GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                        list.INVOICE_CODE = item.invoice_code;
                        list.INVOICE_NUMBER = item.invoice_number;
                        list.INVOICING_PARTY_NAME = item.invoicing_party_name;
                        list.PAYER_PARTY_NAME = item.payer_party_name;
                        list.TOTAL_AMOUNT = FormatHelper.GetStr(item.total_amount);
                        list.SFTYPENAME = item.sftypename;
                        list.STATUS = item.STATUS;
                        list.SAVEDDATE_TIME = item.saveddate_time;
                        list.ISPRINT = FormatHelper.GetDecimal(item.isprint) != 1 ? "0" : "1";
                        _out.HISISSUELISTS.Add(list);
                    }
                }
                if (_out.HISISSUELISTS.Count == 0)
                {
                    dataReturn.Code = CodeDefine.UIMessageType_Error;
                    dataReturn.Msg = "无可打印的发票!";
                    goto EndPoint;
                }
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);

            }
            catch (Exception ex)
            {
                dataReturn.Code = 7;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.ToString();
            }
        EndPoint:
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
        public static string B_GetHisIssueBySfzno_b(string json_in)
        {
            GetHisIssueBySfzno_IN _in = JSONSerializer.Deserialize<GetHisIssueBySfzno_IN>(json_in);
            GetHisIssueBySfzno_OUT _out = new GetHisIssueBySfzno_OUT();
            DataReturn dataReturn = new DataReturn();
            if (_in.HOSPATID == "1")
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "无明细";
            }
            else
            {

                _out.HISISSUELISTS = new List<GetHisIssueBySfzno_OUT.Hisissuelist>();
                GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                list.INVOICE_CODE = "32060121";
                list.INVOICE_NUMBER = "0212521549";
                list.INVOICING_PARTY_NAME = "南京市江宁区湖熟街道社区卫生服务中心";
                list.PAYER_PARTY_NAME = "金淑暧";
                list.TOTAL_AMOUNT = "67.00";
                list.SFTYPENAME = "门诊";
                list.STATUS = "已红冲";
                list.SAVEDDATE_TIME = "2021-07-09 17:21";
                list.ISPRINT = "0";
                _out.HISISSUELISTS.Add(list);

                list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                list.INVOICE_CODE = "32060121";
                list.INVOICE_NUMBER = "0212521548";
                list.INVOICING_PARTY_NAME = "南京市江宁区湖熟街道社区卫生服务中心";
                list.PAYER_PARTY_NAME = "金淑暧";
                list.TOTAL_AMOUNT = "10.00";
                list.SFTYPENAME = "门诊";
                list.STATUS = "";
                list.SAVEDDATE_TIME = "2021-07-09 17:06";
                list.ISPRINT = "0";
                _out.HISISSUELISTS.Add(list);


                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);


            }
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }

        public class BODY
        {
            public string CLBZ { get; set; }
            public string CLJG { get; set; }
            public List<HisissuelistItem> hisissuelist { get; set; }
        }
        public class HisissuelistItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string invoice_code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string invoice_number { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public decimal total_amount { get; set; }
            /// <summary>
            /// 南医大二附院
            /// </summary>
            public string invoicing_party_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string payer_party_name { get; set; }
            /// <summary>
            /// 门诊
            /// </summary>
            public string sftypename { get; set; }
            /// <summary>
            /// 已红冲
            /// </summary>
            public string STATUS { get; set; }
            public string saveddate_time { get; set; }
            public string isprint { get; set; }
        }
    }
}
