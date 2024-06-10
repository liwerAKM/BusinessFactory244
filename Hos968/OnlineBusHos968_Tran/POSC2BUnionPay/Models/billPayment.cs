using System.ComponentModel.DataAnnotations;

namespace POSC2BUnionPay.Models
{
    public class billPayment
    {
        [Display(Name = "账单业务类型")]
        public string billBizType { get; set; }

        [Display(Name = "开票金额")]
        public string invoiceAmount { get; set; }

        [Display(Name = "商户订单号")]
        public string merOrderId { get; set; }

        [Display(Name = "交易参考号")]
        public string paySeqId { get; set; }

        [Display(Name = "账单流水总金额")]
        public string totalAmount { get; set; }

        [Display(Name = "实付金额")]
        public string buyerPayAmount { get; set; }

        [Display(Name = "钱包折扣金额")]
        public string couponAmount { get; set; }

        [Display(Name = "折扣金额")]
        public string discountAmount { get; set; }

        [Display(Name = "买家ID")]
        public string buyerId { get; set; }

        [Display(Name = "买家用户名")]
        public string buyerUsername { get; set; }

        [Display(Name = "支付详情")]
        public string payDetail { get; set; }

        [Display(Name = "支付时间")]
        public string payTime { get; set; }
        [Display(Name = "结算时间")]
        public string settleDate { get; set; }

        [Display(Name = "交易状态")]
        public string status { get; set; }

        [Display(Name = "目标平台单号")]
        public string targetOrderId { get; set; }

        [Display(Name = "目标系统")]
        public string targetSys { get; set; }


    }
}
