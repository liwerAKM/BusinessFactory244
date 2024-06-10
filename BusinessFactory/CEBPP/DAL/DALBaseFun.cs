using EBPP.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP.DAL
{
    public class DALBaseFun
    {

        public static long GetEBPPID()
        {
           long  id = 0;
            DbHelperMySQL.GetSysIdBase("EBPPID", out id);
            return id;
        }

        public static bool SaveEBPP(  Mebppquery mebppquery,   Mebppmain mebppmain ,List<Mebppinfo> lMebppinfolist, List<Mebppmx2> lMebppmxlist, out string  errMesage )
        {
            errMesage = "";
            StringBuilder strSql = new StringBuilder();
           
            using (MySqlConnection conn = new MySqlConnection(DbHelperMySQL.ConnectionStringMySQL))
            {
                conn.Open();
                using (MySqlTransaction trans = conn.BeginTransaction())
                {
                    MySqlCommand cmd = new MySqlCommand();
                    try
                    {
                        DALebppmain.Add(mebppmain, cmd, conn, trans);
                        foreach (Mebppmx2 info in lMebppmxlist)
                        {
                            DALebppmx2.Add(info, cmd, conn, trans);
                        }
                        
                        foreach (Mebppinfo info in lMebppinfolist)
                        {
                            DALebppinfo.Add(info, cmd, conn, trans);
                        }
                        DALebppquery.Add(mebppquery, cmd, conn, trans);
                        UpdateApplyCount(mebppmain,cmd, conn, trans);
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {

                        trans.Rollback();
                        errMesage = ex.Message;
                        EBPPLog.Error("SaveEBPP:" + mebppquery.idcardNo+ mebppquery.OrgName + mebppquery.billID, "保存失败!" + ex.ToString());
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 更新每人上传统计数据
        /// </summary>
        public static void UpdateApplyCount(Mebppmain mebppmain, MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSqlopen = new StringBuilder();
            strSql.Append("insert into applycount(");
            strSql.Append("ApplyDate,unifiedOrgCode,GhCount,MzCount,ZyCount,QdCount)");


            strSqlopen.Append("insert into applycountopen(");
            strSqlopen.Append("OpenDate,unifiedOrgCode,GhCount,MzCount,ZyCount,QdCount)");

            if (mebppmain.EBPPType == 1)
            {
                strSql.Append("values (@ApplyDate,@unifiedOrgCode,1,0,0,0)");
                strSql.Append("ON DUPLICATE KEY UPDATE  GhCount=GhCount+1;");

                strSqlopen.Append("values (@OpenDate,@unifiedOrgCode,1,0,0,0)");
                strSqlopen.Append("ON DUPLICATE KEY UPDATE  GhCount=GhCount+1;");
            }
            else if (mebppmain.EBPPType == 2)
            {
                strSql.Append("values (@ApplyDate,@unifiedOrgCode,0,1,0,0)");
                strSql.Append("ON DUPLICATE KEY UPDATE  MzCount=MzCount+1;");

                strSqlopen.Append("values (@OpenDate,@unifiedOrgCode,0,1,0,0)");
                strSqlopen.Append("ON DUPLICATE KEY UPDATE  MzCount=MzCount+1;");
            }
            else if (mebppmain.EBPPType == 3)
            {
                strSql.Append("values (@ApplyDate,@unifiedOrgCode,0,0,1,0)");
                strSql.Append("ON DUPLICATE KEY UPDATE  ZyCount=ZyCount+1;");

                strSqlopen.Append("values (@OpenDate,@unifiedOrgCode,0,0,1,0)");
                strSqlopen.Append("ON DUPLICATE KEY UPDATE  ZyCount=ZyCount+1;");
            }
            else
            {
                strSql.Append("values (@ApplyDate,@unifiedOrgCode,0,0,0,1)");
                strSql.Append("ON DUPLICATE KEY UPDATE  QdCount=QdCount+1;");

                strSqlopen.Append("values (@OpenDate,@unifiedOrgCode,0,0,0,1)");
                strSqlopen.Append("ON DUPLICATE KEY UPDATE   QdCount=QdCount+1;");

            }
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ApplyDate", MySqlDbType.Date),
                    new MySqlParameter("@unifiedOrgCode", MySqlDbType.VarChar,20),
            new MySqlParameter("@OpenDate", MySqlDbType.Date),};
            parameters[0].Value = mebppmain.CreatTime;
            parameters[1].Value = mebppmain.unifiedOrgCode ;
            parameters[2].Value = mebppmain.openTime ;
            DbHelperMySQL.PrepareCommand(cmd, conn, trans, strSql.ToString() + strSqlopen.ToString(), parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }

        
        /// <summary>
        ///作废电子票据
        /// </summary>
        /// <param name="SerialNum"></param>
        /// <returns></returns>
        public static bool RepealeEBPP(long  EBPPID, string idcardNo, DateTime openData)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from     ebppmx2  where EBPPID=@EBPPID;");
            strSql.Append(" delete from     ebppinfo  where EBPPID=@EBPPID;");
            strSql.Append(" delete  from     ebppquery  where idcardNo=@idcardNo and openData= @openData and  EBPPID=@EBPPID;");
            strSql.Append(" delete from     ebppmain  where EBPPID=@EBPPID;");
            MySqlParameter[] parameters = {   new MySqlParameter("@idcardNo", MySqlDbType.VarChar, 18),
                    new MySqlParameter("@openData", MySqlDbType.Date),
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64, 20)};
            parameters[0].Value = idcardNo;
            parameters[1].Value = openData;
            parameters[2].Value = EBPPID;

            return DbHelperMySQL.ExecuteSqlTran(strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        ///查询电子票据明细数据
        /// </summary>
        /// <param name="SerialNum"></param>
        /// <returns></returns>
        public static DataSet GetEBPPMx(long EBPPID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from     ebppmain  where EBPPID=@EBPPID;");
            strSql.Append(" select * from     ebppinfo  where EBPPID=@EBPPID;");
            strSql.Append("select *  from     ebppmx2  where EBPPID=@EBPPID;");
            MySqlParameter[] parameters = {   
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64, 20)};

            parameters[0].Value = EBPPID;

            return DbHelperMySQL.Query(strSql.ToString(), parameters) ;
        }

        /// <summary>
        ///查询电子票据明细数据ebppinfo
        /// </summary>
        /// <param name="SerialNum"></param>
        /// <returns></returns>
        public static DataSet GetEBPPInfo(long EBPPID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select * from     ebppmain  where EBPPID=@EBPPID;");
            strSql.Append(" select * from     ebppinfo  where EBPPID=@EBPPID and (DataType =1 or DataType =6);");
           
            MySqlParameter[] parameters = {
                    new MySqlParameter("@EBPPID", MySqlDbType.Int64, 20)};

            parameters[0].Value = EBPPID;

            return DbHelperMySQL.Query(strSql.ToString(), parameters);
        }

    }


}
