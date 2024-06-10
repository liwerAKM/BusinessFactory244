using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib.Model
{
    /// <summary>
    /// 机构实体
    /// </summary>
    [Serializable]
    public partial class Ssorganization
    {
        public Ssorganization()
        { }
        #region Model
        private string _org_id;
        private string _org_name;
        private string _porg_id;
        private int _org_type = 1;
        private bool _mark_stop = false;
        private DateTime _add_time;
        private DateTime? _stop_time;
        /// <summary>
        /// 
        /// </summary>
        public string ORG_ID
        {
            set { _org_id = value; }
            get { return _org_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string org_name
        {
            set { _org_name = value; }
            get { return _org_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PORG_ID
        {
            set { _porg_id = value; }
            get { return _porg_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ORG_Type
        {
            set { _org_type = value; }
            get { return _org_type; }
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
        public DateTime ADD_TIME
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? STOP_TIME
        {
            set { _stop_time = value; }
            get { return _stop_time; }
        }
        #endregion Model

    }

    public partial class STsorganization
    {
        public STsorganization()
        { }
        #region Model
        private string _org_id;
        private string _org_name;
        private string _porg_id;
        private int _org_type = 1;
        private bool _mark_stop = false;
        private DateTime _add_time;
        private DateTime  _stop_time;
        /// <summary>
        /// 
        /// </summary>
        public string ORG_ID
        {
            set { _org_id = value; }
            get { return _org_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string org_name
        {
            set { _org_name = value; }
            get { return _org_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PORG_ID
        {
            set { _porg_id = value; }
            get { return _porg_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ORG_Type
        {
            set { _org_type = value; }
            get { return _org_type; }
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
        public DateTime ADD_TIME
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime  STOP_TIME
        {
            set { _stop_time = value; }
            get { return _stop_time; }
        }
        #endregion Model

    }
}
