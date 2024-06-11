using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("loghos")]
	public class Loghos
	{
		/// <summary>
		/// UID
		/// </summary>
		[SugarColumn(ColumnName ="UID")]
		public string UID { get; set; }

		/// <summary>
		/// InTime
		/// </summary>
		[SugarColumn(ColumnName ="InTime")]
		public DateTime? InTime { get; set; }

		/// <summary>
		/// InXml
		/// </summary>
		[SugarColumn(ColumnName ="InXml")]
		public string InXml { get; set; }

		/// <summary>
		/// OutTime
		/// </summary>
		[SugarColumn(ColumnName ="OutTime")]
		public DateTime? OutTime { get; set; }

		/// <summary>
		/// OutXml
		/// </summary>
		[SugarColumn(ColumnName ="OutXml")]
		public string OutXml { get; set; }

		/// <summary>
		/// HOS_ID
		/// </summary>
		[SugarColumn(ColumnName ="HOS_ID")]
		public string HOS_ID { get; set; }

		/// <summary>
		/// TYPE
		/// </summary>
		[SugarColumn(ColumnName ="TYPE")]
		public string TYPE { get; set; }

		/// <summary>
		/// SFZ_NO
		/// </summary>
		[SugarColumn(ColumnName ="SFZ_NO")]
		public string SFZ_NO { get; set; }

		/// <summary>
		/// PAT_NAME
		/// </summary>
		[SugarColumn(ColumnName ="PAT_NAME")]
		public string PAT_NAME { get; set; }

	}
}

