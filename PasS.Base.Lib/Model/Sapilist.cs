using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib.Model
{
    /// <summary>
    /// apilist:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Sapilist
    {
        public Sapilist()
        { }
        #region Model
        private int _api_id;
        private string _api_name;
        private string _api_note;
        private bool _mark_stop = false;
        private DateTime _add_time;
        private bool _isbus_id = false;
        private DateTime? _stop_time;
        /// <summary>
        /// 
        /// </summary>
        public int API_ID
        {
            set { _api_id = value; }
            get { return _api_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string API_Name
        {
            set { _api_name = value; }
            get { return _api_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string API_Note
        {
            set { _api_note = value; }
            get { return _api_note; }
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
        public DateTime Add_Time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool isBUS_ID
        {
            set { _isbus_id = value; }
            get { return _isbus_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Stop_time
        {
            set { _stop_time = value; }
            get { return _stop_time; }
        }
        #endregion Model

    }
}
