using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("posc2b_tran")]
	public class Posc2bTran
	{
		/// <summary>
		/// COMM_SN
		/// </summary>
		[SugarColumn(ColumnName ="COMM_SN",IsPrimaryKey=true)]
		public string COMM_SN { get; set; }

		/// <summary>
		/// TXN_TYPE
		/// </summary>
		[SugarColumn(ColumnName ="TXN_TYPE")]
		public string TXN_TYPE { get; set; }

		/// <summary>
		/// HOS_ID
		/// </summary>
		[SugarColumn(ColumnName ="HOS_ID")]
		public string HOS_ID { get; set; }

		/// <summary>
		/// mid
		/// </summary>
		[SugarColumn(ColumnName ="mid")]
		public string mid { get; set; }

		/// <summary>
		/// tid
		/// </summary>
		[SugarColumn(ColumnName ="tid")]
		public string tid { get; set; }

		/// <summary>
		/// instMid
		/// </summary>
		[SugarColumn(ColumnName ="instMid")]
		public string instMid { get; set; }

		/// <summary>
		/// billNo
		/// </summary>
		[SugarColumn(ColumnName ="billNo")]
		public string billNo { get; set; }

		/// <summary>
		/// billQRCode
		/// </summary>
		[SugarColumn(ColumnName ="billQRCode")]
		public string billQRCode { get; set; }

		/// <summary>
		/// billDate
		/// </summary>
		[SugarColumn(ColumnName ="billDate")]
		public string billDate { get; set; }

		/// <summary>
		/// createTime
		/// </summary>
		[SugarColumn(ColumnName ="createTime")]
		public string createTime { get; set; }

		/// <summary>
		/// billStatus
		/// </summary>
		[SugarColumn(ColumnName ="billStatus")]
		public string billStatus { get; set; }

		/// <summary>
		/// billDesc
		/// </summary>
		[SugarColumn(ColumnName ="billDesc")]
		public string billDesc { get; set; }

		/// <summary>
		/// totalAmount
		/// </summary>
		[SugarColumn(ColumnName ="totalAmount")]
		public int? totalAmount { get; set; }

		/// <summary>
		/// merName
		/// </summary>
		[SugarColumn(ColumnName ="merName")]
		public string merName { get; set; }

		/// <summary>
		/// memo
		/// </summary>
		[SugarColumn(ColumnName ="memo")]
		public string memo { get; set; }

		/// <summary>
		/// notifyId
		/// </summary>
		[SugarColumn(ColumnName ="notifyId")]
		public string notifyId { get; set; }

		/// <summary>
		/// secureStatus
		/// </summary>
		[SugarColumn(ColumnName ="secureStatus")]
		public string secureStatus { get; set; }

		/// <summary>
		/// completeAmount
		/// </summary>
		[SugarColumn(ColumnName ="completeAmount")]
		public int? completeAmount { get; set; }

		/// <summary>
		/// merOrderId
		/// </summary>
		[SugarColumn(ColumnName ="merOrderId")]
		public string merOrderId { get; set; }

		/// <summary>
		/// refundOrderId
		/// </summary>
		[SugarColumn(ColumnName ="refundOrderId")]
		public string refundOrderId { get; set; }

		/// <summary>
		/// refundStatus
		/// </summary>
		[SugarColumn(ColumnName ="refundStatus")]
		public string refundStatus { get; set; }

		/// <summary>
		/// notify_time
		/// </summary>
		[SugarColumn(ColumnName ="notify_time")]
		public DateTime? notify_time { get; set; }

	}
}

