using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

using System.Threading;

using System.Data;
using ZZJSLB.Base.Lib;
using ZZJBusinessInterface;

namespace ZZJProcessBusTest
{
    /// <summary>
    /// 测试两数相加
    /// </summary>
    public class ProcessTest : ProcessingBusinessAsyncResultByte
    {

        public override bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out)
        {
            string InBusinessInfo = base.GetStrData(In);
            string OutBusinessInfo =  "return"+InBusinessInfo ;
            Out = base.GetByte(OutBusinessInfo);
            return true;
        }

    }
   
   
}
