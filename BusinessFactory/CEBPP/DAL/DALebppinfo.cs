using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBPP.Model;
using System.Data;

namespace EBPP.DAL
{
   public  class DALebppinfo
    {
        public DALebppinfo()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long EBPPID, short  DataType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ebppinfo");
            strSql.Append(" where EBPPID=@EBPPID and DataType=@DataType ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@DataType", MySqlDbType.Int16 ,10)         };
            parameters[0].Value = EBPPID;
            parameters[1].Value = DataType;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }
      

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Mebppinfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppinfo(");
            strSql.Append("EBPPID,DataType,JSON,D1)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@DataType,@JSON,@D1)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@DataType", MySqlDbType.Int16,10),
                    new MySqlParameter("@JSON", MySqlDbType.VarChar,2000),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.DataType;
            parameters[2].Value = model.JSON;
            parameters[3].Value = model.D1;

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
        public bool Update(Mebppinfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ebppinfo set ");
            strSql.Append("JSON=@JSON,");
            strSql.Append("D1=@D1");
            strSql.Append(" where EBPPID=@EBPPID and DataType=@DataType ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@JSON", MySqlDbType.VarChar,2000),
                    new MySqlParameter("@D1", MySqlDbType.DateTime),
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@DataType", MySqlDbType.Int16,10)};
            parameters[0].Value = model.JSON;
            parameters[1].Value = model.D1;
            parameters[2].Value = model.EBPPID;
            parameters[3].Value = model.DataType;

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
        public bool Delete(long EBPPID, Int16 DataType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ebppinfo ");
            strSql.Append(" where EBPPID=@EBPPID and DataType=@DataType ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@DataType", MySqlDbType.Int16,10)         };
            parameters[0].Value = EBPPID;
            parameters[1].Value = DataType;

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
        public Mebppinfo GetModel(long EBPPID, Int16 DataType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EBPPID,DataType,JSON,D1 from ebppinfo ");
            strSql.Append(" where EBPPID=@EBPPID and DataType=@DataType ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@DataType", MySqlDbType.Int16,10)         };
            parameters[0].Value = EBPPID;
            parameters[1].Value = DataType;

            Mebppinfo model = new Mebppinfo();
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
        public Mebppinfo DataRowToModel(DataRow row)
        {
            Mebppinfo model = new Mebppinfo();
            if (row != null)
            {
                if (row["EBPPID"] != null && row["EBPPID"].ToString() != "")
                {
                    model.EBPPID = long.Parse(row["EBPPID"].ToString());
                }
                if (row["DataType"] != null)
                {
                    model.DataType = short.Parse(row["DataType"].ToString());
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
            strSql.Append("select EBPPID,DataType,JSON,D1 ");
            strSql.Append(" FROM ebppinfo ");
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
            strSql.Append("select count(1) FROM ebppinfo ");
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
                strSql.Append("order by T.DataType desc");
            }
            strSql.Append(")AS Row, T.*  from ebppinfo T ");
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
			parameters[0].Value = "ebppinfo";
			parameters[1].Value = "DataType";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperMySQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void Add(Mebppinfo model, MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppinfo(");
            strSql.Append("EBPPID,DataType,JSON,D1)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@DataType,@JSON,@D1)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@DataType", MySqlDbType.Int16,10),
                    new MySqlParameter("@JSON", MySqlDbType.VarChar,2000),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.DataType;
            parameters[2].Value = model.JSON;
            parameters[3].Value = model.D1;

            DbHelperMySQL.PrepareCommand(cmd, conn, trans, strSql.ToString(), parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        #endregion  ExtensionMethod
    }

}
