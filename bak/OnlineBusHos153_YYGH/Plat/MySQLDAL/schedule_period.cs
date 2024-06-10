using System;
using System.Data;
using System.Text;
using DB.Core;
using MySql.Data.MySqlClient;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:schedule_period
    /// </summary>
    public partial class schedule_period 
    {
        public schedule_period()
        { }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME, string PERIOD_START)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from schedule_period");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;
            parameters[3].Value = SCH_DATE;
            parameters[4].Value = SCH_TIME;
            parameters[5].Value = PERIOD_START;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.schedule_period model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into schedule_period(");
            strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,PERIOD_START,PERIOD_END,COUNT_ALL,COUNT_YET)");
            strSql.Append(" values (");
            strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@PERIOD_START,@PERIOD_END,@COUNT_ALL,@COUNT_YET)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10),
					new MySqlParameter("@PERIOD_END", MySqlDbType.VarChar,10),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_YET", MySqlDbType.Int32,11)};
            parameters[0].Value = model.HOS_ID;
            parameters[1].Value = model.DEPT_CODE;
            parameters[2].Value = model.DOC_NO;
            parameters[3].Value = model.SCH_DATE;
            parameters[4].Value = model.SCH_TIME;
            parameters[5].Value = model.PERIOD_START;
            parameters[6].Value = model.PERIOD_END;
            parameters[7].Value = model.COUNT_ALL;
            parameters[8].Value = model.COUNT_YET;

            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Update(Plat.Model.schedule_period model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update schedule_period set ");
            strSql.Append("PERIOD_END=@PERIOD_END,");
            strSql.Append("COUNT_ALL=@COUNT_ALL,");
            strSql.Append("COUNT_YET=@COUNT_YET");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PERIOD_END", MySqlDbType.VarChar,10),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_YET", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.PERIOD_END;
            parameters[1].Value = model.COUNT_ALL;
            parameters[2].Value = model.COUNT_YET;
            parameters[3].Value = model.HOS_ID;
            parameters[4].Value = model.DEPT_CODE;
            parameters[5].Value = model.DOC_NO;
            parameters[6].Value = model.SCH_DATE;
            parameters[7].Value = model.SCH_TIME;
            parameters[8].Value = model.PERIOD_START;

            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME, string PERIOD_START)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from schedule_period ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;
            parameters[3].Value = SCH_DATE;
            parameters[4].Value = SCH_TIME;
            parameters[5].Value = PERIOD_START;

            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString(), parameters);
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
        public Plat.Model.schedule_period GetModel(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME, string PERIOD_START)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,PERIOD_START,PERIOD_END,COUNT_ALL,COUNT_YET from schedule_period ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;
            parameters[3].Value = SCH_DATE;
            parameters[4].Value = SCH_TIME;
            parameters[5].Value = PERIOD_START;

            Plat.Model.schedule_period model = new Plat.Model.schedule_period();
            DataSet ds = DbHelperMySQLZZJ.Query(strSql.ToString(), parameters);
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
        public Plat.Model.schedule_period DataRowToModel(DataRow row)
        {
            Plat.Model.schedule_period model = new Plat.Model.schedule_period();
            if (row != null)
            {
                if (row["HOS_ID"] != null)
                {
                    model.HOS_ID = row["HOS_ID"].ToString();
                }
                if (row["DEPT_CODE"] != null)
                {
                    model.DEPT_CODE = row["DEPT_CODE"].ToString();
                }
                if (row["DOC_NO"] != null)
                {
                    model.DOC_NO = row["DOC_NO"].ToString();
                }
                if (row["SCH_DATE"] != null)
                {
                    model.SCH_DATE = row["SCH_DATE"].ToString();
                }
                if (row["SCH_TIME"] != null)
                {
                    model.SCH_TIME = row["SCH_TIME"].ToString();
                }
                if (row["PERIOD_START"] != null)
                {
                    model.PERIOD_START = row["PERIOD_START"].ToString();
                }
                if (row["PERIOD_END"] != null)
                {
                    model.PERIOD_END = row["PERIOD_END"].ToString();
                }
                if (row["COUNT_ALL"] != null && row["COUNT_ALL"].ToString() != "")
                {
                    model.COUNT_ALL = int.Parse(row["COUNT_ALL"].ToString());
                }
                if (row["COUNT_YET"] != null && row["COUNT_YET"].ToString() != "")
                {
                    model.COUNT_YET = int.Parse(row["COUNT_YET"].ToString());
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
            strSql.Append("select HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,PERIOD_START,PERIOD_END,COUNT_ALL,COUNT_YET ");
            strSql.Append(" FROM schedule_period ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM schedule_period ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQLZZJ.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.PERIOD_START desc");
            }
            strSql.Append(")AS Row, T.*  from schedule_period T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }
        public DataTable GetList(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME)
        {
            string sqlcmd = string.Format(@"SELECT PERIOD_START,PERIOD_END,(COUNT_ALL-COUNT_YET)as COUNT_REM from schedule_period
            where HOS_ID='{0}' and DEPT_CODE='{1}'and DOC_NO='{2}' and SCH_DATE='{3}' and SCH_TIME='{4}'", HOS_ID, DEPT_CODE, DOC_NO, SCH_DATE, SCH_TIME);

            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
        /// <summary>
        /// 自助机获取医院排班时间段
        /// </summary>
        /// <param name="HOS_ID">医院代码</param>
        /// <param name="DEPT_CODE">科室代码</param>
        /// <param name="DOC_NO">医生代码</param>
        /// <param name="SCH_DATE">排班日期</param>
        /// <param name="SCH_TIME">排班上下午</param>
        /// <returns></returns>
        public DataTable GetList_ZZJ(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME)
        {
            string sqlcmd = string.Format(@"SELECT PERIOD_START,PERIOD_END,(COUNT_ALL-COUNT_YET)as COUNT_REM from schedule_period
            where HOS_ID='{0}' and DEPT_CODE='{1}'and DOC_NO='{2}' and SCH_DATE='{3}' and SCH_TIME='{4}'", HOS_ID, DEPT_CODE, DOC_NO, SCH_DATE, SCH_TIME);

            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }

        public DataTable GetList(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME,int PAGEINDEX,int PAGESIZE)
        {
            string sqlcmd = string.Format(@"SELECT PERIOD_START,PERIOD_END,(COUNT_ALL-COUNT_YET)as COUNT_REM from schedule_period
            where HOS_ID='{0}' and DEPT_CODE='{1}'and DOC_NO='{2}' and SCH_DATE='{3}' and SCH_TIME='{4}'", HOS_ID, DEPT_CODE, DOC_NO, SCH_DATE, SCH_TIME);

                //sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + (PAGESIZE * PAGEINDEX);
            sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;

            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
    }
}

