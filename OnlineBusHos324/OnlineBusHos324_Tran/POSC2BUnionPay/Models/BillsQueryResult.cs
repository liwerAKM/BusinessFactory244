using System.ComponentModel.DataAnnotations;

namespace POSC2BUnionPay.Models
{
    public class BillsQueryResult
    {
        [Display(Name = "平台错误码")]
        public string errCode { get; set; }

        [Display(Name = "平台错误信息")]
        public string errMsg { get; set; }

        public string msgId { get; set; }
        public string msgType { get; set; }
        public string msgSrc { get; set; }
        public string srcReserve { get; set; }
        public string responseTimestamp { get; set; }

        [Display(Name = "商户号")]
        public string mid { get; set; }

        [Display(Name = "终端号")]
        public string tid { get; set; }

        [Display(Name = "业务类型")]
        public string instMid { get; set; }

        [Display(Name = "账单号")]
        public string billNo { get; set; }
        public string billDate { get; set; }
        public string createTime { get; set; }

        [Display(Name = "账单状态")]
        public string billStatus { get; set; }
        [Display(Name = "支付总金额，单位为分")]
        public string totalAmount { get; set; }
        public string billQRCode { get; set; }
        public string billDesc { get; set; }
        public string memberId { get; set; }
        public string counterNo { get; set; }
        public string merName { get; set; }
        public string memo { get; set; }
        public string secureStatus { get; set; }
        public string completeAmount { get; set; }

        public  billPayment billPayment{get;set;}
    }
}
