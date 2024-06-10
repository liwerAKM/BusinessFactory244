using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using System.Reflection.Emit;
using OnlineBusHos319_YYGH.Model;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace OnlineBusHos319_YYGH.BUS
{
    class REGISTERAPPTSAVE
    {
        public static string B_REGISTERAPPTSAVE(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN _in = JSONSerializer.Deserialize<Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_IN>(json_in);
                Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT _out = new Model.REGISTERAPPTSAVE_M.REGISTERAPPTSAVE_OUT();

                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string SCH_DATE = string.IsNullOrEmpty(_in.SCH_DATE) ? DateTime.Today.ToString("yyyy-MM-dd") : _in.SCH_DATE.Trim();
                string MDTRT_CERT_TYPE = string.IsNullOrEmpty(_in.MDTRT_CERT_TYPE) ? "" : _in.MDTRT_CERT_TYPE.Trim();
                string MDTRT_CERT_NO = string.IsNullOrEmpty(_in.MDTRT_CERT_NO) ? "" : _in.MDTRT_CERT_NO.Trim();
                string CARD_SN = string.IsNullOrEmpty(_in.CARD_SN) ? "" : _in.CARD_SN.Trim();
                string BEGNTIME = string.IsNullOrEmpty(_in.BEGNTIME) ? "" : _in.BEGNTIME.Trim();
                string CERTNO = string.IsNullOrEmpty(_in.CERTNO) ? "" : _in.CERTNO.Trim();
                string PSN_CERT_TYPE = string.IsNullOrEmpty(_in.PSN_CERT_TYPE) ? "" : _in.PSN_CERT_TYPE.Trim();
                string PSN_NAME = string.IsNullOrEmpty(_in.PSN_NAME) ? "" : _in.PSN_NAME.Trim();
                string HOSPATID = string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim();
                string YLCARD_TYPE = string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim();
                string REGISTER_TYPE = string.IsNullOrEmpty(_in.REGISTER_TYPE) ? "" : _in.REGISTER_TYPE.Trim();
                string PERIOD_START = string.IsNullOrEmpty(_in.PERIOD_START) ? "" : _in.PERIOD_START.Trim();
                string PERIOD_END = string.IsNullOrEmpty(_in.PERIOD_END) ? "" : _in.PERIOD_END.Trim();
                string HOS_SN = string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim();
                string IS_YY = string.IsNullOrEmpty(_in.IS_YY) ? "" : _in.IS_YY;

                if (IS_YY=="2"&&string.IsNullOrEmpty(HOS_SN))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "取号功能预约单号不能为空";
                    goto EndPoint;
                }
              
                string inputjson = "";
                string type = "";
                HISModels.BasesiInfo basesiInfo = new HISModels.BasesiInfo()//医保基础信息 
                {
                    mdtrt_cert_type = MDTRT_CERT_TYPE,
                    mdtrt_cert_no = MDTRT_CERT_NO,
                    card_sn = CARD_SN,
                    begntime = BEGNTIME,
                    certno = CERTNO,
                    psn_cert_type = PSN_CERT_TYPE,
                    psn_name = PSN_NAME
                };
                string bookingNo = "";

                if (IS_YY == "2") //预约取号
                {
                    bookingNo = HOS_SN;
                    HISModels.A207.A207Request a207Request = new HISModels.A207.A207Request()//取号入参
                    {
                        patiId = HOSPATID,
                        hospCode = _hospCode,
                        operCode = _operCode,
                        operName = _operName,
                        operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        bookingNo = bookingNo,
                        selfPay = YLCARD_TYPE == "2" ? "0" : "1",
                        pactCode = YLCARD_TYPE == "2" ? "03" : "01",
                        siInfo = basesiInfo
                    };
                    type = "A207";
                    inputjson = JSONSerializer.Serialize(a207Request);

                }
                else
                {

                    if (SCH_DATE == DateTime.Now.ToString("yyyy-MM-dd"))//当天
                    {
                        HISModels.A202.A202Request a202Request = new HISModels.A202.A202Request()
                        {
                            hospCode = _hospCode,
                            operCode = _operCode,
                            operName = _operName,
                            operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            patiId = HOSPATID,
                            scheduleNo = REGISTER_TYPE,
                            selfPay = YLCARD_TYPE == "2" ? "0" : "1",
                            pactCode = YLCARD_TYPE == "2" ? "03" : "01",
                            siInfo = basesiInfo
                        };

                        type = "A202";
                        inputjson = JSONSerializer.Serialize(a202Request);
                    }
                    else//预约
                    {
                        HISModels.A205.A205Request a205Request = new HISModels.A205.A205Request()//先锁号
                        {
                            beginTime = PERIOD_START,
                            endTime = PERIOD_END,
                            hospCode = _hospCode,
                            operCode = _operCode,
                            operName = _operName,
                            operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            patiId = HOSPATID,
                            scheduleNo = REGISTER_TYPE,
                            sequenceNo = ""
                        };
                        string A205outjson = "";
                        string inJson = JSONSerializer.Serialize(a205Request);
                        if (!PubFunc.CallHISService(HOS_ID, inJson, "A205", ref A205outjson))
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = A205outjson;
                            goto EndPoint;
                        }

                        HISModels.baseRsponse baseRsponse205 = JSONSerializer.Deserialize<HISModels.baseRsponse>(A205outjson);
                        JObject pairs = new JObject();
                        if (baseRsponse205.code == "200")
                        {
                            pairs = baseRsponse205.GetInput<JObject>();
                        }
                        else
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = baseRsponse205.message;
                            goto EndPoint;
                        }


                        bookingNo = pairs["bookingNo"].ToString();
                        HISModels.A207.A207Request a207Request = new HISModels.A207.A207Request()//取号入参
                        {
                            patiId = HOSPATID,
                            hospCode = _hospCode,
                            operCode = _operCode,
                            operName = _operName,
                            operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            bookingNo = bookingNo,
                            selfPay = YLCARD_TYPE == "2" ? "0" : "1",
                            pactCode = YLCARD_TYPE == "2" ? "03" : "01",
                            siInfo = basesiInfo
                        };
                        type = "A207";
                        inputjson = JSONSerializer.Serialize(a207Request);

                    }

                }
                string his_rtnjson = "";
                if (!PubFunc.CallHISService(HOS_ID, inputjson, type, ref his_rtnjson))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnjson;
                    goto EndPoint;
                }

                try
                {
                    HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);
                    if (baseRsponse.code != "200")
                    {

                        dataReturn.Code = 1;
                        dataReturn.Msg = baseRsponse.message;
                        goto EndPoint;

                    }

                    JObject Keys = baseRsponse.GetInput<JObject>();
                    _out.HOS_SN = Keys["clinicNo"].ToString();
                    _out.HOS_SNAPPT = bookingNo;
                    _out.JEALL = Keys["totCost"].ToString();
                    _out.APPT_PAY = Keys["totCost"].ToString();
                    _out.APPT_ORDER = SCH_DATE == DateTime.Now.ToString("yyyy-MM-dd") ? Keys["invoiceNo"].ToString() : bookingNo; //Keys["invoiceNo"].ToString();
                    _out.APPT_TIME = SCH_DATE + " " + PERIOD_START;
                    _out.APPT_PLACE = "";
                    _out.INVOICE_NO = Keys["invoiceNo"].ToString();
                    _out.PSN_CASH_PAY = Keys["ownCost"].ToString();
                    _out.ECO_COST = Keys["ecoCost"].ToString();
                    _out.MEDFEE_SUMAMT = Keys["totCost"].ToString();
                    if (YLCARD_TYPE == "2")
                    {
                        if (Keys["totCost"].ToString() == "0")//0元号
                        {
                            _out.ACCT_PAY = "0";
                            _out.BALC = "0";
                            _out.FUND_PAY_SUMAMT = "0";
                        }
                        else
                        {
                            _out.ACCT_PAY = Keys["siOutput"]["setlinfo"]["acct_pay"].ToString();
                            _out.BALC = Keys["siOutput"]["setlinfo"]["balc"].ToString();
                            _out.FUND_PAY_SUMAMT = Keys["siOutput"]["setlinfo"]["fund_pay_sumamt"].ToString();
                        }
                    }

                    
              

                    #region 平台数据保存
                    try
                    {
                        var db = new DbMySQLZZJ().Client;

                        SqlSugarModel.RegisterAppt modelregister_appt = new SqlSugarModel.RegisterAppt();

                        int reg_id = 0;//预约标识
                        if (!PubFunc.GetSysID("REGISTER_APPT", out reg_id))
                        {
                            goto EndPoint;
                        }
                        int pat_id = 0;
                        var pat_info=db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.SFZ_NO == _in.SFZ_NO).First();
                        if (pat_info != null) 
                        {
                            pat_id = pat_info.PAT_ID;
                        }

                        modelregister_appt.REG_ID = reg_id;
                        modelregister_appt.HOS_ID = _in.HOS_ID;
                        modelregister_appt.PAT_ID = pat_id;

                        modelregister_appt.SFZ_NO = _in.SFZ_NO;
                        modelregister_appt.PAT_NAME = _in.PAT_NAME;
                        modelregister_appt.BIRTHDAY = _in.BIRTHDAY;
                        modelregister_appt.SEX = _in.SEX;
                        modelregister_appt.ADDRESS = _in.ADDRESS;
                        modelregister_appt.MOBILE_NO = _in.MOBILE_NO;

                        modelregister_appt.YLCARD_NO = _in.YLCARD_NO;
                        modelregister_appt.YLCARD_TYPE = FormatHelper.GetInt(_in.YLCARD_TYPE);

                        modelregister_appt.DEPT_CODE = _in.DEPT_CODE;
                        modelregister_appt.DOC_NO = _in.DOC_NO;
                        modelregister_appt.DEPT_NAME = "";
                        modelregister_appt.DOC_NAME = "";

                        modelregister_appt.SCH_DATE = _in.SCH_DATE;
                        modelregister_appt.SCH_TIME = _in.SCH_TIME;
                        modelregister_appt.SCH_TYPE = _in.SCH_TYPE;
                        modelregister_appt.PERIOD_START = _in.PERIOD_START;
                        modelregister_appt.PERIOD_END = _in.PERIOD_END;
                        modelregister_appt.TIME_POINT = "";
                        modelregister_appt.REGISTER_TYPE = _in.REGISTER_TYPE;
                        modelregister_appt.IS_FZ = false;//初复诊


                        modelregister_appt.ZL_FEE = FormatHelper.GetDecimal(_out.APPT_PAY);
                        modelregister_appt.GH_FEE = 0;
                        modelregister_appt.APPT_PAY = FormatHelper.GetDecimal(_out.APPT_PAY);

                        modelregister_appt.APPT_DATE = DateTime.Now.Date;
                        modelregister_appt.APPT_TIME = DateTime.Now.ToString("HH:mm:ss");

                        modelregister_appt.HOS_SN = _out.HOS_SN;
                        modelregister_appt.APPT_SN = _out.HOS_SN;//待定
                        modelregister_appt.APPT_ORDER = _out.APPT_ORDER;//待定
                        modelregister_appt.ZS_NAME = _out.APPT_PLACE;//预约标记
                        modelregister_appt.APPT_TYPE = "0";//预约标记
                        modelregister_appt.HOSPATID = _in.HOSPATID;//预约标记

                        modelregister_appt.USER_ID = _in.USER_ID;
                        modelregister_appt.lTERMINAL_SN = _in.LTERMINAL_SN;
                        modelregister_appt.SOURCE = "ZZJ";
                      
                        var row = db.Insertable(modelregister_appt).ExecuteCommand();
                    }
                    catch (Exception ex) 
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "REGISTERAPPTSAVE";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }
                    #endregion

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
