using PasS.Base.Lib.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{
    /// <summary>
    /// SLBInf信息
    /// </summary>
    public class SLBInfo
    {
        public SLBInfo()
        {
        }
        public SLBInfo(SLBInfo sLBInfo)
        {
            this.SLBID = sLBInfo.SLBID;
            this.SLBName = sLBInfo.SLBName;
            this.IP = sLBInfo.IP;
            this.Port = sLBInfo.Port;
            this.Status = sLBInfo.Status;
            this.IP2 = sLBInfo.IP2;
            this.Port2 = sLBInfo.Port2;
            this.IP3 = sLBInfo.IP3;
            this.Port3 = sLBInfo.Port3;
            this.Http = sLBInfo.Http;
            this.HttpPort = sLBInfo.HttpPort;
            this.WebSocketPort = sLBInfo.WebSocketPort;
            this.WSPath = sLBInfo.WSPath;
            this.GroupID = sLBInfo.GroupID;
            this.OrgID = sLBInfo.OrgID;
        }
        /// <summary>
        /// 负载均衡ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        ///  负载均衡名称
        /// </summary>
        public string SLBName { get; set; }

        public string IP { get; set; }
        public int Port { get; set; }

        /// <summary>
        /// 1 上线 ；0 下线 ;-1 异常
        /// </summary>
        public Int16 Status { get; set; }

        public string IP2 { get; set; }
        public int Port2 { get; set; }


        public string IP3 { get; set; }
        public int Port3 { get; set; }
        /// <summary>
        /// 是否使用HTTP监听
        /// </summary>
        public bool Http { get; set; }
        public int HttpPort { get; set; }

        /// <summary>
        /// 提供给BusServer调用的WebSocketPort
        /// </summary>
        public int WebSocketPort { get; set; }
        /// <summary>
        ///提供给BusServer调用WebSocketServer http服务中转路径
        /// </summary>
        public string WSPath { get; set; }
        /// <summary>
        ///分组ID
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        ///机构ID
        /// </summary>
        public string  OrgID { get; set; }

    }

    public class SSysRegisterInfoClientSLB
    {
        public string ClientIP { get; set; }
        public string OrgID { get; set; }


    }

    /// <summary>
    /// 系统消息结构
    /// </summary>
    public class SSysInfo
    {
        public SSysInfo()
        {
        }
        public SSysInfo(SysInfoType sysInfoType)
        {
            InfoType = sysInfoType;
        }

        /// <summary>
        /// 设置业务内容 并加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetInfoEncrypt<T>(T t)
        {
            string Infot = JsonConvert.SerializeObject(t);
            if (Infot.Length > 2000)
            {
                Infot = GZipHelper.CompressToBase64(Infot);
                this.Zip = 1;
            }
            if (ET == 0)
            {
                ET = 1;
            }
            Info = AESHelper.EncryptDefault(Infot);
        }
        /// <summary>
        ///  加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void EncryptInfo()
        {
            if (ET == 0 && !string.IsNullOrEmpty(Info))
            {
                ET = 1;
                Info = AESHelper.EncryptDefault(Info);
            }
        }
        /// <summary>
        /// 设置业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetInfo<T>(T t)
        {
            Info = JsonConvert.SerializeObject(t);
            if (Info.Length > 2000)
            {
                Info = GZipHelper.CompressToBase64(Info);
                this.Zip = 1;
            }
        }
        /// <summary>
        /// 获取业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public T GetInfo<T>()
        {
            if (string.IsNullOrEmpty(Info))
            {
                return default(T);
            }

            if (ET == 1)
            {
                string infot = AESHelper.DecryptDefault(Info);
                if (this.Zip == 1)
                {
                    infot = GZipHelper.DecompressBase64(infot);
                }
                return JsonConvert.DeserializeObject<T>(infot);
            }
            else
            {
                if (this.Zip == 1)
                {
                    string infot = GZipHelper.DecompressBase64(Info);
                    return JsonConvert.DeserializeObject<T>(infot);
                }
                else
                    return JsonConvert.DeserializeObject<T>(Info);
            }
        }

        public string Get_Info()
        {
            if (ET == 1)
            {
                string infot = AESHelper.DecryptDefault(Info);
                if (this.Zip == 1)
                {
                    infot = GZipHelper.DecompressBase64(infot);
                }
                return infot;
            }
            else
            {
                if (this.Zip == 1)
                {
                    return GZipHelper.DecompressBase64(Info);
                }
                else
                    return Info;
            }
        }

        /// <summary>
        ///压缩 0 未压缩；1 GZipStream 压缩；  先压缩再加密，先解密再解压
        /// </summary>
        public int Zip { get; set; }

        /// <summary>
        /// 操作类别
        /// </summary>
        public SysInfoType InfoType { get; set; }
        /// <summary>
        /// 消息内容 根据Typeid定义
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES 
        /// </summary>
        public int ET { get; set; }
        ///// <summary>
        ///// 加密或签名的对应系统配置的密钥或公钥ID
        ///// </summary>
        //public string SEID { get; set; }

    }

  
    /// <summary>
    /// 获取指定BusID 对应的服务ID 消息结构体
    /// </summary>
    public class SGetBusServer
    {
        public string ID { get; set; }
        /// <summary>
        /// 1Get 2 Free
        /// </summary>
        public int GetorFree { get; set; }
        public string BusID { get; set; }
        /// <summary>
        /// 独占并发标记，有此标记的整个系统中只允许一个并发
        /// </summary>
        public string UniqueConcurrentFlag { get; set; }
        /// <summary>
        /// 1 成功； 0 业务已满请等待 ;-1 没有对应业务服务或服务器
        /// </summary>
        public int Ret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BusServerID { get; set; }


    }
    /// <summary>
    ///系统注册消息体
    /// </summary>
    public class SSysRegisterInfo
    {

        public SSysRegisterInfo(RegisterIdentity registerIdentity, string ID)
        {
            Register_Identity = registerIdentity;
            this.ID = ID;
        }


        public string ID;
        /// <summary>
        /// 注册标记
        /// </summary>
        public RegisterIdentity Register_Identity;


        /// <summary>
        /// 设置业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetInfo<T>(T t)
        {
            Info = JsonConvert.SerializeObject(t);
        }
        /// <summary>
        /// 获取业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public T GetInfo<T>()
        {
            if (string.IsNullOrEmpty(Info))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(Info);
        }


        /// <summary>
        /// 消息内容 根据RegisterIdentity定义
        /// </summary>
        public string Info;


        /// <summary>
        /// 签名类别 0:不签名；1:MD5 ; 2:RSA ;3:RSA2  (在非0 情况下与EncryptType对应)
        /// </summary>
        public int SignType { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int EncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID
        /// </summary>
        public string SEID { get; set; }

        
    }

    /// <summary>
    /// 注册身份
    /// </summary>
    public enum RegisterIdentity
    {
        ZZJClient = 1,
        BusServer = 2,
        BusServerCallOthter = 3,
        ServerLoadBalancing = 4,
        GSLBServer = 5,
        AccessServer = 6,
        OperationMaintenance = 7,
        ClientSLB = 8,
        WebManage = 9

    }

    /// <summary>
    ///注册信息返回结果
    /// </summary>
    public class SSysRegisterRetInfo
    {
        public SSysRegisterRetInfo()
        {

        }

        public SSysRegisterRetInfo(string ServerID, string ClientID, string ClientIP, int ClientPort)
        {

            this.ServerID = ServerID;
            this.ClientID = ClientID;
            this.ClientIP = ClientIP;
            this.ClientPort = ClientPort;
            ReslutCode = 1;
        }
        public SSysRegisterRetInfo(string ServerID, string ClientID, string ClientIP, int ClientPort, string SessionID)
        {

            this.ServerID = ServerID;
            this.ClientID = ClientID;
            this.ClientIP = ClientIP;
            this.ClientPort = ClientPort;
            this.SessionID = SessionID;
            ReslutCode = 1;
        }

        /// <summary>
        /// WebSocketClient在服务端的SessionID
        /// </summary>
        public string SessionID { get; set; }

        public string ServerID { get; set; }
        public string ClientID { get; set; }
        public string ClientIP { get; set; }
        public int ClientPort { get; set; }

        /// <summary>
        /// Token 可根据系统需要返回Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 返回结果标识 1成功
        /// </summary>
        public int ReslutCode { get; set; }
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string ResultMessage { get; set; }
        public RegisterIdentity Register_Identity { get; set; }

    }
    /// <summary>
    /// 客户端<see cref="SLBClient"/> 向<see cref="AccessServer"/> 请求可以负载均衡SLB消息体
    /// </summary>
    public class SClientAccessServerGetSLB
    {
        /// <summary>
        ///  0 首次获取 1 只获取备用 此时MainSLBInfo要包含主SLBInfo
        /// </summary>
        /// <param name="Get_Type"></param>
        public SClientAccessServerGetSLB(int Get_Type)
        {
            GetType = Get_Type;
        }
        /// <summary>
        /// 0 首次获取 1 只获取备用 此时MainSLBInfo要包含主SLBInfo
        /// </summary>
        public int GetType;
        public SLBInfo MainSLBInfo;

    }

    public class SClientAccessServerGetSLBRet
    {
        /// <summary>
        /// 返回结果标识 1成功
        /// </summary>
        public int ReslutCode { get; set; }
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        ///获取备用连接时 返回备用连接是否比主连接空闲
        /// </summary>
        public bool MainIsBusy { get; set; }
        public SLBInfo sLBInfo;
        /// <summary>
        /// 备用连接
        /// </summary>
        public SClientAccessServerGetSLBRet StandbySLB;
    }

    /// <summary>
    /// 处理任务转发的消息体
    /// </summary>
    public class SockTCPAssignInfoPT
    {

        public SockTCPAssignInfoPT()
        {
        }
        public SockTCPAssignInfoPT(SLBBusinessInfo sockTCPAssignInfoc)
        {
            this.BusID = sockTCPAssignInfoc.BusID;
            this.buffer = sockTCPAssignInfoc.BusData;
            this.TID = sockTCPAssignInfoc.TID;
        }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string BusID { get; set; }

        /// <summary>
        /// ClientID
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// 请求唯一ID
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// PerformTasksID
        /// </summary>
        public string PerformTasksID { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string buffer { get; set; }
    }


    /// <summary>
    ///  Business状态
    /// </summary>
    public enum BusinessStatus
    {

        /// <summary>
        ///  静默状态，表示当前不进行任务的处理工作。
        /// </summary>
        SILENCE = 0,
        /// <summary>
        /// 表示机器已经加入整个作业的分配中。
        /// </summary>
        WORKING = 1,
        /// <summary>
        /// 表示机器已经失去联系。且与任何机器都没有连接。
        /// </summary>
        OFFLINE = 2
    }


    /// <summary>
    /// 跟踪条件
    /// </summary>
    public class STrackConditions
    {
        /// <summary>
        /// 此跟踪唯一ID
        /// </summary>
        public string TCID { get; set; }

        /// <summary>
        ///  <see cref="ServerLoadBalancing"/>运行端唯一ID
        /// </summary>
        public string SLBID { get; set; }

        /// <summary>
        /// BusServerID
        /// </summary>

        public string BusServerID { get; set; }
        /// <summary>
        /// BusID
        /// </summary>

        public string BusID { get; set; }
        /// <summary>
        /// 客户端ID (IP:Port)
        /// </summary>
        public string ClientID { get; set; }
        /// <summary>
        /// 客户端灰度测试标记
        /// </summary>
        public string CTag { get; set; }
        /// <summary>
        /// 跟踪内容
        /// </summary>
        public TrackInfoType InfoType { get; set; }


        /// <summary>
        /// 0 停止 1开始
        /// </summary>
        public int Status { get; set; }
    }
    /// <summary>
    /// 跟踪内容类型
    /// </summary>
    public enum TrackInfoType
    {
        /// <summary>
        /// 出人数量
        /// </summary>
        Count,
        /// <summary>
        /// 入参
        /// </summary>
        InData,
        /// <summary>
        /// 出参
        /// </summary>
        OutData,
        /// <summary>
        /// 入参和出参
        /// </summary>
        InOutData,
        /// <summary>
        /// 可修改入参
        /// </summary>
        InDatamodification,
        /// <summary>
        /// 可修改出参
        /// </summary>
        OutDatamodification
    }
    /// <summary>
    /// 跟踪结果
    /// </summary>
    public class STrackResult
    {
        public STrackResult()
        {
            FTime = DateTime.Now;
        }
        public STrackResult(SLBInfoHeadBusS sLBInfoHeadBus)
        {
            FTime = DateTime.Now;
            BusID = sLBInfoHeadBus.BusID;
            ClientID = sLBInfoHeadBus.ClientID;
            SLBID = sLBInfoHeadBus.SLBID;
            BusServerID = sLBInfoHeadBus.BusServerID;
            TID = sLBInfoHeadBus.TID;

        }


        /// <summary>
        /// 此跟踪唯一ID
        /// </summary>
        public string TCID;
        /// <summary>
        ///  <see cref="ServerLoadBalancing"/>运行端唯一ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        ///  <see cref="ServerLoadBalancing"/>运行端唯一ID
        /// </summary>
        public string BusServerID;
        public string BusID;
        /// <summary>
        /// 客户端ID (IP:Port)
        /// </summary>
        public string ClientID;
        /// <summary>
        /// 客户端灰度测试标记
        /// </summary>
        public string CTag;

        public int InCount;

        public int OutCount;
        /// <summary>
        /// 此业务唯一ID
        /// </summary>
        public string TID;
        /// <summary>
        /// 入参
        /// </summary>
        public byte[] InData;
        /// <summary>
        /// 出参
        /// </summary>
        public byte[] OutData;
        /// <summary>
        ///压缩 0 未压缩；1 GZipStream 压缩；
        /// </summary>
        public int InZip { get; set; }
        /// <summary>
        ///压缩 0 未压缩；1 GZipStream 压缩；
        /// </summary>
        public int OutZip { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int InEncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID
        /// </summary>
        public string InSEID { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int OutEncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID
        /// </summary>
        public string OutSEID { get; set; }
        /// <summary>
        /// 首次记录时间
        /// </summary>
        public DateTime FTime;
        /// <summary>
        /// 获取出参时间
        /// </summary>
        public DateTime OutTime;


    }
    /// <summary>
    /// 业务执行日志 BusServiceAdapter中生成
    /// </summary>
    public class BusLogData
    {
        public BusLogData()
        {
        }
        public BusLogData(SLBInfoHeadBusS sLBInfoHeadBusS, string Indata, DateTime InTime, string OutData, DateTime OutTime, int LogType, bool BLogSeparate,
            BusinessInfoVersionStatus busSstatus, string version)
        {
            this.sLBInfoHeadBusS = sLBInfoHeadBusS;
            this.busSstatus = busSstatus;
            this.version = version;
            this.Indata = Indata;
            this.OutData = OutData;
            this.InTime = InTime;
            this.OutTime = OutTime;
            this.LogType = LogType;
            this.BLogSeparate = BLogSeparate;
            ResultStatus = 1;

        }
        public BusLogData(SLBInfoHeadBusS sLBInfoHeadBusS, string Indata, DateTime InTime, string OutData, DateTime OutTime, int LogType, bool BLogSeparate,
        string ErrorData)
        {
            this.sLBInfoHeadBusS = sLBInfoHeadBusS;
            this.busSstatus = BusinessInfoVersionStatus.Publish;
            this.version = "0.0.0.0";
            this.Indata = Indata;
            this.OutData = OutData;
            this.InTime = InTime;
            this.OutTime = OutTime;
            this.LogType = LogType;
            this.BLogSeparate = BLogSeparate;
            this.ErrorData = ErrorData;
            ResultStatus = 1;
        }
        public SLBInfoHeadBusS sLBInfoHeadBusS;
        public BusinessInfoVersionStatus busSstatus;
        public string version;
        public string Indata;
        public string OutData;
        public DateTime InTime;
        public DateTime OutTime;
        public string ErrorData;
        /// <summary>
        /// 执行时长(毫秒)
        /// </summary>
        public Int32 ExecuteTime;
        /// <summary>
        /// 日志类别0	只记录异常
        ///1	记录入参
        ///2	记录出参
        ///3	记录入参和出参
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// 业务日志单独处理
        /// </summary>
        public bool BLogSeparate { get; set; }

        /// <summary>
        ///  当此BusTID TID非空时优先存储BusTID,否则存储SLBInfoHeadBusS.TID
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        ///结果状态 sucess = 1,
        /// BusinessAbnormal = 0,
        /// BusinessError = -1,
        /// SysError = -2
        /// </summary>
        public int ResultStatus { get; set; }

    }

    /// <summary>
    /// 支付等异步通知类参数
    /// </summary>
    public class SNotifyPortal
    {
        public string HttpMethod;
        /// <summary>
        /// 子URL
        /// </summary>
        public string NotifyURL;
        /// <summary>
        ///入参字符串UTF8不做UrlDecode
        /// </summary>
        public string Param;

        /// <summary>
        ///Headers
        /// </summary>
        public Dictionary<string, string> Headers;

        public void SetHeaders(NameValueCollection RequestHeaders)
        {
            Headers = new Dictionary<string, string>();
            foreach (string key in RequestHeaders)
            {
                Headers.Add(key, RequestHeaders[key]);
            }
        }
    }
    /// <summary>
    /// 支付等异步通知类参数
    /// </summary>
    public class SNotifyPortalResult
    {
        public string ContentType;

        /// <summary>
        ///直接返回字符串
        /// </summary>
        public string Param;
    }



    /// <summary>
    ///API用户用于Spring内存
    /// </summary>
    public class SAPIUserInfo
    {

        public SAPIUserInfo()

        {
        }
        public SAPIUserInfo(apiuser api_user)
        {
            this.APIU_ID = api_user.APIU_ID;
            this.APIU_Name = api_user.APIU_Name;
            this.AESKey = api_user.AESKey;
            this.RSAPubKey = api_user.RSAPubKey;
            this.RSA2PubKey = api_user.RSA2PubKey;
            this.IP_whitelist = api_user.IP_whitelist;
            this.Mark_Stop = api_user.Mark_Stop;
            this.RSA2PubID = api_user.RSA2PubID;
            this.RSAPubID = api_user.RSAPubID;
            this.SpringRSAID = api_user.SpringRSAID;
            this.SpringRSAPriKey = api_user.SpringRSAPriKey;
            this.SpringRSAPubKey = api_user.SpringRSAPubKey;
            this.AType = api_user.AType;
        }

        /// <summary>
        ///  
        /// </summary>
        public string APIU_ID;
        /// <summary>
        ///  
        /// </summary>
        public string APIU_Name;
        /// <summary>
        ///  
        /// </summary>
        public string AESKey;
        /// <summary>
        /// 用户RSA公钥
        /// </summary>
        public string RSAPubKey;
        public int RSAPubID;

        ///// <summary>
        ///// 用户将停用的RSA公钥，在设置新的公钥到RSAPubKey 后，RSAPubKeyDeprecated将保持之前的公钥一段时间
        ///// </summary>
        //public string RSAPubKeyDeprecated;



        /// <summary>
        /// 平台RSA私钥 如果为空则用平台系统配置的统一RSA私钥
        /// </summary>
        public string SpringRSAPriKey;
        ///// <summary>
        /////  平台将停用的RSA私钥钥，在设置新的私钥到SpringRSAPriKey 后，SpringRSAPriKeyDeprecated将保持之前的私钥一段时间
        ///// </summary>
        //public string SpringRSAPriKeyDeprecated;
        /// <summary>
        /// 平台RSA公钥
        /// </summary>
        public string SpringRSAPubKey;
        /// <summary>
        /// 平台RSAID
        /// </summary>
        public int SpringRSAID;
        /// <summary>
        ///  平台将停用的RSA公钥，在设置新的公钥到RSAPubKey 后，RSAPubKeyDeprecated将保持之前的公钥一段时间
        /// </summary>
      //  public string SpringRSAPubKeyDeprecated;



        /// <summary>
        /// 用户RSA2公钥
        /// </summary>
        public string RSA2PubKey;
        public int RSA2PubID;

        public string IP_whitelist;

        public bool Mark_Stop;

        /// <summary>
        /// 用户RSA公钥变化
        /// </summary>
        public bool RSAPubKeyChnage = false;
        /// <summary>
        /// 用户RSA2公钥变化
        /// </summary>
        public bool RSA2PubKeyChnage = false;
        /// <summary>
        ///平台RSA公钥变化
        /// </summary>
        public bool SpringRSAChnage = false;

        /// <summary>
        ///平台RSA2公钥变化
        /// </summary>
        public bool SpringRSA2Chnage = false;

        /// <summary>
        ///AES Key 变化
        /// </summary>
        public bool AESChnage = false;

        /// <summary>
        /// 用户类型:1 默认用户；2 WebSocke动态秘钥初始化用户
        /// </summary>
        public int AType = 1;


    }

    /// <summary>
    ///API用户用于Spring内存 (包含过期Key)
    /// </summary>
    public class SAPIUserInfoCKey
    {

        public SAPIUserInfoCKey()

        {
        }
        public SAPIUserInfoCKey(SAPIUserInfo api_user)
        {
            this.APIU_ID = api_user.APIU_ID;
            this.APIU_Name = api_user.APIU_Name;
            this.AESKey = api_user.AESKey;
            this.RSAPubKey = api_user.RSAPubKey;
            this.RSA2PubKey = api_user.RSA2PubKey;
            this.IP_whitelist = api_user.IP_whitelist;
            this.Mark_Stop = api_user.Mark_Stop;
            this.RSA2PubID = api_user.RSA2PubID;
            this.RSAPubID = api_user.RSAPubID;
            this.SpringRSAID = api_user.SpringRSAID;
            this.SpringRSAPriKey = api_user.SpringRSAPriKey;
            this.SpringRSAPubKey = api_user.SpringRSAPubKey;
        }

        public void Update(SAPIUserInfo api_user)
        {

            this.APIU_Name = api_user.APIU_Name;
            this.AESKey = api_user.AESKey;
            this.RSAPubKey = api_user.RSAPubKey;
            this.RSA2PubKey = api_user.RSA2PubKey;
            this.IP_whitelist = api_user.IP_whitelist;
            this.Mark_Stop = api_user.Mark_Stop;
            this.RSA2PubID = api_user.RSA2PubID;
            this.RSAPubID = api_user.RSAPubID;
            this.SpringRSAID = api_user.SpringRSAID;
            this.SpringRSAPriKey = api_user.SpringRSAPriKey;
            this.SpringRSAPubKey = api_user.SpringRSAPubKey;
        }
        public SAPIUserInfoCKey(apiuser api_user)
        {
            this.APIU_ID = api_user.APIU_ID;
            this.APIU_Name = api_user.APIU_Name;
            this.AESKey = api_user.AESKey;
            this.RSAPubKey = api_user.RSAPubKey;
            this.RSA2PubKey = api_user.RSA2PubKey;
            this.IP_whitelist = api_user.IP_whitelist;
            this.Mark_Stop = api_user.Mark_Stop;
            this.RSA2PubID = api_user.RSA2PubID;
            this.RSAPubID = api_user.RSAPubID;
            this.SpringRSAID = api_user.SpringRSAID;
            this.SpringRSAPriKey = api_user.SpringRSAPriKey;
            this.SpringRSAPubKey = api_user.SpringRSAPubKey;
        }

        /// <summary>
        ///  
        /// </summary>
        public string APIU_ID;
        /// <summary>
        ///  
        /// </summary>
        public string APIU_Name;
        /// <summary>
        ///  
        /// </summary>
        public string AESKey;
        /// <summary>
        /// 用户RSA公钥 验签 加密
        /// </summary>
        public string RSAPubKey;
        public int RSAPubID;

        ///// <summary>
        ///// 用户将停用的RSA公钥，在设置新的公钥到RSAPubKey 后，RSAPubKeyDeprecated将保持之前的公钥一段时间
        ///// </summary>
        //public string RSAPubKeyDeprecated;



        /// <summary>
        /// 平台RSA私钥 如果为空则用平台系统配置的统一RSA私钥
        /// </summary>
        public string SpringRSAPriKey;
        ///// <summary>
        /////  平台将停用的RSA私钥钥，在设置新的私钥到SpringRSAPriKey 后，SpringRSAPriKeyDeprecated将保持之前的私钥一段时间
        ///// </summary>
        //public string SpringRSAPriKeyDeprecated;
        /// <summary>
        /// 平台RSA公钥
        /// </summary>
        public string SpringRSAPubKey;
        /// <summary>
        /// 平台RSAID
        /// </summary>
        public int SpringRSAID;
        /// <summary>
        ///  平台将停用的RSA公钥，在设置新的公钥到RSAPubKey 后，RSAPubKeyDeprecated将保持之前的公钥一段时间
        /// </summary>
      //  public string SpringRSAPubKeyDeprecated;



        /// <summary>
        /// 用户RSA2公钥
        /// </summary>
        public string RSA2PubKey;
        public int RSA2PubID;

        public string IP_whitelist;

        public bool Mark_Stop;


        /// <summary>
        ///AESKey列表：key RSAID
        /// </summary>
        public Dictionary<int, string> AESList = new Dictionary<int, string>();

        /// <summary>
        ///用户RSA公钥 key RSAID
        /// </summary>
        public Dictionary<int, string> UserRsaList = new Dictionary<int, string>();
        /// <summary>
        ///用户RSA2公钥 key RSAID
        /// </summary>
        public Dictionary<int, string> UserRsa2List = new Dictionary<int, string>();

        /// <summary>
        ///平台RSA私钥 key RSAID 
        /// </summary>
        public Dictionary<int, string> SpringRsaList = new Dictionary<int, string>();

        /// <summary>
        ///平台RSA2私钥 key RSAID
        /// </summary>
        public Dictionary<int, string> SpringRsa2List = new Dictionary<int, string>();

    }

    /// <summary>
    /// API信息 用于Spring内存
    /// </summary>
    public class SAPIList
    {
        public SAPIList()
        {
        }

        public SAPIList(Sapilist sapilist)
        {
            this.API_ID = sapilist.API_ID;
            this.API_Name = sapilist.API_Name;
            this.isBUS_ID = sapilist.isBUS_ID;
            this.Mark_Stop = sapilist.Mark_Stop;
        }


        public int API_ID;
        public string API_Name;
        public bool isBUS_ID;
        public bool Mark_Stop;
    }
    /// <summary>
    /// API用户增加或者重新加载访问API权限
    /// </summary>
    public class SUpdateAPIUserSaddAPI
    {

        public SUpdateAPIUserSaddAPI()
        {
            UserList = new List<string>();
            APIList = new List<int>();
        }
        public List<string> UserList;
        /// <summary>
        /// 1增加 ；2 重新加载； 3 移除
        /// </summary>
        public int Type;
        public List<int> APIList;
    }

    /// <summary>
    /// 
    /// </summary>
    public class SSLBAPIACFCheckIn
    {
        /// <summary>
        /// API用户ID 老的SEID
        /// </summary>
        public string AUID;
        /// <summary>
        /// API_Name
        /// </summary>
        public string API_Name;

    }
    /// <summary>
    /// 
    /// </summary>
    public class SSLBAPIACFCheckOut
    {
        /// <summary>
        /// 是否通过验证
        /// </summary>
        public bool Pass;


    }


    public class STokenData
    {
        public STokenData()
        {

        }
        public STokenData(string Token)
        {
            this.Token = Token;
        }
        public string Token { get; set; }
        public string Param { get; set; }

        /// <summary>
        /// 设置业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetParam<T>(T t)
        {
            Param = JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 获取业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public T GetParam<T>()
        {
            if (string.IsNullOrEmpty(Param))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(Param);
        }
    }

    /// <summary>
    /// ping 主机或地址结构
    /// </summary>
    public class SPinghost
    {
        /// <summary>
        ///  主机名或地址
        /// </summary>
        public string HostNameOrAddress;
        /// <summary>
        ///  主机说明
        /// </summary>
        public string HostNote;

        /// <summary>
        /// 通知隐藏主机地址
        /// </summary>
        public bool HideHost;
        /// <summary>
        ///  排查说明
        /// </summary>
        public string TroubleShoot;
        /// <summary>
        /// 0未检测 1 正常  -1 异常
        /// </summary>
        public int Status;

    }

    /// <summary>
    /// 检测端口服务 结构
    /// </summary>
    public class SDetectionPort
    {
        /// <summary>
        ///  主机名或地址
        /// </summary>
        public string HostNameOrAddress;
        /// <summary>
        ///  端口
        /// </summary>
        public int Port;
        /// <summary>
        /// 端口服务说明
        /// </summary>
        public string ServerNote;
        /// <summary>
        ///  排查说明
        /// </summary>
        public string TroubleShoot;
        /// <summary>
        /// 0未检测 1 正常  -1 异常
        /// </summary>
        public int Status;

    }
    /// <summary>
    /// 检测服务 结构
    /// </summary>
    public class STestConnectRet
    {
        /// <summary>
        /// 被检查ID
        /// </summary>
        public string CheckServerID;
        /// <summary>
        ///  检查类别 1 SLB To Busserver
        /// </summary>
        public int ChekcType;
        /// <summary>
        /// 检查ID
        /// </summary>
        public string LookerID;


    }

    /// <summary>
    /// BusServer 缓存WebSocket关系数据
    /// </summary>
    public class SWebSocketInfo
    {
        /// <summary>
        /// 系统中与之关联的ID
        /// </summary>
        public string UserID;
        /// <summary>
        ///WebSocket内部GUID
        /// </summary>
        public string SessionID;

        /// <summary>
        /// IP:Port
        /// </summary>
        public string SLBIPPort;
    }
    /// <summary>
    ///WebSocket登录验证返回必须字段
    /// </summary>
    public class SWebSocketLgoinRetData
    {
        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool Pass;
        /// <summary>
        /// 系统中与之关联的ID
        /// </summary>
        public string UserID;
        /// <summary>
        /// 系统中与之关联的API授权APIU_ID
        /// </summary>
        public string APIU_ID;
    }
    /// <summary>
    ///WebSocket 获取动态秘钥参数 请求参数
    /// </summary>
    public class SpringWebSocketGetKeyDataReq
    {
        /// <summary>
        /// RSA客户端公钥(可空，空则不支持该加密)
        /// </summary>
        public string RSAPublicKey;

        /// <summary>
        /// RSA2 客户端公钥(可空，空则不支持该加密)
        /// </summary>
        public string RSA2PublicKey;

        /// <summary>
        /// APIU_ID
        /// </summary>
        public string APIU_ID;

        /// <summary>
        /// 连接websocket 的URL地址
        /// </summary>
        public string WebSUrl;
    }
    /// <summary>
    ///WebSocket 获取动态秘钥参数返回参数
    /// </summary>
    public class SpringWebSocketGetKeyDataResp
    {
        /// <summary>
        /// RSA客户端公钥(可空，空则不支持该加密)
        /// </summary>
        public string AESKey;
        /// <summary>
        /// RSA客户端公钥(可空，空则不支持该加密)
        /// </summary>
        public string RSAPublicKey;

        /// <summary>
        /// RSA2 客户端公钥(可空，空则不支持该加密)
        /// </summary>
        public string RSA2PublicKey;

    }
    public class ResultStore
    {
        public ResultStore(Int32 CcN, OMCandZZJInfoHead sLbInfoHead, byte[] b_Info)
        {
            CCN = CcN;
            sLBInfoHead = sLbInfoHead;
            bInfo = b_Info;
        }
        public Int32 CCN;
        public OMCandZZJInfoHead sLBInfoHead;
        public byte[] bInfo;
    }
    /// <summary>
    /// spring Http JSON入参结构
    /// </summary>
    public class SpringHttpData
    {
        public SpringHttpData()
        {
        }
        public SpringHttpData(SpringWebSocketData springWeb)
        {
            this.BusID = springWeb.BusID;
            this.CTag = springWeb.BusID;
            this.Param = springWeb.BusID;
            this.ParamType = springWeb.ParamType;
            this.sign = springWeb.sign;
            this.SubBusID = springWeb.SubBusID;
            this.TID = springWeb.TID;
            this.user_id = springWeb.user_id;


        }

        /// <summary>
        /// 入参
        /// </summary>
        public string Param;
        /// <summary>
        /// user_ID
        /// </summary>
        public string user_id;
        /// <summary>
        /// 签名
        /// </summary>
        public string sign;


        /// <summary>
        /// 1 入参为业务数据AES加密 2为RSA加密,3为 为RSA2加密 ;12入参为<see cref="SLBBusinessInfo"/>,13入参为业务明文(此模式只限制公司内部网络使用)
        /// </summary>
        public int ParamType;

        public string BusID;

        /// <summary>
        /// 子业务ID （根据实际业务定义，可空） SUB BUSINESS ID 此ID只参与具体的业务，与Spring无直接关系
        /// </summary>
        public string SubBusID { get; set; }

        /// <summary>
        /// 请求唯一ID 不重复  非必填
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 客户端标记 标记用于灰度发布 非必填
        /// </summary>
        public string CTag { get; set; }

        /// <summary>
        /// 客户端标记 是否启用压缩 0 未压缩 1 压缩 
        /// </summary>
        public int Gzip { get; set; }
    }

    /// <summary>
    /// 业务消息
    /// </summary>
    public class SpringHttpDataOut
    {
        /// <summary>
        /// 请求唯一ID 不重复
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        /// 返回结果标识 1成功 成功时 sign，Param才有值
        /// </summary>
        public int ReslutCode { get; set; }
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign;

        /// <summary>
        /// 返回具体业务内容
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 客户端标记 是否启用压缩 0 未压缩 1 压缩 
        /// </summary>
        public int Gzip { get; set; }

    }

    /// <summary>
    /// spring WebSocket JSON入参结构
    /// </summary>
    public class SpringWebSocketData
    {
        /// <summary>
        /// 入参
        /// </summary>
        public string Param;
        /// <summary>
        /// user_ID
        /// </summary>
        public string user_id;
        /// <summary>
        /// 签名
        /// </summary>
        public string sign;


        /// <summary>
        /// 1 入参为业务数据AES加密 2为RSA加密,3为 为RSA2加密 ;12入参为<see cref="SLBBusinessInfo"/>,13入参为业务明文(此模式只限制公司内部网络使用)
        /// </summary>
        public int ParamType;

        public string BusID;

        /// <summary>
        /// 子业务ID （根据实际业务定义，可空） SUB BUSINESS ID 此ID只参与具体的业务，与Spring无直接关系
        /// </summary>
        public string SubBusID { get; set; }

        /// <summary>
        /// 请求唯一ID 不重复  非必填
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 客户端标记 标记用于灰度发布 非必填
        /// </summary>
        public string CTag { get; set; }
    }

    /// <summary>
    /// spring WebSocket JSON入参结构
    /// </summary>
    public class BusServerWebSocketData
    {
        /// <summary>
        /// 入参
        /// </summary>
        public string Param;

        /// <summary>
        ///  
        /// </summary>
        public string BusID;

        /// <summary>
        /// 签名
        /// </summary>
        public string sign;


        /// <summary>
        /// 1 入参为业务数据AES加密 2为RSA加密,3为 为RSA2加密 ;12入参为<see cref="SLBBusinessInfo"/>,13入参为业务明文(此模式只限制公司内部网络使用)
        /// </summary>
        public int ParamType;



        /// <summary>
        /// 请求唯一ID 不重复  非必填
        /// </summary>
        public string TID { get; set; }

    }
    /// <summary>
    /// MySQL主从关系中 主机名和IP对应关系
    /// </summary>
    public class BusServerMySqlRelicationHsotMatchup
    {
        public BusServerMySqlRelicationHsotMatchup()
        {
        }
        public BusServerMySqlRelicationHsotMatchup(string _BusServerID, MySqlRelicationHsotMatchup hsotMatchup)
        {
            BusServerID = _BusServerID;
            list = hsotMatchup.list;
        }
        public string BusServerID;
        public List<HsotMatchup> list;
    }
    /// <summary>
    /// MySQL主从关系中 主机名和IP对应关系
    /// </summary>
    public class MySqlRelicationHsotMatchup
    {
        public List<HsotMatchup> list;
    }

    /// <summary>
    /// MySQL主从关系中 主机名和IP对应关系
    /// </summary>
    public class HsotMatchup
    {
        public string HostName;
        public string HostIP;
    }

    /// <summary>
    ///通知更新数据
    /// </summary>
    public class NotifyUpdate
    {
        public NotifyUpdate()
        {
        }

        List<NotifyUpdateMX> lsUpdata = new List<NotifyUpdateMX>();

        public void AddUpdata(NotifyUpdateMX notifyUpdate)
        {
            lsUpdata.Add(notifyUpdate);
            SetInfo(lsUpdata);
        }



        /// <summary>
        /// 设置业务内容 并加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetInfoEncrypt<T>(T t)
        {
            string Infot = JsonConvert.SerializeObject(t);
            if (Infot.Length > 2000)
            {
                Infot = GZipHelper.CompressToBase64(Infot);
                this.Zip = 1;
            }
            if (ET == 0)
            {
                ET = 1;
            }
            Info = AESHelper.EncryptDefault(Infot);
        }
        /// <summary>
        ///  加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void EncryptInfo()
        {
            if (ET == 0 && !string.IsNullOrEmpty(Info))
            {
                ET = 1;
                Info = AESHelper.EncryptDefault(Info);
            }
        }
        /// <summary>
        /// 设置业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetInfo<T>(T t)
        {
            Info = JsonConvert.SerializeObject(t);
            if (Info.Length > 2000)
            {
                Info = GZipHelper.CompressToBase64(Info);
                this.Zip = 1;
            }
        }
        /// <summary>
        /// 获取业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public T GetInfo<T>()
        {
            if (string.IsNullOrEmpty(Info))
            {
                return default(T);
            }

            if (ET == 1)
            {
                string infot = AESHelper.DecryptDefault(Info);
                if (this.Zip == 1)
                {
                    infot = GZipHelper.DecompressBase64(infot);
                }
                return JsonConvert.DeserializeObject<T>(infot);
            }
            else
            {
                if (this.Zip == 1)
                {
                    string infot = GZipHelper.DecompressBase64(Info);
                    return JsonConvert.DeserializeObject<T>(infot);
                }
                else
                    return JsonConvert.DeserializeObject<T>(Info);
            }
        }

        public string Get_Info()
        {
            if (ET == 1)
            {
                string infot = AESHelper.DecryptDefault(Info);
                if (this.Zip == 1)
                {
                    infot = GZipHelper.DecompressBase64(infot);
                }
                return infot;
            }
            else
            {
                if (this.Zip == 1)
                {
                    return GZipHelper.DecompressBase64(Info);
                }
                else
                    return Info;
            }
        }





        /// <summary>
        /// 消息内容 根据Typeid定义
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        ///此 字段系统自动赋值， 压缩 0 未压缩；1 GZipStream 压缩；  先压缩再加密，先解密再解压
        /// </summary>
        public int Zip { get; set; }


        /// <summary>
        ///此 字段系统自动赋值， 加密类别 0:不加密；1:AES 
        /// </summary>
        public int ET { get; set; }

    }
    /// <summary>
    /// 单条数据
    /// </summary>
    public class NotifyUpdateMX
    {
        public NotifyUpdateMX()
        {

        }
        public NotifyUpdateMX(string Table_Name, NotifyUpdateType Update_Type, string In_fo, DateTime Update_Time)
        {
            TableName = TableName;
            UpdateType = Update_Type;
            UpdateTime = Update_Time;
            Info = In_fo;
        }
        /// <summary>
        /// Json 格式数据 可以是DataTable 或表对应实体 或实体List 
        /// </summary>
        /// <typeparam name="T">Json 格式数据</typeparam>
        /// <param name="t"></param>
        public void SetInfo<T>(T t)
        {
            Info = JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 数据(表)对应ID或名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 更新类别
        /// </summary>
        public NotifyUpdateType UpdateType { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 消息内容 根据Typeid定义
        /// </summary>
        public string Info { get; set; }
    }
    /// <summary>
    /// 更新类别
    /// </summary>
    public enum NotifyUpdateType
    {
        Add = 1,
        Edit = 2,
        Del = 3
    }
    /// <summary>
    /// 单条数据
    /// </summary>
    public class NotifyUpdateInfoMX
    {
        public NotifyUpdateInfoMX()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Update_Type"></param>
        /// <param name="In_fo">Json 格式数据，需要是List 或者DataTable</param>
        public NotifyUpdateInfoMX(NotifyUpdateType Update_Type, string In_fo)
        {
            UpdateType = Update_Type;
            Info = In_fo;
        }
        /// <summary>
        /// Json 格式数据，需要是List 或者DataTable
        /// </summary>
        /// <typeparam name="T">Json 格式数据，需要是List 或者DataTable</typeparam>
        /// <param name="t"></param>
        public void SetInfo<T>(T t)
        {
            Info = JsonConvert.SerializeObject(t);
        }
        public T GetInfo<T>()
        {
            return JsonConvert.DeserializeObject<T>(Info);
        }

        /// <summary>
        /// 更新类别
        /// </summary>
        public NotifyUpdateType UpdateType { get; set; }

        /// <summary>
        /// 消息内容 根据Typeid定义
        /// </summary>
        public string Info { get; set; }
    }
    /// <summary>
    ///内部通知更新数据-单表数据
    /// </summary>
    public class NotifyUpdateInfoSingleTable
    {
        public NotifyUpdateInfoSingleTable()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data_ID"></param>
        /// <param name="Update_Time">更新时间</param>
        /// <param name="_Scope">更新范围类型 1 全部更新 2 只业务服务器更新 3 业务服务器和指定依赖业务更新</param>
        /// <param name="_Route"> <see cref="Scope"/>为3 时的指定依赖路由标记</param>
        public NotifyUpdateInfoSingleTable(string Data_ID, DateTime Update_Time, int _Scope, string _Route = "")
        {
            DataID = Data_ID;
            UpdateTime = Update_Time;
            Scope = _Scope;
            Route = _Route;
        }
        public List<NotifyUpdateInfoMX> lsUpdata = new List<NotifyUpdateInfoMX>();

        public void AddUpdata(NotifyUpdateInfoMX notifyUpdate)
        {
            lsUpdata.Add(notifyUpdate);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Update_Type"></param>
        /// <param name="In_fo">Json 格式数据，需要是List 或者DataTable</param>
        public void AddUpdata(NotifyUpdateType Update_Type, string In_fo)
        {
            NotifyUpdateInfoMX notifyUpdate = new NotifyUpdateInfoMX();
            notifyUpdate.UpdateType = Update_Type;
            notifyUpdate.Info = In_fo;
            lsUpdata.Add(notifyUpdate);
        }

        public void AddUpdata<T>(NotifyUpdateType Update_Type, T In_fo)
        {
            NotifyUpdateInfoMX notifyUpdate = new NotifyUpdateInfoMX();
            notifyUpdate.UpdateType = Update_Type;
            notifyUpdate.SetInfo(In_fo);
            lsUpdata.Add(notifyUpdate);
        }

        /// <summary>
        /// 数据(表)对应ID或名称
        /// </summary>
        public string DataID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///更新范围类型
        ///1 全部更新
        ///2 只业务服务器更新
        ///3 业务服务器和指定依赖业务更新
        /// </summary>
        public int Scope { get; set; }
        /// <summary>
        /// <see cref="Scope"/>为3 时的指定依赖路由标记
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 消息内容 根据Typeid定义
        /// </summary>
        public string Info { get; set; }



    }
    /// <summary>
    ///内部通知更新数据
    /// </summary>
    public class NotifyUpdateInfo
    {
        public NotifyUpdateInfo()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data_ID"></param>
        /// <param name="Update_Time">更新时间</param>
        /// <param name="_Scope">更新范围类型 1 全部更新 2 只业务服务器更新 3 业务服务器和指定依赖业务更新</param>
        /// <param name="_Route"> <see cref="Scope"/>为3 时的指定依赖路由标记</param>
        public NotifyUpdateInfo(string Data_ID, DateTime Update_Time, int _Scope, string _Route = "")
        {
            BusID = 1;
            DataID = Data_ID;
            UpdateTime = Update_Time;
            Scope = _Scope;
            Route = _Route;
        }
        public NotifyUpdateInfo(NotifyUpdateInfoSingleTable notifyUpdate)
        {
            BusID = 1;
            DataID = notifyUpdate.DataID;
            UpdateTime = notifyUpdate.UpdateTime;
            Scope = notifyUpdate.Scope;
            Route = notifyUpdate.Route;
        }


        /// <summary>
        /// 设置业务内容 并加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetInfoEncrypt<T>(T t)
        {
            string Infot = JsonConvert.SerializeObject(t);
            if (Infot.Length > 2000)
            {
                Infot = GZipHelper.CompressToBase64(Infot);
                this.Zip = 1;
            }
            if (ET == 0)
            {
                ET = 1;
            }
            Info = AESHelper.EncryptDefault(Infot);
        }
        /// <summary>
        ///  加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void EncryptInfo()
        {
            if (ET == 0 && !string.IsNullOrEmpty(Info))
            {
                ET = 1;
                Info = AESHelper.EncryptDefault(Info);
            }
        }
        /// <summary>
        /// 设置业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetInfo<T>(T t)
        {
            Info = JsonConvert.SerializeObject(t);
            if (Info.Length > 2000)
            {
                Info = GZipHelper.CompressToBase64(Info);
                this.Zip = 1;
            }
        }
        /// <summary>
        /// 获取业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public T GetInfo<T>()
        {
            if (string.IsNullOrEmpty(Info))
            {
                return default(T);
            }

            if (ET == 1)
            {
                string infot = AESHelper.DecryptDefault(Info);
                if (this.Zip == 1)
                {
                    infot = GZipHelper.DecompressBase64(infot);
                }
                return JsonConvert.DeserializeObject<T>(infot);
            }
            else
            {
                if (this.Zip == 1)
                {
                    string infot = GZipHelper.DecompressBase64(Info);
                    return JsonConvert.DeserializeObject<T>(infot);
                }
                else
                    return JsonConvert.DeserializeObject<T>(Info);
            }
        }

        public string Get_Info()
        {
            if (ET == 1)
            {
                string infot = AESHelper.DecryptDefault(Info);
                if (this.Zip == 1)
                {
                    infot = GZipHelper.DecompressBase64(infot);
                }
                return infot;
            }
            else
            {
                if (this.Zip == 1)
                {
                    return GZipHelper.DecompressBase64(Info);
                }
                else
                    return Info;
            }
        }
        /// <summary>
        /// 1 单表 <see cref="NotifyUpdateInfoSingleTable "/> 此时<see cref="DataID"/>有效；
        /// 2 多表<see cref="List NotifyUpdateInfoSingleTable  "/>
        /// 4 配置变化<see cref="SLocalDataDefChnage "/>
        /// </summary>
        public int BusID { get; set; }

        /// <summary>
        /// 数据(表)对应ID或名称
        /// </summary>
        public string DataID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///更新范围类型
        ///1 全部更新
        ///2 只业务服务器更新
        ///3 业务服务器和指定依赖业务更新客户端
        ///4  指定依赖业务更新客户端(一般用于Scope变化时通知)
        /// </summary>
        public int Scope { get; set; }
        /// <summary>
        /// <see cref="Scope"/>为3 时的指定依赖路由标记
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 消息内容 根据Typeid定义
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        ///此 字段系统自动赋值， 压缩 0 未压缩；1 GZipStream 压缩；  先压缩再加密，先解密再解压
        /// </summary>
        public int Zip { get; set; }




        /// <summary>
        ///此 字段系统自动赋值， 加密类别 0:不加密；1:AES 
        /// </summary>
        public int ET { get; set; }

    }

    /// <summary>
    /// 服务器对应的BusID 负载汇总
    /// </summary>
    public class BusinessInfoOnline
    {
        public BusinessInfoOnline()
        {
            UniqueConcurrentFlagList = new List<string>();
        }
        public BusinessInfoOnline Copy()
        {
            BusinessInfoOnline busInfoOnline = new BusinessInfoOnline();
            busInfoOnline.BusID = BusID;
            busInfoOnline.busType = busType;
            busInfoOnline.busDDLBS = busDDLBS;
            busInfoOnline.busStatus = busStatus;
            busInfoOnline.Maxconcurrent = Maxconcurrent;
            busInfoOnline.TheConcurrent = TheConcurrent;
            TTL = busInfoOnline.TTL;
            busInfoOnline.UniqueConcurrentFlagList = new List<string>();
            foreach (string str in UniqueConcurrentFlagList)
            {
                busInfoOnline.UniqueConcurrentFlagList.Add(str);
            }
            busInfoOnline.dcBusinessInfoVerOnline = new Dictionary<string, BusinessInfoVerOnline>();
            foreach (string key in dcBusinessInfoVerOnline.Keys)
            {
                busInfoOnline.dcBusinessInfoVerOnline.Add(key, dcBusinessInfoVerOnline[key].Copy());
            }
            busInfoOnline.AllCount = AllCount;
            return busInfoOnline;
        }
        public BusinessInfoOnline(BusinessInfoOnline busInfoOnline)
        {
            BusID = busInfoOnline.BusID;
            busType = busInfoOnline.busType;
            busDDLBS = busInfoOnline.busDDLBS;
            busStatus = busInfoOnline.busStatus;
            Maxconcurrent = busInfoOnline.Maxconcurrent;
            TheConcurrent = busInfoOnline.TheConcurrent;
            TTL = busInfoOnline.TTL;
            UniqueConcurrentFlagList = new List<string>();
            foreach (string str in busInfoOnline.UniqueConcurrentFlagList)
            {
                UniqueConcurrentFlagList.Add(str);
            }
            AllCount = busInfoOnline.AllCount;
        }
        public BusinessInfoOnline(BusinessInfo businessInfo)
        {
            BusID = businessInfo.BusID;
            busType = businessInfo.busType;
            busDDLBS = businessInfo.busDDLBS;
            TTL = businessInfo.TTL;
            if (busDDLBS == BusDDLBS.LimitMaxConcurrentRandomRule)
            { Maxconcurrent = businessInfo.Maxconcurrent; }

            UniqueConcurrentFlagList = new List<string>();
        }

        public string BusID;

        /// <summary>
        /// 业务负载均衡策略
        /// </summary>
        public BusDDLBS busDDLBS;
        /// <summary>
        /// 业务状态
        /// </summary>
        public BusOnlineStatus busStatus;
        /// <summary>
        /// 业务类别
        /// </summary>
        public BusType busType;

        /// <summary>
        /// CacheData生存时长(秒)；当<see cref="busType"/>为<see cref="BusType.CacheData"/>时有效
        /// </summary>
        public int TTL;

        /// <summary>
        /// 最大并发量 
        /// </summary>
        public int Maxconcurrent;
        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent;

        /// <summary>
        /// 唯一并发标记列表
        /// </summary>
        public List<String> UniqueConcurrentFlagList;

        /// <summary>
        /// 总量
        /// </summary>
        public int AllCount;
        /// <summary>
        /// 业务版本信息 Key:(BusID + "^" + BusVersion).GetHashCode()
        /// </summary>

        public Dictionary<string, BusinessInfoVerOnline> dcBusinessInfoVerOnline = new Dictionary<string, BusinessInfoVerOnline>();
    }

    /// <summary>
    /// 服务器对应的BusID BusVersion 负载汇总
    /// </summary>
    public class BusinessInfoVerOnline
    {
        public BusinessInfoVerOnline()
        {
            UniqueConcurrentFlagList = new List<string>();
        }
        public BusinessInfoVerOnline Copy()
        {
            BusinessInfoVerOnline busInfoOnline = new BusinessInfoVerOnline();
            busInfoOnline.BusID = BusID;
            busInfoOnline.BusVersion = BusVersion;
            busInfoOnline.busType = busType;
            busInfoOnline.busDDLBS = busDDLBS;
            busInfoOnline.busStatus = busStatus;
            busInfoOnline.Maxconcurrent = Maxconcurrent;
            busInfoOnline.TheConcurrent = TheConcurrent;
            busInfoOnline.TTL = TTL;
            busInfoOnline.UniqueConcurrentFlagList = new List<string>();
            busInfoOnline.ComMinVer = ComMinVer;
            busInfoOnline.ComMaxVer = ComMaxVer;
            busInfoOnline.ComMinVerN = ComMinVerN;
            busInfoOnline.ComMaxVerN = ComMaxVerN;
            foreach (string str in UniqueConcurrentFlagList)
            {
                busInfoOnline.UniqueConcurrentFlagList.Add(str);
            }
            busInfoOnline.AllCount = AllCount;
            return busInfoOnline;
        }
        //public BusinessInfoVerOnline(BusinessInfoOnline busInfoOnline)
        //{
        //    BusID = busInfoOnline.BusID;
        //    busType = busInfoOnline.busType;
        //    busDDLBS = busInfoOnline.busDDLBS;
        //    busStatus = busInfoOnline.busStatus;
        //    Maxconcurrent = busInfoOnline.Maxconcurrent;
        //    TheConcurrent = busInfoOnline.TheConcurrent;
        //    TTL = busInfoOnline.TTL;
        //    UniqueConcurrentFlagList = new List<string>();
        //    foreach (string str in busInfoOnline.UniqueConcurrentFlagList)
        //    {
        //        UniqueConcurrentFlagList.Add(str);
        //    }
        //    AllCount = busInfoOnline.AllCount;
        //}
        public BusinessInfoVerOnline(BusinessInfoBusVersion businessInfo)
        {
            BusID = businessInfo.BusID;
            BusVersion = businessInfo.BusVersion;
            ComMinVer = businessInfo.ComMinVer;
            ComMaxVer = businessInfo.ComMaxVer;
            ComMinVerN = DbHelper.VersionTodecimal(ComMinVer);
            ComMaxVerN = DbHelper.VersionTodecimal(ComMaxVer);
            busType = businessInfo.busType;
            TTL = businessInfo.TTL;

            if (busDDLBS == BusDDLBS.LimitMaxConcurrentRandomRule)
            { Maxconcurrent = businessInfo.Maxconcurrent; }

            UniqueConcurrentFlagList = new List<string>();
        }

        public string BusID;
        /// <summary>
        /// 业务版本号
        /// </summary>
        public string BusVersion;

        /// <summary>
        /// 业务负载均衡策略
        /// </summary>
        public BusDDLBS busDDLBS;
        /// <summary>
        /// 业务状态
        /// </summary>
        public BusOnlineStatus busStatus;
        /// <summary>
        /// 业务类别
        /// </summary>
        public BusType busType;

        /// <summary>
        /// CacheData生存时长(秒)；当<see cref="busType"/>为<see cref="BusType.CacheData"/>时有效
        /// </summary>
        public int TTL;

        /// <summary>
        /// 最大并发量 
        /// </summary>
        public int Maxconcurrent;
        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent;

        /// <summary>
        /// 唯一并发标记列表
        /// </summary>
        public List<String> UniqueConcurrentFlagList;

        /// <summary>
        /// 总量
        /// </summary>
        public int AllCount;

        /// <summary>
        /// 兼容最小版本  
        /// </summary>
        public string ComMinVer { get; set; }
        /// <summary>
        /// 兼容最大版本
        /// </summary>
        public string ComMaxVer { get; set; }
        /// <summary>
        /// 兼容最小版本  
        /// </summary>
        public decimal ComMinVerN { get; set; }
        /// <summary>
        /// 兼容最大版本
        /// </summary>
        public decimal ComMaxVerN { get; set; }

        public override int GetHashCode()
        {
            return (BusID + "^" + BusVersion).GetHashCode();
        }

    }
    public class SGetTableUpdataData
    {
        public string TableNameL;
        public DateTime UpdateTime;
    }
    /// <summary>
    /// 本地表配置变化通知消息
    /// </summary>
    public class SLocalDataDefChnage
    {
        /// <summary>
        /// 1 新增
        /// 2 删除
        /// 3变化 需要删除原表
        /// 4变化 需要删除视图
        /// 5变化 需要删除原存储过程
        /// 6变化 只需要清空内存表
        /// </summary>
        public int CType;
        public SLocalDataDef NewLocalDataDef;
    }


    public class ClientSystemAndProcessInfo
    {

        public ClientSystemAndProcessInfo()
        {
        }
        public ClientSystemAndProcessInfo(SystemAndProcessInfo processInfo)
        {

            this.Id = processInfo.Id;
            this.ProcessName = processInfo.ProcessName;
            this.TotalProcessorTime = processInfo.TotalProcessorTime;
            this.WorkingSet64 = processInfo.WorkingSet64;
            this.FileName = processInfo.FileName;
            this.ProcessorName = processInfo.ProcessorName;
            this.ProcessorCount = processInfo.ProcessorCount;
            this.CpuLoad = processInfo.CpuLoad;
            this.MemoryAvailable = processInfo.MemoryAvailable;
            this.PhysicalMemory = processInfo.PhysicalMemory;
            this.MachineName = processInfo.MachineName;
            this.OSVersion = processInfo.OSVersion;
            this.IPAddress = processInfo.IPAddress;
            this.Drives = processInfo.Drives;
        }
        public List<DiskInfo> Drives;

        public void Update(SystemAndProcessDynamicInfo DynamicInfo)
        {
            this.CpuLoad = DynamicInfo.CpuLoad;
            this.MemoryAvailable = DynamicInfo.MemoryAvailable;
            this.TotalProcessorTime = DynamicInfo.TotalProcessorTime;
            this.WorkingSet64 = DynamicInfo.WorkingSet64;

        }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string UDID { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string UDName { get; set; }

        /// <summary>
        /// SLBID
        /// </summary>
        public string SLBID { get; set; }


        /// <summary>
        ///HOS_ID
        /// </summary>
        public string OrgID { get; set; }
        /// <summary>
        ///在医院的SLBID
        /// </summary>
        public string ClientSLBID { get; set; }
        /// <summary>
        /// 进程唯一标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 总处理器处理时间(毫秒)
        /// </summary>
        public double TotalProcessorTime { get; set; }
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public long WorkingSet64;
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public decimal WorkingSet64MB()
        {
            return WorkingSet64 / 1024m / 1024m;
        }
        /// <summary>
        /// 模块的完整路径
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 获取CPU名称
        /// </summary>
        public string ProcessorName { get; set; }
        /// <summary>
        /// 获取CPU个数 
        /// </summary>
        public int ProcessorCount { get; set; }
        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad { get; set; }
        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable { get; set; }

        /// <summary>
        /// 计算机名
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string OSVersion { get; set; }


        /// <summary>
        /// IP列表
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 获取可用内存 
        /// </summary>

        public decimal MemoryAvailableMB()
        {
            return MemoryAvailable / 1024m / 1024m;
        }
        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public long PhysicalMemory { get; set; }

        /// <summary>
        /// 获取物理内存 
        /// </summary>
        /// <returns></returns>

        public decimal PhysicalMemoryMB()
        {
            return PhysicalMemory / 1024m / 1024m;
        }

    }


    public class ClientSystemAndProcessInfoClientSLB
    {
        public List<ClientSystemAndProcessInfo> clients = new List<ClientSystemAndProcessInfo>();
        public string FBOOMCID { get; set; }
    }
    public class GetSClientSystemAndProcessInfo
    {
        public string HOS_ID { get; set; }

        public bool FromClientSLB { get; set; }

        public string FBOSLBID { get; set; }
        public string FBOOMCID { get; set; }

    }

    public class SScreenBegin
    {

        /// <summary>
        /// 显示百分比
        /// </summary>
        public int percent = 100;
        /// <summary>
        /// 两幅图片之间间隔时间 
        /// </summary>
        public int Ttimeinterval = 300;
        /// <summary>
        /// 压缩等级，0到100，0 最差质量，100 最佳  默认100
        /// </summary>
        public long level = 50;


    }
    /// <summary>
    /// 移除服务器信息消息体
    /// </summary>
    public class SBusServerRemoveInfo
    {
        public string BusServerID;
        /// <summary>
        /// 是否已经宕机离线
        /// </summary>
        public bool Alreadyoffline;

        /// <summary>
        /// 是否要求BusServer程序重新启动
        /// </summary>
        public bool ReStart;
        /// <summary>
        /// 说明:移除原因等
        /// </summary>
        public string Note;
    }
    /// <summary>
    /// 通知指定BusServer当前在线SLB列表
    /// </summary>
    public class SOnLineSLBlistToBusServer
    {
        /// <summary>
        ///SLBInfoS
        /// </summary>
        public List<SLBInfo> SLBList;
        /// <summary>
        /// 业务服务器ID
        /// </summary>
        public string BusServerID;

    }
    /// <summary>
    /// 更新指定BusServer的HTTP服务允许接收的业务
    /// </summary>
    public class UpSBusServerHttpBus
    {
        /// <summary>
        /// 业务ID S
        /// </summary>
        public List<string> BusIDS;
        /// <summary>
        /// 业务服务器ID
        /// </summary>
        public string BusServerID;
        /// <summary>
        /// 允许还是删除
        /// </summary>
        public bool Enable;

    }
    /// <summary>
    /// BusServer的当前状态结构
    /// </summary>
    public class SBusServerRates
    {
        public string BusServerID;
        public Dictionary<string, SBusServerBusRates> sdBusRates = new Dictionary<string, SBusServerBusRates>();
        public SystemAndProcessInfo SystemandProcessInfo = new SystemAndProcessInfo();

        /// <summary>
        /// 已经连接的SLB数量
        /// </summary>
        public int ConnectSLB { get; set; }

        /// <summary>
        /// 在SLB上显示的IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 正在调用的业务数量
        /// </summary>
        public int runningBusCount { get; set; }

        public int ServerToSLBRates { get; set; }
        public int SLBToServerRates { get; set; }
        /// <summary>
        /// BusServer到SLB请求调用其他BusServer服务
        /// </summary>
        public int ServerCallOtherToSLBRates { get; set; }
        /// <summary>
        /// SLB返回 BusServer请求调用其他BusServer服务的结果
        /// </summary>
        public int SLBToServerCallOtherBackRates { get; set; }
        /// <summary>
        /// SLB返回: BusServer请求调用其他BusServer服务，但无可用服务
        /// </summary>
        public int SLBToServerCallOtherNoServerRates { get; set; }


        /// <summary>
        /// 连接的SLBID列表
        /// </summary>
        public List<string> SLBList { get; set; } = new List<string>();

        /// <summary>
        /// Http服务是否运行
        /// </summary>
        public bool HttpIsRun { get; set; }
        /// <summary>
        /// Http运行端口
        /// </summary>
        public int HttpPort { get; set; }
        /// <summary>
        /// 业务日志是否通过HTTPAPI上传保存
        /// 只有BusServer BusLogAPISave为True且UseHttpSpringAPI为Tue时生效
        /// </summary>
        public bool LogHttpAPISave { get; set; }
    }

    public class SBusServerBusRates
    {
        public SBusServerBusRates()
        {

        }
        public SBusServerBusRates(SBusServerBusRates busServerBusRates)
        {


            BusID = busServerBusRates.BusID;
            TheConcurrent = busServerBusRates.TheConcurrent;
            AllCount = busServerBusRates.AllCount;
            InRates = busServerBusRates.InRates;
            OutRates = busServerBusRates.OutRates;
            StatPeriod = busServerBusRates.StatPeriod;
            StatRangeData = busServerBusRates.GetStatData();




        }
        public SBusServerBusRates(SBusServerBusRates busServerBusRates, bool Stat)
        {
            BusID = busServerBusRates.BusID;
            TheConcurrent = busServerBusRates.TheConcurrent;
            AllCount = busServerBusRates.AllCount;
            InRates = busServerBusRates.InRates;
            OutRates = busServerBusRates.OutRates;
            this.Stat = Stat;
            DicstatPeriodNo = 2;
            this.statPeriod = DicstatPeriod[DicstatPeriodNo];
        }
        Dictionary<int, int> DicstatPeriod
        {
            get
            {
                if (dicstatPeriod == null)
                {
                    dicstatPeriod = new Dictionary<int, int>();
                    dicstatPeriod.Add(1, 60);
                    dicstatPeriod.Add(2, 120);
                    dicstatPeriod.Add(3, 300);
                    dicstatPeriod.Add(4, 600);
                    dicstatPeriod.Add(5, 1800);
                    dicstatPeriod.Add(6, 3600);
                    dicstatPeriod.Add(7, 7200);
                    dicstatPeriod.Add(8, 18000);
                    dicstatPeriod.Add(9, 36000);
                    dicstatPeriod.Add(10, 86400);
                }
                return dicstatPeriod;
            }
        }
        Dictionary<int, int> dicstatPeriod = null;
        /// <summary>
        /// 统计周期编号
        /// </summary>
        int DicstatPeriodNo = 3;

        /// <summary>
        /// 统计业务错误状态
        /// </summary>
        bool Stat = true;
        /// <summary>
        /// 统计周期，秒
        /// </summary>
        int statPeriod = 300;
        /// <summary>
        /// 统计内部周期，秒
        /// </summary>
        int StatPeriodDeci = 4;
        /// <summary>
        /// 统计内部周期数StatPeriodDeci*StatPeriodDeciCount=statPeriod
        /// </summary>
        const int StatPeriodDeciCount = 30;
        /// <summary>
        /// 统计内部周期当前累计值，当大于StatPeriodDeci时 重启计算
        /// </summary>
        int statPeriodadd = 0;
        /// <summary>
        /// 重新设定统计周期后统计次数
        /// </summary>
        int reStatPeriodDeciCount = 0;
        Queue<StatData> queueStat = new Queue<StatData>();

        public void StatUpdate(int intervalSecond)
        {
            if (!Stat)
                return;

            statPeriodadd += intervalSecond;
            if (statPeriodadd >= StatPeriodDeci)
            {
                if (queueStat.Count >= StatPeriodDeciCount)
                {
                    int statCountAll = queueStat.Sum(o => o.AllCount);

                    if (statCountAll <= 10 && DicstatPeriodNo < DicstatPeriod.Count && reStatPeriodDeciCount >= StatPeriodDeciCount)
                    {
                        statPeriod = DicstatPeriod[DicstatPeriodNo++];
                        StatPeriodDeci = statPeriod / StatPeriodDeciCount;
                        reStatPeriodDeciCount = 1;
                    }
                    else if (statCountAll > 1200 && DicstatPeriodNo > 1 && reStatPeriodDeciCount >= StatPeriodDeciCount)
                    {
                        statPeriod = DicstatPeriod[DicstatPeriodNo--];
                        StatPeriodDeci = statPeriod / StatPeriodDeciCount;
                        statPeriodadd = intervalSecond;
                        queueStat.Dequeue();//删除第一个
                        StatData statData = new StatData();
                        queueStat.Enqueue(statData);
                        reStatPeriodDeciCount = 1;
                    }
                    else
                    {
                        statPeriodadd = statPeriodadd % StatPeriodDeci;
                        queueStat.Dequeue();//删除第一个
                        StatData statData = new StatData();
                        queueStat.Enqueue(statData);
                        reStatPeriodDeciCount = reStatPeriodDeciCount > 1000 ? 1000 : reStatPeriodDeciCount + 1;
                    }
                }
                else//未到最大数 增加 queueStat
                {
                    statPeriodadd = statPeriodadd % StatPeriodDeci;
                    StatData statData = new StatData();
                    queueStat.Enqueue(statData);
                    reStatPeriodDeciCount = reStatPeriodDeciCount > 1000 ? 1000 : reStatPeriodDeciCount + 1;
                }

            }
            else if (queueStat.Count == 0)
            {
                StatData statData = new StatData();
                queueStat.Enqueue(statData);
                reStatPeriodDeciCount = reStatPeriodDeciCount > 1000 ? 1000 : reStatPeriodDeciCount + 1;
            }

            queueStat.Last().AllCount += InRates;
            queueStat.Last().AbnormaCount += AbnormalRates;
            queueStat.Last().BusAbnormalCount += BusAbnormalRates;
            queueStat.Last().BusErrorCount += BusErrorRates;
            queueStat.Last().SysErrorCount += SysErrorRates;

            if (!string.IsNullOrEmpty(LBusAM))
                queueStat.Last().LBusAM = LBusAM;
            if (!string.IsNullOrEmpty(LBusEM))
                queueStat.Last().LBusEM = LBusEM;
            if (!string.IsNullOrEmpty(LSysEM))
                queueStat.Last().LSysEM = LSysEM;

            InRates = 0;
            OutRates = 0;
            AbnormalRates = 0;
            BusAbnormalRates = 0;
            BusErrorRates = 0;
            SysErrorRates = 0;

        }

        public StatData GetStatData()
        {
            StatData statData = new StatData();
            if (Stat)
            {
                statData.AllCount = queueStat.Sum(o => o.AllCount);
                statData.AbnormaCount = queueStat.Sum(o => o.AbnormaCount);
                statData.BusErrorCount = queueStat.Sum(o => o.BusErrorCount);
                statData.BusAbnormalCount = queueStat.Sum(o => o.BusAbnormalCount);
                statData.SysErrorCount = queueStat.Sum(o => o.SysErrorCount);
                if (queueStat.Count > 0)
                {
                    statData.LBusAM = queueStat.Last().LBusAM;
                    statData.LBusEM = queueStat.Last().LBusEM;
                    statData.LSysEM = queueStat.Last().LSysEM;
                }

            }


            return statData;
        }
        /// <summary>
        /// 业务执行异常统计
        /// </summary>
        public StatData StatRangeData { get; set; } = new StatData();

        /// <summary>
        /// 统计周期，秒
        /// </summary>
        public int StatPeriod
        {
            get
            {
                return statPeriod;
            }
            set
            {
                statPeriod = value;
                StatPeriodDeci = statPeriod / StatPeriodDeciCount;
            }
        }

        public string BusID;


        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent;


        /// <summary>
        /// 总量
        /// </summary>
        public int AllCount;
        /// <summary>
        /// 接收速率
        /// </summary>
        public int InRates;

        /// <summary>
        /// 返回速率
        /// </summary>
        public int OutRates;
        /// <summary>
        /// 异常结果速率
        /// </summary>
        public int AbnormalRates;

        /// <summary>
        /// 当前统计范围业务异常速率
        /// </summary>
        public int BusAbnormalRates;

        /// <summary>
        /// 异常结果速率业务错误速率
        /// </summary>
        public int BusErrorRates;

        /// <summary>
        ///异常结果速率系统错误速率
        /// </summary>
        public int SysErrorRates;

        public void SetLBusEM(string LBusEM)
        {
            this.LBusEM = LBusEM;
        }
        public void SetLBusAM(string LBusAM)
        {
            this.LBusAM = LBusAM;
        }
        public void SetLSysEM(string LSysEM)
        {
            this.LSysEM = LSysEM;
        }
        /// <summary>
        /// 最后一次业务错误消息
        /// </summary>
        string LBusEM;
        /// <summary>
        /// 最后一次业务一次消息
        /// </summary>
        string LBusAM;
        /// <summary>
        /// 最后一次系统异常消息
        /// </summary>
        string LSysEM;
        public class StatData
        {/// <summary>
         /// 当前统计范围总数量
         /// </summary>
            public int AllCount;
            /// <summary>
            ///异常数量
            /// </summary>
            public int AbnormaCount;
            /// <summary>
            /// 当前统计范围业务异常数量
            /// </summary>
            public int BusAbnormalCount;
            /// <summary>
            /// 当前统计范围业务错误数量
            /// </summary>
            public int BusErrorCount;
            /// <summary>
            /// 当前统计范围系统错误数量
            /// </summary>
            public int SysErrorCount;
            /// <summary>
            /// 最后一次业务错误消息
            /// </summary>
            public string LBusEM;
            /// <summary>
            /// 最后一次业务一次消息
            /// </summary>
            public string LBusAM;
            /// <summary>
            /// 最后一次系统异常消息
            /// </summary>
            public string LSysEM;
        }
    }
}


