using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSC2BUnionPay.Models
{
    public class Notify
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string COMM_SN { get; set; }
        /// <summary>
        /// 交易类型 
        /// </summary>
        public string TXN_TYPE { get; set; }
        public string HOS_ID { get; set; }
        public string mid { get; set; }
        public string tid { get; set; }
        public string instMid { get; set; }
        public string billNo { get; set; }
        public string billQRCode { get; set; }
        public string billDate { get; set; }
        public string createTime { get; set; }
        public string billStatus { get; set; }
        public string billDesc { get; set; }
        public string totalAmount { get; set; }
        public string memberId { get; set; }
        public string counterNo { get; set; }
        public string merName { get; set; }
        public string memo { get; set; }
        public string notifyId { get; set; }
        public string secureStatus { get; set; }
        public string completeAmount { get; set; }
        public string notify_time { get; set; }

    }
}
