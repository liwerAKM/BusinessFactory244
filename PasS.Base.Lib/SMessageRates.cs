using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib
{ /// <summary>
  /// 服务器状态
  /// </summary>
    public class SMessageRates
    {
        /// <summary>
        ///连接 客户端数量
        /// </summary>
        public int ClientCount { get; set; }
        /// <summary>
        /// 连接WebSocket数量
        /// </summary>
        public int WebSocketCount { get; set; }
        /// <summary>
        ///客户端到PaaS消息总频率 
        /// </summary>
        public int ClientToPaaSRates { get; set; }
        /// <summary>
        ///PTserver到SLB消息总频率 
        /// </summary>
        public int ServerToSLBRates { get; set; }
        /// <summary>
        /// SLB到PTserver消息总频率 
        /// </summary>
        public int SLBToServerRates { get; set; }
        /// <summary>
        ///  SLB到客户端消息总频率 
        /// </summary>
        public int SLBToClientRates { get; set; }

        /// <summary>
        /// 缓存直接返回客户端
        /// </summary>
        public int SLBCacheToClientRates { get; set; }
        /// <summary>
        /// 未处理直接返回客户端
        /// </summary>
        public int SLBUTToClientRates { get; set; }
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
        /// SLB缓存的数据数量
        /// </summary>
        public int CacheDataCount { get; set; }


        /// <summary>
        /// 等待队列数量
        /// </summary>
        public int WaitTakes { get; set; }
      
        /// <summary>
        /// Http 发送消息到SLB
        /// </summary>
        public int HttpToSLBRates { get; set; }

        /// <summary>
        /// 连接的webSocket终端数量
        /// </summary>
        public int SocketClientCount { get; set; }

        /// <summary>
        /// 连接的webSocket未验证终端数量
        /// </summary>
        public int SocketWaitAuthCount { get; set; }

        /// <summary>
        /// WebSocket 发送消息到SLB
        /// </summary>
        public int WebSocketToSLBRates { get; set; }
        /// <summary>
        ///  SLB发送消息到WebSocket
        /// </summary>
        public int SLBToSLBWebSocketRates { get; set; }


        /// <summary>
        ///  SLB接收到的系统信息
        /// </summary>
        public int SLBSysInofRates { get; set; }
        /// <summary>
        ///  SLB当前线程数
        /// </summary>
        public int Threads { get; set; }


    }
    /// <summary>
    /// SLB当前负载及系统状态
    /// </summary>
    public class SLBCurrentStatus
    {
        public SLBCurrentStatus()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initializeData">初始化数据</param>
        public SLBCurrentStatus(bool initializeData)
        {
            if (initializeData)
            {
                sMessageRates = new SMessageRates();
                cdBusRates = new Dictionary<string, BusRates>();
                SystemandProcessInfo = new SystemAndProcessInfo();
             
            }
        }
        public SMessageRates sMessageRates { get; set; }

        /// <summary>
        /// 业务负载状态 从<see cref="GlobalServerLoadBalancingStrategy"/> 中获取
        ///Key： BusID 
        /// </summary>
        public Dictionary<string, BusRates> cdBusRates { get; set; }

        public SystemAndProcessInfo SystemandProcessInfo { get; set; }

        public string SLBID { get; set; }

        /// <summary>
        ///ClientSLB  公司连接医院的客户端时需要，其他时为空
        /// </summary>
        public string ClientSLBID { get; set; }
        /// <summary>
        /// 医院客户ID，只有公司连接医院的客户端时非空，其他时为空
        /// </summary>
        public string OrgID { get; set; }
        /// <summary>
        /// 连接的BusServerID列表
        /// </summary>
        public List<string> BuServerList { get; set; }
        /// <summary>
        /// 连接的开发测试终端列表
        /// </summary>
        public List<string> BusDevTestList { get; set; }
    }


    public class BusRates
    {
        public BusRates()
        {
            UniqueConcurrentFlagList = new List<string>();
        }
        public BusRates(BusinessInfoOnline busInfoOnline)
        {
            UniqueConcurrentFlagList = new List<string>();
            if (busInfoOnline != null)
            {
                BusID = busInfoOnline.BusID;
                busType = busInfoOnline.busType;
                busDDLBS = busInfoOnline.busDDLBS;
                busStatus = busInfoOnline.busStatus;
                Maxconcurrent = busInfoOnline.Maxconcurrent;
                TheConcurrent = busInfoOnline.TheConcurrent;
                UniqueConcurrentFlagList = new List<string>();
                if (UniqueConcurrentFlagList != null)
                    foreach (string str in busInfoOnline.UniqueConcurrentFlagList)
                    {
                        UniqueConcurrentFlagList.Add(str);
                    }
                AllCount = busInfoOnline.AllCount;
            }
        }

        public string BusID { get; set; }

        /// <summary>
        /// 业务负载均衡策略
        /// </summary>
        public BusDDLBS busDDLBS { get; set; }
        /// <summary>
        /// 业务状态
        /// </summary>
        public BusOnlineStatus busStatus { get; set; }
        /// <summary>
        /// 业务类别
        /// </summary>
        public BusType busType { get; set; }

        /// <summary>
        /// 最大并发量 
        /// </summary>
        public int Maxconcurrent { get; set; }
        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent { get; set; }

        /// <summary>
        /// 唯一并发标记列表
        /// </summary>
        public List<String> UniqueConcurrentFlagList;

        /// <summary>
        /// 总量
        /// </summary>
        public int AllCount { get; set; }
        public int Rates { get; set; }
    }



}
