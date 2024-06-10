using System;
namespace Plat.Model
{
    /// <summary>
    /// register_pay:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class register_pay
    {
        public register_pay()
        { }
        #region Model
        private int _pay_id;
        private int _reg_id;
        private string _hos_id;
        private int _pat_id;
        private int _regpat_id;
        private string _opt_sn;
        private string _hos_sn;
        private string _sfz_no;
        private int _ylcard_type;
        private string _ylcard_no;
        private string _pat_name;
        private string _sex;
        private string _dept_code;
        private string _birthday;
        private string _dept_name;
        private string _doc_no;
        private string _doc_name;
        private string _dis_name;
        private string _gh_type;
        private string _hos_gh_type;
        private string _hos_gh_name;
        private decimal _zl_fee;
        private decimal _gh_fee;
        private decimal _cash_je;
        private string _jz_code;
        private int _pay_type;
        private string _yb_sn;
        private string _yb_type;
        private string _yb_fylb;
        private string _yb_gh_order;
        private decimal _yb_zlfee = 0.00M;
        private decimal _yb_ghfee = 0.00M;
        private decimal _xjzf = 0.00M;
        private decimal _zhzf = 0.00M;
        private decimal? _zhye = 0.00M;
        private string _xzm;
        private string _xzmch;
        private string _ybbzm;
        private string _ybbzmc;
        private decimal _je_all = 0.00M;
        private string _appt_order;
        private string _appt_sn;
        private bool _is_dz = false;
        private DateTime _dj_date;
        private string _dj_time;
        private bool _is_th;
        private DateTime? _rh_date;
        private string _th_time;
        /// <summary>
        /// 支付标示
        /// </summary>
        public int PAY_ID
        {
            set { _pay_id = value; }
            get { return _pay_id; }
        }
        /// <summary>
        /// 预约标识
        /// </summary>
        public int REG_ID
        {
            set { _reg_id = value; }
            get { return _reg_id; }
        }
        /// <summary>
        /// 医院代码
        /// </summary>
        public string HOS_ID
        {
            set { _hos_id = value; }
            get { return _hos_id; }
        }
        /// <summary>
        /// 持卡人唯一索引
        /// </summary>
        public int PAT_ID
        {
            set { _pat_id = value; }
            get { return _pat_id; }
        }
        /// <summary>
        /// 用户唯一索引
        /// </summary>
        public int REGPAT_ID
        {
            set { _regpat_id = value; }
            get { return _regpat_id; }
        }
        /// <summary>
        /// 病人门诊号
        /// </summary>
        public string OPT_SN
        {
            set { _opt_sn = value; }
            get { return _opt_sn; }
        }
        /// <summary>
        /// 院内唯一流水号
        /// </summary>
        public string HOS_SN
        {
            set { _hos_sn = value; }
            get { return _hos_sn; }
        }
        /// <summary>
        /// 病人身份证
        /// </summary>
        public string SFZ_NO
        {
            set { _sfz_no = value; }
            get { return _sfz_no; }
        }
        /// <summary>
        /// 医疗卡类别
        /// </summary>
        public int YLCARD_TYPE
        {
            set { _ylcard_type = value; }
            get { return _ylcard_type; }
        }
        /// <summary>
        /// 医疗卡卡号
        /// </summary>
        public string YLCARD_NO
        {
            set { _ylcard_no = value; }
            get { return _ylcard_no; }
        }
        /// <summary>
        /// 病人姓名
        /// </summary>
        public string PAT_NAME
        {
            set { _pat_name = value; }
            get { return _pat_name; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string SEX
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 科室代码
        /// </summary>
        public string DEPT_CODE
        {
            set { _dept_code = value; }
            get { return _dept_code; }
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string BIRTHDAY
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 科室名称
        /// </summary>
        public string DEPT_NAME
        {
            set { _dept_name = value; }
            get { return _dept_name; }
        }
        /// <summary>
        /// 专家代码
        /// </summary>
        public string DOC_NO
        {
            set { _doc_no = value; }
            get { return _doc_no; }
        }
        /// <summary>
        /// 专家名称
        /// </summary>
        public string DOC_NAME
        {
            set { _doc_name = value; }
            get { return _doc_name; }
        }
        /// <summary>
        /// 专病名称
        /// </summary>
        public string DIS_NAME
        {
            set { _dis_name = value; }
            get { return _dis_name; }
        }
        /// <summary>
        /// 挂号类别
        /// </summary>
        public string GH_TYPE
        {
            set { _gh_type = value; }
            get { return _gh_type; }
        }
        /// <summary>
        /// 院内挂号类别ID
        /// </summary>
        public string HOS_GH_TYPE
        {
            set { _hos_gh_type = value; }
            get { return _hos_gh_type; }
        }
        /// <summary>
        /// 院内挂号类别名
        /// </summary>
        public string HOS_GH_NAME
        {
            set { _hos_gh_name = value; }
            get { return _hos_gh_name; }
        }
        /// <summary>
        /// 院内诊疗费
        /// </summary>
        public decimal ZL_FEE
        {
            set { _zl_fee = value; }
            get { return _zl_fee; }
        }
        /// <summary>
        /// 院内挂号费
        /// </summary>
        public decimal GH_FEE
        {
            set { _gh_fee = value; }
            get { return _gh_fee; }
        }
        /// <summary>
        /// 本次挂号现金支付金额
        /// </summary>
        public decimal CASH_JE
        {
            set { _cash_je = value; }
            get { return _cash_je; }
        }
        /// <summary>
        /// 人员费用结算类别编码必填
        /// </summary>
        public string JZ_CODE
        {
            set { _jz_code = value; }
            get { return _jz_code; }
        }
        /// <summary>
        /// 现金支付方式
        /// </summary>
        public int PAY_TYPE
        {
            set { _pay_type = value; }
            get { return _pay_type; }
        }
        /// <summary>
        /// 医保单据号
        /// </summary>
        public string YB_SN
        {
            set { _yb_sn = value; }
            get { return _yb_sn; }
        }
        /// <summary>
        /// 医保类别
        /// </summary>
        public string YB_TYPE
        {
            set { _yb_type = value; }
            get { return _yb_type; }
        }
        /// <summary>
        /// 医保费用类别
        /// </summary>
        public string YB_FYLB
        {
            set { _yb_fylb = value; }
            get { return _yb_fylb; }
        }
        /// <summary>
        /// 医保挂号序号
        /// </summary>
        public string YB_GH_ORDER
        {
            set { _yb_gh_order = value; }
            get { return _yb_gh_order; }
        }
        /// <summary>
        /// 医保诊疗费支付金额（HIS产生）
        /// </summary>
        public decimal YB_ZLFEE
        {
            set { _yb_zlfee = value; }
            get { return _yb_zlfee; }
        }
        /// <summary>
        /// 医保挂号费支付金额（HIS产生）
        /// </summary>
        public decimal YB_GHFEE
        {
            set { _yb_ghfee = value; }
            get { return _yb_ghfee; }
        }
        /// <summary>
        /// 医保现金支付
        /// </summary>
        public decimal XJZF
        {
            set { _xjzf = value; }
            get { return _xjzf; }
        }
        /// <summary>
        /// 医保个人账户支付
        /// </summary>
        public decimal ZHZF
        {
            set { _zhzf = value; }
            get { return _zhzf; }
        }
        /// <summary>
        /// 医保个人账户余额
        /// </summary>
        public decimal? ZHYE
        {
            set { _zhye = value; }
            get { return _zhye; }
        }
        /// <summary>
        /// 医保险种码
        /// </summary>
        public string XZM
        {
            set { _xzm = value; }
            get { return _xzm; }
        }
        /// <summary>
        /// 医保险种名称
        /// </summary>
        public string XZMCH
        {
            set { _xzmch = value; }
            get { return _xzmch; }
        }
        /// <summary>
        /// 医保病种码
        /// </summary>
        public string YBBZM
        {
            set { _ybbzm = value; }
            get { return _ybbzm; }
        }
        /// <summary>
        /// 医保病种名称
        /// </summary>
        public string YBBZMC
        {
            set { _ybbzmc = value; }
            get { return _ybbzmc; }
        }
        /// <summary>
        /// 挂号成功支付总金额
        /// </summary>
        public decimal JE_ALL
        {
            set { _je_all = value; }
            get { return _je_all; }
        }
        /// <summary>
        /// 预约序号
        /// </summary>
        public string APPT_ORDER
        {
            set { _appt_order = value; }
            get { return _appt_order; }
        }
        /// <summary>
        /// 预约流水号
        /// </summary>
        public string APPT_SN
        {
            set { _appt_sn = value; }
            get { return _appt_sn; }
        }
        /// <summary>
        /// 初复诊标识
        /// </summary>
        public bool IS_DZ
        {
            set { _is_dz = value; }
            get { return _is_dz; }
        }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DJ_DATE
        {
            set { _dj_date = value; }
            get { return _dj_date; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public string DJ_TIME
        {
            set { _dj_time = value; }
            get { return _dj_time; }
        }
        /// <summary>
        /// 是否退号
        /// </summary>
        public bool IS_TH
        {
            set { _is_th = value; }
            get { return _is_th; }
        }
        /// <summary>
        /// 退号日期
        /// </summary>
        public DateTime? RH_DATE
        {
            set { _rh_date = value; }
            get { return _rh_date; }
        }
        /// <summary>
        /// 退号时间
        /// </summary>
        public string TH_TIME
        {
            set { _th_time = value; }
            get { return _th_time; }
        }
        public string SOURCE { get; set; }
        public string USER_ID { get; set; }
        public string lTERMINAL_SN { get; set; }
        #endregion Model

    }
}

