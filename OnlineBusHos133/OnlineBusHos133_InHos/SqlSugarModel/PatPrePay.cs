using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlSugarModel
{
    /// <summary>
    /// 预交金库
    /// </summary>

    [SugarTable("pat_prepay")]
    public class PatPrePay
    {
        /// <summary>
		/// PAT_ID
		/// </summary>
		[SugarColumn(ColumnName = "PAT_ID", IsPrimaryKey = true)]
        public int PAT_ID { get; set; }

        [SugarColumn(ColumnName = "PAY_ID")]
        public int PAY_ID { get; set; }

        [SugarColumn(ColumnName = "HOS_ID")]
        public string HOS_ID { get; set; }

        [SugarColumn(ColumnName = "HOS_PAT_ID")]
        public string HOS_PAT_ID { get; set; }

        [SugarColumn(ColumnName = "REGPAT_ID")]
        public int REGPAT_ID { get; set; }

        [SugarColumn(ColumnName = "HOS_PAY_SN")]
        public string HOS_PAY_SN { get; set; }

        [SugarColumn(ColumnName = "CASH_JE")]
        public decimal CASH_JE { get; set; }

        [SugarColumn(ColumnName = "DJ_TIME")]
        public DateTime DJ_TIME { get; set; }

        [SugarColumn(ColumnName = "lTERMINAL_SN")]
        public string lTERMINAL_SN { get; set; }

        [SugarColumn(ColumnName = "USER_ID")]
        public string USER_ID { get; set; }

 
    }
}
