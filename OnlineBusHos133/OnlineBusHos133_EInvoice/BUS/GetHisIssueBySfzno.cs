using CommonModel;
using ConstData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos133_EInvoice.Model;
using Soft.Core;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Xml;
using System.Data;

namespace OnlineBusHos133_EInvoice.BUS
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
                bool flag = false;
                if (_in.IS_REPRINT.Equals("1"))//补打
                {
                    flag = true;
                }
                string patient_id = "";
                string SFZ_NO = _in.SFZ_NO;
                string HOSPATID=_in.HOSPATID;
                dic.Add("Bizcode", "");//发票号

                if (string.IsNullOrEmpty(SFZ_NO) || string.IsNullOrEmpty(HOSPATID))//
                {
                    string YLCARD_TYPE = FormatHelper.GetStr(_in.YLCARD_TYPE).Equals("1") ? "6" : FormatHelper.GetStr(_in.YLCARD_TYPE);
                    string posturl = "http://130.150.206.40:8003/service.asmx";
                    string inxml = string.Format("<ROOT><HEADER><TYPE>GETPATINFO</TYPE><CZLX>4</CZLX></HEADER><BODY><YLCARD_TYPE>{0}</YLCARD_TYPE><YLCARD_NO>{1}</YLCARD_NO><SFZ_NO></SFZ_NO><PAT_NAME></PAT_NAME><HOS_ID>133</HOS_ID><lTERMINAL_SN>qhzzj1</lTERMINAL_SN><USER_ID>qhzzj001</USER_ID></BODY></ROOT>", YLCARD_TYPE, _in.YLCARD_NO);
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("xmlstr", inxml);
                    XmlDocument doc_sec = WebServiceHelper.QuerySoapWebService(posturl, "BusinessHos", hashtable);
                    string outxml = doc_sec.InnerText;

                    DataTable dt = XMLHelper.X_GetXmlData(outxml, "ROOT/BODY").Tables[0];
                    if (dt.Columns.Contains("CLBZ") && dt.Rows[0]["CLBZ"].ToString() == "0")
                    {
                        SFZ_NO = dt.Rows[0]["SFZ_NO"].ToString();
                        if (string.IsNullOrEmpty(HOSPATID))
                        {
                            patient_id = dt.Rows[0]["HOSPATID"].ToString();
                        }

                    }
                    else
                    {
                        dataReturn.Code = -1;
                        dataReturn.Msg = "查询人员信息失败";
                        goto EndPoint;
                    }
                }
                else
                {
                    patient_id = HOSPATID;
                }

                dic.Add("sfz_no", SFZ_NO);
                if (string.IsNullOrEmpty(SFZ_NO))
                {
                    dic.Add("patient_id", patient_id);//院内卡号或门诊号
                }
                else
                {
                    dic.Add("patient_id", "");//院内卡号或门诊号
                }
                
                if (flag)
                {
                    dic.Add("begin_date", DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd"));
                    dic.Add("end_date",  DateTime.Now.ToString("yyyy-MM-dd") );
                }
                else
                {
                    dic.Add("begin_date", DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
                    dic.Add("end_date", DateTime.Now.ToString("yyyy-MM-dd"));
                }
              
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

                string json = JSONSerializer.Serialize(root_rtn.ROOT.BODY);
                BODY body = JSONSerializer.Deserialize<BODY>(json);
                if (body.CLBZ != "0")
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = body.CLJG;
                    goto EndPoint;
                }

                _out.HISISSUELISTS = new List<GetHisIssueBySfzno_OUT.Hisissuelist>();

                if (body.CLBZ == "0" && body.hisissuelist.Count > 0)
                {
                    foreach (HisissuelistItem item in body.hisissuelist)
                    {
                        if (flag && item.print_times < 6)
                        {
                            GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                            list.INVOICE_CODE = item.invoice_code;
                            list.INVOICE_NUMBER = item.invoice_number;
                            list.INVOICING_PARTY_NAME = item.invoicing_party_name;
                            list.PAYER_PARTY_NAME = item.payer_party_name;
                            list.TOTAL_AMOUNT = FormatHelper.GetStr(item.total_amount);
                            list.SFTYPENAME = item.sftypename;
                            list.STATUS = item.STATUS;
                            list.SAVEDDATE_TIME = item.issue_date;
                            list.ISPRINT = "0";
                            _out.HISISSUELISTS.Add(list);
                        }
                        else if (FormatHelper.GetDecimal(item.isprint) == 0)
                        {
                            GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                            list.INVOICE_CODE = item.invoice_code;
                            list.INVOICE_NUMBER = item.invoice_number;
                            list.INVOICING_PARTY_NAME = item.invoicing_party_name;
                            list.PAYER_PARTY_NAME = item.payer_party_name;
                            list.TOTAL_AMOUNT = FormatHelper.GetStr(item.total_amount);
                            list.SFTYPENAME = item.sftypename;
                            list.STATUS = item.STATUS;
                            list.SAVEDDATE_TIME = item.issue_date;
                            list.ISPRINT = FormatHelper.GetDecimal(item.isprint) != 1 ? "0" : "1";
                            _out.HISISSUELISTS.Add(list);

                        }
                    }


                }

                if (!string.IsNullOrEmpty(SFZ_NO)&&!string.IsNullOrEmpty(patient_id))//身份证查不到就再调用一次
                {
                    dic["patient_id"] = patient_id;
                    dic["sfz_no"] = "";
                    /*
                    dic.Clear();
                    dic.Add("HOS_ID", FormatHelper.GetStr(_in.HOS_ID));
                    dic.Add("Bizcode", "");//发票号
                    dic.Add("sfz_no", "");//发票号
                    dic.Add("patient_id", patient_id);//发票号

                    if (flag)
                    {
                        dic.Add("begin_date", DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd"));
                        dic.Add("end_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        dic.Add("begin_date", DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
                        dic.Add("end_date", DateTime.Now.ToString("yyyy-MM-dd"));
                    }
                    */
                    root.BODY = dic;
                    injson = JSONSerializer.Serialize(root);
                    his_rtnxml = "";

                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, injson, ref his_rtnxml))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml;
                        goto EndPoint;
                    }
                    root_rtn = JSONSerializer.Deserialize<Root_rtn>(his_rtnxml);

                    json = JSONSerializer.Serialize(root_rtn.ROOT.BODY);
                    body = JSONSerializer.Deserialize<BODY>(json);

                    if (body.CLBZ == "0" && body.hisissuelist.Count > 0)
                    {
                        foreach (HisissuelistItem item in body.hisissuelist)
                        {
                            if (flag && item.print_times < 6)
                            {
                                GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                                list.INVOICE_CODE = item.invoice_code;
                                list.INVOICE_NUMBER = item.invoice_number;
                                list.INVOICING_PARTY_NAME = item.invoicing_party_name;
                                list.PAYER_PARTY_NAME = item.payer_party_name;
                                list.TOTAL_AMOUNT = FormatHelper.GetStr(item.total_amount);
                                list.SFTYPENAME = item.sftypename;
                                list.STATUS = item.STATUS;
                                list.SAVEDDATE_TIME = item.issue_date;
                                list.ISPRINT = "0";
                                _out.HISISSUELISTS.Add(list);
                            }
                            else if (FormatHelper.GetDecimal(item.isprint) == 0)
                            {
                                GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                                list.INVOICE_CODE = item.invoice_code;
                                list.INVOICE_NUMBER = item.invoice_number;
                                list.INVOICING_PARTY_NAME = item.invoicing_party_name;
                                list.PAYER_PARTY_NAME = item.payer_party_name;
                                list.TOTAL_AMOUNT = FormatHelper.GetStr(item.total_amount);
                                list.SFTYPENAME = item.sftypename;
                                list.STATUS = item.STATUS;
                                list.SAVEDDATE_TIME = item.issue_date;
                                list.ISPRINT = FormatHelper.GetDecimal(item.isprint) != 1 ? "0" : "1";
                                _out.HISISSUELISTS.Add(list);

                            }
                        }
                    }
                }
              
                if (_out.HISISSUELISTS.Count == 0)
                {
                    dataReturn.Code = CodeDefine.UIMessageType_Error;
                    dataReturn.Msg = "无可打印的发票!";
                    goto EndPoint;
                }

                //去重
                _out.HISISSUELISTS=_out.HISISSUELISTS.GroupBy(x=>x.INVOICE_NUMBER).Select(x=>x.First()).ToList();
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
            public string issue_date { get; set; }
            public string isprint { get; set; }
            public int print_times { get; set; }
        }
    }
}
