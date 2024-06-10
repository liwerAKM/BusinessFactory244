using CommonModel;
using DB.Core;
using Newtonsoft.Json;
using OnlineBusHos319_Common.Model;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using System.Text;
using System.Xml;
using static OnlineBusHos319_Common.Model.HISModels.A101;

namespace OnlineBusHos319_Common.BUS
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

                string inputjson = "";
                if (TYPE != "1")
                {
                    //if (YLCARD_TYPE == "2")//医保卡自己取读卡出参
                    //{
                    //    DataTable dtPsnDetail = DbHelperMySQLInsur.Query("select * from chs_psn where psn_no='" + YLCARD_NO + "'").Tables[0];
                    //    if (dtPsnDetail.Rows.Count == 0)
                    //    {
                    //        dataReturn.Code = 1;
                    //        dataReturn.Msg = "未取到医保信息，建档失败";
                    //        goto EndPoint;
                    //    }

                    //    PAT_CARD_OUT = dtPsnDetail.Rows[0]["chsOutput1101"].ToString();
                    //}


                    string _hospCode = "12321283469108887C";
                    string _operCode = "zzj01";
                    string _operName = "自助机01";

                    HISModels.A201.A201Request a201Request = new HISModels.A201.A201Request()
                    {
                        birthdate = BIRTHDAY,
                        cardNo = "",//YLCARD_NO,
                        cardType = "", //(YLCARD_TYPE == "2" || YLCARD_TYPE == "4") ? "0" : "1",
                        countryCode = "1",
                        genderCode = SEX == "男" ? "M" : "F",
                        homeCity = "",
                        homeDistrict = "",
                        homeProvince = "",
                        hospCode = _hospCode,
                        idcardNo = SFZ_NO,
                        linkAddress = ADDRESS,
                        linkTel = MOBILE_NO,
                        nationCode = "01",
                        operCode = _operCode,
                        operName = _operName,
                        operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        patiName = PAT_NAME,
                        pactCode = YLCARD_TYPE != "2" ? "01" : "02",
                        pactName = YLCARD_TYPE != "2" ? "自费" : "国家医保"
                    };
                     inputjson = JsonConvert.SerializeObject(a201Request);
                                                              
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

                
                string his_rtnjson = "";

                if (!PubFunc.CallHISService(HOS_ID, inputjson, "A201", ref his_rtnjson))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnjson;
                    goto EndPoint;
                }

                _out.HIS_RTNXML = his_rtnjson;
                try
                {
                    HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);

                    if (baseRsponse.code == "200")
                    {
                        HISModels.PatinfoData patinfoData = baseRsponse.GetInput<HISModels.PatinfoData>();

                        _out.HOSPATID = patinfoData.patiId;

                    }
                    else
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = baseRsponse.message;
                        goto EndPoint;
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
