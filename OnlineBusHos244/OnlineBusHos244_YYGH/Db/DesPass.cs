﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBusHos244_YYGH
{
    class DesPass
    {
		/// <summary>
		/// 将MySQL配置文件参数的密码Password 从密码改为明文
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		internal static string DecryptMysqlConfigPwd(string connectionString)
		{
			string _connectionString = connectionString;
			try
			{
				string[] cons = _connectionString.Split(';');
				foreach (string con in cons)
				{
					string tcon = con.Trim();
					if (tcon.StartsWith("password", StringComparison.CurrentCultureIgnoreCase))
					{
						string[] tcons = tcon.Split('=');
						string pwd = tcons[1];
						pwd = pwd.Trim('\'');
						string pwdmw =PasS.Base.Lib.DESEncrypt.Decrypt(pwd);
						_connectionString = _connectionString.Replace(pwd, pwdmw);
						break;
					}
				}
			}
			catch (Exception ex)
			{
			}
			return _connectionString;
		}
	}
}
