using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel
{
    /// <summary>
    /// 发送的数据
    /// </summary>
    public class DataSend
    {
        /// <summary>
        ///操作ID
        /// </summary>
        public int Action_Id { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Param { get; set; }
    }
    /// <summary>
    /// 返回的数据
    /// </summary>
    public class DataReturn
    {
        /// <summary>
        /// 消息码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息说明
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public string Param { get; set; }
    }
}
