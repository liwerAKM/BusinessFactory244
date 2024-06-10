using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Class
{
    class EInvoiceDownload_IN
    {
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HOS_ID { get; set; }
        /// <summary>
        /// 操作员ID
        /// </summary>
        public string USER_ID { get; set; }
        /// <summary>
        /// 自助机终端号
        /// </summary>
        public string LTREMINAL_SN { get; set; }
        /// <summary>
        /// 票据代码
        /// </summary>
        public string INVOICE_CODE { get; set; }
        /// <summary>
        /// 电子票据号码
        /// </summary>
        public string INVOICE_NUMBER { get; set; }
        /// <summary>
        /// 其他条件
        /// </summary>
        public string FILTER { get; set; }
    }
    class EInvoiceDownload_OUT
    {
        /// <summary>
        /// 发票文件内容,base64 编码； 
        /// </summary>
        public string  INVOICEFILEDATA { get; set; }
        /// <summary>
        /// 发票清单文件内容,base64 编码； 
        /// </summary>
        public string   INVENTORYFILEDATA{ get; set; }
        /// <summary>
        /// 文件下载地址 
        /// </summary>
        public string  INVOICE_URL{ get; set; }
    }
}
