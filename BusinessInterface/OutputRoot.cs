using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessInterface
{
    public class OutputRoot
    {

        public OutputRoot()
        {
        }

        public OutputRoot(InputRoot inputRoot)
        {
            msgid = inputRoot.msgid;
        }
        /// <summary>
        /// 返回应答码 
        /// 0000成功 其他失败
        /// </summary>
        public string RspCode { get; set; }

        /// <summary>
        /// 返回应答描述
        /// </summary>
        public string RspMsg { get; set; }

        /// <summary>
        /// 接收方报文ID
        /// </summary>
        public string nf_refmsgid { get; set; }


        /// <summary>
        /// 发送方报文ID
        /// </summary>
        public string msgid { get; set; }

        /// <summary>
        /// 交易输出
        /// </summary>
        public object OutData { get; set; }

    }
}
