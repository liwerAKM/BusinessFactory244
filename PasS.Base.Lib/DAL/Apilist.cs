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
    /// 数据访问类:apilist
    /// </summary>
    public partial class Apilist
    {
        public Apilist()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQLMySpring.GetMaxID("API_ID", "apilist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int API_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from apilist");
            strSql.Append(" where API_ID=@API_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@API_ID", MySqlDbType.Int32,11)         };
            parameters[0].Value = API_ID;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Sapilist  model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into apilist(");
            strSql.Append("API_ID,API_Name,API_Note,Mark_Stop,Add_Time,isBUS_ID,Stop_time)");
            strSql.Append(" values (");
            strSql.Append("@API_ID,@API_Name,@API_Note,@Mark_Stop,@Add_Time,@isBUS_ID,@Stop_time)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@API_ID", MySqlDbType.Int32,11),
                    new MySqlParameter("@API_Name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@API_Note", MySqlDbType.VarChar,60),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@Add_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@isBUS_ID", MySqlDbType.Bit),
                    new MySqlParameter("@Stop_time", MySqlDbType.DateTime)};
            parameters[0].Value = model.API_ID;
            parameters[1].Value = model.API_Name;
            parameters[2].Value = model.API_Note;
            parameters[3].Value = model.Mark_Stop;
            parameters[4].Value = model.Add_Time;
            parameters[5].Value = model.isBUS_ID;
            parameters[6].Value = model.Stop_time;

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
        public bool Update(Sapilist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update apilist set ");
            strSql.Append("API_Name=@API_Name,");
            strSql.Append("API_Note=@API_Note,");
            strSql.Append("Mark_Stop=@Mark_Stop,");
            strSql.Append("Add_Time=@Add_Time,");
            strSql.Append("isBUS_ID=@isBUS_ID,");
            strSql.Append("Stop_time=@Stop_time");
            strSql.Append(" where API_ID=@API_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@API_Name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@API_Note", MySqlDbType.VarChar,60),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@Add_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@isBUS_ID", MySqlDbType.Bit),
                    new MySqlParameter("@Stop_time", MySqlDbType.DateTime),
                    new MySqlParameter("@API_ID", MySqlDbType.Int32,11)};
            parameters[0].Value = model.API_Name;
            parameters[1].Value = model.API_Note;
            parameters[2].Value = model.Mark_Stop;
            parameters[3].Value = model.Add_Time;
            parameters[4].Value = model.isBUS_ID;
            parameters[5].Value = model.Stop_time;
            parameters[6].Value = model.API_ID;

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
        public bool Delete(int API_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from apilist ");
            strSql.Append(" where API_ID=@API_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@API_ID", MySqlDbType.Int32,11)         };
            parameters[0].Value = API_ID;

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

        public SAPIList GetSAPIList(int API_ID)
        {
            Sapilist sapilist = GetModel(API_ID);
            if (sapilist != null)
            {
                return new SAPIList(sapilist);
            }
            return null;
        }
        /// <summary>
        /// 获取在用的SAPIList
        /// </summary>
        /// <returns></returns>
        public List<SAPIList> GetSAPIListUsed()
        {
            List<SAPIList> list = new List<SAPIList>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select API_ID,API_Name,API_Note,Mark_Stop,Add_Time,isBUS_ID,Stop_time ");
            strSql.Append(" FROM apilist where Mark_Stop=0 ");
             
            DataTable dt= DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            foreach (DataRow dr in dt.Rows )
            {
                list.Add(new SAPIList(DataRowToModel(dr)));
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Sapilist GetModel(int API_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select API_ID,API_Name,API_Note,Mark_Stop,Add_Time,isBUS_ID,Stop_time from apilist ");
            strSql.Append(" where API_ID=@API_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@API_ID", MySqlDbType.Int32,11)         };
            parameters[0].Value = API_ID;

            Sapilist model = new Sapilist();
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
        public Sapilist DataRowToModel(DataRow row)
        {
            Sapilist model = new Sapilist();
            if (row != null)
            {
                if (row["API_ID"] != null && row["API_ID"].ToString() != "")
                {
                    model.API_ID = int.Parse(row["API_ID"].ToString());
                }
                if (row["API_Name"] != null)
                {
                    model.API_Name = row["API_Name"].ToString();
                }
                if (row["API_Note"] != null)
                {
                    model.API_Note = row["API_Note"].ToString();
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
                if (row["Add_Time"] != null && row["Add_Time"].ToString() != "")
                {
                    model.Add_Time = DateTime.Parse(row["Add_Time"].ToString());
                }
                if (row["isBUS_ID"] != null && row["isBUS_ID"].ToString() != "")
                {
                    if ((row["isBUS_ID"].ToString() == "1") || (row["isBUS_ID"].ToString().ToLower() == "true"))
                    {
                        model.isBUS_ID = true;
                    }
                    else
                    {
                        model.isBUS_ID = false;
                    }
                }
                if (row["Stop_time"] != null && row["Stop_time"].ToString() != "")
                {
                    model.Stop_time = DateTime.Parse(row["Stop_time"].ToString());
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
            strSql.Append("select API_ID,API_Name,API_Note,Mark_Stop,Add_Time,isBUS_ID,Stop_time ");
            strSql.Append(" FROM apilist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable  GetSysAPI(int SYSID ,bool Contain_Stop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select API_ID,API_Name,API_Note,Mark_Stop,Add_Time,isBUS_ID,Stop_time ");
            strSql.Append(" FROM apilist ");
            string where = "";

            if (SYSID >= 0)
             //   where = " where ssyspergroup.SYSID = " + SYSID.ToString();

            if (Contain_Stop == false)
            {
                if (where == "") where = " where apilist.Mark_Stop =0 ";
                else where += " and apilist.Mark_Stop =0 ";
            }

            return DbHelperMySQLMySpring.Query(strSql.ToString() + where).Tables[0];
             
        }

        /// <summary>
        /// 获得功能点对应API
        /// </summary>
        public DataTable GetSFPandAPI(int FP_ID, bool Contain_Stop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select apilist.API_ID,API_Name,API_Note,Mark_Stop   FROM apilist
join SFunandAPI on  apilist.API_ID= SFunandAPI.API_ID and SFunandAPI.FP_ID = {0}", FP_ID));

            if (Contain_Stop == false)
            {
                strSql.Append(" and apilist.Mark_Stop =0 ");
            }

            return DbHelperMySQLMySpring.Query(strSql.ToString() ).Tables[0];
        }

        
        /// <summary>
        /// 获取指定API组所含API列表
        /// </summary>
        /// <param name="APIG_ID"></param>
        /// <returns></returns>
        public DataTable GetAPIGAPIList(int APIG_ID )
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"  select a.* 
from apilist a
join APIandGroup b on a.API_ID = b.API_ID
where a.Mark_Stop = 0 and b.APIG_ID = {0}", APIG_ID));
           
             
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取指定API组列表所具有API列表
        /// </summary>
        /// <param name="APIG_ID"></param>
        /// <returns></returns>
        public List<int> GetAPIGlAPIList(List<int> listGroup)
        {
            if(listGroup==null || listGroup.Count==0)
                return   new List<int>();

            List<int> list = new List<int>();
            StringBuilder strSql = new StringBuilder();
            string ListGroup = "";
            foreach (int G_ID in listGroup)
            {
                ListGroup = ListGroup + G_ID.ToString() + ",";
            }
            ListGroup= ListGroup.TrimEnd(',');
            strSql.Append(string.Format(@" select  DISTINCT a.API_ID from
apilist a
join apiandgroup b
on a.API_ID= b.API_ID
where b.APIG_ID  in({0})and a.Mark_Stop= 0", ListGroup));

            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add((int)dr["API_ID"]);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定API组列表所具有API列表
        /// </summary>
        /// <param name="APIG_ID"></param>
        /// <returns></returns>
        public List<int> GetAPIGlAPIList(int APIG_ID)
        {
           
            List<int> list = new List<int>();
            StringBuilder strSql = new StringBuilder();
           
            strSql.Append(string.Format(@" select  DISTINCT a.API_ID from
apilist a
join apiandgroup b
on a.API_ID= b.API_ID
where b.APIG_ID = {0} and a.Mark_Stop= 0", APIG_ID));

            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add((int)dr["API_ID"]);
                }
            }
            return list;
        }


        /// <summary>
        /// 获取指定API用户所具有API列表
        /// </summary>
        /// <param name="APIG_ID"></param>
        /// <returns></returns>
        public List<int>   GetAPIUserAPIList (string APIU_ID)
        {
            List<int> list = new List<int>();
               StringBuilder strSql = new StringBuilder();
            strSql.Append(@"  select a.API_ID from
apilist a
join apianduser b
on a.API_ID= b.API_ID
where b.APIU_ID= @APIU_ID and a.Mark_Stop= 0

union all

select  a.API_ID from
apilist a
join apiandgroup b
on a.API_ID= b.API_ID
join apiuserandgroup c
on b.APIG_ID = c.APIG_ID
where c.APIU_ID=@APIU_ID and a.Mark_Stop= 0");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,50)         };
            parameters[0].Value = APIU_ID;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add((int)dr["API_ID"]);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定API用户所具有API列表
        /// </summary>
        /// <param name="APIG_ID"></param>
        /// <returns></returns>
        public DataTable GetAPIUserAPIListdt(string APIU_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append( @"  select  a.* from
apilist a
join apianduser b
on a.API_ID= b.API_ID
where b.APIU_ID= @APIU_ID  and a.Mark_Stop= 0

union all

select c.APIU_ID, a.* from
apilist a
join apiandgroup b
on a.API_ID= b.API_ID
join apiuserandgroup c
on b.APIG_ID = c.APIG_ID
where c.APIU_ID= @APIU_ID and a.Mark_Stop= 0" );
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,50)         };
            parameters[0].Value = APIU_ID;
          

            return DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];
        }




        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM apilist ");
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
                strSql.Append("order by T.API_ID desc");
            }
            strSql.Append(")AS Row, T.*  from apilist T ");
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
