using System;
namespace Plat.Model
{
    /// <summary>
    /// doc_info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class doc_info
    {
        public doc_info()
        { }
        #region Model
        private string _hos_id;
        private string _dept_code;
        private string _doc_no;
        private string _doc_name;
        private bool _is_expert = false;
        private string _pro_title;
        private string _doc_intro;
        private byte[] _image;
        private DateTime? _image_time;
        private string _doc_order;
        private DateTime _add_time;
        private DateTime? _stop_time;
        private string _doc_skilled;
        private string _sprechstunde;
        private string _image_url;
        //hlw add 2015.11.09 用于判断添加修改或删除
        private int _opera_type = 0;
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
        public string DOC_NAME
        {
            set { _doc_name = value; }
            get { return _doc_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IS_EXPERT
        {
            set { _is_expert = value; }
            get { return _is_expert; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PRO_TITLE
        {
            set { _pro_title = value; }
            get { return _pro_title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOC_INTRO
        {
            set { _doc_intro = value; }
            get { return _doc_intro; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] IMAGE
        {
            set { _image = value; }
            get { return _image; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? IMAGE_TIME
        {
            set { _image_time = value; }
            get { return _image_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOC_ORDER
        {
            set { _doc_order = value; }
            get { return _doc_order; }
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
        public string DOC_SKILLED
        {
            set { _doc_skilled = value; }
            get { return _doc_skilled; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SPRECHSTUNDE
        {
            set { _sprechstunde = value; }
            get { return _sprechstunde; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IMAGE_URL
        {
            set { _image_url = value; }
            get { return _image_url; }
        }
        /// <summary>
        /// 1新增或修改；2删除
        /// </summary>
        public int OPERA_TYPE
        {
            set { _opera_type = value; }
            get { return _opera_type; }
        }
        #endregion Model

    }
}

