using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EBPP.Model
{
    public class Mebppquery
    {
        public Mebppquery()
        { }

        public Mebppquery(Mebppmain mebppmain)
        {
            this.idcardNo = mebppmain.idcardNo;
            this.billID = mebppmain.billID;
            this.openData = mebppmain.openTime ;
            this.EBPPID = mebppmain.EBPPID;
            this.PayType = mebppmain.PayType;
            this.cancel = mebppmain.cancel;
            this.unifiedOrgCode = mebppmain.unifiedOrgCode;
            this.TotalMoney = mebppmain.TotalMoney;
            this.EBPPType = mebppmain.EBPPType;
            this.PDFUrl = mebppmain.PDFUrl;
            this.openTime = mebppmain.openTime;
            this.CreatTime = mebppmain.CreatTime;
            this.RepealTime = mebppmain.RepealTime;
        }
        #region Model
        private string _idcardno;
        private DateTime _opendata;
        private long? _ebppid;
        private bool _cancel = false;
        private string _unifiedorgcode;
        private string _orgname;
        private string _billid;
        private decimal _totalmoney = 0.00M;
        private int _ebpptype;
        private int _paytype;
        private string _pdfurl;
        private DateTime _opentime;
        private DateTime? _creattime;
        private DateTime? _repealtime;
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
        public DateTime openData
        {
            set { _opendata = value; }
            get { return _opendata; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long? EBPPID
        {
            set { _ebppid = value; }
            get { return _ebppid; }
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
        public string OrgName
        {
            set { _orgname = value; }
            get { return _orgname; }
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
        public decimal TotalMoney
        {
            set { _totalmoney = value; }
            get { return _totalmoney; }
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
