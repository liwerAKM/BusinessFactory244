using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBPP.Model;
using System.Data;

namespace EBPP.DAL
{
  public   class DALebppmain
    {
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long EBPPID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ebppmain");
            strSql.Append(" where EBPPID=@EBPPID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20)            };
            parameters[0].Value = EBPPID;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Mebppmain model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppmain(");
            strSql.Append("EBPPID,idcardNo,cancel,unifiedOrgCode,billID,billCode,TotalMoney,PatName,EBPPType,PayType,PDFUrl,OrgName,openTime,CreatTime,RepealTime)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@idcardNo,@cancel,@unifiedOrgCode,@billID,@billCode,@TotalMoney,@PatName,@EBPPType,@PayType,@PDFUrl,@OrgName,@openTime,@CreatTime,@RepealTime)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,18),
                    new MySqlParameter("@cancel", MySqlDbType.Bit),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billCode", MySqlDbType.VarChar,10),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@PatName", MySqlDbType.VarChar,20),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@openTime", MySqlDbType.DateTime),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@RepealTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.idcardNo;
            parameters[2].Value = model.cancel;
            parameters[3].Value = model.unifiedOrgCode;
            parameters[4].Value = model.billID;
            parameters[5].Value = model.billCode;
            parameters[6].Value = model.TotalMoney;
            parameters[7].Value = model.PatName;
            parameters[8].Value = model.EBPPType;
            parameters[9].Value = model.PayType;
            parameters[10].Value = model.PDFUrl;
            parameters[11].Value = model.OrgName;
            parameters[12].Value = model.openTime;
            parameters[13].Value = model.CreatTime;
            parameters[14].Value = model.RepealTime;

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
        public bool Update(Mebppmain model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ebppmain set ");
            strSql.Append("idcardNo=@idcardNo,");
            strSql.Append("cancel=@cancel,");
            strSql.Append("unifiedOrgCode=@unifiedOrgCode,");
            strSql.Append("billID=@billID,");
            strSql.Append("billCode=@billCode,");
            strSql.Append("TotalMoney=@TotalMoney,");
            strSql.Append("PatName=@PatName,");
            strSql.Append("EBPPType=@EBPPType,");
            strSql.Append("PayType=@PayType,");
            strSql.Append("PDFUrl=@PDFUrl,");
            strSql.Append("openTime=@openTime,");
            strSql.Append("CreatTime=@CreatTime,");
            strSql.Append("RepealTime=@RepealTime");
            strSql.Append(" where EBPPID=@EBPPID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,18),
                    new MySqlParameter("@cancel", MySqlDbType.Bit),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billCode", MySqlDbType.VarChar,10),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@PatName", MySqlDbType.VarChar,20),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16 ,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16 ,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@openTime", MySqlDbType.DateTime),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@RepealTime", MySqlDbType.DateTime),
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20)};
            parameters[0].Value = model.idcardNo;
            parameters[1].Value = model.cancel;
            parameters[2].Value = model.unifiedOrgCode;
            parameters[3].Value = model.billID;
            parameters[4].Value = model.billCode;
            parameters[5].Value = model.TotalMoney;
            parameters[6].Value = model.PatName;
            parameters[7].Value = model.EBPPType;
            parameters[8].Value = model.PayType;
            parameters[9].Value = model.PDFUrl;
            parameters[10].Value = model.openTime;
            parameters[11].Value = model.CreatTime;
            parameters[12].Value = model.RepealTime;
            parameters[13].Value = model.EBPPID;

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
        public bool Delete(long EBPPID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ebppmain ");
            strSql.Append(" where EBPPID=@EBPPID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20)            };
            parameters[0].Value = EBPPID;

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
        public bool DeleteList(string EBPPIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ebppmain ");
            strSql.Append(" where EBPPID in (" + EBPPIDlist + ")  ");
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


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Mebppmain GetModel(long EBPPID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EBPPID,idcardNo,cancel,unifiedOrgCode,billID,billCode,TotalMoney,PatName,EBPPType,PayType,OrgName,PDFUrl,openTime,CreatTime,RepealTime from ebppmain ");
            strSql.Append(" where EBPPID=@EBPPID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20)            };
            parameters[0].Value = EBPPID;

            Mebppmain model = new Mebppmain();
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
        public DataTable  GetModel( string unifiedOrgCode,string billID,string billCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select EBPPID,idcardNo,cancel,unifiedOrgCode,billID,billCode,TotalMoney,PatName,EBPPType,PayType,OrgName,PDFUrl,openTime,CreatTime,RepealTime from ebppmain ");
            strSql.Append(" where  ");
            strSql.Append("    unifiedOrgCode=@unifiedOrgCode ");
            strSql.Append("  and  billID=@billID ");
            strSql.Append("  and  billCode=@billCode ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                       new MySqlParameter("@billCode", MySqlDbType.VarChar,10)
                };
            parameters[0].Value = unifiedOrgCode;
            parameters[1].Value = billID;
                  parameters[2].Value = billCode;


            Mebppmain model = new Mebppmain();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return  ds.Tables[0];
            }
            else
            {
                return null;
            }
        }


       
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Mebppmain DataRowToModel(DataRow row)
        {
            Mebppmain model = new Mebppmain();
            if (row != null)
            {
                if (row["EBPPID"] != null && row["EBPPID"].ToString() != "")
                {
                    model.EBPPID = long.Parse(row["EBPPID"].ToString());
                }
                if (row["idcardNo"] != null)
                {
                    model.idcardNo = row["idcardNo"].ToString();
                }
                if (row["cancel"] != null && row["cancel"].ToString() != "")
                {
                    if ((row["cancel"].ToString() == "1") || (row["cancel"].ToString().ToLower() == "true"))
                    {
                        model.cancel = true;
                    }
                    else
                    {
                        model.cancel = false;
                    }
                }
                if (row["unifiedOrgCode"] != null)
                {
                    model.unifiedOrgCode = row["unifiedOrgCode"].ToString();
                }
                if (row["billID"] != null)
                {
                    model.billID = row["billID"].ToString();
                }
                if (row["billCode"] != null)
                {
                    model.billCode = row["billCode"].ToString();
                }
                if (row["TotalMoney"] != null && row["TotalMoney"].ToString() != "")
                {
                    model.TotalMoney = decimal.Parse(row["TotalMoney"].ToString());
                }
                if (row["PatName"] != null)
                {
                    model.PatName = row["PatName"].ToString();
                }
                if (row["EBPPType"] != null && row["EBPPType"].ToString() != "")
                {
                    model.EBPPType = int.Parse(row["EBPPType"].ToString());
                }
                if (row["PayType"] != null && row["PayType"].ToString() != "")
                {
                    model.PayType = int.Parse(row["PayType"].ToString());
                }
                if (row["PDFUrl"] != null)
                {
                    model.PDFUrl = row["PDFUrl"].ToString();
                }
                if (row["OrgName"] != null)
                {
                    model.OrgName = row["OrgName"].ToString();
                }
                if (row["openTime"] != null && row["openTime"].ToString() != "")
                {
                    model.openTime = DateTime.Parse(row["openTime"].ToString());
                }
                if (row["CreatTime"] != null && row["CreatTime"].ToString() != "")
                {
                    model.CreatTime = DateTime.Parse(row["CreatTime"].ToString());
                }
                if (row["RepealTime"] != null && row["RepealTime"].ToString() != "")
                {
                    model.RepealTime = DateTime.Parse(row["RepealTime"].ToString());
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
            strSql.Append("select EBPPID,idcardNo,cancel,unifiedOrgCode,billID,TotalMoney,PatName,EBPPType,PayType,OrgName,PDFUrl,openTime,CreatTime,RepealTime ");
            strSql.Append(" FROM ebppmain ");
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
            strSql.Append("select count(1) FROM ebppmain ");
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
            strSql.Append(")AS Row, T.*  from ebppmain T ");
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
			parameters[0].Value = "ebppmain";
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
        public static void Add(Mebppmain model, MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans)
        {
           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppmain(");
            strSql.Append("EBPPID,idcardNo,cancel,unifiedOrgCode,billID,billCode,TotalMoney,PatName,EBPPType,PayType,PDFUrl,OrgName,openTime,CreatTime,RepealTime)");
            strSql.Append(" values (");
            strSql.Append("@EBPPID,@idcardNo,@cancel,@unifiedOrgCode,@billID,@billCode,@TotalMoney,@PatName,@EBPPType,@PayType,@PDFUrl,@OrgName,@openTime,@CreatTime,@RepealTime)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,18),
                    new MySqlParameter("@cancel", MySqlDbType.Bit),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billCode", MySqlDbType.VarChar,10),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@PatName", MySqlDbType.VarChar,20),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@openTime", MySqlDbType.DateTime),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@RepealTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.EBPPID;
            parameters[1].Value = model.idcardNo;
            parameters[2].Value = model.cancel;
            parameters[3].Value = model.unifiedOrgCode;
            parameters[4].Value = model.billID;
            parameters[5].Value = model.billCode;
            parameters[6].Value = model.TotalMoney;
            parameters[7].Value = model.PatName;
            parameters[8].Value = model.EBPPType;
            parameters[9].Value = model.PayType;
            parameters[10].Value = model.PDFUrl;
            parameters[11].Value = model.OrgName;
            parameters[12].Value = model.openTime;
            parameters[13].Value = model.CreatTime;
            parameters[14].Value = model.RepealTime;

            DbHelperMySQL.PrepareCommand(cmd, conn, trans, strSql.ToString(), parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        #endregion  ExtensionMethod
    }
}
