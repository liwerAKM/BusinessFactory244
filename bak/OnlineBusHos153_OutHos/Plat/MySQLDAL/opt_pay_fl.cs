using System;
using System.Data;
using System.Text;
using DB.Core;
using MySql.Data.MySqlClient;


namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:opt_pay_fl
    /// </summary>
    public partial class opt_pay_fl
    {
        public opt_pay_fl()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string PAY_ID, string FL_NO)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from opt_pay_fl");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;
            parameters[1].Value = FL_NO;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.opt_pay_fl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_fl(");
            strSql.Append("PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@FL_NO,@FL_NAME,@DEPT_CODE,@DEPT_NAME,@FL_JE,@FL_ORDER)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@FL_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@FL_ORDER", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.FL_NO;
            parameters[2].Value = model.FL_NAME;
            parameters[3].Value = model.DEPT_CODE;
            parameters[4].Value = model.DEPT_NAME;
            parameters[5].Value = model.FL_JE;
            parameters[6].Value = model.FL_ORDER;

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
        public bool Update(Plat.Model.opt_pay_fl model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update opt_pay_fl set ");
            strSql.Append("FL_NAME=@FL_NAME,");
            strSql.Append("DEPT_CODE=@DEPT_CODE,");
            strSql.Append("DEPT_NAME=@DEPT_NAME,");
            strSql.Append("FL_JE=@FL_JE,");
            strSql.Append("FL_ORDER=@FL_ORDER");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@FL_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@FL_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@FL_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.FL_NAME;
            parameters[1].Value = model.DEPT_CODE;
            parameters[2].Value = model.DEPT_NAME;
            parameters[3].Value = model.FL_JE;
            parameters[4].Value = model.FL_ORDER;
            parameters[5].Value = model.PAY_ID;
            parameters[6].Value = model.FL_NO;

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
        public bool Delete(string PAY_ID, string FL_NO)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_fl ");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;
            parameters[1].Value = FL_NO;

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
        public Plat.Model.opt_pay_fl GetModel(string PAY_ID, string FL_NO)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER from opt_pay_fl ");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;
            parameters[1].Value = FL_NO;

            Plat.Model.opt_pay_fl model = new Plat.Model.opt_pay_fl();
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
        public Plat.Model.opt_pay_fl DataRowToModel(DataRow row)
        {
            Plat.Model.opt_pay_fl model = new Plat.Model.opt_pay_fl();
            if (row != null)
            {
                if (row["PAY_ID"] != null)
                {
                    model.PAY_ID = row["PAY_ID"].ToString();
                }
                if (row["FL_NO"] != null)
                {
                    model.FL_NO = row["FL_NO"].ToString();
                }
                if (row["FL_NAME"] != null)
                {
                    model.FL_NAME = row["FL_NAME"].ToString();
                }
                if (row["DEPT_CODE"] != null)
                {
                    model.DEPT_CODE = row["DEPT_CODE"].ToString();
                }
                if (row["DEPT_NAME"] != null)
                {
                    model.DEPT_NAME = row["DEPT_NAME"].ToString();
                }
                if (row["FL_JE"] != null && row["FL_JE"].ToString() != "")
                {
                    model.FL_JE = decimal.Parse(row["FL_JE"].ToString());
                }
                if (row["FL_ORDER"] != null)
                {
                    model.FL_ORDER = row["FL_ORDER"].ToString();
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
            strSql.Append("select PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER ");
            strSql.Append(" FROM opt_pay_fl ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList_ZZJ(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER ");
            strSql.Append(" FROM opt_pay_fl ");
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
            strSql.Append("select count(1) FROM opt_pay_fl ");
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
                strSql.Append("order by T.FL_NO desc");
            }
            strSql.Append(")AS Row, T.*  from opt_pay_fl T ");
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
            parameters[0].Value = "opt_pay_fl";
            parameters[1].Value = "FL_NO";
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

