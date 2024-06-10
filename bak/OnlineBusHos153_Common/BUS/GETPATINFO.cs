
using CommonModel;
using OnlineBusHos153_Common.Model;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using DB.Core;

namespace OnlineBusHos153_Common.BUS
{
    class GETPATINFO
    {
        public static string B_GETPATINFO(string json_in)
        {
            if (GlobalVar.DoBussiness == "0")
            {
                return UnDoBusiness(json_in);
            }
            else
            {
                return DoBusiness(json_in);
            }
        }
        public static string DoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETPATINFO_M.GETPATINFO_IN _in = JSONSerializer.Deserialize<GETPATINFO_M.GETPATINFO_IN>(json_in);
                GETPATINFO_M.GETPATINFO_OUT _out = new GETPATINFO_M.GETPATINFO_OUT();
                Dictionary<string, string> dic_filter = GlobalVar.Get_Filter(FormatHelper.GetStr(_in.FILTER));

                string HOS_ID = FormatHelper.GetStr(_in.HOS_ID);
                string YLCARD_TYPE = FormatHelper.GetStr(_in.YLCARD_TYPE);//0 0无卡1院内卡2医保卡3 市民卡4身份证
                string YLCARD_NO = FormatHelper.GetStr(_in.YLCARD_NO);
                string SFZ_NO = FormatHelper.GetStr(_in.SFZ_NO);
                string LTERMINAL_SN = FormatHelper.GetStr(_in.LTERMINAL_SN);
                string USER_ID = FormatHelper.GetStr(_in.USER_ID);
                string PAT_CARD_OUT = dic_filter.ContainsKey("PAT_CARD_OUT") ? dic_filter["PAT_CARD_OUT"] : "";
                if (YLCARD_TYPE == "4" && YLCARD_NO.Length == 15)
                {
                    YLCARD_NO = QHZZJCommonFunction.Convert15to18(YLCARD_NO);
                }
                DataTable dtpat_info = null;
                string PAT_NAME = ""; string ADDRESS = ""; string TELE = "";
                SFZ_NO = QHZZJCommonFunction.Convert15to18(SFZ_NO);
                if (YLCARD_TYPE == "12")//电子健康卡
                {
                    if (YLCARD_NO == "")
                    {
                        dataReturn.Code = 999;
                        dataReturn.Msg = "卡号不能为空";
                        goto EndPoint;
                    }

                    RtnModel RtnModel = CommonFunction.GetHealthCardInfo(YLCARD_NO);
                    if (RtnModel.code == "0")
                    {
                        SFZ_NO = RtnModel.data.idNumber;
                        PAT_NAME = RtnModel.data.realname;
                        ADDRESS = RtnModel.data.addr;
                        TELE = RtnModel.data.cellphone;
                    }
                    else
                    {
                        dataReturn.Code = 999;
                        dataReturn.Msg = "无法解析二维码";
                        goto EndPoint;
                    }
                    dtpat_info = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "SFZ_NO='" + SFZ_NO + "' ", "PAT_NAME", "SEX", "MOBILE_NO", "ADDRESS", "SFZ_NO", "pat_id", "GUARDIAN_NAME", "YB_CARDNO", "FZH");
                    if (dtpat_info == null || dtpat_info.Rows.Count == 0)
                    {
                        dataReturn.Code = 1111;
                        dataReturn.Msg = "平台无对应病人信息";
                        _out.IS_EXIST = "0";
                        _out.PAT_NAME = PAT_NAME;
                        AgeAndUnit ageAndUnit = QHZZJCommonFunction.GetAgeBySFZ(SFZ_NO);
                        _out.SEX = ageAndUnit.SEX.ToString();
                        _out.AGE = ageAndUnit.Age.ToString() + ageAndUnit.AgeUnit.ToString();
                        _out.MOBILE_NO = TELE;
                        _out.ADDRESS = ADDRESS;
                        _out.SFZ_NO = SFZ_NO;
                        _out.BIR_DATE = ageAndUnit.BirthDay.ToString("yyyy-MM-dd");
                        goto EndPoint;
                    }

                }
                else if (SFZ_NO != "")
                {
                    switch (YLCARD_TYPE)
                    {
                        case "4"://身份证
                        case "2"://医保卡
                            dtpat_info = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "SFZ_NO='" + SFZ_NO + "'", "PAT_NAME", "SEX", "MOBILE_NO", "ADDRESS", "SFZ_NO", "pat_id", "GUARDIAN_NAME", "YB_CARDNO", "FZH");
                            break;
                    }
                }
                else
                {
                    switch (YLCARD_TYPE)
                    {
                        case "4"://身份证
                            dtpat_info = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "SFZ_NO='" + YLCARD_NO + "'", "PAT_NAME", "SEX", "MOBILE_NO", "ADDRESS", "SFZ_NO", "pat_id", "GUARDIAN_NAME", "YB_CARDNO", "FZH");
                            break;
                        case "2"://医保卡
                            dtpat_info = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "YB_CARDNO='" + YLCARD_NO + "'", "PAT_NAME", "SEX", "MOBILE_NO", "ADDRESS", "SFZ_NO", "pat_id", "GUARDIAN_NAME", "YB_CARDNO", "FZH");
                            break;
                    }
                }
                if (dtpat_info == null || dtpat_info.Rows.Count == 0)
                {
                    dataReturn.Code = 111;
                    dataReturn.Msg = "平台无对应病人信息";
                    goto EndPoint;
                }
                string CLBZ = "";string CLJG = "";
                if (YLCARD_TYPE == "2")
                {
                    //if (dtpat_info.Rows[0]["YB_CARDNO"].ToString().Trim() == "")//先自费后医保病人
                    //{
                    //    dataReturn.Code = 111;
                    //    dataReturn.Msg = "病人自费转医保还未建档";
                    //    goto EndPoint;
                    //}
                    //new Plat.MySQLDAL.BaseFunction().UpdateList("pat_info", "pat_id='" + dtpat_info.Rows[0]["pat_id"].ToString().Trim() + "'", "YB_CARDNO='" + YLCARD_NO + "'");
                    NEWYLCARD(int.Parse(dtpat_info.Rows[0]["pat_id"].ToString().Trim()), HOS_ID, LTERMINAL_SN, YLCARD_TYPE, YLCARD_NO, USER_ID, LTERMINAL_SN, PAT_CARD_OUT, "", "", "", "", out CLBZ, out CLJG);
                    goto EndPoint1;
                }
                if (YLCARD_TYPE == "3")
                {
                    if (dtpat_info.Rows[0]["SMK_CARDNO"].ToString().Trim() == "")//先自费后市民卡病人
                    {
                        dataReturn.Code = 111;
                        dataReturn.Msg = "病人自费转市民卡还未建档";
                        goto EndPoint;
                    }
                    new Plat.MySQLDAL.BaseFunction().UpdateList("pat_info", "pat_id='" + dtpat_info.Rows[0]["pat_id"].ToString().Trim() + "'", "SMK_CARDNO='" + YLCARD_NO + "'");
                }
                #region 每次都获取最新的院内号

                DataTable dtBind_1 = new DataTable();
                NEWYLCARD(int.Parse(dtpat_info.Rows[0]["pat_id"].ToString().Trim()), HOS_ID, LTERMINAL_SN, YLCARD_TYPE, YLCARD_NO, USER_ID, LTERMINAL_SN, PAT_CARD_OUT, "", "", "", "", out CLBZ, out CLJG);
                EndPoint1:
                if (CLBZ == "0")
                {
                    dtBind_1 = new Plat.MySQLDAL.BaseFunction().GetList("pat_card_bind", "pat_id='" + dtpat_info.Rows[0]["pat_id"].ToString().Trim() + "' and hos_id='" + HOS_ID + "' and ylcartd_type='" + YLCARD_TYPE + "'", "YLCARD_NO");
                }
                else
                {
                    dataReturn.Code = 999;
                    dataReturn.Msg = CLJG;
                    goto EndPoint;
                }
                #endregion


                try
                {
                    _out.IS_EXIST = "1";
                    _out.PAT_NAME = dtpat_info.Columns.Contains("PAT_NAME") ? FormatHelper.GetStr(dtpat_info.Rows[0]["PAT_NAME"]) : "";
                    _out.SEX = dtpat_info.Columns.Contains("SEX") ? FormatHelper.GetStr(dtpat_info.Rows[0]["SEX"]) : "";
                    _out.AGE = dtpat_info.Columns.Contains("AGE") ? FormatHelper.GetStr(dtpat_info.Rows[0]["AGE"]) : "";
                    _out.MOBILE_NO = dtpat_info.Columns.Contains("TEL_NO") ? FormatHelper.GetStr(dtpat_info.Rows[0]["TEL_NO"]) : "";
                    _out.ADDRESS = dtpat_info.Columns.Contains("ADDRESS") ? FormatHelper.GetStr(dtpat_info.Rows[0]["ADDRESS"]) : "";
                    _out.SFZ_NO = dtpat_info.Columns.Contains("SFZ_NO") ? FormatHelper.GetStr(dtpat_info.Rows[0]["SFZ_NO"]) : "";
                    _out.HOSPATID = dtpat_info.Columns.Contains("OPT_SN") ? FormatHelper.GetStr(dtpat_info.Rows[0]["OPT_SN"]) : "";
                    _out.BIR_DATE = dtpat_info.Columns.Contains("BIR_DATE") ? FormatHelper.GetStr(dtpat_info.Rows[0]["BIR_DATE"]) : "";
                    _out.GUARDIAN_NAME = dtpat_info.Columns.Contains("GUARDIAN_NAME") ? FormatHelper.GetStr(dtpat_info.Rows[0]["GUARDIAN_NAME"]) : "";
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:" + ex + ";";
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
        public static string UnDoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                GETPATINFO_M.GETPATINFO_IN _in = JSONSerializer.Deserialize<GETPATINFO_M.GETPATINFO_IN>(json_in);
                GETPATINFO_M.GETPATINFO_OUT _out = new GETPATINFO_M.GETPATINFO_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETPATINFO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_TYPE", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DIS_NO", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NBGRBH", "");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BARCODE", "");

                string YB_OUT = "";
                if (_in.YLCARD_TYPE == "2")
                {
                    DataTable dt = DbHelperMySQLInsur.Query("select * from chs_psn where psn_no='" + _in.YLCARD_NO + "' ").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        YB_OUT=GlobalVar.Base64Encode( dt.Rows[0]["chsOutput1101"].ToString());
                    }
                }
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_OUT", YB_OUT);

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }

                _out.HIS_RTNXML = his_rtnxml;
                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];

                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        _out.IS_EXIST = "0";
                        goto EndPoint1;
                    }
                    _out.IS_EXIST = "1";
                    _out.PAT_NAME = dtrev.Columns.Contains("PAT_NAME") ? FormatHelper.GetStr(dtrev.Rows[0]["PAT_NAME"]) : "";
                    _out.SEX = dtrev.Columns.Contains("SEX") ? FormatHelper.GetStr(dtrev.Rows[0]["SEX"]) : "";
                    _out.AGE = dtrev.Columns.Contains("AGE") ? FormatHelper.GetStr(dtrev.Rows[0]["AGE"]) : "";
                    _out.MOBILE_NO = dtrev.Columns.Contains("TEL_NO") ? FormatHelper.GetStr(dtrev.Rows[0]["TEL_NO"]) : "";
                    _out.ADDRESS = dtrev.Columns.Contains("ADDRESS") ? FormatHelper.GetStr(dtrev.Rows[0]["ADDRESS"]) : "";
                    _out.SFZ_NO = dtrev.Columns.Contains("SFZ_NO") ? FormatHelper.GetStr(dtrev.Rows[0]["SFZ_NO"]) : "";
                    _out.HOSPATID = dtrev.Columns.Contains("BARCODE") ? FormatHelper.GetStr(dtrev.Rows[0]["BARCODE"]) : "";
                    _out.BIR_DATE = dtrev.Columns.Contains("BIR_DATE") ? FormatHelper.GetStr(dtrev.Rows[0]["BIR_DATE"]) : QHZZJCommonFunction.GetBirthdayByIDCard(_out.SFZ_NO);
                    _out.GUARDIAN_NAME = dtrev.Columns.Contains("GUARDIAN_NAME") ? FormatHelper.GetStr(dtrev.Rows[0]["GUARDIAN_NAME"]) : "";
                EndPoint1:
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:" + ex + ";";
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }

        /// <summary>
        /// 获取新医院医保卡
        /// </summary>
        /// <param name="PAT_ID"></param>
        /// <param name="HOS_ID"></param>
        /// <param name="lter"></param>
        /// <param name="ylcard_type"></param>
        /// <param name="yb_out"></param>
        /// <param name="CLBZ"></param>
        /// <param name="CLJG"></param>
        public static void NEWYLCARD(int PAT_ID, string HOS_ID, string lter, string YLCARD_TYPE, string YLCARD_NO, string USER_ID, string lTERMINAL_SN, string YB_OUT, string PAT_TYPE, string DIS_NO, string NBGRBH, string BARCODE, out string CLBZ, out string CLJG)
        {
            try
            {
                DataTable dtpatinfo = new Plat.MySQLDAL.BaseFunction().GetList("pat_info", "pat_id='" + PAT_ID + "'", "PAT_NAME,SEX,BIRTHDAY,ADDRESS,GUARDIAN_NAME,GUARDIAN_SFZ_NO,SFZ_NO,MOBILE_NO");

                XmlDocument doc = new XmlDocument();
                doc = QHXmlMode.GetBaseXml("SENDCARDINFO", "0");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARTD_TYPE", YLCARD_TYPE == "2" ? YLCARD_TYPE : "4");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", YLCARD_NO.Trim() == "2" ? YLCARD_NO : FormatHelper.GetStr(dtpatinfo.Rows[0]["SFZ_NO"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", FormatHelper.GetStr(dtpatinfo.Rows[0]["PAT_NAME"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", FormatHelper.GetStr(dtpatinfo.Rows[0]["SEX"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIRTHDAY", FormatHelper.GetStr(dtpatinfo.Rows[0]["BIRTHDAY"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_SFZ_NO", FormatHelper.GetStr(dtpatinfo.Rows[0]["GUARDIAN_SFZ_NO"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_NAME", FormatHelper.GetStr(dtpatinfo.Rows[0]["GUARDIAN_NAME"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", FormatHelper.GetStr(dtpatinfo.Rows[0]["SFZ_NO"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", FormatHelper.GetStr(dtpatinfo.Rows[0]["MOBILE_NO"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "ADDRESS", FormatHelper.GetStr(dtpatinfo.Rows[0]["ADDRESS"]));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPERATOR", USER_ID);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_OUT", YB_OUT);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FZH", "");
                string his_rtnxml = "";
                if (!GlobalVar.CALLSERVICE(HOS_ID, doc.InnerXml, ref his_rtnxml))
                {
                    CLBZ = "1";
                    CLJG = "his_rtnxml";
                    return;
                }

                DataTable dt_result = XMLHelper.X_GetXmlData(his_rtnxml, "ROOT/BODY").Tables[0];
                if (dt_result != null & dt_result.Rows.Count > 0)
                {
                    if (dt_result.Rows[0]["CLBZ"].ToString().Trim() == "0")
                    {
                        CLBZ = "0";
                        CLJG = "";
                        if (dt_result.Columns.Contains("HOSPATID"))
                        {
                            BARCODE = dt_result.Rows[0]["HOSPATID"].ToString().Trim();
                        }
                        else
                        {
                            BARCODE = dt_result.Rows[0]["BARCODE"].ToString().Trim();
                        }

                        if (new Plat.MySQLDAL.BaseFunction().GetList("pat_card_bind", "HOS_ID=" + HOS_ID + " and pat_id = '" + PAT_ID + "' and YLCARTD_TYPE=" + YLCARD_TYPE + "", "YLCARD_NO").Rows.Count > 0)
                        {
                            new Plat.MySQLDAL.BaseFunction().UpdateList("pat_card_bind", "HOS_ID=" + HOS_ID + " and pat_id = '" + PAT_ID + "' and YLCARTD_TYPE=" + YLCARD_TYPE + "", "YLCARD_NO='" + YLCARD_NO + "' and BAND_TIME='"+DateTime.Now+"'");
                        }
                        else
                        {
                            Plat.Model.pat_card_bind bind = new Plat.Model.pat_card_bind();
                            bind.HOS_ID = HOS_ID;
                            bind.PAT_ID = PAT_ID;
                            bind.YLCARTD_TYPE = Convert.ToInt32(YLCARD_TYPE); //多次建档
                            bind.YLCARD_NO = BARCODE;
                            bind.MARK_BIND = 1;
                            bind.BAND_TIME = DateTime.Now;
                            new Plat.MySQLDAL.pat_card_bind().Add(bind);
                        }
                    }
                    else
                    {
                        CLBZ = dt_result.Rows[0]["CLBZ"].ToString().Trim();
                        CLJG = dt_result.Rows[0]["CLJG"].ToString().Trim();
                        return;
                    }
                }
                else
                {
                    CLBZ = "6";
                    CLJG = "医院建档失败";
                    return;
                }
            }
            catch (Exception ex)
            {
                CLBZ = "6";
                CLJG = ex.ToString();
            }

        }
    }
}
