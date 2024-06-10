using System;
using System;
using System.Data;
using System.Text;
using DB.Core;
using MySql.Data.MySqlClient;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:unionpay_tran
    /// </summary>
    public partial class unionpay_tran 
    {
        public unionpay_tran()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string ORDERID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from unionpay_tran");
            strSql.Append(" where ORDERID=@ORDERID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30)			};
            parameters[0].Value = ORDERID;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.unionpay_tran model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into unionpay_tran(");
            strSql.Append("ORDERID,MERID,QUERYID,TN,JE,currencyCode,TXN_TYPE,txnSubType,bizType,channelType,accessType,eserved,reqReserved,REFCODE,orderDesc,TERMCODE,DJ_TIME,txnTime,AT_Time,ATrespCode,ATrespMsg)");
            strSql.Append(" values (");
            strSql.Append("@ORDERID,@MERID,@QUERYID,@TN,@JE,@currencyCode,@TXN_TYPE,@txnSubType,@bizType,@channelType,@accessType,@eserved,@reqReserved,@REFCODE,@orderDesc,@TERMCODE,@DJ_TIME,@txnTime,@AT_Time,@ATrespCode,@ATrespMsg)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30),
					new MySqlParameter("@MERID", MySqlDbType.VarChar,50),
					new MySqlParameter("@QUERYID", MySqlDbType.VarChar,30),
					new MySqlParameter("@TN", MySqlDbType.VarChar,21),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@currencyCode", MySqlDbType.VarChar,5),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@txnSubType", MySqlDbType.VarChar,2),
					new MySqlParameter("@bizType", MySqlDbType.VarChar,10),
					new MySqlParameter("@channelType", MySqlDbType.VarChar,2),
					new MySqlParameter("@accessType", MySqlDbType.VarChar,1),
					new MySqlParameter("@eserved", MySqlDbType.VarChar,10),
					new MySqlParameter("@reqReserved", MySqlDbType.VarChar,10),
					new MySqlParameter("@REFCODE", MySqlDbType.VarChar,6),
					new MySqlParameter("@orderDesc", MySqlDbType.VarChar,20),
					new MySqlParameter("@TERMCODE", MySqlDbType.VarChar,15),
					new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@txnTime", MySqlDbType.DateTime),
					new MySqlParameter("@AT_Time", MySqlDbType.DateTime),
					new MySqlParameter("@ATrespCode", MySqlDbType.VarChar,10),
					new MySqlParameter("@ATrespMsg", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.ORDERID;
            parameters[1].Value = model.MERID;
            parameters[2].Value = model.QUERYID;
            parameters[3].Value = model.TN;
            parameters[4].Value = model.JE;
            parameters[5].Value = model.currencyCode;
            parameters[6].Value = model.TXN_TYPE;
            parameters[7].Value = model.txnSubType;
            parameters[8].Value = model.bizType;
            parameters[9].Value = model.channelType;
            parameters[10].Value = model.accessType;
            parameters[11].Value = model.eserved;
            parameters[12].Value = model.reqReserved;
            parameters[13].Value = model.REFCODE;
            parameters[14].Value = model.orderDesc;
            parameters[15].Value = model.TERMCODE;
            parameters[16].Value = model.DJ_TIME;
            parameters[17].Value = model.txnTime;
            parameters[18].Value = model.AT_Time;
            parameters[19].Value = model.ATrespCode;
            parameters[20].Value = model.ATrespMsg;

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
        public bool Update(Plat.Model.unionpay_tran model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update unionpay_tran set ");
            strSql.Append("MERID=@MERID,");
            strSql.Append("QUERYID=@QUERYID,");
            strSql.Append("TN=@TN,");
            strSql.Append("JE=@JE,");
            strSql.Append("currencyCode=@currencyCode,");
            strSql.Append("TXN_TYPE=@TXN_TYPE,");
            strSql.Append("txnSubType=@txnSubType,");
            strSql.Append("bizType=@bizType,");
            strSql.Append("channelType=@channelType,");
            strSql.Append("accessType=@accessType,");
            strSql.Append("eserved=@eserved,");
            strSql.Append("reqReserved=@reqReserved,");
            strSql.Append("REFCODE=@REFCODE,");
            strSql.Append("orderDesc=@orderDesc,");
            strSql.Append("TERMCODE=@TERMCODE,");
            strSql.Append("DJ_TIME=@DJ_TIME,");
            strSql.Append("txnTime=@txnTime,");
            strSql.Append("AT_Time=@AT_Time,");
            strSql.Append("ATrespCode=@ATrespCode,");
            strSql.Append("ATrespMsg=@ATrespMsg");
            strSql.Append(" where ORDERID=@ORDERID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@MERID", MySqlDbType.VarChar,50),
					new MySqlParameter("@QUERYID", MySqlDbType.VarChar,30),
					new MySqlParameter("@TN", MySqlDbType.VarChar,21),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@currencyCode", MySqlDbType.VarChar,5),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@txnSubType", MySqlDbType.VarChar,2),
					new MySqlParameter("@bizType", MySqlDbType.VarChar,10),
					new MySqlParameter("@channelType", MySqlDbType.VarChar,2),
					new MySqlParameter("@accessType", MySqlDbType.VarChar,1),
					new MySqlParameter("@eserved", MySqlDbType.VarChar,10),
					new MySqlParameter("@reqReserved", MySqlDbType.VarChar,10),
					new MySqlParameter("@REFCODE", MySqlDbType.VarChar,6),
					new MySqlParameter("@orderDesc", MySqlDbType.VarChar,20),
					new MySqlParameter("@TERMCODE", MySqlDbType.VarChar,15),
					new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@txnTime", MySqlDbType.DateTime),
					new MySqlParameter("@AT_Time", MySqlDbType.DateTime),
					new MySqlParameter("@ATrespCode", MySqlDbType.VarChar,10),
					new MySqlParameter("@ATrespMsg", MySqlDbType.VarChar,10),
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.MERID;
            parameters[1].Value = model.QUERYID;
            parameters[2].Value = model.TN;
            parameters[3].Value = model.JE;
            parameters[4].Value = model.currencyCode;
            parameters[5].Value = model.TXN_TYPE;
            parameters[6].Value = model.txnSubType;
            parameters[7].Value = model.bizType;
            parameters[8].Value = model.channelType;
            parameters[9].Value = model.accessType;
            parameters[10].Value = model.eserved;
            parameters[11].Value = model.reqReserved;
            parameters[12].Value = model.REFCODE;
            parameters[13].Value = model.orderDesc;
            parameters[14].Value = model.TERMCODE;
            parameters[15].Value = model.DJ_TIME;
            parameters[16].Value = model.txnTime;
            parameters[17].Value = model.AT_Time;
            parameters[18].Value = model.ATrespCode;
            parameters[19].Value = model.ATrespMsg;
            parameters[20].Value = model.ORDERID;

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
        public bool Delete(string ORDERID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from unionpay_tran ");
            strSql.Append(" where ORDERID=@ORDERID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30)			};
            parameters[0].Value = ORDERID;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string ORDERIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from unionpay_tran ");
            strSql.Append(" where ORDERID in (" + ORDERIDlist + ")  ");
            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString());
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
        public Plat.Model.unionpay_tran GetModel(string ORDERID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ORDERID,MERID,QUERYID,TN,JE,currencyCode,TXN_TYPE,txnSubType,bizType,channelType,accessType,eserved,reqReserved,REFCODE,orderDesc,TERMCODE,DJ_TIME,txnTime,AT_Time,ATrespCode,ATrespMsg from unionpay_tran ");
            strSql.Append(" where ORDERID=@ORDERID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30)			};
            parameters[0].Value = ORDERID;

            Plat.Model.unionpay_tran model = new Plat.Model.unionpay_tran();
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
        public Plat.Model.unionpay_tran DataRowToModel(DataRow row)
        {
            Plat.Model.unionpay_tran model = new Plat.Model.unionpay_tran();
            if (row != null)
            {
                if (row["ORDERID"] != null)
                {
                    model.ORDERID = row["ORDERID"].ToString();
                }
                if (row["MERID"] != null)
                {
                    model.MERID = row["MERID"].ToString();
                }
                if (row["QUERYID"] != null)
                {
                    model.QUERYID = row["QUERYID"].ToString();
                }
                if (row["TN"] != null)
                {
                    model.TN = row["TN"].ToString();
                }
                if (row["JE"] != null && row["JE"].ToString() != "")
                {
                    model.JE = decimal.Parse(row["JE"].ToString());
                }
                if (row["currencyCode"] != null)
                {
                    model.currencyCode = row["currencyCode"].ToString();
                }
                if (row["TXN_TYPE"] != null)
                {
                    model.TXN_TYPE = row["TXN_TYPE"].ToString();
                }
                if (row["txnSubType"] != null)
                {
                    model.txnSubType = row["txnSubType"].ToString();
                }
                if (row["bizType"] != null)
                {
                    model.bizType = row["bizType"].ToString();
                }
                if (row["channelType"] != null)
                {
                    model.channelType = row["channelType"].ToString();
                }
                if (row["accessType"] != null)
                {
                    model.accessType = row["accessType"].ToString();
                }
                if (row["eserved"] != null)
                {
                    model.eserved = row["eserved"].ToString();
                }
                if (row["reqReserved"] != null)
                {
                    model.reqReserved = row["reqReserved"].ToString();
                }
                if (row["REFCODE"] != null)
                {
                    model.REFCODE = row["REFCODE"].ToString();
                }
                if (row["orderDesc"] != null)
                {
                    model.orderDesc = row["orderDesc"].ToString();
                }
                if (row["TERMCODE"] != null)
                {
                    model.TERMCODE = row["TERMCODE"].ToString();
                }
                if (row["DJ_TIME"] != null && row["DJ_TIME"].ToString() != "")
                {
                    model.DJ_TIME = DateTime.Parse(row["DJ_TIME"].ToString());
                }
                if (row["txnTime"] != null && row["txnTime"].ToString() != "")
                {
                    model.txnTime = DateTime.Parse(row["txnTime"].ToString());
                }
                if (row["AT_Time"] != null && row["AT_Time"].ToString() != "")
                {
                    model.AT_Time = DateTime.Parse(row["AT_Time"].ToString());
                }
                if (row["ATrespCode"] != null)
                {
                    model.ATrespCode = row["ATrespCode"].ToString();
                }
                if (row["ATrespMsg"] != null)
                {
                    model.ATrespMsg = row["ATrespMsg"].ToString();
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
            strSql.Append("select ORDERID,MERID,QUERYID,TN,JE,currencyCode,TXN_TYPE,txnSubType,bizType,channelType,accessType,eserved,reqReserved,REFCODE,orderDesc,TERMCODE,DJ_TIME,txnTime,AT_Time,ATrespCode,ATrespMsg ");
            strSql.Append(" FROM unionpay_tran ");
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
            strSql.Append("select count(1) FROM unionpay_tran ");
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
                strSql.Append("order by T.ORDERID desc");
            }
            strSql.Append(")AS Row, T.*  from unionpay_tran T ");
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

        #endregion  ExtensionMethod
    }
}

