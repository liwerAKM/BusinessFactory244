using CommonModel;
using DB.Core;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos324_Common.BUS
{
    class GETPATRECORD
    {
        public static string B_GETPATRECORD(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETPATRECORD_M.GETPATRECORD_IN _in = JSONSerializer.Deserialize<Model.GETPATRECORD_M.GETPATRECORD_IN>(json_in);
                Model.GETPATRECORD_M.GETPATRECORD_OUT _out = new Model.GETPATRECORD_M.GETPATRECORD_OUT();
                string HOS_ID = FormatHelper.GetStr(_in.HOS_ID);
                string SFZ_NO = FormatHelper.GetStr(_in.SFZ_NO);
                string PAT_NAME= FormatHelper.GetStr(_in.PAT_NAME);
                string SEX = FormatHelper.GetStr(_in.SEX);
                string ADDRESS = FormatHelper.GetStr(_in.ADDRESS);
                string BIRTHDAY = FormatHelper.GetStr(_in.BIR_DATE);
                string GUARDIAN_NAME = FormatHelper.GetStr(_in.GUARDIAN_NAME);
                string GUARDIAN_SFZ_NO = FormatHelper.GetStr(_in.GUARDIAN_SFZ_NO);
                string MOBILE_NO = FormatHelper.GetStr(_in.MOBILE_NO);
                string YLCARD_TYPE = PubFunc.GETHISYLCARDTYPE(_in.YLCARD_TYPE);
                string YLCARD_NO = FormatHelper.GetStr(_in.YLCARD_NO);
                string USER_ID = FormatHelper.GetStr(_in.USER_ID);
                string PAT_CARD_OUT = FormatHelper.GetStr(_in.PAT_CARD_OUT);
                string lTERMINAL_SN = FormatHelper.GetStr(_in.lTERMINAL_SN);
                string NATION = FormatHelper.GetStr(_in.NATION);
                string TYPE = FormatHelper.GetStr(_in.TYPE);
                AgeAndUnit ageAndUnit = QHZZJCommonFunction.GetAgeBySFZ(SFZ_NO);
                XmlDocument doc = new XmlDocument();
                if (TYPE != "1")
                {
                    if (YLCARD_TYPE == "2")//医保卡自己取读卡出参
                    {
                        DataTable dtPsnDetail = DbHelperMySQLInsur.Query("select * from chs_psn where psn_no='" + YLCARD_NO + "'").Tables[0];
                        if (dtPsnDetail.Rows.Count == 0)
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "未取到医保信息，建档失败";
                            goto EndPoint;
                        }

                        PAT_CARD_OUT = dtPsnDetail.Rows[0]["chsOutput1101"].ToString();
                    }

                    doc = QHXmlMode.GetBaseXml("SENDCARDINFO", "0");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", YLCARD_TYPE == "2" ? YLCARD_TYPE : "4");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", YLCARD_TYPE.Trim() == "2" ? YLCARD_NO : SFZ_NO);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", PAT_NAME);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SEX", SEX);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIRTHDAY", ageAndUnit.BirthDay.ToString("yyyy-MM-dd"));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_SFZ_NO", GUARDIAN_SFZ_NO);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "GUARDIAN_NAME", GUARDIAN_NAME);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", SFZ_NO);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", MOBILE_NO);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "ADDRESS", string.IsNullOrEmpty(ADDRESS)?"南京":ADDRESS);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPERATOR", lTERMINAL_SN);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_OUT", PAT_CARD_OUT);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "NATIONAL", NATION);
                }
                else
                {
                    string HOSPATID = FormatHelper.GetStr(_in.HOS_PAT_ID);
                    if (string.IsNullOrEmpty(HOSPATID))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = "病人院内号不能为空";
                        goto EndPoint;
                    }
                    doc = QHXmlMode.GetBaseXml("UPDATEIDCARDORMOBILE", "0");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "lTERMINAL_SN", lTERMINAL_SN);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", USER_ID);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", HOSPATID);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", SFZ_NO);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MOBILE_NO", MOBILE_NO);
                }

                string inxml = doc.InnerXml;
                string his_rtnxml = "";

                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
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
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        goto EndPoint;
                    }
                    else if (TYPE == "1")
                    {
                        dataReturn.Code = 0;
                        dataReturn.Msg = "SUCCESS";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }

                    if (dtrev.Columns.Contains("HOSPATID"))
                    {
                        _out.HOSPATID = dtrev.Columns.Contains("HOSPATID") ? FormatHelper.GetStr(dtrev.Rows[0]["HOSPATID"]) : "";
                    }
                    else
                    {
                        _out.HOSPATID = dtrev.Columns.Contains("BARCODE") ? FormatHelper.GetStr(dtrev.Rows[0]["BARCODE"]) : "";
                    }

                    var db = new DbMySQLZZJ().Client;
                    SqlSugarModel.PatInfo patInfo = new SqlSugarModel.PatInfo();
                    SqlSugarModel.PatCard patCard = new SqlSugarModel.PatCard();
                    SqlSugarModel.PatCardBind patCardBind = new SqlSugarModel.PatCardBind();
                    //如果身份证不为空，先用身份证查询
                    if (!string.IsNullOrEmpty(_in.SFZ_NO))
                    {
                        patInfo = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.SFZ_NO == _in.SFZ_NO).First();

                        if (patInfo != null)
                        {
                            patCard = db.Queryable<SqlSugarModel.PatCard>().Where(t => t.PAT_ID == patInfo.PAT_ID && t.YLCARD_TYPE == FormatHelper.GetInt(_in.YLCARD_TYPE) && t.YLCARD_NO == _in.YLCARD_NO).First();
                            //如果不同的卡对应不同的HOSPTAID，需要加上卡号去查
                            patCardBind = db.Queryable<SqlSugarModel.PatCardBind>().Where(t => t.HOS_ID == _in.HOS_ID && t.PAT_ID == patInfo.PAT_ID).First();

                        }
                    }
                    else   //通过卡获取
                    {
                        patCard = db.Queryable<SqlSugarModel.PatCard>().Where(t => t.YLCARD_TYPE == FormatHelper.GetInt(_in.YLCARD_TYPE) && t.YLCARD_NO == _in.YLCARD_NO).First();

                        if (patCard != null)
                        {
                            patInfo = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.PAT_ID == patCard.PAT_ID).First();
                            //如果不同的卡对应不同的HOSPTAID，需要加上卡号去查
                            patCardBind = db.Queryable<SqlSugarModel.PatCardBind>().Where(t => t.HOS_ID == _in.HOS_ID && t.PAT_ID == patInfo.PAT_ID).First();

                        }
                    }
                    if (patInfo == null)
                    {

                        int pat_id = 0;
                        if (!PubFunc.GetSysID("pat_info", out pat_id))
                        {
                            dataReturn.Code = 5;
                            dataReturn.Msg = "[提示]建档失败，请联系医院处理";
                            dataReturn.Param = "获取pat_info的sysid失败";
                            goto EndPoint;
                        }

                        patInfo = new SqlSugarModel.PatInfo();
                        patInfo.PAT_ID = pat_id;
                        patInfo.SFZ_NO = SFZ_NO;
                        patInfo.PAT_NAME = PAT_NAME;
                        patInfo.SEX = SEX;
                        patInfo.BIRTHDAY = BIRTHDAY;
                        patInfo.ADDRESS = ADDRESS;
                        patInfo.MOBILE_NO = MOBILE_NO;
                        patInfo.GUARDIAN_NAME = GUARDIAN_NAME;
                        patInfo.GUARDIAN_SFZ_NO = GUARDIAN_SFZ_NO;
                        patInfo.CREATE_TIME = DateTime.Now;
                        patInfo.MARK_DEL = false;
                        patInfo.OPER_TIME = DateTime.Now;
                        patInfo.NOTE = _in.LTERMINAL_SN;
                        db.Insertable(patInfo).ExecuteCommand();
                    }
                    if (patCard == null)
                    {
                        patCard = new SqlSugarModel.PatCard();
                        patCard.PAT_ID = patInfo.PAT_ID;
                        patCard.YLCARD_TYPE = FormatHelper.GetInt(_in.YLCARD_TYPE);
                        patCard.YLCARD_NO = _in.YLCARD_NO;
                        patCard.CREATE_TIME = DateTime.Now;
                        patCard.MARK_DEL = "0";
                        db.Insertable(patCard).ExecuteCommand();
                    }
                    if (patCardBind == null)
                    {
                        patCardBind = new SqlSugarModel.PatCardBind();
                        patCardBind.HOS_ID = _in.HOS_ID;
                        patCardBind.PAT_ID = patInfo.PAT_ID;
                        patCardBind.YLCARD_TYPE = FormatHelper.GetInt(_in.YLCARD_TYPE);
                        patCardBind.YLCARD_NO = _in.YLCARD_NO;
                        patCardBind.HOSPATID = _out.HOSPATID;
                        patCardBind.MARK_BIND = 1;
                        patCardBind.BAND_TIME = DateTime.Now;
                        db.Insertable(patCardBind).ExecuteCommand();
                    }


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
                dataReturn.Msg = "程序处理异常";
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}
