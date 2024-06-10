using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{
    public class BusLogHelper
    {
        public static bool SaveLog(BusLogData busLogData)
        {
            if (string.IsNullOrEmpty(busLogData.ErrorData))//非错误日志
            {
                if (busLogData.busSstatus == BusinessInfoVersionStatus.Publish)
                {//非测试业务

                    return BusLogHelper.SavebusLog(busLogData);
                }
                else//测试业务
                {
                    return BusLogHelper.SavebusTestlog(busLogData);
                }
            }
            else//错误日志
            {
                return BusLogHelper.SavebusError(busLogData);
            }

        }

        public static Boolean SavebusLog(BusLogData busLogData)
        {

            try
            {
                string sqlcmd;
                if (busLogData.BLogSeparate)
                {
                    sqlcmd = @"INSERT INTO `log" + busLogData.sLBInfoHeadBusS.BusID + @"`  (BusID ,TID,Version,ExecuteTime,InTime,OutTime ,ClientID,SLBID,BusServerID,InData,OutData,Header,ResultStatus)
values
(@BusID ,@TID,@Version,@ExecuteTime,@InTime,@OutTime ,@ClientID,@SLBID,@BusServerID,@InData,@OutData,@Header,@ResultStatus)";
                }
                else
                {
                    sqlcmd = @"INSERT INTO `buslog`  (BusID,TID,Version,ExecuteTime,InTime,OutTime ,ClientID,SLBID,BusServerID,InData,OutData,Header,ResultStatus)
values
(@BusID,@TID,@Version,@ExecuteTime,@InTime,@OutTime ,@ClientID,@SLBID,@BusServerID,@InData,@OutData,@Header,@ResultStatus)";
                }
                MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID",busLogData.sLBInfoHeadBusS.BusID ),
                     new MySqlParameter("@TID",busLogData.TID  ),
                    new MySqlParameter("@Version",busLogData.version ),
                    new MySqlParameter("@ExecuteTime", busLogData.ExecuteTime),
                    new MySqlParameter("@InTime",busLogData.InTime),
                    new MySqlParameter("@OutTime",busLogData.OutTime),
                    new MySqlParameter("@ClientID",busLogData.sLBInfoHeadBusS.ClientID),
                    new MySqlParameter("@SLBID",busLogData.sLBInfoHeadBusS.SLBID),
                    new MySqlParameter("@BusServerID",busLogData.sLBInfoHeadBusS.BusServerID),
                    new MySqlParameter("@InData",busLogData.Indata),
                    new MySqlParameter("@OutData",busLogData.OutData),
                    new MySqlParameter("@Header",busLogData.sLBInfoHeadBusS.ToString()),
                    new MySqlParameter("@ResultStatus",busLogData.ResultStatus )
                                                };
                DbHelperMySQLMySpringLog.ExecuteSql(sqlcmd, parameters);
            }
            catch (Exception ex)
            {
                PasSLog.Error("BusLogHelper.SavebusLog", ex.ToString());
                return false;
            }
            return true;
        }
        public static Boolean SavebusTestlog(BusLogData busLogData)
        {
            try
            {
                string sqlcmd = @"INSERT INTO `busTestlog`  (BusID,TID,Version,VersionStatus,ExecuteTime,InTime,OutTime ,ClientID,SLBID,BusServerID,InData,OutData,Header,ResultStatus)
values
(@BusID,@TID,@Version,@VersionStatus,@ExecuteTime,@InTime,@OutTime ,@ClientID,@SLBID,@BusServerID,@InData,@OutData,@Header,@ResultStatus)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID",busLogData.sLBInfoHeadBusS.BusID ),
                    new MySqlParameter("@TID",busLogData.TID  ),
                    new MySqlParameter("@Version",busLogData.version ),
                    new MySqlParameter("@VersionStatus", busLogData.busSstatus),
                    new MySqlParameter("@ExecuteTime", busLogData.ExecuteTime),
                    new MySqlParameter("@InTime",busLogData.InTime),
                    new MySqlParameter("@OutTime",busLogData.OutTime),
                    new MySqlParameter("@ClientID",busLogData.sLBInfoHeadBusS.ClientID),
                    new MySqlParameter("@SLBID",busLogData.sLBInfoHeadBusS.SLBID),
                    new MySqlParameter("@BusServerID",busLogData.sLBInfoHeadBusS.BusServerID),
                    new MySqlParameter("@InData",busLogData.Indata),
                    new MySqlParameter("@OutData",busLogData.OutData),
                    new MySqlParameter("@Header",busLogData.sLBInfoHeadBusS.ToString()),
                       new MySqlParameter("@Header",busLogData.sLBInfoHeadBusS.ToString()) ,
                       new MySqlParameter("@ResultStatus",busLogData.ResultStatus)
                                                };
                DbHelperMySQLMySpringLog.ExecuteSql(sqlcmd, parameters);
            }
            catch (Exception ex)
            {
                PasSLog.Error("BusLogHelper.SavebusTestlog", ex.ToString());
            }
            try
            {
                DbHelper.BusInfoTestCountSet(busLogData.sLBInfoHeadBusS.BusID, busLogData.version, busLogData.busSstatus);
            }
            catch (Exception ex)
            {
                PasSLog.Error("BusLogHelper.SavebusTestlog", ex.ToString());
                return false;
            }
            return true;
        }
        public static Boolean SavebusError(BusLogData busLogData)
        {
            try
            {
                string sqlcmd = @"INSERT INTO `buserrorlog`  (BusID,TID,Version,VersionStatus,ExecuteTime,InTime,OutTime ,ClientID,SLBID,BusServerID,InData,OutData,Header,ErrorData)
values
(@BusID,@TID,@Version,@VersionStatus,@ExecuteTime,@InTime,@OutTime ,@ClientID,@SLBID,@BusServerID,@InData,@OutData,@Header,@ErrorData)";

                MySqlParameter[] parameters = {
                    new MySqlParameter("@BusID",busLogData.sLBInfoHeadBusS.BusID ),
                    new MySqlParameter("@TID",busLogData.TID  ),
                    new MySqlParameter("@Version",busLogData.version ),
                    new MySqlParameter("@VersionStatus", busLogData.busSstatus),
                    new MySqlParameter("@ExecuteTime", busLogData.ExecuteTime),
                    new MySqlParameter("@InTime",busLogData.InTime),
                    new MySqlParameter("@OutTime",busLogData.OutTime),
                    new MySqlParameter("@ClientID",busLogData.sLBInfoHeadBusS.ClientID),
                    new MySqlParameter("@SLBID",busLogData.sLBInfoHeadBusS.SLBID),
                    new MySqlParameter("@BusServerID",busLogData.sLBInfoHeadBusS.BusServerID),
                    new MySqlParameter("@InData",busLogData.Indata),
                    new MySqlParameter("@OutData",busLogData.OutData),
                    new MySqlParameter("@Header",busLogData.sLBInfoHeadBusS.ToString()),
                       new MySqlParameter("@ErrorData",busLogData.ErrorData)
                                                };
                DbHelperMySQLMySpringLog.ExecuteSql(sqlcmd, parameters);
            }
            catch (Exception ex)
            {
                PasSLog.Error("BusLogHelper.SavebusError", ex.ToString());
                return false;
            }
            return true;
        }


    }
}
