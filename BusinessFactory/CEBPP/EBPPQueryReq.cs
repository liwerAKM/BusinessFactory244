using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
    /// <summary>
    /// 快速查询显示请求
    /// </summary>
   public  class EBPPQueryReq
    {
        /// <summary>
        ///  String	24	是 证件号码(身份证)
        /// </summary>
        public string idcardNo;
        /// <summary>
        /// 查起始日期，将返回从这个日期起到现在的票据数据
        /// </summary>
        public DateTime  limitData;
    }
}
