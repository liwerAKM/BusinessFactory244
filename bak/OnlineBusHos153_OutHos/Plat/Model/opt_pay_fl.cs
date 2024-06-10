using System;
namespace Plat.Model
{
    /// <summary>
    /// opt_pay_fl:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class opt_pay_fl
    {
        public opt_pay_fl()
        { }
        #region Model
        private string _pay_id;
        private string _fl_no;
        private string _fl_name;
        private string _dept_code;
        private string _dept_name;
        private decimal _fl_je;
        private string _fl_order;
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
        public string FL_NO
        {
            set { _fl_no = value; }
            get { return _fl_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FL_NAME
        {
            set { _fl_name = value; }
            get { return _fl_name; }
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
        public decimal FL_JE
        {
            set { _fl_je = value; }
            get { return _fl_je; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FL_ORDER
        {
            set { _fl_order = value; }
            get { return _fl_order; }
        }
        #endregion Model

    }
}

