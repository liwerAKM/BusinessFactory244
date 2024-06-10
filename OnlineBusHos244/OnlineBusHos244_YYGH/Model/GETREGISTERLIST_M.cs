using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos244_YYGH.Model
{
    class GETREGISTERLIST_M
    {
        public class GETREGISTERLIST_IN
        {
            /// <summary>
            /// 医院ID
            /// </summary>
            public string HOS_ID { get; set; }
            /// <summary>
            /// 操作员唯一ID
            /// </summary>
            public string USER_ID { get; set; }
            /// <summary>
            /// 自助终端编号
            /// </summary>
            public string LTERMINAL_SN { get; set; }
            /// <summary>
            /// 患者在院唯一id
            /// </summary>
            public string HOSPATID { get; set; }
            /// <summary>
            /// 医疗卡类型
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

            public string FILTER { get; set; }

            public string YY_TYPE { get; set; }
        }

        public class AppTist
        {
            /// <summary>
            /// 院内挂号唯一流水号
            /// </summary>
            public string HOS_SN { get; set; }
            /// <summary>
            /// 支付总金额
            /// </summary>
            public string APPT_PAY { get; set; }
            public string APPT_TYPE { get; set; }
            public string APPT_ORDER { get; set; }
            public string OPT_SN { get; set; }
            public string PRO_TITLE { get; set; }
            public string USER_ID { get; set; }
            public string YB_INFO { get; set; }
            /// <summary>
            /// 总金额
            /// </summary>
            public string JEALL { get; set; }
            /// <summary>
            /// 预约序号
            /// </summary>
            public string APPT_ORDERP { get; set; }
            /// <summary>
            /// 预约时间
            /// </summary>
            public string APPT_TIME { get; set; }
            /// <summary>
            /// 预约就诊地点
            /// </summary>
            public string APPT_PLACE { get; set; }
            /// <summary>
            /// 医疗卡类型
            /// </summary>
            public string YLCARD_TYPE { get; set; }
            /// <summary>
            /// 科室id
            /// </summary>
            public string DEPT_CODE { get; set; }
            /// <summary>
            /// 科室名称
            /// </summary>
            public string DEPT_NAME { get; set; }
            /// <summary>
            /// 科室简介
            /// </summary>
            public string DEPT_INTRO { get; set; }
            /// <summary>
            /// 科室序号
            /// </summary>
            public string DEPT_ORDER { get; set; }
            /// <summary>
            /// 科室类型
            /// </summary>
            public string DEPT_TYPE { get; set; }
            /// <summary>
            /// 科室地址
            /// </summary>
            public string DEPT_ADDRESS { get; set; }
            /// <summary>
            /// 医生id
            /// </summary>
            public string DOC_NO { get; set; }
            /// <summary>
            /// 医生姓名
            /// </summary>
            public string DOC_NAME { get; set; }
            /// <summary>
            /// 挂号费
            /// </summary>
            public string GH_FEE { get; set; }
            /// <summary>
            /// 诊疗费
            /// </summary>
            public string ZL_FEE { get; set; }
            /// <summary>
            /// 挂号总费用
            /// </summary>
            public string ALL_FEE { get; set; }
            /// <summary>
            /// 排班类型
            /// </summary>
            public string SCH_TYPE { get; set; }
            /// <summary>
            /// 排班日期
            /// </summary>
            public string SCH_DATE { get; set; }
            /// <summary>
            /// 排班时间
            /// </summary>
            public string SCH_TIME { get; set; }
            /// <summary>
            /// 排班开始时间段
            /// </summary>
            public string PERIOD_START { get; set; }
            /// <summary>
            /// /排班结束时间段
            /// </summary>
            public string PERIOD_END { get; set; }
            /// <summary>
            /// 号别
            /// </summary>
            public string REGISTER_TYPE { get; set; }
            /// <summary>
            /// 号别名称
            /// </summary>
            public string REGISTER_TYPE_NAME { get; set; }
            /// <summary>
            /// 排班明细id
            /// </summary>
            public string PBMXXH { get; set; }
            /// <summary>
            /// 预约号序
            /// </summary>
            public string YYHX { get; set; }
        }

        public class GETREGISTERLIST_OUT
        {
            public List<AppTist> APPTLIST { get; set; }
            public string HIS_RTNXML { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string PARAMETERS { get; set; }
        }
    }
}
