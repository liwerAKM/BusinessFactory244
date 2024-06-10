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
	/// 数据访问类:apigroup
	/// </summary>
	public partial class Apigroup
    {
        public Apigroup()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQLMySpring.GetMaxID("APIG_ID", "apigroup");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int APIG_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from apigroup");
            strSql.Append(" where APIG_ID=@APIG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIG_ID", MySqlDbType.Int32,4)         };
            parameters[0].Value = APIG_ID;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SApigroup model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into apigroup(");
            strSql.Append("APIG_ID,APIG_Name,Mark_Stop,APIG_Note,APIG_Type,SYSID,Add_Time,Stop_Time,AddUser_ID)");
            strSql.Append(" values (");
            strSql.Append("@APIG_ID,@APIG_Name,@Mark_Stop,@APIG_Note,@APIG_Type,@SYSID,@Add_Time,@Stop_Time,@AddUser_ID)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIG_ID", MySqlDbType.Int32,4),
                    new MySqlParameter("@APIG_Name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@APIG_Note", MySqlDbType.VarChar,100),
                    new MySqlParameter("@APIG_Type", MySqlDbType.Int32,4),
                    new MySqlParameter("@SYSID", MySqlDbType.Int32,4),
                    new MySqlParameter("@Add_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@Stop_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@AddUser_ID", MySqlDbType.VarChar,20)};
            parameters[0].Value = model.APIG_ID;
            parameters[1].Value = model.APIG_Name;
            parameters[2].Value = model.Mark_Stop;
            parameters[3].Value = model.APIG_Note;
            parameters[4].Value = model.APIG_Type;
            parameters[5].Value = model.SYSID;
            parameters[6].Value = model.Add_Time;
            parameters[7].Value = model.Stop_Time;
            parameters[8].Value = model.AddUser_ID;

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
        public bool Update(SApigroup model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update apigroup set ");
            strSql.Append("APIG_Name=@APIG_Name,");
            strSql.Append("Mark_Stop=@Mark_Stop,");
            strSql.Append("APIG_Note=@APIG_Note,");
            strSql.Append("APIG_Type=@APIG_Type,");
            strSql.Append("SYSID=@SYSID,");
            strSql.Append("Add_Time=@Add_Time,");
            strSql.Append("Stop_Time=@Stop_Time,");
            strSql.Append("AddUser_ID=@AddUser_ID");
            strSql.Append(" where APIG_ID=@APIG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIG_Name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@APIG_Note", MySqlDbType.VarChar,100),
                    new MySqlParameter("@APIG_Type", MySqlDbType.Int32,4),
                    new MySqlParameter("@SYSID", MySqlDbType.Int32,4),
                    new MySqlParameter("@Add_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@Stop_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@AddUser_ID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@APIG_ID", MySqlDbType.Int32,4)};
            parameters[0].Value = model.APIG_Name;
            parameters[1].Value = model.Mark_Stop;
            parameters[2].Value = model.APIG_Note;
            parameters[3].Value = model.APIG_Type;
            parameters[4].Value = model.SYSID;
            parameters[5].Value = model.Add_Time;
            parameters[6].Value = model.Stop_Time;
            parameters[7].Value = model.AddUser_ID;
            parameters[8].Value = model.APIG_ID;

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
        public bool Delete(int APIG_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from apigroup ");
            strSql.Append(" where APIG_ID=@APIG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIG_ID", MySqlDbType.Int32,4)         };
            parameters[0].Value = APIG_ID;

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
        public bool DeleteList(string APIG_IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from apigroup ");
            strSql.Append(" where APIG_ID in (" + APIG_IDlist + ")  ");
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
        public SApigroup GetModel(int APIG_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select APIG_ID,APIG_Name,Mark_Stop,APIG_Note,APIG_Type,SYSID,Add_Time,Stop_Time,AddUser_ID from apigroup ");
            strSql.Append(" where APIG_ID=@APIG_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIG_ID", MySqlDbType.Int32,4)         };
            parameters[0].Value = APIG_ID;

           SApigroup model = new SApigroup();
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
        public SApigroup DataRowToModel(DataRow row)
        {
           SApigroup model = new SApigroup();
            if (row != null)
            {
                if (row["APIG_ID"] != null && row["APIG_ID"].ToString() != "")
                {
                    model.APIG_ID = int.Parse(row["APIG_ID"].ToString());
                }
                if (row["APIG_Name"] != null)
                {
                    model.APIG_Name = row["APIG_Name"].ToString();
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
                if (row["APIG_Note"] != null)
                {
                    model.APIG_Note = row["APIG_Note"].ToString();
                }
                if (row["APIG_Type"] != null && row["APIG_Type"].ToString() != "")
                {
                    model.APIG_Type = int.Parse(row["APIG_Type"].ToString());
                }
                if (row["SYSID"] != null && row["SYSID"].ToString() != "")
                {
                    model.SYSID = int.Parse(row["SYSID"].ToString());
                }
                if (row["Add_Time"] != null && row["Add_Time"].ToString() != "")
                {
                    model.Add_Time = DateTime.Parse(row["Add_Time"].ToString());
                }
                if (row["Stop_Time"] != null && row["Stop_Time"].ToString() != "")
                {
                    model.Stop_Time = DateTime.Parse(row["Stop_Time"].ToString());
                }
                if (row["AddUser_ID"] != null)
                {
                    model.AddUser_ID = row["AddUser_ID"].ToString();
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
            strSql.Append("select APIG_ID,APIG_Name,Mark_Stop,APIG_Note,APIG_Type,SYSID,Add_Time,Stop_Time,AddUser_ID ");
            strSql.Append(" FROM apigroup ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListAndSysName(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select apigroup.* ,IFNULL(Ssysteminfo.SYS_NAME ,'')SYS_NAME ,IFNULL(Code_name ,'') Code_name ");
 
            strSql.Append(" FROM apigroup left   join Ssysteminfo on apigroup.SYSID=Ssysteminfo.SYSID ");
            strSql.Append(" join allcodeint on apigroup.APIG_Type = allcodeint.Code_id and allcodeint.All_lbmc = 'APIGroupType'");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取指定机构所能授权的组
        /// </summary>
        public DataTable  GetListORG(string ORG_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select  a.APIG_ID,a.APIG_Name,b.SYSID ,IFNULL(c.SYS_NAME ,'')SYS_NAME
 from apigroup a
join sorgandsys b on a.SYSID = b.SYSID
left join Ssysteminfo c on a.SYSID = c.SYSID
where b.ORG_ID =@ORG_ID and a.Mark_Stop = 0", ORG_ID));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10)         };
            parameters[0].Value = ORG_ID;

            return DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM apigroup ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQLMySpring.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.APIG_ID desc");
            }
            strSql.Append(")AS Row, T.*  from apigroup T ");
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
