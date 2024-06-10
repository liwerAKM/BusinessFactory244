using PasS.Base.Lib.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib.DAL
{
   
    /// <summary>
	/// 数据访问类:ssysteminfo
	/// </summary>
	public partial class Ssysteminfo
    {
        public Ssysteminfo()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQLMySpring.GetMaxID("SYSID", "ssysteminfo");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int SYSID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ssysteminfo");
            strSql.Append(" where SYSID=@SYSID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@SYSID", MySqlDbType.Int32,11)          };
            parameters[0].Value = SYSID;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Sssysteminfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ssysteminfo(");
            strSql.Append("SYSID,SYS_NAME,Mark_Stop,SYS_Type,ADD_TIME,STOP_TIME,Version,Note)");
            strSql.Append(" values (");
            strSql.Append("@SYSID,@SYS_NAME,@Mark_Stop,@SYS_Type,@ADD_TIME,@STOP_TIME,@Version,@Note)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@SYSID", MySqlDbType.Int32,11),
                    new MySqlParameter("@SYS_NAME", MySqlDbType.VarChar,10),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@SYS_Type", MySqlDbType.Int32,11),
                    new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
                    new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,30),
                    new MySqlParameter("@Note", MySqlDbType.VarChar,50)};
            parameters[0].Value = model.SYSID;
            parameters[1].Value = model.SYS_NAME;
            parameters[2].Value = model.Mark_Stop;
            parameters[3].Value = model.SYS_Type;
            parameters[4].Value = model.ADD_TIME;
            parameters[5].Value = model.STOP_TIME;
            parameters[6].Value = model.Version;
            parameters[7].Value = model.Note;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Update(Sssysteminfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ssysteminfo set ");
            strSql.Append("SYS_NAME=@SYS_NAME,");
            strSql.Append("Mark_Stop=@Mark_Stop,");
            strSql.Append("SYS_Type=@SYS_Type,");
            strSql.Append("ADD_TIME=@ADD_TIME,");
            strSql.Append("STOP_TIME=@STOP_TIME,");
            strSql.Append("Version=@Version,");
            strSql.Append("Note=@Note");
            strSql.Append(" where SYSID=@SYSID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@SYS_NAME", MySqlDbType.VarChar,10),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@SYS_Type", MySqlDbType.Int32,11),
                    new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
                    new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,30),
                    new MySqlParameter("@Note", MySqlDbType.VarChar,50),
                    new MySqlParameter("@SYSID", MySqlDbType.Int32,11)};
            parameters[0].Value = model.SYS_NAME;
            parameters[1].Value = model.Mark_Stop;
            parameters[2].Value = model.SYS_Type;
            parameters[3].Value = model.ADD_TIME;
            parameters[4].Value = model.STOP_TIME;
            parameters[5].Value = model.Version;
            parameters[6].Value = model.Note;
            parameters[7].Value = model.SYSID;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(int SYSID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ssysteminfo ");
            strSql.Append(" where SYSID=@SYSID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@SYSID", MySqlDbType.Int32,11)          };
            parameters[0].Value = SYSID;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public Sssysteminfo GetModel(int SYSID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SYSID,SYS_NAME,Mark_Stop,SYS_Type,ADD_TIME,STOP_TIME,Version,Note from ssysteminfo ");
            strSql.Append(" where SYSID=@SYSID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@SYSID", MySqlDbType.Int32,11)          };
            parameters[0].Value = SYSID;

            Sssysteminfo model = new Sssysteminfo();
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
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
        public Sssysteminfo DataRowToModel(DataRow row)
        {
            Sssysteminfo model = new Sssysteminfo();
            if (row != null)
            {
                if (row["SYSID"] != null && row["SYSID"].ToString() != "")
                {
                    model.SYSID = int.Parse(row["SYSID"].ToString());
                }
                if (row["SYS_NAME"] != null)
                {
                    model.SYS_NAME = row["SYS_NAME"].ToString();
                }
                if (row["Mark_Stop"] != null && row["Mark_Stop"].ToString() != "")
                {
                    if ((row["Mark_Stop"].ToString() == "1") || (row["Mark_Stop"].ToString().ToLower() == "true"))
                    {
                        model.Mark_Stop = true;
                    }
                    else
                    {
                        model.Mark_Stop = false;
                    }
                }
                if (row["SYS_Type"] != null && row["SYS_Type"].ToString() != "")
                {
                    model.SYS_Type = int.Parse(row["SYS_Type"].ToString());
                }
                if (row["ADD_TIME"] != null && row["ADD_TIME"].ToString() != "")
                {
                    model.ADD_TIME = DateTime.Parse(row["ADD_TIME"].ToString());
                }
                if (row["STOP_TIME"] != null && row["STOP_TIME"].ToString() != "")
                {
                    model.STOP_TIME = DateTime.Parse(row["STOP_TIME"].ToString());
                }
                if (row["Version"] != null)
                {
                    model.Version = row["Version"].ToString();
                }
                if (row["Note"] != null)
                {
                    model.Note = row["Note"].ToString();
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
            strSql.Append("select SYSID,SYS_NAME,Mark_Stop,SYS_Type,ADD_TIME,STOP_TIME,Version,Note ");
            strSql.Append(" FROM ssysteminfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListandTypeName(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SYSID,SYS_NAME,Mark_Stop,SYS_Type,ADD_TIME,STOP_TIME,Version,Note,Code_name ");
            
            strSql.Append(" FROM ssysteminfo join allcodeint on ssysteminfo.SYS_Type=allcodeint.Code_id and allcodeint.All_lbmc ='SystemType'  ");
            if (strWhere.Trim() != "")
                if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取 SYSID SYS_NAME 列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="addALL">是否显示‘全部系统’</param>
        /// <returns></returns>
        public DataSet GetID_Name(string strWhere,bool addALL)
        {
            StringBuilder strSql = new StringBuilder();
            if (addALL)
            {
                strSql.Append(@"select - 1 SYSID,'全部系统'SYS_NAME
    union");
                
            }
          

            strSql.Append(" select  SYSID, SYS_NAME FROM ssysteminfo  ");
            if (strWhere.Trim() != "")
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        public DataSet GetID_Name(string ORG_ID, bool Contain_Stop, bool addALL)
        {
            StringBuilder strSql = new StringBuilder();
            if (addALL)
            {
                strSql.Append(@"select - 1 SYSID,'全部系统'SYS_NAME
    union ");

            }
            if (ORG_ID == "-1")
            {
                strSql.Append(string.Format(@"select DISTINCT a.SYSID,SYS_NAME  FROM ssysteminfo a
join sorgandsys b on a.SYSID = b.SYSID "));
                if (!Contain_Stop)
                {
                    strSql.Append("  where a.Mark_Stop=0 ");
                }
            }
            else
            {
                strSql.Append(string.Format(@"  select DISTINCT a.SYSID,SYS_NAME  FROM ssysteminfo a
join sorgandsys b on a.SYSID = b.SYSID
where b.ORG_ID = '{0}'  ", ORG_ID));
                if (!Contain_Stop)
                {
                    strSql.Append(" and a.Mark_Stop=0 ");
                }
            }

           
            return DbHelperMySQLMySpring.Query(strSql.ToString());
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
                strSql.Append("order by T.SYSID desc");
            }
            strSql.Append(")AS Row, T.*  from ssysteminfo T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        

        #endregion  BasicMethod
        
    }
}
