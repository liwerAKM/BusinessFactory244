using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineBusHos324_Common.Model
{
    public class GETGOODSLIST
    {
        public class GETGOODSLIST_IN
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
            public string LTERMINAL_SN { get; set; }
            /// <summary>
            /// 类别id
            /// </summary>
            public string CATE_CODE { get; set; }
            /// <summary>
            /// 拼音首字母大写
            /// </summary>
            public string PINYINCODE { get; set; }

            /// <summary>
            /// 数据来源
            /// </summary>
            public string SOURCE { get; set; }
            /// <summary>
            /// 其他条件
            /// </summary>
            public string FILTER { get; set; }
        }
        public class GETGOODSLIST_OUT
        {

            public List<ITEM> ITEMLIST { get; set; }

            public string HIS_RTNXML { get; set; }

            /// <summary>
            /// 其他条件
            /// </summary>
            public string PARAMETERS { get; set; }
        }
        public class ITEM
        {
            /// <summary>
            /// 物品代码
            /// </summary>
            public string ITEM_CODE { get; set; }
            /// <summary>
            /// 物品名称
            /// </summary>
            public string ITEM_NAME { get; set; }
            /// <summary>
            /// 计价单位
            /// </summary>
            public string ITEM_UNIT { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public string ITEM_PRICE { get; set; }
            /// <summary>
            /// 医保自付比例
            /// </summary>
            public string YL_PREC { get; set; }
            /// <summary>
            /// 规格
            /// </summary>
            public string ITEM_GG { get; set; }
            /// <summary>
            /// 除外内容
            /// </summary>
            public string EXCEPT { get; set; }
            /// <summary>
            /// 批准文号
            /// </summary>
            public string APPR_NUM { get; set; }


        }
    }
}
