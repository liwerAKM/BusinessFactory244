using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 公共参数
    /// </summary>
    public class MyPubConstant
    {
        /// <summary>
        /// 触发BusServer重启的BusServer自身占用线程数。
        /// </summary>
        public static string CurrentOrgID
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("CurrentOrgID");
                }
                catch (Exception ex)
                {
                    return "-1";
                }
            }
        }
        /// <summary>
        /// 获取MySQL连接字符串
        /// </summary>
        public static string ConnectionStringMySpringMySQL
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionStringMySpringMySQL"];
                if (ConStringEncrypt)
                {
                    _connectionString = DecryptMysqlConfigPwd(_connectionString);
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 获取MySQLCache连接字符串
        /// </summary>
        public static string ConnectionStringCacheMySQL
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionStringCacheMySQL"];
                if (ConStringEncrypt)
                {
                    _connectionString = DecryptMysqlConfigPwd(_connectionString);
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 获取基础数据更新log连接字符串
        /// </summary>
        public static string ConnectionStringCacheLogMySQL
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionStringCacheLogMySQL"];
                if (ConStringEncrypt)
                {
                    _connectionString = DecryptMysqlConfigPwd(_connectionString);
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 获取SQLServer连接字符串
        /// </summary>
        public static string ConnectionStringCacheSQLServer
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionStringCacheSQLServer"];
                if (ConStringEncrypt)
                {
                    _connectionString = DecryptMysqlConfigPwd(_connectionString);
                }
                return _connectionString;
            }
        }

        /// <summary>
        /// 获取SQLServer连接字符串
        /// </summary>
        public static string ConnectionStringEBPP
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionStringEBPP"];
                if (ConStringEncrypt)
                {
                    _connectionString = DecryptMysqlConfigPwd(_connectionString);
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 获取SQLServer连接字符串
        /// </summary>
        public static string ConnectionStringlogEBPP
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionStringlogEBPP"];
                if (ConStringEncrypt)
                {
                    _connectionString = DecryptMysqlConfigPwd(_connectionString);
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 字符串是否加密
        /// </summary>
        public static bool ConStringEncrypt
        {
            get
            {
                string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                if (string.Compare(ConStringEncrypt, "true", true) == 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 将MySQL配置文件参数的密码Password 从密码改为明文
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        static string DecryptMysqlConfigPwd(string connectionString)
        {
            string _connectionString = connectionString;
            try
            {
                string[] cons = _connectionString.Split(';');
                foreach (string con in cons)
                {
                    string tcon = con.Trim();
                    if (tcon.StartsWith("password", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string[] tcons = tcon.Split('=');
                        string pwd = tcons[1];
                        pwd = pwd.Trim('\'');
                        string pwdmw = DESEncrypt.Decrypt(pwd);
                        _connectionString = _connectionString.Replace(pwd, pwdmw);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _connectionString;
        }

        public static string ConnectionStringMySpringLogMySQL
        {
            get
            {
                string _connectionString = ConfigurationManager.AppSettings["ConnectionStringMySpringLogMySQL"];
                if (ConStringEncrypt)
                {
                    _connectionString = DecryptMysqlConfigPwd(_connectionString);
                }
                return _connectionString;
            }
        }
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;
            }
        }
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        protected Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// 当前SLBID
        /// </summary>
        public static string SLBID
        {
            get
            {
                return ConfigurationManager.AppSettings["SLBID"];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string OMSMainOrgID
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["OMSMainOrgID"];
                }
                catch (Exception ex)
                {
                    return "0";
                }
            }
        }

        /// <summary>
        /// 当前ZZJID
        /// </summary>
        public static string ZZJID
        {
            get
            {
                return ConfigurationManager.AppSettings["ZZJID"];
            }
        }
        /// <summary>
        ///是否加载本地lBusinessInfo配置文件
        /// </summary>
        public static bool  LoadLocalBusinessInfoConfigInfo
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["LoadLocalBusinessInfoConfigInfo"].ToLower().Equals("true");
                }
                catch
                {
                    return false;
                }
            }
        }
        
        /// <summary>
        ///  -是否以Windows服务运行
        /// </summary>
        public static bool RunAsServices
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["RunAsServices"].ToLower().Equals("true");

                }
                catch
                {
                    return false;
                }
            }
        }
        /// <summary>
        ///连接中断后重试连次数
        /// </summary>
        /// <returns></returns>
        public static int RetryConnectCount()
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings["RetryConnectCount"]);
            }
            catch (Exception ex)
            {
                return 10;
            }
        }

        public const string BusIdSLBAPIACFCheck = "SLBAPIACFCheck";
        /// <summary>
        /// 是否通过HttpSpringAPI进行内部函数操作(前置在外地的BusServer不能通过数据库直接访问)
        /// </summary>
        public static bool UseHttpSpringAPI
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["UseHttpSpringAPI"].ToLower().Equals("true");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        /// <summary>
        ///  HTTPSpring服务地址
        /// </summary>
        public static string HttpServerURL
        {
            get
            {
                return ConfigurationManager.AppSettings["HttpServerURL"];
            }
        }

        /// <summary>
        ///当前 BusServerID
        /// </summary>
        public static string BusServerID
        {
            get
            {
                return ConfigurationManager.AppSettings["BusServerID"];
            }
        }
        /// <summary>
        ///是否是监控服务，对于监控服务,系统会对<see cref="AccessServer"/>、<see cref = "ServerLoadBalancing" /> 等采取重连策略, 以最大可能保证能连接到各系统
        /// </summary>
        /// <returns></returns>
        public static bool IsMonitorService()
        {
            try
            {
                return ConfigurationManager.AppSettings["IsMonitorService"].ToLower().Equals("true");
            }
            catch (Exception ex)
            {
                return false;
            }
            ;
        }
        /// <summary>
        ///SLB作为在医院的客户端连接公司
        /// </summary>
        /// <returns></returns>
        public static bool SLBisHosClient
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SLBisHosClient"].ToLower().Equals("true");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 医院的SLB作为客户端连接公司，公司的SLBWebSocket地址
        /// </summary>
        public static string ClientSLBWebSocketUrl
        {
            get
            {
                try
                {
                    // return   redisHelper.HGet("GSLBSInfo","GSLBSServer.IP");
                    return DbHelper.SysInfoGet("ClientSLBWebSocketUrl");
                }
                catch
                {
                    return "";
                }
            }
        }
        /// <summary>
        ///   的SLBWebSocket地址
        /// </summary>
        public static string WebSocketSLBUrl
        {
            get
            {
                try
                {
                    // return   redisHelper.HGet("GSLBSInfo","GSLBSServer.IP");
                    return ConfigurationManager.AppSettings["WebSocketSLBUrl"];
                }
                catch
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 医院的SLB作为客户端连接公司，医院ID
        /// </summary>
        public static string ClientSLBHosID
        {
            get
            {
                try
                {
                    // return   redisHelper.HGet("GSLBSInfo","GSLBSServer.IP");
                    return DbHelper.SysInfoGet("ClientSLBHosID");
                }
                catch
                {
                    return "";
                }
            }
        }
        public static int LOG_LEVENL = 3;
        /// <summary>
        ///   HTTP 监听端口
        /// </summary>
        public static int HttpPort
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["HttpPort"]);
                }
                catch (Exception ex)
                {
                    return 8905;
                }
            }
        }
        /// <summary>
        ///   HTTP 监听的本地IP
        /// </summary>
        public static string HttpLocalIP
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["HttpLocalIP"];
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }
        static string operationMaintenanceID = "";
        /// <summary>
        ///当前 OperationMaintenanceID
        /// </summary>
        public static string OperationMaintenanceID
        {
            get
            {
                if (string.IsNullOrEmpty(operationMaintenanceID))
                {
                    return ConfigurationManager.AppSettings["OperationMaintenanceID"];
                }
                else
                {
                    return operationMaintenanceID;
                }
            }
            set
            {
                operationMaintenanceID = value;
            }
        }
      static   ServerDDLBS _serverDDLBS = ServerDDLBS.NULL;
        /// <summary>
        ///  服务器负载均衡策略种类,整个系统只允许一种
        /// </summary>
        public static ServerDDLBS serverDDLBS
        {
            get
            {
                if (_serverDDLBS == ServerDDLBS.NULL)
                {
                    try
                    {
                        //_serverDDLBS = (ServerDDLBS)int.Parse(redisHelper.HGet("GSLBSInfo", "ServerDDLBS"));
                        _serverDDLBS = (ServerDDLBS)int.Parse(DbHelper.SysInfoGet("ServerDDLBS"));
                    }
                    catch
                    {
                        _serverDDLBS = ServerDDLBS.WeightMinimumLoadRule;
                    }
                }
                return _serverDDLBS;
            }
            set
            {
                _serverDDLBS = value;
                //  redisHelper.HSet("GSLBSInfo", "ServerDDLBS", value.ToString());
                DbHelper.SysInfoSet("ServerDDLBS", value.ToString());
            }
        }
        static GSLBSSModel _GSLBSS_Model = GSLBSSModel.NULL;
        /// <summary>
        /// 全局负载均衡策略GSLBS模式
        /// </summary>
        public static GSLBSSModel GSLBSS_Model
        {
            get
            {
                if (_GSLBSS_Model == GSLBSSModel.NULL)
                {
                    try
                    {
                        //  _GSLBSS_Model =(GSLBSSModel) int.Parse(redisHelper.HGet("GSLBSInfo", "GSLBSSModel"));
                        _GSLBSS_Model = (GSLBSSModel)int.Parse(DbHelper.SysInfoGet("GSLBSSModel"));
                    }
                    catch (Exception ex)
                    {
                        _GSLBSS_Model = GSLBSSModel.Self_government;
                    }
                }
                return _GSLBSS_Model;
            }
            set
            {
                _GSLBSS_Model = value;
            }
        }
        /// <summary>
        /// 全局负载均衡策略GSLBS中央组件 GSLBSServerIP 
        /// </summary>
        public static string GSLBSServer_IP
        {
            get
            {
                try
                {
                    // return   redisHelper.HGet("GSLBSInfo","GSLBSServer.IP");
                    return DbHelper.SysInfoGet("GSLBSServer.IP");
                }
                catch
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 全局负载均衡策略GSLBS中央组件 GSLBSServer端口
        /// </summary>
        public static int GSLBSServer_Port
        {
            get
            {
                try
                {
                    // return int.Parse(redisHelper.HGet("GSLBSInfo", "GSLBSServer.Port"));
                    return int.Parse(DbHelper.SysInfoGet("GSLBSServer.Port"));
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// 开启BusServer服务异常
        /// </summary>
        public static bool TurnOnBusServerException
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("TurnOnBusServerException").ToLower().Equals("true");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 是否记录耗时业务;
        /// </summary>
        public static bool RecordSlowExecuteBus
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("RecordSlowExecuteBus").ToLower().Equals("true");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///耗时业务时长记录点
        /// </summary>
        public static int LongExecuteTime
        {
            get
            {
                try
                {
                    return int.Parse(SpringAPI.SysInfoGet("LongExecuteTime"));
                }
                catch (Exception ex)
                {
                    return 1000;
                }
            }
        }

        /// <summary>
        ///耗时业务日志保存方式
        /// </summary>
        public static int SlowExecuteOutput
        {
            get
            {
                try
                {
                    return int.Parse(DbHelper.SysInfoGet("SlowExecuteOutput"));
                }
                catch (Exception ex)
                {
                    return 1;
                }

            }
        }
        /// <summary>
        ///业务数据采用压缩阀值(字节)，当业务数据Bydy大于此长度时进行压缩(系统消息不压缩)
        /// </summary>
        public static int ZipBufferSizeThreshold
        {
            get
            {
                try
                {
                    return int.Parse(DbHelper.SysInfoGet("ZipBufferSizeThreshold"));
                }
                catch (Exception ex)
                {
                    return 2000;
                }

            }
        }

        /// <summary>
        /// 是否开启SLBHttp服务
        /// </summary>
        public static bool OpenSLBHttpServer
        {
            get
            {
                try
                {

                    return DbHelper.SysInfoGet("OpenSLBHttpServer").ToLower().Equals("true");
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// 是否开启SLBWebSocket服务
        /// </summary>
        public static bool OpenSLBWebSocket
        {
            get
            {
                try
                {

                    return DbHelper.SysInfoGet("OpenSLBWebSocket").ToLower().Equals("true");
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// SLB Http服务端口是否开启非加密接口MyspringClear
        /// </summary>
        public static bool SLBHttpMyspringClear
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("SLBHttpMyspringClear").ToLower().Equals("true");

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// 是否开启API用户鉴权控制系统
        /// </summary>
        public static bool OpenAPIACF
        {
            get
            {
                try
                {
                    return SpringAPI.SysInfoGet("OpenAPIACF").ToLower().Equals("true");

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// TCP客户端是否开启API用户鉴权控制系统
        /// </summary>
        public static bool OpenAPIACF_TCP
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("OpenAPIACFTCP").ToLower().Equals("true");

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// HTTP客户端是否开启API用户鉴权控制系统
        /// </summary>
        public static bool OpenAPIACF_HTTP
        {
            get
            {
                try
                {
                    return SpringAPI.SysInfoGet("OpenAPIACFHTTP").ToLower().Equals("true");

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// HTTP客户端是否开启API用户鉴权控制系统
        /// </summary>
        public static Version OMCMinVersion
        {
            get
            {
                try
                {
                    string sOMCMinVersion = SpringAPI.SysInfoGet("OMCMinVersion");
                    Version version = Version.Parse(sOMCMinVersion);
                    return version;
                }
                catch (Exception ex)
                {
                    return Version.Parse("1.0.0.0"); ;
                }

            }
        }


        /// <summary>
        /// 平台用RSA私钥
        /// </summary>
        public static string RSAPrivateKey
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("RSAPrivateKey");

                }
                catch (Exception ex)
                {
                    return "";
                }

            }
        }

        /// <summary>
        /// 平台用RSA公钥
        /// </summary>
        public static string RSAPublicKey
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("RSAPublicKey");

                }
                catch (Exception ex)
                {
                    return "";
                }

            }
        }



        /// <summary>
        /// 即将弃用的RSA私钥，当RSA更换时，请将更换前的私钥先设置在此，为空时表示暂未弃用或者弃用的已经下架
        /// </summary>
        public static string RSAPriDeprecated
        {
            get
            {
                try
                {
                    return DbHelper.SysInfoGet("RSAPriDeprecated");

                }
                catch (Exception ex)
                {
                    return "";
                }

            }
        }



        /// <summary>
        ///  签名类别 0:不签名；1:MD5 ; 2:RSA ;3:RSA2  (在非0 情况下与EncryptType对应)
        /// </summary>
        public static int SignType
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["SignType"]);

                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
        }
        /// <summary>
        ///  加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public static int EncryptType
        {
            get
            {
                try
                {

                    return int.Parse(ConfigurationManager.AppSettings["EncryptType"]);
                }
                catch (Exception ex)
                {
                    return 0;
                }

            }
        }


        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID
        /// </summary>
        public static string SEID
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SEID"];

                }
                catch
                {
                    return "";
                }

            }
        }
         
        /// <summary>
        /// 为普通客户端提供的 WebSocket 监听端口
        /// </summary>
        public static int WebSocketForClientPort
        {
            get
            {
                try
                {

                    return int.Parse(ConfigurationManager.AppSettings["WebSocketPort"]);
                }
                catch (Exception ex)
                {
                    return 8920;
                }

            }
        }

       /// <summary>
          ///   HTTP 监听的本地端口
          /// </summary>
        public static int HttpLocalPort
        {
            get
            {
                try
                {

                    return int.Parse(ConfigurationManager.AppSettings["HttpLocalPort"]);
                }
                catch (Exception ex)
                {
                    return 5000;
                }

            }
        }
        /// <summary>
        /// 此BusServer 是否单点承接服务。当开启单点服务时，BusServer 不连接SLB，独立开启Http 及WebSocket 服务，相关鉴权登录等都独自完成。
        /// </summary>
        public static bool BusSingleService
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["BusSingleService"].ToLower().Equals("true");

                }
                catch
                {
                    return false;
                }

            }
        }
        /// <summary>
        ///   OMCWebSocketUrl 如果本地有则优先取本地
        /// </summary>
        public static string OMCWebSocketUrl
        {
            get
            {
                try
                {

                    return ConfigurationManager.AppSettings["OMCWebSocketUrl"];
                }
                catch (Exception ex)
                {
                    return "";
                }

            }
        }
        /// <summary>
        ///   SLBBusSocketUrl 如果本地有则优先取本地
        /// </summary>
        public static string SLBBusSocketUrl
        {
            get
            {
                try
                {

                    return ConfigurationManager.AppSettings["SLBBusSocketUrl"];
                }
                catch (Exception ex)
                {
                    return "";
                }

            }
        }

        /// <summary>
        /// 本地库模式 1 MYSQL ;2 SQLite ;3 无库纯内存  ;其他 不启用
        /// </summary>
        public static int LocalDataModel
        {
            get
            {
                try
                {
                    return int.Parse(ConfigurationManager.AppSettings["LocalDataModel"]);

                }
                catch
                {
                    return 0;
                }

            }
        }
        /// <summary>
        /// 触发SLB重启的SLB自身占用线程数。
        /// </summary>
        public static int SLBReStartThreads
        {
            get
            {
                try
                {
                    return int.Parse(DbHelper.SysInfoGet("SLBReStartThreads"));
                }
                catch (Exception ex)
                {
                    return 300;
                }

            }
        }
        /// <summary>
        /// 触发BusServer重启的BusServer自身占用线程数。
        /// </summary>
        public static int BusServerReStartThreads
        {
            get
            {
                try
                {
                    return int.Parse(DbHelper.SysInfoGet("BusServerReStartThreads"));
                }
                catch (Exception ex)
                {
                    return 300;
                }
            }
        }

        public static DataTable EnumToDataTable(Type enumType, string key, string val)
        {
            string[] Names = System.Enum.GetNames(enumType);

            Array Values = System.Enum.GetValues(enumType);

            DataTable table = new DataTable();
            table.Columns.Add(key, System.Type.GetType("System.String"));
            table.Columns.Add(val, System.Type.GetType("System.Int32"));
            table.Columns[key].Unique = true;
            for (int i = 0; i < Values.Length; i++)
            {
                DataRow DR = table.NewRow();
                DR[key] = Names[i].ToString();
                DR[val] = (int)Values.GetValue(i);
                table.Rows.Add(DR);
            }
            return table;
        }
    }
}
