using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBPP.Model;
using System.Data;
namespace EBPP.DAL
{
   public  class DALorginfo
    {   /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(MOrginfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into orginfo(");
            strSql.Append("unifiedOrgCode,OrgName)");
            strSql.Append(" values (");
            strSql.Append("@unifiedOrgCode,@OrgName)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50)};
            
            parameters[0].Value = model.unifiedOrgCode;
            parameters[1].Value = model.OrgName;
          

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<MOrginfo>   GetModels()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select unifiedOrgCode,OrgName from orginfo ");

            List<MOrginfo> list = new List<MOrginfo>();
            Mebppmx model = new Mebppmx();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString() );
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(DataRowToModel(dr));

                }
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MOrginfo DataRowToModel(DataRow row)
        {
            MOrginfo model = new MOrginfo();
            if (row != null)
            {
           
                if (row["unifiedOrgCode"] != null)
                {
                    model.unifiedOrgCode = row["unifiedOrgCode"].ToString();
                }
                if (row["OrgName"] != null)
                {
                    model.OrgName = row["OrgName"].ToString();
                }
               
            }
            return model;
        }

    }
}
