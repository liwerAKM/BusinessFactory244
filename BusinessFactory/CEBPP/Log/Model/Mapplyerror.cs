using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP.Log.Model
{
    /// <summary>
    /// 上传内容错误日志
    /// </summary>
  public   class Mapplyerror
    {
        public Mapplyerror()
        { }
        #region Model
        private DateTime _applytime;
        private string _unifiedorgcode="";
        private string _orgname = "";
        private string _billid = "";
        private string _billcode = "";
        private decimal _totalmoney = 0.00M;
        private string _idcardno = "";
        private string _patname = "";
        private int _ebpptype = 1;
        private int _paytype = 1;
        private string _pdfurl = "";
        private DateTime _creattime;
        private string _indata = "";
        private string _outdata = "";
        /// <summary>
        /// 
        /// </summary>
        public DateTime applyTime
        {
            set { _applytime = value; }
            get { return _applytime; }
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
        public string idcardNo
        {
            set { _idcardno = value; }
            get { return _idcardno; }
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
        public DateTime CreatTime
        {
            set { _creattime = value; }
            get { return _creattime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InData
        {
            set { _indata = value; }
            get { return _indata; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OutData
        {
            set { _outdata = value; }
            get { return _outdata; }
        }
        #endregion Model

    }
}
