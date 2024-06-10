using CommonModel;
using ConstData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos36_EInvoice.Model;
using Soft.Core;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Data;

namespace OnlineBusHos36_EInvoice.BUS
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

                string HOSPATID = "";
                string YLCARD_TYPE = string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim();
               

                string his_rtnxml = "";

                if (YLCARD_TYPE != "1")
                {
                    if (YLCARD_TYPE == "4" && _in.YLCARD_NO.Length == 15)
                    {
                        _in.YLCARD_NO = QHZZJCommonFunction.Convert15to18(_in.YLCARD_NO);
                    }
                    YLCARD_TYPE = GETHISYLCARDTYPE(YLCARD_TYPE);//卡类型需要转换
                    #region 通过getpatinfo获取患者院内号
                    XmlDocument doc = QHXmlMode.GetBaseXml("GETPATINFO", "1");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", YLCARD_TYPE); //string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_TYPE", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DIS_NO", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NBGRBH", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_OUT", "");

                    string inXml = doc.InnerXml;

                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inXml, ref his_rtnxml))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = "获取人员信息失败：" + his_rtnxml;
                        goto EndPoint;
                    }


                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0" || !dtrev.Columns.Contains("HOSPATID") || string.IsNullOrEmpty(dtrev.Rows[0]["HOSPATID"].ToString()))
                    {
                        dataReturn.Code = -1;
                        dataReturn.Msg = "未获取到有效人员信息";
                        goto EndPoint;
                    }

                    HOSPATID = dtrev.Rows[0]["HOSPATID"].ToString();
                }
                else
                {
                    HOSPATID = _in.YLCARD_NO;
                }
                #endregion

                #region 调用发票列表查询接口
                XmlDocument doc2 = QHXmlMode.GetBaseXml("GETHISISSUEBYSFZNO", "1");
                XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc2, "ROOT/BODY", "BARCODE", HOSPATID);
                his_rtnxml = "";
                string inXml2 = doc2.InnerXml;
                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inXml2, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "获取发票列表失败：" + his_rtnxml;
                    goto EndPoint;
                }

                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = CodeDefine.UIMessageType_Error;
                        dataReturn.Msg = "自助机仅支持更换近九十天内的发票，如需更换较长时间发票，请去人工窗口，谢谢配合！";
                        goto EndPoint;
                    }
                    DataTable dtFP = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/INVOICELIST").Tables[0];

                    string SFTYPE = "";
                    _out.HISISSUELISTS = new List<GetHisIssueBySfzno_OUT.Hisissuelist>();
                    foreach (DataRow dr in dtFP.Rows)
                    {

                        switch (dr["INVOICE_BUSI_TYPE"].ToString())
                        {
                            case "挂号":
                                SFTYPE = "1";
                                break;
                            case "收费":
                                SFTYPE = "2";
                                break;
                            default:
                                SFTYPE = dr["INVOICE_BUSI_TYPE"].ToString();
                                break;
                        }

                        GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                        list.INVOICE_CODE = dr["INVOICE_CODE"].ToString();
                        list.INVOICE_NUMBER = dr["INVOICE_NUM"].ToString();
                        list.INVOICING_PARTY_NAME = "";
                        list.PAYER_PARTY_NAME = dr["PAYER_PARTY_NAME"].ToString();
                        list.TOTAL_AMOUNT = FormatHelper.GetStr(dr["TOTAL_AMOUNT"].ToString());
                        list.SFTYPENAME = dr["SFTYPENAME"].ToString();
                        list.STATUS = "";
                        list.SAVEDDATE_TIME = dr["SAVEDDATE_TIME"].ToString();
                        list.ISPRINT = "0";
                        list.BIZ_CODE = dr["BIZCODE"].ToString();
                        list.SFTYPE = SFTYPE;
                        _out.HISISSUELISTS.Add(list);

                    }
                    if (_out.HISISSUELISTS.Count == 0)
                    {
                        dataReturn.Code = CodeDefine.UIMessageType_Error;
                        dataReturn.Msg = "自助机仅支持更换近七天内的发票，如需更换较长时间发票，请去人工窗口，谢谢配合！";
                        goto EndPoint;
                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    //根据发票时间倒序排序
                    _out.HISISSUELISTS=_out.HISISSUELISTS.OrderByDescending(t=>t.SAVEDDATE_TIME).ToList();

                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 6;
                    dataReturn.Msg = "解析his出参失败";
                    dataReturn.Param = ex.ToString();
                    goto EndPoint;
                }


                #endregion

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

        public static string GETHISYLCARDTYPE(string YLCARD_TYPE)
        {
            switch (YLCARD_TYPE)
            {
                case "1":
                    YLCARD_TYPE = "6"; break;
                case "2":
                    YLCARD_TYPE = "2"; break;
                case "4":
                    YLCARD_TYPE = "4"; break;
                case "7":
                    YLCARD_TYPE = "7"; break;
                default:
                    YLCARD_TYPE = ""; break;
            }
            return YLCARD_TYPE;
        }
    }
}
