using System;
namespace Plat.Model
{
    /// <summary>
    /// ccbpay_tran:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ccbpay_tran
    {
        public ccbpay_tran()
        { }
        #region Model
        private string _orderid;
        private string _curcode;
        private string _merchantid;
        private string _posid;
        private string _branchid;
        private decimal _je;
        private DateTime _dj_time;
        private string _txcode;
        private string _remark1;
        private string _remark2;
        private string _clientip;
        private string _reginfo;
        private string _referer;
        private string _proinfo;
        private string _txn_type;
        /// <summary>
        /// 
        /// </summary>
        public string ORDERID
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CURCODE
        {
            set { _curcode = value; }
            get { return _curcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MerchantID
        {
            set { _merchantid = value; }
            get { return _merchantid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PosID
        {
            set { _posid = value; }
            get { return _posid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BranchID
        {
            set { _branchid = value; }
            get { return _branchid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Je
        {
            set { _je = value; }
            get { return _je; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime DJ_TIME
        {
            set { _dj_time = value; }
            get { return _dj_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TXCODE
        {
            set { _txcode = value; }
            get { return _txcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REMARK1
        {
            set { _remark1 = value; }
            get { return _remark1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REMARK2
        {
            set { _remark2 = value; }
            get { return _remark2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CLIENTIP
        {
            set { _clientip = value; }
            get { return _clientip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REGINFO
        {
            set { _reginfo = value; }
            get { return _reginfo; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REFERER
        {
            set { _referer = value; }
            get { return _referer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PROINFO
        {
            set { _proinfo = value; }
            get { return _proinfo; }
        }
        public string TXN_TYPE
        {
            set { _txn_type = value; }
            get { return _txn_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string trade_code
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string trade_message
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string error_code
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string error_message
        {
            get;
            set;
        }
        #endregion Model

    }
}

