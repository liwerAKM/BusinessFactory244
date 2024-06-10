using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EBPP.Model;
using System.Data;

namespace EBPP.DAL
{
   public  class DALebppquery
    {
        #region  BasicMethod



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Mebppquery model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppquery(");
            strSql.Append("idcardNo,openData,EBPPID,cancel,unifiedOrgCode,OrgName,billID,TotalMoney,EBPPType,PayType,PDFUrl,openTime,CreatTime,RepealTime)");
            strSql.Append(" values (");
            strSql.Append("@idcardNo,@openData,@EBPPID,@cancel,@unifiedOrgCode,@OrgName,@billID,@TotalMoney,@EBPPType,@PayType,@PDFUrl,@openTime,@CreatTime,@RepealTime)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,18),
                    new MySqlParameter("@openData", MySqlDbType.Date),
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@cancel", MySqlDbType.Bit),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16 ,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@openTime", MySqlDbType.DateTime),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@RepealTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.idcardNo;
            parameters[1].Value = model.openData;
            parameters[2].Value = model.EBPPID;
            parameters[3].Value = model.cancel;
            parameters[4].Value = model.unifiedOrgCode;
            parameters[5].Value = model.OrgName;
            parameters[6].Value = model.billID;
            parameters[7].Value = model.TotalMoney;
            parameters[8].Value = model.EBPPType;
            parameters[9].Value = model.PayType;
            parameters[10].Value = model.PDFUrl;
            parameters[11].Value = model.openTime;
            parameters[12].Value = model.CreatTime;
            parameters[13].Value = model.RepealTime;

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
        public bool Update(Mebppquery model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ebppquery set ");
            strSql.Append("idcardNo=@idcardNo,");
            strSql.Append("openData=@openData,");
            strSql.Append("EBPPID=@EBPPID,");
            strSql.Append("cancel=@cancel,");
            strSql.Append("unifiedOrgCode=@unifiedOrgCode,");
            strSql.Append("OrgName=@OrgName,");
            strSql.Append("billID=@billID,");
            strSql.Append("TotalMoney=@TotalMoney,");
            strSql.Append("EBPPType=@EBPPType,");
            strSql.Append("PayType=@PayType,");
            strSql.Append("PDFUrl=@PDFUrl,");
            strSql.Append("openTime=@openTime,");
            strSql.Append("CreatTime=@CreatTime,");
            strSql.Append("RepealTime=@RepealTime");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,18),
                    new MySqlParameter("@openData", MySqlDbType.Date ),
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@cancel", MySqlDbType.Bit),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@openTime", MySqlDbType.DateTime),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@RepealTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.idcardNo;
            parameters[1].Value = model.openData;
            parameters[2].Value = model.EBPPID;
            parameters[3].Value = model.cancel;
            parameters[4].Value = model.unifiedOrgCode;
            parameters[5].Value = model.OrgName;
            parameters[6].Value = model.billID;
            parameters[7].Value = model.TotalMoney;
            parameters[8].Value = model.EBPPType;
            parameters[9].Value = model.PayType;
            parameters[10].Value = model.PDFUrl;
            parameters[11].Value = model.openTime;
            parameters[12].Value = model.CreatTime;
            parameters[13].Value = model.RepealTime;

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
            strSql.Append("delete from ebppquery ");
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
        public Mebppquery GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select idcardNo,openData,EBPPID,cancel,unifiedOrgCode,OrgName,billID,TotalMoney,EBPPType,PayType,PDFUrl,openTime,CreatTime,RepealTime from ebppquery ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
            };

