using System;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 高四位CCN  0-1000  为系统数据 ；  1000  -2000 为服务端主动发给客户端；2000   -3000 客户端主动发给服务端
    /// </summary>
    public class HCCNValye
    {
        /// <summary>
        /// 发送给所有医院 或者客户端标记
        /// </summary>
        public const string SendToALLTag = "A.L_L";
        /// <summary>
        /// 是系统消息1-100000000
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsSysInfo(int HCCN)
        {
            return HCCN >= 0 && HCCN < 1000;

        }
        /// <summary>
        /// 是SLB调用自助机管理端400-499
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsSLBRequestManage(int HCCN)
        {
            return HCCN >= 400 && HCCN < 500;

        }
        /// <summary>
        /// 是自助机管理端调用SLB 500-599
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsManageRequestSLB(int HCCN)
        {
            return HCCN >= 500 && HCCN < 600;

        }



        /// <summary>
        /// 是服务器请求客户端
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsServerRequestClient(int HCCN)
        {
            return HCCN >= 1000 && HCCN < 2000;
        }
        /// <summary>
        /// 客户端请求服务端
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsClientrRequestServer(int HCCN)
        {
            return HCCN >= 2000 && HCCN < 3000;
        }
        /// <summary>
        /// 内部系统信息
        /// </summary>
        public const int SLBSys = 0;

        /// <summary>
        /// OMC通过SLB发送给Client
        /// </summary>
        public const int OMC_SLB_Client = 11;
        /// <summary>  
        ///  OMC通过SLB返回Client
        /// </summary>
        public const int OMC_SLBR_Client = 12;

        /// <summary>
        /// Client通过SLB发送给OMC
        /// </summary>
        public const int Client_SLB_OMC = 13;
        /// <summary>  
        ///  Client通过SLB发返回OMC
        /// </summary>
        public const int Client_SLBR_OMC = 14;
    }


    /// <summary>
    /// 数据类别  0-1000 0000为系统数据 ； 
    /// </summary>
    public class CCNSysValye
    {
        /// <summary>
        /// 判断这些是SLB 接收到直接发给客户的CCN
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public bool SLBSendToClinet(int CCN)
        {
            if (CCN == BusServerToSCToClinet)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是系统消息1-100000000
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsSysInfo(int CCN)
        {
            return CCN > 1 && CCN < 10000000;

        }
        /// <summary>
        /// 是服务器请求客户端
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsServerRequestClient(int CCN)
        {
            return CCN >= 10000000 && CCN < 20000000;
        }
        /// <summary>
        /// 客户端请求服务端 20000000 -30000000
        /// </summary>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public static bool IsClientrRequestServer(int CCN)
        {
            return CCN >= 20000000 && CCN < 30000000;
        }
        /// <summary>
        /// 注册
        /// </summary>
        public const int Register = 1;
        /// <summary>
        /// 系统 交互信息SSysInfo
        /// </summary>
        public const int SysInfo = 10;
        /// <summary>
        /// 获取或释放指定BusName 对应的服务ID<see cref="SGetBusServer"/> 
        /// </summary>
        public const int GetFreeBusServer = 11;

        /// <summary>
        /// 发送连接是否存在的测试<see cref="GUID"/> 
        /// </summary>
        public const int TestConnect = 13;
        /// <summary>
        /// 发送连接是否存在的测试返回<see cref="原GUID"/> 
        /// </summary>
        public const int TestConnectRet = 14;

        /// <summary>
        /// 发送文件,head 为<see cref="FileInfoHead"/>
        /// </summary>
        public const int SendFile = 100;



        /// <summary>
        /// 客户端请求业务(首次)结果:一般情况下 Clinet>SLB>BusServer
        /// </summary>
        public const int ClinetToBusServerResult = 101;
        /// <summary>
        /// 客户端与服务端交互式的非首次数据，不光是RPC 而且是远程过程控制
        /// </summary>
        public const int ClinetandBusServerInteractive = 102;
        /// <summary>
        /// 客户端与服务端交互式的非首次数据，不光是RPC 而且是远程过程控制
        /// </summary>
        public const int ClinetandBusServerInteractiveBack = 103;
        /// <summary>
        ///  Interactive服务端主动发给客户端结束， 不光是RPC 而且是远程过程控制
        /// </summary>
        public const int BusServerToClinetInteractiveEnd = 104;

        ///<summary>
        /// 基础数据发生变更 一般如果BuServer或者TCP客户端有此类基础数据，需要通知到相关程序<see cref="NotifyUpdateInfo"/>
        /// </summary>
        public const int NotifyDataUpdate = 106;

        /// <summary>
        /// BusServer调用其他BusServer   BusServer>SLB>BusServer
        /// </summary>
        public const int BusServerCallOtherBus = 112;

        /// <summary>
        /// BusServer调用其他BusServer返回结果 BusServer>SLB>BusServer
        /// </summary>
        public const int BusServerCallOtherBusBack = 118;

        /// <summary>
        /// BusServer返回结果给客户端 BusServer>SLB>Clinet
        /// </summary>
        public const int BusServerToSCToClinet = 119;



        /// <summary>
        /// BusServer调用WebSocket客户端 BusServer>SLB>WebSocketClinet
        /// 对应调用返回也用此
        /// </summary>
        public const int BusServerToSCToWebSocket = 120;

        /// <summary>
        ///201 BusServer(SLB判断并返回给客户端用的)SLB>Clinet
        /// </summary>
        public const int BusServerOffline = 201;
        /// <summary>
        /// 已经提交BusServer，但BusServer掉线；结果未知(SLB判断并返回给客户端用的)SLB>Clinet
        /// </summary>
        public const int BusServerException = 202;

        /// <summary>
        ///Clinet无法连接到SLB
        /// </summary>
        public const int ClinetConSLBException = 203;




    }
}
