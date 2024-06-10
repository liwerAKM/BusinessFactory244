using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{

    /// <summary>
    /// 获取明细显示返回
    /// </summary>
    public class EBPPGetMxResp
    {/// <summary>
     /// 返回结果代码 "0"时成功
     /// </summary>
        public string Code;
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string Msg;

        public List<EBPPSaveReq> eBPPMxList;
    }

    /// <summary>
    /// 获取Info明细显示返回
    /// </summary>
    public class EBPPGetInfoByIDResp
    {/// <summary>
     /// 返回结果代码 "0"时成功
     /// </summary>
        public string Code;
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string Msg;

        public EBPPSaveReq  eBPPMx;
    }
}
