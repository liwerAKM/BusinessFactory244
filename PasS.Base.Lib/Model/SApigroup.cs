using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib.Model
{
    /// <summary>
    /// apigroup:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class SApigroup
    {
        public SApigroup()
        { }
        #region Model
        private int _apig_id;
        private string _apig_name;
        private bool _mark_stop = false;
        private string _apig_note;
        private int _apig_type = 1;
        private int _sysid = 0;
        private DateTime _add_time;
        private DateTime? _stop_time;
        private string _adduser_id;
        /// <summary>
        /// 
        /// </summary>
        public int APIG_ID
        {
            set { _apig_id = value; }
            get { return _apig_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APIG_Name
        {
            set { _apig_name = value; }
            get { return _apig_name; }
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
        public string APIG_Note
        {
            set { _apig_note = value; }
            get { return _apig_note; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int APIG_Type
        {
            set { _apig_type = value; }
            get { return _apig_type; }
        }
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
        public DateTime Add_Time
        {
            set { _add_time = value; }
            get { return _add_time; }
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
        public string AddUser_ID
        {
            set { _adduser_id = value; }
            get { return _adduser_id; }
        }
        #endregion Model


    }
}
