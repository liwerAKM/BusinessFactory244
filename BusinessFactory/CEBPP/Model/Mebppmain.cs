using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EBPP.Model
{
  public   class Mebppmain
    {
        public Mebppmain()
        { }
        #region Model
        private long _ebppid;
        private string _idcardno;
        private bool _cancel = false;
        private string _unifiedorgcode;
        private string _billid;
        private string _billcode;
        private decimal _totalmoney = 0.00M;
        private string _patname;
        private int _ebpptype = 1;
        private int _paytype = 1;
        private string _pdfurl;
        private string _orgname;
        private DateTime _opentime;
        private DateTime? _creattime;
        private DateTime? _repealtime;
        /// <summary>
        /// 
        /// </summary>
        public long EBPPID
        {
            set { _ebppid = value; }
            get { return _ebppid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string idcardNo
        {
            set { _idcardno = value; }
            get { return _idcardno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool cancel
        {
            set { _cancel = value; }
            get { return _cancel; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string unifiedOrgCode
        {
            set { _unifiedorgcode = value; }
            get { return _unifiedorgcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string billID
        {
            set { _billid = value; }
            get { return _billid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string billCode
        {
            set { _billcode = value; }
            get { return _billcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal TotalMoney
        {
            set { _totalmoney = value; }
            get { return _totalmoney; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PatName
        {
            set { _patname = value; }
            get { return _patname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int EBPPType
        {
            set { _ebpptype = value; }
            get { return _ebpptype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PDFUrl
        {
            set { _pdfurl = value; }
            get { return _pdfurl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrgName
        {
            set { _orgname = value; }
            get { return _orgname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime openTime
        {
            set { _opentime = value; }
            get { return _opentime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreatTime
        {
            set { _creattime = value; }
            get { return _creattime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? RepealTime
        {
            set { _repealtime = value; }
            get { return _repealtime; }
        }
        #endregion Model

    }
}
