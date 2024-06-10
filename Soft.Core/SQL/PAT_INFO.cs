using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DB.Core;
using MySql.Data.MySqlClient;
using Soft.Core;//Please add references

namespace Soft.Core.SQL
{
    /// <summary>
    /// 数据访问类:pat_info
    /// </summary>
    public partial class PAT_INFO
    {
        public PAT_INFO()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("PAT_ID", "pat_info");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PAT_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from pat_info");
            strSql.Append(" where PAT_ID=@PAT_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = PAT_ID;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsPatAdd(int PAT_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from pat_info_add");
            strSql.Append(" where PAT_ID=@PAT_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11)         };
            parameters[0].Value = PAT_ID;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int PAT_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from pat_info ");
            strSql.Append(" where PAT_ID=@PAT_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = PAT_ID;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool DeleteList(string PAT_IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from pat_info ");
            strSql.Append(" where PAT_ID in (" + PAT_IDlist + ")  ");
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        ///根据身份证和注册ID获取持卡人信息
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public DataTable GetBySfzAndRegID(string SFZ_SECRET, int REGPAT_ID)
        {
            string sqlcmd = string.Format(@" select * from regtopat a,register_pat b,pat_info c
            where a.PAT_ID=c.PAT_ID and a.REGPAT_ID=b.REGPAT_ID
            and c.SFZ_SECRET=@SFZ_SECRET and b.REGPAT_ID=@REGPAT_ID and c.mark_del=0", SFZ_SECRET, REGPAT_ID);


            MySqlParameter[] parameters = {
                    new MySqlParameter("@SFZ_SECRET", MySqlDbType.VarChar,11),
                    new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11)
            };
            parameters[0].Value = SFZ_SECRET;
            parameters[1].Value = REGPAT_ID;

            DataTable dtReturn = DbHelperMySQL.Query(sqlcmd, parameters).Tables[0];
            return dtReturn;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Soft.Core.Model.PAT_INFO GetModel(int PAT_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAT_ID,SFZ_NO,PAT_NAME,SEX,BIRTHDAY,ADDRESS,MOBILE_NO,GUARDIAN_NAME,GUARDIAN_SFZ_NO,CREATE_TIME,MARK_DEL,DEL_TIME,OPER_TIME,NOTE,SFZ_SECRET,MOBILE_SECRET,GUARDIAN_SFZ_NO,G_SFZ_SECRET from pat_info ");
            strSql.Append(" where PAT_ID=@PAT_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = PAT_ID;

            Soft.Core.Model.PAT_INFO model = new Soft.Core.Model.PAT_INFO();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
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
        public Soft.Core.Model.PAT_INFO DataRowToModel(DataRow row)
        {
            Soft.Core.Model.PAT_INFO model = new Soft.Core.Model.PAT_INFO();
            if (row != null)
            {
                if (row["PAT_ID"] != null && row["PAT_ID"].ToString() != "")
                {
                    model.PAT_ID = int.Parse(row["PAT_ID"].ToString());
                }
                if (row["SFZ_NO"] != null)
                {
                    model.SFZ_NO = row["SFZ_NO"].ToString();
                }
                if (row["PAT_NAME"] != null)
                {
                    model.PAT_NAME = row["PAT_NAME"].ToString();
                }
                if (row["SEX"] != null)
                {
                    model.SEX = row["SEX"].ToString();
                }
                if (row["BIRTHDAY"] != null)
                {
                    model.BIRTHDAY = row["BIRTHDAY"].ToString();
                }
                if (row["ADDRESS"] != null)
                {
                    model.ADDRESS = row["ADDRESS"].ToString();
                }
                if (row["MOBILE_NO"] != null)
                {
                    model.MOBILE_NO = row["MOBILE_NO"].ToString();
                }
                if (row["GUARDIAN_NAME"] != null)
                {
                    model.GUARDIAN_NAME = row["GUARDIAN_NAME"].ToString();
                }
                if (row["GUARDIAN_SFZ_NO"] != null)
                {
                    model.GUARDIAN_SFZ_NO = row["GUARDIAN_SFZ_NO"].ToString();
                }
                if (row["CREATE_TIME"] != null && row["CREATE_TIME"].ToString() != "")
                {
                    model.CREATE_TIME = DateTime.Parse(row["CREATE_TIME"].ToString());
                }
                if (row["MARK_DEL"] != null && row["MARK_DEL"].ToString() != "")
                {
                    if ((row["MARK_DEL"].ToString() == "1") || (row["MARK_DEL"].ToString().ToLower() == "true"))
                    {
                        model.MARK_DEL = true;
                    }
                    else
                    {
                        model.MARK_DEL = false;
                    }
                }
                if (row["DEL_TIME"] != null && row["DEL_TIME"].ToString() != "")
                {
                    model.DEL_TIME = DateTime.Parse(row["DEL_TIME"].ToString());
                }
                if (row["OPER_TIME"] != null && row["OPER_TIME"].ToString() != "")
                {
                    model.OPER_TIME = DateTime.Parse(row["OPER_TIME"].ToString());
                }
                if (row["NOTE"] != null)
                {
                    model.NOTE = row["NOTE"].ToString();
                }
                if (row["SFZ_SECRET"] != null)
                {
                    model.SFZ_SECRET = row["SFZ_SECRET"].ToString();
                }
                if (row["MOBILE_SECRET"] != null)
                {
                    model.MOBILE_SECRET = row["MOBILE_SECRET"].ToString();
                }
                if (row["GUARDIAN_SFZ_NO"] != null)
                {
                    model.GUARDIAN_SFZ_NO = row["GUARDIAN_SFZ_NO"].ToString();
                }
                if (row["G_SFZ_SECRET"] != null)
                {
                    model.G_SFZ_SECRET = row["G_SFZ_SECRET"].ToString();
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
            strSql.Append("select PAT_ID,SFZ_NO,PAT_NAME,SEX,BIRTHDAY,ADDRESS,MOBILE_NO,GUARDIAN_NAME,GUARDIAN_SFZ_NO,CREATE_TIME,MARK_DEL,DEL_TIME,OPER_TIME,NOTE ");
            strSql.Append(" FROM pat_info ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
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
                strSql.Append("order by T.PAT_ID desc");
            }
            strSql.Append(")AS Row, T.*  from pat_info T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod

    }
}
