using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
   public  class EbPPTest
    {

        public static bool Save(string Data)
        {
            EBPPSaveReq eBPPSaveReq = JsonConvert.DeserializeObject<EBPPSaveReq>(Data);
            for (int i = 0; i < 1; i++)
            {
                eBPPSaveReq.billID = Guid.NewGuid().ToString().Substring(0, 20);
                eBPPSaveReq.openTime = DateTime.Now.ToString("yyyyMMddHmmss");
                eBPPSaveReq.totalFee = eBPPSaveReq.items.Sum(t => t.iJ);
                new EBPPHelper().DealEBPPSaveReq(eBPPSaveReq);
            }
            return true;

        }

        public static bool Rrepal(string Data)
        {
            EBPPRepealReq eBPPRepealReq = JsonConvert.DeserializeObject<EBPPRepealReq>(Data);
            new EBPPHelper().DealEBPPRepealReq(eBPPRepealReq);
            return true;

        }
    }
}
