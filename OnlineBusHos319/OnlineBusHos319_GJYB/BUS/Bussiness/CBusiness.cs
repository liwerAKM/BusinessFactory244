
using CommonModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineBusHos319_GJYB.Model;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace OnlineBusHos319_GJYB.BUS
{
    /// <summary>
    /// C方案
    /// </summary>
    public class CBusiness :BBusiness
    {
        //读卡不用重写
        /*
        public override void PSNQUERY(string inputdata, out string outputdata)
        {
            throw new NotImplementedException();
        }
        */

        public override DataReturn REGTRY(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_REGTRY_M.GJYB_REGTRY_IN _in = JSONSerializer.Deserialize<GJYB_REGTRY_M.GJYB_REGTRY_IN>(injson);
            GJYB_REGTRY_M.GJYB_REGTRY_OUT _out = new GJYB_REGTRY_M.GJYB_REGTRY_OUT();
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

            string msg = "";
            string infno = "";
            bool flag = false;
            #region 挂号登记2201
            //险种的处理
            string insutype = "";
            Models.OutputRoot out1101 = JsonConvert.DeserializeObject<Models.OutputRoot>(chspsn.chsOutput1101);
            Models.RT1101.Root rt1101 = out1101.GetOutput<Models.RT1101.Root>();
            insutype = rt1101.insuinfo[0].insutype;

            Models.T2201.Root t2201 = new Models.T2201.Root();
            t2201.data = new  Models.T2201.Data();
            t2201.data.psn_no = psn_no;
            t2201.data.insutype = insutype;
            t2201.data.begntime = "";//
            t2201.data.mdtrt_cert_type = chspsn.mdtrt_cert_type;
            t2201.data.mdtrt_cert_no = chspsn.mdtrt_cert_no;
            t2201.data.ipt_otp_no = "";//
            t2201.data.atddr_no = "";//
            t2201.data.dr_name = "";//
            t2201.data.dept_code = "";//
            t2201.data.dept_name = "";//
            t2201.data.caty = "";//
            t2201.data.expContent = "";

            infno = "2201";
            Models.InputRoot inputRoot2201 = new Models.InputRoot();
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2201, ref inputRoot2201, ref msg);
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
            #endregion

            #region 门诊就诊信息上传A【2203A】
            //医疗类别的处理
            string med_type = "11";
            Models.T2203.Root t2203 = new Models.T2203.Root();
            t2203.mdtrtinfo = new Models.T2203.mdtrtinfo();
            t2203.mdtrtinfo.mdtrt_id = mdtrt_id;
            t2203.mdtrtinfo.psn_no = psn_no;
            t2203.mdtrtinfo.med_type = med_type;
            t2203.mdtrtinfo.begntime = "";//
            t2203.mdtrtinfo.main_cond_dscr = "";//
            t2203.mdtrtinfo.dise_codg = "";//
            t2203.mdtrtinfo.dise_name = "";//
            t2203.mdtrtinfo.birctrl_type = "";
            t2203.mdtrtinfo.birctrl_matn_date = "";
            t2203.mdtrtinfo.matn_type = "";
            t2203.mdtrtinfo.geso_val = 0;
            t2203.mdtrtinfo.expContent = "";

            List<Models.T2203.diseinfo> diseinfos = new List<Models.T2203.diseinfo>();
            Models.T2203.diseinfo diseinfo = new Models.T2203.diseinfo();
            diseinfo.diag_type = "1";
            diseinfo.diag_srt_no = "1";
            diseinfo.diag_code = "";//
            diseinfo.diag_name = "";//
            diseinfo.diag_dept = "";//
            diseinfo.dise_dor_no = "";//
            diseinfo.dise_dor_name = "";//
            diseinfo.diag_time = "";//
            diseinfo.vali_flag = "1";
            diseinfos.Add(diseinfo);
            t2203.diseinfo = diseinfos;

            infno = "2203A";
            Models.InputRoot inputRoot2203 = new Models.InputRoot();
           
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2203, ref inputRoot2203, ref msg);
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
            #endregion
            //门诊费用明细信息撤销【2205】

            #region 门诊费用明细信息上传【2204】
            Models.T2204.Root t2204 = new Models.T2204.Root();
            List<Models.T2204.Feedetail> feedetails = new List<Models.T2204.Feedetail>();
            //HIS明细数据
            DataTable dtDetail = new DataTable();
            foreach (DataRow dr in dtDetail.Rows)
            {
                Models.T2204.Feedetail feedetail = new Models.T2204.Feedetail();
                feedetail.mdtrt_id =mdtrt_id;
                feedetail.psn_no = psn_no;

                feedetail.feedetl_sn = FormatHelper.GetStr(dr[""]);

                feedetail.chrg_bchno = FormatHelper.GetStr(dr[""]);
                feedetail.dise_codg = FormatHelper.GetStr(dr[""]);
                feedetail.rxno = FormatHelper.GetStr(dr[""]);
                feedetail.rx_circ_flag = FormatHelper.GetStr(dr[""]);
                feedetail.fee_ocur_time = FormatHelper.GetStr(dr[""]);
                feedetail.med_list_codg = FormatHelper.GetStr(dr[""]);
                feedetail.medins_list_codg = FormatHelper.GetStr(dr[""]);
                feedetail.det_item_fee_sumamt = FormatHelper.GetDecimal(dr[""]);
                feedetail.cnt = FormatHelper.GetDecimal(dr[""]);
                feedetail.pric = FormatHelper.GetDecimal(dr[""]);
                feedetail.sin_dos_dscr = FormatHelper.GetStr(dr[""]);
                feedetail.used_frqu_dscr = FormatHelper.GetStr(dr[""]);
                feedetail.prd_days = FormatHelper.GetDecimal(dr[""]);
                feedetail.medc_way_dscr = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dept_codg = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dept_name = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dr_codg = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dr_name = FormatHelper.GetStr(dr[""]);
                feedetail.acord_dept_codg = FormatHelper.GetStr(dr[""]);
                feedetail.acord_dept_name = FormatHelper.GetStr(dr[""]);
                feedetail.orders_dr_code = FormatHelper.GetStr(dr[""]);
                feedetail.orders_dr_name = FormatHelper.GetStr(dr[""]);
                feedetail.hosp_appr_flag = FormatHelper.GetStr(dr[""]);
                feedetail.tcmdrug_used_way = FormatHelper.GetStr(dr[""]);
                feedetail.etip_flag = FormatHelper.GetStr(dr[""]);
                feedetail.etip_hosp_code = FormatHelper.GetStr(dr[""]);
                feedetail.dscg_tkdrug_flag = FormatHelper.GetStr(dr[""]);
                feedetail.matn_fee_flag = FormatHelper.GetStr(dr[""]);
                if (!string.IsNullOrEmpty(FormatHelper.GetStr(dr[""])) || !string.IsNullOrEmpty(FormatHelper.GetStr(dr[""])))
                {
                    feedetail.expContent = new Models.T2204.ExpContent();
                    feedetail.expContent.tcmherb_prov_code = FormatHelper.GetStr(dr[""]);
                    feedetail.expContent.mcs_prov_code = FormatHelper.GetStr(dr[""]);
                }
                feedetails.Add(feedetail);
            }
            t2204.feedetail = feedetails;

            infno = "2204";
            Models.InputRoot inputRoot2204 = new Models.InputRoot();

            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2204, ref inputRoot2204, ref msg);
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
            #endregion
            #region 门诊预结算【2206】
            Models.T2206.Root t2206 = new Models.T2206.Root();
            t2206.data = new Models.T2206.Data();
            t2206.data.psn_no = psn_no;
            t2206.data.mdtrt_cert_type = chspsn.mdtrt_cert_type;
            t2206.data.mdtrt_cert_no = chspsn.mdtrt_cert_no.Split('|')[0];
            t2206.data.med_type = med_type;
            t2206.data.medfee_sumamt =0;//总金额
            t2206.data.psn_setlway = "1";
            t2206.data.mdtrt_id =mdtrt_id;
            t2206.data.chrg_bchno = "";//
            t2206.data.acct_used_flag = "1";
            t2206.data.insutype = insutype;
            t2206.data.expContent = "";

            infno = "2206";
            Models.InputRoot inputRoot2206 = new Models.InputRoot();

            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2206, ref inputRoot2206, ref msg);
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
            #endregion
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
        public override DataReturn OUTPTRY(string injson)
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

            string msg = "";
            string infno = "";
            bool flag = false;
            bool reg = false;//是否需要重新登记
            string mdtrt_id = "";
            //医疗类别的处理
            string med_type = "11";
            //险种的处理
            string insutype = "";

            string chsOutput2201 = "";
            Models.InputRoot inputRoot2201 = new Models.InputRoot();
            Models.InputRoot inputRoot2203 = new Models.InputRoot();
            if (reg) 
            {
                #region 挂号登记2201
                Models.OutputRoot out1101 = JsonConvert.DeserializeObject<Models.OutputRoot>(chspsn.chsOutput1101);
                Models.RT1101.Root rt1101 = out1101.GetOutput<Models.RT1101.Root>();
                insutype = rt1101.insuinfo[0].insutype;

                Models.T2201.Root t2201 = new Models.T2201.Root();
                t2201.data = new Models.T2201.Data();
                t2201.data.psn_no = psn_no;
                t2201.data.insutype = insutype;
                t2201.data.begntime = "";//
                t2201.data.mdtrt_cert_type = chspsn.mdtrt_cert_type;
                t2201.data.mdtrt_cert_no = chspsn.mdtrt_cert_no;
                t2201.data.ipt_otp_no = "";//
                t2201.data.atddr_no = "";//
                t2201.data.dr_name = "";//
                t2201.data.dept_code = "";//
                t2201.data.dept_name = "";//
                t2201.data.caty = "";//
                t2201.data.expContent = "";

                infno = "2201";

                flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2201, ref inputRoot2201, ref msg);
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
                chsOutput2201 = JsonConvert.SerializeObject(outputRoot2201);
                #endregion

                #region 门诊就诊信息上传A【2203A】

                Models.T2203.Root t2203 = new Models.T2203.Root();
                t2203.mdtrtinfo = new Models.T2203.mdtrtinfo();
                t2203.mdtrtinfo.mdtrt_id = mdtrt_id;
                t2203.mdtrtinfo.psn_no = psn_no;
                t2203.mdtrtinfo.med_type = med_type;
                t2203.mdtrtinfo.begntime = "";//
                t2203.mdtrtinfo.main_cond_dscr = "";//
                t2203.mdtrtinfo.dise_codg = "";//
                t2203.mdtrtinfo.dise_name = "";//
                t2203.mdtrtinfo.birctrl_type = "";
                t2203.mdtrtinfo.birctrl_matn_date = "";
                t2203.mdtrtinfo.matn_type = "";
                t2203.mdtrtinfo.geso_val = 0;
                t2203.mdtrtinfo.expContent = "";

                List<Models.T2203.diseinfo> diseinfos = new List<Models.T2203.diseinfo>();
                Models.T2203.diseinfo diseinfo = new Models.T2203.diseinfo();
                diseinfo.diag_type = "1";
                diseinfo.diag_srt_no = "1";
                diseinfo.diag_code = "";//
                diseinfo.diag_name = "";//
                diseinfo.diag_dept = "";//
                diseinfo.dise_dor_no = "";//
                diseinfo.dise_dor_name = "";//
                diseinfo.diag_time = "";//
                diseinfo.vali_flag = "1";
                diseinfos.Add(diseinfo);
                t2203.diseinfo = diseinfos;

                infno = "2203A";


                flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2203, ref inputRoot2203, ref msg);
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
                #endregion
            }
            //门诊费用明细信息撤销【2205】
            if (!reg)
            {
                //不是重新登记的，撤销明细
                infno = "2205";
                Models.InputRoot inputRoot2205 = new Models.InputRoot();
                JObject jin2205 = new JObject();
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
            #region 门诊费用明细信息上传【2204】
            Models.T2204.Root t2204 = new Models.T2204.Root();
            List<Models.T2204.Feedetail> feedetails = new List<Models.T2204.Feedetail>();
            //HIS明细数据
            DataTable dtDetail = new DataTable();
            foreach (DataRow dr in dtDetail.Rows)
            {
                Models.T2204.Feedetail feedetail = new Models.T2204.Feedetail();
                feedetail.mdtrt_id = mdtrt_id;
                feedetail.psn_no = psn_no;

                feedetail.feedetl_sn = FormatHelper.GetStr(dr[""]);

                feedetail.chrg_bchno = FormatHelper.GetStr(dr[""]);
                feedetail.dise_codg = FormatHelper.GetStr(dr[""]);
                feedetail.rxno = FormatHelper.GetStr(dr[""]);
                feedetail.rx_circ_flag = FormatHelper.GetStr(dr[""]);
                feedetail.fee_ocur_time = FormatHelper.GetStr(dr[""]);
                feedetail.med_list_codg = FormatHelper.GetStr(dr[""]);
                feedetail.medins_list_codg = FormatHelper.GetStr(dr[""]);
                feedetail.det_item_fee_sumamt = FormatHelper.GetDecimal(dr[""]);
                feedetail.cnt = FormatHelper.GetDecimal(dr[""]);
                feedetail.pric = FormatHelper.GetDecimal(dr[""]);
                feedetail.sin_dos_dscr = FormatHelper.GetStr(dr[""]);
                feedetail.used_frqu_dscr = FormatHelper.GetStr(dr[""]);
                feedetail.prd_days = FormatHelper.GetDecimal(dr[""]);
                feedetail.medc_way_dscr = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dept_codg = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dept_name = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dr_codg = FormatHelper.GetStr(dr[""]);
                feedetail.bilg_dr_name = FormatHelper.GetStr(dr[""]);
                feedetail.acord_dept_codg = FormatHelper.GetStr(dr[""]);
                feedetail.acord_dept_name = FormatHelper.GetStr(dr[""]);
                feedetail.orders_dr_code = FormatHelper.GetStr(dr[""]);
                feedetail.orders_dr_name = FormatHelper.GetStr(dr[""]);
                feedetail.hosp_appr_flag = FormatHelper.GetStr(dr[""]);
                feedetail.tcmdrug_used_way = FormatHelper.GetStr(dr[""]);
                feedetail.etip_flag = FormatHelper.GetStr(dr[""]);
                feedetail.etip_hosp_code = FormatHelper.GetStr(dr[""]);
                feedetail.dscg_tkdrug_flag = FormatHelper.GetStr(dr[""]);
                feedetail.matn_fee_flag = FormatHelper.GetStr(dr[""]);
                if (!string.IsNullOrEmpty(FormatHelper.GetStr(dr[""])) || !string.IsNullOrEmpty(FormatHelper.GetStr(dr[""])))
                {
                    feedetail.expContent = new Models.T2204.ExpContent();
                    feedetail.expContent.tcmherb_prov_code = FormatHelper.GetStr(dr[""]);
                    feedetail.expContent.mcs_prov_code = FormatHelper.GetStr(dr[""]);
                }
                feedetails.Add(feedetail);
            }
            t2204.feedetail = feedetails;

            infno = "2204";
            Models.InputRoot inputRoot2204 = new Models.InputRoot();

            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2204, ref inputRoot2204, ref msg);
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
            #endregion
            #region 门诊预结算【2206】
            Models.T2206.Root t2206 = new Models.T2206.Root();
            t2206.data = new Models.T2206.Data();
            t2206.data.psn_no = psn_no;
            t2206.data.mdtrt_cert_type = chspsn.mdtrt_cert_type;
            t2206.data.mdtrt_cert_no = chspsn.mdtrt_cert_no.Split('|')[0];
            t2206.data.med_type = med_type;
            t2206.data.medfee_sumamt = 0;//总金额
            t2206.data.psn_setlway = "1";
            t2206.data.mdtrt_id = mdtrt_id;
            t2206.data.chrg_bchno = "";//
            t2206.data.acct_used_flag = "1";
            t2206.data.insutype = insutype;
            t2206.data.expContent = "";

            infno = "2206";
            Models.InputRoot inputRoot2206 = new Models.InputRoot();

            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2206, ref inputRoot2206, ref msg);
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
            #endregion
            //保存记录
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

        public override DataReturn SETTLE(string injson)
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

            SqlSugarModel.ChsPsn chspsn = db.Queryable<SqlSugarModel.ChsPsn>().Where(t => t.psn_no == psn_no).Single();
            if (chspsn == null)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "未取到个人信息";
                return dataReturn;
            }

            Models.InputRoot input = JsonConvert.DeserializeObject<Models.InputRoot>(chsInput2206);


            //调用HIS获取结算报文
            //Models.T2207.Root t2207 = new Models.T2207.Root();
            Models.T2207.Root t2207 = input.GetInput<Models.T2207.Root>();//直接把预结算参数取过来
            if (t2207.data == null)
            {
                t2207.data = new Models.T2207.Data();
            }
            t2207.data.psn_no = psn_no;
            t2207.data.mdtrt_cert_type = chspsn.mdtrt_cert_type;
            t2207.data.mdtrt_cert_no = chspsn.mdtrt_cert_no;
            /*t2207.data.med_type = pre_setlinfo.med_type;
            t2207.data.medfee_sumamt = pre_setlinfo.medfee_sumamt;
            t2207.data.psn_setlway = pre_setlinfo.psn_setlway;
            t2207.data.mdtrt_id = pre_setlinfo.mdtrt_id;
            t2207.data.chrg_bchno = pre_setlinfo.chrg_bchno;
            t2207.data.insutype = pre_setlinfo.insutype;
            t2207.data.acct_used_flag = "1";
            t2207.data.invono = "";
            t2207.data.fulamt_ownpay_amt = pre_setlinfo.fulamt_ownpay_amt;
            t2207.data.overlmt_selfpay = pre_setlinfo.overlmt_selfpay;
            t2207.data.preselfpay_amt = pre_setlinfo.preselfpay_amt;
            t2207.data.inscp_scp_amt = pre_setlinfo.inscp_scp_amt;
            t2207.data.expContent = pre_setlinfo.expContent;*/
            string msg = "";
            string infno = "";
            bool flag = false;
            //门诊结算【2207】
            infno = "2207";
            Models.InputRoot inputRoot2207 = new Models.InputRoot();
            flag = CSBHelper.CreateInputRoot(HOS_ID, infno, "", opter_no, insuplc_admdvs, t2207, ref inputRoot2207, ref msg);
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

        /*
        public override void REFUND(string inputdata, out string outputdata)
        {
            throw new NotImplementedException();
        }
        */


        public override DataReturn COMMON(string injson)
        {
            throw new NotImplementedException();
        }
    }
}
