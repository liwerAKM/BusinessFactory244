using System;
namespace Plat.Model
{
    /// <summary>
    /// dept_info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class dept_info
    {
        public dept_info()
        { }
        #region Model
        private string _hos_id;
        private string _dept_code;
        private string _dept_code_esb;
        private string _dept_name;
        private string _dept_intro;
        private string _dept_url;
        private string _dept_tel;
        private string _dept_sup_code;
        private string _dept_type;
        private string _dept_order;
        private string _dept_address;
        private string _dept_use;
        private DateTime? _add_date;
        private DateTime? _stop_date;
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
        public string DEPT_CODE_ESB
        {
            set { _dept_code_esb = value; }
            get { return _dept_code_esb; }
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
        public string DEPT_INTRO
        {
            set { _dept_intro = value; }
            get { return _dept_intro; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_URL
        {
            set { _dept_url = value; }
            get { return _dept_url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_TEL
        {
            set { _dept_tel = value; }
            get { return _dept_tel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_SUP_CODE
        {
            set { _dept_sup_code = value; }
            get { return _dept_sup_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_TYPE
        {
            set { _dept_type = value; }
            get { return _dept_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_ORDER
        {
            set { _dept_order = value; }
            get { return _dept_order; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_ADDRESS
        {
            set { _dept_address = value; }
            get { return _dept_address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_USE
        {
            set { _dept_use = value; }
            get { return _dept_use; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ADD_DATE
        {
            set { _add_date = value; }
            get { return _add_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? STOP_DATE
        {
            set { _stop_date = value; }
            get { return _stop_date; }
        }
        public int OPERA_TYPE
        {
            get;
            set;
        }
        #endregion Model

    }
}

