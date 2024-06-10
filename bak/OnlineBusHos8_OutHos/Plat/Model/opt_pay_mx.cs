using System;
namespace Plat.Model
{
    /// <summary>
    /// opt_pay_mx:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class opt_pay_mx
    {
        public opt_pay_mx()
        { }
        #region Model
        private string _pay_id;
        private string _fl_no;
        private string _item_type;
        private string _item_id;
        private string _item_name;
        private string _item_gg;
        private string _count;
        private string _item_unit;
        private decimal _cost;
        private decimal _je;
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
        public string ITEM_TYPE
        {
            set { _item_type = value; }
            get { return _item_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ITEM_ID
        {
            set { _item_id = value; }
            get { return _item_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ITEM_NAME
        {
            set { _item_name = value; }
            get { return _item_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ITEM_GG
        {
            set { _item_gg = value; }
            get { return _item_gg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COUNT
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ITEM_UNIT
        {
            set { _item_unit = value; }
            get { return _item_unit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal COST
        {
            set { _cost = value; }
            get { return _cost; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal je
        {
            set { _je = value; }
            get { return _je; }
        }
        #endregion Model

    }
}

