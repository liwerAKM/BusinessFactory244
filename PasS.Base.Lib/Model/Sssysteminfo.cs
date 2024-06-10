using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib.Model
{
    
    /// <summary>
    /// ssysteminfo:系统 实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sssysteminfo
    {
        public Sssysteminfo()
        { }
        #region Model
        private int _sysid;
        private string _sys_name;
        private bool _mark_stop = false;
        private int _sys_type = 1;
        private DateTime _add_time;
        private DateTime? _stop_time;
        private string _version = "1.0.0.0";
        private string _note;
        /// <summary>
        /// 
        /// </summary>
        public int SYSID
        {
            set { _sysid = value; }
            get { return _sysid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SYS_NAME
        {
            set { _sys_name = value; }
            get { return _sys_name; }
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
        public int SYS_Type
        {
            set { _sys_type = value; }
            get { return _sys_type; }
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
        /// <summary>
        /// 
        /// </summary>
        public string Version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }
        #endregion Model

    }
}
