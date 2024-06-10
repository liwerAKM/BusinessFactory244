using System;
namespace Plat.Model
{
    /// <summary>
    /// unionpay_tran:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class unionpay_tran
    {
        public unionpay_tran()
        { }
        #region Model
        private string _orderid;
        private string _merid;
        private string _queryid;
        private string _tn;
        private decimal _je;
        private string _currencycode = "156";
        private string _txn_type;
        private string _txnsubtype;
        private string _biztype;
        private string _channeltype;
        private string _accesstype;
        private string _eserved;
        private string _reqreserved;
        private string _refcode;
        private string _orderdesc;
        private string _termcode;
        private DateTime _dj_time;
        private DateTime? _txntime;
        private DateTime? _at_time;
        private string _atrespcode;
        private string _atrespmsg;
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
        public string MERID
        {
            set { _merid = value; }
            get { return _merid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QUERYID
        {
            set { _queryid = value; }
            get { return _queryid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TN
        {
            set { _tn = value; }
            get { return _tn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal JE
        {
            set { _je = value; }
            get { return _je; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string currencyCode
        {
            set { _currencycode = value; }
            get { return _currencycode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TXN_TYPE
        {
            set { _txn_type = value; }
            get { return _txn_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string txnSubType
        {
            set { _txnsubtype = value; }
            get { return _txnsubtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string bizType
        {
            set { _biztype = value; }
            get { return _biztype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string channelType
        {
            set { _channeltype = value; }
            get { return _channeltype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string accessType
        {
            set { _accesstype = value; }
            get { return _accesstype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string eserved
        {
            set { _eserved = value; }
            get { return _eserved; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string reqReserved
        {
            set { _reqreserved = value; }
            get { return _reqreserved; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REFCODE
        {
            set { _refcode = value; }
            get { return _refcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string orderDesc
        {
            set { _orderdesc = value; }
            get { return _orderdesc; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TERMCODE
        {
            set { _termcode = value; }
            get { return _termcode; }
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
        public DateTime? txnTime
        {
            set { _txntime = value; }
            get { return _txntime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AT_Time
        {
            set { _at_time = value; }
            get { return _at_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATrespCode
        {
            set { _atrespcode = value; }
            get { return _atrespcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ATrespMsg
        {
            set { _atrespmsg = value; }
            get { return _atrespmsg; }
        }
        #endregion Model

    }
}

