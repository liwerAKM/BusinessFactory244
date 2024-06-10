﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZZJ_Tran.Model
{
    class GETQRCODE_M
    {
        public class GETQRCODE_IN
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
            /// 现金金额
            /// </summary>
            public string CASH_JE { get; set; }
            /// <summary>
            /// 支付方式
            /// </summary>
            public string DEAL_TYPE { get; set; }
            /// <summary>
            /// 0挂号 1门诊缴费 2病历本  4预交金  5出院结算
            /// </summary>
            public string TYPE { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            public string PAT_NAME { get; set; }
            /// <summary>
            /// 身份证号
            /// </summary>
            public string SFZ_NO { get; set; }
            /// <summary>
            /// 患者院内唯一索引
            /// </summary>
            public string HOSPATID { get; set; }

            /// <summary>
            /// 有效时间
            /// </summary>
            public string TIME_EXPIRE { get; set; }
            /// <summary>
            /// 数据来源
            /// </summary>
            public string SOURCE { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string FILTER { get; set; }
        }

        public class GETQRCODE_OUT
        {
            /// <summary>
            /// 二维码地址
            /// </summary>
            public string QRCODE{ get; set; }
            /// <summary>
            /// 交易流水号
            /// </summary>
            public string QUERYID { get; set; }

            /// <summary>
            /// HIS交易出参
            /// </summary>
            public string HIS_RTNXML { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string PARAMETERS { get; set; }

        }
    }
}
