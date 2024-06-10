using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{/// <summary>
 /// 撤销或红冲申请
 /// </summary>
    public class EBPPRepealReq
    {  /// <summary>
       /// String	18	是 医院编号 统一社会信用代码
       /// </summary>
        public string unifiedOrgCode;
      
      
        /// <summary>
        /// String	30	是 医院内部票据号 发票时为发票号，清单时为住院号
        /// </summary>
        public string BillID;

        /// <summary>
        /// String	10	是 发票时为票据代码， 清单时为住院次数
        /// </summary>
        public string BillCode;
        /// <summary>
        ///  String	24	是 证件号码(身份证)
        /// </summary>
        public string idcardNo;
     

    }
}