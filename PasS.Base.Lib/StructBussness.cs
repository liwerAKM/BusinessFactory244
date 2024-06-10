using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasS.Base.Lib.Model;

namespace PasS.Base.Lib
{ /// <summary>
  /// 服务器负载均衡策略种类；整个系统只允许一种
  /// </summary>
    public enum ServerDDLBS
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        NULL = -1,
        /// <summary>
        /// 按照服务器权重最小负载，也就是按照权重比例选择当前负载最小的那个，这个是默认值。
        /// </summary>
        WeightMinimumLoadRule = 0,

        /// <summary>
        /// 按照权重随机选择。
        /// </summary>
        WeightRandomRule = 1,

        /// <summary>
        /// 轮询策略， 以轮询的方式选择服务器。所有实例中所启动的服务会被循环访问
        /// </summary>
        RoundRobinRule = 2

        ///// <summary>
        ///// 限定最大并发数
        ///// </summary>
        //LimitMaxConcurrentRandomRule = 4


    }

    /// <summary>
    /// 业务负载均衡策略种类
    /// </summary>
    public enum BusDDLBS
    {

        /// <summary>
        /// 无特殊策略 按照上级载体策略执行
        /// </summary>
        NoPolicy = 0,

        /// <summary>
        /// 限定该业务总的最大并发数
        /// </summary>
        LimitMaxConcurrentRandomRule = 1,

        /// <summary>
        /// 分别限定每一个业务服务器最大并发数
        /// </summary>
        LimitServerMaxConcurrentRandomRule = 2
    }
    /// <summary>
    ///业务当前状态
    /// </summary>
    public enum BusStatus
    {
        /// <summary>
        /// 停止将从在线业务中删除
        /// </summary>
        Offline = 0,
        /// <summary>
        /// 表示服务在线 可以正常运行对应<see cref="BusOnlineStatus.WORKING"/><see cref="BusOnlineStatus.READY"/>
        /// </summary>
        Publish = 1,
        /// <summary>
        /// 暂停状态，表示当前不进行任务的处理工作 对应<see cref="BusOnlineStatus.SILENCE"/>
        /// </summary>
        Suspend = 2,
    }

    /// <summary>
    ///业务的业务版本当前状态
    /// </summary>
    public enum BusBusVersionStatus
    {
        /// <summary>
        /// 停止将从在线业务中删除
        /// </summary>
        Offline = 0,
        /// <summary>
        /// 表示服务在线 可以正常运行对应<see cref="BusOnlineStatus.WORKING"/><see cref="BusOnlineStatus.READY"/>
        /// </summary>
        Publish = 1,
        /// <summary>
        /// 暂停状态，表示当前不进行任务的处理工作 对应<see cref="BusOnlineStatus.SILENCE"/>
        /// </summary>
        Suspend = 2,
    }
    /// <summary>
    ///服务器中在线业务当前状态
    /// </summary>
    public enum BusOnlineStatus
    {
        /// <summary>
        /// 表示没有任何服务器执行此工作;当有服务器接入执行工作时自动切换为WORKING
        /// </summary>
        READY = 0,
        /// <summary>
        /// 表示服务在线 可以正常运行。
        /// </summary>
        WORKING = 1,
        /// <summary>
        /// 静默状态，表示当前不进行任务的处理工作。
        /// </summary>
        SILENCE = 2,
        /// <summary>
        /// 表示服务出现问题需要处理。
        /// </summary>
        ERROE = 3

    }
    /// <summary>
    ///BusinessInfo不同版本业务状态
    /// </summary>
    public enum BusinessInfoVersionStatus
    {
        /// <summary>
        /// 表示下线不再使用
        /// </summary>
        Offline = 0,
        /// <summary>
        ///发布使用的
        /// </summary> 
        Publish = 1,
        /// <summary>
        /// 经过测试可以发布的
        /// </summary>
        Releasable = 2,
        /// <summary>
        ///一级测试，小规模用户测试
        /// </summary>
        TestLevel1 = 3,
        /// <summary>
        ///二级测试，指定部分人员测试
        /// </summary>
        TestLevel2 = 4,
        /// <summary>
        ///三级级测试，最初开发测试
        /// </summary>
        TestLevel3 = 5,
        /// <summary>
        ///刚创建的，不做任何测试及发布
        /// </summary>
        Setup = 6

    }

   

    
    /// <summary>
    /// 服务器对应的BusID配置明细
    /// </summary>
    public class BusinessInfo
    {
        public BusinessInfo()
        {

        }
        public BusinessInfo(businessinfoversion businessInfoVersion)
        {
            this.BusID = businessInfoVersion.BusID;
            this.ProjectID = businessInfoVersion.projectID;
            this.DllPath = businessInfoVersion.FilePath;
            this.DllName = businessInfoVersion.DllName;
            this.version = businessInfoVersion.Version;

        }
        /// <summary>
        /// 业务模块ID 
        /// </summary>
        public string BMODID;

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusID;


        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusName;

        /// <summary>
        /// 版本号
        /// </summary>
        public string version;

        /// <summary>
        /// 业务类别
        /// </summary>
        public BusType busType;

        /// <summary>
        /// CacheData生存时长(秒)；当<see cref="busType"/>为<see cref="BusType.CacheData"/>时有效
        /// </summary>
        public int TTL;

        /// <summary>
        /// 业务负载均衡策略
        /// </summary>
        public BusDDLBS busDDLBS;
        /// <summary>
        /// 最大并发量 busDDLBS为<see cref="BusDDLBS.LimitMaxConcurrentRandomRule" /> 时有效
        /// </summary>
        public int Maxconcurrent;

        /// <summary>
        /// 执行此工作程序所在dll 例如:ProcessBusTest.dll ; 
        /// 必须参数
        /// </summary>
        public string DllName { get; set; }
        /// <summary>
        /// 命名空间和类名 例如:ProcessBusTest.ProcessBusShowR
        /// 必须参数
        /// </summary>
        public string NamespaceClass { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string DllPath { get; set; }
        /// <summary>
        /// 工程ID
        /// </summary>
        public string ProjectID { get; set; }

        /// <summary>
        /// 日志类别0	只记录异常
        /// 1	记录入参
        /// 2	记录出参
        /// 3	记录入参和出参
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// 业务日志单独处理
        /// </summary>
        public bool BLogSeparate { get; set; }
        /// <summary>
        /// 慢业务记录点(毫秒)（0时将按系统默认值）
        /// </summary>
        public int RecordSlowExecute { get; set; }

        /// <summary>
        /// 业务异常推送消息组ID MessageSendGroupID
        /// </summary>
        public int MSGID { get; set; }


        public string fileID
        {
            get
            {
                return "ProjectID:" + ProjectID + ",Path:" + DllPath + ",Name:" + DllName + ",Version:" + version;

            }
        }
        /// <summary>
        /// 该业务下的业务版本
        /// </summary>
        public List<BusinessInfoBusVersion> BusVersions = new List<BusinessInfoBusVersion>();

    }
    public class BusinessInfoBusVersion
    {
        public BusinessInfoBusVersion()
        {

        }
        public BusinessInfoBusVersion(businessinfoversion businessInfoVersion)
        {
            this.BusID = businessInfoVersion.BusID;
            this.ProjectID = businessInfoVersion.projectID;
            this.DllPath = businessInfoVersion.FilePath;
            this.DllName = businessInfoVersion.DllName;
            this.version = businessInfoVersion.Version;

        }

        public BusinessInfoBusVersion(BusinessInfo business)
        {
            this.BusID = business.BusID;
            this.BusName = business.BusName;
            this.ProjectID = business.ProjectID;
            this.busType = business.busType;
            this.TTL = business.TTL;
            this.busType = business.busType;
            this.BusVersion = "1.0";
            this.DllName = business.DllName;
            this.DllPath = business.DllPath;
            this.LogType = business.LogType;
            this.Maxconcurrent = business.Maxconcurrent;
            this.NamespaceClass = business.NamespaceClass;
            this.RecordSlowExecute = business.RecordSlowExecute;
            this.version = business.version;
            this.VersionN = business.version;

        }
        public void Updateinfo(BusinessInfo business)
        {
            this.BusID = business.BusID;
            this.BusName = business.BusName;
            this.ProjectID = business.ProjectID;
            this.busType = business.busType;
            this.TTL = business.TTL;
        }
        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusID;



        /// <summary>
        /// 业务业务版本号(业务版本中用  addby wanglei 20200210)
        /// </summary>
        public string BusVersion;


        /// <summary>
        /// 兼容最小版本  
        /// </summary>
        public string ComMinVer { get; set; }
        /// <summary>
        /// 兼容最大版本
        /// </summary>
        public string ComMaxVer { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusName;

        public string ProjectID { get; set; }


        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionN;

        /// <summary>
        /// 版本号
        /// </summary>
        public string version;
        /// <summary>
        /// 业务类别
        /// </summary>
        public BusType busType;

        /// <summary>
        /// CacheData生存时长(秒)；当<see cref="busType"/>为<see cref="BusType.CacheData"/>时有效
        /// </summary>
        public int TTL;
        /// <summary>
        /// 执行此工作程序所在dll 例如:ProcessBusTest.dll ; 
        /// 必须参数
        /// </summary>
        public string DllName { get; set; }

        /// <summary>
        /// 命名空间和类名 例如:ProcessBusTest.ProcessBusShowR
        /// 必须参数
        /// </summary>
        public string NamespaceClass { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string DllPath { get; set; }

        /// <summary>
        /// 最大并发量 busDDLBS为<see cref="BusDDLBS.LimitMaxConcurrentRandomRule" /> 时有效
        /// </summary>
        public int Maxconcurrent;


        /// <summary>
        /// 日志类别0	只记录异常
        /// 1	记录入参
        /// 2	记录出参
        /// 3	记录入参和出参
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// 业务日志单独处理
        /// </summary>
        public bool BLogSeparate { get; set; }
        /// <summary>
        /// 慢业务记录点(毫秒)（0时将按系统默认值）
        /// </summary>
        public int RecordSlowExecute { get; set; }


        public string fileID
        {
            get
            {
                return "ProjectID:" + ProjectID + ",Path:" + DllPath + ",Name:" + DllName + ",Version:" + version;

            }
        }

    }
   
    /// <summary>
    /// 业务执行异常类别
    /// </summary>
    public enum EHandingBusinessResult
    {
        sucess = 1,
        BusinessAbnormal = 0,
        BusinessError = -1,
        SysError = -2
    }
    /// <summary>
    /// 业务消息
    /// </summary>
    public class SLBBusinessInfo
    {

        public SLBBusinessInfo()
        {
        }

       
        public SLBBusinessInfo(SLBBusinessInfo sLBBusinessInfo)
        {
            this.BusID = sLBBusinessInfo.BusID;
            this.CTag = sLBBusinessInfo.CTag;
            this.TID = sLBBusinessInfo.TID;
            this.SubBusID = sLBBusinessInfo.SubBusID;
        }

        public SLBBusinessInfo(SLBBusinessInfo sLBBusinessInfo, string BusName)
        {
            this.BusID = BusName;
            this.CTag = sLBBusinessInfo.CTag;
            this.TID = sLBBusinessInfo.TID;
            this.SubBusID = sLBBusinessInfo.SubBusID;
        }
      
       
        /// <summary>
        /// 设置业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetBusData<T>(T t)
        {
            BusData = JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 获取业务内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public T GetBusData<T>()
        {
            if (string.IsNullOrEmpty(BusData))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(BusData);
        }
        /// <summary>
        /// 业务服务ID
        /// </summary>
        public string BusID { get; set; }

        /// <summary>
        /// 子业务ID  SUB BUSINESS ID 此ID只参与具体的业务，与Spring无直接关系
        /// </summary>
        public string SubBusID { get; set; }


        /// <summary>
        /// 请求唯一ID 不重复
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 具体业务内容
        /// </summary>
        public string BusData { get; set; }

        /// <summary>
        /// 客户端标记 标记用于灰度发布
        /// </summary>
        public string CTag { get; set; }

        /// <summary>
        /// 返回结果标识 1成功
        /// </summary>
        public int ReslutCode { get; set; }
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        /// API用户ID( 入参不用出传入此参数，执行端会自动加入)
        /// </summary>
        public string AUID { get; set; }

        /// <summary>
        /// 业务版本
        /// </summary>
        public string BusVersion { get; set; }

    }
    /// <summary>
    /// 通知指定BusID BusVersion
    /// </summary>
    public class SBusIDBusVersion
    {
        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusID;

        /// <summary>
        /// 业务版本号
        /// </summary>
        public string BusVersion;

    }
    /// <summary>
    /// 灰度测试标识
    /// </summary>
    public class SGrayLevelTest
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserID;
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex;
        /// <summary>
        /// 地理位置
        /// </summary>
        public string Location;
        /// <summary>
        /// 设备型号
        /// </summary>
        public string DeviceModel;
        /// <summary>
        /// 系统版本
        /// </summary>
        public string OSversion;
        /// <summary>
        /// 自定义
        /// </summary>
        public string UserDefined;

    }

    /// <summary>
    /// 单个服务器对应的BusName BusVer信息 add by wanglei 20200211
    /// BusName^BusServerID^BusVersion
    /// </summary>
    public class SingleBusinessInfoVer
    {
        public SingleBusinessInfoVer()
        {

        }
        public SingleBusinessInfoVer(SingleBusinessInfoVer infoVer)
        {
            this.BusID = infoVer.BusID;
            this.BusServerID = infoVer.BusServerID;
            this.BusVersion = infoVer.BusVersion;
            this.Maxconcurrent = infoVer.Maxconcurrent;
            this.TheConcurrent = infoVer.TheConcurrent;
        }
        public SingleBusinessInfoVer(BusinessInfoBusVersion infoVer)
        {
            this.BusID = infoVer.BusID;
            this.BusVersion = infoVer.BusVersion;
            this.BusVersion = infoVer.BusVersion;
            this.Maxconcurrent = infoVer.Maxconcurrent;

        }



        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusID;
        /// <summary>
        /// 业务服务器ID
        /// </summary>
        public string BusServerID;

        /// <summary>
        /// 业务版本号
        /// </summary>
        public string BusVersion;
        /// <summary>
        /// 文件版本号值  addby 王磊20200803
        /// </summary>
        public decimal VersionN;
        /// <summary>
        /// 最大并发量
        /// </summary>
        public int Maxconcurrent;
        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent;



        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(SingleBusinessInfoVer))
                return false;

            SingleBusinessInfoVer hcc = obj as SingleBusinessInfoVer;
            return this.BusID + "^" + this.BusServerID + "^" + BusVersion == hcc.BusID + "^" + hcc.BusServerID + "^" + hcc.BusVersion;
        }

        public override int GetHashCode()
        {
            return (BusID + "^" + BusServerID + "^" + BusVersion).GetHashCode();
        }

        public int GetBusIDBusServerIDHashCode()
        {
            return (BusID + "^" + BusServerID).GetHashCode();
        }

        public override string ToString()
        {
            return BusID + "^" + BusServerID + "^" + BusVersion;
        }
    }
    /// <summary>
    /// 单个服务器对应的BusName 信息
    /// BusID^BusServerID 
    /// </summary>
    public class SingleBusServerInfo
    {
        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusID;
        /// <summary>
        /// 业务服务器ID
        /// </summary>
        public string BusServerID;
        /// <summary>
        /// 最大并发量
        /// </summary>
        public int Maxconcurrent;
        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent;



        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(SingleBusServerInfo))
                return false;

            SingleBusServerInfo hcc = obj as SingleBusServerInfo;
            return this.BusID + "^" + this.BusServerID == hcc.BusID + "^" + hcc.BusServerID;
        }

        public override int GetHashCode()
        {
            return (BusID + "^" + BusServerID).GetHashCode();
        }

        public override string ToString()
        {
            return BusID + "^" + BusServerID;
        }
    }

    /// <summary>
    /// 服务器信息
    /// </summary>
    public class BusServerInfo
    {
        /// <summary>
        /// 服务器内部ID标识符用于数据库及管理
        /// </summary>
        public string BusServerID;

        /// <summary>
        /// 权重 默认100；0 时不再请求
        /// </summary>
        public int Weight;

        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent;

        /// <summary>
        /// 最大并发量 无效参数 后期将去除
        /// </summary>
        public int Maxconcurrent;

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string BusServerName;


        /// <summary>
        /// 签名类别 0:不签名；1:AES-MD5 ; 2:RSA ;3:RSA2  (在非0 情况下与EncryptType对应)
        /// </summary>
        public int SignType { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int EncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认
        /// </summary>
        public string SEID { get; set; }
        /// <summary>
        /// 是否使用HTTP服务
        /// </summary>
        public bool UseHttp { get; set; }
        /// <summary>
        /// http端口
        /// </summary>
        public int HttpPort { get; set; }

        /// <summary>
        /// 是否经过网闸或者防火墙重新做了地址转换
        /// </summary>
        public bool PassGAP { get; set; }
        /// <summary>
        /// 日志是否通过HTTPAPI上传保存，否则通过配置的数据库保存
        /// </summary>
        public bool BusLogAPISave { get; set; }


    }

   
    public class SBusServerBusInfo
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusID;
        /// <summary>
        /// 业务服务器ID
        /// </summary>
        public string BusServerID;
        /// <summary>
        /// 最大并发量
        /// </summary>
        public int Maxconcurrent;
        /// <summary>
        /// 状态
        /// </summary>
        public int Status;

    }
 
/// <summary>
/// SLB通过心跳检测BusServer服务器停启业务服务消息
/// </summary>
public class SHeartbeatBusServerInfo
{
    public string BusServerID;
    /// <summary>
    /// 是停止服务，false 则为启动服务
    /// </summary>
    public bool StopServices;
    /// <summary>
    /// 检测次数
    /// </summary>
    public int TestTime;
    /// <summary>
    /// 说明:移除原因等
    /// </summary>
    public string Note;
}
}
