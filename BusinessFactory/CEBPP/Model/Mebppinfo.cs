using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EBPP.Model
{
  public  class Mebppinfo
    {
        public Mebppinfo()
        { }
        #region Model
        private long _ebppid;
        private Int16 _datatype;
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
        ///0.原始所有数据
        ///1,Patinfo 病人基本信息
        ///2,ChargeCollectGenreal普通医保
        ///3.ChargeCollectCadre省医保、省干保、市干保
        ///5.proSettleInfo为结算数据
        ///6.proReceiptDetail为按类别汇总
        ///7.prescriptionDetails处方
        ///4.费用明细
        /// </summary>
        public Int16 DataType
        {
            set { _datatype = value; }
            get { return _datatype; }
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
