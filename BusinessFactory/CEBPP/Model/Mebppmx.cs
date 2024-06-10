using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EBPP.Model
{
    /// <summary>
    /// 明细项目
    /// </summary>
  public   class Mebppmx
    {
        public Mebppmx()
        { }
        public Mebppmx(ListInfo listInfo)
        {
            this.D1 = DateTime.Now;
            this.Orde = listInfo.order;
            this.ItemType = listInfo.iT;
            this.ItemName = listInfo.iN;
            this.ItemGG = listInfo.iG;
            this.ItemUnit = listInfo.iU;
            this.ItemPrice = listInfo.iP;
            this.ItemCount = listInfo.iC;
            this.ItemJe = listInfo.iJ;
            this.UpLimit = listInfo.uL;
            this.SelfFee = listInfo.sR;
            this.SelfRate = listInfo.sF;
            this.DeductFee = listInfo.dF;
            this.ItemTypeCode = listInfo.iTC;

        }
        #region Model
        private long _ebppid;
        private string _orde;
        private string _itemtype;
        private string _itemtypecode;
        private string _itemname;
        private string _itemgg;
        private string _itemunit;
        private decimal _itemprice = 0.0000M;
        private decimal _itemcount = 0.00M;
        private decimal _itemje = 0.0000M;
        private decimal _selfrate = 0.00M;
        private string _uplimit;
        private decimal _selffee = 0.0000M;
        private decimal _deductfee = 0.0000M;
        private DateTime _d1;
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
        public string Orde
        {
            set { _orde = value; }
            get { return _orde; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemType
        {
            set { _itemtype = value; }
            get { return _itemtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemTypeCode
        {
            set { _itemtypecode = value; }
            get { return _itemtypecode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemName
        {
            set { _itemname = value; }
            get { return _itemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemGG
        {
            set { _itemgg = value; }
            get { return _itemgg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemUnit
        {
            set { _itemunit = value; }
            get { return _itemunit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal ItemPrice
        {
            set { _itemprice = value; }
            get { return _itemprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal ItemCount
        {
            set { _itemcount = value; }
            get { return _itemcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal ItemJe
        {
            set { _itemje = value; }
            get { return _itemje; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SelfRate
        {
            set { _selfrate = value; }
            get { return _selfrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UpLimit
        {
            set { _uplimit = value; }
            get { return _uplimit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal SelfFee
        {
            set { _selffee = value; }
            get { return _selffee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal DeductFee
        {
            set { _deductfee = value; }
            get { return _deductfee; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime D1
        {
            set { _d1 = value; }
            get { return _d1; }
        }
        #endregion Model

    }
}
