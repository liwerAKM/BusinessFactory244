using System;
using System.Collections.Generic;
using System.Text;
using CommonModel;
using Soft.Core;
using System.Linq;
namespace OnlineBusHos36_GJYB.BUS
{
    class GJYB_PSNINFOQUERY
    {
        public static string B_GJYB_PSNINFOQUERY(string json_in)
        {
            Model.GJYB_PSNINFOQUERY_M.GJYB_PSNINFOQUERY_IN _in = JSONSerializer.Deserialize<Model.GJYB_PSNINFOQUERY_M.GJYB_PSNINFOQUERY_IN>(json_in);
            Model.GJYB_PSNINFOQUERY_M.GJYB_PSNINFOQUERY_OUT _out = new Model.GJYB_PSNINFOQUERY_M.GJYB_PSNINFOQUERY_OUT();
            DataReturn dataReturnQUERY = new DataReturn();
            Model.GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN gJYB_PSNQUERY_IN = new Model.GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN()
            {
                USER_ID = _in.USER_ID,
                PSN_NAME = _in.PSN_NAME,
                LTERMINAL_SN = _in.LTERMINAL_SN,
                HOS_ID = _in.HOS_ID,
                BEGNTIME = _in.BEGNTIME,
                CARD_SN = _in.CARD_SN,
                CERTNO = _in.CERTNO,
                MDTRT_CERT_NO = _in.MDTRT_CERT_NO,
                MDTRT_CERT_TYPE = _in.MDTRT_CERT_TYPE,
                PSN_CERT_TYPE = _in.PSN_CERT_TYPE
            };
            DataReturn dataReturn = GlobalVar.business.PSNINFOQUERY(JSONSerializer.Serialize(gJYB_PSNQUERY_IN));
            if (dataReturn.Code == 0)
            {
       
                Model.GJYB_PSNQUERY_M.GJYB_PSNINFOQUERY_OUT gJYB_PSNQUERY_OUT = JSONSerializer.Deserialize<Model.GJYB_PSNQUERY_M.GJYB_PSNINFOQUERY_OUT>(dataReturn.Param);

        
                Models.RT1101NEW.Root rt1101 = JSONSerializer.Deserialize<Models.RT1101NEW.Root>(gJYB_PSNQUERY_OUT.PAT_CARD_OUT);
                var insurinfo = rt1101.output.insuinfo.Find(t => string.IsNullOrEmpty(t.paus_insu_date) || t.paus_insu_date.CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) > 0);
     
                string Exp = rt1101.output.baseinfo.exp_content;

                Models.ExpContent expout = JSONSerializer.Deserialize<Models.ExpContent>(Exp);

      
                Models.RT5360.Root rt5360 = JSONSerializer.Deserialize<Models.RT5360.Root>(gJYB_PSNQUERY_OUT.CHSOUTPUT5360);
         
                Models.RT5206.Root rt5206 = JSONSerializer.Deserialize<Models.RT5206.Root>(gJYB_PSNQUERY_OUT.CHSOUTPUT5206);

                _out.CARD_NO = "";
                _out.CERTNO = rt1101.output.baseinfo.certno;
                _out.PSN_NAME = rt1101.output.baseinfo.psn_name;
                _out.GEND = rt1101.output.baseinfo.gend == "1" ? "男" : "女";
                _out.NATY = rt1101.output.baseinfo.naty == "01" ? "汉族" : "";
                _out.BRDY = rt1101.output.baseinfo.brdy;
                _out.PSN_NO = rt1101.output.baseinfo.psn_no;
                _out.PSN_TYPE = PSN_TYPENAME(insurinfo.psn_type);
                _out.CVLSERV_FLAG = insurinfo.cvlserv_flag == "0" ? "否" : "是";
                _out.PSN_INSU_STAS = insurinfo.psn_insu_stas;

                if (rt5360 != null && rt5360.output.data.Count > 0)
                {
                    string medtype_name = "";
                    string dise_code = "";
                    string dise_name = "";
                    foreach (Models.RT5360.insuinfo insuinfo in rt5360.output.data)
                    {
                        medtype_name += "|" + Yltypename(insuinfo.med_type);
                        dise_code += "|" + insuinfo.dise_codg;
                        dise_name += "|" + insuinfo.dise_name;

                    }
                    _out.MED_TYPE = medtype_name;
                    _out.DISE_CODG = dise_code;
                    _out.DISE_NAME = dise_name;
                }

                _out.ACCT_USED_FLAG = "";
                _out.PSN_SETLWAY = "";
                _out.BALC = insurinfo.balc.ToString();
                _out.INSUTYPE = insutypename(insurinfo.insutype);
                _out.PSN_INSU_DATE = insurinfo.psn_insu_date;
                _out.PAUS_INSU_DATE = insurinfo.paus_insu_date;

                string CUM = "";
                if (rt5206 != null && rt5206.output.cuminfo.Count > 0)
                {
                    var query = rt5206.output.cuminfo.GroupBy(item => new { item.year, item.insutype }).Select
                         (a => new
                         {
                             year = a.Key.year,
                             insutype = a.Key.insutype,
                             cumSum = a.Sum(c => FormatHelper.GetDecimal(c.cum))
                         }).ToList();
                    foreach (var items in query)
                    {
                        CUM += "|" + insutypename(items.insutype) + "&" + items.year + "&" + items.cumSum.ToString();
                    }

                    //for (int i = 0; i < rt5206.output.cuminfo.Count; i++)
                    //{ 
                    //    CUM += "|" + insutypename(rt5206.output.cuminfo[i].insutype) + "&" + rt5206.output.cuminfo[i].cum_ym + "&" + rt5206.output.cuminfo[i].cum_type_code + "&" + rt5206.output.cuminfo[i].cum.ToString();
                    //}
                }

                #region 处理门特相关数据
                string SPDISE_CONTENT = "";
                if (FormatHelper.GetDecimal(expout.opt_spdise_tmor_chmo_balc) != 0)
                {
                    SPDISE_CONTENT += "|" + "门特恶性肿瘤放化疗余额:" + expout.opt_spdise_tmor_chmo_balc;
                }
                if (FormatHelper.GetDecimal(expout.opt_spdise_tmor_medn_balc) != 0)
                {
                    SPDISE_CONTENT += "|" + "门特恶性肿瘤针对性药物:" + expout.opt_spdise_tmor_medn_balc;
                }
                if (FormatHelper.GetDecimal(expout.opt_spdise_tmor_asst_medn_balc) != 0)
                {
                    SPDISE_CONTENT += "|" + "门特恶性肿瘤辅助药物余额:" + expout.opt_spdise_tmor_asst_medn_balc;
                }
                if (FormatHelper.GetDecimal(expout.opt_spdise_blo_abd_diay_medn_balc) != 0)
                {
                    SPDISE_CONTENT += "|" + "门特血腹透余额:" + expout.opt_spdise_blo_abd_diay_medn_balc;
                }
                if (FormatHelper.GetDecimal(expout.opt_spdise_blo_abd_diay_asst_medn_balc) != 0)
                {
                    SPDISE_CONTENT += "|" + "门特血腹透辅助药物余额:" + expout.opt_spdise_blo_abd_diay_asst_medn_balc;
                }
                if (FormatHelper.GetDecimal(expout.opt_spdise_organ_transplant_medn_balc) != 0)
                {
                    SPDISE_CONTENT += "|" + "门特器官移植余额:" + expout.opt_spdise_organ_transplant_medn_balc;
                }
                if (FormatHelper.GetDecimal(expout.opt_spdise_organ_transplant_asst_medn_balc) != 0)
                {
                    SPDISE_CONTENT += "|" + "门特器官移植辅助药物余额:" + expout.opt_spdise_organ_transplant_asst_medn_balc;
                }
                #endregion

                _out.CUM = CUM;
                _out.INHOSP_STAS = expout.inhosp_stas == "0" ? "出院" : "在院";
                _out.OPSP_BALC = expout.opsp_balc;
                _out.OPT_POOL_BALC = expout.otp_pool_balc;
                _out.OPT_SPDISE_CONTENT = SPDISE_CONTENT;
                _out.TRT_CHK_RSLT = expout.trt_chk_rslt;
                _out.HIS_RTNXML = dataReturn.Param;
                dataReturnQUERY.Code = 0;
                dataReturnQUERY.Msg = "SUCCESS";
                dataReturnQUERY.Param = JSONSerializer.Serialize(_out);


            }
            else
            {
                dataReturnQUERY.Code = 1;
                dataReturnQUERY.Msg = "未获取相关信息";
                dataReturnQUERY.Param = dataReturn.Param;
            }
            string json_out = JSONSerializer.Serialize(dataReturnQUERY);
            return json_out;
        }


