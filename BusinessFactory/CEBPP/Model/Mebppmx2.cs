using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EBPP.Model
{
    /// <summary>
    /// 明细项目
    /// </summary>
  public   class Mebppmx2
    {
        public Mebppmx2()
        { }
        public Mebppmx2(ListInfo listInfo)
        {
            this.D1 = DateTime.Now;
            this.Orde = string.IsNullOrEmpty(listInfo.order) ? "" : listInfo.order;

            this.ItemName = string.IsNullOrEmpty(listInfo.iN) ? "" : listInfo.iN;
            this.iBM = string.IsNullOrEmpty(listInfo.iBM) ? "" : listInfo.iBM;


        }
        public Mebppmx2(string Orde, string ItemName, string iBM)
        {
            this.D1 = DateTime.Now;
            this.Orde = Orde;

            this.ItemName = ItemName;
            this.iBM = iBM;


        }
        #region Model
        private long _ebppid;
        private string _orde;
        private string _itemname;
        private string _ibm;
        private string _json;
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
        public string ItemName
        {
            set { _itemname = value; }
            get { return _itemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string iBM
        {
            set { _ibm = value; }
            get { return _ibm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string JSON
        {
            set { _json = value; }
            get { return _json; }
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
