using System;
using System.Data;
using System.Text;
using DB.Core;
using Log.Core.Model;
using MySql.Data.MySqlClient;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:schedule
    /// </summary>
    public partial class schedule
    {
        public schedule()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from schedule");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;
            parameters[3].Value = SCH_DATE;
            parameters[4].Value = SCH_TIME;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists_ZZJ(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from schedule");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;
            parameters[3].Value = SCH_DATE;
            parameters[4].Value = SCH_TIME;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.schedule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into schedule(");
            strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,SCH_TYPE,COUNT_ALL,COUNT_REM,GH_FEE,ZL_FEE,START_TIME,END_TIME,COUNT_ESB,COUNT_PAY,COUNT_DEF,WAIT_OPEN_TIME,PRO_TITLE,CAN_WAIT,REGISTER_TYPE)");
            strSql.Append(" values (");
            strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@SCH_TYPE,@COUNT_ALL,@COUNT_REM,@GH_FEE,@ZL_FEE,@START_TIME,@END_TIME,@COUNT_ESB,@COUNT_PAY,@COUNT_DEF,@WAIT_OPEN_TIME,@PRO_TITLE,@CAN_WAIT,@REGISTER_TYPE)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@REGISTER_TYPE", MySqlDbType.VarChar,5)};
            parameters[0].Value = model.HOS_ID;
            parameters[1].Value = model.DEPT_CODE;
            parameters[2].Value = model.DOC_NO;
            parameters[3].Value = model.SCH_DATE;
            parameters[4].Value = model.SCH_TIME;
            parameters[5].Value = model.SCH_TYPE;
            parameters[6].Value = model.COUNT_ALL;
            parameters[7].Value = model.COUNT_REM;
            parameters[8].Value = model.GH_FEE;
            parameters[9].Value = model.ZL_FEE;
            parameters[10].Value = model.START_TIME;
            parameters[11].Value = model.END_TIME;
            parameters[12].Value = model.COUNT_ESB;
            parameters[13].Value = model.COUNT_PAY;
            parameters[14].Value = model.COUNT_DEF;
            parameters[15].Value = model.WAIT_OPEN_TIME;
            parameters[16].Value = model.PRO_TITLE;
            parameters[17].Value = model.CAN_WAIT;
            parameters[18].Value = model.REGISTER_TYPE;

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
        public bool Update(Plat.Model.schedule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update schedule set ");
            strSql.Append("SCH_TYPE=@SCH_TYPE,");
            strSql.Append("COUNT_ALL=@COUNT_ALL,");
            strSql.Append("COUNT_REM=@COUNT_REM,");
            strSql.Append("GH_FEE=@GH_FEE,");
            strSql.Append("ZL_FEE=@ZL_FEE,");
            strSql.Append("START_TIME=@START_TIME,");
            strSql.Append("END_TIME=@END_TIME,");
            strSql.Append("COUNT_ESB=@COUNT_ESB,");
            strSql.Append("COUNT_PAY=@COUNT_PAY,");
            strSql.Append("COUNT_DEF=@COUNT_DEF,");
            strSql.Append("WAIT_OPEN_TIME=@WAIT_OPEN_TIME,");
            strSql.Append("PRO_TITLE=@PRO_TITLE,");
            strSql.Append("CAN_WAIT=@CAN_WAIT,");
            strSql.Append("REGISTER_TYPE=@REGISTER_TYPE");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@REGISTER_TYPE", MySqlDbType.VarChar,5),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)};
            parameters[0].Value = model.SCH_TYPE;
            parameters[1].Value = model.COUNT_ALL;
            parameters[2].Value = model.COUNT_REM;
            parameters[3].Value = model.GH_FEE;
            parameters[4].Value = model.ZL_FEE;
            parameters[5].Value = model.START_TIME;
            parameters[6].Value = model.END_TIME;
            parameters[7].Value = model.COUNT_ESB;
            parameters[8].Value = model.COUNT_PAY;
            parameters[9].Value = model.COUNT_DEF;
            parameters[10].Value = model.WAIT_OPEN_TIME;
            parameters[11].Value = model.PRO_TITLE;
            parameters[12].Value = model.CAN_WAIT;
            parameters[13].Value = model.REGISTER_TYPE;
            parameters[14].Value = model.HOS_ID;
            parameters[15].Value = model.DEPT_CODE;
            parameters[16].Value = model.DOC_NO;
            parameters[17].Value = model.SCH_DATE;
            parameters[18].Value = model.SCH_TIME;

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
        public bool Delete(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from schedule ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;
            parameters[3].Value = SCH_DATE;
            parameters[4].Value = SCH_TIME;

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
        public Plat.Model.schedule GetModel(string HOS_ID, string DEPT_CODE, string DOC_NO, string SCH_DATE, string SCH_TIME)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,SCH_TYPE,COUNT_ALL,COUNT_REM,GH_FEE,ZL_FEE,START_TIME,END_TIME,COUNT_ESB,COUNT_PAY,COUNT_DEF,WAIT_OPEN_TIME,PRO_TITLE,CAN_WAIT,REGISTER_TYPE from schedule ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;
            parameters[3].Value = SCH_DATE;
            parameters[4].Value = SCH_TIME;

            Plat.Model.schedule model = new Plat.Model.schedule();
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
        public Plat.Model.schedule DataRowToModel(DataRow row)
        {
            Plat.Model.schedule model = new Plat.Model.schedule();
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
                if (row["SCH_TYPE"] != null && row["SCH_TYPE"].ToString() != "")
                {
                    model.SCH_TYPE = int.Parse(row["SCH_TYPE"].ToString());
                }
                if (row["COUNT_ALL"] != null && row["COUNT_ALL"].ToString() != "")
                {
                    model.COUNT_ALL = int.Parse(row["COUNT_ALL"].ToString());
                }
                if (row["COUNT_REM"] != null && row["COUNT_REM"].ToString() != "")
                {
                    model.COUNT_REM = int.Parse(row["COUNT_REM"].ToString());
                }
                if (row["GH_FEE"] != null && row["GH_FEE"].ToString() != "")
                {
                    model.GH_FEE = decimal.Parse(row["GH_FEE"].ToString());
                }
                if (row["ZL_FEE"] != null && row["ZL_FEE"].ToString() != "")
                {
                    model.ZL_FEE = decimal.Parse(row["ZL_FEE"].ToString());
                }
                if (row["START_TIME"] != null)
                {
                    model.START_TIME = row["START_TIME"].ToString();
                }
                if (row["END_TIME"] != null)
                {
                    model.END_TIME = row["END_TIME"].ToString();
                }
                if (row["COUNT_ESB"] != null && row["COUNT_ESB"].ToString() != "")
                {
                    model.COUNT_ESB = int.Parse(row["COUNT_ESB"].ToString());
                }
                if (row["COUNT_PAY"] != null && row["COUNT_PAY"].ToString() != "")
                {
                    model.COUNT_PAY = int.Parse(row["COUNT_PAY"].ToString());
                }
                if (row["COUNT_DEF"] != null && row["COUNT_DEF"].ToString() != "")
                {
                    model.COUNT_DEF = int.Parse(row["COUNT_DEF"].ToString());
                }
                if (row["WAIT_OPEN_TIME"] != null && row["WAIT_OPEN_TIME"].ToString() != "")
                {
                    model.WAIT_OPEN_TIME = DateTime.Parse(row["WAIT_OPEN_TIME"].ToString());
                }
                if (row["PRO_TITLE"] != null)
                {
                    model.PRO_TITLE = row["PRO_TITLE"].ToString();
                }
                if (row["CAN_WAIT"] != null && row["CAN_WAIT"].ToString() != "")
                {
                    if ((row["CAN_WAIT"].ToString() == "1") || (row["CAN_WAIT"].ToString().ToLower() == "true"))
                    {
                        model.CAN_WAIT = true;
                    }
                    else
                    {
                        model.CAN_WAIT = false;
                    }
                }
                if (row["REGISTER_TYPE"] != null)
                {
                    model.REGISTER_TYPE = row["REGISTER_TYPE"].ToString();
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
            strSql.Append("select HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,SCH_TYPE,COUNT_ALL,COUNT_REM,GH_FEE,ZL_FEE,START_TIME,END_TIME,COUNT_ESB,COUNT_PAY,COUNT_DEF,WAIT_OPEN_TIME,PRO_TITLE,CAN_WAIT,REGISTER_TYPE ");
            strSql.Append(" FROM schedule ");
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
            strSql.Append("select count(1) FROM schedule ");
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
                strSql.Append("order by T.SCH_TIME desc");
            }
            strSql.Append(")AS Row, T.*  from schedule T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }
        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        /// 获取数据列表（医生排班）
        /// </summary>
        /// <param name="HOS_ID">医院ID</param>
        /// <param name="DEPT_CODE">科室代码</param>
        /// <param name="SCH_TYPE">排班类型</param>
        /// <returns></returns>
        public DataTable GetList(string HOS_ID, string DEPT_CODE, string SCH_TYPE, string DOC_NO)
        {
            string where = "";
            if (SCH_TYPE == "01")
            {
                where = " and date(SCH_DATE)>curdate()";
            }
            else
            {
                where = " and date(SCH_DATE) = curdate()";
            }


            string sqlcmd = string.Format(@"select distinct a.SCH_DATE,a.SCH_TIME,a.SCH_TYPE,a.COUNT_REM,a.CAN_WAIT,
a.GH_FEE,a.ZL_FEE,a.START_TIME,a.END_TIME,d.DOC_ORDER,a.PRO_TITLE
 from schedule a,hospital b,doc_info d
 where a.HOS_ID=b.HOS_ID and a.DOC_NO=d.DOC_NO   and a.HOS_ID=d.HOS_ID
and a.HOS_ID='{0}' and a.DEPT_CODE='{1}' and a.DOC_NO='{2}' {3}", HOS_ID, DEPT_CODE, DOC_NO, where);


//            string sqlcmd = string.Format(@"select distinct a.SCH_DATE,a.SCH_TIME,a.SCH_TYPE,a.COUNT_REM,a.CAN_WAIT,
//a.GH_FEE,a.ZL_FEE,a.START_TIME,a.END_TIME,a.DOC_ORDER,a.PRO_TITLE
// from zjpb a
// where a.HOS_ID='{0}' and a.DEPT_CODE='{1}' and a.DOC_NO='{2}' {3}", HOS_ID, DEPT_CODE, DOC_NO, where);

            if (HOS_ID == "")
            {
                sqlcmd = string.Format(@"select distinct a.SCH_DATE,a.SCH_TIME,a.SCH_TYPE,a.COUNT_REM,a.CAN_WAIT,
a.GH_FEE,a.ZL_FEE,a.START_TIME,a.END_TIME,d.DOC_ORDER,a.PRO_TITLE
 from schedule a,hospital b,dept_info c,doc_info d
 where a.HOS_ID=b.HOS_ID and a.DOC_NO=d.DOC_NO  and a.HOS_ID=c.HOS_ID and a.HOS_ID=d.HOS_ID and a.DEPT_CODE=d.DEPT_CODE
and a.DEPT_CODE=c.DEPT_CODE a.DEPT_CODE='{0}' and a.DOC_NO='{1}' {2}", DEPT_CODE, DOC_NO, where);
            }
            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
        /// <summary>
        /// 获取数据列表(科室排班)
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <param name="DEPT_CODE"></param>
        /// <param name="SCH_TYPE"></param>
        /// <returns></returns>
        public DataTable GetListKS(string HOS_ID, string DEPT_CODE, string SCH_TYPE)
        {
            string where = "";
            if (SCH_TYPE == "01")
            {
                where = " and date(SCH_DATE)>curdate()";
            }
            else
            {
                where = " and date(SCH_DATE) = curdate()";
            }
//            string sqlcmd = string.Format(@" select a.DOC_NO ,a.SCH_DATE,
//a.SCH_TIME,a.SCH_TYPE,a.COUNT_REM,a.CAN_WAIT,
//a.GH_FEE,a.ZL_FEE,a.START_TIME,a.END_TIME,b.DEPT_ORDER
// from schedule a,dept_info b,hospital c
//where a.DEPT_CODE=b.DEPT_CODE and a.HOS_ID=b.HOS_ID and b.HOS_ID=c.HOS_ID and a.HOS_ID='{0}' and a.DEPT_CODE='{1}' and SCH_TYPE='1'  {2}", HOS_ID, DEPT_CODE, where);

            string sqlcmd = string.Format(@"select *from kspb a where a.HOS_ID='{0}' and a.DEPT_CODE='{1}' {2}", HOS_ID, DEPT_CODE, where);


            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
        /// <summary>
        /// 获取数据列表 医院接口
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <param name="DEPT_CODE"></param>
        /// <param name="SCH_DATE"></param>
        /// <returns></returns>
        public DataTable GetListYY(string HOS_ID, string DEPT_CODE, string SCH_DATE)
        {
//            string sqlcmd = string.Format(@"select a.DEPT_CODE,c.DEPT_NAME,a.DOC_NO,a.SCH_DATE,a.SCH_TIME,a.SCH_TYPE,a.COUNT_REM,
//a.GH_FEE,a.ZL_FEE,a.START_TIME,a.END_TIME,d.DOC_ORDER
// from schedule a,hospital b,dept_info c,doc_info d
// where a.HOS_ID=b.HOS_ID and a.DOC_NO=d.DOC_NO  and a.HOS_ID=c.HOS_ID and a.HOS_ID=d.HOS_ID and a.DEPT_CODE=d.DEPT_CODE
//and a.DEPT_CODE=c.DEPT_CODE and SCH_DATE='{0}' and a.HOS_ID='{1}' and a.DEPT_CODE='{2}' ", SCH_DATE, HOS_ID, DEPT_CODE);

            string sqlcmd = string.Format(@"select a.DEPT_CODE,c.DEPT_NAME,a.DOC_NO,a.SCH_DATE,a.SCH_TIME,a.SCH_TYPE,a.COUNT_REM,a.CAN_WAIT,
a.GH_FEE,a.ZL_FEE,a.START_TIME,a.END_TIME,c.DEPT_ORDER
 from schedule a,hospital b,dept_info c
 where a.HOS_ID=b.HOS_ID and a.HOS_ID=c.HOS_ID
and a.DEPT_CODE=c.DEPT_CODE and SCH_DATE='{0}' and a.HOS_ID='{1}' and a.DEPT_CODE='{2}' and sch_type=1",SCH_DATE,HOS_ID,DEPT_CODE);
            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }

        /// <summary>
        /// 获取指定医院科室排班列表
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddByTran(Plat.Model.schedule schedule, Plat.Model.schedule_period[] period)
        {
            StringBuilder strSql = new StringBuilder();

            System.Collections.Hashtable table = new System.Collections.Hashtable();
            if (schedule.OPERA_TYPE == 1)
            {
                if (!Exists(schedule.HOS_ID, schedule.DEPT_CODE, schedule.DOC_NO, schedule.SCH_DATE, schedule.SCH_TIME))
                {

                    /*strSql = new StringBuilder();
                    strSql.Append("insert into schedule(");
                    strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,SCH_TYPE,COUNT_ALL,COUNT_REM,GH_FEE,ZL_FEE,START_TIME,END_TIME,COUNT_ESB,COUNT_PAY,COUNT_DEF,WAIT_OPEN_TIME,PRO_TITLE,CAN_WAIT)");
                    strSql.Append(" values (");
                    strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@SCH_TYPE,@COUNT_ALL,@COUNT_REM,@GH_FEE,@ZL_FEE,@START_TIME,@END_TIME,@COUNT_ESB,@COUNT_PAY,@COUNT_DEF,@WAIT_OPEN_TIME,@PRO_TITLE,@CAN_WAIT)");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit)};
                    parameters[0].Value = schedule.HOS_ID;
                    parameters[1].Value = schedule.DEPT_CODE;
                    parameters[2].Value = schedule.DOC_NO;
                    parameters[3].Value = schedule.SCH_DATE;
                    parameters[4].Value = schedule.SCH_TIME;
                    parameters[5].Value = schedule.SCH_TYPE;
                    parameters[6].Value = schedule.COUNT_ALL;
                    parameters[7].Value = schedule.COUNT_REM;
                    parameters[8].Value = schedule.GH_FEE;
                    parameters[9].Value = schedule.ZL_FEE;
                    parameters[10].Value = schedule.START_TIME;
                    parameters[11].Value = schedule.END_TIME;
                    parameters[12].Value = schedule.COUNT_ESB;
                    parameters[13].Value = schedule.COUNT_PAY;
                    parameters[14].Value = schedule.COUNT_DEF;
                    parameters[15].Value = schedule.WAIT_OPEN_TIME;
                    parameters[16].Value = schedule.PRO_TITLE;
                    parameters[17].Value = schedule.CAN_WAIT;
                    */

                    strSql = new StringBuilder();
                    strSql.Append("insert into schedule(");
                    strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,SCH_TYPE,COUNT_ALL,COUNT_REM,GH_FEE,ZL_FEE,START_TIME,END_TIME,COUNT_ESB,COUNT_PAY,COUNT_DEF,WAIT_OPEN_TIME,PRO_TITLE,CAN_WAIT,REGISTER_TYPE,YB_CODE)");
                    strSql.Append(" values (");
                    strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@SCH_TYPE,@COUNT_ALL,@COUNT_REM,@GH_FEE,@ZL_FEE,@START_TIME,@END_TIME,@COUNT_ESB,@COUNT_PAY,@COUNT_DEF,@WAIT_OPEN_TIME,@PRO_TITLE,@CAN_WAIT,@REGISTER_TYPE,@YB_CODE)");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@REGISTER_TYPE", MySqlDbType.VarChar,5),
                    new MySqlParameter("@YB_CODE", MySqlDbType.VarChar,10)};
                    parameters[0].Value = schedule.HOS_ID;
                    parameters[1].Value = schedule.DEPT_CODE;
                    parameters[2].Value = schedule.DOC_NO;
                    parameters[3].Value = schedule.SCH_DATE;
                    parameters[4].Value = schedule.SCH_TIME;
                    parameters[5].Value = schedule.SCH_TYPE;
                    parameters[6].Value = schedule.COUNT_ALL;
                    parameters[7].Value = schedule.COUNT_REM;
                    parameters[8].Value = schedule.GH_FEE;
                    parameters[9].Value = schedule.ZL_FEE;
                    parameters[10].Value = schedule.START_TIME;
                    parameters[11].Value = schedule.END_TIME;
                    parameters[12].Value = schedule.COUNT_ESB;
                    parameters[13].Value = schedule.COUNT_PAY;
                    parameters[14].Value = schedule.COUNT_DEF;
                    parameters[15].Value = schedule.WAIT_OPEN_TIME;
                    parameters[16].Value = schedule.PRO_TITLE;
                    parameters[17].Value = schedule.CAN_WAIT;
                    parameters[18].Value = schedule.REGISTER_TYPE;
                    parameters[19].Value = schedule.YB_CODE;
                    table.Add(strSql.ToString(), parameters);
                }
                else
                {
                    strSql = new StringBuilder();
                    /*strSql.Append("update schedule set ");
                    strSql.Append("SCH_TYPE=@SCH_TYPE,");
                    strSql.Append("COUNT_ALL=@COUNT_ALL,");
                    strSql.Append("COUNT_REM=@COUNT_REM,");
                    strSql.Append("GH_FEE=@GH_FEE,");
                    strSql.Append("ZL_FEE=@ZL_FEE,");
                    strSql.Append("START_TIME=@START_TIME,");
                    strSql.Append("END_TIME=@END_TIME,");
                    strSql.Append("COUNT_ESB=@COUNT_ESB,");
                    strSql.Append("COUNT_PAY=@COUNT_PAY,");
                    strSql.Append("COUNT_DEF=@COUNT_DEF,");
                    strSql.Append("WAIT_OPEN_TIME=@WAIT_OPEN_TIME,");
                    strSql.Append("PRO_TITLE=@PRO_TITLE,");
                    strSql.Append("CAN_WAIT=@CAN_WAIT");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)};
                    parameters[0].Value = schedule.SCH_TYPE;
                    parameters[1].Value = schedule.COUNT_ALL;
                    parameters[2].Value = schedule.COUNT_REM;
                    parameters[3].Value = schedule.GH_FEE;
                    parameters[4].Value = schedule.ZL_FEE;
                    parameters[5].Value = schedule.START_TIME;
                    parameters[6].Value = schedule.END_TIME;
                    parameters[7].Value = schedule.COUNT_ESB;
                    parameters[8].Value = schedule.COUNT_PAY;
                    parameters[9].Value = schedule.COUNT_DEF;
                    parameters[10].Value = schedule.WAIT_OPEN_TIME;
                    parameters[11].Value = schedule.PRO_TITLE;
                    parameters[12].Value = schedule.CAN_WAIT;
                    parameters[13].Value = schedule.HOS_ID;
                    parameters[14].Value = schedule.DEPT_CODE;
                    parameters[15].Value = schedule.DOC_NO;
                    parameters[16].Value = schedule.SCH_DATE;
                    parameters[17].Value = schedule.SCH_TIME;
                     */
                    strSql.Append("update schedule set ");
                    strSql.Append("SCH_TYPE=@SCH_TYPE,");
                    strSql.Append("COUNT_ALL=@COUNT_ALL,");
                    strSql.Append("COUNT_REM=@COUNT_REM,");
                    strSql.Append("GH_FEE=@GH_FEE,");
                    strSql.Append("ZL_FEE=@ZL_FEE,");
                    strSql.Append("START_TIME=@START_TIME,");
                    strSql.Append("END_TIME=@END_TIME,");
                    strSql.Append("COUNT_ESB=@COUNT_ESB,");
                    strSql.Append("COUNT_PAY=@COUNT_PAY,");
                    strSql.Append("COUNT_DEF=@COUNT_DEF,");
                    strSql.Append("WAIT_OPEN_TIME=@WAIT_OPEN_TIME,");
                    strSql.Append("PRO_TITLE=@PRO_TITLE,");
                    strSql.Append("CAN_WAIT=@CAN_WAIT,");
                    strSql.Append("REGISTER_TYPE=@REGISTER_TYPE,");
                    strSql.Append("YB_CODE=@YB_CODE");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@REGISTER_TYPE", MySqlDbType.VarChar,5),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
                    new MySqlParameter("@YB_CODE", MySqlDbType.VarChar,10),};
                    parameters[0].Value = schedule.SCH_TYPE;
                    parameters[1].Value = schedule.COUNT_ALL;
                    parameters[2].Value = schedule.COUNT_REM;
                    parameters[3].Value = schedule.GH_FEE;
                    parameters[4].Value = schedule.ZL_FEE;
                    parameters[5].Value = schedule.START_TIME;
                    parameters[6].Value = schedule.END_TIME;
                    parameters[7].Value = schedule.COUNT_ESB;
                    parameters[8].Value = schedule.COUNT_PAY;
                    parameters[9].Value = schedule.COUNT_DEF;
                    parameters[10].Value = schedule.WAIT_OPEN_TIME;
                    parameters[11].Value = schedule.PRO_TITLE;
                    parameters[12].Value = schedule.CAN_WAIT;
                    parameters[13].Value = schedule.REGISTER_TYPE;
                    parameters[14].Value = schedule.HOS_ID;
                    parameters[15].Value = schedule.DEPT_CODE;
                    parameters[16].Value = schedule.DOC_NO;
                    parameters[17].Value = schedule.SCH_DATE;
                    parameters[18].Value = schedule.SCH_TIME;
                    parameters[19].Value = schedule.YB_CODE;
                    table.Add(strSql.ToString(), parameters);
                }
            }
            else if (schedule.OPERA_TYPE == 2)
            {
                strSql.Append("delete from schedule ");
                strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
                MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
                parameters[0].Value = schedule.HOS_ID;
                parameters[1].Value = schedule.DEPT_CODE;
                parameters[2].Value = schedule.DOC_NO;
                parameters[3].Value = schedule.SCH_DATE;
                parameters[4].Value = schedule.SCH_TIME;

                table.Add(strSql.ToString(), parameters);
            }

            for (int i = 0; i < period.Length; i++)
            {
                if (schedule.OPERA_TYPE == 2)//删除排班表则排版时间段表也删除
                {
                    strSql = new StringBuilder();
                    strSql.Append("delete from schedule_period ");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)	};
                    parameters[0].Value = period[i].HOS_ID;
                    parameters[1].Value = period[i].DEPT_CODE;
                    parameters[2].Value = period[i].DOC_NO;
                    parameters[3].Value = period[i].SCH_DATE;
                    parameters[4].Value = period[i].SCH_TIME;

                    int j = i;
                    while (j-- > 0)
                    {
                        strSql.Append(" ");
                    }
                    table.Add(strSql.ToString(), parameters);
                    continue;
                }
                if (period[i].OPERA_TYPE == 1)
                {
                    if (!period[i].IsExists)
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into schedule_period(");
                        strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,PERIOD_START,PERIOD_END,COUNT_ALL,COUNT_YET)");
                        strSql.Append(" values (");
                        strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@PERIOD_START,@PERIOD_END,@COUNT_ALL,@COUNT_YET)");

                        MySqlParameter[] parameters1 = {
					    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					    new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					    new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					    new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					    new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10),
					    new MySqlParameter("@PERIOD_END", MySqlDbType.VarChar,10),
					    new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					    new MySqlParameter("@COUNT_YET", MySqlDbType.Int32,11)};
                        parameters1[0].Value = period[i].HOS_ID;
                        parameters1[1].Value = period[i].DEPT_CODE;
                        parameters1[2].Value = period[i].DOC_NO;
                        parameters1[3].Value = period[i].SCH_DATE;
                        parameters1[4].Value = period[i].SCH_TIME;
                        parameters1[5].Value = period[i].PERIOD_START;
                        parameters1[6].Value = period[i].PERIOD_END;
                        parameters1[7].Value = period[i].COUNT_ALL;
                        parameters1[8].Value = period[i].COUNT_YET;
                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters1);
                    }
                    else
                    {
                        strSql = new StringBuilder();
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
                        parameters[0].Value = period[i].PERIOD_END;
                        parameters[1].Value = period[i].COUNT_ALL;
                        parameters[2].Value = period[i].COUNT_YET;
                        parameters[3].Value = period[i].HOS_ID;
                        parameters[4].Value = period[i].DEPT_CODE;
                        parameters[5].Value = period[i].DOC_NO;
                        parameters[6].Value = period[i].SCH_DATE;
                        parameters[7].Value = period[i].SCH_TIME;
                        parameters[8].Value = period[i].PERIOD_START;

                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                }
                else if (period[i].OPERA_TYPE == 2)
                {
                    strSql = new StringBuilder();
                    strSql.Append("delete from schedule_period ");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)			};
                    parameters[0].Value = period[i].HOS_ID;
                    parameters[1].Value = period[i].DEPT_CODE;
                    parameters[2].Value = period[i].DOC_NO;
                    parameters[3].Value = period[i].SCH_DATE;
                    parameters[4].Value = period[i].SCH_TIME;
                    parameters[5].Value = period[i].PERIOD_START;

                    int j = i;
                    while (j-- > 0)
                    {
                        strSql.Append(" ");
                    }
                    table.Add(strSql.ToString(), parameters);
                }
            }
            try
            {
                DbHelperMySQLZZJ.ExecuteSqlTran(table);
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    ModSqlError modSqlError = new ModSqlError();
                    modSqlError.TYPE = "获取指定医院科室排班列表";
                    modSqlError.time = DateTime.Now;
                    modSqlError.EXCEPTION = ex.ToString().Replace("'", "\"");
                    new Log.Core.MySQLDAL.DalSqlERRROR().Add(modSqlError);
                }
                catch
                {

                }
                return false;
            }
        }


        /// <summary>
        /// 获取指定医院科室排班列表
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddByTran_ZZJ(Plat.Model.schedule schedule, Plat.Model.schedule_period[] period)
        {
            StringBuilder strSql = new StringBuilder();

            System.Collections.Hashtable table = new System.Collections.Hashtable();
            if (schedule.OPERA_TYPE == 1)
            {
                if (!Exists_ZZJ(schedule.HOS_ID, schedule.DEPT_CODE, schedule.DOC_NO, schedule.SCH_DATE, schedule.SCH_TIME))
                {

                    /*strSql = new StringBuilder();
                    strSql.Append("insert into schedule(");
                    strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,SCH_TYPE,COUNT_ALL,COUNT_REM,GH_FEE,ZL_FEE,START_TIME,END_TIME,COUNT_ESB,COUNT_PAY,COUNT_DEF,WAIT_OPEN_TIME,PRO_TITLE,CAN_WAIT)");
                    strSql.Append(" values (");
                    strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@SCH_TYPE,@COUNT_ALL,@COUNT_REM,@GH_FEE,@ZL_FEE,@START_TIME,@END_TIME,@COUNT_ESB,@COUNT_PAY,@COUNT_DEF,@WAIT_OPEN_TIME,@PRO_TITLE,@CAN_WAIT)");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit)};
                    parameters[0].Value = schedule.HOS_ID;
                    parameters[1].Value = schedule.DEPT_CODE;
                    parameters[2].Value = schedule.DOC_NO;
                    parameters[3].Value = schedule.SCH_DATE;
                    parameters[4].Value = schedule.SCH_TIME;
                    parameters[5].Value = schedule.SCH_TYPE;
                    parameters[6].Value = schedule.COUNT_ALL;
                    parameters[7].Value = schedule.COUNT_REM;
                    parameters[8].Value = schedule.GH_FEE;
                    parameters[9].Value = schedule.ZL_FEE;
                    parameters[10].Value = schedule.START_TIME;
                    parameters[11].Value = schedule.END_TIME;
                    parameters[12].Value = schedule.COUNT_ESB;
                    parameters[13].Value = schedule.COUNT_PAY;
                    parameters[14].Value = schedule.COUNT_DEF;
                    parameters[15].Value = schedule.WAIT_OPEN_TIME;
                    parameters[16].Value = schedule.PRO_TITLE;
                    parameters[17].Value = schedule.CAN_WAIT;
                    */

                    strSql = new StringBuilder();
                    strSql.Append("insert into schedule(");
                    strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,SCH_TYPE,COUNT_ALL,COUNT_REM,GH_FEE,ZL_FEE,START_TIME,END_TIME,COUNT_ESB,COUNT_PAY,COUNT_DEF,WAIT_OPEN_TIME,PRO_TITLE,CAN_WAIT,REGISTER_TYPE)");
                    strSql.Append(" values (");
                    strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@SCH_TYPE,@COUNT_ALL,@COUNT_REM,@GH_FEE,@ZL_FEE,@START_TIME,@END_TIME,@COUNT_ESB,@COUNT_PAY,@COUNT_DEF,@WAIT_OPEN_TIME,@PRO_TITLE,@CAN_WAIT,@REGISTER_TYPE)");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@REGISTER_TYPE", MySqlDbType.VarChar,5)};
                    parameters[0].Value = schedule.HOS_ID;
                    parameters[1].Value = schedule.DEPT_CODE;
                    parameters[2].Value = schedule.DOC_NO;
                    parameters[3].Value = schedule.SCH_DATE;
                    parameters[4].Value = schedule.SCH_TIME;
                    parameters[5].Value = schedule.SCH_TYPE;
                    parameters[6].Value = schedule.COUNT_ALL;
                    parameters[7].Value = schedule.COUNT_REM;
                    parameters[8].Value = schedule.GH_FEE;
                    parameters[9].Value = schedule.ZL_FEE;
                    parameters[10].Value = schedule.START_TIME;
                    parameters[11].Value = schedule.END_TIME;
                    parameters[12].Value = schedule.COUNT_ESB;
                    parameters[13].Value = schedule.COUNT_PAY;
                    parameters[14].Value = schedule.COUNT_DEF;
                    parameters[15].Value = schedule.WAIT_OPEN_TIME;
                    parameters[16].Value = schedule.PRO_TITLE;
                    parameters[17].Value = schedule.CAN_WAIT;
                    parameters[18].Value = schedule.REGISTER_TYPE;
                    table.Add(strSql.ToString(), parameters);
                }
                else
                {
                    strSql = new StringBuilder();
                    /*strSql.Append("update schedule set ");
                    strSql.Append("SCH_TYPE=@SCH_TYPE,");
                    strSql.Append("COUNT_ALL=@COUNT_ALL,");
                    strSql.Append("COUNT_REM=@COUNT_REM,");
                    strSql.Append("GH_FEE=@GH_FEE,");
                    strSql.Append("ZL_FEE=@ZL_FEE,");
                    strSql.Append("START_TIME=@START_TIME,");
                    strSql.Append("END_TIME=@END_TIME,");
                    strSql.Append("COUNT_ESB=@COUNT_ESB,");
                    strSql.Append("COUNT_PAY=@COUNT_PAY,");
                    strSql.Append("COUNT_DEF=@COUNT_DEF,");
                    strSql.Append("WAIT_OPEN_TIME=@WAIT_OPEN_TIME,");
                    strSql.Append("PRO_TITLE=@PRO_TITLE,");
                    strSql.Append("CAN_WAIT=@CAN_WAIT");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)};
                    parameters[0].Value = schedule.SCH_TYPE;
                    parameters[1].Value = schedule.COUNT_ALL;
                    parameters[2].Value = schedule.COUNT_REM;
                    parameters[3].Value = schedule.GH_FEE;
                    parameters[4].Value = schedule.ZL_FEE;
                    parameters[5].Value = schedule.START_TIME;
                    parameters[6].Value = schedule.END_TIME;
                    parameters[7].Value = schedule.COUNT_ESB;
                    parameters[8].Value = schedule.COUNT_PAY;
                    parameters[9].Value = schedule.COUNT_DEF;
                    parameters[10].Value = schedule.WAIT_OPEN_TIME;
                    parameters[11].Value = schedule.PRO_TITLE;
                    parameters[12].Value = schedule.CAN_WAIT;
                    parameters[13].Value = schedule.HOS_ID;
                    parameters[14].Value = schedule.DEPT_CODE;
                    parameters[15].Value = schedule.DOC_NO;
                    parameters[16].Value = schedule.SCH_DATE;
                    parameters[17].Value = schedule.SCH_TIME;
                     */
                    strSql.Append("update schedule set ");
                    strSql.Append("SCH_TYPE=@SCH_TYPE,");
                    strSql.Append("COUNT_ALL=@COUNT_ALL,");
                    strSql.Append("COUNT_REM=@COUNT_REM,");
                    strSql.Append("GH_FEE=@GH_FEE,");
                    strSql.Append("ZL_FEE=@ZL_FEE,");
                    strSql.Append("START_TIME=@START_TIME,");
                    strSql.Append("END_TIME=@END_TIME,");
                    strSql.Append("COUNT_ESB=@COUNT_ESB,");
                    strSql.Append("COUNT_PAY=@COUNT_PAY,");
                    strSql.Append("COUNT_DEF=@COUNT_DEF,");
                    strSql.Append("WAIT_OPEN_TIME=@WAIT_OPEN_TIME,");
                    strSql.Append("PRO_TITLE=@PRO_TITLE,");
                    strSql.Append("CAN_WAIT=@CAN_WAIT,");
                    strSql.Append("REGISTER_TYPE=@REGISTER_TYPE");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@SCH_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_REM", MySqlDbType.Int32,11),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@START_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@END_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@COUNT_ESB", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_PAY", MySqlDbType.Int32,11),
					new MySqlParameter("@COUNT_DEF", MySqlDbType.Int32,11),
					new MySqlParameter("@WAIT_OPEN_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@CAN_WAIT", MySqlDbType.Bit),
					new MySqlParameter("@REGISTER_TYPE", MySqlDbType.VarChar,5),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)};
                    parameters[0].Value = schedule.SCH_TYPE;
                    parameters[1].Value = schedule.COUNT_ALL;
                    parameters[2].Value = schedule.COUNT_REM;
                    parameters[3].Value = schedule.GH_FEE;
                    parameters[4].Value = schedule.ZL_FEE;
                    parameters[5].Value = schedule.START_TIME;
                    parameters[6].Value = schedule.END_TIME;
                    parameters[7].Value = schedule.COUNT_ESB;
                    parameters[8].Value = schedule.COUNT_PAY;
                    parameters[9].Value = schedule.COUNT_DEF;
                    parameters[10].Value = schedule.WAIT_OPEN_TIME;
                    parameters[11].Value = schedule.PRO_TITLE;
                    parameters[12].Value = schedule.CAN_WAIT;
                    parameters[13].Value = schedule.REGISTER_TYPE;
                    parameters[14].Value = schedule.HOS_ID;
                    parameters[15].Value = schedule.DEPT_CODE;
                    parameters[16].Value = schedule.DOC_NO;
                    parameters[17].Value = schedule.SCH_DATE;
                    parameters[18].Value = schedule.SCH_TIME;

                    table.Add(strSql.ToString(), parameters);
                }
            }
            else if (schedule.OPERA_TYPE == 2)
            {
                strSql.Append("delete from schedule ");
                strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
                MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
                parameters[0].Value = schedule.HOS_ID;
                parameters[1].Value = schedule.DEPT_CODE;
                parameters[2].Value = schedule.DOC_NO;
                parameters[3].Value = schedule.SCH_DATE;
                parameters[4].Value = schedule.SCH_TIME;

                table.Add(strSql.ToString(), parameters);
            }

            for (int i = 0; i < period.Length; i++)
            {
                if (schedule.OPERA_TYPE == 2)//删除排班表则排版时间段表也删除
                {
                    strSql = new StringBuilder();
                    strSql.Append("delete from schedule_period ");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)	};
                    parameters[0].Value = period[i].HOS_ID;
                    parameters[1].Value = period[i].DEPT_CODE;
                    parameters[2].Value = period[i].DOC_NO;
                    parameters[3].Value = period[i].SCH_DATE;
                    parameters[4].Value = period[i].SCH_TIME;

                    int j = i;
                    while (j-- > 0)
                    {
                        strSql.Append(" ");
                    }
                    table.Add(strSql.ToString(), parameters);
                    continue;
                }
                if (period[i].OPERA_TYPE == 1)
                {
                    if (!period[i].IsExists)
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into schedule_period(");
                        strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,SCH_DATE,SCH_TIME,PERIOD_START,PERIOD_END,COUNT_ALL,COUNT_YET)");
                        strSql.Append(" values (");
                        strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@SCH_DATE,@SCH_TIME,@PERIOD_START,@PERIOD_END,@COUNT_ALL,@COUNT_YET)");

                        MySqlParameter[] parameters1 = {
					    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					    new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					    new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					    new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					    new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10),
					    new MySqlParameter("@PERIOD_END", MySqlDbType.VarChar,10),
					    new MySqlParameter("@COUNT_ALL", MySqlDbType.Int32,11),
					    new MySqlParameter("@COUNT_YET", MySqlDbType.Int32,11)};
                        parameters1[0].Value = period[i].HOS_ID;
                        parameters1[1].Value = period[i].DEPT_CODE;
                        parameters1[2].Value = period[i].DOC_NO;
                        parameters1[3].Value = period[i].SCH_DATE;
                        parameters1[4].Value = period[i].SCH_TIME;
                        parameters1[5].Value = period[i].PERIOD_START;
                        parameters1[6].Value = period[i].PERIOD_END;
                        parameters1[7].Value = period[i].COUNT_ALL;
                        parameters1[8].Value = period[i].COUNT_YET;
                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters1);
                    }
                    else
                    {
                        strSql = new StringBuilder();
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
                        parameters[0].Value = period[i].PERIOD_END;
                        parameters[1].Value = period[i].COUNT_ALL;
                        parameters[2].Value = period[i].COUNT_YET;
                        parameters[3].Value = period[i].HOS_ID;
                        parameters[4].Value = period[i].DEPT_CODE;
                        parameters[5].Value = period[i].DOC_NO;
                        parameters[6].Value = period[i].SCH_DATE;
                        parameters[7].Value = period[i].SCH_TIME;
                        parameters[8].Value = period[i].PERIOD_START;

                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                }
                else if (period[i].OPERA_TYPE == 2)
                {
                    strSql = new StringBuilder();
                    strSql.Append("delete from schedule_period ");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)			};
                    parameters[0].Value = period[i].HOS_ID;
                    parameters[1].Value = period[i].DEPT_CODE;
                    parameters[2].Value = period[i].DOC_NO;
                    parameters[3].Value = period[i].SCH_DATE;
                    parameters[4].Value = period[i].SCH_TIME;
                    parameters[5].Value = period[i].PERIOD_START;

                    int j = i;
                    while (j-- > 0)
                    {
                        strSql.Append(" ");
                    }
                    table.Add(strSql.ToString(), parameters);
                }
            }
            try
            {
                DbHelperMySQLZZJ.ExecuteSqlTran(table);
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    ModSqlError modSqlError = new ModSqlError();
                    modSqlError.TYPE = "获取指定医院科室排班列表";
                    modSqlError.time = DateTime.Now;
                    modSqlError.EXCEPTION = ex.ToString().Replace("'", "\"");
                    new Log.Core.MySQLDAL.DalSqlERRROR().Add(modSqlError);
                }
                catch
                {

                }
                return false;
            }
        }


        /// <summary>
        /// 获取自助机专家排班数据
        /// </summary>
        /// <param name="HOS_ID">医院代码</param>
        /// <param name="DEPT_CODE">科室代码</param>
        /// <param name="SCH_TYPE">排班类型</param>
        /// <param name="DOC_NO">医生代码</param>
        /// <returns></returns>
        public DataTable GetList_ZZJ(string HOS_ID, string DEPT_CODE, string SCH_TYPE, string DOC_NO)
        {
            string where = "";
            if (SCH_TYPE == "01")
            {
                where = " and date(SCH_DATE)>curdate()";
            }
            else
            {
                where = " and date(SCH_DATE) = curdate()";
            }

            string sqlcmd = string.Format(@"select distinct a.SCH_DATE,a.SCH_TIME,a.SCH_TYPE,a.COUNT_REM,a.CAN_WAIT,
a.GH_FEE,a.ZL_FEE,a.START_TIME,a.END_TIME,d.DOC_ORDER,a.PRO_TITLE
 from schedule a,hospital b,doc_info d
 where a.HOS_ID=b.HOS_ID and a.DOC_NO=d.DOC_NO   and a.HOS_ID=d.HOS_ID
and a.HOS_ID='{0}' and a.DEPT_CODE='{1}' and a.DOC_NO='{2}' {3}", HOS_ID, DEPT_CODE, DOC_NO, where);

            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
        /// <summary>
        /// 获取科室排班数据
        /// </summary>
        /// <param name="HOS_ID">医院代码</param>
        /// <param name="DEPT_CODE">科室代码</param>
        /// <param name="SCH_TYPE">排班类型</param>
        /// <returns></returns>
        public DataTable GetListKS_ZZJ(string HOS_ID, string DEPT_CODE, string SCH_TYPE)
        {
            string where = "";
            if (SCH_TYPE == "01")
            {
                where = " and date(SCH_DATE)>curdate()";
            }
            else
            {
                where = " and date(SCH_DATE) = curdate()";
            }
            string sqlcmd = string.Format(@"select *from kspb a where a.HOS_ID='{0}' and a.DEPT_CODE='{1}' {2}", HOS_ID, DEPT_CODE, where);
            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
        #endregion  ExtensionMethod
    }
}

