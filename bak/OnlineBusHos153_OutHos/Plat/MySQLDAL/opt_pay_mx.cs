using System;
using System.Data;
using System.Text;
using DB.Core;
using MySql.Data.MySqlClient;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:opt_pay_mx
    /// </summary>
    public partial class opt_pay_mx 
    {
        public opt_pay_mx()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string PAY_ID, string FL_NO, string ITEM_TYPE, string ITEM_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from opt_pay_mx");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO and ITEM_TYPE=@ITEM_TYPE and ITEM_ID=@ITEM_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;
            parameters[1].Value = FL_NO;
            parameters[2].Value = ITEM_TYPE;
            parameters[3].Value = ITEM_ID;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.opt_pay_mx model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_mx(");
            strSql.Append("PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@FL_NO,@ITEM_TYPE,@ITEM_ID,@ITEM_NAME,@ITEM_GG,@COUNT,@ITEM_UNIT,@COST,@je)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_NAME", MySqlDbType.VarChar,30),
					new MySqlParameter("@ITEM_GG", MySqlDbType.VarChar,30),
					new MySqlParameter("@COUNT", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_UNIT", MySqlDbType.VarChar,10),
					new MySqlParameter("@COST", MySqlDbType.Decimal,10),
					new MySqlParameter("@je", MySqlDbType.Decimal,10)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.FL_NO;
            parameters[2].Value = model.ITEM_TYPE;
            parameters[3].Value = model.ITEM_ID;
            parameters[4].Value = model.ITEM_NAME;
            parameters[5].Value = model.ITEM_GG;
            parameters[6].Value = model.COUNT;
            parameters[7].Value = model.ITEM_UNIT;
            parameters[8].Value = model.COST;
            parameters[9].Value = model.je;

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
        public bool Update(Plat.Model.opt_pay_mx model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update opt_pay_mx set ");
            strSql.Append("ITEM_NAME=@ITEM_NAME,");
            strSql.Append("ITEM_GG=@ITEM_GG,");
            strSql.Append("COUNT=@COUNT,");
            strSql.Append("ITEM_UNIT=@ITEM_UNIT,");
            strSql.Append("COST=@COST,");
            strSql.Append("je=@je");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO and ITEM_TYPE=@ITEM_TYPE and ITEM_ID=@ITEM_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@ITEM_NAME", MySqlDbType.VarChar,30),
					new MySqlParameter("@ITEM_GG", MySqlDbType.VarChar,30),
					new MySqlParameter("@COUNT", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_UNIT", MySqlDbType.VarChar,10),
					new MySqlParameter("@COST", MySqlDbType.Decimal,10),
					new MySqlParameter("@je", MySqlDbType.Decimal,10),
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.ITEM_NAME;
            parameters[1].Value = model.ITEM_GG;
            parameters[2].Value = model.COUNT;
            parameters[3].Value = model.ITEM_UNIT;
            parameters[4].Value = model.COST;
            parameters[5].Value = model.je;
            parameters[6].Value = model.PAY_ID;
            parameters[7].Value = model.FL_NO;
            parameters[8].Value = model.ITEM_TYPE;
            parameters[9].Value = model.ITEM_ID;

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
        public bool Delete(string PAY_ID, string FL_NO, string ITEM_TYPE, string ITEM_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_mx ");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO and ITEM_TYPE=@ITEM_TYPE and ITEM_ID=@ITEM_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;
            parameters[1].Value = FL_NO;
            parameters[2].Value = ITEM_TYPE;
            parameters[3].Value = ITEM_ID;

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
        public Plat.Model.opt_pay_mx GetModel(string PAY_ID, string FL_NO, string ITEM_TYPE, string ITEM_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je from opt_pay_mx ");
            strSql.Append(" where PAY_ID=@PAY_ID and FL_NO=@FL_NO and ITEM_TYPE=@ITEM_TYPE and ITEM_ID=@ITEM_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;
            parameters[1].Value = FL_NO;
            parameters[2].Value = ITEM_TYPE;
            parameters[3].Value = ITEM_ID;

            Plat.Model.opt_pay_mx model = new Plat.Model.opt_pay_mx();
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
        public Plat.Model.opt_pay_mx DataRowToModel(DataRow row)
        {
            Plat.Model.opt_pay_mx model = new Plat.Model.opt_pay_mx();
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
                if (row["ITEM_TYPE"] != null)
                {
                    model.ITEM_TYPE = row["ITEM_TYPE"].ToString();
                }
                if (row["ITEM_ID"] != null)
                {
                    model.ITEM_ID = row["ITEM_ID"].ToString();
                }
                if (row["ITEM_NAME"] != null)
                {
                    model.ITEM_NAME = row["ITEM_NAME"].ToString();
                }
                if (row["ITEM_GG"] != null)
                {
                    model.ITEM_GG = row["ITEM_GG"].ToString();
                }
                if (row["COUNT"] != null)
                {
                    model.COUNT = row["COUNT"].ToString();
                }
                if (row["ITEM_UNIT"] != null)
                {
                    model.ITEM_UNIT = row["ITEM_UNIT"].ToString();
                }
                if (row["COST"] != null && row["COST"].ToString() != "")
                {
                    model.COST = decimal.Parse(row["COST"].ToString());
                }
                if (row["je"] != null && row["je"].ToString() != "")
                {
                    model.je = decimal.Parse(row["je"].ToString());
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
            strSql.Append("select PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je ");
            strSql.Append(" FROM opt_pay_mx ");
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
            strSql.Append("select PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je ");
            strSql.Append(" FROM opt_pay_mx ");
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
            strSql.Append("select count(1) FROM opt_pay_mx ");
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
                strSql.Append("order by T.ITEM_ID desc");
            }
            strSql.Append(")AS Row, T.*  from opt_pay_mx T ");
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
            parameters[0].Value = "opt_pay_mx";
            parameters[1].Value = "ITEM_ID";
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

