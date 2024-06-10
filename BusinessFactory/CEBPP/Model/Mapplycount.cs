using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP.Model
{
  public   class Mapplycount
    {
        #region Model
        private DateTime _applydate;
        private string _unifiedorgcode;
        private int _ghcount = 0;
        private int _mzcount = 0;
        private int _zycount = 0;
        private int _qdcount = 0;
        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate
        {
            set { _applydate = value; }
            get { return _applydate; }
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
        public int GhCount
        {
            set { _ghcount = value; }
            get { return _ghcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MzCount
        {
            set { _mzcount = value; }
            get { return _mzcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ZyCount
        {
            set { _zycount = value; }
            get { return _zycount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int QdCount
        {
            set { _qdcount = value; }
            get { return _qdcount; }
        }
        #endregion Model

    }
}
