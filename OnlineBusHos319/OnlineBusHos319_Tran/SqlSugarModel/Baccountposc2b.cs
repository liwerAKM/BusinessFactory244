using System;
using SqlSugar;

namespace SqlSugarModel
{
	[SugarTable("baccountposc2b")]
	public class Baccountposc2b
	{
		/// <summary>
		/// HOS_ID
		/// </summary>
		[SugarColumn(ColumnName ="HOS_ID",IsPrimaryKey=true)]
		public string HOS_ID { get; set; }

		/// <summary>
		/// msgSrc
		/// </summary>
		[SugarColumn(ColumnName ="msgSrc")]
		public string msgSrc { get; set; }

		/// <summary>
		/// msgSrcId
		/// </summary>
		[SugarColumn(ColumnName ="msgSrcId")]
		public string msgSrcId { get; set; }

		/// <summary>
		/// md5Key
		/// </summary>
		[SugarColumn(ColumnName ="md5Key")]
		public string md5Key { get; set; }

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
		/// appid
		/// </summary>
		[SugarColumn(ColumnName ="appid")]
		public string appid { get; set; }

		/// <summary>
		/// appkey
		/// </summary>
		[SugarColumn(ColumnName ="appkey")]
		public string appkey { get; set; }

		/// <summary>
		/// d1
		/// </summary>
		[SugarColumn(ColumnName ="d1")]
		public DateTime? d1 { get; set; }

		/// <summary>
		/// d2
		/// </summary>
		[SugarColumn(ColumnName ="d2")]
		public DateTime? d2 { get; set; }

		/// <summary>
		/// apiURL
		/// </summary>
		[SugarColumn(ColumnName ="apiURL")]
		public string apiURL { get; set; }

	}
}

