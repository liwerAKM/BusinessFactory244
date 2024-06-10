using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessInterface
{
    public class InputRoot
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusID { get; set; }
        /// <summary>
        /// 医院编号
        /// </summary>
        public string HOS_ID { get; set; }


        /// <summary>
        /// 交易编号
        /// </summary>
        public string infno { get; set; }
        /// <summary>
        /// 发送方报文ID 
        /// 说明：定点医药机构编号(12)+时间(14)+顺序号(4)时间格式：yyyyMMddHHmmss
        /// </summary>
        public string msgid { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string signtype { get; set; }

        /// <summary>
        /// 接口版本号
        /// 例如：“V1.0”，版本号由医保下发通知。
        /// </summary>
        public string infver { get; set; }

        /// <summary>
        /// 签名信息
        /// </summary>
        public string sign { get; set; }
       
        /// <summary>
        /// 经办人类别
        /// 1-经办人；2-自助终端；3-移动终端
        /// </summary>
        public string opter_type { get; set; }
        /// <summary>
        /// 经办人编号
        /// </summary>
        public string opter_no { get; set; }

        /// <summary>
        /// 交易输入
        /// </summary>
        public object InData { get; set; }

        /// <summary>
        /// 客户端标记 标记用于灰度发布 非必填
        /// </summary>
        public string CTag { get; set; }

        /// <summary>
        /// 客户端标记 是否启用压缩 0 未压缩 1 压缩 
        /// </summary>
        public int Gzip { get; set; }
    }
}
