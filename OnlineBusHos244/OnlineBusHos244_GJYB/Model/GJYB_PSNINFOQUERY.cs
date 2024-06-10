using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos244_GJYB.Model
{
    class GJYB_PSNINFOQUERY_M
    {
        public class GJYB_PSNINFOQUERY_IN
        {
            /// <summary>
            /// 医院ID
            /// </summary>
            public string HOS_ID { get; set; }
            /// <summary>
            /// 操作员ID
            /// </summary>
            public string USER_ID { get; set; }
            /// <summary>
            /// 自助机终端号
            /// </summary>
            public string LTERMINAL_SN { get; set; }
            /// <summary>
            /// 医疗类型
            /// </summary>
            public string YLCARD_TYPE { get; set; }
            /// <summary>
            /// 医疗卡号
            /// </summary>
            public string YLCARD_NO { get; set; }
            /// <summary>
            /// 身份证号
            /// </summary>
            public string SFZ_NO { get; set; }
            /// <summary>
            /// 来源
            /// </summary>
            public string SOURCE { get; set; }
            /// <summary>
            /// 就诊凭证类型
            /// </summary>
            public string MDTRT_CERT_TYPE { get; set; }
            /// <summary>
            /// 就诊凭证编号
            /// </summary>
            public string MDTRT_CERT_NO { get; set; }
            /// <summary>
            /// 卡识别码
            /// </summary>
            public string CARD_SN { get; set; }
            /// <summary>
            /// 开始时间
            /// </summary>
            public string BEGNTIME { get; set; }
            /// <summary>
            /// 证件号码
            /// </summary>
            public string CERTNO { get; set; }
            /// <summary>
            /// 人员证件类型
            /// </summary>
            public string PSN_CERT_TYPE { get; set; }
            /// <summary>
            /// 人员姓名
            /// </summary>
            public string PSN_NAME { get; set; }
            public string FILTER { get; set; }

        }

        public class GJYB_PSNINFOQUERY_OUT
        {
            /// <summary>
            /// 社保卡号
            /// </summary>
            public string CARD_NO { get; set; }
            /// <summary>
            /// 身份证号
            /// </summary>
            public string CERTNO { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string PSN_NAME { get; set; }
            /// <summary>
            /// 性别
            /// </summary>
            public string GEND { get; set; }
            /// <summary>
            /// 民族
            /// </summary>
            public string NATY { get; set; }
            /// <summary>
            /// 出生日期
            /// </summary>
            public string BRDY { get; set; }
            /// <summary>
            /// 医疗人员类型
            /// </summary>
            public string PSN_TYPE { get; set; }
            /// <summary>
            /// 公务员标志
            /// </summary>
            public string CVLSERV_FLAG { get; set; }
            /// <summary>
            /// 医保状态
            /// </summary>
            public string PSN_INSU_STAS { get; set; }
            /// <summary>
            /// 医疗类别
            /// </summary>
            public string MED_TYPE { get; set; }
            /// <summary>
            /// 病种编码
            /// </summary>
            public string DISE_CODG { get; set; }
            /// <summary>
            /// 病种名称
            /// </summary>
            public string   DISE_NAME{ get; set; }
            /// <summary>
            /// 账户使用标志
            /// </summary>
            public string ACCT_USED_FLAG { get; set; }
            /// <summary>
            /// 结算方式
            /// </summary>
            public string PSN_SETLWAY { get; set; }
            /// <summary>
            /// 医保账户余额
            /// </summary>
            public string BALC { get; set; }
            /// <summary>
            /// 人员编号
            /// </summary>
            public string PSN_NO { get; set; }
            /// <summary>
            /// 险种类型
            /// </summary>
            public string INSUTYPE { get; set; }
            /// <summary>
            /// 参保日期
            /// </summary>
            public string PSN_INSU_DATE { get; set; }
            /// <summary>
            /// 暂停参保日期
            /// </summary>
            public string PAUS_INSU_DATE { get; set; }
            /// <summary>
            /// 年度累计
            /// </summary>
            public string CUM { get; set; }
            /// <summary>
            /// 在院状态
            /// </summary>
            public string INHOSP_STAS { get; set; }
            /// <summary>
            /// 门慢剩余金额
            /// </summary>
            public string OPSP_BALC { get; set; }
            /// <summary>
            /// 门统剩余金额
            /// </summary>
            public string OPT_POOL_BALC { get; set; }
            /// <summary>
            /// 门特辅助检查和用药余额
            /// </summary>
            public string OPT_SPDISE_CONTENT { get; set; }
            /// <summary>
            /// 门诊统筹不支付原因
            /// </summary>
            public string TRT_CHK_RSLT { get; set; }
            /// <summary>
            /// HIS参数
            /// </summary>
            public string HIS_RTNXML { get; set; }
            /// <summary>
            /// 其他参数
            /// </summary>
            public string PARAMETER { get; set; }

        }
    }
}
