using System;
using System.Data;
using System.Text;
using DB.Core;
using Log.Core.Model;
using MySql.Data.MySqlClient;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:dept_info
    /// </summary>
    public partial class dept_info 
    {
        public dept_info()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string HOS_ID, string DEPT_CODE)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dept_info");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists_ZZJ(string HOS_ID, string DEPT_CODE)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from dept_info");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.dept_info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dept_info(");
            strSql.Append("HOS_ID,DEPT_CODE,DEPT_CODE_ESB,DEPT_NAME,DEPT_INTRO,DEPT_URL,DEPT_TEL,DEPT_SUP_CODE,DEPT_TYPE,DEPT_ORDER,DEPT_ADDRESS,DEPT_USE,ADD_DATE,STOP_DATE)");
            strSql.Append(" values (");
            strSql.Append("@HOS_ID,@DEPT_CODE,@DEPT_CODE_ESB,@DEPT_NAME,@DEPT_INTRO,@DEPT_URL,@DEPT_TEL,@DEPT_SUP_CODE,@DEPT_TYPE,@DEPT_ORDER,@DEPT_ADDRESS,@DEPT_USE,@ADD_DATE,@STOP_DATE)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@DEPT_URL", MySqlDbType.VarChar,100),
					new MySqlParameter("@DEPT_TEL", MySqlDbType.VarChar,15),
					new MySqlParameter("@DEPT_SUP_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_ADDRESS", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_USE", MySqlDbType.VarChar,30),
					new MySqlParameter("@ADD_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime)};
            parameters[0].Value = model.HOS_ID;
            parameters[1].Value = model.DEPT_CODE;
            parameters[2].Value = model.DEPT_CODE_ESB;
            parameters[3].Value = model.DEPT_NAME;
            parameters[4].Value = model.DEPT_INTRO;
            parameters[5].Value = model.DEPT_URL;
            parameters[6].Value = model.DEPT_TEL;
            parameters[7].Value = model.DEPT_SUP_CODE;
            parameters[8].Value = model.DEPT_TYPE;
            parameters[9].Value = model.DEPT_ORDER;
            parameters[10].Value = model.DEPT_ADDRESS;
            parameters[11].Value = model.DEPT_USE;
            parameters[12].Value = model.ADD_DATE;
            parameters[13].Value = model.STOP_DATE;

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
        public bool Update(Plat.Model.dept_info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update dept_info set ");
            strSql.Append("DEPT_CODE_ESB=@DEPT_CODE_ESB,");
            strSql.Append("DEPT_NAME=@DEPT_NAME,");
            strSql.Append("DEPT_INTRO=@DEPT_INTRO,");
            strSql.Append("DEPT_URL=@DEPT_URL,");
            strSql.Append("DEPT_TEL=@DEPT_TEL,");
            strSql.Append("DEPT_SUP_CODE=@DEPT_SUP_CODE,");
            strSql.Append("DEPT_TYPE=@DEPT_TYPE,");
            strSql.Append("DEPT_ORDER=@DEPT_ORDER,");
            strSql.Append("DEPT_ADDRESS=@DEPT_ADDRESS,");
            strSql.Append("DEPT_USE=@DEPT_USE,");
            strSql.Append("ADD_DATE=@ADD_DATE,");
            strSql.Append("STOP_DATE=@STOP_DATE");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@DEPT_URL", MySqlDbType.VarChar,100),
					new MySqlParameter("@DEPT_TEL", MySqlDbType.VarChar,15),
					new MySqlParameter("@DEPT_SUP_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_ADDRESS", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_USE", MySqlDbType.VarChar,30),
					new MySqlParameter("@ADD_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.DEPT_CODE_ESB;
            parameters[1].Value = model.DEPT_NAME;
            parameters[2].Value = model.DEPT_INTRO;
            parameters[3].Value = model.DEPT_URL;
            parameters[4].Value = model.DEPT_TEL;
            parameters[5].Value = model.DEPT_SUP_CODE;
            parameters[6].Value = model.DEPT_TYPE;
            parameters[7].Value = model.DEPT_ORDER;
            parameters[8].Value = model.DEPT_ADDRESS;
            parameters[9].Value = model.DEPT_USE;
            parameters[10].Value = model.ADD_DATE;
            parameters[11].Value = model.STOP_DATE;
            parameters[12].Value = model.HOS_ID;
            parameters[13].Value = model.DEPT_CODE;

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
        public bool Delete(string HOS_ID, string DEPT_CODE)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dept_info ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;

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
        public Plat.Model.dept_info GetModel(string HOS_ID, string DEPT_CODE)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HOS_ID,DEPT_CODE,DEPT_CODE_ESB,DEPT_NAME,DEPT_INTRO,DEPT_URL,DEPT_TEL,DEPT_SUP_CODE,DEPT_TYPE,DEPT_ORDER,DEPT_ADDRESS,DEPT_USE,ADD_DATE,STOP_DATE from dept_info ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;

            Plat.Model.dept_info model = new Plat.Model.dept_info();
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
        public Plat.Model.dept_info DataRowToModel(DataRow row)
        {
            Plat.Model.dept_info model = new Plat.Model.dept_info();
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
                if (row["DEPT_CODE_ESB"] != null)
                {
                    model.DEPT_CODE_ESB = row["DEPT_CODE_ESB"].ToString();
                }
                if (row["DEPT_NAME"] != null)
                {
                    model.DEPT_NAME = row["DEPT_NAME"].ToString();
                }
                if (row["DEPT_INTRO"] != null)
                {
                    model.DEPT_INTRO = row["DEPT_INTRO"].ToString();
                }
                if (row["DEPT_URL"] != null)
                {
                    model.DEPT_URL = row["DEPT_URL"].ToString();
                }
                if (row["DEPT_TEL"] != null)
                {
                    model.DEPT_TEL = row["DEPT_TEL"].ToString();
                }
                if (row["DEPT_SUP_CODE"] != null)
                {
                    model.DEPT_SUP_CODE = row["DEPT_SUP_CODE"].ToString();
                }
                if (row["DEPT_TYPE"] != null)
                {
                    model.DEPT_TYPE = row["DEPT_TYPE"].ToString();
                }
                if (row["DEPT_ORDER"] != null)
                {
                    model.DEPT_ORDER = row["DEPT_ORDER"].ToString();
                }
                if (row["DEPT_ADDRESS"] != null)
                {
                    model.DEPT_ADDRESS = row["DEPT_ADDRESS"].ToString();
                }
                if (row["DEPT_USE"] != null)
                {
                    model.DEPT_USE = row["DEPT_USE"].ToString();
                }
                if (row["ADD_DATE"] != null && row["ADD_DATE"].ToString() != "")
                {
                    model.ADD_DATE = DateTime.Parse(row["ADD_DATE"].ToString());
                }
                if (row["STOP_DATE"] != null && row["STOP_DATE"].ToString() != "")
                {
                    model.STOP_DATE = DateTime.Parse(row["STOP_DATE"].ToString());
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
            strSql.Append("select HOS_ID,DEPT_CODE,DEPT_CODE_ESB,DEPT_NAME,DEPT_INTRO,DEPT_URL,DEPT_TEL,DEPT_SUP_CODE,DEPT_TYPE,DEPT_ORDER,DEPT_ADDRESS,DEPT_USE,ADD_DATE,STOP_DATE ");
            strSql.Append(" FROM dept_info ");
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
            strSql.Append("select count(1) FROM dept_info ");
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
                strSql.Append("order by T.DEPT_CODE desc");
            }
            strSql.Append(")AS Row, T.*  from dept_info T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }

        /// <summary>
        /// 自助机获取科室信息
        /// </summary>
        /// <param name="USE_TYPE">01 科室预约 02 科室挂号 05专家预约 06专家挂号 空值不过滤</param>
        /// <param name="FILT_TYPE">01 按大科 02 按科室名称 03 按科室类别 04 按科室代码 05 不过滤</param>
        /// <param name="FILT_VALUE"></param>
        /// <param name="HOS_ID"></param>
        /// <param name="RETURN_TYPE"></param>
        /// <param name="PAGEINDEX"></param>
        /// <param name="PAGESIZE"></param>
        /// <param name="PAGECOUNT"></param>
        /// <returns></returns>
        public DataTable GetDeptInfo_ZZJ(string USE_TYPE, string FILT_TYPE, string FILT_VALUE,string HOS_ID,string RETURN_TYPE, int PAGEINDEX, int PAGESIZE, ref int PAGECOUNT)
        {
            string select = "";
            string sqlcmd = "";

            switch (RETURN_TYPE)
            {
                case "01":
                    select = "a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_INTRO,b.DEPT_URL,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS";
                    break;
                case "02":
                    select = "b.DEPT_CODE,b.DEPT_NAME,b.DEPT_ORDER";
                    break;
            }

            if (FILT_TYPE == "01")
            {
                sqlcmd = string.Format(@"select {0} from hospital a,dept_info b where a.HOS_ID=b.HOS_ID and a.HOS_ID={1}
                and (DEPT_SUP_CODE is null or trim(DEPT_SUP_CODE)='')",select,HOS_ID);
            }
            else if (USE_TYPE == "")
            {
                sqlcmd = string.Format(@"SELECT {0}
from hospital a,dept_info b
 where a.HOS_ID=b.HOS_ID and a.HOS_ID='{1}'",select,HOS_ID);
            }
            else
            {

                string where_USE_TYPE = "";
                switch (USE_TYPE)
                {
                    case "01":
                        where_USE_TYPE = ("  and sch_type=1 and SCH_DATE>'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                        break;
                    case "02":
                        if (DateTime.Now.Hour < 12)
                        {
                            where_USE_TYPE = (" and sch_type=1 and SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                        }
                        else
                        {
                            where_USE_TYPE = (" and sch_type=1 and SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and sch_time='下午'");
                        }
                        break;
                    case "05":
                        where_USE_TYPE = ("  and sch_type=2 and SCH_DATE>'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                        break;
                    case "06":
                        if (DateTime.Now.Hour < 12)
                        {
                            where_USE_TYPE = (" and sch_type=2 and SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                        }
                        else
                        {
                            where_USE_TYPE = (" and sch_type=2 and SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and sch_time='下午'");
                        }
                        break;
                    case "07":
                        where_USE_TYPE = (" and sch_type=3 and SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                        break;
                    case "08":
                        where_USE_TYPE = (" and sch_type in(1,2) and SCH_DATE>'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                        break;
                    case "09":
                        where_USE_TYPE = (" and sch_type in(1,2) and SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                        break;
                }

                string where_FILT_TYPE = "";
                switch (FILT_TYPE)
                {
                    case "02":
                        where_FILT_TYPE = " and c.DEPT_NAME='" + FILT_VALUE + "'";
                        break;
                    case "03":
                        where_FILT_TYPE = " and c.DEPT_TYPE='" + FILT_VALUE + "'";
                        break;
                    case "04":
                        where_FILT_TYPE = " and c.DEPT_CODE='" + FILT_VALUE + "'";
                        break;
                }

                sqlcmd = string.Format(@"SELECT {0}
from hospital a,dept_info b,dept_info c
 where a.HOS_ID=b.HOS_ID and a.HOS_ID=c.HOS_ID
and b.DEPT_SUP_CODE=c.DEPT_CODE {1} and a.HOS_ID={2} and b.dept_code in(select dept_code from `schedule` where hos_id={2} {3} )", select, where_FILT_TYPE, HOS_ID, where_USE_TYPE);//and count_rem>0

                if (FILT_TYPE!="01"&&FILT_VALUE=="")
                {
                    where_FILT_TYPE = " and 1=1";
                    sqlcmd = string.Format(@"SELECT {0}
from hospital a,dept_info b
 where a.HOS_ID=b.HOS_ID
and a.HOS_ID={2} and b.dept_code in(select dept_code from `schedule` where hos_id={2} {3} )", select, where_FILT_TYPE, HOS_ID, where_USE_TYPE);//and count_rem>0
                }
            }

            PAGECOUNT = BaseFunction.PAGECount_ZZJ(sqlcmd);
            PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
            sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;
            return DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];
        }


        #endregion  BasicMethod
        #region  ExtensionMethod
        public DataTable GetDeptInfo(string USE_TYPE, string FILT_TYPE, string FILT_VALUE, string RETURN_TYPE, int PAGEINDEX, int PAGESIZE, ref int PAGECOUNT)
        {
            /*
             01可用于预约的数据（科室）；
02 可用于实时挂号的数据；（科室）
03 可用于住院申请的数据；
04 可用于实时叫号的数据（科室）
05可用于预约的数据（专家）
06可用于实时挂号的数据（专家）
07可用于实时叫号的数据（专家）
空值不过滤
             */
            string sqlcmd = "";
            string selectWord = "a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_INTRO,b.DEPT_URL,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS";
            string where = "";
            if (RETURN_TYPE == "02")
            {
                selectWord = " a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS";
            }
            if (FILT_TYPE == "01")//医院代码
            {
                where = " and a.hos_id='" + FILT_VALUE + "'";
            }
            else if (FILT_TYPE == "02")
            {
                where = " and b.dept_name='" + FILT_VALUE + "'";
            }
            else if (FILT_TYPE == "03")
            {
                where = " and b.DEPT_TYPE='" + FILT_VALUE + "'";
            }
            switch (USE_TYPE)
            {
                case "01":
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info_yyks b
                    where a.hos_id=b.hos_id {1}",selectWord,where);
                    break;
                case "02":
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info_ghks b
                    where a.hos_id=b.hos_id {1}", selectWord, where);
                    break;
                case "03":
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info_zyks b
                    where a.hos_id=b.hos_id {1}", selectWord, where);
                    break;
                case "04":
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info_jhks b
                    where a.hos_id=b.hos_id {1}", selectWord, where);
                    break;
                case "05":
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info_yyzj b
                    where a.hos_id=b.hos_id {1}", selectWord, where);
                    break;
                case "06":
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info_ghzj b
                    where a.hos_id=b.hos_id {1}", selectWord, where);
                    break;
                case "07":
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info_jhzj b
                    where a.hos_id=b.hos_id {1}", selectWord, where);
                    break;
                default:
                    sqlcmd = string.Format(@"select distinct {0} from hospital a,dept_info b
                    where a.hos_id=b.hos_id {1}", selectWord, where);
                    break;
                    
            }


            string con = "";
            if (FILT_TYPE == "01")
            {
                string srsql = string.Format(@"select 1 from platform.dept_info where HOS_ID='{0}' and  (dept_order is null or dept_order='')", FILT_VALUE);
                if (DbHelperMySQLZZJ.Query(srsql).Tables[0].Rows.Count == 0)
                {
                    con = " order by dept_order";
                    sqlcmd += con;
                }
            }
            if (con == "")
            {
                sqlcmd += " order by dept_code";
            }

            PAGECOUNT = BaseFunction.PAGECountUnionAll(sqlcmd);
            PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
            //sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + (PAGESIZE * PAGEINDEX);
            sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;
            return DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];

            /*
            string sch_type = "1";
            if (USE_TYPE == "05" || USE_TYPE == "06" || USE_TYPE == "07")
            {
                sch_type = "2";
            }
            string sqlcmd = "";//
            string where = USE_TYPE == "" ? "" : " and LOCATE('" + USE_TYPE + "',b.DEPT_USE)>0";

            if (USE_TYPE == "01"||USE_TYPE=="05")
            {
                sqlcmd = string.Format(@"select DISTINCT DEPT_CODE from `schedule` where hos_id='{0}' and sch_date>CURDATE() and sch_type={1}",FILT_VALUE,sch_type);
            }
            //else if(USE_TYPE=="02"||USE_TYPE=="04"||USE_TYPE=="06"||USE_TYPE=="07")
            else if (USE_TYPE == "02" || USE_TYPE == "06" || USE_TYPE == "07")
            {
                sqlcmd = string.Format(@"select DISTINCT DEPT_CODE from `schedule` where hos_id='{0}' and sch_date=CURDATE() and sch_type={1}", FILT_VALUE,sch_type);
            }
            DataTable dtSche=sqlcmd==""?new DataTable(): DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dtSche.Rows)
            {
                sb.Append(dr[0].ToString().Trim() + ",");
            }
            string dept_codes = sb.Length > 0 ? sb.ToString().TrimEnd(',') : "";

            string  where1=dept_codes==""?"":" and b.dept_code in("+dept_codes+")";

            //if (USE_TYPE == "01" || USE_TYPE == "02"||USE_TYPE=="04"||USE_TYPE=="06"||USE_TYPE=="05"||USE_TYPE=="07")
            if (USE_TYPE == "01" || USE_TYPE == "02" || USE_TYPE == "06" || USE_TYPE == "05" || USE_TYPE == "07")
            {
                if (where1 == "")
                {
                    return new DataTable();
                }
            }
            /*
             01:按医院ID
02：按科室名称
03：按科室类别

             
            switch (FILT_TYPE)
            {
                case "01":
                    sqlcmd = string.Format(@"select a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_INTRO,b.DEPT_URL,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                    from hospital a,dept_info b
                    where a.hos_id=b.hos_id and a.hos_id='{0}' {1} {2}", FILT_VALUE, where,where1);
                    if (RETURN_TYPE == "02")
                    {
                        sqlcmd = string.Format(@"select a.HOS_ID,a.hos_name,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                        from hospital a,dept_info b
                        where a.hos_id=b.hos_id and a.hos_id='{0}' {1} {2}", FILT_VALUE, where,where1);
                    }
                    break;
                case "02":
                    sqlcmd = string.Format(@"select a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_INTRO,b.DEPT_URL,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                    from hospital a,dept_info b
                    where a.hos_id=b.hos_id and b.dept_name ='{0}' {1} {2}", FILT_VALUE, where, where1);
                    if (RETURN_TYPE == "02")
                    {
                        sqlcmd = string.Format(@"select a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                        from hospital a,dept_info b
                        where a.hos_id=b.hos_id and b.dept_name ='{0}' {1} {2}", FILT_VALUE, where, where1);
                    }
                    break;
                case "03":
                    sqlcmd = string.Format(@"select a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_INTRO,b.DEPT_URL,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                    from hospital a,dept_info b
                    where a.hos_id=b.hos_id and b.DEPT_TYPE ='{0}' {1} ", FILT_VALUE, where);
                    if (RETURN_TYPE == "02")
                    {
                        sqlcmd = string.Format(@"select a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                        from hospital a,dept_info b
                        where a.hos_id=b.hos_id and b.DEPT_TYPE ='{0}' {1}", FILT_VALUE, where);
                    }
                    break;
            }
            if (USE_TYPE == "")
            {
                sqlcmd = string.Format(@"select a.HOS_ID,a.HOS_NAME,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_INTRO,b.DEPT_URL,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                    from hospital a,dept_info b
                    where a.hos_id=b.hos_id and a.hos_id='{0}'", FILT_VALUE);
                if (RETURN_TYPE == "02")
                {
                    sqlcmd = string.Format(@"select a.HOS_ID,a.hos_name,b.DEPT_CODE,b.DEPT_NAME,b.DEPT_ORDER,b.DEPT_TYPE,b.DEPT_ADDRESS
                        from hospital a,dept_info b
                         where a.hos_id=b.hos_id and a.hos_id='{0}'", FILT_VALUE);
                }
            }
            if (sqlcmd == "")
            {
                return null;
            }
            else
            {
                PAGECOUNT = BaseFunction.PAGECount(sqlcmd);
                PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
                //sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + (PAGESIZE * PAGEINDEX);
                sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;
                return DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];
            }
            */
        }
        /// <summary>
        /// 上传科室基本信息
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public bool AddByTran(Plat.Model.dept_info[] dept)
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            StringBuilder strSql = new StringBuilder();
            for (int i = 0; i < dept.Length; i++)
            {
                if (dept[i].OPERA_TYPE == 1)
                {
                    if (!Exists(dept[i].HOS_ID, dept[i].DEPT_CODE))
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into dept_info(");
                        strSql.Append("HOS_ID,DEPT_CODE,DEPT_CODE_ESB,DEPT_NAME,DEPT_INTRO,DEPT_URL,DEPT_TEL,DEPT_SUP_CODE,DEPT_TYPE,DEPT_ORDER,DEPT_ADDRESS,DEPT_USE,ADD_DATE,STOP_DATE)");
                        strSql.Append(" values (");
                        strSql.Append("@HOS_ID,@DEPT_CODE,@DEPT_CODE_ESB,@DEPT_NAME,@DEPT_INTRO,@DEPT_URL,@DEPT_TEL,@DEPT_SUP_CODE,@DEPT_TYPE,@DEPT_ORDER,@DEPT_ADDRESS,@DEPT_USE,@ADD_DATE,@STOP_DATE)");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@DEPT_URL", MySqlDbType.VarChar,100),
					new MySqlParameter("@DEPT_TEL", MySqlDbType.VarChar,15),
					new MySqlParameter("@DEPT_SUP_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_ADDRESS", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_USE", MySqlDbType.VarChar,30),
					new MySqlParameter("@ADD_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime)};
                        parameters[0].Value = dept[i].HOS_ID;
                        parameters[1].Value = dept[i].DEPT_CODE;
                        parameters[2].Value = dept[i].DEPT_CODE_ESB;
                        parameters[3].Value = dept[i].DEPT_NAME;
                        parameters[4].Value = dept[i].DEPT_INTRO;
                        parameters[5].Value = dept[i].DEPT_URL;
                        parameters[6].Value = dept[i].DEPT_TEL;
                        parameters[7].Value = dept[i].DEPT_SUP_CODE;
                        parameters[8].Value = dept[i].DEPT_TYPE;
                        parameters[9].Value = dept[i].DEPT_ORDER;
                        parameters[10].Value = dept[i].DEPT_ADDRESS;
                        parameters[11].Value = dept[i].DEPT_USE;
                        parameters[12].Value = dept[i].ADD_DATE;
                        parameters[13].Value = dept[i].STOP_DATE;

                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                    else
                    {
                    //    strSql = new StringBuilder();
                    //    strSql.Append("update dept_info set ");
                    //    strSql.Append("DEPT_CODE_ESB=@DEPT_CODE_ESB,");
                    //    strSql.Append("DEPT_NAME=@DEPT_NAME,");
                    //    strSql.Append("DEPT_INTRO=@DEPT_INTRO,");
                    //    strSql.Append("DEPT_URL=@DEPT_URL,");
                    //    strSql.Append("DEPT_TEL=@DEPT_TEL,");
                    //    strSql.Append("DEPT_SUP_CODE=@DEPT_SUP_CODE,");
                    //    strSql.Append("DEPT_TYPE=@DEPT_TYPE,");
                    //    strSql.Append("DEPT_ORDER=@DEPT_ORDER,");
                    //    strSql.Append("DEPT_ADDRESS=@DEPT_ADDRESS,");
                    //    strSql.Append("DEPT_USE=@DEPT_USE,");
                    //    strSql.Append("ADD_DATE=@ADD_DATE,");
                    //    strSql.Append("STOP_DATE=@STOP_DATE");
                    //    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
                    //    MySqlParameter[] parameters = {
                    //new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
                    //new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
                    //new MySqlParameter("@DEPT_INTRO", MySqlDbType.VarChar,200),
                    //new MySqlParameter("@DEPT_URL", MySqlDbType.VarChar,100),
                    //new MySqlParameter("@DEPT_TEL", MySqlDbType.VarChar,15),
                    //new MySqlParameter("@DEPT_SUP_CODE", MySqlDbType.VarChar,10),
                    //new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
                    //new MySqlParameter("@DEPT_ORDER", MySqlDbType.VarChar,10),
                    //new MySqlParameter("@DEPT_ADDRESS", MySqlDbType.VarChar,30),
                    //new MySqlParameter("@DEPT_USE", MySqlDbType.VarChar,30),
                    //new MySqlParameter("@ADD_DATE", MySqlDbType.DateTime),
                    //new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime),
                    //new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
                    //new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)};
                    //    parameters[0].Value = dept[i].DEPT_CODE_ESB;
                    //    parameters[1].Value = dept[i].DEPT_NAME;
                    //    parameters[2].Value = dept[i].DEPT_INTRO;
                    //    parameters[3].Value = dept[i].DEPT_URL;
                    //    parameters[4].Value = dept[i].DEPT_TEL;
                    //    parameters[5].Value = dept[i].DEPT_SUP_CODE;
                    //    parameters[6].Value = dept[i].DEPT_TYPE;
                    //    parameters[7].Value = dept[i].DEPT_ORDER;
                    //    parameters[8].Value = dept[i].DEPT_ADDRESS;
                    //    parameters[9].Value = dept[i].DEPT_USE;
                    //    parameters[10].Value = dept[i].ADD_DATE;
                    //    parameters[11].Value = dept[i].STOP_DATE;
                    //    parameters[12].Value = dept[i].HOS_ID;
                    //    parameters[13].Value = dept[i].DEPT_CODE;


                        strSql = new StringBuilder();
                        strSql.Append("update dept_info set ");
                        strSql.Append("DEPT_CODE_ESB=@DEPT_CODE_ESB,");
                        strSql.Append("DEPT_NAME=@DEPT_NAME,");
                        strSql.Append("DEPT_TYPE=@DEPT_TYPE,");
                        strSql.Append("DEPT_ORDER=@DEPT_ORDER");
                        strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
                    new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
                    new MySqlParameter("@DEPT_ORDER", MySqlDbType.VarChar,5),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)};
                        parameters[0].Value = dept[i].DEPT_CODE_ESB;
                        parameters[1].Value = dept[i].DEPT_NAME;
                        parameters[2].Value = dept[i].DEPT_TYPE;
                        parameters[3].Value = dept[i].DEPT_ORDER;
                        parameters[4].Value = dept[i].HOS_ID;
                        parameters[5].Value = dept[i].DEPT_CODE;

                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                }
                else if (dept[i].OPERA_TYPE == 2)
                {
                    strSql = new StringBuilder();
                    dept[i].STOP_DATE = DateTime.Now;
                    strSql.Append("update dept_info set ");
                    strSql.Append("STOP_DATE=@STOP_DATE");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)};
                    parameters[0].Value = dept[i].STOP_DATE;
                    parameters[1].Value = dept[i].HOS_ID;
                    parameters[2].Value = dept[i].DEPT_CODE;

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
                    modSqlError.TYPE = "上传医生基本信息";
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
        /// 上传科室基本信息
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public bool AddByTran_ZZJ(Plat.Model.dept_info[] dept)
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            StringBuilder strSql = new StringBuilder();
            for (int i = 0; i < dept.Length; i++)
            {
                if (dept[i].OPERA_TYPE == 1)
                {
                    if (!Exists_ZZJ(dept[i].HOS_ID, dept[i].DEPT_CODE))
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into dept_info(");
                        strSql.Append("HOS_ID,DEPT_CODE,DEPT_CODE_ESB,DEPT_NAME,DEPT_INTRO,DEPT_URL,DEPT_TEL,DEPT_SUP_CODE,DEPT_TYPE,DEPT_ORDER,DEPT_ADDRESS,DEPT_USE,ADD_DATE,STOP_DATE)");
                        strSql.Append(" values (");
                        strSql.Append("@HOS_ID,@DEPT_CODE,@DEPT_CODE_ESB,@DEPT_NAME,@DEPT_INTRO,@DEPT_URL,@DEPT_TEL,@DEPT_SUP_CODE,@DEPT_TYPE,@DEPT_ORDER,@DEPT_ADDRESS,@DEPT_USE,@ADD_DATE,@STOP_DATE)");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@DEPT_URL", MySqlDbType.VarChar,100),
					new MySqlParameter("@DEPT_TEL", MySqlDbType.VarChar,15),
					new MySqlParameter("@DEPT_SUP_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_ADDRESS", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_USE", MySqlDbType.VarChar,30),
					new MySqlParameter("@ADD_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime)};
                        parameters[0].Value = dept[i].HOS_ID;
                        parameters[1].Value = dept[i].DEPT_CODE;
                        parameters[2].Value = dept[i].DEPT_CODE_ESB;
                        parameters[3].Value = dept[i].DEPT_NAME;
                        parameters[4].Value = dept[i].DEPT_INTRO;
                        parameters[5].Value = dept[i].DEPT_URL;
                        parameters[6].Value = dept[i].DEPT_TEL;
                        parameters[7].Value = dept[i].DEPT_SUP_CODE;
                        parameters[8].Value = dept[i].DEPT_TYPE;
                        parameters[9].Value = dept[i].DEPT_ORDER;
                        parameters[10].Value = dept[i].DEPT_ADDRESS;
                        parameters[11].Value = dept[i].DEPT_USE;
                        parameters[12].Value = dept[i].ADD_DATE;
                        parameters[13].Value = dept[i].STOP_DATE;

                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                    else
                    {
                        //    strSql = new StringBuilder();
                        //    strSql.Append("update dept_info set ");
                        //    strSql.Append("DEPT_CODE_ESB=@DEPT_CODE_ESB,");
                        //    strSql.Append("DEPT_NAME=@DEPT_NAME,");
                        //    strSql.Append("DEPT_INTRO=@DEPT_INTRO,");
                        //    strSql.Append("DEPT_URL=@DEPT_URL,");
                        //    strSql.Append("DEPT_TEL=@DEPT_TEL,");
                        //    strSql.Append("DEPT_SUP_CODE=@DEPT_SUP_CODE,");
                        //    strSql.Append("DEPT_TYPE=@DEPT_TYPE,");
                        //    strSql.Append("DEPT_ORDER=@DEPT_ORDER,");
                        //    strSql.Append("DEPT_ADDRESS=@DEPT_ADDRESS,");
                        //    strSql.Append("DEPT_USE=@DEPT_USE,");
                        //    strSql.Append("ADD_DATE=@ADD_DATE,");
                        //    strSql.Append("STOP_DATE=@STOP_DATE");
                        //    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
                        //    MySqlParameter[] parameters = {
                        //new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
                        //new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
                        //new MySqlParameter("@DEPT_INTRO", MySqlDbType.VarChar,200),
                        //new MySqlParameter("@DEPT_URL", MySqlDbType.VarChar,100),
                        //new MySqlParameter("@DEPT_TEL", MySqlDbType.VarChar,15),
                        //new MySqlParameter("@DEPT_SUP_CODE", MySqlDbType.VarChar,10),
                        //new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
                        //new MySqlParameter("@DEPT_ORDER", MySqlDbType.VarChar,10),
                        //new MySqlParameter("@DEPT_ADDRESS", MySqlDbType.VarChar,30),
                        //new MySqlParameter("@DEPT_USE", MySqlDbType.VarChar,30),
                        //new MySqlParameter("@ADD_DATE", MySqlDbType.DateTime),
                        //new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime),
                        //new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
                        //new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)};
                        //    parameters[0].Value = dept[i].DEPT_CODE_ESB;
                        //    parameters[1].Value = dept[i].DEPT_NAME;
                        //    parameters[2].Value = dept[i].DEPT_INTRO;
                        //    parameters[3].Value = dept[i].DEPT_URL;
                        //    parameters[4].Value = dept[i].DEPT_TEL;
                        //    parameters[5].Value = dept[i].DEPT_SUP_CODE;
                        //    parameters[6].Value = dept[i].DEPT_TYPE;
                        //    parameters[7].Value = dept[i].DEPT_ORDER;
                        //    parameters[8].Value = dept[i].DEPT_ADDRESS;
                        //    parameters[9].Value = dept[i].DEPT_USE;
                        //    parameters[10].Value = dept[i].ADD_DATE;
                        //    parameters[11].Value = dept[i].STOP_DATE;
                        //    parameters[12].Value = dept[i].HOS_ID;
                        //    parameters[13].Value = dept[i].DEPT_CODE;


                        strSql = new StringBuilder();
                        strSql.Append("update dept_info set ");
                        strSql.Append("DEPT_CODE_ESB=@DEPT_CODE_ESB,");
                        strSql.Append("DEPT_NAME=@DEPT_NAME,");
                        strSql.Append("DEPT_TYPE=@DEPT_TYPE");
                        strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@DEPT_CODE_ESB", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
                    new MySqlParameter("@DEPT_TYPE", MySqlDbType.VarChar,20),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)};
                        parameters[0].Value = dept[i].DEPT_CODE_ESB;
                        parameters[1].Value = dept[i].DEPT_NAME;
                        parameters[2].Value = dept[i].DEPT_TYPE;
                        parameters[3].Value = dept[i].HOS_ID;
                        parameters[4].Value = dept[i].DEPT_CODE;

                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                }
                else if (dept[i].OPERA_TYPE == 2)
                {
                    strSql = new StringBuilder();
                    dept[i].STOP_DATE = DateTime.Now;
                    strSql.Append("update dept_info set ");
                    strSql.Append("STOP_DATE=@STOP_DATE");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@STOP_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10)};
                    parameters[0].Value = dept[i].STOP_DATE;
                    parameters[1].Value = dept[i].HOS_ID;
                    parameters[2].Value = dept[i].DEPT_CODE;

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
                    modSqlError.TYPE = "上传医生基本信息";
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
        #endregion  ExtensionMethod
    }
}

