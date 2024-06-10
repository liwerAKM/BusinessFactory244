using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasS.Base.Lib.Model;

namespace PasS.Base.Lib.DAL
{
    /// <summary>
    /// 数据访问类:businessinfoversion
    /// </summary>
    public partial class BusinessinfoVersion
    {
        public BusinessinfoVersion()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string BusID, decimal VersionN)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from businessinfoversion");
            strSql.Append(" where BusID=@BusID and VersionN=@VersionN ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20)         };
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string BusID, string BusVersion, decimal VersionN)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from businessinfoversion");
            strSql.Append(" where BusID=@BusID  and  BusVersion =@BusVersion and VersionN=@VersionN ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20) ,
              new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)};
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = BusVersion;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add( businessinfoversion model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into businessinfoversion(");
            strSql.Append("BusID,BusVersion,VersionN,Status,projectID,FilePath,DllName,Version)");
            strSql.Append(" values (");
            strSql.Append("@BusID,@BusVersion,@VersionN,@Status,@projectID,@FilePath,@DllName,@Version)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20),
                    new MySqlParameter("@Status", MySqlDbType.Int16,4),
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@DllName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20),
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)};
            parameters[0].Value = model.BusID;
            parameters[1].Value = model.VersionN;
            parameters[2].Value = model.Status;
            parameters[3].Value = model.projectID;
            parameters[4].Value = model.FilePath;
            parameters[5].Value = model.DllName;
            parameters[6].Value = model.Version;
            parameters[7].Value = model.BusVersion;
            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Update( businessinfoversion model, decimal VersionNOld)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update businessinfoversion set ");
            strSql.Append("VersionN=@VersionN,");
            strSql.Append("Status=@Status,");
            strSql.Append("projectID=@projectID,");
            strSql.Append("FilePath=@FilePath,");
            strSql.Append("DllName=@DllName,");
            strSql.Append("Version=@Version");
            strSql.Append(" where BusID=@BusID and BusVersion=@BusVersion and VersionN=@VersionNOld ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@Status", MySqlDbType.Int16,4),
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@DllName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20),
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20),
             new MySqlParameter("@VersionNOld", MySqlDbType.Decimal,20),
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)};
            parameters[0].Value = model.Status;
            parameters[1].Value = model.projectID;
            parameters[2].Value = model.FilePath;
            parameters[3].Value = model.DllName;
            parameters[4].Value = model.Version;
            parameters[5].Value = model.BusID;
            parameters[6].Value = model.VersionN;
            parameters[7].Value = VersionNOld;
            parameters[8].Value = model.BusVersion;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Update(  businessinfoversion model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update businessinfoversion set ");
            strSql.Append("Status=@Status,");
            strSql.Append("projectID=@projectID,");
            strSql.Append("FilePath=@FilePath,");
            strSql.Append("DllName=@DllName,");
            strSql.Append("Version=@Version");
            strSql.Append(" where BusID=@BusID and BusVersion=@BusVersion and VersionN=@VersionN ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@Status", MySqlDbType.Int16,4),
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@DllName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20),
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20),
                new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)};
            parameters[1].Value = model.projectID;
            parameters[2].Value = model.FilePath;
            parameters[3].Value = model.DllName;
            parameters[4].Value = model.Version;
            parameters[5].Value = model.BusID;
            parameters[6].Value = model.VersionN;
            parameters[7].Value = model.BusVersion;
            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Delete(string BusID, string BusVersion, decimal VersionN)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from businessinfoversion");
            strSql.Append(" where BusID=@BusID  and  BusVersion =@BusVersion and VersionN=@VersionN ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20) ,
              new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)};
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = BusVersion;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(string BusID, decimal VersionN)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from businessinfoversion ");
            strSql.Append(" where BusID=@BusID and VersionN=@VersionN ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20)         };
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from businessinfoversion ");
            strSql.Append(" where BusID=@BusID  ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20)  };
            parameters[0].Value = BusID;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public  businessinfoversion GetModel(string BusID, decimal VersionN)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BusID,VersionN,Status,projectID,FilePath,DllName,Version from businessinfoversion ");
            strSql.Append(" where BusID=@BusID and VersionN=@VersionN ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionN", MySqlDbType.Decimal,20)         };
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;

           businessinfoversion model = new businessinfoversion();
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
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
        public  businessinfoversion DataRowToModel(DataRow row)
        {
            businessinfoversion model = new  businessinfoversion();
            if (row != null)
            {
                if (row["BusID"] != null)
                {
                    model.BusID = row["BusID"].ToString();
                }
                if (row["BusVersion"] != null)
                {
                    model.BusVersion = row["BusVersion"].ToString();
                }
                if (row["VersionN"] != null && row["VersionN"].ToString() != "")
                {
                    model.VersionN = decimal.Parse(row["VersionN"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["projectID"] != null)
                {
                    model.projectID = row["projectID"].ToString();
                }
                if (row["FilePath"] != null)
                {
                    model.FilePath = row["FilePath"].ToString();
                }
                if (row["DllName"] != null)
                {
                    model.DllName = row["DllName"].ToString();
                }
                if (row["Version"] != null)
                {
                    model.Version = row["Version"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',DllName,',Version:',Version) as'FileID', BusID,BusVersion,VersionN,Status,projectID,FilePath,DllName,Version ");
            strSql.Append(" FROM businessinfoversion ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }

        public ConcurrentDictionary<int,  businessinfoversion> GetTestLevelList(string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',DllName,',Version:',Version) as'FileID', BusID,BusVersion,VersionN,Status,projectID,FilePath,DllName,Version ");
            strSql.Append(" FROM businessinfoversion ");
            strSql.Append(" where BusID=@BusID  and  `Status` in(5,4,3)  ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20)  };
            parameters[0].Value = BusID;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ConcurrentDictionary<int,  businessinfoversion> concurrentDictionary = new ConcurrentDictionary<int, Model.businessinfoversion>();
                foreach (DataRow dr in dt.Rows)
                {
                   businessinfoversion businessinfoversion = DataRowToModel(dr);
                    concurrentDictionary.TryAdd(businessinfoversion.Status, businessinfoversion);
                }
                return concurrentDictionary;
            }
            else
            {
                return null;
            }
        }
        public ConcurrentDictionary<int,  businessinfoversion> GetTestLevelList(string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',DllName,',Version:',Version) as'FileID', BusID,BusVersion,VersionN,Status,projectID,FilePath,DllName,Version ");
            strSql.Append(" FROM businessinfoversion ");
            strSql.Append(" where BusID=@BusID   and BusVersion =@BusVersion and  `Status` in(5,4,3)  ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20) , new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)  };
            parameters[0].Value = BusID;
            parameters[1].Value = BusVersion;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ConcurrentDictionary<int,  businessinfoversion> concurrentDictionary = new ConcurrentDictionary<int, Model.businessinfoversion>();
                foreach (DataRow dr in dt.Rows)
                {
                     businessinfoversion businessinfoversion = DataRowToModel(dr);
                    concurrentDictionary.TryAdd(businessinfoversion.Status, businessinfoversion);
                }
                return concurrentDictionary;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM businessinfoversion ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQLMySpring.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        #endregion  BasicMethod

    }
}
