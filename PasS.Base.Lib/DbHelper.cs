using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PasS.Base.Lib
{
    public class DbHelper
    {
        /// <summary>
        /// 获取指定基础表ID 
        /// </summary>
        /// <param name="SYSID_NAME">表标识</param>
        /// <param name="Sys_Id">最新ID</param>
        /// <returns></returns>
        public static bool GetSysIdBase(string SYSID_NAME, out int Sys_Id)
        {

            Sys_Id = -1;
            using (MySqlConnection conn = new MySqlConnection(DbHelperMySQLMySpring.connectionString))
            {
                string sp_Name = "GETSYSIDBASE";

                if (conn.State != ConnectionState.Open) conn.Open();
                MySqlCommand sqlcmd = new MySqlCommand(sp_Name, conn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("SYSIDNAME", SYSID_NAME);
                MySqlParameter sql_param = sqlcmd.Parameters.Add("SYSID", MySqlDbType.Int32);
                sql_param.Direction = ParameterDirection.Output;
                sqlcmd.ExecuteNonQuery();
                if (sql_param.Value == DBNull.Value)
                {
                    string strSQL = " INSERT INTO `sysidbase` VALUES('" + SYSID_NAME + "',  1);";
                    if (DbHelperMySQLMySpring.ExecuteSql(strSQL) > 0)
                    {
                        Sys_Id = 1;
                        return true;
                    }
                    return false;
                }
                else if ((int)sql_param.Value <= 0)
                {
                    return false;
                }
                else
                {
                    Sys_Id = (int)sql_param.Value;
                    return true;
                }
            }
        }
        public static DataTable AllCodeInt(string All_lbmc)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Code_ID,Code_Name from AllCodeInt where All_lbmc=@All_lbmc ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@All_lbmc", MySqlDbType.VarChar,20)   };
            parameters[0].Value = All_lbmc;
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        #region  SysInfo
        public static DataTable SysInfoGet()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from sysinfo ");
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static string SysInfoGet(string Key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value from sysinfo where ID=@ID");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50)          };
            parameters[0].Value = Key;
            object ovalue = DbHelperMySQLMySpring.GetSingle(strSql.ToString(), parameters);
            if (ovalue != null)
            {
                return (string)ovalue;
            }
            return "";
        }

        public static T SysInfoGet<T>(string Key)
        {
            string value = SysInfoGet(Key);
            if (value != "" && value.Trim() != "")
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }
        public static bool SysInfoSet(string Key, string Value)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update  sysinfo set Value =@Value where ID=@ID");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
             new MySqlParameter("@Value", MySqlDbType.VarChar,500)  };
            parameters[0].Value = Key;
            parameters[1].Value = Value;
            return DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters) == 1;
        }
        public static bool SysInfoSet(string Key, string Value, string KeyNote, string ValueNote)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into sysinfo(");
            strSql.Append("ID,Value, IDNote,ValueNote )");
            strSql.Append(" values (");
            strSql.Append("  @ID,@Value,@IDNote,@ValueNote )");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 Value=@Value,IDNote=@IDNote,ValueNote=@ValueNote ");


            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Value", MySqlDbType.VarChar,500),
                    new MySqlParameter("@IDNote", MySqlDbType.VarChar,200),
                    new MySqlParameter("@ValueNote", MySqlDbType.VarChar,200) };
            parameters[0].Value = Key;
            parameters[1].Value = Value;
            parameters[2].Value = KeyNote;
            parameters[3].Value = ValueNote;

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
        public static bool SysInfoSet<T>(string Key, T Value)
        {
            return SysInfoSet(Key, JsonConvert.SerializeObject(Value));
        }
        #endregion  SysInfo

        #region  SLBInfo

        public static SLBInfo SLBInfoGet(string SLBID)
        {
            List<SLBInfo> list = SLBInfoListGet("ID = '" + SLBID.ReplaceSQLWhere() + "'");
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return null;
        }
        public static List<SLBInfo> SLBInfoListGet(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from SLBInfo ");
            if (!string.IsNullOrEmpty(where))
                strSql.Append("where " + where);

            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                List<SLBInfo> list = new List<SLBInfo>();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SLBInfo sLBInfo = new SLBInfo();
                        sLBInfo.SLBID = dr["ID"].ToString();
                        sLBInfo.IP = dr["IP"].ToString();
                        sLBInfo.Port = (int)dr["Port"];
                        sLBInfo.IP2 = dr["IP2"].ToString();
                        sLBInfo.Port2 = (int)dr["Port2"];
                        sLBInfo.SLBName = dr["SLBName"].ToString();
                        sLBInfo.Status = short.Parse(dr["Status"].ToString());
                        if (dt.Columns.Contains("IP3"))
                            sLBInfo.IP3 = dr["IP3"].ToString();
                        if (dt.Columns.Contains("Port3"))
                            sLBInfo.Port3 = (int)dr["Port3"];
                        if (dt.Columns.Contains("WSport"))
                            sLBInfo.WebSocketPort = (int)dr["WSport"];
                        if (dt.Columns.Contains("WSPath"))
                            sLBInfo.WSPath =  dr["WSPath"].ToString();
                        if (dt.Columns.Contains("OrgID"))
                            sLBInfo.OrgID = dr["OrgID"].ToString();
                        if (dt.Columns.Contains("GroupID"))
                            sLBInfo.GroupID =(int) dr["GroupID"];
                        list.Add(sLBInfo);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取BusServer对应的SLB信息列表（如果有网闸映射，则替换成对应地址）
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public static List<SLBInfo> SLBInfoListGetForServer(string BusServerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from SLBInfo  where Status =1;");
            strSql.Append("select *  from   GAPSLBIPForBusServer  where BusServerID =@ID ;");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50)};
            parameters[0].Value = BusServerID;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            DataTable dt = ds.Tables[0];
            DataTable dtGAP = ds.Tables[1];
            if (dt != null && dt.Rows.Count > 0)
            {
                List<SLBInfo> list = new List<SLBInfo>();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SLBInfo sLBInfo = new SLBInfo();
                        sLBInfo.SLBID = dr["ID"].ToString();
                        sLBInfo.IP = dr["IP"].ToString();
                        sLBInfo.Port = (int)dr["Port"];
                        sLBInfo.IP2 = dr["IP2"].ToString();
                        sLBInfo.Port2 = (int)dr["Port2"];
                        sLBInfo.SLBName = dr["SLBName"].ToString();
                        sLBInfo.Status = short.Parse(dr["Status"].ToString());
                        if (dt.Columns.Contains("IP3"))
                            sLBInfo.IP3 = dr["IP3"].ToString();
                        if (dt.Columns.Contains("Port3"))
                            sLBInfo.Port3 = (int)dr["Port3"];
                        DataRow[] drs = dtGAP.Select("SLBID ='" + sLBInfo.SLBID + "' ");
                        if (drs.Length > 0)
                        {

                            sLBInfo.IP = drs[0]["IP"].ToString();
                            sLBInfo.Port = (int)drs[0]["Port"];
                            if (dtGAP.Columns.Contains("IP2"))
                                sLBInfo.IP2 = drs[0]["IP2"].ToString();
                            if (dtGAP.Columns.Contains("Port2"))
                                sLBInfo.Port2 = (int)drs[0]["Port2"];
                        }
                        list.Add(sLBInfo);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取BusServer对应的指定SLB信息（如果有网闸映射，则替换成对应地址）
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <param name="SLBID"></param>
        /// <returns></returns>
        public static SLBInfo SLBInfoGetForServer(string BusServerID, string SLBID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from SLBInfo  where ID = @ID;");
            strSql.Append("select *  from   GAPSLBIPForBusServer  where BusServerID = @BusServerID ;");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50)
            };
            parameters[0].Value = SLBID;
            parameters[1].Value = BusServerID;
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            DataTable dt = ds.Tables[0];
            DataTable dtGAP = ds.Tables[1];
            if (dt != null && dt.Rows.Count > 0)
            {
                SLBInfo sLBInfo = new SLBInfo();
                try
                {
                    DataRow dr = dt.Rows[0];
                    {
                        sLBInfo.SLBID = dr["ID"].ToString();
                        sLBInfo.IP = dr["IP"].ToString();
                        sLBInfo.Port = (int)dr["Port"];
                        sLBInfo.IP2 = dr["IP2"].ToString();
                        sLBInfo.Port2 = (int)dr["Port2"];
                        sLBInfo.SLBName = dr["SLBName"].ToString();
                        sLBInfo.Status = short.Parse(dr["Status"].ToString());
                        if (dt.Columns.Contains("IP3"))
                            sLBInfo.IP3 = dr["IP3"].ToString();
                        if (dt.Columns.Contains("Port3"))
                            sLBInfo.Port3 = (int)dr["Port3"];
                        DataRow[] drs = dtGAP.Select("SLBID ='" + sLBInfo.SLBID + "' ");
                        if (drs.Length > 0)
                        {

                            sLBInfo.IP = drs[0]["IP"].ToString();
                            sLBInfo.Port = (int)drs[0]["Port"];
                            if (dtGAP.Columns.Contains("IP2"))
                                sLBInfo.IP2 = drs[0]["IP2"].ToString();
                            if (dtGAP.Columns.Contains("Port2"))
                                sLBInfo.Port2 = (int)drs[0]["Port2"];
                        }
                    }
                    return sLBInfo;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static DataTable SLBInfoTableGet(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from SLBInfo ");
            if (!string.IsNullOrEmpty(where))
                strSql.Append("where " + where);

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }

        public static DataTable SLBInfoCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select count(1)  as'All' , COUNT(case Status when 1 then 1 end) as 'run'   from SLBInfo where Status>=0");

            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];

        }



        public static bool SLBInfoSetSet(SLBInfo sLBInfo)
        {
            //hlw add 2021.07.15 WSPort WSPath 要有默认值

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into SLBInfo(");
            strSql.Append("ID,Status, IP,Port,IP2, Port2,IP3, Port3,SLBName,Value,AddTime,LastEditTime,WSPort,WSPath)");
            strSql.Append(" values (");
            strSql.Append("  @ID,@Status,@IP,@Port,@IP2,@Port2,@IP3,@Port3,@SLBName,@Value,@AddTime ,@LastEditTime,@WSPort,@WSPath)");
            strSql.Append(" ON DUPLICATE KEY UPDATE  ");
            strSql.Append("  Status=@Status,IP=@IP,Port=@Port,IP2=@IP2,Port2=@Port2,IP3=@IP3,Port3=@Port3,SLBName=@SLBName,Value=@Value,LastEditTime =@LastEditTime,WSPort=@WSPort,WSPath=@WSPath");


            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Status", MySqlDbType.Int16 ),
                    new MySqlParameter("@IP", MySqlDbType.VarChar,15),
                    new MySqlParameter("@Port", MySqlDbType.UInt32),
                     new MySqlParameter("@IP2", MySqlDbType.VarChar,15),
                    new MySqlParameter("@Port2", MySqlDbType.UInt32),
                      new MySqlParameter("@IP3", MySqlDbType.VarChar,15),
                    new MySqlParameter("@Port3", MySqlDbType.UInt32),
                    new MySqlParameter("@SLBName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Value", MySqlDbType.VarChar,1000),
                    new MySqlParameter("@AddTime",  MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime),
                    new MySqlParameter("@WSPort", MySqlDbType.VarChar),
                    new MySqlParameter("@WSPath", MySqlDbType.VarChar)
            };
            parameters[0].Value = sLBInfo.SLBID;
            parameters[1].Value = sLBInfo.Status;
            parameters[2].Value = sLBInfo.IP;
            parameters[3].Value = sLBInfo.Port;
            parameters[4].Value = sLBInfo.IP2;
            parameters[5].Value = sLBInfo.Port2;
            parameters[6].Value = sLBInfo.IP3;
            parameters[7].Value = sLBInfo.Port3;
            parameters[8].Value = sLBInfo.SLBName;
            parameters[9].Value = "";
            parameters[10].Value = DateTime.Now;
            parameters[11].Value = DateTime.Now;
            parameters[12].Value = "8910";
            parameters[13].Value = "SLB1";
            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }



            //StringBuilder strSql = new StringBuilder();
            //strSql.Append("insert in to SLBInfo(");
            //strSql.Append("ID,Status, IP,Port,IP2, Port2,IP3, Port3,SLBName,Value,AddTime,LastEditTime)");
            //strSql.Append(" values (");
            //strSql.Append("  @ID,@Status,@IP,@Port,@IP2,@Port2,@IP3,@Port3,@SLBName,@Value,@AddTime ,@LastEditTime )");
            //strSql.Append(" ON DUPLICATE KEY UPDATE  ");
            //strSql.Append("  Status=@Status,IP=@IP,Port=@Port,IP2=@IP2,Port2=@Port2,IP3=@IP3,Port3=@Port3,SLBName=@SLBName,Value=@Value,LastEditTime =@LastEditTime");


            //MySqlParameter[] parameters = {
            //        new MySqlParameter("@ID", MySqlDbType.VarChar,50),
            //        new MySqlParameter("@Status", MySqlDbType.Int16 ),
            //        new MySqlParameter("@IP", MySqlDbType.VarChar,15),
            //        new MySqlParameter("@Port", MySqlDbType.UInt32),
            //         new MySqlParameter("@IP2", MySqlDbType.VarChar,15),
            //        new MySqlParameter("@Port2", MySqlDbType.UInt32),
            //          new MySqlParameter("@IP3", MySqlDbType.VarChar,15),
            //        new MySqlParameter("@Port3", MySqlDbType.UInt32),
            //        new MySqlParameter("@SLBName", MySqlDbType.VarChar,50),
            //        new MySqlParameter("@Value", MySqlDbType.VarChar,1000),
            //        new MySqlParameter("@AddTime",  MySqlDbType.DateTime),
            //        new MySqlParameter("@LastEditTime", MySqlDbType.DateTime)};
            //parameters[0].Value = sLBInfo.SLBID;
            //parameters[1].Value = sLBInfo.Status;
            //parameters[2].Value = sLBInfo.IP;
            //parameters[3].Value = sLBInfo.Port;
            //parameters[4].Value = sLBInfo.IP2;
            //parameters[5].Value = sLBInfo.Port2;
            //parameters[6].Value = sLBInfo.IP3;
            //parameters[7].Value = sLBInfo.Port3;
            //parameters[8].Value = sLBInfo.SLBName;
            //parameters[9].Value = "";
            //parameters[10].Value = DateTime.Now;
            //parameters[11].Value = DateTime.Now;
            //int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
            //if (rows > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
        public static bool SLBInfoSetStatus(string SLBID, short Status)
        {
            StringBuilder strSql = new StringBuilder();
            Console.WriteLine(string.Format("{0} 更新信息SLBID： {1}  Status = {2}", DateTime.Now.ToString(), SLBID, Status));
            strSql.Append("update SLBInfo set Status =@Status ,LastEditTime =@LastEditTime where ID=@ID");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Status", MySqlDbType.Int16 ),
             new MySqlParameter("@LastEditTime", MySqlDbType.DateTime)};
            parameters[0].Value = SLBID;
            parameters[1].Value = Status;
            parameters[2].Value = DateTime.Now;
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
        #endregion  SLBInfo


        #region  BusServer

        public static BusServerInfo BusServerGet(string BusServerID)
        {
            BusServerInfo busServerInfo = new BusServerInfo();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value from BusServer where ID= @ID");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50)   };
            parameters[0].Value = BusServerID;
            object ovalue = DbHelperMySQLMySpring.GetSingle(strSql.ToString(), parameters);

            if (ovalue != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<BusServerInfo>((string)ovalue);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static DataTable BusServerCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select count(1)  as'All' , COUNT(case Status when 1 then 1 end) as 'run'   from BusServer where Status>=0");
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static DataTable BusServerTableGet(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from BusServer ");
            if (!string.IsNullOrEmpty(where))
                strSql.Append("where " + where);
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }

        }

        public static List<BusServerInfo> BusServerListGet(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value from BusServer ");
            if (!string.IsNullOrEmpty(where))
                strSql.Append("where " + where);
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                List<BusServerInfo> list = new List<BusServerInfo>();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    { list.Add(JsonConvert.DeserializeObject<BusServerInfo>(dr["Value"].ToString())); }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static bool BusServerSet(BusServerInfo busServerInfo)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BusServer(");
            strSql.Append("ID ,Status, Weight, BusServerName,Value,AddTime,LastEditTime,LastDownFileTime)");
            strSql.Append(" values (");
            strSql.Append("  @ID,@Status,@Weight,@BusServerName,@Value,@AddTime ,@LastEditTime,'2018-07-01 00:00:00' )");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 Weight=@Weight,BusServerName=@BusServerName,Value=@Value,LastEditTime =@LastEditTime");


            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Status", MySqlDbType.Int16 ),
                    new MySqlParameter("@Weight", MySqlDbType.Int32),
                    new MySqlParameter("@BusServerName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Value", MySqlDbType.VarChar,1000),
                    new MySqlParameter("@AddTime",  MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime)};
            parameters[0].Value = busServerInfo.BusServerID;
            parameters[1].Value = 0;
            parameters[2].Value = busServerInfo.Weight;
            parameters[3].Value = busServerInfo.BusServerName;
            parameters[4].Value = JsonConvert.SerializeObject(busServerInfo);
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = DateTime.Now;
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
        public static bool BusServerUpDownTime(string BusServerID)
        {
            string strSQL = string.Format("update BusServer set LastDownFileTime =@LastDownFileTime  where ID=@ID");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@LastDownFileTime", MySqlDbType.DateTime)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = DateTime.Now;
            int rows = DbHelperMySQLMySpring.ExecuteSql(strSQL, parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool BusServerSet(BusServerInfo busServerInfo, int Status)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BusServer(");
            strSql.Append("ID ,Status, Weight, BusServerName,Value,AddTime,LastEditTime)");
            strSql.Append(" values (");
            strSql.Append("  @ID,@Status,@Weight,@BusServerName,@Value,@AddTime ,@LastEditTime )");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 Weight=@Weight,Status =@Status,BusServerName=@BusServerName,Value=@Value,LastEditTime =@LastEditTime");


            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Status", MySqlDbType.Int16 ),
                    new MySqlParameter("@Weight", MySqlDbType.Int32),
                    new MySqlParameter("@BusServerName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Value", MySqlDbType.VarChar,1000),
                    new MySqlParameter("@AddTime",  MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime)};
            parameters[0].Value = busServerInfo.BusServerID;
            parameters[1].Value = Status;
            parameters[2].Value = busServerInfo.Weight;
            parameters[3].Value = busServerInfo.BusServerName;
            parameters[4].Value = JsonConvert.SerializeObject(busServerInfo);
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = DateTime.Now;
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
        public static bool BusServerSetStatus(string BusServerID, int Status)
        {
            string strSQL = string.Format(" update BusServer set Status =@Status where ID = @ID ");
            MySqlParameter[] parameters = {
                new MySqlParameter("@ID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Status", MySqlDbType.Int16 )};
            parameters[0].Value = BusServerID;
            parameters[1].Value = Status;

            Console.WriteLine(string.Format("{0} 更新信息BusServerID： {2}  Status = {1}",
                DateTime.Now, Status, BusServerID));
            try
            {
                int rows = DbHelperMySQLMySpring.ExecuteSql(strSQL, parameters);
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("{0} SQLERROE:{1} ", DateTime.Now, ex.Message);
                PasSLog.Error("DbHelper.BusServerSetStatus", message);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                return false;
            }

        }
        public static bool BusServerDel(string BusServerID)
        {
            int rows = DbHelperMySQLMySpring.ExecuteSql(string.Format(" delete from  BusServer  where ID='[0}'", BusServerID));
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion  BusServer

        #region  BusServerBusInfo
        public static List<SingleBusServerInfo> BusServerBusInfoSingleListByServerGet(string BusServerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select BusinessInfo.Value , busserverbusinfo.Maxconcurrent from BusinessInfo 
join busserverbusinfo on BusinessInfo.BusID = busserverbusinfo.BusID
where busserverbusinfo.BusServerID = @ID and busserverbusinfo.Status =1"));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50)};
            parameters[0].Value = BusServerID;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                List<SingleBusServerInfo> list = new List<SingleBusServerInfo>();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BusinessInfo businessInfo = JsonConvert.DeserializeObject<BusinessInfo>(dr["Value"].ToString());
                        SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
                        if (businessInfo.busDDLBS == BusDDLBS.LimitServerMaxConcurrentRandomRule)
                        {
                            singleBusServerInfo.Maxconcurrent = (int)dr["Maxconcurrent"];
                        }
                        singleBusServerInfo.BusServerID = BusServerID;
                        singleBusServerInfo.BusID = businessInfo.BusID;
                        singleBusServerInfo.TheConcurrent = 0;
                        list.Add(singleBusServerInfo);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static SBusServerBusInfo SBusServerBusInfoGet(string BusServerID, string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select * from   busserverbusinfo  
where  BusServerID = @BusServerID  and  BusID = @BusID  ", BusServerID, BusID));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50),
              new MySqlParameter("@BusID", MySqlDbType.VarChar,20)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    DataRow dr = dt.Rows[0];

                    SBusServerBusInfo sBusServerBusInfo = new SBusServerBusInfo();
                    sBusServerBusInfo.BusID = BusID;
                    sBusServerBusInfo.BusServerID = BusServerID;
                    sBusServerBusInfo.Status = int.Parse(dr["Status"].ToString());
                    sBusServerBusInfo.Maxconcurrent = int.Parse(dr["Maxconcurrent"].ToString());

                    return sBusServerBusInfo;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        public static SingleBusServerInfo BusServerBusInfoSingleByServerGet(string BusServerID, string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select BusinessInfo.Value , busserverbusinfo.Maxconcurrent from BusinessInfo 
join busserverbusinfo on BusinessInfo.BusID = busserverbusinfo.BusID
where busserverbusinfo.BusServerID =@BusServerID  and busserverbusinfo.BusID = @BusID and busserverbusinfo.Status =1"));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50),
              new MySqlParameter("@BusID", MySqlDbType.VarChar,20)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    DataRow dr = dt.Rows[0];
                    BusinessInfo businessInfo = JsonConvert.DeserializeObject<BusinessInfo>(dr["Value"].ToString());
                    SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
                    if (businessInfo.busDDLBS == BusDDLBS.LimitServerMaxConcurrentRandomRule)
                    {
                        singleBusServerInfo.Maxconcurrent = (int)dr["Maxconcurrent"];
                    }
                    singleBusServerInfo.BusServerID = BusServerID;
                    singleBusServerInfo.BusID = businessInfo.BusID;
                    singleBusServerInfo.TheConcurrent = 0;
                    return singleBusServerInfo;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //20210803
        public static SingleBusinessInfoVer BusServerBusInfoSingleByServerGet(string BusServerID, string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select busserverbusinfoVer.*,businessInfo.Value from busserverbusinfoVer 
join BusinessInfoBusVer on BusinessInfoBusVer.BusID = busserverbusinfoVer.BusID and BusinessInfoBusVer.BusVersion = busserverbusinfoVer.BusVersion
join BusinessInfo on  busserverbusinfoVer.BusID=BusinessInfo.BusID
where busserverbusinfoVer.BusServerID =@BusServerID  and busserverbusinfoVer.BusID = @BusID and  busserverbusinfoVer.BusVersion =@BusVersion and BusinessInfoBusVer.Status =1"));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50),
              new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                new MySqlParameter("@BusVersion", MySqlDbType.VarChar,50)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;
            parameters[2].Value = BusVersion;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    DataRow dr = dt.Rows[0];
                    BusinessInfo businessInfo = JsonConvert.DeserializeObject<BusinessInfo>(dr["Value"].ToString());
                    SingleBusinessInfoVer singleBusServerInfo = new SingleBusinessInfoVer();
                    if (businessInfo.busDDLBS == BusDDLBS.LimitServerMaxConcurrentRandomRule)
                    {
                        singleBusServerInfo.Maxconcurrent = (int)dr["Maxconcurrent"];
                    }
                    singleBusServerInfo.VersionN = (decimal)dr["VersionN"];
                    singleBusServerInfo.BusServerID = BusServerID;
                    singleBusServerInfo.BusID = businessInfo.BusID;
                    singleBusServerInfo.BusVersion = BusVersion;
                    singleBusServerInfo.TheConcurrent = 0;
                    return singleBusServerInfo;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static DataTable BusServerBusInfoGet(string BusServerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.BusName ,a.busDDLBS,b.*
 from BusinessInfo as a
join busserverbusinfo  as b on a.BusID = b.BusID
where b.BusServerID = @BusServerID ", BusServerID));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50)

            };
            parameters[0].Value = BusServerID;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static DataTable BusServerBusInfoGetByBusID(string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.BusServerName ,b.*
 from busserver as a
join busserverbusinfo  as b on a.ID = b.BusServerID
where b.BusID = '{0}'", BusID));

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public static bool BusServerBusInfoSet(string BusServerID, string BusID, int Status, int Maxconcurrent)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into busserverbusinfo(");
            strSql.Append("BusServerID,BusID,Status,Maxconcurrent,AddTime,LastEditTime)");
            strSql.Append(" values (");
            strSql.Append("  @BusServerID,@BusID,@Status,@Maxconcurrent,@AddTime,@LastEditTime)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 Status =@Status,Maxconcurrent=@Maxconcurrent,LastEditTime=@LastEditTime");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@Status", MySqlDbType.Int32) ,
             new MySqlParameter("@Maxconcurrent", MySqlDbType.Int32),
               new MySqlParameter("@AddTime", MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;
            parameters[2].Value = Status;
            parameters[3].Value = Maxconcurrent;
            parameters[4].Value = DateTime.Now;
            parameters[5].Value = DateTime.Now;
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


        public static DataTable BusServerBusInfoByBusID(string BusID, bool isOffline)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.BusName ,a.busDDLBS,b.*
 from BusinessInfo as a
join busserverbusinfo  as b on a.BusID = b.BusID
where b.BusID = '{0}' ", BusID));
            if (isOffline)
            {
                strSql.Append(" and b.status = 0");
            }
            else
            {
                strSql.Append(" and b.status > 0");
            }

            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static bool BusServerBusInfoStatus(string BusID, bool Online)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("update busserverbusinfoVer ");
            strSql.Append("	set  Status = @Status,LastEditTime=@LastEditTime where BusID =@BusID and  Status = @StatusOld");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                       new MySqlParameter("@Status",MySqlDbType.Int32),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime),
                new MySqlParameter("@StatusOld",MySqlDbType.Int32),};
            parameters[0].Value = BusID;
            parameters[1].Value = Online ? 1 : -2;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = Online ? -2 : 1;
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
        public static bool BusServerBusInfoDel(string BusServerID, string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from busserverbusinfo ");
            strSql.Append("where BusServerID=@BusServerID and BusID=@BusID ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;

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

        #endregion  BusServerBusInfo

        #region  BusServerBusInfoVer



        public static List<SingleBusinessInfoVer> BusServerBusInfoSingleVerListByServerGet(string BusServerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.Value , c.busDDLBS, b.Maxconcurrent,b.VersionN
 from BusinessInfo c 
join businessinfobusver a on c .BusID = a.BusID
join  busserverbusinfover b on a.BusID = b.BusID and a.BusVersion=b.BusVersion
where b.BusServerID = @ID and b.Status =1"));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@ID", MySqlDbType.VarChar,50)};
            parameters[0].Value = BusServerID;
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                List<SingleBusinessInfoVer> list = new List<SingleBusinessInfoVer>();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BusinessInfoBusVersion businessInfo = JsonConvert.DeserializeObject<BusinessInfoBusVersion>(dr["Value"].ToString());
                        SingleBusinessInfoVer singleBusServerInfo = new SingleBusinessInfoVer();
                        if (((BusDDLBS)dr["busDDLBS"]) == BusDDLBS.LimitServerMaxConcurrentRandomRule)
                        {
                            singleBusServerInfo.Maxconcurrent = (int)dr["Maxconcurrent"];
                        }
                        singleBusServerInfo.BusServerID = BusServerID;
                        singleBusServerInfo.BusID = businessInfo.BusID;
                        singleBusServerInfo.BusVersion = businessInfo.BusVersion;
                        singleBusServerInfo.BusVersion = businessInfo.BusVersion;
                        singleBusServerInfo.VersionN= (decimal)dr["VersionN"];
                        singleBusServerInfo.TheConcurrent = 0;
                        list.Add(singleBusServerInfo);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static bool BusServerBusInfoVerSet(string BusServerID, string BusID, string BusVersion, int Status, int Maxconcurrent, decimal VersionN)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into busserverbusinfoVer(");
            strSql.Append("BusServerID,BusID,BusVersion,VersionN,Status,Maxconcurrent,AddTime,LastEditTime)");
            strSql.Append(" values (");
            strSql.Append("  @BusServerID,@BusID,@BusVersion,@VersionN,@Status,@Maxconcurrent,@AddTime,@LastEditTime)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 Status =@Status,Maxconcurrent=@Maxconcurrent,LastEditTime=@LastEditTime ,VersionN =@VersionN");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@Status", MySqlDbType.Int32) ,
             new MySqlParameter("@Maxconcurrent", MySqlDbType.Int32),
               new MySqlParameter("@AddTime", MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime),
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20),
               new MySqlParameter("@VersionN", MySqlDbType.Decimal,20)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;
            parameters[2].Value = Status;
            parameters[3].Value = Maxconcurrent;
            parameters[4].Value = DateTime.Now;
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = BusVersion;
            parameters[7].Value = VersionN;
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
        public static DataTable BusServerBusInfoVerByBusIDBusVer(string BusID, string BusVersion, bool isOffline)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.BusName ,a.busDDLBS,b.*
 from BusinessInfo as a
join busserverbusinfoVer  as b on a.BusID = b.BusID
where b.BusID = '{0}'  and b.BusVersion ='{1}'", BusID, BusVersion));
            if (isOffline)
            {
                strSql.Append(" and b.status <= 0");
            }
            else
            {
                strSql.Append(" and b.status > 0");
            }

            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusID"></param>
        /// <param name="BusVersion"></param>
        ///  <param name="Status">1 Online; 0 offline -1 业务版本停止触发停止；-2 业务停止触发停止 </param>
        /// <returns></returns>
        public static DataTable BusServerBusInfoVerByBusIDBusVer(string BusID, string BusVersion, int Status)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@" select a.Value , c.busDDLBS, b.Maxconcurrent,b.BusServerID
    from BusinessInfo c
join businessinfobusver a on c.BusID = a.BusID
join busserverbusinfover b on a.BusID = b.BusID and a.BusVersion = b.BusVersion
where b.BusID = '{0}'  and b.BusVersion ='{1}'    and b.status ={2}", BusID, BusVersion, Status));


            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusID"></param>
        ///  <param name="Status">1 Online; 0 offline -1 业务版本停止触发停止；-2 业务停止触发停止 </param>
        /// <returns></returns>
        public static DataTable BusServerIDByBusID(string BusID, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"            select  distinct BusServerid
 from  busserverbusinfover  
where  BusID = '{0}'   and status ={1} ", BusID, Status));


            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static DataTable BusServerInfoByBusID(string BusID, bool isOffline)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"            select a.Value , c.busDDLBS, b.Maxconcurrent,b.BusServerID
    from BusinessInfo c
join businessinfobusver a on c.BusID = a.BusID
join busserverbusinfover b on a.BusID = b.BusID and a.BusVersion = b.BusVersion
where b.BusID = '{0}'   ", BusID));
            if (isOffline)
            {
                strSql.Append(" and b.status <= 0");
            }
            else
            {
                strSql.Append(" and b.status > 0");
            }

            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusID"></param>
        ///  <param name="Status">1 Online; 0 offline -1 业务版本停止触发停止；-2 业务停止触发停止 </param>
        /// <returns></returns>
        public static DataTable BusServerInfoByBusID(string BusID, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.Value , c.busDDLBS, b.Maxconcurrent,b.BusServerID
    from BusinessInfo c
join businessinfobusver a on c.BusID = a.BusID
join busserverbusinfover b on a.BusID = b.BusID and a.BusVersion = b.BusVersion
where b.BusID = '{0}'   and b.status ={1}", BusID, Status));


            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BusID"></param>
        /// <param name="BusVersion"></param>
        /// <param name="Status">1 Online; 0 offline -1 业务版本停止触发停止；-2 业务停止触发停止 </param>
        /// <returns></returns>
        public static bool BusServerBusInfoVerStatus(string BusID, string BusVersion, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update busserverbusinfoVer ");
            strSql.Append("	set  Status = @Status,LastEditTime=@LastEditTime where BusID =@BusID and BusVersion=@BusVersion");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                       new MySqlParameter("@Status",MySqlDbType.Int32),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime),
                new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)};
            parameters[0].Value = BusID;
            parameters[1].Value = Status;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = BusVersion;
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

        public static bool BusServerBusInfoVerDel(string BusServerID, string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from busserverbusinfoVer ");
            strSql.Append("where BusServerID=@BusServerID and BusID=@BusID  and BusVersion=@BusVersion");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,20)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;
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

        public static DataTable BusServerBusInfoGetByBusIDBusVersion(string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.BusServerName ,b.*
 from busserver as a
join busserverbusinfover  as b on a.ID = b.BusServerID
       
where b.BusID = '{0}' and b.BusVersion='{1}'", BusID, BusVersion));

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        #endregion  BusServerBusInfoVer

        #region BusServerBusHTTP
        public static DataTable BusServerBusHTTPGet(string BusServerID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.BusName ,a.busDDLBS,b.*
 from BusinessInfo as a
join BusServerBusHTTP  as b on a.BusID = b.BusID
where b.BusServerID = @BusServerID  and b.status =1 ", BusServerID));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50)

            };
            parameters[0].Value = BusServerID;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// BusServer允许开放HTTP服务的BusIDList
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public static List<string> BusServerBusHTTPGetBusIDList(string BusServerID)
        {
            List<string> list = new List<string>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select  b.*
from  BusServerBusHTTP  as b  
where b.BusServerID = @BusServerID  and b.status =1", BusServerID));
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50)
            };
            parameters[0].Value = BusServerID;
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(dr["BusID"].ToString().Trim());
                    }
                }
            }
            return list;
        }


        public static bool BusServerBusHTTPSet(string BusServerID, string BusID, bool Enable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BusServerBusHTTP(");
            strSql.Append("BusServerID,BusID,Status ,AddTime,LastEditTime)");
            strSql.Append(" values (");
            strSql.Append("  @BusServerID,@BusID,@Status, @AddTime,@LastEditTime)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 Status =@Status,  LastEditTime=@LastEditTime");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusServerID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@Status", MySqlDbType.Int32) ,
               new MySqlParameter("@AddTime", MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime)};
            parameters[0].Value = BusServerID;
            parameters[1].Value = BusID;
            parameters[2].Value = Enable ? 1 : 0;
            parameters[3].Value = DateTime.Now;
            parameters[4].Value = DateTime.Now;
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
        #endregion

        #region  BusinessInfo

        public static BusinessInfo BusinessInfoGet(string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value from BusinessInfo where BusID=@BusID");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50)          };
            parameters[0].Value = BusID;
            object ovalue = DbHelperMySQLMySpring.GetSingle(strSql.ToString(), parameters);

            if (ovalue != null)
            {
                try
                {
                    return JsonConvert.DeserializeObject<BusinessInfo>((string)ovalue);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static List<BusinessInfo> BusinessInfoListGet(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value from BusinessInfo  ");
            if (!string.IsNullOrEmpty(where))
                strSql.Append("where " + where);
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                List<BusinessInfo> list = new List<BusinessInfo>();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    { list.Add(JsonConvert.DeserializeObject<BusinessInfo>(dr["Value"].ToString())); }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static bool BusinessInfoSet(BusinessInfo businessInfo, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BusinessInfo(");
            strSql.Append("BusID,BusName,VersionN,Status, BMODID,busType,busDDLBS,DllName,Namespace_Class,Version,ProjectID,Value,AddTime,LastEditTime,FilePath)");
            strSql.Append(" values (");
            strSql.Append("  @BusID,@BusName,@VersionN,@Status,@BMODID,@busType,@busDDLBS,@DllName,@NamespaceClass,@Version,@ProjectID,@Value,@AddTime ,@LastEditTime ,@FilePath)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 BusName =@BusName,VersionN=@VersionN,Status =@Status ,Version= @Version,ProjectID=@ProjectID,BMODID=@BMODID,busType=@busType,busDDLBS=@busDDLBS,DllName=@DllName,Namespace_Class=@NamespaceClass,Value=@Value,LastEditTime =@LastEditTime,FilePath=@FilePath");


            MySqlParameter[] parameters = {
                 new MySqlParameter("@BusID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@BusName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Status", MySqlDbType.Int32 ),
                    new MySqlParameter("@BMODID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@busType", MySqlDbType.Int32),
                    new MySqlParameter("@DllName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@NamespaceClass", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Value", MySqlDbType.VarChar,2000),
                    new MySqlParameter("@AddTime",  MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime),
             new MySqlParameter("@VersionN", MySqlDbType.Decimal,20),
              new MySqlParameter("@Version", MySqlDbType.VarChar,25),
              new MySqlParameter("@ProjectID", MySqlDbType.VarChar,20),
            new MySqlParameter("@busDDLBS", MySqlDbType.Int32 ),
             new MySqlParameter("@FilePath", MySqlDbType.VarChar,100)};

            if (string.IsNullOrWhiteSpace(businessInfo.BusName))
            {
                businessInfo.BusName = businessInfo.BusID;
            }
            Version version = null;
            Version.TryParse(businessInfo.version, out version);
            decimal deVersion = 0;
            if (version != null)
            {
                deVersion = VersionTodecimal(version);
            }

            parameters[0].Value = businessInfo.BusID;
            parameters[1].Value = businessInfo.BusName;
            parameters[2].Value = Status;
            parameters[3].Value = businessInfo.BMODID;
            parameters[4].Value = businessInfo.busType;
            parameters[5].Value = businessInfo.DllName;
            parameters[6].Value = businessInfo.NamespaceClass;
            parameters[7].Value = JsonConvert.SerializeObject(businessInfo);
            parameters[8].Value = DateTime.Now;
            parameters[9].Value = DateTime.Now;
            parameters[10].Value = deVersion;
            parameters[11].Value = businessInfo.version;
            parameters[12].Value = businessInfo.ProjectID;
            parameters[13].Value = businessInfo.busDDLBS;
            parameters[14].Value = businessInfo.DllPath;
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
        public static DataTable BusinessInfoTableGet(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from BusinessInfo  ");
            if (!string.IsNullOrEmpty(where))
                strSql.Append("where " + where);
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static DataTable BusinessInfoCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select count(1)  as'All' , COUNT(case Status when 1 then 1 end) as 'run'   from BusinessInfo");
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static DataTable BusinessInfoStatus()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@" select a.BusID,a.BusName ,a.Status,d.Code_Name as 'StatusName' , count(b.BusID) as 'ServerCount', count(c.id) as'RunServerCount'
from
 businessinfo as a
LEFT OUTER JOIN busserverbusinfo as b
on a.BusID = b.BusID
LEFT OUTER JOIN busserver as c
on b.BusServerID = c.ID and c.`Status`= 1
LEFT OUTER JOIN allcodeint as d
on a.`Status`=d.Code_ID and d.all_lbmc='BusinessStatus'
GROUP BY
a.BusID, a.BusName,a.Status");
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }


        public bool BusinessInfoExists(string projectID, string FilePath, string Filename, string Version)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from BusinessInfo");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20)          };
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;
            parameters[3].Value = Version;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }
        public static bool BusinessInfoDelete(string BusID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(
                  "Delete from dependentfile where BusID=@BusID ;" +
                   "Delete from busserverbushttp where BusID=@BusID ;" +
                "Delete from busserverbusinfover where BusID=@BusID ;" +
                "Delete from busserverbusinfo where BusID=@BusID ;" +
                "Delete from businessinfobusver where BusID=@BusID ;" +
                "Delete from BusinessInfo where BusID=@BusID ;");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50)          };
            parameters[0].Value = BusID;
            DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
            return true;
        }
     
        #endregion

        #region  BusinessInfoBusVer

        public static bool BusinessInfoBusVerSet(BusinessInfoBusVersion businessInfo, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into businessinfobusver(");
            strSql.Append("BusID,BusVersion,VersionN,Status,DllName,Namespace_Class,Version,ProjectID,Value,AddTime,LastEditTime,FilePath)");
            strSql.Append(" values (");
            strSql.Append("  @BusID,@BusVersion,@VersionN,@Status,@DllName,@NamespaceClass,@Version,@ProjectID,@Value,@AddTime ,@LastEditTime ,@FilePath)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 VersionN=@VersionN,Status =@Status ,Version= @Version,ProjectID=@ProjectID,DllName=@DllName,Namespace_Class=@NamespaceClass,Value=@Value,LastEditTime =@LastEditTime,FilePath=@FilePath");


            MySqlParameter[] parameters = {
                 new MySqlParameter("@BusID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@BusVersion", MySqlDbType.VarChar,26),
                    new MySqlParameter("@Status", MySqlDbType.Int32 ),
                    new MySqlParameter("@DllName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@NamespaceClass", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Value", MySqlDbType.VarChar,2000),
                    new MySqlParameter("@AddTime",  MySqlDbType.DateTime),
                    new MySqlParameter("@LastEditTime", MySqlDbType.DateTime),
             new MySqlParameter("@VersionN", MySqlDbType.Decimal,20),
              new MySqlParameter("@Version", MySqlDbType.VarChar,25),
              new MySqlParameter("@ProjectID", MySqlDbType.VarChar,20),
             new MySqlParameter("@FilePath", MySqlDbType.VarChar,100)};


            Version version = null;
            Version.TryParse(businessInfo.version, out version);
            decimal deVersion = 0;
            if (version != null)
            {
                deVersion = VersionTodecimal(version);
            }

            parameters[0].Value = businessInfo.BusID;
            parameters[1].Value = businessInfo.BusVersion;
            parameters[2].Value = Status;
            parameters[3].Value = businessInfo.DllName;
            parameters[4].Value = businessInfo.NamespaceClass;
            parameters[5].Value = JsonConvert.SerializeObject(businessInfo);
            parameters[6].Value = DateTime.Now;
            parameters[7].Value = DateTime.Now;
            parameters[8].Value = deVersion;
            parameters[9].Value = businessInfo.version;
            parameters[10].Value = businessInfo.ProjectID;
            parameters[11].Value = businessInfo.DllPath;
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
        public static List<BusinessInfoBusVersion> BusinessInfoVerListGet(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Value,BusVersion from businessinfobusver  ");
            if (!string.IsNullOrEmpty(where))
                strSql.Append("where " + where);
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                List<BusinessInfoBusVersion> list = new List<BusinessInfoBusVersion>();
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BusinessInfoBusVersion infoBusVersion = JsonConvert.DeserializeObject<BusinessInfoBusVersion>(dr["Value"].ToString());
                        if (string.IsNullOrEmpty(infoBusVersion.ComMinVer))
                        {
                            infoBusVersion.ComMinVer = "1.0.0.0";
                        }
                        if (string.IsNullOrEmpty(infoBusVersion.ComMaxVer))
                        {
                            infoBusVersion.ComMaxVer = "1.0.65535.65535";
                        }
                        if (string.IsNullOrEmpty(infoBusVersion.BusVersion))
                        {
                            infoBusVersion.BusVersion = dr["BusVersion"].ToString();
                        }
                        list.Add(infoBusVersion);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static BusinessInfoBusVersion BusinessInfoVerGet(string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select Value from BusinessInfo where BusID=@BusID; 
                select Value,BusVersion from businessinfobusver where BusID=@BusID and BusVersion =@BusVersion");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50)   ,
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,50)   };
            parameters[0].Value = BusID;
            parameters[1].Value = BusVersion;
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);

            if (ds != null)
            {
                try
                {
                    // BusID,BusVersion,VersionN,Status,DllName,Namespace_Class,Version,ProjectID,Value,AddTime,LastEditTime,FilePath
                    BusinessInfo businessInfo = JsonConvert.DeserializeObject<BusinessInfo>((string)ds.Tables[0].Rows[0]["Value"]);
                    BusinessInfoBusVersion businessVer = JsonConvert.DeserializeObject<BusinessInfoBusVersion>((string)ds.Tables[1].Rows[0]["Value"]);
                    businessVer.Updateinfo(businessInfo);
                    if (string.IsNullOrEmpty(businessVer.ComMinVer))
                    {
                        businessVer.ComMinVer = "1.0.0.0";
                    }
                    if (string.IsNullOrEmpty(businessVer.ComMaxVer))
                    {
                        businessVer.ComMaxVer = "1.0.65535.65535";
                    }
                    if (string.IsNullOrEmpty(businessVer.BusVersion))
                    {
                        businessVer.BusVersion = (string)ds.Tables[1].Rows[0]["BusVersion"];
                    }
                    return businessVer;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static BusinessInfoBusVersion BusinessInfoVerGet(string BusServerID,string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select Value from BusinessInfo where BusID=@BusID; 
                select Value,BusVersion from businessinfobusver where BusID=@BusID and BusVersion =@BusVersion;
                  select a.* from BusinessInfoVersion a join  busserverbusinfover b on   a.BusID=b.BusID and a.BusVersion =b.BusVersion and a.VersionN =b.VersionN  
where b.BusServerID =@BusServerID and b.BusID=@BusID and b.BusVersion =@BusVersion; ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50)   ,
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,50),
             new MySqlParameter("@BusServerID", MySqlDbType.VarChar,50)   };
            parameters[0].Value = BusID;
            parameters[1].Value = BusVersion;
            parameters[2].Value = BusServerID;
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);

            if (ds != null)
            {
                try
                {
                    // BusID,BusVersion,VersionN,Status,DllName,Namespace_Class,Version,ProjectID,Value,AddTime,LastEditTime,FilePath
                    BusinessInfo businessInfo = JsonConvert.DeserializeObject<BusinessInfo>((string)ds.Tables[0].Rows[0]["Value"]);
                    BusinessInfoBusVersion businessVer = JsonConvert.DeserializeObject<BusinessInfoBusVersion>((string)ds.Tables[1].Rows[0]["Value"]);
                    businessVer.Updateinfo(businessInfo);
                    if (string.IsNullOrEmpty(businessVer.ComMinVer))
                    {
                        businessVer.ComMinVer = "1.0.0.0";
                    }
                    if (string.IsNullOrEmpty(businessVer.ComMaxVer))
                    {
                        businessVer.ComMaxVer = "1.0.65535.65535";
                    }
                    if (string.IsNullOrEmpty(businessVer.BusVersion))
                    {
                        businessVer.BusVersion = (string)ds.Tables[1].Rows[0]["BusVersion"];
                    }

                    businessVer.VersionN = ds.Tables[2].Rows[0]["VersionN"].ToString();
                    businessVer.DllName = ds.Tables[2].Rows[0]["DllName"].ToString();
                    businessVer.DllPath = ds.Tables[2].Rows[0]["FilePath"].ToString();
                    businessVer.version = ds.Tables[2].Rows[0]["version"].ToString();
                    businessVer.ProjectID = ds.Tables[2].Rows[0]["ProjectID"].ToString();
                    return businessVer;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static bool BusinessInfoBusVerDelete(string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("Delete from busserverbusinfover where BusID=@BusID  and BusVersion =@BusVersion;" +
                "Delete from BusinessInfoBusVer where BusID=@BusID  and BusVersion =@BusVersion;");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50)  ,
                new MySqlParameter("@BusVersion", MySqlDbType.VarChar,50)         };
            parameters[0].Value = BusID;
            parameters[1].Value = BusVersion;
            DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
            return true;
        }
        #endregion BusinessInfoBusVer

        #region 灰度测试标记 StatusAndMark
        //
        public static DataSet GetDefaultStatusAndMark()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" call GetDefaultStatusAndMark()");
            return DbHelperMySQLMySpring.Query(strSql.ToString());

        }
        /// <summary>
        ///默认灰度测试标记
        /// </summary>
        /// <returns></returns>
        public static ConcurrentDictionary<string, List<string>> GetDefaultStatusAndMarkcd()
        {
            return StatusAndMarkdsTocd(GetDefaultStatusAndMark());
        }
        private static ConcurrentDictionary<string, List<string>> StatusAndMarkdsTocd(DataSet ds)
        {
            ConcurrentDictionary<string, List<string>> sdDefaultTestMarkT = new ConcurrentDictionary<string, List<string>>();
            if (ds.Tables.Count == 2 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr0 in ds.Tables[0].Rows)
                {
                    string StatusMarkType = dr0["StatusMarkType"].ToString();
                    DataRow[] drs = ds.Tables[1].Select("StatusMarkType='" + StatusMarkType + "'");
                    if (drs.Length > 0)
                    {
                        List<string> list = new List<string>();
                        foreach (DataRow dr1 in drs)
                        {
                            list.Add(dr1["MarkValue"].ToString().Trim());
                        }
                        sdDefaultTestMarkT.TryAdd(StatusMarkType, list);
                    }
                }
            }
            return sdDefaultTestMarkT;
        }
        public static DataSet GetBusStatusAndMark(string BusID, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" call GetBusStatusAndMark( '" + BusID + "','" + BusVersion + "')");
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取状态和
        /// </summary>
        /// <param name="BusID"></param>
        /// <param name="BusVersion"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, List<string>> GetBusStatusAndMarkcd(string BusID, string BusVersion)
        {
            return StatusAndMarkdsTocd(GetBusStatusAndMark(BusID, BusVersion));
        }
        #endregion  StatusAndMark


        #region  TestGroupBusVersio

        public static DataTable TestGroupBusVersionGetRecommend(string BusID, decimal VersionN, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select DISTINCT
 @VersionN as 'VersionN', c.DefaultStatus as 'Status',  c.GroupID,c.GroupName ,true as 'check', true as 'Default'
from  testgroup as c  
 where   c.DefaultStatus= @Status
union
select  DISTINCT
 b.VersionN,b.StatusG as 'Status' ,c.GroupID,c.GroupName, true as 'check',false as 'Default'
from   testgroupbusversion  as b
join testgroup    as c on b.GroupID = c.GroupID
 where b.BusID = @BusID and b.VersionN = @VersionN and b.StatusG=  @Status
 union
 select  DISTINCT
 @VersionN as 'VersionN',b.StatusG as 'Status' ,c.GroupID,c.GroupName, false as 'check',false as 'Default'
from testgroupbusversion as b 
join testgroup    as c on b.GroupID = c.GroupID
 where b.BusID = @BusID and b.VersionN =0 and b.StatusG =   @Status
and  b.GroupID not in (select   GroupID 
from   testgroupbusversion 
 where BusID =@BusID and VersionN = @VersionN  and StatusG=   @Status  ) ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50) ,
             new MySqlParameter("@VersionN", MySqlDbType.Decimal),
             new MySqlParameter("@Status", MySqlDbType.Int32 )};
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = Status;
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static DataTable TestGroupBusVersionGetRecommend(string BusID, string BusVersion, decimal VersionN, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select DISTINCT
 @VersionN as 'VersionN', c.DefaultStatus as 'Status',  c.GroupID,c.GroupName ,true as 'check', true as 'Default'
from  testgroup as c  
 where   c.DefaultStatus= @Status
union
select  DISTINCT
 b.VersionN,b.StatusG as 'Status' ,c.GroupID,c.GroupName, true as 'check',false as 'Default'
from   testgroupbusversion  as b
join testgroup    as c on b.GroupID = c.GroupID
 where b.BusID = @BusID  and b.BusVersion = @BusVersion  and b.VersionN = @VersionN and b.StatusG=  @Status
 union
 select  DISTINCT
 @VersionN as 'VersionN',b.StatusG as 'Status' ,c.GroupID,c.GroupName, false as 'check',false as 'Default'
from testgroupbusversion as b 
join testgroup    as c on b.GroupID = c.GroupID
 where b.BusID = @BusID and b.BusVersion = @BusVersion and b.VersionN =0 and b.StatusG =   @Status
and  b.GroupID not in (select   GroupID 
from   testgroupbusversion 
 where BusID =@BusID and BusVersion = @BusVersion  and VersionN = @VersionN  and StatusG=   @Status  ) ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50) ,
             new MySqlParameter("@VersionN", MySqlDbType.Decimal),
             new MySqlParameter("@Status", MySqlDbType.Int32 ),
              new MySqlParameter("@BusVersion",MySqlDbType.VarChar,50 )
            };
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = Status;
            parameters[3].Value = BusVersion;
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        public static bool TestGroupBusVersionSet(int GroupID, string BusID, decimal VersionN, int Status, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TestGroupBusVersion(");
            strSql.Append("BusID,VersionN,GroupID,StatusG,BusVersion)");
            strSql.Append(" values (");
            strSql.Append("  @BusID,@VersionN,@GroupID,@Status,@BusVersion)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50) ,
             new MySqlParameter("@VersionN", MySqlDbType.Decimal),
             new MySqlParameter("@Status", MySqlDbType.Int32 ),
             new MySqlParameter("@GroupID", MySqlDbType.Int32),
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,50) };
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = Status;
            parameters[3].Value = GroupID;
            parameters[4].Value = BusVersion;

            try
            {
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
            catch
            {
                return false;
            }
        }


        public static bool TestGroupBusVersionDel(int GroupID, string BusID, decimal VersionN, int Status, string BusVersion)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update      TestGroupBusVersion   set VersionN= 0 ");
            strSql.Append("where BusID=  @BusID and   GroupID=@GroupID and VersionN=@VersionN and StatusG=@Status and BusVersion =@BusVersion");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50) ,
             new MySqlParameter("@VersionN", MySqlDbType.Decimal),
             new MySqlParameter("@Status", MySqlDbType.Int32 ),
             new MySqlParameter("@GroupID", MySqlDbType.Int32),
             new MySqlParameter("@BusVersion", MySqlDbType.VarChar,50) };
            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = Status;
            parameters[3].Value = GroupID;
            parameters[4].Value = BusVersion;
            int rows = 1;
            try
            {
                //rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
                strSql = new StringBuilder();
                strSql.Append("delete from       TestGroupBusVersion    ");
                strSql.Append("where BusID=  @BusID and   GroupID=@GroupID and VersionN=@VersionN and StatusG=@Status and BusVersion =@BusVersion");
                rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);

            }
            catch
            {
                rows = 0;
            }
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool TestGroupBusVersionDelDef(string BusID, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete  from  TestGroupBusVersion where ");
            strSql.Append("BusID=  @BusID and  VersionN= 0 , and StatusG=@Status)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,50) ,
             new MySqlParameter("@Status", MySqlDbType.Int32 )};
            parameters[0].Value = BusID;
            parameters[1].Value = Status;
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
        public static bool TestGroupBusVersionSetDef(int GroupID, string BusID, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TestGroupBusVersion(");
            strSql.Append("BusID,VersionN,GroupID,StatusG)");
            strSql.Append(" values (");
            strSql.Append("  @BusID,@VersionN,@GroupID,@Status)");
            MySqlParameter[] parameters = {
             new MySqlParameter("@BusID", MySqlDbType.VarChar,50) ,
             new MySqlParameter("@VersionN", MySqlDbType.Decimal),
             new MySqlParameter("@Status", MySqlDbType.Int32 ),
             new MySqlParameter("@GroupID", MySqlDbType.Int32)};
            parameters[0].Value = BusID;
            parameters[1].Value = 0;
            parameters[2].Value = Status;
            parameters[3].Value = GroupID;

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

        #endregion  TestGroupBusVersio

        #region  TestGroup
        // 
        public static DataTable TestGroupGet()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from  TestGroup");
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static bool TestGroupSet(int GroupID, string GroupName, int DefaultStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TestGroup(");
            strSql.Append("GroupID,GroupName,DefaultStatus)");
            strSql.Append(" values (");
            strSql.Append("  @GroupID,@GroupName,@DefaultStatus)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 GroupName =@GroupName,DefaultStatus=@DefaultStatus");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@GroupID", MySqlDbType.Int32),
                    new MySqlParameter("@GroupName", MySqlDbType.VarChar,50),
                    new MySqlParameter("@DefaultStatus", MySqlDbType.Int32)          };
            parameters[0].Value = GroupID;
            parameters[1].Value = GroupName;
            parameters[2].Value = DefaultStatus;
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
        public static bool TestGroupDel(int GroupID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from  TestGroupMember where GroupID= @GroupID;");
            strSql.Append("delete from  TestGroupBusVersion where GroupID= @GroupID;");
            strSql.Append("delete from  TestGroup where GroupID= @GroupID;");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@GroupID", MySqlDbType.Int32)         };
            parameters[0].Value = GroupID;

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
        #endregion  TestGroup

        #region  TestMark
        //TestMark
        public static DataTable TestMarkGet()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from  TestMark");
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static bool TestMarkSet(int MarkID, string MarkType, string MarkValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TestMark(");
            strSql.Append("MarkID,MarkType,MarkValue)");
            strSql.Append(" values (");
            strSql.Append("  @MarkID,@MarkType,@MarkValue)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 MarkType =@MarkType,MarkValue=@MarkValue");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@MarkID", MySqlDbType.Int32),
                    new MySqlParameter("@MarkType", MySqlDbType.VarChar,20),
                    new MySqlParameter("@MarkValue",  MySqlDbType.VarChar,50)          };
            parameters[0].Value = MarkID;
            parameters[1].Value = MarkType;
            parameters[2].Value = MarkValue;
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

        public static bool TestMarkDel(int MarkID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from  TestGroupMember where MarkID= @MarkID;");
            strSql.Append("delete from  TestMark where MarkID= @MarkID;");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@MarkID", MySqlDbType.Int32)         };
            parameters[0].Value = MarkID;

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
        #endregion  TestMark

        #region  TestGroupMember
        //TestGroupMember
        public static DataTable TestGroupMemberGet(int GroupID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select a.* ,b.MarkType,b.MarkValue
from TestGroupMember as a
join testmark  as b on a.MarkID = b.MarkID
where GroupID =" + GroupID.ToString());
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static bool TestGroupMemberAdd(int GroupID, int MarkID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TestGroupMember(");
            strSql.Append("GroupID,MarkID)");
            strSql.Append(" values (");
            strSql.Append("  @GroupID,@MarkID )");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@GroupID", MySqlDbType.Int32),
                    new MySqlParameter("@MarkID",  MySqlDbType.Int32)          };
            parameters[0].Value = GroupID;
            parameters[1].Value = MarkID;
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
        public static bool TestGroupMemberDel(int GroupID, int MarkID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from  TestGroupMember ");
            strSql.Append(" where GroupID=@GroupID  and MarkID=@MarkID");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@GroupID", MySqlDbType.Int32),
                    new MySqlParameter("@MarkID",  MySqlDbType.Int32)          };
            parameters[0].Value = GroupID;
            parameters[1].Value = MarkID;
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
        #endregion  TestGroupMember

        #region DependentFile
        public static DataTable DependentFileGet(string BusID, decimal VersionN)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select *
from DependentFile  
where BusID ='{0}' and VersionN ={1}", BusID, VersionN));
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        public static bool DependentFileInsert(string BusID, decimal VersionN, string projectID, string FilePath, string Filename, string version)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into DependentFile(");
            strSql.Append("BusID,VersionN,ProjectID, FilePath,Filename,version,AddTime)");
            strSql.Append(" values (");
            strSql.Append("  @BusID,@VersionN,@ProjectID,@FilePath,@Filename,@version,@AddTime)");

            MySqlParameter[] parameters = {
                 new MySqlParameter("@BusID", MySqlDbType.VarChar,50),
                  new MySqlParameter("@VersionN", MySqlDbType.Decimal,20),
             new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20),
                    new MySqlParameter("@AddTime",  MySqlDbType.DateTime)};

            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = projectID;
            parameters[3].Value = FilePath;
            parameters[4].Value = Filename;
            parameters[5].Value = version;
            parameters[6].Value = DateTime.Now;
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

        public static bool DependentFileDelete(string BusID, decimal VersionN, string projectID, string FilePath, string Filename, string version)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from  DependentFile where ");
            strSql.Append("BusID= @BusID  and VersionN = @VersionN and ProjectID=@ProjectID and  FilePath =@FilePath and Filename=@Filename and version=@version ");


            MySqlParameter[] parameters = {
                 new MySqlParameter("@BusID", MySqlDbType.VarChar,50),
                  new MySqlParameter("@VersionN", MySqlDbType.Decimal,20),
             new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20)};

            parameters[0].Value = BusID;
            parameters[1].Value = VersionN;
            parameters[2].Value = projectID;
            parameters[3].Value = FilePath;
            parameters[4].Value = Filename;
            parameters[5].Value = version;

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
        public static bool DependentFileDelete(string BusID, decimal VersionN)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from  DependentFile where ");
            strSql.Append("BusID= @BusID  and VersionN = @VersionN");


            MySqlParameter[] parameters = {
                 new MySqlParameter("@BusID", MySqlDbType.VarChar,50),
                  new MySqlParameter("@VersionN", MySqlDbType.Decimal,20)};
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

        #endregion


        #region ApiGroup


        /// <summary>
        /// 获取指定APIUser_id所属的组
        /// </summary>
        /// <param name="APIUser_id"></param>
        /// <returns></returns>
        public static DataTable ApiUserGroupTbGet(string APIUser_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select 
a.APIG_ID, a.APIG_Name
from apigroup a
join APIUserAndGroup b
on a.APIG_ID = b.APIG_ID
where b.APIU_ID = '{0}' and a.Mark_Stop = 0", APIUser_id));
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }


        public static bool ApiUserandGroupUpdate(string APIUser_id, List<int> addGroup, List<int> delGroup, string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            if (delGroup != null)
            {
                foreach (int APIG_ID in delGroup)
                    strSql.Append(string.Format("delete from  APIUserAndGroup where APIU_ID ='{0}' and  APIG_ID={1} ;", APIUser_id, APIG_ID));
            }
            if (addGroup != null)
            {
                foreach (int APIG_ID in addGroup)
                    strSql.Append(string.Format(@" INSERT INTO `apiuserandgroup`
 (`APIU_ID`,`APIG_ID`,`Add_Time`,`AddUser_ID` )
 VALUES('{0}', '{1}', '{2}', '{3}')  ; ", APIUser_id, APIG_ID, DateTime.Now, User_ID));
            }

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ApiGroupandUserUpdate(int APIG_ID, List<string> addList, List<string> delList, string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            if (delList != null)
            {
                foreach (string APIUser_id in delList)
                    strSql.Append(string.Format("delete from  APIUserAndGroup where APIU_ID ='{0}' and  APIG_ID={1} ;", APIUser_id, APIG_ID));
            }
            if (addList != null)
            {
                foreach (string APIUser_id in addList)
                    strSql.Append(string.Format(@" INSERT INTO `apiuserandgroup`
 (`APIU_ID`,`APIG_ID`,`Add_Time`,`AddUser_ID` )
 VALUES('{0}', '{1}', '{2}', '{3}')  ; ", APIUser_id, APIG_ID, DateTime.Now, User_ID));
            }

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString());
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
        /// 获取指定API_ID 对应的组
        /// </summary>
        /// <param name="API_ID"></param>
        /// <returns></returns>
        public static DataTable ApiListandGroupTbGet(int API_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.APIG_ID, a.APIG_Name
from apigroup a
join apiandgroup b
on a.APIG_ID = b.APIG_ID
where b.API_ID = {0} and a.Mark_Stop = 0", API_ID));
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 更新指定API_ID对应组
        /// </summary>
        /// <param name="API_ID"></param>
        /// <param name="addGroup"></param>
        /// <param name="delGroup"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public static bool ApiListandGroupUpdate(int API_ID, List<int> addGroup, List<int> delGroup, string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            if (delGroup != null)
            {
                foreach (int APIG_ID in delGroup)
                    strSql.Append(string.Format("delete from  APIandGroup where API_ID = {0}  and  APIG_ID={1} ;", API_ID, APIG_ID));
            }
            if (addGroup != null)
            {
                foreach (int APIG_ID in addGroup)
                    strSql.Append(string.Format(@" INSERT INTO `APIandGroup`
 (`API_ID`,`APIG_ID`,`Add_Time`,`AddUser_ID` )
 VALUES('{0}', '{1}', '{2}', '{3}')  ; ", API_ID, APIG_ID, DateTime.Now, User_ID));
            }

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString());
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
        /// 更新指定API组对应API
        /// </summary>
        /// <param name="API_ID"></param>
        /// <param name="addAPIL"></param>
        /// <param name="delAPIL"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public static bool ApiGroupandAPIUpdate(int APIG_ID, List<int> addAPIL, List<int> delAPIL, string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            if (delAPIL != null)
            {
                foreach (int API_ID in delAPIL)
                    strSql.Append(string.Format("delete from  APIandGroup where API_ID = {0}  and  APIG_ID={1} ;", API_ID, APIG_ID));
            }
            if (addAPIL != null)
            {
                foreach (int API_ID in addAPIL)
                    strSql.Append(string.Format(@" INSERT INTO `APIandGroup`
 (`API_ID`,`APIG_ID`,`Add_Time`,`AddUser_ID` )
 VALUES('{0}', '{1}', '{2}', '{3}')  ; ", API_ID, APIG_ID, DateTime.Now, User_ID));
            }

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString());
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
        /// 获取指定机构授权的系统
        /// </summary>
        /// <param name="ORG_ID"></param>
        /// <returns></returns>
        public static DataTable SorgandSysTbGet(string ORG_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.SYSID, a.SYS_Name
from ssysteminfo a
join SorgandSys b
on a.SYSID = b.SYSID
where b.ORG_ID = '{0}' and a.Mark_Stop = 0", ORG_ID));
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 更机构具有的系统权限
        /// </summary>
        /// <param name="ORG_ID"></param>
        /// <param name="addGroup"></param>
        /// <param name="delGroup"></param>
        /// <param name="User_ID"></param>
        /// <returns></returns>
        public static bool SorgandSysUpdate(string ORG_ID, List<int> addGroup, List<int> delGroup, string User_ID)
        {
            StringBuilder strSql = new StringBuilder();
            if (delGroup != null)
            {
                foreach (int SYSID in delGroup)
                    strSql.Append(string.Format("delete from  SorgandSys where ORG_ID ='{0}' and  SYSID={1} ;", ORG_ID, SYSID));
            }
            if (addGroup != null)
            {
                foreach (int SYSID in addGroup)
                    strSql.Append(string.Format(@" INSERT INTO `SorgandSys`
 (`ORG_ID`,`SYSID`,`Add_Time`,`AddUser_ID` )
 VALUES('{0}', '{1}', '{2}', '{3}')  ; ", ORG_ID, SYSID, DateTime.Now, User_ID));
            }

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        public static bool BusInfoTestCountSet(string BusID, string version, BusinessInfoVersionStatus VersionStatus)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into BusTestCount(");
            strSql.Append("BusID,version, VersionStatus,TestCount,FirstTime,LastTime)");
            strSql.Append(" values (  @BusID,@version, @VersionStatus,@TestCount,@FirstTime,@LastTime)");
            strSql.Append("	ON DUPLICATE KEY UPDATE  ");
            strSql.Append("	 TestCount =TestCount+1, LastTime=@LastTime");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@version", MySqlDbType.VarChar,20),
                    new MySqlParameter("@VersionStatus", MySqlDbType.Int32) ,
             new MySqlParameter("@TestCount", MySqlDbType.Int32),
               new MySqlParameter("@FirstTime", MySqlDbType.DateTime),
                    new MySqlParameter("@LastTime", MySqlDbType.DateTime)};
            parameters[0].Value = BusID;
            parameters[1].Value = version;
            parameters[2].Value = (int)VersionStatus;
            parameters[3].Value = 1;
            parameters[4].Value = DateTime.Now;
            parameters[5].Value = DateTime.Now;
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

        #region 主机及端口服务检测

        /// <summary>
        /// 获取需要检测的主机或地址
        /// </summary>
        /// <returns></returns>
        public static List<SPinghost> GetPinghost()
        {
            List<SPinghost> list = new List<SPinghost>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select *  from Pinghost "));
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {

                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SPinghost sPinghost = new SPinghost();
                        sPinghost.HostNameOrAddress = dr["HostNameOrAddress"].ToString();
                        sPinghost.HostNote = dr["HostNote"].ToString();
                        sPinghost.HideHost = dr["HideHost"].ToString() == "1";

                        sPinghost.TroubleShoot = dr["TroubleShoot"].ToString();
                        sPinghost.Status = 0;
                        list.Add(sPinghost);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return list;
                }
            }
            else
            {
                return list;
            }
        }

        /// <summary>
        /// 获取需要检测的端口服务
        /// </summary>
        /// <returns></returns>
        public static List<SDetectionPort> GetDetectionPort()
        {
            List<SDetectionPort> list = new List<SDetectionPort>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select *  from DetectionPort "));
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {

                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SDetectionPort sDetectionPort = new SDetectionPort();
                        sDetectionPort.HostNameOrAddress = dr["HostNameOrAddress"].ToString();
                        sDetectionPort.Port = (int)dr["Port"];
                        sDetectionPort.ServerNote = dr["ServerNote"].ToString();
                        sDetectionPort.TroubleShoot = dr["TroubleShoot"].ToString();
                        sDetectionPort.Status = 0;
                        list.Add(sDetectionPort);
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    return list;
                }
            }
            else
            {
                return list;
            }
        }


        #endregion
        /// <summary>
        /// 字符型版本号转换成数值型
        /// </summary>
        /// <param name="strVersion"></param>
        /// <returns></returns>
        public static decimal VersionTodecimal(string strVersion)
        {
            Version version = null;
            Version.TryParse(strVersion, out version);

            return VersionTodecimal(version);
        }

        public static decimal VersionTodecimal(Version ver)
        {
            decimal Step = 65536;
            if (ver != null)
            {
                decimal Verint = 0;
                if (ver.Revision >= 0)
                {
                    Verint += ver.Revision;
                }
                if (ver.Build >= 0)
                {
                    Verint += ver.Build * Step;
                }
                if (ver.Minor >= 0)
                {
                    Verint += ver.Minor * Step * Step;
                }
                if (ver.Major >= 0)
                {
                    Verint += ver.Major * Step * Step * Step;
                }
                return Verint;
            }
            else
            {
                return Step * Step * Step;
            }
        }
        public static bool BusinessInfoSetStatus(string BusName, BusinessStatus businessStatus)
        {
            int rows = DbHelperMySQLMySpring.ExecuteSql(string.Format(" update BusinessInfo set Status ={0}where BusName='[1}'", (int)businessStatus, BusName));
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
        /// MySpringLog 中创建日志表
        /// </summary>
        /// <param name="tabSQL"></param>
        /// <returns></returns>
        public static bool CreateLogTable(string tabSQL)
        {
            try
            {
                DbHelperMySQLMySpringLog.ExecuteSql(tabSQL);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
