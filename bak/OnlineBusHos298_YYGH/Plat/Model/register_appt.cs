using System;
namespace Plat.Model
{
    /// <summary>
    /// register_appt:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class register_appt
    {
        public register_appt()
        { }
        #region Model
        private int _reg_id;
        private string _hos_id;
        private int _pat_id;
        private string _sch_date;
        private string _sch_time;
        private string _dept_code;
        private string _doc_no;
        private int _regpat_id;
        private string _time_bucket;
        private string _time_point;
        private string _sfz_no;
        private string _pat_name;
        private string _birthday;
        private string _sex;
        private int _ylcard_type;
        private string _ylcard_no;
        private string _dept_name;
        private string _doc_name;
        private string _dis_name;
        private string _gh_type;
        private string _hos_sn;
        private string _hos_fh_type;
        private decimal _zl_fee;
        private decimal _gh_fee;
        private string _appt_type;
        private string _appt_order;
        private string _pay_status;
        private string _appt_sn;
        private decimal _appt_pay = 0.00M;
        private bool _is_fz = false;
        private DateTime _appt_tate;
        private string _appt_time;
        private DateTime? _pay_expiration;
        private DateTime _appt_expiration;
        private string _lterminal_sn;
        private string _appt_way;
        private string _period_start;
        private bool _in_pay_state;
        /// <summary>
        /// 
        /// </summary>
        public int REG_ID
        {
            set { _reg_id = value; }
            get { return _reg_id; }
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
        public string SCH_DATE
        {
            set { _sch_date = value; }
            get { return _sch_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SCH_TIME
        {
            set { _sch_time = value; }
            get { return _sch_time; }
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
        public string DOC_NO
        {
            set { _doc_no = value; }
            get { return _doc_no; }
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
        public string TIME_BUCKET
        {
            set { _time_bucket = value; }
            get { return _time_bucket; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TIME_POINT
        {
            set { _time_point = value; }
            get { return _time_point; }
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
        public string PAT_NAME
        {
            set { _pat_name = value; }
            get { return _pat_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BIRTHDAY
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SEX
        {
            set { _sex = value; }
            get { return _sex; }
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
        public string DEPT_NAME
        {
            set { _dept_name = value; }
            get { return _dept_name; }
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
        public string GH_TYPE
        {
            set { _gh_type = value; }
            get { return _gh_type; }
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
        public string HOS_FH_TYPE
        {
            set { _hos_fh_type = value; }
            get { return _hos_fh_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal ZL_FEE
        {
            set { _zl_fee = value; }
            get { return _zl_fee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal GH_FEE
        {
            set { _gh_fee = value; }
            get { return _gh_fee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APPT_TYPE
        {
            set { _appt_type = value; }
            get { return _appt_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APPT_ORDER
        {
            set { _appt_order = value; }
            get { return _appt_order; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PAY_STATUS
        {
            set { _pay_status = value; }
            get { return _pay_status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APPT_SN
        {
            set { _appt_sn = value; }
            get { return _appt_sn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal APPT_PAY
        {
            set { _appt_pay = value; }
            get { return _appt_pay; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IS_FZ
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime APPT_TATE
        {
            set { _appt_tate = value; }
            get { return _appt_tate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APPT_TIME
        {
            set { _appt_time = value; }
            get { return _appt_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? PAY_EXPIRATION
        {
            set { _pay_expiration = value; }
            get { return _pay_expiration; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime APPT_EXPIRATION
        {
            set { _appt_expiration = value; }
            get { return _appt_expiration; }
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
        /// 
        /// </summary>
        public string APPT_WAY
        {
            set { _appt_way = value; }
            get { return _appt_way; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PERIOD_START
        {
            set { _period_start = value; }
            get { return _period_start; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IN_PAY_STATE
        {
            set { _in_pay_state = value; }
            get { return _in_pay_state; }
        }

        public string SOURCE { get; set; }
        public string USER_ID { get; set; }
        public string HOS_SN_HIS { get; set; }
        #endregion Model

    }
}