        public static string PSN_TYPENAME(string Psn_type)
        {
            string PSN_NAME = "";
            switch (Psn_type)
            {
                case "11":
                    PSN_NAME = "在职";
                    break;
                case "12":
                    PSN_NAME = "退休人员";
                    break;
                case "1204":
                    PSN_NAME = "退职职工";
                    break;
                case "995307":
                    PSN_NAME = "退休审核期";
                    break;
                case "13":
                    PSN_NAME = "一般离休";
                    break;
                case "136007":
                    PSN_NAME = "地市级离休";
                    break;
                case "130311":
                    PSN_NAME = "副省级报销离休";
                    break;
                case "1300":
                    PSN_NAME = "一般离休(浦口)";
                    break;
                case "130107":
                    PSN_NAME = "区四套班子(浦口)";
                    break;
                case "136005":
                    PSN_NAME = "副省级离休";
                    break;
                case "130106":
                    PSN_NAME = "军干离休";
                    break;
                case "130105":
                    PSN_NAME = "区四套班子";
                    break;
                case "34":
                    PSN_NAME = "建国前老工人";
                    break;
                case "995310":
                    PSN_NAME = "二乙伤残军人";
                    break;
                case "156001":
                    PSN_NAME = "普通居民";
                    break;
                case "149901":
                    PSN_NAME = "学生儿童";
                    break;
                case "1404":
                    PSN_NAME = "大学生";
                    break;
                case "1105":
                    PSN_NAME = "农民工";
                    break;
                case "1401":
                    PSN_NAME = "新生儿";
                    break;
                case "140101":
                    PSN_NAME = "准新生儿";
                    break;
            }
            return PSN_NAME;
        }


