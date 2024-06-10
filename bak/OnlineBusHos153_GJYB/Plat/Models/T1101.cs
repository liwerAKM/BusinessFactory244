using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos153_GJYB.Models
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

            public string expContent { get; set; }
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
    #endregion
}
