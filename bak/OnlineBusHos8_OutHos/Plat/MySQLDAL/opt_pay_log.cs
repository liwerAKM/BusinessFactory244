using System;
using System;
using System.Data;
using System.Text;
using DB.Core;
using MySql.Data.MySqlClient;
namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:opt_pay_log
    /// </summary>
    public partial class opt_pay_log 
    {
        public opt_pay_log()
        { }
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.opt_pay_log model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@STATES", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HSP_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.STATES;
            parameters[2].Value = model.HOS_ID;
            parameters[3].Value = model.PAT_ID;
            parameters[4].Value = model.HSP_SN;
            parameters[5].Value = model.JEALL;
            parameters[6].Value = model.CASH_JE;
            parameters[7].Value = model.DJ_DATE;
            parameters[8].Value = model.DJ_TIME;
            parameters[9].Value = model.lTERMINAL_SN;

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
        public bool Update(Plat.Model.opt_pay_log model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update opt_pay_log set ");
            strSql.Append("PAY_ID=@PAY_ID,");
            strSql.Append("STATES=@STATES,");
            strSql.Append("HOS_ID=@HOS_ID,");
            strSql.Append("PAT_ID=@PAT_ID,");
            strSql.Append("HSP_SN=@HSP_SN,");
            strSql.Append("JEALL=@JEALL,");
            strSql.Append("CASH_JE=@CASH_JE,");
            strSql.Append("DJ_DATE=@DJ_DATE,");
            strSql.Append("DJ_TIME=@DJ_TIME,");
            strSql.Append("lTERMINAL_SN=@lTERMINAL_SN");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@STATES", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HSP_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.STATES;
            parameters[2].Value = model.HOS_ID;
            parameters[3].Value = model.PAT_ID;
            parameters[4].Value = model.HSP_SN;
            parameters[5].Value = model.JEALL;
            parameters[6].Value = model.CASH_JE;
            parameters[7].Value = model.DJ_DATE;
            parameters[8].Value = model.DJ_TIME;
            parameters[9].Value = model.lTERMINAL_SN;

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
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_log ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
			};

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
        public Plat.Model.opt_pay_log GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN from opt_pay_log ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
			};

            Plat.Model.opt_pay_log model = new Plat.Model.opt_pay_log();
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
        public Plat.Model.opt_pay_log DataRowToModel(DataRow row)
        {
            Plat.Model.opt_pay_log model = new Plat.Model.opt_pay_log();
            if (row != null)
            {
                if (row["PAY_ID"] != null)
                {
                    model.PAY_ID = row["PAY_ID"].ToString();
                }
                if (row["STATES"] != null)
                {
                    model.STATES = row["STATES"].ToString();
                }
                if (row["HOS_ID"] != null)
                {
                    model.HOS_ID = row["HOS_ID"].ToString();
                }
                if (row["PAT_ID"] != null && row["PAT_ID"].ToString() != "")
                {
                    model.PAT_ID = int.Parse(row["PAT_ID"].ToString());
                }
                if (row["HSP_SN"] != null)
                {
                    model.HSP_SN = row["HSP_SN"].ToString();
                }
                if (row["JEALL"] != null && row["JEALL"].ToString() != "")
                {
                    model.JEALL = decimal.Parse(row["JEALL"].ToString());
                }
                if (row["CASH_JE"] != null && row["CASH_JE"].ToString() != "")
                {
                    model.CASH_JE = decimal.Parse(row["CASH_JE"].ToString());
                }
                if (row["DJ_DATE"] != null && row["DJ_DATE"].ToString() != "")
                {
                    model.DJ_DATE = DateTime.Parse(row["DJ_DATE"].ToString());
                }
                if (row["DJ_TIME"] != null)
                {
                    model.DJ_TIME = row["DJ_TIME"].ToString();
                }
                if (row["lTERMINAL_SN"] != null)
                {
                    model.lTERMINAL_SN = row["lTERMINAL_SN"].ToString();
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
            strSql.Append("select PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN ");
            strSql.Append(" FROM opt_pay_log ");
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
            strSql.Append("select count(1) FROM opt_pay_log ");
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
                strSql.Append("order by T. desc");
            }
            strSql.Append(")AS Row, T.*  from opt_pay_log T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLZZJ.Query(strSql.ToString());
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
            parameters[0].Value = "opt_pay_log";
            parameters[1].Value = "";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperMySQLZZJ.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