        public static string Yltypename(string med_type)
        {
            string yltype_name = "";
            switch (med_type)
            {
                case "140101":
                    yltype_name = "门诊大病";
                    break;
                case "990902":
                    yltype_name = "门诊专项用药";
                    break;
                case "510103":
                    yltype_name = "居民产前检查";
                    break;
                case "1404":
                    yltype_name = "居民门诊两病";
                    break;
                case "1301":
                    yltype_name = "急诊抢救";
                    break;
                case "13":
                    yltype_name = "急诊";
                    break;
                case "110106":
                    yltype_name = "门诊精神病";
                    break;
                case "110107":
                    yltype_name = "门诊艾滋病";
                    break;
                case "510102":
                    yltype_name = "产前检查";
                    break;
                case "140201":
                    yltype_name = "门诊特病";
                    break;
                case "140104":
                    yltype_name = "门诊慢病";
                    break;
                case "530102":
                    yltype_name = "计划生育门诊";
                    break;
                case "110104":
                    yltype_name = "门诊统筹";
                    break;
            }
            return yltype_name;

        }


        public static string insutypename(string insutype)
        {
            string insutype_name = "";
            switch (insutype)
            {
                case "310":
                    insutype_name = "职工基本医疗保险";
                    break;
                case "312":
                    insutype_name = "农民工住院医疗";
                    break;
                case "390":
                    insutype_name = "城乡居民基本医疗保险";
                    break;
                case "320":
                    insutype_name = "公务员医疗补助";
                    break;
                case "510":
                    insutype_name = "生育保险";
                    break;
                case "340":
                    insutype_name = "离休人员医疗保障";
                    break;

            }
            return insutype_name;
        }


    }
}
