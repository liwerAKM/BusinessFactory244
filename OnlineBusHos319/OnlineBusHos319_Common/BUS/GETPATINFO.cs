using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using DB.Core;
using OnlineBusHos319_Common.Model;
using System.Reflection.Emit;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using static OnlineBusHos319_Common.Model.HISModels.A102;

namespace OnlineBusHos319_Common.BUS
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
                Model.GETPATINFO_M.GETPATINFO_IN _in = JSONSerializer.Deserialize<Model.GETPATINFO_M.GETPATINFO_IN>(json_in);
                Model.GETPATINFO_M.GETPATINFO_OUT _out = new Model.GETPATINFO_M.GETPATINFO_OUT();
                Dictionary<string, string> dic_filter = PubFunc.Get_Filter(FormatHelper.GetStr(_in.FILTER));

                string HOS_ID = FormatHelper.GetStr(_in.HOS_ID);
                string YLCARD_TYPE = FormatHelper.GetStr(_in.YLCARD_TYPE);//0 0无卡1院内卡2医保卡3 市民卡4身份证
                string YLCARD_NO = FormatHelper.GetStr(_in.YLCARD_NO);
                string SFZ_NO = FormatHelper.GetStr(_in.SFZ_NO);
                string LTERMINAL_SN = FormatHelper.GetStr(_in.LTERMINAL_SN);
                string USER_ID = FormatHelper.GetStr(_in.USER_ID);
                string PAT_CARD_OUT = dic_filter.ContainsKey("PAT_CARD_OUT") ? dic_filter["PAT_CARD_OUT"] : "";

                var db = new DbMySQLZZJ().Client;

                //YLCARD_TYPE = PubFunc.GETHISYLCARDTYPE(_in.YLCARD_TYPE);

                if (YLCARD_TYPE == "4" && YLCARD_NO.Length == 15)
                {
                    YLCARD_NO = QHZZJCommonFunction.Convert15to18(YLCARD_NO);
                }

                string PAT_NAME = "";
                string ADDRESS = "";
                string TELE = "";
                SFZ_NO = QHZZJCommonFunction.Convert15to18(SFZ_NO);

                bool IsEHealthCard = false;
                EHealthCard.qrCodeAnalysisResponse response = new EHealthCard.qrCodeAnalysisResponse();
                if (_in.YLCARD_TYPE == "8")//电子健康卡
                {
                    response = EHealthCard.EHealthCardBus.GetHealthCardInfo(_in.YLCARD_NO);
                    if (response.code == 0)
                    {
                        //按身份证使用
                        _in.SFZ_NO = (response.data.idNumber).ToUpper();//身份证的x转成大写
                        _in.YLCARD_TYPE = "4";
                        _in.YLCARD_NO = _in.SFZ_NO;
                        IsEHealthCard = true;
                    }
                    else
                    {
                        dataReturn.Code = 999;
                        dataReturn.Msg = "电子健康卡信息查询失败：" + response.msg;
                        goto EndPoint;
                    }
                }
                SqlSugarModel.PatInfo patInfo = null;
                SqlSugarModel.PatCard patCard = null;
                SqlSugarModel.PatCardBind patCardBind = null;

                ////如果身份证不为空，先用身份证查询
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
                //没有绑过卡，调用医院接口
                //if (patInfo == null || patCardBind == null)
                if (true)
                {
                 

                    string _hospCode = "12321283469108887C";
                    string _operCode = "zzj01";
                    string _operName = "自助机01";



                    string inputjson = "";
                    string type = "";
                    if (YLCARD_TYPE == "4" || YLCARD_TYPE == "2")//实体卡
                    {
                        if (YLCARD_TYPE == "2")//用身份证
                        {
                            YLCARD_NO = SFZ_NO;
                        }
                        type = "A102";
                        HISModels.A102.A102Request a102Request = new HISModels.A102.A102Request()
                        {
                            idCardNo = YLCARD_NO,
                            hospCode = _hospCode,
                            operCode = _operCode,
                            operName = _operName,
                            operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        inputjson = JsonConvert.SerializeObject(a102Request);
                    }
                    else//院内号
                    {
                        type = "A101";
                        HISModels.A101.A101Request a101Request = new HISModels.A101.A101Request()
                        {
                            patiId = YLCARD_NO,
                            hospCode = _hospCode,
                            operCode = _operCode,
                            operName = _operName,
                            operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        inputjson = JsonConvert.SerializeObject(a101Request);
                    }



                    
                    string his_rtnjson = "";
                    if (!PubFunc. CallHISService( HOS_ID,inputjson, type, ref  his_rtnjson))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnjson;
                        goto EndPoint;
                    }
                    _out.IS_EXIST = "1";
                    _out.HIS_RTNXML = "";
                    try
                    {
                        HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);

                        
                        if (baseRsponse.code == "200")
                        {
                            HISModels.PatinfoData patinfoData = baseRsponse.GetInput<HISModels.PatinfoData>();
                          
                            int age = 0;
                            try
                            {
                                age = CommonFunction.CalculateAgeCorrect(DateTime.Parse(patinfoData.birthDate), DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                age = 0;
                            }

                            _out.IS_EXIST = "1";
                            _out.PAT_NAME = patinfoData.patiName;
                            _out.SEX = patinfoData.genderCode == "M" ? "男" : "女";
                            _out.AGE = age.ToString();
                            _out.MOBILE_NO = patinfoData.linkTel;
                            _out.ADDRESS = patinfoData.linkAddress;
                            _out.SFZ_NO = patinfoData.idCardNo;
                            _out.HOSPATID = patinfoData.patiId;
                            _out.BIR_DATE = patinfoData.birthDate.Split("T")[0];
                            _out.GUARDIAN_NAME = "";
                            _out.GUARDIAN_SFZ_NO = "";



                        }
                        else
                        {
                            dataReturn.Code = 0;
                            dataReturn.Msg = baseRsponse.message;
                            _out.IS_EXIST = "0";
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
                        dataReturn.Param = ex.Message;
                    }
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
