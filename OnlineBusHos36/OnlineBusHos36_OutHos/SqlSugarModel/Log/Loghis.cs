using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("loghis")]
	public class Loghis
	{
		/// <summary>
		/// Type
		/// </summary>
		[SugarColumn(ColumnName ="Type")]
		public string Type { get; set; }

		/// <summary>
		/// InXml
		/// </summary>
		[SugarColumn(ColumnName ="InXml")]
		public string InXml { get; set; }

		/// <summary>
		/// intime
		/// </summary>
		[SugarColumn(ColumnName ="intime")]
		public string intime { get; set; }

		/// <summary>
		/// OutXml
		/// </summary>
		[SugarColumn(ColumnName ="OutXml")]
		public string OutXml { get; set; }

		/// <summary>
		/// outtime
		/// </summary>
		[SugarColumn(ColumnName ="outtime")]
		public string outtime { get; set; }

		/// <summary>
		/// uid
		/// </summary>
		[SugarColumn(ColumnName ="uid")]
		public string uid { get; set; }

	}
}

