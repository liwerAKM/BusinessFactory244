using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("baccountposc2btid")]
	public class Baccountposc2btid
	{
		/// <summary>
		/// HOS_ID
		/// </summary>
		[SugarColumn(ColumnName ="HOS_ID",IsPrimaryKey=true)]
		public string HOS_ID { get; set; }

		/// <summary>
		/// tid
		/// </summary>
		[SugarColumn(ColumnName ="tid")]
		public string tid { get; set; }

		/// <summary>
		/// LTERMINAL_SN
		/// </summary>
		[SugarColumn(ColumnName ="LTERMINAL_SN")]
		public string LTERMINAL_SN { get; set; }

		/// <summary>
		/// USER_ID
		/// </summary>
		[SugarColumn(ColumnName ="USER_ID")]
		public string USER_ID { get; set; }

	}
}

