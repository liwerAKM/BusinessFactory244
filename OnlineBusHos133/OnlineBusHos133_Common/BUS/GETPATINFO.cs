﻿using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using DB.Core;
using System.Net;

namespace OnlineBusHos133_Common.BUS
{
    class GETPATINFO
    {
        public static string B_GETPATINFO(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETPATINFO_M.GETPATINFO_IN _in = JSONSerializer.Deserialize<Model.GETPATINFO_M.GETPATINFO_IN>(json_in);//入参容器
                Model.GETPATINFO_M.GETPATINFO_OUT _out = new Model.GETPATINFO_M.GETPATINFO_OUT();//出参容器
                Dictionary<string, string> dic_filter = PubFunc.Get_Filter(FormatHelper.GetStr(_in.FILTER));

                string HOS_ID = FormatHelper.GetStr(_in.HOS_ID);
                string YLCARD_TYPE = FormatHelper.GetStr(_in.YLCARD_TYPE);//0 0无卡1院内卡2医保卡3 市民卡4身份证
                string YLCARD_NO = FormatHelper.GetStr(_in.YLCARD_NO);
                string SFZ_NO =string.IsNullOrEmpty(_in.SFZ_NO)?"": FormatHelper.GetStr(_in.SFZ_NO);
                string LTERMINAL_SN = FormatHelper.GetStr(_in.LTERMINAL_SN);
                string USER_ID = FormatHelper.GetStr(_in.USER_ID);
                string PAT_CARD_OUT = dic_filter.ContainsKey("PAT_CARD_OUT") ? dic_filter["PAT_CARD_OUT"] : "";

                var db = new DbMySQLZZJ().Client;



                if (YLCARD_TYPE == "4" && YLCARD_NO.Length == 15)
                {
                    YLCARD_NO = QHZZJCommonFunction.Convert15to18(YLCARD_NO);
                }

                if (!string.IsNullOrEmpty(SFZ_NO) && SFZ_NO.Length == 15)
                {
                    SFZ_NO = QHZZJCommonFunction.Convert15to18(SFZ_NO);
                }

                //bool IsEHealthCard = false;
                EHealthCard.qrCodeAnalysisResponse response = new EHealthCard.qrCodeAnalysisResponse();
                //if (_in.YLCARD_TYPE == "6")//电子健康卡
                //{
                //    response = EHealthCard.EHealthCardBus.GetHealthCardInfo(_in.YLCARD_NO);
                //    if (response.code == 0)
                //    {
                //        //按身份证使用
                //        _in.SFZ_NO = (response.data.idNumber).ToUpper();//身份证的x转成大写
                //        _in.YLCARD_TYPE = "4";
                //        _in.YLCARD_NO = _in.SFZ_NO;
                //        //IsEHealthCard = true;
                //    }
                //    else
                //    {
                //        dataReturn.Code = 999;
                //        dataReturn.Msg = "电子健康卡信息查询失败：" + response.msg;
                //        goto EndPoint;
                //    }
                //}
                SqlSugarModel.PatInfo patInfo = new SqlSugarModel.PatInfo();
                SqlSugarModel.PatCard patCard = new SqlSugarModel.PatCard();
                SqlSugarModel.PatCardBind patCardBind = new SqlSugarModel.PatCardBind();

                //如果身份证不为空，先用身份证查询
                if (!string.IsNullOrEmpty(SFZ_NO))
                {
                    patInfo = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.SFZ_NO == SFZ_NO).First();

                    if (patInfo != null)
                    {
                        patCard = db.Queryable<SqlSugarModel.PatCard>().Where(t => t.PAT_ID == patInfo.PAT_ID && t.YLCARD_TYPE == FormatHelper.GetInt(_in.YLCARD_TYPE) && t.YLCARD_NO == _in.YLCARD_NO).First();
                        //如果不同的卡对应不同的HOSPTAID，需要加上卡号去查
                        patCardBind = db.Queryable<SqlSugarModel.PatCardBind>().Where(t => t.HOS_ID == _in.HOS_ID && t.PAT_ID == patInfo.PAT_ID).First();

                    }
                }
                else   //通过卡获取
                {
                    if (YLCARD_TYPE.Equals("1"))//条码单独处理 add at 20221109
                    {
                        patCardBind = db.Queryable<SqlSugarModel.PatCardBind>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOSPATID == YLCARD_NO).First();//条码就是院内号

                        if (patCardBind!=null)
                        {
                            patInfo = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.PAT_ID == patCardBind.PAT_ID).First();
                            patCard = db.Queryable<SqlSugarModel.PatCard>().Where(t => t.PAT_ID == patCardBind.PAT_ID).First();
                        }
                        else
                        {
                            patInfo = null;
                            patCard = null;
                        }

                    }
                    else
                    {
                        patCard = db.Queryable<SqlSugarModel.PatCard>().Where(t => t.YLCARD_TYPE == FormatHelper.GetInt(_in.YLCARD_TYPE) && t.YLCARD_NO == _in.YLCARD_NO).First();

                        if (patCard != null)
                        {
                            patInfo = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.PAT_ID == patCard.PAT_ID).First();
                            //如果不同的卡对应不同的HOSPTAID，需要加上卡号去查
                            patCardBind = db.Queryable<SqlSugarModel.PatCardBind>().Where(t => t.HOS_ID == _in.HOS_ID && t.PAT_ID == patInfo.PAT_ID).First();
                        }
                        else
                        {
                            patInfo = null;
                            patCardBind = null;
                        }
                    }
                }
                //没有绑过卡，调用医院接口，高淳中医院不调用直接返回
                //if (patInfo == null || patCardBind == null)
                if(true)//必掉用his
                {

                    //_out.IS_EXIST = "0";
                    //dataReturn.Code = 0;
                    //dataReturn.Msg = "无用户信息，请使用身份证或者医保卡操作";
                    //dataReturn.Param = JSONSerializer.Serialize(_out);
                    //goto EndPoint;
                    #region his如果有getpatinfo接口则调用并存储数据 没有直接通知无信息
                    
                    XmlDocument doc = QHXmlMode.GetBaseXml("GETPATINFO", "1");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", "qhzzj001");
                    string YLCARD_TYPE_temp = "";
                    string YLCARD_NO_temp = "";
                    if(!string.IsNullOrEmpty(_in.SFZ_NO))
                    {
                        YLCARD_TYPE_temp = "4";
                        YLCARD_NO_temp = _in.SFZ_NO;
                    }
                    else
                    {
                        YLCARD_TYPE_temp = _in.YLCARD_TYPE.Trim().Equals("1") ? "6" : _in.YLCARD_TYPE.Trim();
                        YLCARD_NO_temp = string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim();
                    }


                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", YLCARD_TYPE_temp);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", YLCARD_NO_temp);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());

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
                            _out.IS_EXIST = "0";
                            //if (IsEHealthCard)
                            //{
                            //    _out.PAT_NAME = response.data.realname;
                            //    _out.SFZ_NO = response.data.idNumber;
                            //    _out.SEX = IDCardHelper.getSex(response.data.idNumber);
                            //    _out.AGE = IDCardHelper.getAge(response.data.idNumber).ToString();
                            //    _out.BIR_DATE = IDCardHelper.getBirthday(response.data.idNumber);
                            //    _out.MOBILE_NO = response.data.cellphone;
                            //    _out.ADDRESS = response.data.addr;
                            //}
                            if (YLCARD_TYPE == "1")
                            {
                                dataReturn.Code = 1;
                                dataReturn.Msg = "获取人员信息失败";
                                goto EndPoint;
                            }
                            goto EndPoint1;


                        }

                        if (string.IsNullOrEmpty(FormatHelper.GetStr(dtrev.Rows[0]["SFZ_NO"])))
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "身份证信息缺失，请前往窗口补录身份证";
                            goto EndPoint;
                        }



                        _out.IS_EXIST = "1";
                        _out.PAT_NAME = dtrev.Columns.Contains("PAT_NAME") ? FormatHelper.GetStr(dtrev.Rows[0]["PAT_NAME"]) : "";
                        _out.SEX = dtrev.Columns.Contains("SEX") ? FormatHelper.GetStr(dtrev.Rows[0]["SEX"]) : "";
                        _out.AGE = dtrev.Columns.Contains("AGE") ? FormatHelper.GetStr(dtrev.Rows[0]["AGE"]) : "";
                        _out.MOBILE_NO = dtrev.Columns.Contains("TEL_NO") ? FormatHelper.GetStr(dtrev.Rows[0]["TEL_NO"]) : "";
                        _out.ADDRESS = dtrev.Columns.Contains("ADDRESS") ? FormatHelper.GetStr(dtrev.Rows[0]["ADDRESS"]) : "";
                        _out.SFZ_NO = dtrev.Columns.Contains("SFZ_NO") ? FormatHelper.GetStr(dtrev.Rows[0]["SFZ_NO"]) : "";
                        _out.HOSPATID = dtrev.Columns.Contains("HOSPATID") ? FormatHelper.GetStr(dtrev.Rows[0]["HOSPATID"]) : "";
                        _out.HOSPATIDS = dtrev.Columns.Contains("HOSPATIDS") ? FormatHelper.GetStr(dtrev.Rows[0]["HOSPATIDS"]) : "";
                        _out.YY_LSH = dtrev.Columns.Contains("YY_LSH") ? FormatHelper.GetStr(dtrev.Rows[0]["YY_LSH"]) : "";
                        _out.BIR_DATE = dtrev.Columns.Contains("BIRTHDAY") ? FormatHelper.GetStr(dtrev.Rows[0]["BIRTHDAY"]) : QHZZJCommonFunction.GetBirthdayByIDCard(_out.SFZ_NO);
                        _out.GUARDIAN_NAME = dtrev.Columns.Contains("GUARDIAN_NAME") ? FormatHelper.GetStr(dtrev.Rows[0]["GUARDIAN_NAME"]) : "";
                        _out.GUARDIAN_SFZ_NO = dtrev.Columns.Contains("GUARDIAN_SFZ_NO") ? FormatHelper.GetStr(dtrev.Rows[0]["GUARDIAN_SFZ_NO"]) : "";
                        //if (string.IsNullOrEmpty(_out.HOSPATID))
                        //{

                        //}

                        if (string.IsNullOrEmpty(FormatHelper.GetStr(dtrev.Rows[0]["MOBILE_NO"])))//无手机号
                        {
                            _out.IS_EXIST = "0";
                            dataReturn.Code = 0;
                            dataReturn.Msg = "院内用户信息手机号缺失，请调用建档接口完善";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                            goto EndPoint;
                        }

                        if (patInfo == null) 
                        {
                          
                            int pat_id = 0;
                            if (!PubFunc.GetSysID("pat_info", out pat_id))
                            {
                                dataReturn.Code = 5;
                                dataReturn.Msg = "[提示]建档失败，请联系医院处理";
                                dataReturn.Param  ="获取pat_info的sysid失败";
                                goto EndPoint;
                            }

                            patInfo = new SqlSugarModel.PatInfo();
                            patInfo.PAT_ID = pat_id;
                            patInfo.SFZ_NO = _out.SFZ_NO;
                            patInfo.PAT_NAME = _out.PAT_NAME;
                            patInfo.SEX = _out.SEX;
                            patInfo.BIRTHDAY = _out.BIR_DATE;
                            patInfo.ADDRESS = _out.ADDRESS;
                            patInfo.MOBILE_NO = _out.MOBILE_NO;
                            patInfo.GUARDIAN_NAME = _out.GUARDIAN_NAME;
                            patInfo.GUARDIAN_SFZ_NO = _out.GUARDIAN_SFZ_NO;
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
                            patCard.YLCARD_TYPE =FormatHelper.GetInt(_in.YLCARD_TYPE);
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
                            patCardBind.YY_LSH = _out.YY_LSH;
                            patCardBind.MARK_BIND = 1;
                            patCardBind.BAND_TIME = DateTime.Now;
                            db.Insertable(patCardBind).ExecuteCommand();
                        }

                        EndPoint1:
                        dataReturn.Code = 0;
                        dataReturn.Msg = "SUCCESS";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                    }
                    catch (Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                        dataReturn.Param = ex.Message;
                    }
                     
                    #endregion
                }
                else
                {
                    _out.IS_EXIST = "1";
                    _out.PAT_NAME = patInfo.PAT_NAME;
                    _out.SEX = patInfo.SEX;
                    _out.AGE =IDCardHelper.getAge(patInfo.SFZ_NO).ToString();
                    _out.MOBILE_NO = patInfo.MOBILE_NO;
                    _out.ADDRESS = patInfo.ADDRESS;
                    _out.SFZ_NO = patInfo.SFZ_NO;
                    _out.HOSPATID = patCardBind.HOSPATID;
                    _out.BIR_DATE = patInfo.BIRTHDAY;
                    _out.GUARDIAN_NAME = patInfo.GUARDIAN_NAME;
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.Message;
            }

            EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            
            return json_out;
        }


    }
}