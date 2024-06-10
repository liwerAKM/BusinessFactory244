using System;
namespace Plat.Model
{
    /// <summary>
    /// schedule:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class schedule
    {
        public schedule()
        { }
        #region Model
        private string _hos_id;
        private string _dept_code;
        private string _doc_no;
        private string _sch_date;
        private string _sch_time;
        private int _sch_type;
        private int _count_all = 0;
        private int _count_rem = 0;
        private decimal _gh_fee = 0.00M;
        private decimal _zl_fee = 0.00M;
        private string _start_time;
        private string _end_time;
        private int _count_esb;
        private int _count_pay = 0;
        private int _count_def = 0;
        private DateTime? _wait_open_time;
        private string _pro_title;
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
        public int SCH_TYPE
        {
            set { _sch_type = value; }
            get { return _sch_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int COUNT_ALL
        {
            set { _count_all = value; }
            get { return _count_all; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int COUNT_REM
        {
            set { _count_rem = value; }
            get { return _count_rem; }
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
        public decimal ZL_FEE
        {
            set { _zl_fee = value; }
            get { return _zl_fee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string START_TIME
        {
            set { _start_time = value; }
            get { return _start_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string END_TIME
        {
            set { _end_time = value; }
            get { return _end_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int COUNT_ESB
        {
            set { _count_esb = value; }
            get { return _count_esb; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int COUNT_PAY
        {
            set { _count_pay = value; }
            get { return _count_pay; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int COUNT_DEF
        {
            set { _count_def = value; }
            get { return _count_def; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? WAIT_OPEN_TIME
        {
            set { _wait_open_time = value; }
            get { return _wait_open_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PRO_TITLE
        {
            set { _pro_title = value; }
            get { return _pro_title; }
        }
        public int OPERA_TYPE
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool CAN_WAIT
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string REGISTER_TYPE
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public string YB_CODE
        {
            get;
            set;
        }
        #endregion Model

    }
}

