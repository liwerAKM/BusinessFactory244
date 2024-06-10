using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBPP.Model;
using System.Data;

namespace EBPP.DAL
{
    /// <summary>
    /// 数据访问类:ebppmx2
    /// </summary>
    public partial class DALebppmx2
    {
        public DALebppmx2()
        { }
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Mebppmx2 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppmx2(");
            strSql.Append("EBPPID,Orde,ItemName,iBM,JSON,D1)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@Orde,@ItemName,@iBM,@JSON,@D1)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@Orde", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ItemName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@iBM", MySqlDbType.VarChar,50),
                    new MySqlParameter("@JSON", MySqlDbType.VarChar,1000),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.Orde;
            parameters[2].Value = model.ItemName;
            parameters[3].Value = model.iBM;
            parameters[4].Value = model.JSON;
            parameters[5].Value = model.D1;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(Mebppmx2 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ebppmx2 set ");
            strSql.Append("EBPPID=@EBPPID,");
            strSql.Append("Orde=@Orde,");
            strSql.Append("ItemName=@ItemName,");
            strSql.Append("iBM=@iBM,");
            strSql.Append("JSON=@JSON,");
            strSql.Append("D1=@D1");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.UInt64,20),
                    new MySqlParameter("@Orde", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ItemName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@iBM", MySqlDbType.VarChar,50),
                    new MySqlParameter("@JSON", MySqlDbType.VarChar,1000),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.Orde;
            parameters[2].Value = model.ItemName;
            parameters[3].Value = model.iBM;
            parameters[4].Value = model.JSON;
            parameters[5].Value = model.D1;

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
        /// 删除一条数据
        /// </summary>
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ebppmx2 ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
            };

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
        public Mebppmx2 GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EBPPID,Orde,ItemName,iBM,JSON,D1 from ebppmx2 ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
            };

            Mebppmx2 model = new Mebppmx2();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Mebppmx2 DataRowToModel(DataRow row)
        {
            Mebppmx2 model = new Mebppmx2();
            if (row != null)
            {
                if (row["EBPPID"] != null && row["EBPPID"].ToString() != "")
                {
                    model.EBPPID = long.Parse(row["EBPPID"].ToString());
                }
                if (row["Orde"] != null)
                {
                    model.Orde = row["Orde"].ToString();
                }
                if (row["ItemName"] != null)
                {
                    model.ItemName = row["ItemName"].ToString();
                }
                if (row["iBM"] != null)
                {
                    model.iBM = row["iBM"].ToString();
                }
                if (row["JSON"] != null)
                {
                    model.JSON = row["JSON"].ToString();
                }
                if (row["D1"] != null && row["D1"].ToString() != "")
                {
                    model.D1 = DateTime.Parse(row["D1"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EBPPID,Orde,ItemName,iBM,JSON,D1 ");
            strSql.Append(" FROM ebppmx2 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ebppmx2 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T. desc");
            }
            strSql.Append(")AS Row, T.*  from ebppmx2 T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			MySqlParameter[] parameters = {
					new MySqlParameter("@tblName", MySqlDbType.VarChar, 255),
					new MySqlParameter("@fldName", MySqlDbType.VarChar, 255),
					new MySqlParameter("@PageSize", MySqlDbType.Int32),
					new MySqlParameter("@PageIndex", MySqlDbType.Int32),
					new MySqlParameter("@IsReCount", MySqlDbType.Bit),
					new MySqlParameter("@OrderType", MySqlDbType.Bit),
					new MySqlParameter("@strWhere", MySqlDbType.VarChar,1000),
					};
			parameters[0].Value = "ebppmx2";
			parameters[1].Value = "";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperMySQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod
        public static void Add(Mebppmx2 model, MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppmx2(");
            strSql.Append("EBPPID,Orde,ItemName,iBM,JSON,D1)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@Orde,@ItemName,@iBM,@JSON,@D1)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@Orde", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ItemName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@iBM", MySqlDbType.VarChar,50),
                    new MySqlParameter("@JSON", MySqlDbType.VarChar,1000),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.Orde;
            parameters[2].Value = model.ItemName;
            parameters[3].Value = model.iBM;
            parameters[4].Value = model.JSON;
            parameters[5].Value = model.D1;

            DbHelperMySQL.PrepareCommand(cmd, conn, trans, strSql.ToString(), parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        #endregion  ExtensionMethod

    }
}
