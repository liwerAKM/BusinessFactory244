using System.ComponentModel.DataAnnotations;

namespace POSC2BUnionPay.Models
{
    public class BillRefundResult
    {
        [Display(Name = "平台错误码")]
        public string errCode { get; set; }

        [Display(Name = "平台错误信息")]
        public string errMsg { get; set; }

        [Display(Name = "消息ID")]
        public string msgId { get; set; }

        [Display(Name = "消息类型")]
        public string msgType { get; set; }

        [Display(Name = "报文响应时间")]
        public string responseTimestamp { get; set; }

        [Display(Name = "账单时间")]
        public string billDate { get; set; }

        [Display(Name = "账单号")]
        public string billNo { get; set; }

        [Display(Name = "账单二维码")]
        public string billQRCode { get; set; }

        [Display(Name = "账单状态")]
        public string billStatus { get; set; }

        [Display(Name = "商户号")]
        public string mid { get; set; }

        [Display(Name = "终端号")]
        public string tid { get; set; }

        [Display(Name = "业务类型")]
        public string instMid { get; set; }

        [Display(Name = "支付总金额，单位为分")]
        public string totalAmount { get; set; }

        [Display(Name = "原订单商户订单号")]
        public string merOrderId { get; set; }

        [Display(Name = "退货订单号")]
        public string refundOrderId { get; set; }

        [Display(Name = "目标系统退货订单号")]
        public string refundTargetOrderId { get; set; }

        [Display(Name = "退款时间")]
        public string refundPayTime { get; set; }

        [Display(Name = "退款结果")]
        public string refundStatus { get; set; }
    }
}
