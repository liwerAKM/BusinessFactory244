using System;
namespace Plat.Model
{
    /// <summary>
    /// register_wait:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class register_wait
    {
        public register_wait()
        { }
        #region Model
        private string _wait_id;
        private int _regpat_id;
        private string _hos_id;
        private string _dept_code;
        private string _doc_no;
        private int _pat_id;
        private DateTime? _save_time;
        private bool _is_cancel;
        private DateTime? _cancel_time;
        private bool _is_add = false;
        private string _sch_date;
        private string _sch_time;
        private DateTime? _sch_addtimr;
        private DateTime? _exp_time;
        private bool _is_appt = false;
        private int? _reg_id;
        private DateTime? _app_time;
        private bool _is_default = false;
        private string _lterminal_sn;
        /// <summary>
        /// 
        /// </summary>
        public string WAIT_ID
        {
            set { _wait_id = value; }
            get { return _wait_id; }
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
        public string HOS_ID
        {
            set { _hos_id = value; }
            get { return _hos_id; }
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
        public int PAT_ID
        {
            set { _pat_id = value; }
            get { return _pat_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SAVE_TIME
        {
            set { _save_time = value; }
            get { return _save_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IS_CANCEL
        {
            set { _is_cancel = value; }
            get { return _is_cancel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CANCEL_TIME
        {
            set { _cancel_time = value; }
            get { return _cancel_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IS_ADD
        {
            set { _is_add = value; }
            get { return _is_add; }
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
        public DateTime? SCH_ADDTIMR
        {
            set { _sch_addtimr = value; }
            get { return _sch_addtimr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EXP_TIME
        {
            set { _exp_time = value; }
            get { return _exp_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IS_APPT
        {
            set { _is_appt = value; }
            get { return _is_appt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? REG_ID
        {
            set { _reg_id = value; }
            get { return _reg_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? APP_TIME
        {
            set { _app_time = value; }
            get { return _app_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IS_DEFAULT
        {
            set { _is_default = value; }
            get { return _is_default; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string lTERMINAL_SN
        {
            set { _lterminal_sn = value; }
            get { return _lterminal_sn; }
        }
        #endregion Model

    }
}

