using System;
namespace PasS.Base.Lib.Model
{
    /// <summary>
    /// businessinfoversion:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class businessinfoversion
    {
        public businessinfoversion()
        { }
        public businessinfoversion(BusinessInfo businessInfo)
        {
            BusID = businessInfo.BusID;
            VersionN = DbHelper.VersionTodecimal(businessInfo.version);
            Version = businessInfo.version;
            projectID = businessInfo.ProjectID ;
            FilePath = businessInfo.DllPath;
            DllName = businessInfo.DllName;
          //  LastEditUser = businessInfo.LastEditUser;
            Status = 1;
        }
        #region Model
        private string _busid;
        private string _Busversion;
        private decimal _versionn;
        private int _status = 1;
        private string _projectid;
        private string _filepath;
        private string _dllname;
        private string _version;
        private string _lastedituser;
        
        /// <summary>
        /// 
        /// </summary>
        public string BusID
        {
            set { _busid = value; }
            get { return _busid; }
        }
        public string BusVersion
        {
            set { _Busversion = value; }
            get { return _Busversion; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal VersionN
        {
            set { _versionn = value; }
            get { return _versionn; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
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
        public string DllName
        {
            set { _dllname = value; }
            get { return _dllname; }
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
        public string LastEditUser
        {
            set { _lastedituser = value; }
            get { return _lastedituser; }
        }

    
        #endregion Model

    }
}

