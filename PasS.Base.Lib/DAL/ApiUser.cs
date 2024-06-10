using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using PasS.Base.Lib.Model;
using System.Collections.Generic;

namespace PasS.Base.Lib.DAL
{
    /// <summary>
    /// 数据访问类:apiuser
    /// </summary>
    public partial class APIUser
    {
        public APIUser()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string APIU_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from apiuser");
            strSql.Append(" where APIU_ID=@APIU_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,10)          };
            parameters[0].Value = APIU_ID;

            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(apiuser model)
        {
            StringBuilder strSql = new StringBuilder();
            int RSAPubID = 0, RSA2PubID = 0, SpringRSAID = 0;
           
            strSql.Append("insert into apiuser(");
            strSql.Append("APIU_ID,APIU_Name,AESKey,RSAPubKey,RSAPubID,RSA2PubKey,RSA2PubID,IP_whitelist,Add_Time,Mark_Stop,Stop_Time,ORG_ID,SpringRSAPriKey,SpringRSAPubKey,SpringRSAID,AType)");
            strSql.Append(" values (");
            strSql.Append("@APIU_ID,@APIU_Name,@AESKey,@RSAPubKey,@RSAPubID,@RSA2PubKey,@RSA2PubID,@IP_whitelist,@Add_Time,@Mark_Stop,@Stop_Time,@ORG_ID,@SpringRSAPriKey,@SpringRSAPubKey,@SpringRSAID,@AType);");
            if (!string.IsNullOrEmpty(model.RSAPubKey))
            {
                DbHelper.GetSysIdBase("APIUSERRSAID", out RSAPubID);
                strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@RSAPubID,'RSA',@RSAPubKey) ;");
            }
            if (!string.IsNullOrEmpty(model.RSA2PubKey))
            {
                DbHelper.GetSysIdBase("APIUSERRSAID", out RSA2PubID);
                strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@RSA2PubID,'RSA2',@RSA2PubKey) ;");

            }
            if (!string.IsNullOrEmpty(model.SpringRSAPriKey))
            {
                DbHelper.GetSysIdBase("APIUSERRSAID", out SpringRSAID);
                strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@SpringRSAID,'SRSAPri',@SpringRSAPriKey) ;");
                strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@SpringRSAID,'SRSAPub',@SpringRSAPubKey) ;");

            }
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@APIU_Name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@AESKey", MySqlDbType.VarChar,50),
                    new MySqlParameter("@RSAPubKey", MySqlDbType.VarChar,500),
                    new MySqlParameter("@RSA2PubKey", MySqlDbType.VarChar,500),
                    new MySqlParameter("@IP_whitelist", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Add_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@Stop_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@RSAPubID", MySqlDbType.Int32),
                    new MySqlParameter("@RSA2PubID", MySqlDbType.Int32),
                      new MySqlParameter("@SpringRSAPriKey", MySqlDbType.VarChar,100),
                        new MySqlParameter("@SpringRSAPubKey", MySqlDbType.VarChar,100),
               new MySqlParameter("@SpringRSAID", MySqlDbType.Int32),
                new MySqlParameter("@AType", MySqlDbType.Int32)};
            parameters[0].Value = model.APIU_ID;
            parameters[1].Value = model.APIU_Name;
            parameters[2].Value = model.AESKey;
            parameters[3].Value = model.RSAPubKey;
            parameters[4].Value = model.RSA2PubKey;
            parameters[5].Value = model.IP_whitelist;
            parameters[6].Value = model.Add_Time;
            parameters[7].Value = model.Mark_Stop;
            parameters[8].Value = model.Stop_Time;
            parameters[9].Value = model.ORG_ID;
            parameters[10].Value = RSAPubID;
            parameters[11].Value = RSA2PubID;
            parameters[12].Value = model.SpringRSAPriKey;
            parameters[13].Value = model.SpringRSAPubKey;
            parameters[14].Value = SpringRSAID;
            parameters[15].Value = model.AType;
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
            catch(Exception ex)
            {
                return false;
            }
        
        }
        /// <summary>
        /// 旧Key过期天数，实时获取
        /// </summary>
        /// <returns></returns>
        int ExpiresDate()
        {
            try { return int.Parse(DbHelper.SysInfoGet("RSAExpiresDate")); }
            catch
            {
                return 7;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RSAPubKeyChnage"></param>
        /// <returns></returns>
        public bool Update(apiuser model, bool RSAPubKeyChnage, bool RSA2PubKeyChnage, bool SpringRSAChnage,bool AESChnage,string OldAESKey)
        {
            StringBuilder strSql = new StringBuilder();
            if (RSAPubKeyChnage)
            {
                strSql.Append($@"update  apiuserrsalist a, apiuser b set a.ExpiresDate = date_add(now(), interval {ExpiresDate()} day)
where a.APIU_ID = b.APIU_ID and a.RSAPubID = b.RSAPubID and a.RType = 'RSA' and a.APIU_ID = @APIU_ID;");
                int RSAPubID = 0;
                if (!string.IsNullOrEmpty(model.RSAPubKey))
                {
                    DbHelper.GetSysIdBase("APIUSERRSAID", out RSAPubID);
                    strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@RSAPubID,'RSA',@RSAPubKey) ;");
                }
                model.RSAPubID = RSAPubID;
            }
            if (RSA2PubKeyChnage)
            {
                strSql.Append($@"update  apiuserrsalist a, apiuser b set a.ExpiresDate = date_add(now(), interval {ExpiresDate()} day)
where a.APIU_ID = b.APIU_ID and a.RSAPubID = b.RSA2PubID and a.RType = 'RSA2' and a.APIU_ID = @APIU_ID;");
                int RSA2PubID = 0;
                if (!string.IsNullOrEmpty(model.RSA2PubKey))
                {
                    DbHelper.GetSysIdBase("APIUSERRSAID", out RSA2PubID);
                    strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@RSA2PubID,'RSA2',@RSA2PubKey) ;");
                }
                model.RSA2PubID = RSA2PubID;
            }
            if (SpringRSAChnage)
            {
                strSql.Append($@"update  apiuserrsalist a, apiuser b set a.ExpiresDate = date_add(now(), interval {ExpiresDate()} day)
where a.APIU_ID = b.APIU_ID and a.RSAPubID = b.SpringRSAID and (a.RType = 'SRSAPri' or a.RType = 'SRSAPub') and a.APIU_ID = @APIU_ID;");
                int SpringRSAID = 0;
                DbHelper.GetSysIdBase("APIUSERRSAID", out SpringRSAID);
                if (!string.IsNullOrEmpty(model.SpringRSAPriKey))
                {
                    strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@SpringRSAID,'SRSAPri',@SpringRSAPriKey) ;");
                    strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@SpringRSAID,'SRSAPub',@SpringRSAPubKey) ;");
                }
                model.SpringRSAID = SpringRSAID;
            }
            if (AESChnage)
            {//AESKey 最新的值在apiuserrsalist 中不保存 ，只将过期的存在apiuserrsalist 中

                int RSAPubID = 0;
                if (!string.IsNullOrEmpty(OldAESKey))
                {
                    DbHelper.GetSysIdBase("APIUSERRSAID", out RSAPubID);
                    strSql.Append($@" insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey,ExpiresDate) values (@APIU_ID,{RSAPubID},'AES',@OldAESKey,date_add(now(), interval {ExpiresDate()} day) ) ;");
                }
               
            }
            strSql.Append("update apiuser set ");
            strSql.Append("APIU_Name=@APIU_Name,");
            if (AESChnage)
                strSql.Append("AESKey=@AESKey,");
            if (RSAPubKeyChnage)
            {
                strSql.Append("RSAPubKey=@RSAPubKey,");
                strSql.Append("RSAPubID=@RSAPubID,");

            }
            if (RSA2PubKeyChnage)
            {
                strSql.Append("RSA2PubKey=@RSA2PubKey,");
                strSql.Append("RSA2PubID=@RSA2PubID,");
            }
            if (SpringRSAChnage)
            {
                strSql.Append("SpringRSAPriKey=@SpringRSAPriKey,");
                strSql.Append("SpringRSAPubKey=@SpringRSAPubKey,");
                strSql.Append("SpringRSAID=@SpringRSAID,");
            }
            strSql.Append("IP_whitelist=@IP_whitelist,");
            strSql.Append("Add_Time=@Add_Time,");
            strSql.Append("Mark_Stop=@Mark_Stop,");
            strSql.Append("Stop_Time=@Stop_Time,");
            strSql.Append("ORG_ID=@ORG_ID");
            strSql.Append(" where APIU_ID=@APIU_ID ;");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_Name", MySqlDbType.VarChar,50),
                    new MySqlParameter("@AESKey", MySqlDbType.VarChar,50),
                    new MySqlParameter("@RSAPubKey", MySqlDbType.VarChar,500),
                    new MySqlParameter("@RSA2PubKey", MySqlDbType.VarChar,500),
                    new MySqlParameter("@IP_whitelist", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Add_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@Mark_Stop", MySqlDbType.Bit),
                    new MySqlParameter("@Stop_Time", MySqlDbType.DateTime),
                    new MySqlParameter("@ORG_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@RSAPubID", MySqlDbType.Int32),
                    new MySqlParameter("@RSA2PubID", MySqlDbType.Int32),
                      new MySqlParameter("@SpringRSAPriKey", MySqlDbType.VarChar,100),
                        new MySqlParameter("@SpringRSAPubKey", MySqlDbType.VarChar,100),
               new MySqlParameter("@SpringRSAID", MySqlDbType.Int32),
             new MySqlParameter("@OldAESKey", MySqlDbType.VarChar,100)};

            parameters[0].Value = model.APIU_Name;
            parameters[1].Value = model.AESKey;
            parameters[2].Value = model.RSAPubKey;
            parameters[3].Value = model.RSA2PubKey;
            parameters[4].Value = model.IP_whitelist;
            parameters[5].Value = model.Add_Time;
            parameters[6].Value = model.Mark_Stop;
            parameters[7].Value = model.Stop_Time;
            parameters[8].Value = model.ORG_ID;
            parameters[9].Value = model.APIU_ID;
            parameters[10].Value = model.RSAPubID;
            parameters[11].Value = model.RSA2PubID;
            parameters[12].Value = model.SpringRSAPriKey;
            parameters[13].Value = model.SpringRSAPubKey;
            parameters[14].Value = model.SpringRSAID;
            parameters[15].Value = OldAESKey;
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
            catch(Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 更新用户RSA公钥
        /// </summary>
        /// <param name="model"></param>
        /// <param name="RSAPubKeyChnage"></param>
        /// <returns></returns>
        public bool UpdateUserRSA(string APIU_ID, string RSAPubKey)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append($@"update  apiuserrsalist a, apiuser b set a.ExpiresDate = date_add(now(), interval {ExpiresDate()} day)
where a.APIU_ID = b.APIU_ID and a.RSAPubID = b.RSAPubID and a.RType = 'RSA' and a.APIU_ID = @APIU_ID;");
            int RSAPubID = 0;
            if (!string.IsNullOrEmpty(RSAPubKey))
            {
                DbHelper.GetSysIdBase("APIUSERRSAID", out RSAPubID);
                strSql.Append("insert into apiuserrsalist(APIU_ID,RSAPubID,RType,RSAPubKey) values (@APIU_ID,@RSAPubID,'RSA',@RSAPubKey) ;");
            }
            strSql.Append("update apiuser set ");
            strSql.Append("RSAPubKey=@RSAPubKey,");
            strSql.Append("RSAPubID=@RSAPubID");
            strSql.Append(" where APIU_ID=@APIU_ID ;");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@RSAPubKey", MySqlDbType.VarChar,500),
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@RSAPubID", MySqlDbType.Int32)
                  };

            parameters[0].Value = RSAPubKey;
            parameters[1].Value = APIU_ID;
            parameters[2].Value = RSAPubID;

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
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string APIU_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from apiuser ");
            strSql.Append(" where APIU_ID=@APIU_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,10)          };
            parameters[0].Value = APIU_ID;

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
        public apiuser GetModel(string APIU_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select APIU_ID,APIU_Name,AESKey,RSAPubKey,RSA2PubKey,IP_whitelist,Add_Time,Mark_Stop,Stop_Time,ORG_ID ,RSAPubID,RSA2PubID,SpringRSAPriKey,SpringRSAPubKey,SpringRSAID ,AType from apiuser ");
            strSql.Append(" where APIU_ID=@APIU_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,10)          };
            parameters[0].Value = APIU_ID;

            apiuser model = new apiuser();
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
        /// 获取当前未过期RSA等Key
        /// </summary>
        /// <returns></returns>
        public DataTable GetApiUserRsalistUsed()
        {
            string strSql = "select  * from  apiuserrsalist where IFNULL(ExpiresDate ,CURDATE()) >= CURDATE();";
            DataTable dt = DbHelperMySQLMySpring.Query(strSql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获取当前未过期RSA等Key
        /// </summary>
        /// <param name="APIU_ID"></param>
        /// <returns></returns>
        public DataTable GetApiUserRsalistUsed(string APIU_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  * from  apiuserrsalist where IFNULL(ExpiresDate ,CURDATE()) >= CURDATE() ");
            strSql.Append(" and  APIU_ID=@APIU_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APIU_ID", MySqlDbType.VarChar,10)          };
            parameters[0].Value = APIU_ID;

            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters).Tables[0];
            return dt;
        }

        public List<SAPIUserInfo> GetSAPIUserInfoUsed()
        {
            List<SAPIUserInfo> list = new List<SAPIUserInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select APIU_ID,APIU_Name,AESKey,RSAPubKey,RSA2PubKey,IP_whitelist,Add_Time,Mark_Stop,Stop_Time,ORG_ID ,RSAPubID,RSA2PubID,SpringRSAPriKey,SpringRSAPubKey,SpringRSAID ,AType  from apiuser ");
            strSql.Append(" where Mark_Stop =0  and AType=1");
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new SAPIUserInfo(DataRowToModel(dr)));
                }
            }
            return list;
        }
        /// <summary>
        /// WebSocket动态秘钥用户
        /// </summary>
        /// <returns></returns>
        public List<SAPIUserInfo> GetSAPIUserInfoWebSocketUsed()
        {
            List<SAPIUserInfo> list = new List<SAPIUserInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select APIU_ID,APIU_Name,AESKey,RSAPubKey,RSA2PubKey,IP_whitelist,Add_Time,Mark_Stop,Stop_Time,ORG_ID ,RSAPubID,RSA2PubID,SpringRSAPriKey,SpringRSAPubKey,SpringRSAID ,AType from apiuser ");
            strSql.Append(" where Mark_Stop =0  and AType=2");
            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new SAPIUserInfo(DataRowToModel(dr)));
                }
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public apiuser DataRowToModel(DataRow row)
        {
            apiuser model = new apiuser();
            if (row != null)
            {
                if (row["APIU_ID"] != null)
                {
                    model.APIU_ID = row["APIU_ID"].ToString();
                }
                if (row["APIU_Name"] != null)
                {
                    model.APIU_Name = row["APIU_Name"].ToString();
                }
                if (row["AESKey"] != null)
                {
                    model.AESKey = row["AESKey"].ToString();
                }
                if (row["RSAPubKey"] != null)
                {
                    model.RSAPubKey = row["RSAPubKey"].ToString();
                }
                if (row["RSA2PubKey"] != null)
                {
                    model.RSA2PubKey = row["RSA2PubKey"].ToString();
                }
                if (row["IP_whitelist"] != null)
                {
                    model.IP_whitelist = row["IP_whitelist"].ToString();
                }
                if (row["Add_Time"] != null && row["Add_Time"].ToString() != "")
                {
                    model.Add_Time = DateTime.Parse(row["Add_Time"].ToString());
                }
                if (row["Mark_Stop"] != null && row["Mark_Stop"].ToString() != "")
                {
                    if ((row["Mark_Stop"].ToString() == "1") || (row["Mark_Stop"].ToString().ToLower() == "true"))
                    {
                        model.Mark_Stop = true;
                    }
                    else
                    {
                        model.Mark_Stop = false;
                    }
                }
                if (row["Stop_Time"] != null && row["Stop_Time"].ToString() != "")
                {
                    model.Stop_Time = DateTime.Parse(row["Stop_Time"].ToString());
                }
                if (row["ORG_ID"] != null)
                {
                    model.ORG_ID = row["ORG_ID"].ToString();
                }

                if (row["RSAPubID"] != null)
                {
                    model.RSAPubID = int.Parse(row["RSAPubID"].ToString());
                }
                if (row["RSA2PubID"] != null)
                {
                    model.RSA2PubID = int.Parse(row["RSA2PubID"].ToString());
                }
                if (row["SpringRSAID"] != null)
                {
                    model.SpringRSAID = int.Parse(row["SpringRSAID"].ToString());
                }
                if (row["SpringRSAPriKey"] != null)
                {
                    model.SpringRSAPriKey = row["SpringRSAPriKey"].ToString();
                }
                if (row["SpringRSAPubKey"] != null)
                {
                    model.SpringRSAPubKey = row["SpringRSAPubKey"].ToString();
                }
                if (row.Table.Columns.Contains("AType") &&row["AType"] != null)
                {
                    model.AType = int.Parse(row["AType"].ToString());
                }
                else
                {
                    model.AType = 1;
                }
            }
            return model;
       }
        public   DataTable APIUserTbGet(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select  apiuser.*,b.org_name from  apiuser 
LEFT JOIN
sorganization b
on apiuser.ORG_ID=b.ORG_ID");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            
            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取指定系统授权单位下的用户
        /// </summary>
        /// <param name="SYSID"></param>
        /// <returns></returns>
        public DataTable SystemUserList(int SYSID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select a.APIU_ID, a.APIU_Name, b.ORG_ID, b.org_name, c.SYSID

 from APIUser a
join sorganization b on a.ORG_ID= b.ORG_ID and a.Mark_Stop= 0
join SorgandSys c on b.ORG_ID= c.ORG_ID
where c.SYSID = {0} and a.Mark_Stop= 0 and b.Mark_Stop= 0", SYSID));

            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取指定用户组下的用户
        /// </summary>
        /// <param name="SYSID"></param>
        /// <returns></returns>
        public DataTable GroupUserList(int APIG_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select  a.APIU_ID,a.APIU_Name,b.ORG_ID,b.org_name,c.APIG_ID 

 from APIUser a 
join sorganization b on a.ORG_ID=b.ORG_ID and a.Mark_Stop=0
join  apiuserandgroup c on a.APIU_ID=c.APIU_ID
where c.APIG_ID ={0} and a.Mark_Stop=0 and   b.Mark_Stop=0", APIG_ID));

            return DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 获取指定用户组下的用户
        /// </summary>
        /// <param name="APIG_ID"></param>
        /// <returns></returns>
        public List<string > GroupUserList2(int APIG_ID)
        {

            List<string> list = new List<string>();
            StringBuilder strSql = new StringBuilder();

            strSql.Append(string.Format(@" 
select DISTINCT a.APIU_ID FROM
apiuser a 
join apiuserandgroup
b on a.APIU_ID=b.APIU_ID
where b.APIG_ID ={0} and a.Mark_Stop= 0", APIG_ID));

            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add( dr["APIU_ID"].ToString());
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定用户组下的用户
        /// </summary>
        /// <param name="APIG_ID"></param>
        /// <returns></returns>
        public List<string> GroupUserList2(List<int> listGroup)
        {
            if (listGroup == null || listGroup.Count == 0)
                return new List<string>();

         
            string ListGroup = "";
            foreach (int G_ID in listGroup)
            {
                ListGroup = ListGroup + G_ID.ToString() + ",";
            }
            ListGroup= ListGroup.TrimEnd(',');
            List<string> list = new List<string>();
            StringBuilder strSql = new StringBuilder();

            strSql.Append(string.Format(@" 
select DISTINCT a.APIU_ID FROM
apiuser a 
join apiuserandgroup
b on a.APIU_ID=b.APIU_ID
where b.APIG_ID  in({0})and a.Mark_Stop= 0", ListGroup));

            DataTable dt = DbHelperMySQLMySpring.Query(strSql.ToString()).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr["APIU_ID"].ToString());
                }
            }
            return list;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select APIU_ID,APIU_Name,AESKey,RSAPubKey,RSA2PubKey,IP_whitelist,Add_Time,Mark_Stop,Stop_Time,ORG_ID ,RSAPubID,RSA2PubID,SpringRSAPriKey,SpringRSAPubKey,SpringRSAID,AType");
            strSql.Append(" FROM apiuser ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM apiuser ");
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
                strSql.Append("order by T.APIU_ID desc");
            }
            strSql.Append(")AS Row, T.*  from apiuser T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        
        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
 