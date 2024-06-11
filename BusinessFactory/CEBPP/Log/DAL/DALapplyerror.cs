using System;
using EBPP.Log.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP.Log.DAL
{
 public    class DALapplyerror
    {
        public DALapplyerror()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from applyerror where 0=1");

            return DbHelperMySQLLog.Exists(strSql.ToString());
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Mapplyerror model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into applyerror(");
            strSql.Append("applyTime,unifiedOrgCode,OrgName,billID,billCode,TotalMoney,idcardNo,PatName,EBPPType,PayType,PDFUrl,CreatTime,InData,OutData)");
            strSql.Append(" values (");
            strSql.Append("@applyTime,@unifiedOrgCode,@OrgName,@billID,@billCode,@TotalMoney,@idcardNo,@PatName,@EBPPType,@PayType,@PDFUrl,@CreatTime,@InData,@OutData)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@applyTime", MySqlDbType.DateTime),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,20),
                    new MySqlParameter("@PatName", MySqlDbType.VarChar,20),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@InData", MySqlDbType.LongText),
                    new MySqlParameter("@OutData", MySqlDbType.LongText)};
            parameters[0].Value = model.applyTime;
            parameters[1].Value = model.unifiedOrgCode;
            parameters[2].Value = model.OrgName;
            parameters[3].Value = model.billID;
            parameters[4].Value = model.billCode;
            parameters[5].Value = model.TotalMoney;
            parameters[6].Value = model.idcardNo;
            parameters[7].Value = model.PatName;
            parameters[8].Value = model.EBPPType;
            parameters[9].Value = model.PayType;
            parameters[10].Value = model.PDFUrl;
            parameters[11].Value = model.CreatTime;
            parameters[12].Value = model.InData;
            parameters[13].Value = model.OutData;

            int rows = DbHelperMySQLLog.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Update(Mapplyerror model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update applyerror set ");
            strSql.Append("applyTime=@applyTime,");
            strSql.Append("unifiedOrgCode=@unifiedOrgCode,");
            strSql.Append("OrgName=@OrgName,");
            strSql.Append("billID=@billID,");
            strSql.Append("billCode=@billCode,");
            strSql.Append("TotalMoney=@TotalMoney,");
            strSql.Append("idcardNo=@idcardNo,");
            strSql.Append("PatName=@PatName,");
            strSql.Append("EBPPType=@EBPPType,");
            strSql.Append("PayType=@PayType,");
            strSql.Append("PDFUrl=@PDFUrl,");
            strSql.Append("CreatTime=@CreatTime,");
            strSql.Append("InData=@InData,");
            strSql.Append("OutData=@OutData");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@applyTime", MySqlDbType.DateTime),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@OrgName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@billID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@billCode", MySqlDbType.VarChar,20),
                    new MySqlParameter("@TotalMoney", MySqlDbType.Decimal,12),
                    new MySqlParameter("@idcardNo", MySqlDbType.VarChar,20),
                    new MySqlParameter("@PatName", MySqlDbType.VarChar,20),
                    new MySqlParameter("@EBPPType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PayType", MySqlDbType.Int16,6),
                    new MySqlParameter("@PDFUrl", MySqlDbType.VarChar,200),
                    new MySqlParameter("@CreatTime", MySqlDbType.DateTime),
                    new MySqlParameter("@InData", MySqlDbType.LongText),
                    new MySqlParameter("@OutData", MySqlDbType.LongText)};
            parameters[0].Value = model.applyTime;
            parameters[1].Value = model.unifiedOrgCode;
            parameters[2].Value = model.OrgName;
            parameters[3].Value = model.billID;
            parameters[4].Value = model.billCode;
            parameters[5].Value = model.TotalMoney;
            parameters[6].Value = model.idcardNo;
            parameters[7].Value = model.PatName;
            parameters[8].Value = model.EBPPType;
            parameters[9].Value = model.PayType;
            parameters[10].Value = model.PDFUrl;
            parameters[11].Value = model.CreatTime;
            parameters[12].Value = model.InData;
            parameters[13].Value = model.OutData;

            int rows = DbHelperMySQLLog.ExecuteSql(strSql.ToString(), parameters);
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
            strSql.Append("delete from applyerror ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
            };

            int rows = DbHelperMySQLLog.ExecuteSql(strSql.ToString(), parameters);
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
        public Mapplyerror GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select applyTime,unifiedOrgCode,OrgName,billID,billCode,TotalMoney,idcardNo,PatName,EBPPType,PayType,PDFUrl,CreatTime,InData,OutData from applyerror ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {
            };

            Mapplyerror model = new Mapplyerror();
            DataSet ds = DbHelperMySQLLog.Query(strSql.ToString(), parameters);
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
        public Mapplyerror DataRowToModel(DataRow row)
        {
            Mapplyerror model = new Mapplyerror();
            if (row != null)
            {
                if (row["applyTime"] != null && row["applyTime"].ToString() != "")
                {
                    model.applyTime = DateTime.Parse(row["applyTime"].ToString());
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
                if (row["billCode"] != null)
                {
                    model.billCode = row["billCode"].ToString();
                }
                if (row["TotalMoney"] != null && row["TotalMoney"].ToString() != "")
                {
                    model.TotalMoney = decimal.Parse(row["TotalMoney"].ToString());
                }
                if (row["idcardNo"] != null)
                {
                    model.idcardNo = row["idcardNo"].ToString();
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
                if (row["CreatTime"] != null && row["CreatTime"].ToString() != "")
                {
                    model.CreatTime = DateTime.Parse(row["CreatTime"].ToString());
                }
                if (row["InData"] != null)
                {
                    model.InData = row["InData"].ToString();
                }
                if (row["OutData"] != null)
                {
                    model.OutData = row["OutData"].ToString();
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
            strSql.Append("select applyTime,unifiedOrgCode,OrgName,billID,billCode,TotalMoney,idcardNo,PatName,EBPPType,PayType,PDFUrl,CreatTime,InData,OutData ");
            strSql.Append(" FROM applyerror ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLLog.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM applyerror ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQLLog.GetSingle(strSql.ToString());
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
            strSql.Append(")AS Row, T.*  from applyerror T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLLog.Query(strSql.ToString());
        }



        #endregion  BasicMethod

        /// <summary>
        /// 获取上传数据 按发票生成时间
        /// </summary>
        public List<MapplyerrorQ> GetListQ(DateTime StartDate, DateTime EndDate, string unifiedOrgCode,bool ContainsInData)
        {
            List<MapplyerrorQ> list = new List<MapplyerrorQ>();
            StringBuilder strSql = new StringBuilder();
            if(ContainsInData)
               strSql.Append(@"select  * from   applyerror ");
            else
                strSql.Append("select applyTime,unifiedOrgCode,OrgName,billID,billCode,TotalMoney,idcardNo,PatName,EBPPType,PayType,PDFUrl,OutData  FROM applyerror ");

            strSql.Append("where applyTime between @StartDate  and @EndDate ");
            if (!string.IsNullOrEmpty(unifiedOrgCode))
            {
                strSql.Append("   and  unifiedOrgCode=@unifiedOrgCode ");
            }

            MySqlParameter[] parameters = {
                    new MySqlParameter("@StartDate", MySqlDbType.DateTime),
                     new MySqlParameter("@EndDate", MySqlDbType.DateTime),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20)           };
            parameters[0].Value = StartDate;
            parameters[1].Value = EndDate;
            parameters[2].Value = unifiedOrgCode;

            DataSet ds = DbHelperMySQLLog.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ContainsInData)
                        list.Add(DataRowToModelQ2(dr));
                    else
                        list.Add(DataRowToModelQ(dr));
                }
            }

            return list;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MapplyerrorQ DataRowToModelQ(DataRow row)
        {
            MapplyerrorQ model = new MapplyerrorQ();
            if (row != null)
            {
                if (row["applyTime"] != null && row["applyTime"].ToString() != "")
                {
                    model.applyTime = DateTime.Parse(row["applyTime"].ToString());
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
                if (row["billCode"] != null)
                {
                    model.billCode = row["billCode"].ToString();
                }
                if (row["TotalMoney"] != null && row["TotalMoney"].ToString() != "")
                {
                    model.TotalMoney = decimal.Parse(row["TotalMoney"].ToString());
                }
                if (row["idcardNo"] != null)
                {
                    model.idcardNo = row["idcardNo"].ToString();
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
               
                if (row["OutData"] != null)
                {
                    model.OutData = row["OutData"].ToString();
                }
            }
            return model;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MapplyerrorQ2 DataRowToModelQ2(DataRow row)
        {
            MapplyerrorQ2 model = new MapplyerrorQ2();
            if (row != null)
            {
                if (row["applyTime"] != null && row["applyTime"].ToString() != "")
                {
                    model.applyTime = DateTime.Parse(row["applyTime"].ToString());
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
                if (row["billCode"] != null)
                {
                    model.billCode = row["billCode"].ToString();
                }
                if (row["TotalMoney"] != null && row["TotalMoney"].ToString() != "")
                {
                    model.TotalMoney = decimal.Parse(row["TotalMoney"].ToString());
                }
                if (row["idcardNo"] != null)
                {
                    model.idcardNo = row["idcardNo"].ToString();
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
                if (row["CreatTime"] != null && row["CreatTime"].ToString() != "")
                {
                    model.CreatTime = DateTime.Parse(row["CreatTime"].ToString());
                }
                if (row["InData"] != null)
                {
                    model.InData = row["InData"].ToString();
                }
                if (row["OutData"] != null)
                {
                    model.OutData = row["OutData"].ToString();
                }
            }
            return model;
        }


    }
}
