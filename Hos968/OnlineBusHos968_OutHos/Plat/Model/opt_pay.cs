using System;
namespace Plat.Model
{
    /// <summary>
    /// opt_pay:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class opt_pay
    {
        public opt_pay()
        { }
        #region Model
        private string _pay_id;
        private string _hos_id;
        private int _pat_id;
        private int _regpat_id;
        private string _hos_sn;
        private string _opt_sn;
        private string _hos_reg_sn;
        private string _pre_no;
        private string _sfz_no;
        private int _ylcard_type;
        private string _ylcard_no;
        private string _pat_name;
        private string _dept_code;
        private string _dept_name;
        private string _doc_no;
        private string _doc_name;
        private string _dis_name;
        private string _pay_lterminal_sn;
        private decimal _cash_je;
        private int _pay_type;
        private decimal _jeall = 0.00M;
        private string _jz_code;
        private string _ybdjh;
        private decimal _grzl = 0.00M;
        private decimal _grzf = 0.00M;
        private decimal _tczf = 0.00M;
        private decimal _dbzf = 0.00M;
        private decimal _xjzf = 0.00M;
        private decimal _zhzf = 0.00M;
        private decimal _hm = 0.00M;
        private decimal _cs = 0.00M;
        private decimal _zfy = 0.00M;
        private decimal _yf = 0.00M;
        private decimal _xmfy = 0.00M;
        private decimal _lcl = 0.00M;
        private decimal? _zhye = 0.00M;
        private string _xzm;
        private string _xzmch;
        private string _man_type;
        private string _bzfyy;
        private string _fylb;
        private string _ybbzm;
        private string _ybbzmc;
        private DateTime _dj_date;
        private string _dj_time;
        private string _rcpt_no;
        private string _hos_pay_sn;
        private bool? _is_tz;
        private DateTime? _tz_date;
        private string _tz_time;
        private string _pay_id_in;
        private string _lterminal_sn;
        /// <summary>
        /// 
        /// </summary>
        public string PAY_ID
        {
            set { _pay_id = value; }
            get { return _pay_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HOS_ID
        {
            set { _hos_id = value; }
            get { return _hos_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PAT_ID
        {
            set { _pat_id = value; }
            get { return _pat_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int REGPAT_ID
        {
            set { _regpat_id = value; }
            get { return _regpat_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HOS_SN
        {
            set { _hos_sn = value; }
            get { return _hos_sn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OPT_SN
        {
            set { _opt_sn = value; }
            get { return _opt_sn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HOS_REG_SN
        {
            set { _hos_reg_sn = value; }
            get { return _hos_reg_sn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PRE_NO
        {
            set { _pre_no = value; }
            get { return _pre_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SFZ_NO
        {
            set { _sfz_no = value; }
            get { return _sfz_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int YLCARD_TYPE
        {
            set { _ylcard_type = value; }
            get { return _ylcard_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YLCARD_NO
        {
            set { _ylcard_no = value; }
            get { return _ylcard_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PAT_NAME
        {
            set { _pat_name = value; }
            get { return _pat_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_CODE
        {
            set { _dept_code = value; }
            get { return _dept_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_NAME
        {
            set { _dept_name = value; }
            get { return _dept_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOC_NO
        {
            set { _doc_no = value; }
            get { return _doc_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOC_NAME
        {
            set { _doc_name = value; }
            get { return _doc_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DIS_NAME
        {
            set { _dis_name = value; }
            get { return _dis_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PAY_lTERMINAL_SN
        {
            set { _pay_lterminal_sn = value; }
            get { return _pay_lterminal_sn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal CASH_JE
        {
            set { _cash_je = value; }
            get { return _cash_je; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PAY_TYPE
        {
            set { _pay_type = value; }
            get { return _pay_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal JEALL
        {
            set { _jeall = value; }
            get { return _jeall; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string JZ_CODE
        {
            set { _jz_code = value; }
            get { return _jz_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ybDJH
        {
            set { _ybdjh = value; }
            get { return _ybdjh; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal GRZL
        {
            set { _grzl = value; }
            get { return _grzl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal GRZF
        {
            set { _grzf = value; }
            get { return _grzf; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal TCZF
        {
            set { _tczf = value; }
            get { return _tczf; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal DBZF
        {
            set { _dbzf = value; }
            get { return _dbzf; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal XJZF
        {
            set { _xjzf = value; }
            get { return _xjzf; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal ZHZF
        {
            set { _zhzf = value; }
            get { return _zhzf; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal HM
        {
            set { _hm = value; }
            get { return _hm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal CS
        {
            set { _cs = value; }
            get { return _cs; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal ZFY
        {
            set { _zfy = value; }
            get { return _zfy; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal YF
        {
            set { _yf = value; }
            get { return _yf; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal XMFY
        {
            set { _xmfy = value; }
            get { return _xmfy; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal LCL
        {
            set { _lcl = value; }
            get { return _lcl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ZHYE
        {
            set { _zhye = value; }
            get { return _zhye; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string XZM
        {
            set { _xzm = value; }
            get { return _xzm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string XZMCH
        {
            set { _xzmch = value; }
            get { return _xzmch; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string man_type
        {
            set { _man_type = value; }
            get { return _man_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BZFYY
        {
            set { _bzfyy = value; }
            get { return _bzfyy; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FYLB
        {
            set { _fylb = value; }
            get { return _fylb; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YBBZM
        {
            set { _ybbzm = value; }
            get { return _ybbzm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string YBBZMC
        {
            set { _ybbzmc = value; }
            get { return _ybbzmc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DJ_DATE
        {
            set { _dj_date = value; }
            get { return _dj_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DJ_TIME
        {
            set { _dj_time = value; }
            get { return _dj_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RCPT_NO
        {
            set { _rcpt_no = value; }
            get { return _rcpt_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HOS_PAY_SN
        {
            set { _hos_pay_sn = value; }
            get { return _hos_pay_sn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? IS_TZ
        {
            set { _is_tz = value; }
            get { return _is_tz; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? TZ_DATE
        {
            set { _tz_date = value; }
            get { return _tz_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TZ_TIME
        {
            set { _tz_time = value; }
            get { return _tz_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PAY_ID_IN
        {
            set { _pay_id_in = value; }
            get { return _pay_id_in; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string lTERMINAL_SN
        {
            set { _lterminal_sn = value; }
            get { return _lterminal_sn; }
        }
        /// <summary>
        /// 锁定记录表的PAY_ID
        /// </summary>
        public string LockPAY_ID
        {
            get;
            set;
        }
        public string SOURCE { get; set; }
        public string USER_ID { get; set; }
        #endregion Model

    }
}

