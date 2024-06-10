using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using DB.Core;
namespace Soft.Core.SQL
{
    public class BaseQuery
    {
        public static bool InsertValid(string GUID)
        {
            try
            {
                string sqlcmd = string.Format(@"INSERT INTO `common`.`checkduprequest` (`GUID`) VALUES (@GUID)");

                MySqlParameter[] parameters = {
                    new MySqlParameter("@GUID", MySqlDbType.VarChar,32)         };
                parameters[0].Value = GUID;

                var data = DbHelperMySQLZZJ.ExecuteSql(sqlcmd,parameters);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
