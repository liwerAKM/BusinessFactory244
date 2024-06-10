using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib.Model
{
    /// <summary>
    /// fileinfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class fileinfo
    {
        public fileinfo()
        { }
        #region Model
        private string _projectid = "default";
        private string _filepath;
        private string _filename;
        private string _version;
        private bool _isdirect;
        private string _extension;
        private string _filedescription;
        private string _companyname;
        private string _filesize;
        private DateTime? _ctime;
        private DateTime _mtime;
        private DateTime? _uptime;
        private Byte[] _images;
        private string _note;

        private string _comminversion = "1.0.0.0";
        private string _commaxversion = "1.0.65535.65535";
        /// <summary>
        /// 
        /// </summary>
        public string projectID
        {
            set { _projectid = value; }
            get { return _projectid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FilePath
        {
            set { _filepath = value; }
            get { return _filepath; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Filename
        {
            set { _filename = value; }
            get { return _filename; }
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
        public bool isdirect
        {
            set { _isdirect = value; }
            get { return _isdirect; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Extension
        {
            set { _extension = value; }
            get { return _extension; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileDescription
        {
            set { _filedescription = value; }
            get { return _filedescription; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CompanyName
        {
            set { _companyname = value; }
            get { return _companyname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FileSize
        {
            set { _filesize = value; }
            get { return _filesize; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? cTime
        {
            set { _ctime = value; }
            get { return _ctime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime MTime
        {
            set { _mtime = value; }
            get { return _mtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? upTime
        {
            set { _uptime = value; }
            get { return _uptime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] images
        {
            set { _images = value; }
            get { return _images; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ComMinVersion
        {
            set { _comminversion = value; }
            get { return _comminversion; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ComMaxVersion
        {
            set { _commaxversion = value; }
            get { return _commaxversion; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MD5
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int ImageID
        {
            set;
            get;
        }
        #endregion Model

    }
}
