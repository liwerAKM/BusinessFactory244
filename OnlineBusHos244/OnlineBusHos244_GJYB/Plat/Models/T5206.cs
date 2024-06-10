using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos244_GJYB.Models
{
    #region N：人员累计信息查询

    public class T5206
    {
        /// <summary>
        /// 输入（节点标识：data）
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 人员编号
            /// </summary>
            public string psn_no { get; set; }
            /// <summary>
            /// 累计年月
            /// </summary>
            public string cum_ym { get; set; }


        }

        public class Root
        {
            public Data data { get; set; }

        }
    }


    public class RT5206
    {
        /// <summary>
        /// 输出（节点标识：cuminfo）
        /// </summary>
        public class CuminfoItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string cum_type_code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string year { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cum { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string cum_ym { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string insutype { get; set; }
        }
        public class Output
        {
            /// <summary>
            /// 
            /// </summary>
            public List<CuminfoItem> cuminfo { get; set; }
        }
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string infcode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string inf_refmsgid { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string refmsg_time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string respond_time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string enctype { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string signtype { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string err_msg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Output output { get; set; }
        }
    }
    #endregion
}
