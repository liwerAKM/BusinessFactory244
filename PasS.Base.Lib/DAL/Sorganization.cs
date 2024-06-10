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
    /// 数据访问类:sorganization
    /// </summary>
    public partial class Sorganization
    {
        public Sorganization()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string ORG_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from sorganization");
            strSql.Append(" where ORG_ID=@ORG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10)           };
            parameters[0].Value = ORG_ID;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Ssorganization model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sorganization(");
            strSql.Append("ORG_ID,org_name,PORG_ID,ORG_Type,Mark_Stop,ADD_TIME,STOP_TIME)");
            strSql.Append(" values (");
            strSql.Append("@ORG_ID,@org_name,@PORG_ID,@ORG_Type,@Mark_Stop,@ADD_TIME,@STOP_TIME)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@org_name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@PORG_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ORG_Type", MySqlDbType.Int32,11),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
                    new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime)};
            parameters[0].Value = model.ORG_ID;
            parameters[1].Value = model.org_name;
            parameters[2].Value = model.PORG_ID;
            parameters[3].Value = model.ORG_Type;
            parameters[4].Value = model.Mark_Stop;
            parameters[5].Value = model.ADD_TIME;
            parameters[6].Value = model.STOP_TIME;

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
        public bool Update(Ssorganization model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update sorganization set ");
            strSql.Append("org_name=@org_name,");
            strSql.Append("PORG_ID=@PORG_ID,");
            strSql.Append("ORG_Type=@ORG_Type,");
            strSql.Append("Mark_Stop=@Mark_Stop,");
            strSql.Append("ADD_TIME=@ADD_TIME,");
            strSql.Append("STOP_TIME=@STOP_TIME");
            strSql.Append(" where ORG_ID=@ORG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@org_name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@PORG_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ORG_Type", MySqlDbType.Int32,11),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
                    new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.org_name;
            parameters[1].Value = model.PORG_ID;
            parameters[2].Value = model.ORG_Type;
            parameters[3].Value = model.Mark_Stop;
            parameters[4].Value = model.ADD_TIME;
            parameters[5].Value = model.STOP_TIME;
            parameters[6].Value = model.ORG_ID;

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
        public bool Delete(string ORG_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from sorganization ");
            strSql.Append(" where ORG_ID=@ORG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10)           };
            parameters[0].Value = ORG_ID;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string ORG_IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from sorganization ");
            strSql.Append(" where ORG_ID in (" + ORG_IDlist + ")  ");
            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString());
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
        public Ssorganization GetModel(string ORG_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ORG_ID,org_name,PORG_ID,ORG_Type,Mark_Stop,ADD_TIME,STOP_TIME from sorganization ");
            strSql.Append(" where ORG_ID=@ORG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10)           };
            parameters[0].Value = ORG_ID;

            Ssorganization model = new Ssorganization();
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
        /// 获取父机构ID
        /// </summary>
        public string  GetPORG_ID(string ORG_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PORG_ID from sorganization ");
            strSql.Append(" where ORG_ID=@ORG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10)           };
            parameters[0].Value = ORG_ID;

            Ssorganization model = new Ssorganization();
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["PORG_ID"].ToString() ;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取子机构列表
        /// </summary>
        public DataTable  GetChildORG(string ORG_ID,bool Contain_Stop)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ORG_ID,org_name,PORG_ID,ORG_Type,Mark_Stop,ADD_TIME,STOP_TIME  from sorganization ");
            strSql.Append(" where ORG_ID=@ORG_ID ");
            if (!Contain_Stop)
            {
                strSql.Append(" and a.Mark_Stop=0 ");
            }
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10)           };
            parameters[0].Value = ORG_ID;

            Ssorganization model = new Ssorganization();
            DataTable  dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];
            return dt;
        }


        


    /// <summary>
    /// 得到一个对象实体
    /// </summary>
    public Ssorganization DataRowToModel(DataRow row)
        {
            Ssorganization model = new Ssorganization();
            if (row != null)
            {
                if (row["ORG_ID"] != null)
                {
                    model.ORG_ID = row["ORG_ID"].ToString();
                }
                if (row["org_name"] != null)
                {
                    model.org_name = row["org_name"].ToString();
                }
                if (row["PORG_ID"] != null)
                {
                    model.PORG_ID = row["PORG_ID"].ToString();
                }
                if (row["ORG_Type"] != null && row["ORG_Type"].ToString() != "")
                {
                    model.ORG_Type = int.Parse(row["ORG_Type"].ToString());
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
                if (row["ADD_TIME"] != null && row["ADD_TIME"].ToString() != "")
                {
                    model.ADD_TIME = DateTime.Parse(row["ADD_TIME"].ToString());
                }
                if (row["STOP_TIME"] != null && row["STOP_TIME"].ToString() != "")
                {
                    model.STOP_TIME = DateTime.Parse(row["STOP_TIME"].ToString());
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
            strSql.Append("select ORG_ID,org_name,PORG_ID,ORG_Type,Mark_Stop,ADD_TIME,STOP_TIME ");
            strSql.Append(" FROM sorganization ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }
        public DataSet GetID_Name(string strWhere, bool addALL)
        {
            StringBuilder strSql = new StringBuilder();
            if (addALL)
            {
                strSql.Append(@"select '-1' ORG_ID,'全部机构'org_name
    union ");

            }
            strSql.Append(" select ORG_ID,org_name ");
            strSql.Append(" FROM sorganization ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
                strSql.Append("order by T.ORG_ID desc");
            }
            strSql.Append(")AS Row, T.*  from sorganization T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }


        #endregion  BasicMethod

        public DataSet GetID_Name(int sys_ID, bool Contain_Stop, bool addALL)
        {
            StringBuilder strSql = new StringBuilder();
            if (addALL)
            {
                strSql.Append(@"select '-1' ORG_ID,'全部机构'org_name
    union ");

            }
            if (sys_ID == -1)
            {
                strSql.Append(string.Format(@"select DISTINCT a.ORG_ID,org_name  FROM sorganization a
join sorgandsys b on a.ORG_ID = b.ORG_ID ", sys_ID));
                if (!Contain_Stop)
                {
                    strSql.Append("  where a.Mark_Stop=0 ");
                }
            }
            else
            {
                strSql.Append(string.Format(@" select DISTINCT a.ORG_ID,org_name  FROM sorganization a
join sorgandsys b on a.ORG_ID = b.ORG_ID
where b.SYSID = {0}  ", sys_ID));
                if (!Contain_Stop)
                {
                    strSql.Append(" and a.Mark_Stop=0 ");
                }
            }

            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

       

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListandTypeName(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ORG_ID,org_name,PORG_ID,ORG_Type,Mark_Stop,ADD_TIME,STOP_TIME,Code_name ");
            strSql.Append(" FROM sorganization join allcodeint on sorganization.ORG_Type=allcodeint.Code_id and allcodeint.All_lbmc ='OrganizationType'  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }
      
    }
}
