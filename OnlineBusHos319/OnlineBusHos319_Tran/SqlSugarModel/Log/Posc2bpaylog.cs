using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("posc2bpaylog")]
	public class Posc2bpaylog
	{
		/// <summary>
		/// HOS_ID
		/// </summary>
		[SugarColumn(ColumnName ="HOS_ID")]
		public string HOS_ID { get; set; }

		/// <summary>
		/// tradetype
		/// </summary>
		[SugarColumn(ColumnName ="tradetype")]
		public string tradetype { get; set; }

		/// <summary>
		/// billno
		/// </summary>
		[SugarColumn(ColumnName ="billno")]
		public string billno { get; set; }

		/// <summary>
		/// indata
		/// </summary>
		[SugarColumn(ColumnName ="indata")]
		public string indata { get; set; }

		/// <summary>
		/// intime
		/// </summary>
		[SugarColumn(ColumnName ="intime")]
		public DateTime intime { get; set; }

		/// <summary>
		/// outdata
		/// </summary>
		[SugarColumn(ColumnName ="outdata")]
		public string outdata { get; set; }

		/// <summary>
		/// outtime
		/// </summary>
		[SugarColumn(ColumnName ="outtime")]
		public DateTime outtime { get; set; }

	}
}

