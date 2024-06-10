using CommonModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBusHos8_GJYB.Model;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace OnlineBusHos8_GJYB.BUS
{
    /// <summary>
    /// B方案，HIS提供接口返回医保交易参数，启航调用医保
    /// </summary>
    public class BBusiness : IBusiness
    {
        public virtual DataReturn PSNQUERY(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN _in = JSONSerializer.Deserialize<GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN>(injson);
            GJYB_PSNQUERY_M.GJYB_PSNQUERY_OUT _out = new GJYB_PSNQUERY_M.GJYB_PSNQUERY_OUT();
            string HOS_ID = _in.HOS_ID;
            string opter_no = _in.USER_ID;

            Models.T1101.Root t1101 = new Models.T1101.Root();
            t1101.data = new Models.T1101.Data();
            t1101.data.mdtrt_cert_type = _in.MDTRT_CERT_TYPE;
            t1101.data.mdtrt_cert_no = _in.MDTRT_CERT_NO; 
            t1101.data.card_sn = _in.CARD_SN; 
            t1101.data.begntime = _in.BEGNTIME;
            t1101.data.psn_cert_type = _in.PSN_CERT_TYPE; 
            t1101.data.certno = _in.CERTNO; 
            t1101.data.psn_name = _in.PSN_NAME; 

            string insuplc_admdvs ="" ; //_in.INSUPLC_ADMDVS

            string msg = "";
            string infno = "";
            bool flag = false;
            infno = "1101";
            Models.InputRoot inputRoot1101 = new Models.InputRoot();
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t1101, ref inputRoot1101, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot1101 = GlobalVar.YBTrans(HOS_ID, inputRoot1101);
            if (outputRoot1101.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot1101.err_msg;
                return dataReturn;
            }

            Models.RT1101.Root rt1101 = outputRoot1101.GetOutput<Models.RT1101.Root>();
            //保存记录
            SqlSugarModel.ChsPsn chsPsn = new SqlSugarModel.ChsPsn();
            chsPsn.psn_no = rt1101.baseinfo.psn_no;
            if (rt1101.insuinfo != null) 
            {
                chsPsn.insuplc_admdvs = rt1101.insuinfo[0].insuplc_admdvs; 
            }
            chsPsn.chsOutput1101 = JsonConvert.SerializeObject(outputRoot1101);
            chsPsn.mdtrt_cert_type = t1101.data.mdtrt_cert_type;
            chsPsn.mdtrt_cert_no = t1101.data.mdtrt_cert_no;
            chsPsn.card_sn = t1101.data.card_sn;
            chsPsn.begntime = t1101.data.begntime;
            chsPsn.psn_cert_type = t1101.data.psn_cert_type;
            chsPsn.certno = t1101.data.certno;
            chsPsn.psn_name = t1101.data.psn_name;
            chsPsn.save_time = DateTime.Now;
            var db = new DbContext().Client;
            db.Saveable(chsPsn).ExecuteCommand();


            if (rt1101.insuinfo == null) 
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "您的医保卡无参保信息";
                return dataReturn;

            }
            //取psn_insu_date为空或者大于当前日期
            var insurinfo = rt1101.insuinfo.Find(t => string.IsNullOrEmpty(t.paus_insu_date) || t.paus_insu_date.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) > 0);

            if (insurinfo==null)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "您的医保卡已暂停参保";
                return dataReturn;
            }
            _out.PSN_NAME = rt1101.baseinfo.psn_name;
            _out.PSN_NO = rt1101.baseinfo.psn_no;
            _out.SEX = rt1101.baseinfo.gend == "1" ? "男" : "女";
            _out.NATION = rt1101.baseinfo.naty;
            _out.BIRTHDAY = rt1101.baseinfo.brdy;
            _out.AGE = rt1101.baseinfo.age;
            _out.SFZ_NO = rt1101.baseinfo.certno;
            string balc = FormatHelper.GetStr(insurinfo.balc);
            string insutype = FormatHelper.GetStr(insurinfo.insutype);
            _out.BALANCE = balc;
            _out.INSUTYPE = insutype;
            _out.PAT_CARD_OUT= JsonConvert.SerializeObject(rt1101);
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;
        }

        public virtual DataReturn REGTRY(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_REGTRY_M.GJYB_REGTRY_IN _in = JSONSerializer.Deserialize<GJYB_REGTRY_M.GJYB_REGTRY_IN>(injson);
            GJYB_REGTRY_M.GJYB_REGTRY_OUT _out = new GJYB_REGTRY_M.GJYB_REGTRY_OUT();
            string HOS_SN = _in.HOS_SN;
            string psn_no =_in.PSN_NO;
            string HOS_ID = _in.HOS_ID;
            string opter_no =_in.USER_ID;
            var db = new DbContext().Client;
            SqlSugarModel.ChsPsn chspsn = db.Queryable<SqlSugarModel.ChsPsn>().Where(t => t.psn_no == psn_no).Single();
            if (chspsn==null)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "未取到个人信息";
                return dataReturn;
            }
            string chsOutput1101 = chspsn.chsOutput1101;
            string insuplc_admdvs = chspsn.insuplc_admdvs;//参保地行政区划
            //if (string.IsNullOrEmpty(insuplc_admdvs))
            //{
            //    Model.psn_insuinfo psn_Insuinfo = RedisDataHelper.GetRedisPsnInsuInfo(psn_no);
            //    if (psn_Insuinfo != null)
            //    {
            //        insuplc_admdvs = psn_Insuinfo.insuplc_admdvs;
            //    }
            //}


            //获取医保挂号预结算参数：调用3.3.13获取挂号医保预结算数据（TYPE：GETCHSREGTRY）[医保B方案]
            string chsInput2201 = "";
            string chsInput2203 = "";
            string chsInput2204 = "";
            string chsInput2206 = "";
           
            string msg = "";

            //挂号登记2201
            string infno = "2201";
            Models.InputRoot inputRoot2201 = new Models.InputRoot();
            JObject jin2201 = JObject.Parse(chsInput2201);
            bool flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "",opter_no, insuplc_admdvs, jin2201, ref inputRoot2201, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2201 = GlobalVar.YBTrans(HOS_ID, inputRoot2201);
            if (outputRoot2201.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2201.err_msg;
                return dataReturn;
            }
            //医保出参处理
            var jout2201 = JObject.FromObject(outputRoot2201.output);
            string mdtrt_id = jout2201["data"]["mdtrt_id"].ToString();

            //门诊就诊信息上传A【2203A】
            infno = "2203A";
            Models.InputRoot inputRoot2203 = new Models.InputRoot();
            JObject jin2203 = JObject.Parse(chsInput2203);
            jin2203["mdtrtinfo"]["mdtrt_id"] = mdtrt_id;
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2203, ref inputRoot2203, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2203 = GlobalVar.YBTrans(HOS_ID, inputRoot2203);
            if (outputRoot2203.infcode!="0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2203.err_msg;
                return dataReturn;
            }
            //门诊费用明细信息撤销【2205】

            //门诊费用明细信息上传【2204】
            infno = "2204";
            Models.InputRoot inputRoot2204 = new Models.InputRoot();
            JObject jin2204 = JObject.Parse(chsInput2204);
            var listmx = jin2204["feedetail"];
            for (int i = 0; i < listmx.Count(); i++)
            {
                jin2204["feedetail"][i]["mdtrt_id"] = mdtrt_id;
            }

            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2204, ref inputRoot2204, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2204 = GlobalVar.YBTrans(HOS_ID, inputRoot2204);
            if (outputRoot2204.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2204.err_msg;
                return dataReturn;
            }

            //门诊预结算【2206】
            infno = "2206";
            Models.InputRoot inputRoot2206 = new Models.InputRoot();
            JObject jin2206 = JObject.Parse(chsInput2206);
            jin2206["data"]["mdtrt_id"] = mdtrt_id;
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2206, ref inputRoot2206, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2206 = GlobalVar.YBTrans(HOS_ID, inputRoot2206);
            if (outputRoot2206.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2206.err_msg;
                return dataReturn;
            }

            //保存记录
            SqlSugarModel.ChsTran chsTran = new SqlSugarModel.ChsTran();
            chsTran.HOS_ID = HOS_ID;
            chsTran.BIZ_TYPE = "01";
            chsTran.BIZ_NO = HOS_SN;
            chsTran.TRAN_ID = chsTran.HOS_ID + "_" + chsTran.BIZ_TYPE + "_" + chsTran.BIZ_NO;
            chsTran.psn_no = psn_no;
            chsTran.insuplc_admdvs = insuplc_admdvs;
            chsTran.mdtrt_id = mdtrt_id;
            chsTran.chsOutput1101 = chsOutput1101;

            chsTran.chsInput2201 = JsonConvert.SerializeObject(inputRoot2201);
            chsTran.chsOutput2201 = JsonConvert.SerializeObject(outputRoot2201);
            chsTran.chsInput2203 = JsonConvert.SerializeObject(inputRoot2203);
            chsTran.chsInput2204 = JsonConvert.SerializeObject(inputRoot2204);
            chsTran.chsOutput2204 = JsonConvert.SerializeObject(outputRoot2204);
            chsTran.chsInput2206 = JsonConvert.SerializeObject(inputRoot2206);
            chsTran.chsOutput2206 = JsonConvert.SerializeObject(outputRoot2206);

            chsTran.create_time = DateTime.Now;
            db.Saveable(chsTran).ExecuteCommand();


            Models.RT2206.Root rt2206 = outputRoot2206.GetOutput<Models.RT2206.Root>();
            _out.MDTRT_ID = rt2206.setlinfo.mdtrt_id;
            _out.MEDFEE_SUMAMT = FormatHelper.GetStr(rt2206.setlinfo.medfee_sumamt);
            _out.ACCT_PAY = FormatHelper.GetStr(rt2206.setlinfo.acct_pay);
            _out.PSN_CASH_PAY = FormatHelper.GetStr(rt2206.setlinfo.psn_cash_pay);
            _out.FUND_PAY_SUMAMT = FormatHelper.GetStr(rt2206.setlinfo.fund_pay_sumamt);
            _out.OTH_PAY = FormatHelper.GetStr(rt2206.setlinfo.oth_pay);
            _out.BALC = FormatHelper.GetStr(rt2206.setlinfo.balc);
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;
        }
        public virtual DataReturn OUTPTRY(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_OUTPTRY_M.GJYB_OUTPTRY_IN _in = JSONSerializer.Deserialize<GJYB_OUTPTRY_M.GJYB_OUTPTRY_IN>(injson);
            GJYB_OUTPTRY_M.GJYB_OUTPTRY_OUT _out = new GJYB_OUTPTRY_M.GJYB_OUTPTRY_OUT();
            string HOS_SN = _in.HOS_SN;
            string psn_no = _in.PSN_NO; 
            string HOS_ID = _in.HOS_ID; 
            string opter_no = _in.USER_ID; 
            var db = new DbContext().Client;
            SqlSugarModel.ChsPsn chspsn = db.Queryable<SqlSugarModel.ChsPsn>().Where(t => t.psn_no == psn_no).Single();
            if (chspsn == null)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "未取到个人信息";
                return dataReturn;
            }
            string chsOutput1101 = chspsn.chsOutput1101;
            string insuplc_admdvs = chspsn.insuplc_admdvs;//参保地行政区划
            //if (string.IsNullOrEmpty(insuplc_admdvs))
            //{
            //    Model.psn_insuinfo psn_Insuinfo = RedisDataHelper.GetRedisPsnInsuInfo(psn_no);
            //    if (psn_Insuinfo != null)
            //    {
            //        insuplc_admdvs = psn_Insuinfo.insuplc_admdvs;
            //    }
            //}

            //3.4.16获取门诊缴费医保预结算数据（TYPE：GETCHSOUTPTRY）[医保B方案]
            string chsInput2201 = "";
            string chsInput2203 = "";
            string chsInput2204 = "";
            string chsInput2206 = "";

            string msg = "";
            string infno = "";
            bool flag = false;
            bool reg = false;//是否需要重新登记
            string mdtrt_id = "";
            //挂号登记2201
            string chsOutput2201 = "";
            Models.InputRoot inputRoot2201 = new Models.InputRoot();
            if (!string.IsNullOrEmpty(chsInput2201))
            {
                reg = true;
                infno = "2201";
              
                JObject jin2201 = JObject.Parse(chsInput2201);
                flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2201, ref inputRoot2201, ref msg);
                if (!flag)
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = msg;
                    return dataReturn;

                }
                //调用医保
                Models.OutputRoot outputRoot2201 = GlobalVar.YBTrans(HOS_ID, inputRoot2201);
                if (outputRoot2201.infcode != "0")
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = outputRoot2201.err_msg;
                    return dataReturn;
                }
                //医保出参处理
                var jout2201 = JObject.FromObject(outputRoot2201.output);
                mdtrt_id = jout2201["data"]["mdtrt_id"].ToString();
                chsOutput2201= JsonConvert.SerializeObject(outputRoot2201);
            }
            Models.InputRoot inputRoot2203 = new Models.InputRoot();
            if (!string.IsNullOrEmpty(chsInput2203))
            {
                //门诊就诊信息上传A【2203A】
                infno = "2203A";
             
                JObject jin2203 = JObject.Parse(chsInput2203);
                if (reg)
                {
                    jin2203["mdtrtinfo"]["mdtrt_id"] = mdtrt_id;
                }
                flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2203, ref inputRoot2203, ref msg);
                if (!flag)
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = msg;
                    return dataReturn;
                }
                //调用医保
                Models.OutputRoot outputRoot2203 = GlobalVar.YBTrans(HOS_ID, inputRoot2203);
                if (outputRoot2203.infcode != "0")
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = outputRoot2203.err_msg;
                    return dataReturn;
                }
            }
            //门诊费用明细信息撤销【2205】
            if (!reg)
            {
                //不是重新登记的，撤销明细
                infno = "2205";
                Models.InputRoot inputRoot2205 = new Models.InputRoot();
                JObject jin2205 =new JObject();
                JObject jin2205data = new JObject();
                jin2205data["mdtrt_id"] = mdtrt_id;
                jin2205data["chrg_bchno"] = "0000";
                jin2205data["psn_no"] = psn_no;
                jin2205["data"] = jin2205data;

                flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2205, ref inputRoot2205, ref msg);
                if (!flag)
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = msg;
                    return dataReturn;
                }
                //调用医保
                Models.OutputRoot outputRoot2205 = GlobalVar.YBTrans(HOS_ID, inputRoot2205);
                //if (outputRoot2205.infcode != "0")
                //{
                //    outputdata = QHXmlMode.GetRtnXml(TYPE, PATIENTTYPE, "8", outputRoot2205.err_msg).InnerXml;
                //    return;
                //}
            }
            //门诊费用明细信息上传【2204】
            infno = "2204";
            Models.InputRoot inputRoot2204 = new Models.InputRoot();
            JObject jin2204 = JObject.Parse(chsInput2204);
            if (reg)
            {
                var listmx = jin2204["feedetail"];
                for (int i = 0; i < listmx.Count(); i++)
                {
                    jin2204["feedetail"][i]["mdtrt_id"] = mdtrt_id;
                }
            }
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2204, ref inputRoot2204, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2204 = GlobalVar.YBTrans(HOS_ID, inputRoot2204);
            if (outputRoot2204.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2204.err_msg;
                return dataReturn;
            }

            //门诊预结算【2206】
            infno = "2206";
            Models.InputRoot inputRoot2206 = new Models.InputRoot();
            JObject jin2206 = JObject.Parse(chsInput2206);
            if (reg)
            {
                jin2206["data"]["mdtrt_id"] = mdtrt_id;
            }
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2206, ref inputRoot2206, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2206 = GlobalVar.YBTrans(HOS_ID, inputRoot2206);
            if (outputRoot2206.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2206.err_msg;
                return dataReturn;
            }

            //保存记录
            #region 
            SqlSugarModel.ChsTran chsTran = new SqlSugarModel.ChsTran();
            chsTran.HOS_ID = HOS_ID;
            chsTran.BIZ_TYPE = "02";//01挂号 02缴费
            chsTran.BIZ_NO = HOS_SN;
            chsTran.TRAN_ID = chsTran.HOS_ID + "_" + chsTran.BIZ_TYPE + "_" + chsTran.BIZ_NO;
            chsTran.psn_no = psn_no;
            chsTran.insuplc_admdvs = insuplc_admdvs;
            chsTran.mdtrt_id = mdtrt_id;
            chsTran.chsOutput1101 = chsOutput1101;
            chsTran.chsInput2201 = JsonConvert.SerializeObject(inputRoot2201);
            chsTran.chsOutput2201 = chsOutput2201;
            chsTran.chsInput2203 = JsonConvert.SerializeObject(inputRoot2203);
            chsTran.chsInput2204 = JsonConvert.SerializeObject(inputRoot2204);
            chsTran.chsOutput2204 = JsonConvert.SerializeObject(outputRoot2204);
            chsTran.chsInput2206 = JsonConvert.SerializeObject(inputRoot2206);
            chsTran.chsOutput2206 = JsonConvert.SerializeObject(outputRoot2206);
            chsTran.create_time = DateTime.Now;
            db.Saveable(chsTran).ExecuteCommand();
            #endregion

            Models.RT2206.Root rt2206 = outputRoot2206.GetOutput<Models.RT2206.Root>();
            _out.MDTRT_ID = rt2206.setlinfo.mdtrt_id;
            _out.MEDFEE_SUMAMT = FormatHelper.GetStr(rt2206.setlinfo.medfee_sumamt);
            _out.ACCT_PAY = FormatHelper.GetStr(rt2206.setlinfo.acct_pay);
            _out.PSN_CASH_PAY = FormatHelper.GetStr(rt2206.setlinfo.psn_cash_pay);
            _out.FUND_PAY_SUMAMT = FormatHelper.GetStr(rt2206.setlinfo.fund_pay_sumamt);
            _out.OTH_PAY = FormatHelper.GetStr(rt2206.setlinfo.oth_pay);
            _out.BALC = FormatHelper.GetStr(rt2206.setlinfo.balc);
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;
        }

        public virtual DataReturn SETTLE(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_SETTLE_M.GJYB_SETTLE_IN _in = JSONSerializer.Deserialize<GJYB_SETTLE_M.GJYB_SETTLE_IN>(injson);
            GJYB_SETTLE_M.GJYB_SETTLE_OUT _out = new GJYB_SETTLE_M.GJYB_SETTLE_OUT();

            string HOS_ID = _in.HOS_ID; 
            string opter_no = _in.USER_ID; 
            string HOS_SN = _in.HOS_SN; 
            string ISGH = _in.ISGH;
            string TRAN_ID = HOS_ID + "_" + (ISGH == "1" ? "01" : "02") + "_" + HOS_SN;
            
          
            var db = new DbContext().Client;
            SqlSugarModel.ChsTran chsTran = db.Queryable<SqlSugarModel.ChsTran>().Where(t => t.TRAN_ID == TRAN_ID).Single();
            if (chsTran == null)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "未取到预结算信息";
                return dataReturn;
            }
            string psn_no = chsTran.psn_no;
            string insuplc_admdvs = chsTran.insuplc_admdvs;
           
            string chsInput2206 = chsTran.chsInput2206;
            string chsOutput2206 = chsTran.chsOutput2206;

            //调用HIS获取结算报文
            string chsInput2207 = "";
            if (ISGH == "1")
            {
                //3.3.14获取挂号医保结算数据（TYPE：GETCHSREGSAVE）[医保B方案]

               
            }
            else 
            {
                //3.4.17获取门诊缴费医保结算数据（TYPE：GETCHSOUTPSAVE）[医保B方案]


            }
            string msg = "";
            string infno = "";
            bool flag = false;
            //门诊结算【2207】
            infno = "2207";
            Models.InputRoot inputRoot2207 = new Models.InputRoot();
            JObject jin2207 = JObject.Parse(chsInput2207);
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2207, ref inputRoot2207, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2207 = GlobalVar.YBTrans(HOS_ID, inputRoot2207);
            if (outputRoot2207.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2207.err_msg;
                return dataReturn;
            }
            Models.RT2207.Root rt2207 = outputRoot2207.GetOutput<Models.RT2207.Root>();

            //保存记录
            chsTran.chsInput2207 = JsonConvert.SerializeObject(inputRoot2207);
            chsTran.chsOutput2207 = JsonConvert.SerializeObject(outputRoot2207);
            chsTran.setl_id = rt2207.setlinfo.setl_id;
            db.Updateable(chsTran).ExecuteCommand();

            _out.MDTRT_ID = rt2207.setlinfo.mdtrt_id;
            _out.SETL_ID = rt2207.setlinfo.mdtrt_id;

            _out.MEDFEE_SUMAMT = FormatHelper.GetStr(rt2207.setlinfo.medfee_sumamt);
            _out.ACCT_PAY = FormatHelper.GetStr(rt2207.setlinfo.acct_pay);
            _out.PSN_CASH_PAY = FormatHelper.GetStr(rt2207.setlinfo.psn_cash_pay);
            _out.FUND_PAY_SUMAMT = FormatHelper.GetStr(rt2207.setlinfo.fund_pay_sumamt);
            _out.OTH_PAY = FormatHelper.GetStr(rt2207.setlinfo.oth_pay);
            _out.BALC = FormatHelper.GetStr(rt2207.setlinfo.balc);
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;
        }

        public virtual DataReturn REFUND(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_REFUND_M.GJYB_REFUND_IN _in = JSONSerializer.Deserialize<GJYB_REFUND_M.GJYB_REFUND_IN>(injson);
            GJYB_REFUND_M.GJYB_REFUND_OUT _out = new GJYB_REFUND_M.GJYB_REFUND_OUT();

            string HOS_ID = _in.HOS_ID; 
            string opter_no = _in.USER_ID; 

            string mdtrt_id = _in.MDTRT_ID; 
            string setl_id = _in.SETL_ID; 

            var db = new DbContext().Client;
            SqlSugarModel.ChsTran chsTran = db.Queryable<SqlSugarModel.ChsTran>().Where(t => t.mdtrt_id == mdtrt_id && t.setl_id== setl_id).Single();
            if (chsTran == null)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "未取到结算信息";
                return dataReturn;
            }
            string psn_no = chsTran.psn_no;
            string insuplc_admdvs = chsTran.insuplc_admdvs;

           
            string msg = "";
            string infno = "";
            bool flag = false;
            //门诊结算撤销【2208】
            infno = "2208";
            Models.InputRoot inputRoot2208 = new Models.InputRoot();
            JObject jin2208 = new JObject();
            JObject jin2208data = new JObject();
            jin2208data["psn_no"] = psn_no;
            jin2208data["mdtrt_id"] = mdtrt_id;
            jin2208data["setl_id"] = setl_id;
            jin2208["data"] = jin2208data;

            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, jin2208, ref inputRoot2208, ref msg);
            if (!flag)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = msg;
                return dataReturn;
            }
            //调用医保
            Models.OutputRoot outputRoot2208 = GlobalVar.YBTrans(HOS_ID, inputRoot2208);
            if (outputRoot2208.infcode != "0")
            {
                dataReturn.Code = 1;
                dataReturn.Msg = outputRoot2208.err_msg;
                return dataReturn;
            }

            //保存记录
            chsTran.chsInput2208 = JsonConvert.SerializeObject(inputRoot2208);
            chsTran.chsOutput2208 = JsonConvert.SerializeObject(outputRoot2208);
            db.Updateable(chsTran).ExecuteCommand();

            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            return dataReturn;
        }

        public virtual DataReturn COMMON(string json_in)
        {
            throw new NotImplementedException();
        }
    }
}
