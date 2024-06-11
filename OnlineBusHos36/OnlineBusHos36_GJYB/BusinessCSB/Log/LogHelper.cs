using Log.Core.MySQLDAL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBusHos36_GJYB
{
    public class LogHelper
    {
        /// <summary>
        /// 保存医保日志
        /// </summary>
        /// <param name="model"></param>
        public static void SaveInsurLog(insurlog model)
        {
            Task task = new Task(() =>
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into insurlog(");
                    strSql.Append("msgid,fixmedins_code,infno,InTime,InData,OutTime,OutData,OutCode,sign_no)");
                    strSql.Append(" values (");
                    strSql.Append("@msgid,@fixmedins_code,@infno,@InTime,@InData,@OutTime,@OutData,@OutCode,@sign_no)");
                    MySqlParameter[] parameters = {
                    new MySqlParameter("@msgid", MySqlDbType.VarChar,30),
                    new MySqlParameter("@fixmedins_code", MySqlDbType.VarChar,12),
                    new MySqlParameter("@infno", MySqlDbType.VarChar,6),
                    new MySqlParameter("@InTime", MySqlDbType.DateTime),
                    new MySqlParameter("@InData", MySqlDbType.Text),
                    new MySqlParameter("@OutTime", MySqlDbType.DateTime),
                    new MySqlParameter("@OutData", MySqlDbType.Text),
                    new MySqlParameter("@OutCode", MySqlDbType.VarChar,4),
                    new MySqlParameter("@sign_no", MySqlDbType.VarChar,30)};
                    parameters[0].Value = model.msgid;
                    parameters[1].Value = model.fixmedins_code;
                    parameters[2].Value = model.infno;
                    parameters[3].Value = model.InTime;
                    parameters[4].Value = model.InData;
                    parameters[5].Value = model.OutTime;
                    parameters[6].Value = model.OutData;
                    parameters[7].Value = model.OutCode;
                    parameters[8].Value = model.sign_no;

                    int rows = DbHelperMySQLInsurlog.ExecuteSql(strSql.ToString(), parameters);
                }
                catch (Exception ex)
                {
                    LogHelper.SaveLocalLog("SaveSqlerror", Newtonsoft.Json.JsonConvert.SerializeObject(model));
                }
            });
            task.Start();


        }

        /// <summary>
        /// 保存平台日志
        /// </summary>
        /// <param name="Bus_no"></param>
        /// <param name="inTime"></param>
        /// <param name="outTime"></param>
        /// <param name="HOS_ID"></param>
        /// <param name="opter_no"></param>
        /// <param name="injson"></param>
        /// <param name="outjson"></param>
        /// <param name="status"></param>
        public static void SavePlatLog(platlog model)
        {
            Task task = new Task(() =>
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into platlog(");
                    strSql.Append("msgid,HOS_ID,Infno,InTime,InData,OutTime,OutData,OutCode,Opter_no)");
                    strSql.Append(" values (");
                    strSql.Append("@msgid,@HOS_ID,@Infno,@InTime,@InData,@OutTime,@OutData,@OutCode,@Opter_no)");
                    MySqlParameter[] parameters = {
                    new MySqlParameter("@msgid", MySqlDbType.VarChar,30),
                    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@Infno", MySqlDbType.VarChar,30),
                    new MySqlParameter("@InTime", MySqlDbType.DateTime),
                    new MySqlParameter("@InData", MySqlDbType.Text),
                    new MySqlParameter("@OutTime", MySqlDbType.DateTime),
                    new MySqlParameter("@OutData", MySqlDbType.Text),
                    new MySqlParameter("@OutCode", MySqlDbType.VarChar,10),
                    new MySqlParameter("@Opter_no", MySqlDbType.VarChar,10)};
                    parameters[0].Value = model.msgid;
                    parameters[1].Value = model.HOS_ID;
                    parameters[2].Value = model.Infno;
                    parameters[3].Value = model.InTime;
                    parameters[4].Value = model.InData;
                    parameters[5].Value = model.OutTime;
                    parameters[6].Value = model.OutData;
                    parameters[7].Value = model.OutCode;
                    parameters[8].Value = model.Opter_no;

                    int rows = DbHelperMySQLInsurlog.ExecuteSql(strSql.ToString(), parameters);

                }
                catch (Exception ex)
                {
                    LogHelper.SaveLocalLog("SaveSqlerror", Newtonsoft.Json.JsonConvert.SerializeObject(model));
                }
            });
            task.Start();
        }

        /// <summary>
        /// 保存异常日志
        /// </summary>
        /// <param name="TYPE"></param>
        /// <param name="inTime"></param>
        /// <param name="ExceptionMessage"></param>
        public static void SaveSqlerror(sqlerrlog model)
        {
            Task task = new Task(() =>
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into sqlerrlog(");
                    strSql.Append("TYPE,inTime,ExceptionMessage)");
                    strSql.Append(" values (");
                    strSql.Append("@TYPE,@inTime,@ExceptionMessage)");
                    MySqlParameter[] parameters = {
                    new MySqlParameter("@TYPE", MySqlDbType.VarChar,100),
                    new MySqlParameter("@inTime", MySqlDbType.DateTime),
                    new MySqlParameter("@ExceptionMessage", MySqlDbType.Text)};
                    parameters[0].Value = model.TYPE;
                    parameters[1].Value = model.inTime;
                    parameters[2].Value = model.ExceptionMessage;

                    int rows = DbHelperMySQLInsurlog.ExecuteSql(strSql.ToString(), parameters);
                }
                catch (Exception ex)
                {
                    LogHelper.SaveLocalLog("SaveSqlerror", Newtonsoft.Json.JsonConvert.SerializeObject(model));
                }
            });
            task.Start();
        }


        public static void SaveLocalLog(string folderName, string logInfo)
        {
            string LogPath =Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
            try
            {
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                StreamWriter sr1 = new StreamWriter(Path.Combine(LogPath,"log" + DateTime.Now.ToString("yyyyMMdd") + ".log"), true);
                sr1.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "==>");
                sr1.WriteLine("输出：" + logInfo);
                sr1.WriteLine("--------------------------------------------------");//50个-
                sr1.Close();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