            Mebppquery model = new Mebppquery();
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
        public Mebppquery DataRowToModel(DataRow row)
        {
            Mebppquery model = new Mebppquery();
            if (row != null)
            {
                if (row["idcardNo"] != null)
                {
                    model.idcardNo = row["idcardNo"].ToString();
                }
                if (row["openData"] != null && row["openData"].ToString() != "")
                {
                    model.openData = DateTime.Parse(row["openData"].ToString());
                }
                if (row["EBPPID"] != null && row["EBPPID"].ToString() != "")
                {
                    model.EBPPID = long.Parse(row["EBPPID"].ToString());
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
                if (row["OrgName"] != null)
                {
                    model.OrgName = row["OrgName"].ToString();
                }
                if (row["billID"] != null)
                {
                    model.billID = row["billID"].ToString();
                }
                if (row["TotalMoney"] != null && row["TotalMoney"].ToString() != "")
                {
                    model.TotalMoney = decimal.Parse(row["TotalMoney"].ToString());
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
        /// 得到一个对象实体
        /// </summary>
        public QuertInfo DataRowToQyertModel(DataRow row)
        {
            QuertInfo model = new QuertInfo();
            if (row != null)
            {
                if (row["idcardNo"] != null)
                {
                    model.idcardNo = row["idcardNo"].ToString();
                }
                if (row["openData"] != null && row["openData"].ToString() != "")
                {
                    model.openData = DateTime.Parse(row["openData"].ToString());
                }
                if (row["EBPPID"] != null && row["EBPPID"].ToString() != "")
                {
                    model.EBPPID = long.Parse(row["EBPPID"].ToString());
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
                if (row["OrgName"] != null)
                {
                    model.OrgName = row["OrgName"].ToString();
                }
                if (row["billID"] != null)
                {
                    model.billID = row["billID"].ToString();
                }
                if (row["TotalMoney"] != null && row["TotalMoney"].ToString() != "")
                {
                    model.TotalMoney = decimal.Parse(row["TotalMoney"].ToString());
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
             
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select idcardNo,openData,EBPPID,cancel,unifiedOrgCode,OrgName,billID,TotalMoney,EBPPType,PayType,PDFUrl,openTime,CreatTime,RepealTime ");
            strSql.Append(" FROM ebppquery ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable  GetList(string idcardNo ,DateTime limitData)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select idcardNo,openData,EBPPID,cancel,unifiedOrgCode,OrgName,billID,TotalMoney,EBPPType,PayType,PDFUrl,openTime,CreatTime,RepealTime ");
            strSql.Append(" FROM ebppquery  where idcardNo =@idcardNo  and openData>=@openData  and cancel =0");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,18),
                    new MySqlParameter("@openData", MySqlDbType.Date )
                 };
            parameters[0].Value = idcardNo;
            parameters[1].Value = limitData;
            return DbHelperMySQL.Query(strSql.ToString(), parameters).Tables[0];
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<QuertInfo > GetQueryList(string idcardNo, DateTime limitData)
        {
            List<QuertInfo> list = new List<QuertInfo>();
               DataTable dt = GetList(idcardNo, limitData);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add( DataRowToQyertModel(dr));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ebppquery ");
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
            strSql.Append(")AS Row, T.*  from ebppquery T ");
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
			parameters[0].Value = "ebppquery";
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
        public static void Add(Mebppquery model, MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ebppquery(");
            strSql.Append("idcardNo,openData,EBPPID,cancel,unifiedOrgCode,OrgName,billID,TotalMoney,EBPPType,PayType,PDFUrl,openTime,CreatTime,RepealTime)");
            strSql.Append(" values (");
            strSql.Append("@idcardNo,@openData,@EBPPID,@cancel,@unifiedOrgCode,@OrgName,@billID,@TotalMoney,@EBPPType,@PayType,@PDFUrl,@openTime,@CreatTime,@RepealTime)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,18),
                    new MySqlParameter("@openData", MySqlDbType.Date),
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64,20),
                    new MySqlParameter("@cancel", MySqlDbType.Bit),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16 ,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@openTime", MySqlDbType.DateTime),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@RepealTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.idcardNo;
            parameters[1].Value = model.openData;
            parameters[2].Value = model.EBPPID;
            parameters[3].Value = model.cancel;
            parameters[4].Value = model.unifiedOrgCode;
            parameters[5].Value = model.OrgName;
            parameters[6].Value = model.billID;
            parameters[7].Value = model.TotalMoney;
            parameters[8].Value = model.EBPPType;
            parameters[9].Value = model.PayType;
            parameters[10].Value = model.PDFUrl;
            parameters[11].Value = model.openTime;
            parameters[12].Value = model.CreatTime;
            parameters[13].Value = model.RepealTime;

            DbHelperMySQL.PrepareCommand(cmd, conn, trans, strSql.ToString(), parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }
        #endregion  ExtensionMethod
    }
}
