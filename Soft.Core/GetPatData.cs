using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using DB.Core;
using PasS.Base.Lib;

namespace Soft.Core
{
    public class GetPatData
    {
        public static DataTable GetPatInfo(int PAT_ID)
        {
            string sqlcmd = string.Format(@"SELECT MOBILE_SECRET,SFZ_SECRET,G_SFZ_SECRET from pat_info where pat_id=@pat_id ");
            MySqlParameter[] parameters = { new MySqlParameter("@pat_id", MySqlDbType.VarChar, 100) };
            parameters[0].Value = PAT_ID;
            DataTable dtPat = DbHelperMySQL.Query(sqlcmd, parameters).Tables[0];

            return dtPat;
        }
        public static Model.PAT_INFO BasePatInfo(int PAT_ID)
        {
            if (DbHelper.SysInfoGet("CenterSyncDataUrl") == "")
            {
                Model.PAT_INFO pAT_INFO = new SQL.PAT_INFO().GetModel(PAT_ID);
                return pAT_INFO;
            }
            else
            {

                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("PAT_ID", PAT_ID.ToString());

                string data = slbCall.GetPostData(JSONSerializer.Serialize(dic), "PatInfoBaseAPI", "GETPATINFO");
                Model.PAT_INFO pAT_INFO = JSONSerializer.Deserialize<Model.PAT_INFO>(data);

                return pAT_INFO;
            }

        }
    }
}
