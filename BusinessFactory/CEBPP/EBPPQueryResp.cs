using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
    /// <summary>
    /// 快速查询返回数据
    /// </summary>
    public class EBPPQueryResp
    {/// <summary>
     /// 返回结果代码 "0"时成功
     /// </summary>
        public string Code;
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string Msg;

        public List<QuertInfo> EBPPList;
    }


    public class QuertInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string idcardNo;

        /// <summary>
        /// 
        /// </summary>
        public DateTime openData;
        /// <summary>
        /// 
        /// </summary>
        public long EBPPID;
        /// <summary>
        /// 
        /// </summary>
        public bool cancel;
        /// <summary>
        /// 
        /// </summary>
        public string unifiedOrgCode;
        /// <summary>
        /// 
        /// </summary>
        public string OrgName;
        /// <summary>
        /// 
        /// </summary>
        public string billID;
        /// <summary>
        /// 
        /// </summary>
        public decimal TotalMoney;
        /// <summary>
        /// 
        /// </summary>
        public int EBPPType;
        /// <summary>
        /// 
        /// </summary>
        public int PayType;
        /// <summary>
        /// 
        /// </summary>
        public string PDFUrl;

    }

}
