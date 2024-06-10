using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSC2BUnionPay.Models
{
    public class modPosc2bpaylog
    {
        public string HOS_ID { get; set; }
        public string tradetype { get; set; }
        public string billno { get; set; }
        public string indata { get; set; }
        public DateTime intime { get; set; }
        public string outdata { get; set; }
        public DateTime outtime { get; set; }
    }
}
