﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBPP.Model;
using System.Data;

namespace EBPP.DAL
{
    public class DALebppmx
    {
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Mebppmx model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppmx(");
            strSql.Append("EBPPID,Orde,ItemType,ItemTypeCode,ItemName,ItemGG,ItemUnit,ItemPrice,ItemCount,ItemJe,SelfRate,UpLimit,SelfFee,DeductFee,D1)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@Orde,@ItemType,@ItemTypeCode,@ItemName,@ItemGG,@ItemUnit,@ItemPrice,@ItemCount,@ItemJe,@SelfRate,@UpLimit,@SelfFee,@DeductFee,@D1)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@Orde", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ItemType", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemTypeCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@ItemGG", MySqlDbType.VarChar,50),
                    new MySqlParameter("@ItemUnit", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemPrice", MySqlDbType.Decimal,10),
                    new MySqlParameter("@ItemCount", MySqlDbType.Decimal,10),
                    new MySqlParameter("@ItemJe", MySqlDbType.Decimal,12),
                    new MySqlParameter("@SelfRate", MySqlDbType.Decimal,4),
                    new MySqlParameter("@UpLimit", MySqlDbType.VarChar,20),
                    new MySqlParameter("@SelfFee", MySqlDbType.Decimal,12),
                    new MySqlParameter("@DeductFee", MySqlDbType.Decimal,12),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.Orde;
            parameters[2].Value = model.ItemType;
            parameters[3].Value = model.ItemTypeCode;
            parameters[4].Value = model.ItemName;
            parameters[5].Value = model.ItemGG;
            parameters[6].Value = model.ItemUnit;
            parameters[7].Value = model.ItemPrice;
            parameters[8].Value = model.ItemCount;
            parameters[9].Value = model.ItemJe;
            parameters[10].Value = model.SelfRate;
            parameters[11].Value = model.UpLimit;
            parameters[12].Value = model.SelfFee;
            parameters[13].Value = model.DeductFee;
            parameters[14].Value = model.D1;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(Mebppmx model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ebppmx set ");
            strSql.Append("EBPPID=@EBPPID,");
            strSql.Append("Orde=@Orde,");
            strSql.Append("ItemType=@ItemType,");
            strSql.Append("ItemTypeCode=@ItemTypeCode,");
            strSql.Append("ItemName=@ItemName,");
            strSql.Append("ItemGG=@ItemGG,");
            strSql.Append("ItemUnit=@ItemUnit,");
            strSql.Append("ItemPrice=@ItemPrice,");
            strSql.Append("ItemCount=@ItemCount,");
            strSql.Append("ItemJe=@ItemJe,");
            strSql.Append("SelfRate=@SelfRate,");
            strSql.Append("UpLimit=@UpLimit,");
            strSql.Append("SelfFee=@SelfFee,");
            strSql.Append("DeductFee=@DeductFee,");
            strSql.Append("D1=@D1");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@Orde", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ItemType", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemTypeCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@ItemGG", MySqlDbType.VarChar,50),
                    new MySqlParameter("@ItemUnit", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemPrice", MySqlDbType.Decimal,10),
                    new MySqlParameter("@ItemCount", MySqlDbType.Decimal,10),
                    new MySqlParameter("@ItemJe", MySqlDbType.Decimal,12),
                    new MySqlParameter("@SelfRate", MySqlDbType.Decimal,4),
                    new MySqlParameter("@UpLimit", MySqlDbType.VarChar,20),
                    new MySqlParameter("@SelfFee", MySqlDbType.Decimal,12),
                    new MySqlParameter("@DeductFee", MySqlDbType.Decimal,12),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.Orde;
            parameters[2].Value = model.ItemType;
            parameters[3].Value = model.ItemTypeCode;
            parameters[4].Value = model.ItemName;
            parameters[5].Value = model.ItemGG;
            parameters[6].Value = model.ItemUnit;
            parameters[7].Value = model.ItemPrice;
            parameters[8].Value = model.ItemCount;
            parameters[9].Value = model.ItemJe;
            parameters[10].Value = model.SelfRate;
            parameters[11].Value = model.UpLimit;
            parameters[12].Value = model.SelfFee;
            parameters[13].Value = model.DeductFee;
            parameters[14].Value = model.D1;

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
        /// 删除一条数据
        /// </summary>
        public bool Delete()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ebppmx ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
            };

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
        /// 得到一个对象实体
        /// </summary>
        public Mebppmx GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EBPPID,Orde,ItemType,ItemTypeCode,ItemName,ItemGG,ItemUnit,ItemPrice,ItemCount,ItemJe,SelfRate,UpLimit,SelfFee,DeductFee,D1 from ebppmx ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
            };

            Mebppmx model = new Mebppmx();
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
        public Mebppmx DataRowToModel(DataRow row)
        {
            Mebppmx model = new Mebppmx();
            if (row != null)
            {
                if (row["EBPPID"] != null && row["EBPPID"].ToString() != "")
                {
                    model.EBPPID = long.Parse(row["EBPPID"].ToString());
                }
                if (row["Orde"] != null)
                {
                    model.Orde = row["Orde"].ToString();
                }
                if (row["ItemType"] != null)
                {
                    model.ItemType = row["ItemType"].ToString();
                }
                if (row["ItemTypeCode"] != null)
                {
                    model.ItemTypeCode = row["ItemTypeCode"].ToString();
                }
                if (row["ItemName"] != null)
                {
                    model.ItemName = row["ItemName"].ToString();
                }
                if (row["ItemGG"] != null)
                {
                    model.ItemGG = row["ItemGG"].ToString();
                }
                if (row["ItemUnit"] != null)
                {
                    model.ItemUnit = row["ItemUnit"].ToString();
                }
                if (row["ItemPrice"] != null && row["ItemPrice"].ToString() != "")
                {
                    model.ItemPrice = decimal.Parse(row["ItemPrice"].ToString());
                }
                if (row["ItemCount"] != null && row["ItemCount"].ToString() != "")
                {
                    model.ItemCount = decimal.Parse(row["ItemCount"].ToString());
                }
                if (row["ItemJe"] != null && row["ItemJe"].ToString() != "")
                {
                    model.ItemJe = decimal.Parse(row["ItemJe"].ToString());
                }
                if (row["SelfRate"] != null && row["SelfRate"].ToString() != "")
                {
                    model.SelfRate = decimal.Parse(row["SelfRate"].ToString());
                }
                if (row["UpLimit"] != null)
                {
                    model.UpLimit = row["UpLimit"].ToString();
                }
                if (row["SelfFee"] != null && row["SelfFee"].ToString() != "")
                {
                    model.SelfFee = decimal.Parse(row["SelfFee"].ToString());
                }
                if (row["DeductFee"] != null && row["DeductFee"].ToString() != "")
                {
                    model.DeductFee = decimal.Parse(row["DeductFee"].ToString());
                }
                if (row["D1"] != null && row["D1"].ToString() != "")
                {
                    model.D1 = DateTime.Parse(row["D1"].ToString());
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
            strSql.Append("select EBPPID,Orde,ItemType,ItemTypeCode,ItemName,ItemGG,ItemUnit,ItemPrice,ItemCount,ItemJe,SelfRate,UpLimit,SelfFee,DeductFee,D1 ");
            strSql.Append(" FROM ebppmx ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ebppmx ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQL.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.EBPPID desc");
            }
            strSql.Append(")AS Row, T.*  from ebppmx T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQL.Query(strSql.ToString());
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
			parameters[0].Value = "ebppmx";
			parameters[1].Value = "EBPPID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperMySQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public static void Add(Mebppmx model, MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppmx(");
            strSql.Append("EBPPID,Orde,ItemType,ItemTypeCode,ItemName,ItemGG,ItemUnit,ItemPrice,ItemCount,ItemJe,SelfRate,UpLimit,SelfFee,DeductFee,D1)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@Orde,@ItemType,@ItemTypeCode,@ItemName,@ItemGG,@ItemUnit,@ItemPrice,@ItemCount,@ItemJe,@SelfRate,@UpLimit,@SelfFee,@DeductFee,@D1)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@Orde", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ItemType", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemTypeCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@ItemGG", MySqlDbType.VarChar,50),
                    new MySqlParameter("@ItemUnit", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ItemPrice", MySqlDbType.Decimal,10),
                    new MySqlParameter("@ItemCount", MySqlDbType.Decimal,10),
                    new MySqlParameter("@ItemJe", MySqlDbType.Decimal,12),
                    new MySqlParameter("@SelfRate", MySqlDbType.Decimal,4),
                    new MySqlParameter("@UpLimit", MySqlDbType.VarChar,20),
                    new MySqlParameter("@SelfFee", MySqlDbType.Decimal,12),
                    new MySqlParameter("@DeductFee", MySqlDbType.Decimal,12),
                    new MySqlParameter("@D1", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.Orde;
            parameters[2].Value = model.ItemType;
            parameters[3].Value = model.ItemTypeCode;
            parameters[4].Value = model.ItemName;
            parameters[5].Value = model.ItemGG;
            parameters[6].Value = model.ItemUnit;
            parameters[7].Value = model.ItemPrice;
            parameters[8].Value = model.ItemCount;
            parameters[9].Value = model.ItemJe;
            parameters[10].Value = model.SelfRate;
            parameters[11].Value = model.UpLimit;
            parameters[12].Value = model.SelfFee;
            parameters[13].Value = model.DeductFee;
            parameters[14].Value = model.D1;

            DbHelperMySQL.PrepareCommand(cmd, conn, trans, strSql.ToString(), parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        #endregion  ExtensionMethod
    }
}
