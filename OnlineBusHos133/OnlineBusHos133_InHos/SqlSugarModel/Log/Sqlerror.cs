using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("sqlerror")]
	public class Sqlerror
	{
		/// <summary>
		/// TYPE
		/// </summary>
		[SugarColumn(ColumnName ="TYPE")]
		public string TYPE { get; set; }

		/// <summary>
		/// Exception
		/// </summary>
		[SugarColumn(ColumnName ="Exception")]
		public string Exception { get; set; }

		/// <summary>
		/// DateTime
		/// </summary>
		[SugarColumn(ColumnName ="DateTime")]
		public DateTime? DateTime { get; set; }

	}
}

