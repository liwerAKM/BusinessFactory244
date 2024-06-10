using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib.Model
{
    /// <summary>
    /// apiuser:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class apiuser
    {
        public apiuser()
        { }
        #region Model
        private string _apiu_id;
        private string _apiu_name;
        private string _aeskey;
        private string _rsapubkey;
        private string _rsa2pubkey;
        private string _ip_whitelist = "0.0.0.0/0";
        private DateTime _add_time;
        private bool _mark_stop = false;
        private DateTime? _stop_time;
        private string _org_id;
        private int  _AType;
        /// <summary>
        /// 
        /// </summary>
        public string APIU_ID
        {
            set { _apiu_id = value; }
            get { return _apiu_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APIU_Name
        {
            set { _apiu_name = value; }
            get { return _apiu_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AESKey
        {
            set { _aeskey = value; }
            get { return _aeskey; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RSAPubKey
        {
            set { _rsapubkey = value; }
            get { return _rsapubkey; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RSAPubID
        {
            set;get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RSA2PubKey
        {
            set { _rsa2pubkey = value; }
            get { return _rsa2pubkey; }
        }
        public int RSA2PubID
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string IP_whitelist
        {
            set { _ip_whitelist = value; }
            get { return _ip_whitelist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Add_Time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool Mark_Stop
        {
            set { _mark_stop = value; }
            get { return _mark_stop; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Stop_Time
        {
            set { _stop_time = value; }
            get { return _stop_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ORG_ID
        {
            set { _org_id = value; }
            get { return _org_id; }
        }

        public string SpringRSAPriKey
        {
            set;
            get;
        }

        public string SpringRSAPubKey
        {
            set;
            get;
        }

        public int SpringRSAID
        {
            set;
            get;
        }

        /// <summary>
        /// 用户类型:1 默认用户；2 WebSocke动态秘钥初始化用户
        /// </summary>
        public int AType
        {
            set;
            get;
        }
        

        #endregion Model

    }
}
