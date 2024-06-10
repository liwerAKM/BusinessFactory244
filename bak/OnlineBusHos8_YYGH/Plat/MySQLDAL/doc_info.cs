using System;
using System.Data;
using System.Text;
using DB.Core;
using Log.Core.Model;
using MySql.Data.MySqlClient;


namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:doc_info
    /// </summary>
    public partial class doc_info 
    {
        public doc_info()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string HOS_ID, string DEPT_CODE, string DOC_NO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from doc_info");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists_ZZJ(string HOS_ID, string DEPT_CODE, string DOC_NO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from doc_info");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.doc_info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into doc_info(");
            strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,DOC_NAME,IS_EXPERT,PRO_TITLE,DOC_INTRO,IMAGE,IMAGE_TIME,DOC_ORDER,ADD_TIME,STOP_TIME,DOC_SKILLED,SPRECHSTUNDE,IMAGE_URL)");
            strSql.Append(" values (");
            strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@DOC_NAME,@IS_EXPERT,@PRO_TITLE,@DOC_INTRO,@IMAGE,@IMAGE_TIME,@DOC_ORDER,@ADD_TIME,@STOP_TIME,@DOC_SKILLED,@SPRECHSTUNDE,@IMAGE_URL)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@IS_EXPERT", MySqlDbType.Bit),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@DOC_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE", MySqlDbType.Binary),
					new MySqlParameter("@IMAGE_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_SKILLED", MySqlDbType.VarChar,200),
					new MySqlParameter("@SPRECHSTUNDE", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE_URL", MySqlDbType.VarChar,200)};
            parameters[0].Value = model.HOS_ID;
            parameters[1].Value = model.DEPT_CODE;
            parameters[2].Value = model.DOC_NO;
            parameters[3].Value = model.DOC_NAME;
            parameters[4].Value = model.IS_EXPERT;
            parameters[5].Value = model.PRO_TITLE;
            parameters[6].Value = model.DOC_INTRO;
            parameters[7].Value = model.IMAGE;
            parameters[8].Value = model.IMAGE_TIME;
            parameters[9].Value = model.DOC_ORDER;
            parameters[10].Value = model.ADD_TIME;
            parameters[11].Value = model.STOP_TIME;
            parameters[12].Value = model.DOC_SKILLED;
            parameters[13].Value = model.SPRECHSTUNDE;
            parameters[14].Value = model.IMAGE_URL;

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
        public bool Update(Plat.Model.doc_info model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update doc_info set ");
            strSql.Append("DOC_NAME=@DOC_NAME,");
            strSql.Append("IS_EXPERT=@IS_EXPERT,");
            strSql.Append("PRO_TITLE=@PRO_TITLE,");
            strSql.Append("DOC_INTRO=@DOC_INTRO,");
            strSql.Append("IMAGE=@IMAGE,");
            strSql.Append("IMAGE_TIME=@IMAGE_TIME,");
            strSql.Append("DOC_ORDER=@DOC_ORDER,");
            strSql.Append("ADD_TIME=@ADD_TIME,");
            strSql.Append("STOP_TIME=@STOP_TIME,");
            strSql.Append("DOC_SKILLED=@DOC_SKILLED,");
            strSql.Append("SPRECHSTUNDE=@SPRECHSTUNDE,");
            strSql.Append("IMAGE_URL=@IMAGE_URL");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@IS_EXPERT", MySqlDbType.Bit),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@DOC_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE", MySqlDbType.Binary),
					new MySqlParameter("@IMAGE_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_SKILLED", MySqlDbType.VarChar,200),
					new MySqlParameter("@SPRECHSTUNDE", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE_URL", MySqlDbType.VarChar,200),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.DOC_NAME;
            parameters[1].Value = model.IS_EXPERT;
            parameters[2].Value = model.PRO_TITLE;
            parameters[3].Value = model.DOC_INTRO;
            parameters[4].Value = model.IMAGE;
            parameters[5].Value = model.IMAGE_TIME;
            parameters[6].Value = model.DOC_ORDER;
            parameters[7].Value = model.ADD_TIME;
            parameters[8].Value = model.STOP_TIME;
            parameters[9].Value = model.DOC_SKILLED;
            parameters[10].Value = model.SPRECHSTUNDE;
            parameters[11].Value = model.IMAGE_URL;
            parameters[12].Value = model.HOS_ID;
            parameters[13].Value = model.DEPT_CODE;
            parameters[14].Value = model.DOC_NO;

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
        public bool Delete(string HOS_ID, string DEPT_CODE, string DOC_NO)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from doc_info ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;

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
        public Plat.Model.doc_info GetModel(string HOS_ID, string DEPT_CODE, string DOC_NO)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select HOS_ID,DEPT_CODE,DOC_NO,DOC_NAME,IS_EXPERT,PRO_TITLE,DOC_INTRO,IMAGE,IMAGE_TIME,DOC_ORDER,ADD_TIME,STOP_TIME,DOC_SKILLED,SPRECHSTUNDE,IMAGE_URL from doc_info ");
            strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)			};
            parameters[0].Value = HOS_ID;
            parameters[1].Value = DEPT_CODE;
            parameters[2].Value = DOC_NO;

            Plat.Model.doc_info model = new Plat.Model.doc_info();
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
        public Plat.Model.doc_info DataRowToModel(DataRow row)
        {
            Plat.Model.doc_info model = new Plat.Model.doc_info();
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
                if (row["DOC_NAME"] != null)
                {
                    model.DOC_NAME = row["DOC_NAME"].ToString();
                }
                if (row["IS_EXPERT"] != null && row["IS_EXPERT"].ToString() != "")
                {
                    if ((row["IS_EXPERT"].ToString() == "1") || (row["IS_EXPERT"].ToString().ToLower() == "true"))
                    {
                        model.IS_EXPERT = true;
                    }
                    else
                    {
                        model.IS_EXPERT = false;
                    }
                }
                if (row["PRO_TITLE"] != null)
                {
                    model.PRO_TITLE = row["PRO_TITLE"].ToString();
                }
                if (row["DOC_INTRO"] != null)
                {
                    model.DOC_INTRO = row["DOC_INTRO"].ToString();
                }
                if (row["IMAGE"] != null && row["IMAGE"].ToString() != "")
                {
                    model.IMAGE = (byte[])row["IMAGE"];
                }
                if (row["IMAGE_TIME"] != null && row["IMAGE_TIME"].ToString() != "")
                {
                    model.IMAGE_TIME = DateTime.Parse(row["IMAGE_TIME"].ToString());
                }
                if (row["DOC_ORDER"] != null)
                {
                    model.DOC_ORDER = row["DOC_ORDER"].ToString();
                }
                if (row["ADD_TIME"] != null && row["ADD_TIME"].ToString() != "")
                {
                    model.ADD_TIME = DateTime.Parse(row["ADD_TIME"].ToString());
                }
                if (row["STOP_TIME"] != null && row["STOP_TIME"].ToString() != "")
                {
                    model.STOP_TIME = DateTime.Parse(row["STOP_TIME"].ToString());
                }
                if (row["DOC_SKILLED"] != null)
                {
                    model.DOC_SKILLED = row["DOC_SKILLED"].ToString();
                }
                if (row["SPRECHSTUNDE"] != null)
                {
                    model.SPRECHSTUNDE = row["SPRECHSTUNDE"].ToString();
                }
                if (row["IMAGE_URL"] != null)
                {
                    model.IMAGE_URL = row["IMAGE_URL"].ToString();
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
            strSql.Append("select HOS_ID,DEPT_CODE,DOC_NO,DOC_NAME,IS_EXPERT,PRO_TITLE,DOC_INTRO,IMAGE,IMAGE_TIME,DOC_ORDER,ADD_TIME,STOP_TIME,DOC_SKILLED,SPRECHSTUNDE,IMAGE_URL ");
            strSql.Append(" FROM doc_info ");
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
            strSql.Append("select count(1) FROM doc_info ");
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
                strSql.Append("order by T.DOC_NO desc");
            }
            strSql.Append(")AS Row, T.*  from doc_info T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }
        /// <summary>
        /// 自助机获取医生列表信息
        /// </summary>
        /// <param name="USE_TYPE">01 预约 02 挂号 03不过滤</param>
        /// <param name="FILT_TYPE">01 按科室代码 02 按科室名称 03 按科室类别 04医生姓名模糊查询</param>
        /// <param name="FILT_VALUE"></param>
        /// <param name="FILT_TYPE2"></param>
        /// <param name="FILT_VALUE2"></param>
        /// <param name="RETURN_TYPE"></param>
        /// <param name="PAGEINDEX"></param>
        /// <param name="PAGESIZE"></param>
        /// <param name="PAGECOUNT"></param>
        /// <returns></returns>
        public DataTable GetDocInfo_ZZJ(string USE_TYPE, string FILT_TYPE, string FILT_VALUE,string HOS_ID, string RETURN_TYPE, int PAGEINDEX, int PAGESIZE, ref int PAGECOUNT)
        {
            //string where = USE_TYPE == "02" ? ("SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "'") : (" SCH_DATE>'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
            string where_USE_TYPE = "";
            switch (USE_TYPE)
            {
                case "01":
                    where_USE_TYPE = (" SCH_DATE>'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                    break;
                case "02":
                    if (DateTime.Now.Hour < 12)
                    {
                        where_USE_TYPE = ("SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");
                    }
                    else
                    {
                        where_USE_TYPE = ("SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and sch_time='下午'");
                    }
                    break;
            }

            string where_FILT_TYPE = "";
            switch (FILT_TYPE)
            {
                case "01":
                    where_FILT_TYPE = "f.DEPT_CODE='" + FILT_VALUE + "'";
                    break;
                case "02":
                    where_FILT_TYPE = "f.DEPT_NAME='" + FILT_VALUE + "'";
                    break;
                case "03":
                    where_FILT_TYPE = "f.DEPT_TYPE='" + FILT_VALUE + "'";
                    break;
                case "04":
                    where_FILT_TYPE = string.Format("g.doc_name like'%{0}%'",FILT_VALUE);
                    break;
            }
            string select = "";

            switch(RETURN_TYPE)
            {
                case "01":
                    select = "b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.is_expert,c.pro_title, c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url";
                    break;
                case "02":
                    select = "c.doc_no,c.doc_name,c.pro_title,DOC_ORDER";
                    break;
            }
         
            string sqlcmd = string.Format(@"select {3}  from dept_info b,doc_info c
        where b.hos_id=c.hos_id and b.dept_code=c.dept_code and b.hos_id={0}
        and c.doc_no in(
select distinct g.DOC_NO from `schedule` e,dept_info f,doc_info g
where e.HOS_ID=f.HOS_ID and e.DEPT_CODE=f.DEPT_CODE and {2} and e.SCH_TYPE=2 and e.HOS_ID={0}
and {1} and e.HOS_ID=g.HOS_ID  and  e.DEPT_CODE=g.DEPT_CODE and e.DOC_NO=g.DOC_NO) and c.DEPT_CODE='{4}'", HOS_ID, where_USE_TYPE, where_FILT_TYPE, select,FILT_VALUE);

            if (USE_TYPE == "03")
            {
                sqlcmd = string.Format(@"select distinct {1}  from dept_info b,doc_info c
        where b.hos_id=c.hos_id and b.dept_code=c.dept_code and b.hos_id={0}",HOS_ID,select);
            }


            PAGECOUNT = BaseFunction.PAGECount_ZZJ(sqlcmd);
            PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
            sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;
            return DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];
        }



        public DataTable GetDocInfo(string USE_TYPE, string FILT_TYPE, string FILT_VALUE,string FILT_TYPE2,string FILT_VALUE2, string RETURN_TYPE, int PAGEINDEX, int PAGESIZE, ref int PAGECOUNT)
        {
            /*
             类型代码如：01可用于预约的数据；02 可用于实时挂号的数据
             */



            string sqlcmd = "";
            string HOS_ID = FILT_VALUE2;

            DataTable dt=DbHelperMySQLZZJ.Query("select dept_code from dept_info WHERE hos_id='"+HOS_ID+"' and  dept_name ='"+FILT_VALUE2+"'").Tables[0];

            string where = USE_TYPE == "02" ? (" where SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd")+"'"):(" where SCH_DATE>'" + DateTime.Now.ToString("yyyy-MM-dd")+"'");
            if (USE_TYPE == "04")
            {
                where = " where SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            }


//            sqlcmd = string.Format(@"
//select a.hos_id,a.hos_name,b.dept_code,'{0}' as  dept_name,c.doc_no,(case when c.doc_name='' then b.pro_title else c.doc_name end)as doc_name,c.is_expert,c.pro_title,
//                    c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url  from hospital a,doc_info c,
//(select DISTINCT DOC_NO,dept_code,pro_title  from `schedule`{2}   and  sch_type='2' and dept_code in 
//(select  dept_code from dept_info WHERE dept_name ='{0}')) b
// where  a.hos_id=c.hos_id  and b.doc_no=c.doc_no 
//  and a.hos_id='{1}' ", FILT_VALUE, FILT_VALUE2, where);


            sqlcmd = string.Format(@"

            select distinct a.hos_id,a.hos_name,b.dept_code,'{0}' as  dept_name,c.doc_no,(case when c.doc_no='NO_DOC_Z' then c.pro_title when c.doc_no='NO_DOC_F' then c.pro_title   else c.doc_name end)as doc_name
            from hospital a,doc_info c,(select DISTINCT DOC_NO,e.dept_code,pro_title  from `schedule` e join   dept_info f on  e.dept_code =f.DEPT_CODE and e.hos_id=f.hos_id and f.HOS_ID='{1}'
            and f.dept_name='{0}' {2}   and  sch_type='2' and  e.hos_id='{1}') b where  a.hos_id=c.hos_id  and b.doc_no=c.doc_no and a.hos_id='{1}'

            ", FILT_VALUE, FILT_VALUE2, where);

//            if (DateTime.Now.Hour>=12)
//            {
//                sqlcmd = string.Format(@"
//
//            select distinct b.pro_title,a.hos_id,a.hos_name,b.dept_code,'{0}' as  dept_name,c.doc_no,(case when c.doc_no='NO_DOC_Z' then c.pro_title when c.doc_no='NO_DOC_F' then c.pro_title   else c.doc_name end)as doc_name
//            from hospital a,doc_info c,(select DISTINCT DOC_NO,e.dept_code,pro_title  from `schedule` e join   dept_info f on  e.dept_code =f.DEPT_CODE and e.hos_id=f.hos_id and f.HOS_ID='{1}'
//            and f.dept_name='{0}' {2}   and  sch_type='2' and  e.hos_id='{1}' and end_time>='12:00:00') b where  a.hos_id=c.hos_id  and b.doc_no=c.doc_no and a.hos_id='{1}'
//
//            ", FILT_VALUE, FILT_VALUE2, where);
//            }


            if (USE_TYPE == "03"&&FILT_VALUE=="")
            {
                sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.is_expert,c.pro_title,
                    c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url  from hospital a,dept_info b,doc_info c
                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and a.hos_id='{0}'", FILT_VALUE2);
            }

            if (USE_TYPE == "03"&&FILT_TYPE == "04"&&FILT_VALUE!="")
            {
                sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.is_expert,c.pro_title,
                    c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url  from hospital a,dept_info b,doc_info c
                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and a.hos_id='{0}' and c.doc_name like'%{1}%' ", FILT_VALUE2, FILT_VALUE);
            }
            if (USE_TYPE == "03" && FILT_TYPE == "02" && FILT_VALUE != "")
            {
                sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.is_expert,c.pro_title,
                    c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url  from hospital a,dept_info b,doc_info c
                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and a.hos_id='{0}' and b.dept_name='{1}' ", FILT_VALUE2, FILT_VALUE);
            }



            //if(FILT_TYPE2=="01")
            //{

            //}
//            string where = USE_TYPE == "02" ? " and a.hos_id='"+HOS_ID+"' and  c.doc_no in(select DISTINCT DOC_NO  from `schedule` where SCH_DATE='" + DateTime.Now.ToString("yyyy-MM-dd") + "')" :
//                                              " and a.hos_id='"+HOS_ID+"' and  c.doc_no in(select DISTINCT DOC_NO  from `schedule` where SCH_DATE>'" + DateTime.Now.ToString("yyyy-MM-dd")+"')";         
//            switch (FILT_TYPE)
//            {
//                case "01":
//                    sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.is_expert,c.pro_title,
//                    c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url  from hospital a,dept_info b,doc_info c
//                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and a.hos_id='{0}'{1} ", FILT_VALUE, where);
//                    if (RETURN_TYPE == "02")
//                    {
//                        sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.pro_title  from hospital a,dept_info b,doc_info c
//                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and a.hos_id='{0}' {1}", FILT_VALUE, where);
//                    }
//                    break;
//                case "02":
//                    sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.is_expert,c.pro_title,
//                    c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url  from hospital a,dept_info b,doc_info c
//                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and b.dept_name ='{0}' {1}", FILT_VALUE, where);
//                    if (RETURN_TYPE == "02")
//                    {
//                        sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.pro_title
//                    from hospital a,dept_info b,doc_info c
//                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and b.dept_name ='{0}' {1}", FILT_VALUE, where);
//                    }
//                    break;
//                case "03":
//                    sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.is_expert,c.pro_title,
//                    c.doc_intro,DOC_SKILLED,c.SPRECHSTUNDE,DOC_ORDER,c.image_url  from hospital a,dept_info b,doc_info c
//                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and b.DEPT_TYPE ='{0}' {1}", FILT_VALUE, where);
//                    if (RETURN_TYPE == "02")
//                    {
//                        sqlcmd = string.Format(@"select a.hos_id,a.hos_name,b.dept_code,b.dept_name,c.doc_no,c.doc_name,c.pro_title
//                    from hospital a,dept_info b,doc_info c
//                    where a.hos_id=b.hos_id and a.hos_id=c.hos_id and b.dept_code=c.dept_code and b.DEPT_TYPE ='{0}' {1}", FILT_VALUE, where);
//                    }
//                    break;
//            }
            if (sqlcmd == "")
            {
                return null;
            }
            else 
            {
                if (!sqlcmd.ToLower().Contains("is_expert"))
                {
                    DataTable dtDoc = DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];

                    dtDoc.Columns.Add("is_expert", typeof(string));
                    dtDoc.Columns.Add("pro_title", typeof(string));
                    dtDoc.Columns.Add("doc_intro", typeof(string));
                    dtDoc.Columns.Add("DOC_SKILLED", typeof(string));
                    dtDoc.Columns.Add("SPRECHSTUNDE", typeof(string));
                    dtDoc.Columns.Add("DOC_ORDER", typeof(string));
                    dtDoc.Columns.Add("image_url", typeof(string));

                    foreach (DataRow dr in dtDoc.Rows)
                    {
                        string HOS_ID_1 = dr["HOS_ID"].ToString().Trim();
                        string DEPT_CODE_1 = dr["DEPT_CODE"].ToString().Trim();
                        string DOC_NO_1 = dr["DOC_NO"].ToString().Trim();

                        //肿瘤模式
                        DataTable info = new BaseFunction().GetList("doc_info", "HOS_ID='" + HOS_ID_1 + "' and DOC_NO='" + DOC_NO_1 + "' and DEPT_CODE='" + DEPT_CODE_1 + "'", "is_expert,pro_title,doc_intro,DOC_SKILLED,SPRECHSTUNDE,DOC_ORDER,image_url");
                        if (info.Rows.Count == 0)
                        {
                            //儿童模式
                            info = new BaseFunction().GetList("doc_info", "HOS_ID='" + HOS_ID_1 + "' and DOC_NO='" + DOC_NO_1 + "'", "is_expert,pro_title,doc_intro,DOC_SKILLED,SPRECHSTUNDE,DOC_ORDER,image_url");
                        }

                        dr["is_expert"] = info.Rows[0]["is_expert"].ToString().Trim();
                        dr["pro_title"] = info.Rows[0]["pro_title"].ToString().Trim();
                        dr["doc_intro"] = info.Rows[0]["doc_intro"].ToString().Trim();
                        dr["DOC_SKILLED"] = info.Rows[0]["DOC_SKILLED"].ToString().Trim();
                        dr["SPRECHSTUNDE"] = info.Rows[0]["SPRECHSTUNDE"].ToString().Trim();
                        dr["DOC_ORDER"] = info.Rows[0]["DOC_ORDER"].ToString().Trim();
                        dr["image_url"] = info.Rows[0]["image_url"].ToString().Trim();
                    }
                    PAGECOUNT = dtDoc.Rows.Count;
                    PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
                    return BaseFunction.GetPagedTable(dtDoc, PAGEINDEX, PAGESIZE);
                }
                else
                {

                    PAGECOUNT = BaseFunction.PAGECount(sqlcmd);
                    PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
                    //sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + (PAGESIZE * PAGEINDEX);
                    sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;
                    return DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];
                }
            }
        }
        /// <summary>
        /// 上传医生基本信息
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public bool AddByTran(Plat.Model.doc_info[] doc)
        {
            StringBuilder strSql = new StringBuilder();
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            for (int i = 0; i < doc.Length; i++)
            {
                if (doc[i].OPERA_TYPE == 1)
                {
                    if (!Exists(doc[i].HOS_ID, doc[i].DEPT_CODE, doc[i].DOC_NO))
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into doc_info(");
                        strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,DOC_NAME,IS_EXPERT,PRO_TITLE,DOC_INTRO,IMAGE,IMAGE_TIME,DOC_ORDER,ADD_TIME,STOP_TIME,DOC_SKILLED,SPRECHSTUNDE,IMAGE_URL)");
                        strSql.Append(" values (");
                        strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@DOC_NAME,@IS_EXPERT,@PRO_TITLE,@DOC_INTRO,@IMAGE,@IMAGE_TIME,@DOC_ORDER,@ADD_TIME,@STOP_TIME,@DOC_SKILLED,@SPRECHSTUNDE,@IMAGE_URL)");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@IS_EXPERT", MySqlDbType.Bit),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@DOC_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE", MySqlDbType.Binary),
					new MySqlParameter("@IMAGE_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_SKILLED", MySqlDbType.VarChar,200),
					new MySqlParameter("@SPRECHSTUNDE", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE_URL", MySqlDbType.VarChar,200)};
                        parameters[0].Value = doc[i].HOS_ID;
                        parameters[1].Value = doc[i].DEPT_CODE;
                        parameters[2].Value = doc[i].DOC_NO;
                        parameters[3].Value = doc[i].DOC_NAME;
                        parameters[4].Value = doc[i].IS_EXPERT;
                        parameters[5].Value = doc[i].PRO_TITLE;
                        parameters[6].Value = doc[i].DOC_INTRO;
                        parameters[7].Value = doc[i].IMAGE;
                        parameters[8].Value = doc[i].IMAGE_TIME;
                        parameters[9].Value = doc[i].DOC_ORDER;
                        parameters[10].Value = DateTime.Now;
                        parameters[11].Value = doc[i].STOP_TIME;
                        parameters[12].Value = doc[i].DOC_SKILLED;
                        parameters[13].Value = doc[i].SPRECHSTUNDE;
                        parameters[14].Value = doc[i].IMAGE_URL;


                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                    else
                    {
                        strSql = new StringBuilder();
                        strSql.Append("update doc_info set ");
                        strSql.Append("DOC_NAME=@DOC_NAME,");
                        strSql.Append("PRO_TITLE=@PRO_TITLE,");
                        strSql.Append("IS_EXPERT=@IS_EXPERT,");
                        strSql.Append("ADD_TIME=@ADD_TIME");
                        strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@IS_EXPERT", MySqlDbType.Bit),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
                    new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ADD_TIME", MySqlDbType.VarChar,20)};
                        parameters[0].Value = doc[i].DOC_NAME;
                        parameters[1].Value = doc[i].IS_EXPERT;
                        parameters[2].Value = doc[i].HOS_ID;
                        parameters[3].Value = doc[i].DEPT_CODE;
                        parameters[4].Value = doc[i].DOC_NO;
                        parameters[5].Value = doc[i].PRO_TITLE;
                        parameters[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                }
                else if (doc[i].OPERA_TYPE == 2)
                {
                    doc[i].STOP_TIME = DateTime.Now;

                    strSql = new StringBuilder();
                    strSql.Append("update doc_info set STOP_TIME=@STOP_TIME");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)};
                    parameters[0].Value = doc[i].STOP_TIME;
                    parameters[1].Value = doc[i].HOS_ID;
                    parameters[2].Value = doc[i].DEPT_CODE;
                    parameters[3].Value = doc[i].DOC_NO;

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
        /// 上传医生基本信息
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public bool AddByTran_ZZJ(Plat.Model.doc_info[] doc)
        {
            StringBuilder strSql = new StringBuilder();
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            for (int i = 0; i < doc.Length; i++)
            {
                if (doc[i].OPERA_TYPE == 1)
                {
                    if (!Exists_ZZJ(doc[i].HOS_ID, doc[i].DEPT_CODE, doc[i].DOC_NO))
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into doc_info(");
                        strSql.Append("HOS_ID,DEPT_CODE,DOC_NO,DOC_NAME,IS_EXPERT,PRO_TITLE,DOC_INTRO,IMAGE,IMAGE_TIME,DOC_ORDER,ADD_TIME,STOP_TIME,DOC_SKILLED,SPRECHSTUNDE,IMAGE_URL)");
                        strSql.Append(" values (");
                        strSql.Append("@HOS_ID,@DEPT_CODE,@DOC_NO,@DOC_NAME,@IS_EXPERT,@PRO_TITLE,@DOC_INTRO,@IMAGE,@IMAGE_TIME,@DOC_ORDER,@ADD_TIME,@STOP_TIME,@DOC_SKILLED,@SPRECHSTUNDE,@IMAGE_URL)");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@IS_EXPERT", MySqlDbType.Bit),
					new MySqlParameter("@PRO_TITLE", MySqlDbType.VarChar,200),
					new MySqlParameter("@DOC_INTRO", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE", MySqlDbType.Binary),
					new MySqlParameter("@IMAGE_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@ADD_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DOC_SKILLED", MySqlDbType.VarChar,200),
					new MySqlParameter("@SPRECHSTUNDE", MySqlDbType.VarChar,200),
					new MySqlParameter("@IMAGE_URL", MySqlDbType.VarChar,200)};
                        parameters[0].Value = doc[i].HOS_ID;
                        parameters[1].Value = doc[i].DEPT_CODE;
                        parameters[2].Value = doc[i].DOC_NO;
                        parameters[3].Value = doc[i].DOC_NAME;
                        parameters[4].Value = doc[i].IS_EXPERT;
                        parameters[5].Value = doc[i].PRO_TITLE;
                        parameters[6].Value = doc[i].DOC_INTRO;
                        parameters[7].Value = doc[i].IMAGE;
                        parameters[8].Value = doc[i].IMAGE_TIME;
                        parameters[9].Value = doc[i].DOC_ORDER;
                        parameters[10].Value = DateTime.Now;
                        parameters[11].Value = doc[i].STOP_TIME;
                        parameters[12].Value = doc[i].DOC_SKILLED;
                        parameters[13].Value = doc[i].SPRECHSTUNDE;
                        parameters[14].Value = doc[i].IMAGE_URL;


                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                    else
                    {
                        strSql = new StringBuilder();
                        strSql.Append("update doc_info set ");
                        strSql.Append("DOC_NAME=@DOC_NAME,");
                        strSql.Append("IS_EXPERT=@IS_EXPERT");
                        strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
                        MySqlParameter[] parameters = {
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@IS_EXPERT", MySqlDbType.Bit),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)};
                        parameters[0].Value = doc[i].DOC_NAME;
                        parameters[1].Value = doc[i].IS_EXPERT;
                        parameters[2].Value = doc[i].HOS_ID;
                        parameters[3].Value = doc[i].DEPT_CODE;
                        parameters[4].Value = doc[i].DOC_NO;


                        int j = i;
                        while (j-- > 0)
                        {
                            strSql.Append(" ");
                        }
                        table.Add(strSql.ToString(), parameters);
                    }
                }
                else if (doc[i].OPERA_TYPE == 2)
                {
                    doc[i].STOP_TIME = DateTime.Now;

                    strSql = new StringBuilder();
                    strSql.Append("update doc_info set STOP_TIME=@STOP_TIME");
                    strSql.Append(" where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO ");
                    MySqlParameter[] parameters = {
					new MySqlParameter("@STOP_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10)};
                    parameters[0].Value = doc[i].STOP_TIME;
                    parameters[1].Value = doc[i].HOS_ID;
                    parameters[2].Value = doc[i].DEPT_CODE;
                    parameters[3].Value = doc[i].DOC_NO;

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

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

