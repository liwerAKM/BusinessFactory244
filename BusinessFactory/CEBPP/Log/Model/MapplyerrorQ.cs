using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP.Log.Model
{
    /// <summary>
    /// 上传内容错误日志
    /// </summary>
  public   class MapplyerrorQ
    {
        public MapplyerrorQ()
        { }
        #region Model
       
        /// <summary>
        /// 
        /// </summary>
        public DateTime applyTime
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string unifiedOrgCode
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OrgName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string billID
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string billCode
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal TotalMoney
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string idcardNo
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PatName
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int EBPPType
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public int PayType
        {
            set;
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PDFUrl
        {
            set;
            get;
        }
       
        
        /// <summary>
        /// 
        /// </summary>
        public string OutData
        {
            set;
            get;
        }
        #endregion Model

    }

    /// <summary>
    /// 上传内容错误日志
    /// </summary>
    public class MapplyerrorQ2: MapplyerrorQ
    {
        public MapplyerrorQ2()
        { }
        #region Model

    
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatTime
        {
            set;
            get;
        }
       
        /// <summary>
        /// 
        /// </summary>
        public string InData
        {
            set;
            get;
        }
        #endregion Model

    }
}
