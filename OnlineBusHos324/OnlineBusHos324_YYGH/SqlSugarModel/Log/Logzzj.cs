using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("logzzj")]
	public class Logzzj
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
		/// type
		/// </summary>
		[SugarColumn(ColumnName ="type")]
		public string type { get; set; }

	}
}

