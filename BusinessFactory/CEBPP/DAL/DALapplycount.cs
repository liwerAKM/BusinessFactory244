using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBPP.Model;
using System.Data;
namespace EBPP.DAL
{
   public  class DALapplycount
    {

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(DateTime ApplyDate, string unifiedOrgCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from applycount");
            strSql.Append(" where ApplyDate=@ApplyDate and unifiedOrgCode=@unifiedOrgCode ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ApplyDate", MySqlDbType.Date ),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20)           };
            parameters[0].Value = ApplyDate;
            parameters[1].Value = unifiedOrgCode;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Mapplycount model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into applycount(");
            strSql.Append("ApplyDate,unifiedOrgCode,GhCount,MzCount,ZyCount,QdCount)");
            strSql.Append(" values (");
            strSql.Append("@ApplyDate,@unifiedOrgCode,@GhCount,@MzCount,@ZyCount,@QdCount)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ApplyDate", MySqlDbType.Date),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@GhCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@MzCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@ZyCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@QdCount", MySqlDbType.Int32,11)};
            parameters[0].Value = model.ApplyDate;
            parameters[1].Value = model.unifiedOrgCode;
            parameters[2].Value = model.GhCount;
            parameters[3].Value = model.MzCount;
            parameters[4].Value = model.ZyCount;
            parameters[5].Value = model.QdCount;

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
        public bool Update(Mapplycount model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update applycount set ");
            strSql.Append("GhCount=@GhCount,");
            strSql.Append("MzCount=@MzCount,");
            strSql.Append("ZyCount=@ZyCount,");
            strSql.Append("QdCount=@QdCount");
            strSql.Append(" where ApplyDate=@ApplyDate and unifiedOrgCode=@unifiedOrgCode ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@GhCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@MzCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@ZyCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@QdCount", MySqlDbType.Int32,11),
                    new MySqlParameter("@ApplyDate", MySqlDbType.Date ),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20)};
            parameters[0].Value = model.GhCount;
            parameters[1].Value = model.MzCount;
            parameters[2].Value = model.ZyCount;
            parameters[3].Value = model.QdCount;
            parameters[4].Value = model.ApplyDate;
            parameters[5].Value = model.unifiedOrgCode;

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
        public bool Delete(DateTime ApplyDate, string unifiedOrgCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from applycount ");
            strSql.Append(" where ApplyDate=@ApplyDate and unifiedOrgCode=@unifiedOrgCode ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ApplyDate", MySqlDbType.Date ),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20)           };
            parameters[0].Value = ApplyDate;
            parameters[1].Value = unifiedOrgCode;

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
        public Mapplycount GetModel(DateTime ApplyDate, string unifiedOrgCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ApplyDate,unifiedOrgCode,GhCount,MzCount,ZyCount,QdCount from applycount ");
            strSql.Append(" where ApplyDate=@ApplyDate and unifiedOrgCode=@unifiedOrgCode ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ApplyDate", MySqlDbType.Date),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20)           };
            parameters[0].Value = ApplyDate;
            parameters[1].Value = unifiedOrgCode;

            Mapplycount model = new Mapplycount();
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
        public Mapplycount DataRowToModel(DataRow row)
        {
            Mapplycount model = new Mapplycount();
            if (row != null)
            {
                if (row["ApplyDate"] != null && row["ApplyDate"].ToString() != "")
                {
                    model.ApplyDate = DateTime.Parse(row["ApplyDate"].ToString());
                }
                if (row["unifiedOrgCode"] != null)
                {
                    model.unifiedOrgCode = row["unifiedOrgCode"].ToString();
                }
                if (row["GhCount"] != null && row["GhCount"].ToString() != "")
                {
                    model.GhCount = int.Parse(row["GhCount"].ToString());
                }
                if (row["MzCount"] != null && row["MzCount"].ToString() != "")
                {
                    model.MzCount = int.Parse(row["MzCount"].ToString());
                }
                if (row["ZyCount"] != null && row["ZyCount"].ToString() != "")
                {
                    model.ZyCount = int.Parse(row["ZyCount"].ToString());
                }
                if (row["QdCount"] != null && row["QdCount"].ToString() != "")
                {
                    model.QdCount = int.Parse(row["QdCount"].ToString());
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
            strSql.Append("select ApplyDate,unifiedOrgCode,GhCount,MzCount,ZyCount,QdCount ");
            strSql.Append(" FROM applycount ");
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
            strSql.Append("select count(1) FROM applycount ");
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
                strSql.Append("order by T.unifiedOrgCode desc");
            }
            strSql.Append(")AS Row, T.*  from applycount T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }




        #endregion  BasicMethod

        /// <summary>
        /// 获取上传数据 按上传时间
        /// </summary>
        public    List < MapplycountQ>  GetListQ(DateTime StartDate, DateTime EndDate, string unifiedOrgCode)
        {
            List<MapplycountQ> list = new List<MapplycountQ>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select  a.*,b.OrgName
from applycount a
left outer join orginfo b
on a.unifiedOrgCode=b.unifiedOrgCode ");
            if(EndDate!=null&& EndDate.Year !=1)
                strSql.Append(" where a.ApplyDate between @StartDate  and @EndDate ");
            else
                strSql.Append(" where a.ApplyDate = @StartDate   ");
            if (!string.IsNullOrEmpty(unifiedOrgCode))
            {
                strSql.Append("   and a.unifiedOrgCode=@unifiedOrgCode ");
            }
          
            MySqlParameter[] parameters = {
                    new MySqlParameter("@StartDate", MySqlDbType.Date),
                     new MySqlParameter("@EndDate", MySqlDbType.Date),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20)           };
            parameters[0].Value = StartDate;
            parameters[1].Value = EndDate;
            parameters[2].Value = unifiedOrgCode;

            Mapplycount model = new Mapplycount();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    list.Add(DataRowToModelQ(dr));
            }
            
            return list;
        }
        /// <summary>
        /// 获取上传数据 按发票生成时间
        /// </summary>
        public List<MapplycountQ> GetListQOpen(DateTime StartDate, DateTime EndDate, string unifiedOrgCode)
        {
            List<MapplycountQ> list = new List<MapplycountQ>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select  a.*,b.OrgName
from applycountopen a
left outer join orginfo b
on a.unifiedOrgCode=b.unifiedOrgCode ");
            if (EndDate != null && EndDate.Year != 1)
                strSql.Append(" where a.Opendate between @StartDate  and @EndDate ");
            else
                strSql.Append(" where a.Opendate = @StartDate  ");
            if (!string.IsNullOrEmpty(unifiedOrgCode))
            {
                strSql.Append("   and a.unifiedOrgCode=@unifiedOrgCode ");
            }

            MySqlParameter[] parameters = {
                    new MySqlParameter("@StartDate", MySqlDbType.Date),
                     new MySqlParameter("@EndDate", MySqlDbType.Date),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20)           };
            parameters[0].Value = StartDate;
            parameters[1].Value = EndDate;
            parameters[2].Value = unifiedOrgCode;

            Mapplycount model = new Mapplycount();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    list.Add(DataRowToModelQ(dr));
            }

            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MapplycountQ DataRowToModelQ(DataRow row)
        {
            MapplycountQ model = new MapplycountQ();
            if (row != null)
            {
                if (row.Table.Columns.Contains("ApplyDate")&&row["ApplyDate"] != null && row["ApplyDate"].ToString() != "")
                {
                    model.Date =((DateTime) row["ApplyDate"]).ToString("yyyy-MM-dd");
                }
                else if (row.Table.Columns.Contains("OpenDate") && row["OpenDate"] != null && row["OpenDate"].ToString() != "")
                {
                    model.Date = ((DateTime)row["OpenDate"]).ToString("yyyy-MM-dd");
                }
                if (row["unifiedOrgCode"] != null)
                {
                    model.unifiedOrgCode = row["unifiedOrgCode"].ToString();
                }
                if (row["GhCount"] != null && row["GhCount"].ToString() != "")
                {
                    model.GhCount = int.Parse(row["GhCount"].ToString());
                }
                if (row["MzCount"] != null && row["MzCount"].ToString() != "")
                {
                    model.MzCount = int.Parse(row["MzCount"].ToString());
                }
                if (row["ZyCount"] != null && row["ZyCount"].ToString() != "")
                {
                    model.ZyCount = int.Parse(row["ZyCount"].ToString());
                }
                if (row["QdCount"] != null && row["QdCount"].ToString() != "")
                {
                    model.QdCount = int.Parse(row["QdCount"].ToString());
                }
                if (row["OrgName"] != null && row["OrgName"].ToString() != "")
                {
                    model.OrgName =row["OrgName"].ToString();
                }
            }
            return model;
        }



    }
}
