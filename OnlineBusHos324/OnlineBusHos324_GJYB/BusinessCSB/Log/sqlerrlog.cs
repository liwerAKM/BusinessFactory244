using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace OnlineBusHos324_GJYB
{
	/// <summary>
	/// sqlerrlog:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sqlerrlog
	{
		public sqlerrlog()
		{}
		#region Model
		private string _type;
		private DateTime? _intime;
		private string _exceptionmessage;
		/// <summary>
		/// 
		/// </summary>
		public string TYPE
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? inTime
		{
			set{ _intime=value;}
			get{return _intime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ExceptionMessage
		{
			set{ _exceptionmessage=value;}
			get{return _exceptionmessage;}
		}
		#endregion Model

	}
}

