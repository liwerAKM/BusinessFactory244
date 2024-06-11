using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace OnlineBusHos244_GJYB
{
	/// <summary>
	/// platlog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class platlog
	{
		public platlog()
		{}
		#region Model
		private string _msgid;
		private string _hos_id;
		private string _infno;
		private DateTime? _intime;
		private string _indata;
		private DateTime? _outtime;
		private string _outdata;
		private string _outcode;
		private string _opter_no;
		/// <summary>
		/// 
		/// </summary>
		public string msgid
		{
			set{ _msgid = value;}
			get{return _msgid; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string HOS_ID
		{
			set{ _hos_id=value;}
			get{return _hos_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Infno
		{
			set{ _infno=value;}
			get{return _infno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? InTime
		{
			set{ _intime=value;}
			get{return _intime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string InData
		{
			set{ _indata=value;}
			get{return _indata;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? OutTime
		{
			set{ _outtime=value;}
			get{return _outtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OutData
		{
			set{ _outdata=value;}
			get{return _outdata;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OutCode
		{
			set{ _outcode=value;}
			get{return _outcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Opter_no
		{
			set{ _opter_no=value;}
			get{return _opter_no;}
		}
		#endregion Model

	}
}

