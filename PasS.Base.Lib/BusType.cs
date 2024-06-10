using System;
using System.Collections.Generic;
using System.Text;

namespace PasS.Base.Lib
{

    /// <summary>
    /// 系统消息类别
    /// </summary>
    public enum SysInfoType
    {

        /// <summary>
        /// 发送连接是否存在的测试 <see cref="STestConnectRet"/>
        /// </summary>
        TestConnect = 4,

        /// <summary>
        /// 首次连接注册信息 注册内容<see cref="SSysRegisterInfo"/> 
        /// </summary>
        Register = 5,
        /// <summary>
        /// 注册信息返回结果
        /// </summary>
        RegisterRet = 6,
        /// <summary>
        /// 客户端向AccessServer 获取SLB地址
        /// </summary>
        ClientAccessServerGetSLB = 7,

        /// <summary>
        /// 新增或者更新一个在线BusServer信息<see cref="BusServerInfo"/>
        /// </summary>
        AddOrUpdateBusServer = 8,
        /// <summary>
        ///彻底移除一个在线一个BusServer信息<see cref="SBusServerRemoveInfo"/>,如果要启动只能到对用服务器手动启动
        /// </summary>
        RemoveBusServer = 9,
        /// <summary>
        ///BusServer 启动业务服务
        /// </summary>
        BusServerStartServices = 10,
        /// <summary>
        /// BusServer停止业务服务,但是能接收系统消息
        /// </summary>
        BusServerStopServices = 11,
        /// <summary>
        /// 增加或更新<see cref="SingleBusServerInfo"/>
        /// </summary>
        AddOrUpdateSingleBusServerInfo = 12,
        /// <summary>
        /// 停止<see cref="SingleBusServerInfo"/>
        /// </summary>
        RemoveSingleBusServerInfo = 13,
        /// <summary>
        /// 增加或修改业务<see cref="BusinessInfoOnline"/>
        /// </summary>
        AddOrUpdateBusinessInfoOnline = 14,


        /// <summary>
        /// 移除业务<see cref="BusID"/>
        /// </summary>
        RemoveBusinessInfoOnline = 15,

        /// <summary>
        /// OtherSLBChnage 上线下线<see cref="SLBInfo"/>，如果是 SLB则通知在线BusServer 如果是BusServer 收到 则连接到SLB;
        /// </summary>
        OtherSLBChnage = 16,
        /// <summary>
        /// SLBOffline 将下线不再接受服务;
        /// </summary>
        SLBOffline = 17,
        /// <summary>
        /// SLBOffline 将下线不再接受服务;
        /// </summary>
        SLBWillOffline = 18,

        /// <summary>
        /// SLB当前负载及系统状态<see cref="SLBCurrentStatus"/>
        /// </summary>
        SLBRatesStatus = 19,
        /// <summary>
        /// BusinessInfo或版本更新<see cref="BusID"/> 需要更新到每一个BusServer
        /// </summary>
        UpdateBusinessInfo = 20,
        /// <summary>
        /// BusinessInfo更新已经存在的业务服务的测试版本<see cref="BusID"/> 需要更新到每一个BusServer
        /// </summary>
        AddOrUpdateBusinessTestVersion = 21,
        /// <summary>
        /// 加载默认测试标记 无参数
        /// </summary>
        IinitializeDefaultTestMark = 22,
        /// <summary>
        /// 更新指定BusID测试标记   参数为 <see cref="BusID"/>
        /// </summary>
        AddorUpdateBusStatusAndTestMark = 23,

        /// <summary>
        /// BusServer上线  参数为 <see cref="BusServerID"/>
        /// </summary>
        BusServerOnline = 24,
        /// <summary>
        /// BusServer下线  参数为 <see cref="BusServerID"/>20181011 改为 <see cref="SBusServerRemoveInfo"/>
        /// </summary>
        BusServerOfflie = 25,
        /// <summary>
        /// BusServer当前负载<see cref="SBusServerRates"/>
        /// </summary>
        BusServerRates = 26,
        /// <summary>
        /// 跟踪请求<see cref="STrackConditions"/>
        /// </summary>
        TrackConditions = 27,

        /// <summary>
        /// 跟踪请求返回结果 List  <see cref=" STrackResult"   />  
        /// </summary>
        TrackResult = 28,

        /// <summary>
        ///更新指定BusServer的HTTP服务允许接收的业务<see cref="UpSBusServerHttpBus"/>
        /// </summary>
        UpdateSingleBusServerHTTPBus = 29,
        /// <summary>
        ///BusServer向SLB获取HTTP服务允许接收的业务
        /// </summary>
        GetSingleBusServerHTTPBus = 30,

        /// <summary>
        /// 通知指定BusServer当前在线SLB列表<see cref="SOnLineSLBlistToBusServer"/>
        /// </summary>
        OnLineSLBlistToBusServer = 31,


        /// <summary>
        /// 添加 修改 或者移除APIinfo<see cref="SAPIList"/>
        /// </summary>
        UpdateSingleAPIInfo = 32,
        /// <summary>
        /// 添加 修改 或者移除APIUserinfo<see cref="SAPIUserInfo"/>
        /// </summary>
        UpdateSingleAPIUserInfo = 33,

        /// <summary>
        ///API用户增加或者重新加载访问API权限<see cref="SUpdateAPIUserSaddAPI"/>
        /// </summary>
        UpdateAPIUserSaddAPI = 34,

