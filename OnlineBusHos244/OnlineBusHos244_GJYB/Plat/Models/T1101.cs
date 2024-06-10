using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos244_GJYB.Models
{
    #region N：6.1.1.1 【1101】人员信息获取

    public class T1101
    {
        /// <summary>
        /// 输入（节点标识：data）
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 01,02,03
            /// </summary>
            public string mdtrt_cert_type { get; set; }// 就诊凭证类型
            /// <summary>
            /// 就诊凭证编号：就诊凭证类型为“01”时填写电子凭证令牌，为“02”时填写身份证号，为“03”时填写社会保障卡卡号
            /// </summary>
            public string mdtrt_cert_no { get; set; }
            /// <summary>
            /// 卡识别码：就诊凭证类型为“03”时必填
            /// </summary>
            public string card_sn { get; set; }// 卡识别码
            public string begntime { get; set; }// 开始时间
            public string psn_cert_type { get; set; }// 人员证件类型
            public string certno { get; set; }// 证件号码
            public string psn_name { get; set; }// 人员姓名

        }

        public class Root
        {
            public Data data { get; set; }
        }
    }


    public class RT1101
    {
        /// <summary>
        /// 输出-基本信息（节点标识：baseinfo）
        /// </summary>
        public class Baseinfo
        {
            public string psn_no { get; set; }//人员编号
            public string psn_cert_type { get; set; }// 人员证件类型
            public string certno { get; set; }// 证件号码
            public string psn_name { get; set; }// 人员姓名
            public string gend { get; set; }//性别
            public string naty { get; set; }// 民族
            public string brdy { get; set; }// 出生日期
            public string age { get; set; }//年龄

            public string exp_content { get; set; }
        }
        /// <summary>
        /// 输出-参保信息列表（节点标识insuinfo）
        /// </summary>
        public class Insuinfo
        {
            public decimal balc { get; set; }//余额
            public string insutype { get; set; }// 险种类型
            public string psn_type { get; set; }//人员类别
            public string psn_insu_stas { get; set; }

            public string psn_insu_date { get; set; }

            public string paus_insu_date { get; set; }
            public string cvlserv_flag { get; set; }//公务员标志
            public string insuplc_admdvs { get; set; }//参保地医保区划
            public string emp_name { get; set; }//单位名称

        }

        /// <summary>
        /// 输出-身份信息列表（节点标识：idetinfo）
        /// </summary>
        public class Idetinfo
        {
            public string psn_idet_type { get; set; }//人员身份类别
            public string psn_type_lv { get; set; }//人员类别等级
            public string memo { get; set; }//备注
            public string begntime { get; set; }//开始时间
            public string endtime { get; set; }//结束时间

        }

        public class Root
        {
            public Baseinfo baseinfo { get; set; }

            public List<Insuinfo> insuinfo { get; set; }

            public List<Idetinfo> idetinfo { get; set; }
        }
    }

    public class EXPOUT
    {
        public class Root
        {
            /// <summary>
            /// 灵活就业人员
            /// </summary>
            public string flx_emp_flag { get; set; }
            /// <summary>
            /// 门慢剩余金额
            /// </summary>
            public string opsp_balc { get; set; }
            /// <summary>
            /// 门诊两病剩余金额
            /// </summary>
            public string otp_dise_balc { get; set; }
        }
    }

    public class ExpContent
    {
        /// <summary>
        /// 灵活就业人员标志
        /// </summary>
        public string flx_emp_flag { get; set; }
        /// <summary>
        /// 门慢剩余金额
        /// </summary>
        public string opsp_balc { get; set; }
        /// <summary>
        /// 门诊两病剩余金额
        /// </summary>
        public string otp_dise_balc { get; set; }
        /// <summary>
        /// 门诊统筹剩余金额
        /// </summary>
        public string otp_pool_balc { get; set; }
        /// <summary>
        /// 小卡剩余金额
        /// </summary>
        public string pery_old_exam_balc { get; set; }
        /// <summary>
        /// 大卡剩余金额
        /// </summary>
        public string pery_new_exam_balc { get; set; }
        /// <summary>
        /// 居民产前检查剩余金额
        /// </summary>
        public string rsdt_pery_old_exam_balc { get; set; }
        /// <summary>
        /// 门特恶性肿瘤放化疗余额
        /// </summary>
        public string opt_spdise_tmor_chmo_balc { get; set; }
        /// <summary>
        /// 门特恶性肿瘤针对性药物
        /// </summary>
        public string opt_spdise_tmor_medn_balc { get; set; }
        /// <summary>
        /// 门特恶性肿瘤辅助药物余额
        /// </summary>
        public string opt_spdise_tmor_asst_medn_balc { get; set; }
        /// <summary>
        /// 门特血腹透余额
        /// </summary>
        public string opt_spdise_blo_abd_diay_medn_balc { get; set; }
        /// <summary>
        /// 门特血腹透辅助药物余额
        /// </summary>
        public string opt_spdise_blo_abd_diay_asst_medn_balc { get; set; }
        /// <summary>
        /// 门特器官移植余额
        /// </summary>
        public string opt_spdise_organ_transplant_medn_balc { get; set; }
        /// <summary>
        /// 门特器官移植辅助药物余额
        /// </summary>
        public string opt_spdise_organ_transplant_asst_medn_balc { get; set; }
        /// <summary>
        /// 门诊大病恶性肿瘤放化疗余额
        /// </summary>
        public string opt_big_dise_tmor_chmo_balc { get; set; }
        /// <summary>
        /// 门诊大病恶性肿瘤针对性药物余额
        /// </summary>
        public string opt_big_dise_tmor_medn_balc { get; set; }
        /// <summary>
        /// 门诊大病恶性肿瘤辅助药物余额
        /// </summary>
        public string opt_big_dise_tomr_asst_medn_balc { get; set; }
        /// <summary>
        /// 门诊大病血腹透余额
        /// </summary>
        public string opt_big_dise_blo_abd_diay_medn_balc { get; set; }
        /// <summary>
        /// 门诊大病血腹透辅助药物余额
        /// </summary>
        public string opt_big_dise_blo_abd_diay_asst_balc { get; set; }
        /// <summary>
        /// 门诊大病器官移植余额
        /// </summary>
        public string opt_big_dise_organ_transplant_medn_balc { get; set; }
        /// <summary>
        /// 门诊大病器官移植辅助药物余额
        /// </summary>
        public string opt_big_dise_organ_transplant_asst_medn_balc { get; set; }
        /// <summary>
        /// 不享受待遇原因
        /// </summary>
        public string trt_chk_rslt { get; set; }
        /// <summary>
        /// 在院状态
        /// </summary>
        public string inhosp_stas { get; set; }

    }



    public class RT1101NEW
    {
        /// <summary>
        /// 输出-基本信息（节点标识：baseinfo）
        /// </summary>
        public class Baseinfo
        {
            public string psn_no { get; set; }//人员编号
            public string psn_cert_type { get; set; }// 人员证件类型
            public string certno { get; set; }// 证件号码
            public string psn_name { get; set; }// 人员姓名
            public string gend { get; set; }//性别
            public string naty { get; set; }// 民族
            public string brdy { get; set; }// 出生日期
            public string age { get; set; }//年龄
            public string exp_content { get; set; }
        }
        /// <summary>
        /// 输出-参保信息列表（节点标识insuinfo）
        /// </summary>
        public class Insuinfo
        {
            public string balc { get; set; }//余额
            public string insutype { get; set; }// 险种类型
            public string psn_type { get; set; }//人员类别
            public string cvlserv_flag { get; set; }//公务员标志
            public string insuplc_admdvs { get; set; }//参保地医保区划
            public string emp_name { get; set; }//单位名称

            public string psn_insu_stas { get; set; }//人员参保状态

            public string psn_insu_date { get; set; }//个人参保日期

            public string paus_insu_date { get; set; }//暂停参保日期

        }

        /// <summary>
        /// 输出-身份信息列表（节点标识：idetinfo）
        /// </summary>
        public class Idetinfo
        {
            public string psn_idet_type { get; set; }//人员身份类别
            public string psn_type_lv { get; set; }//人员类别等级
            public string memo { get; set; }//备注
            public string begntime { get; set; }//开始时间
            public string endtime { get; set; }//结束时间

        }
        public class Output
        {
            /// <summary>
            /// 
            /// </summary>
            public Baseinfo baseinfo { get; set; }

            public List<Insuinfo> insuinfo { get; set; }

            public List<Idetinfo> idetinfo { get; set; }
        }
        public class Root
        {
            /// <summary>
            /// 交易状态码
            /// </summary>
            public string infcode { get; set; }
            /// <summary>
            /// 接收方报文ID
            /// </summary>
            public string inf_refmsgid { get; set; }
            /// <summary>
            /// 接收报文时间
            /// </summary>
            public string refmsg_time { get; set; }
            /// <summary>
            /// 响应报文时间
            /// </summary>
            public string respond_time { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string err_msg { get; set; }
            public Output output { get; set; }
        }
    }


    #endregion
}
