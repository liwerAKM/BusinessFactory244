using System;
namespace Plat.Model
{
    /// <summary>
    /// schedule_period:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class schedule_period
    {
        public schedule_period()
        { }
        #region Model
        private string _hos_id;
        private string _dept_code;
        private string _doc_no;
        private string _sch_date;
        private string _sch_time;
        private string _period_start;
        private string _period_end;
        private int _count_all = 0;
        private int _count_yet = 0;
        //hlw add 2015.11.09 用于判断添加修改或删除
        private int _opera_type = 0;
        /// <summary>
        ///  是否存在
        /// </summary>
        private bool _isExists = false;
        /// <summary>
        /// 
        /// </summary>
        public string HOS_ID
        {
            set { _hos_id = value; }
            get { return _hos_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DEPT_CODE
        {
            set { _dept_code = value; }
            get { return _dept_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOC_NO
        {
            set { _doc_no = value; }
            get { return _doc_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SCH_DATE
        {
            set { _sch_date = value; }
            get { return _sch_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SCH_TIME
        {
            set { _sch_time = value; }
            get { return _sch_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PERIOD_START
        {
            set { _period_start = value; }
            get { return _period_start; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PERIOD_END
        {
            set { _period_end = value; }
            get { return _period_end; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int COUNT_ALL
        {
            set { _count_all = value; }
            get { return _count_all; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int COUNT_YET
        {
            set { _count_yet = value; }
            get { return _count_yet; }
        }
        /// <summary>
        /// 1新增或修改；2删除
        /// </summary>
        public int OPERA_TYPE
        {
            set { _opera_type = value; }
            get { return _opera_type; }
        }
        /// <summary>
        /// 1新增或修改；2删除
        /// </summary>
        public bool IsExists
        {
            set { _isExists = value; }
            get { return _isExists; }
        }
        #endregion Model

    }
}