        /// <summary>
        /// 发送心跳的返回数据
        /// </summary>
        HeartbeatRet = 35,
        /// <summary>
        ///SLB 检测 BusServer 心跳 启动业务服务<see cref="SHeartbeatBusServerInfo"/>
        /// </summary>
        HeartbeatBusServerStartServices = 36,
        /// <summary>
        ///SLB 检测 BusServer 心跳  停止调用其业务服务,但是能接收系统消息<see cref="SHeartbeatBusServerInfo"/>
        /// </summary>
        HeartbeatBusServerStopServices = 37,
        /// <summary>
        /// 更新系统配置表sysinfo中需要及时更新的参数<see cref="SUpdateSysInfo"/>
        /// </summary>
        UpdateSysInfo = 38,
        /// <summary>
        /// 增加BusServer 中WebSocket信息列表 <see cref=" SWebSocketInfo> "/>
        /// </summary>
        AddWebSockeInfo = 39,
        /// <summary>
        ///删除 BusServer WebSocket信息 <see cref="SWebSocketInfo"/>
        /// </summary>
        DelWebSockeInfo = 40,
        /// <summary>
        /// BusServer 向 SLB获取 WebSocket信息列表 ，SLB返回 AddWebSockeInfo 39
        /// </summary>
        GetWebSockeInfoS = 41,
        /// <summary>
        /// 调用 BServerStatus.DLL 并返回对应系统状态数据
        /// </summary>
        ServerStatus = 42,

        /// <summary>
        /// 增加或更新<see cref="SingleBusinessInfoVer"/>
        /// </summary>
        AddOrUpdateSingleBusServerInfoVer = 43,
        /// <summary>
        /// 停止<see cref="SingleBusinessInfoVer"/>
        /// </summary>
        StopSingleBusServerInfoVer = 44,
        /// <summary>
        /// 增加或修改业务<see cref="BusinessInfoVerOnline"/>
        /// </summary>
        AddOrUpdateBusinessInfoVerOnline = 45,

        /// <summary>
        /// BusinessInfo或版本更新<see cref="SingleBusinessInfoVer"/> 仅需要更新到每一个BusServer
        /// </summary>
        UpdateBusinessInfoVer = 46,

        /// <summary>
        /// 移除业务<see cref="SingleBusinessInfoVer"/>
        /// </summary>
        RemoveBusinessInfoVerOnline = 47,

        /// <summary>
        /// 发送系统信息给SLB<see cref="SystemAndProcessInfo"/>
        /// </summary>
        SystemAndProcessInfo = 201,
        /// <summary>
        /// 发送动态信息给SLB<see cref="SystemAndProcessDynamicInfo"/>
        /// </summary>
        SystemAndProcessDynamicInfo = 202,

        /// <summary>
        /// 发送客户端系统信息给监控<see cref="ClientSystemAndProcessInfo"/>
        /// </summary>
        ClientSystemAndProcessInfo = 203,
        /// <summary>
        /// 发送客户端动态信息给给监控<see cref="ClientSystemAndProcessDynamicInfo"/>
        /// </summary>
        ClientSystemAndProcessDynamicInfo = 204,

        /// <summary>
        /// 发送客户端系统信息给监控<see cref="ClientSystemAndProcessInfoClientSLB"/>
        /// </summary>
        ClientSystemAndProcessInfoClinetSLB = 205,
    }

    /// <summary>
    ///全局负载均衡策略GSLBS模式
    /// </summary>
    public enum GSLBSSModel
    {
        /// <summary>
        /// 未赋值
        /// </summary>
        NULL = -1,
        /// <summary>
        /// 由 <see cref="ServerLoadBalancing"/>自行管理 没有集中处理组件，如果只有一个负载均衡器也需设为此
        /// </summary>
        Self_government = 0,
        /// <summary>
        /// 全部由集中处理组件分配 
        /// </summary>
        All = 1,

        /// <summary>
        /// 有限制负载并发量的，由集中处理组件分配，其他自行管理
        /// </summary>
        LimitMaxConcurrent = 2
    }
    /// <summary>
    /// 工作任务种类
    /// </summary>
    public enum BusType
    {
        /// <summary>
        /// 执行并且返回结果,执行者需要继承 <see cref="ProcessingBusinessAsyncResult"/> 
        /// </summary>
        AsyncResult = 0,
        /// <summary>
        ///交互式的 不光是RPC 而且是远程过程控制  执行者需要继承 <see cref="ProcessingBusinessInteractive"/> 
        /// </summary>
        Interactive = 5,
        ///<summary>
        ///提交任务不需要返回结果，执行者需要继承<see cref="ProcessingBusinessNormal"/> 或者<see cref="ProcessingBusinessPublish"/>;
        ///</summary>
        Immediate = 2,
        /// <summary>
        /// 获取数据   执行者需要继承 <see cref="ProcessingBusinessAsyncResult"/> 
        /// </summary>
        GetData = 3,
        /// <summary>
        /// 获取相对不变的基础数据或者静态页面<see cref="ServerLoadBalancing" />会缓存此类结果，   执行者需要继承 <see cref="ProcessingBusinessAsyncResult"/> 
        /// </summary>
        CacheData = 4,

        /// <summary>
        /// 执行二进制并且返回结果,执行者需要继承 <see cref="ProcessingBusinessAsyncResultByte"/> 
        /// </summary>
        AsyncResultByte = 6,
        ///<summary>
        ///执行二进制不需要返回结果，执行者需要继承<see cref="ProcessingBusinessNormalByte"/> 
        ///</summary>
        ImmediateByte = 7,

    }

}
