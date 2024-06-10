using System.ComponentModel.DataAnnotations;

namespace POSC2BUnionPay.Models
{
    public class GetQRCodeResult
    {
        [Display(Name = "平台错误码")]
        public string errCode { get; set; }

        [Display(Name = "平台错误信息")]
        public string errMsg { get; set; }

        [Display(Name = "商户号")]
        public string mid { get; set; }

        [Display(Name = "终端号")]
        public string tid { get; set; }

        [Display(Name = "业务类型")]
        public string instMid { get; set; }

        [Display(Name = "账单号")]
        public string billNo { get; set; }

        [Display(Name = "账单二维码")]
        public string billQRCode { get; set; }  
    }
}
