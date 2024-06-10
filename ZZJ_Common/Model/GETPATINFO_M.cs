﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZZJ_Common.Model
{
    class GETPATINFO_M
    {
        public class GETPATINFO_IN
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
            /// 身份证号
            /// </summary>
            public string SFZ_NO { get; set; }
            /// <summary>
            /// 医疗卡类型
            /// </summary>
            public string YLCARD_TYPE { get; set; }
            /// <summary>
            /// 医疗卡号
            /// </summary>
            public string YLCARD_NO { get; set; }

            /// <summary>
            /// 数据来源
            /// </summary>
            public string SOURCE { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string FILTER { get; set; }
        }


        public class GETPATINFO_OUT
        {
            /// <summary>
            /// 姓名
            /// </summary>
            public string PAT_NAME { get; set; }
            /// <summary>
            /// 性别
            /// </summary>
            public string SEX { get; set; }
            /// <summary>
            /// 年龄
            /// </summary>
            public string AGE { get; set; }
            /// <summary>
            /// 联系方式
            /// </summary>
            public string MOBILE_NO { get; set; }
            /// <summary>
            /// 住址
            /// </summary>
            public string ADDRESS { get; set; }
            /// <summary>
            /// 身份证号
            /// </summary>
            public string SFZ_NO { get; set; }

            /// <summary>
            /// 出生日期
            /// </summary>
            public string BIR_DATE { get; set; }
            /// <summary>
            /// 监护人姓名
            /// </summary>
            public string GUARDIAN_NAME { get; set; }

            /// <summary>
            /// 院内号
            /// </summary>
            public string HOSPATID { get; set; }
            /// <summary>
            /// 是否存在
            /// </summary>
            public string IS_EXIST { get; set; }
            public string HIS_RTNXML { get; set; }

            /// <summary>
            /// 其他条件
            /// </summary>
            public string PARAMETERS { get; set; }

        }
    }
}
