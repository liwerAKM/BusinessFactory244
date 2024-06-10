 
using Newtonsoft.Json;
using PaaS.Comm;
using PasS.Base.Lib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

//////////////////////////////////////////////////////////
/*
 OperationMaintenanceConnect.cs
  Function：管理及监控的连接中心 运行维护TCP连接
    1.连接到<see cref="AccessServer"/> 并获取状态
     通过<see cref="AccessServer"/> 可用的<see cref="ServerLoadBalancing"/>列表
     2.连接到可用的<see cref="ServerLoadBalancing"/>列表 并配置
   3.通过<see cref="ServerLoadBalancing"/> 间接连接 <see cref="SLBBusServer"/> 并进行配置
 Created on:  2018-06-27     
 Original author: 王磊
*/
////////////////////////////////////////////////////////
namespace OMSServer
{ /// <summary>
  /// 显示提示信息
  /// </summary>
  /// <param name="Showinfo"></param>
    public delegate void ShowInfoEventHandler(string Showinfo);
    /// <summary>
    /// 收到SLBRates数据时委派事件
    /// </summary>
    /// <param name="sLBInfo"></param>
    /// <param name="sLBCurrentStatus"></param>
    /// <param name="LoadFactor"></param>
    public delegate void DataSLBRatesEventHandler(SLBInfo sLBInfo, SLBCurrentStatus sLBCurrentStatus, int LoadFactor, bool Online);
  
    
    /// <summary>
    ///  收到BusServerRate数据时委派事件
    /// </summary>
    /// <param name="sBusServerRates"></param>
    /// <param name="Online">在线还是离线</param>
    public delegate void DataBusServerRatesEventHandler(SBusServerRates sBusServerRates,  bool Online);

    /// <summary>
    /// 收到TrackResult数据时委派事件
    /// </summary>
    /// <param name="sBusServerRates"></param>
    public delegate void DataTrackResultEventHandler(List<STrackResult> list);

    /// <summary>
    /// 收到SLB上线下时委派事件
    /// </summary>
    /// <param name="SLBInfo"></param>
    public delegate void SLBInfoUpDownEventHandler(SLBInfo sLBInfo,string Note);
    /// <summary>
    /// AccessServer连接成功或失败提示!
    /// </summary>
    /// <param name="Status">0 连接失败； 1连接成功；2 多次连接失败，3 失败后重连成功</param>
    /// <param name="Note"></param>
    public delegate void AccessServerConnctEventHandler(int Status,string Note);

    /// <summary>
    /// /SLB通过心跳检测BusServer服务器停启业务服务消息
    /// </summary>
    /// <param name="sLBInfo"></param>
    /// <param name="sHeartbeatBusServerInfo"></param>
    public delegate void HeartbeatBusServerInfoEventHandler(SLBInfo sLBInfo, SHeartbeatBusServerInfo sHeartbeatBusServerInfo);



    /// 
    /// <summary>
    ///  收到 BusServer上线下时委派事件
    /// </summary>
    /// <param name="fromsLBInfo">发送消息的SLB</param>
    /// <param name="BusServerID"></param>
    /// <param name="Status">0 下线； 1上线</param>
    public delegate void BusServerUpDownEventHandler(SLBInfo fromsLBInfo, string BusServerID, int Status, string Note);

    /// <summary>
    ///其他系统消息
    /// </summary>
    /// <param name="webSocket4Net"></param>
    /// <param name="sysInfo"></param>
    
    public delegate void OtherSysINofEventHandler( WebSocket4NetSpring webSocket4Net ,SSysInfo sysInfo);
    /// <summary>
    /// 运行维护TCP连接
    /// 1.连接到<see cref="AccessServer"/> 并获取状态
    /// 通过<see cref="AccessServer"/> 获取可用的<see cref="ServerLoadBalancing"/>列表
    /// 2.连接到可用的<see cref="ServerLoadBalancing"/>列表 并配置
    /// 3.通过<see cref="ServerLoadBalancing"/> 间接连接 <see cref="SLBBusServer"/> 并进行配置
    /// </summary>
    public class OperationMaintenanceConnect
    {
        /// <summary>
        /// 是否是监控服务，对于监控服务,系统会对<see cref="AccessServer"/>、<see cref="ServerLoadBalancing"/>等采取重连策略,以最大可能保证能连接到各系统
        /// </summary>
        bool isMonitorService = false;

        //定义Timer类
        System.Threading.Timer threadTimer;


        public OperationMaintenanceConnect()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MonitorService">是否是监控服务，对于监控服务,系统会对<see cref="AccessServer"/>、<see cref="ServerLoadBalancing"/>等采取重连策略,以最大可能保证能连接到各系统</param>
        public OperationMaintenanceConnect(bool MonitorService)
        {
            isMonitorService = MonitorService;
            if (MonitorService)
            {
                threadTimer = new System.Threading.Timer(new TimerCallback(TimerUpTestSLB), null, 6000, 6000);
            }
        }
        Encoding Encoding = Encoding.UTF8;
        /// <summary>
        /// 连接到<see cref="AccessServer"/>的Tcp
        /// </summary>
        WebSocket4NetSpring AccessServerTcpClient = null;
        int AccessServerConnectTime = 0;

        /// <summary>
        /// 负载均衡服务TCP连接列表
        /// SLBID SSLBLoadInfo
        /// </summary>
        ConcurrentDictionary<string, SSLBLoadInfo> cdSLB = new ConcurrentDictionary<string, SSLBLoadInfo>();

        /// <summary>
        /// 等待重新尝试连接的SLBInfo
        /// Key:SLBID <see cref="SSLBLoadInfo"/>
        /// </summary>
        ConcurrentDictionary<string, SSLBLoadInfo> cdWaitReConnectSLBInfo = new ConcurrentDictionary<string, SSLBLoadInfo>();
        /// <summary>
        ///  收到SLBRates数据时委派事件
        /// </summary>
        public event DataSLBRatesEventHandler DataSLBRatesEvent;
        /// <summary>
        ///  收到BusServerRate数据时委派事件
        /// </summary>
        public event DataBusServerRatesEventHandler DataBusServerRatesEvent;
        /// <summary>
        ///  收到TrackResult数据时委派事件
        /// </summary>
        public event DataTrackResultEventHandler DataTrackResultEvent;
        /// <summary>
        /// 收到SLB上线下时委派事件
        /// </summary>
        public event SLBInfoUpDownEventHandler SLBInfoUpDownEvent;
        /// <summary>
        ///  收到 BusServer上线下时委派事件
        /// </summary>
        public event BusServerUpDownEventHandler BusServerUpDownEvent;
        /// <summary>
        ///  AccessServer连接成功或者失败通知 0 连接失败； 1连接成功；2 多次连接失败，3 失败后重连成功
        /// </summary>
        public event AccessServerConnctEventHandler AccessServerConnctEven;
        /// <summary>
        /// 其他系统消息
        /// </summary>
        public event OtherSysINofEventHandler OtherSysINofEvent;

        public event HeartbeatBusServerInfoEventHandler HeartbeatBusServerInfoEvent;
        public event ShowInfoEventHandler ShowInfoEvent ;

        /// <summary>
        /// 收到 控制端和自助机之间通讯
        /// </summary>
        public event ReciveOMC_and_ClientEventHandler ReciveOMC_and_ClientEvent;

        /// <summary>
        /// 定时到点执行的事件
        ///  尝试对数据库中的SLB Status为1 的 进行连接；
        ///  检测当前在线的负载均衡状态，对掉线的 剔除；
        /// </summary>
        /// <param name="value"></param>
        private void TimerUpTestSLB(object value)
        {
            try
            {
                List<SLBInfo> listslb = DbHelper.SLBInfoListGet("Status>=0  and OrgID='"+ MyPubConstant.OMSMainOrgID + "' ");
                if (listslb != null && listslb.Count > 0)
                {
                    //尝试对数据库中的SLB Status为1 的 进行连接
                    //如果连接成功则加入负载均衡表cdSLB

                    foreach (SLBInfo slb in listslb)
                    {
                        if (slb.Status == 1)
                        {
                            if (!cdSLB.ContainsKey(slb.SLBID))
                            {
                                ConnectSLB(slb);
                               
                            }
                        }
                    }


                    SSysInfo sSysInfo = new SSysInfo(SysInfoType.TestConnect);
                    sSysInfo.Info = "TestConnect";
                    try
                    {
                        if (AccessServerTcpClient != null && AccessServerTcpClient.Connected)
                        {
                            AccessServerTcpClient.SendSysInfo(sSysInfo);

                        }
                        else
                        {
                            if (AccessServerTcpClient != null)
                            {
                                AccessServerConnctEven(0, "连接关闭");
                            }
                            ConnectAccessServerTcpClient();
                        }
                    }
                    catch (Exception ex)
                    {
                        AccessServerConnctEven(0, "发送测试失败");
                        try
                        {
                            AccessServerTcpClient.Close();
                            AccessServerTcpClient = null;
                        }
                        catch (Exception ewx)
                        {
                        }
                        ConnectAccessServerTcpClient();
                    }

                    //检测当前在线的负载均衡状态，对掉线的 剔除
                    foreach (string key in cdSLB.Keys)
                    {
                        bool isReconnect = false;//是否已经进行重新连接
                        SSLBLoadInfo sSLBLoadInfo = cdSLB[key];

                        try
                        {
                            try
                            {
                                if (sSLBLoadInfo.SLBLTcpClient.Connected)
                                {//add by  wanglei 20190307 对于能连上SLB但SLB 没有发送状态信息过来的SLB ，此时可能已经假死 无法提供服务，需要关闭重新连接，（此种情况问题最大，可能客户端认为SLB是好的 还一直在请求）

                                    int Seconds = (DateTime.Now - sSLBLoadInfo.LastReciveTime).Seconds;
                                    if ((Seconds >= 60 && Seconds < 80) || (Seconds >= 300 && Seconds % 300 < 8))
                                    {
                                        sSLBLoadInfo.SLBLTcpClient.Close();
                                        SLBInfo sLBInfo = new SLBInfo(sSLBLoadInfo.sLBInfo);
                                        sLBInfo.Status = 0;
                                        SLBInfoUpDownEvent(sLBInfo, "SLB可能出现假死现象，请立即查看！" + Seconds.ToString());
                                    }
                                }
                                if (sSLBLoadInfo.SLBLTcpClient.Connected)
                                {
                                    sSLBLoadInfo.SLBLTcpClient.SendSysInfo(sSysInfo);
                                    //成功 清除等待连接列表中的此SLB
                                    if (cdWaitReConnectSLBInfo.ContainsKey(key))
                                    {
                                        SSLBLoadInfo sLBInfot;
                                        cdWaitReConnectSLBInfo.TryRemove(key, out sLBInfot);
                                    }
                                    string showinfo = string.Format("{0} Test connect to SLB {1} {2}:{3} sucess", DateTime.Now, sSLBLoadInfo.sLBInfo.SLBName, sSLBLoadInfo.sLBInfo.IP, sSLBLoadInfo.sLBInfo.Port);
                                    PasSLog.Info("OperationMaintenanceConnect.TimerUpTestSLB", showinfo);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(showinfo);
                                }
                                else
                                {//连接已经关闭，重新连接
                                    Console.WriteLine(key + "Connected = false ,try isReconnect...");
                                    isReconnect = true;
                                  //  sSLBLoadInfo.SLBLTcpClient.ReCreateTcpClient();
                                    sSLBLoadInfo.SLBLTcpClient.ConnecSLBBusServer();
                                    int WaitCount = 0;
                                    while (!sSLBLoadInfo.SLBLTcpClient.Connected && WaitCount < 100)
                                    {
                                        WaitCount++;
                                        Thread.Sleep(10);
                                    }
                                    if (sSLBLoadInfo.SLBLTcpClient.Connected)
                                    {
                                        SSysRegisterInfo sSysRegisterInfo = new SSysRegisterInfo(RegisterIdentity.OperationMaintenance, MyPubConstant.OperationMaintenanceID);
                                        sSysRegisterInfo.Info = "isReRegister";
                                        sSLBLoadInfo.SLBLTcpClient.SysRegister(sSysRegisterInfo);
                                        //成功 清除等待连接列表中的此SLB
                                        if (cdWaitReConnectSLBInfo.ContainsKey(key))
                                        {
                                            SSLBLoadInfo sLBInfot;
                                            cdWaitReConnectSLBInfo.TryRemove(key, out sLBInfot);
                                        }
                                    }
                                    else
                                    {
                                        cdSLB.TryRemove(key, out sSLBLoadInfo);
                                        cdWaitReConnectSLBInfo.TryAdd(key, sSLBLoadInfo);
                                        sSLBLoadInfo.SLBLTcpClient.Close();

                                        string Message = string.Format("{0} TestSLB:  SLB  {1} Offline, Delete!", DateTime.Now, key);

                                        Message = string.Format("  At present {0} SLB Connected： ", cdSLB.Count);
                                        foreach (string Key2 in cdSLB.Keys)
                                        {
                                            Message += Key2 + " ";
                                        }
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine(Message);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        PasSLog.Info("OperationMaintenanceConnect.TimerUpTestSLB", Message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    // string SLBID = cdSLB[Key].ID;
                                    string showinfo = string.Format("{0} Cannot connect to  SLB  {1}{2} !", DateTime.Now, key, ex.ToString());
                                    Console.ForegroundColor = ConsoleColor.Red;

                                    Console.WriteLine(showinfo);
                                    PasSLog.Error("OperationMaintenanceConnect.TimerUpTestSLB", showinfo + ex.ToString());
                                    if (!sSLBLoadInfo.SLBLTcpClient.Connected && isReconnect)
                                    {//重新尝试连接仍然不成功的，暂时关闭从cdSLB中清除并加入等待重新连接cdWaitReConnectSLBInfo中

                                        cdSLB.TryRemove(key, out sSLBLoadInfo);
                                        cdWaitReConnectSLBInfo.TryAdd(key, sSLBLoadInfo);
                                        sSLBLoadInfo.SLBLTcpClient.Close();
                                        string Message = string.Format("{0} TestSLB:  SLB  {1} Offline, Delete!", DateTime.Now, key);

                                        Message = string.Format("  At present {0} SLB Connected： ", cdSLB.Count);
                                        foreach (string Key2 in cdSLB.Keys)
                                        {
                                            Message += Key2 + " ";
                                        }
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine(Message);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        PasSLog.Info("OperationMaintenanceConnect.TimerUpTestSLB", Message);
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    Console.WriteLine(ex2.ToString());
                                    PasSLog.Error("OperationMaintenanceConnect.TimerUpTestSLBex2", ex2.ToString());
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            PasSLog.Error("OperationMaintenanceConnect.TimerUpTestSLB", ex.ToString());
                        }

                    }

                    //对中断连接的服务器重新尝试连接
                    foreach (string Key in cdWaitReConnectSLBInfo.Keys)
                    {
                        cdWaitReConnectSLBInfo[Key].RetryConnectCount++;
                        if (ConnectSLB(cdWaitReConnectSLBInfo[Key].sLBInfo))
                        {

                        }
                        else if (cdWaitReConnectSLBInfo[Key].RetryConnectCount == MyPubConstant.RetryConnectCount())
                        {
                            //  SSLBLoadInfo sSLBLoadInfo;
                            //   cdWaitReConnectSLBInfo.TryRemove(Key, out sSLBLoadInfo);
                            SLBInfo sLBInfo = new SLBInfo(cdWaitReConnectSLBInfo[Key].sLBInfo);
                            sLBInfo.Status = 0;
                            SLBInfoUpDownEvent(sLBInfo, "From OMC");
                            if (cdSLB.ContainsKey(Key))
                            {
                                SSLBLoadInfo sSLBLoadInfo2;
                                cdSLB.TryRemove(Key, out sSLBLoadInfo2);
                            }
                            //     sSLBLoadInfo = null;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                PasSLog.Error("AccessServer.TimerUp", ex.Message);
            }
            System.GC.Collect();
        }

        /// <summary>
        /// 连接到<see cref="AccessServer"/>并接收其消息
        /// </summary>
        /// <returns></returns>
        public bool ConnectAccessServerTcpClient()
        {/*
            AccessServerConnectTime++;
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(MyPubConstant.GetAccessServerIP()), MyPubConstant.GetAccessServerPort());
            AccessServerTcpClient = new AsyncTcpClientAssign(remoteEP);
            AccessServerTcpClient.DatasSysInfoEvent += AccessServerTcpClientt_DatasSysInfoEvent;
            AccessServerTcpClient.ConnecSLBBusServer();
            int count = 0;
            if (!AccessServerTcpClient.Connected && count < 20)
            {
                Thread.Sleep(50);
                count++;
            }
            if (AccessServerTcpClient.Connected)
            {
                AccessServerConnctEven(1, "连接成功");
                AccessServerTcpClient.SysRegister(new SSysRegisterInfo(SSysRegisterInfo.RegisterIdentity.OperationMaintenance, MyPubConstant.OperationMaintenanceID));
                if (AccessServerConnectTime > MyPubConstant.RetryConnectCount() && isMonitorService)
                {//重新连接成功通知
                    AccessServerConnctEven(3, "重新连接成功");
                }

                AccessServerConnectTime = 0;
                return true;
            }
            else
            {
                if (AccessServerConnectTime == MyPubConstant.RetryConnectCount())
                {//重新连接超过设定次数
                    AccessServerConnctEven(2, "重新连接超过设定次数");
                }
                AccessServerTcpClient = null;
                return false;
            }
            */
            return true;
        }

        /// <summary>
        /// 接收<see cref="AccessServer"/>消息
        /// </summary>
        /// <param name="Tcp_Client"></param>
        /// <param name="sSysInfo"></param>
        private void AccessServerTcpClientt_DatasSysInfoEvent(TcpClient Tcp_Client, SSysInfo sSysInfo)
        {
            if (sSysInfo.InfoType == SysInfoType.OtherSLBChnage)
            {//   SLB 上下线
                SLBInfo sLBInfo = sSysInfo.GetInfo<SLBInfo>();
                if (sLBInfo.Status == 1)
                {
                    ConnectSLB(sLBInfo);
                }
                else if (sLBInfo.Status == 0)
                {
                    RemoveSLBServer(sLBInfo);
                    DataSLBRatesEvent?.Invoke(new SLBInfo(sLBInfo), null, 0, false);
                }
                if (SLBInfoUpDownEvent != null)
                {
                    SLBInfoUpDownEvent(sLBInfo, "From AccessServer");
                }

            }
        }

        /// <summary>
        /// 连接<see cref="ServerLoadBalancing"/>负载均衡服务器 并接收其消息
        /// </summary>
        /// <param name="remoteEP"></param>
        /// <returns></returns>
        public bool ConnectSLB(SLBInfo sLBInfo)
        {

            string WebSocketUrl = $"ws://{sLBInfo.IP }:{sLBInfo.WebSocketPort  }";
            string SLBBusSocketUrl = "";
            if (!string.IsNullOrEmpty(MyPubConstant.SLBBusSocketUrl))
            {
                SLBBusSocketUrl = MyPubConstant.SLBBusSocketUrl;
            }
            if (!string.IsNullOrEmpty(SLBBusSocketUrl))
            {
                WebSocketUrl = SLBBusSocketUrl + '/' + sLBInfo.WSPath;
            }



            WebSocket4NetSpring webSocket4NetSpring = new WebSocket4NetSpring(WebSocketUrl, sLBInfo.SLBID);

            webSocket4NetSpring.DataInteriorSysInfoEvent += WebSocket4NetSpring_DataInteriorSysInfoEvent;
            webSocket4NetSpring.ReciveOMC_and_ClientEvent += WebSocket4NetSpring_ReciveOMC_and_ClientEvent;
            SSysRegisterInfo sSysRegisterInfo = new SSysRegisterInfo(RegisterIdentity.OperationMaintenance, MyPubConstant.OperationMaintenanceID);
            sSysRegisterInfo.Info = "OperationMaintenance";
            if (webSocket4NetSpring.ConnecSLBBusServer())
            {
                webSocket4NetSpring.Login(sSysRegisterInfo);

                SSLBLoadInfo sSLBLoadInfo = new SSLBLoadInfo(webSocket4NetSpring, sLBInfo);

                cdSLB.TryAdd(sLBInfo.SLBID, sSLBLoadInfo);
                ShowInfoEvent?.Invoke($"连接{sLBInfo.SLBID}成功，地址为{WebSocketUrl}");
                return true;
            }
            return webSocket4NetSpring.Connected;


        }

        private void WebSocket4NetSpring_ReciveOMC_and_ClientEvent(WebSocket4NetSpring webSocket4NetClent, int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            ReciveOMC_and_ClientEvent.Invoke(webSocket4NetClent, CCN, Head, bInfo);
        }



        // <summary>
        /// 移除负载均衡服务器
        /// </summary>
        /// <param name="remoteEP"></param>
        /// <returns></returns>
        public bool RemoveSLBServer(SLBInfo sLBInfo)
        {
            string strKey = sLBInfo.IP + ":" + sLBInfo.Port.ToString();
            SSLBLoadInfo sSLBLoadInfo;
            if (cdSLB.ContainsKey(sLBInfo.SLBID))
            {
                cdSLB.TryRemove(sLBInfo.SLBID, out sSLBLoadInfo);
                if (sSLBLoadInfo != null && sSLBLoadInfo.SLBLTcpClient != null)
                    sSLBLoadInfo.SLBLTcpClient.Close();
                sSLBLoadInfo = null;
                DataSLBRatesEvent?.Invoke(new SLBInfo(sLBInfo), null, 0, false);
                return true;
            }
            return false;
        }


        

        /// <summary>
        ///接收 SLB 发过来的数据
        /// </summary>
        /// <param name="SLBTcp"></param>
        /// <param name="sSysInfo"></param>
        private void WebSocket4NetSpring_DataInteriorSysInfoEvent(WebSocket4NetSpring SLBTcp, OMCandZZJInfoHead head,SSysInfo sSysInfo)
        {
            IPEndPoint iPEndPoint = (IPEndPoint)SLBTcp.EndPoint;
            string strIP_Port = iPEndPoint.Address.ToString() + ":" + iPEndPoint.Port.ToString();
            SSLBLoadInfo sSLBLoadInfthis = null;

            if (cdSLB.ContainsKey(SLBTcp.SLBID))
            {
                sSLBLoadInfthis = cdSLB[SLBTcp.SLBID];
            }
            //if (sSLBLoadInfthis == null)
            //{
            //  //  return;
            //}
            if (sSysInfo.InfoType == SysInfoType.SLBOffline)
            {
                if (sSysInfo.Info.ToLower() == "SLBOffline".ToLower())
                {
                    if (sSLBLoadInfthis != null)
                    {
                        string SLBID = sSLBLoadInfthis.sLBInfo.SLBID;
                        cdSLB.TryRemove(SLBID, out sSLBLoadInfthis);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(string.Format("{2}SLB   {0} {1} Offline by SysInfo", sSLBLoadInfthis.SLBLTcpClient.ID, strIP_Port, DateTime.Now));

                        DataSLBRatesEvent?.Invoke(new SLBInfo(sSLBLoadInfthis.sLBInfo), null, 0, false);
                        if (SLBInfoUpDownEvent != null)
                        {
                            SLBInfo sLBInfo = new SLBInfo(sSLBLoadInfthis.sLBInfo);
                            sLBInfo.Status = 0;
                            SLBInfoUpDownEvent(sLBInfo, "From SLB Self");
                        }
                        sSLBLoadInfthis = null;
                    }
                }
            }
            else if (sSysInfo.InfoType == SysInfoType.SLBRatesStatus)
            {
                SLBCurrentStatus sLBCurrentStatus = sSysInfo.GetInfo<SLBCurrentStatus>();
                if (sSLBLoadInfthis != null)
                {
                    sSLBLoadInfthis.sLBCurrentStatus = sLBCurrentStatus;
                    sSLBLoadInfthis.LastReciveTime = DateTime.Now;
                }
                SMessageRates sMessageRates = sLBCurrentStatus.sMessageRates;

                int LoadFactor =    sMessageRates.ClientToPaaSRates + sMessageRates.SLBToClientRates + sMessageRates.SLBToServerRates
                 + sMessageRates.ServerCallOtherToSLBRates;
                if (sSLBLoadInfthis != null)
                {
                    sSLBLoadInfthis.LoadFactorRec += LoadFactor;
                }
                try
                {
                    if (!string.IsNullOrEmpty(sLBCurrentStatus.OrgID)&&! string.IsNullOrEmpty(sLBCurrentStatus.ClientSLBID))
                    {
                        SLBInfo sLBInfo = new SLBInfo();
                        sLBInfo.SLBID = sLBCurrentStatus.ClientSLBID;
                        sLBInfo.SLBName = "客户SLB：" + sLBCurrentStatus.ClientSLBID ;
                        DataSLBRatesEvent?.Invoke(sLBInfo, sLBCurrentStatus, LoadFactor, true);
                    }
                    else if(sSLBLoadInfthis!=null )
                    {
                        DataSLBRatesEvent?.Invoke(new SLBInfo(sSLBLoadInfthis.sLBInfo), sLBCurrentStatus, LoadFactor, true);
                       
                    }
                }
                catch
                { }
            }
            else if (sSysInfo.InfoType == SysInfoType.BusServerRates)
            {
                SBusServerRates sBusServerRates = sSysInfo.GetInfo<SBusServerRates>();
                try
                {
                    DataBusServerRatesEvent?.Invoke(sBusServerRates, true);
                }
                catch
                { }
            }
            else if (sSysInfo.InfoType == SysInfoType.TrackResult)
            {
                List<STrackResult> list = sSysInfo.GetInfo<List<STrackResult>>();
                try
                {
                    DataTrackResultEvent?.Invoke(list);
                }
                catch
                { }
            }
            else if (sSysInfo.InfoType == SysInfoType.BusServerOnline || sSysInfo.InfoType == SysInfoType.BusServerOfflie)
            {
                string BusServerID = sSysInfo.Info;
                string Note = "";
                if (sSysInfo.InfoType == SysInfoType.BusServerOfflie)
                {
                    try
                    {
                        SBusServerRemoveInfo sBusServerRemoveInfo = JsonConvert.DeserializeObject<SBusServerRemoveInfo>(sSysInfo.Info);
                        BusServerID = sBusServerRemoveInfo.BusServerID;
                        Note = sBusServerRemoveInfo.Note + (sBusServerRemoveInfo.ReStart ? ",稍后重启" : "");
                    }
                    catch
                    {
                        Note = "未知原因！";
                    }
                }
                int Status = sSysInfo.InfoType == SysInfoType.BusServerOnline ? 1 : 0;
                try
                {
                    if (sSysInfo.InfoType == SysInfoType.BusServerOfflie)
                    {
                        SBusServerRates sBusServerRates = new SBusServerRates();
                        sBusServerRates.BusServerID = BusServerID;
                        DataBusServerRatesEvent?.Invoke(sBusServerRates, false);
                    }
                    BusServerUpDownEvent?.Invoke(new SLBInfo(sSLBLoadInfthis.sLBInfo), BusServerID, Status, Note);
                }
                catch
                { }
            }
            else if (sSysInfo.InfoType == SysInfoType.HeartbeatBusServerStartServices || sSysInfo.InfoType == SysInfoType.HeartbeatBusServerStopServices)
            {

                try
                {
                    SHeartbeatBusServerInfo sHeartbeatBusServerInfo = sSysInfo.GetInfo<SHeartbeatBusServerInfo>();
                    HeartbeatBusServerInfoEvent?.Invoke(new SLBInfo(sSLBLoadInfthis.sLBInfo), sHeartbeatBusServerInfo);
                }
                catch
                { }
            }
            else  
            {
                OtherSysINofEvent?.Invoke(SLBTcp, sSysInfo);
            }
        }


        /// <summary>
        /// 通过已经上线任意SLB发送
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool SendSSysInfo(SSysInfo sSysInfo)
        {
            SSLBLoadInfo sSLBLoadInfoOnline = null;
            if (cdSLB.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(0, cdSLB.Count - 1);
                sSLBLoadInfoOnline = cdSLB.Values.ElementAt(index);
                if (!sSLBLoadInfoOnline.SLBLTcpClient.Connected)
                {
                    if (cdSLB.Count > 1)
                    {
                        if (index == cdSLB.Count - 1)
                        {
                            index = 0;
                        }
                        else
                        {
                            index++;
                        }
                        sSLBLoadInfoOnline = cdSLB.Values.ElementAt(index);
                    }
                }
            }

            if (sSLBLoadInfoOnline != null)
            {
                //通过已经上线SLB发送
                sSLBLoadInfoOnline.SLBLTcpClient.SendSysInfo(sSysInfo);
                return true;
            }
            return false;
        }
        /// <summary>
        ///  上线所有SLB发送
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool SendSSysInfoToAllSLB(SSysInfo sSysInfo,OMCandZZJInfoHead head =null )
        {
            foreach (SSLBLoadInfo slbload in cdSLB.Values)
            {
                try
                {
                    if (slbload.SLBLTcpClient.Connected)
                        slbload.SLBLTcpClient.SendSysInfo(sSysInfo, head);
                }
                catch (Exception ex)
                {
                }
            }
            return true;
        }
        /// <summary>
        ///  上线所有指定发送
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool SendSSysInfoToSLB(string SLBID,SSysInfo sSysInfo, OMCandZZJInfoHead head = null )
        {
            if (cdSLB.ContainsKey(SLBID))
                return cdSLB[SLBID].SLBLTcpClient.SendSysInfo(sSysInfo, head);
            return true;
        }
        /// <summary>
        /// 通过已经上线任意SLB发送
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <param name="SLBList">优先SLB列表</param>
        /// <returns></returns>
        public bool SendSSysInfo(SSysInfo sSysInfo, List<string> SLBList)
        {
            SSLBLoadInfo sSLBLoadInfoOnline = null;
            foreach (string SLBID in SLBList)
            {
                if (cdSLB.ContainsKey(SLBID) && cdSLB[SLBID].SLBLTcpClient.Connected)
                {
                    sSLBLoadInfoOnline = cdSLB[SLBID];
                    break;
                }
            }


            if (sSLBLoadInfoOnline != null)
            {
                //通过已经上线SLB发送
                sSLBLoadInfoOnline.SLBLTcpClient.SendSysInfo(sSysInfo);
                return true;
            }
            return false;
        }



        //-----------------------------------------------------------------


        /// <summary>
        /// 通知指定的<see cref="ServerLoadBalancing"/>下线
        /// </summary>
        /// <param name="SLBID"></param>
        /// <param name="ReStart">是否重启</param>
        /// <returns></returns>
        public bool SLBOffline(string SLBID, bool ReStart,OMCandZZJInfoHead head=null )
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.SLBOffline);
            
            sSysInfo.Info = "";
            if (ReStart)
            {
                sSysInfo.Info = "ReStart";
            }
            if (cdSLB.ContainsKey(SLBID))
                return cdSLB[SLBID].SLBLTcpClient.SendSysInfo(sSysInfo, head);
            return false;

        }


        /// <summary>
        /// 新增或者更新一个在线BusServer信息<see cref="BusServerInfo"/>
        /// </summary>
        /// <param name="busServerInfo"></param>
        /// <returns></returns>
        public bool AddOrUpdateBusServer(BusServerInfo busServerInfo)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.AddOrUpdateBusServer);
            sSysInfo.SetInfo(busServerInfo);
            return SendSSysInfo(sSysInfo);
        }

        /// <summary>
        /// 彻底移除当前服务
        /// </summary>
        /// <param name="busServerInfo"></param>
        /// <returns></returns>
        public bool RemoveBusServer(SBusServerRemoveInfo sBusServerRemoveInfo)
        {
            //SBusServerRemoveInfo sBusServerRemoveInfo = new SBusServerRemoveInfo();
            //sBusServerRemoveInfo.BusServerIID = MyPubConstant.BusServerID;
            //sBusServerRemoveInfo.Alreadyoffline = false;
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.RemoveBusServer);
            sSysInfo.SetInfo(sBusServerRemoveInfo);
            return SendSSysInfo(sSysInfo);
        }

        /// <summary>
        /// 开始接收新服务
        /// </summary>
        /// <returns></returns>
        public bool StartReceivingService(string BusServerID)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.BusServerStartServices);
            sSysInfo.Info = BusServerID;
            return SendSSysInfo(sSysInfo);
        }

        /// <summary>
        /// 停止接收新服务
        /// </summary>
        /// <returns></returns>
        public bool StopReceivingService(string BusServerID)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.BusServerStopServices);
            sSysInfo.Info = BusServerID;
            return SendSSysInfo(sSysInfo);
        }
        /// <summary>
        /// 彻底移除一个在线一个BusServer信息<see cref="SBusServerRemoveInfo"/>,
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <param name="Alreadyoffline"></param>
        /// <param name="ReStart">是否自动重启，如果要不自动启动只能到对用服务器手动启动</param>
        /// <returns></returns>
        public bool RemoveBusServer(string BusServerID, bool Alreadyoffline, bool ReStart)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.RemoveBusServer);
            SBusServerRemoveInfo sBusServerRemoveInfo = new SBusServerRemoveInfo();
            sBusServerRemoveInfo.BusServerID = BusServerID;
            sBusServerRemoveInfo.Alreadyoffline = Alreadyoffline;
            sBusServerRemoveInfo.ReStart = ReStart;
            sBusServerRemoveInfo.Note = MyPubConstant.OperationMaintenanceID;
            sSysInfo.SetInfo(sBusServerRemoveInfo);
            return SendSSysInfo(sSysInfo);
            // return SendSSysInfoToAllSLB(sSysInfo);
        }


        /// <summary>
        /// 添加或者更新此单个BusServer上的单个业务BusName
        /// </summary>
        /// <param name="singleServerTakeInfo"></param>
        /// <returns></returns>
        public bool AddOrUpdateSingleBusBusiness(SingleBusServerInfo singleBusServerInfo)
        {
            SSysInfo sSysInfo = new SSysInfo();
            sSysInfo.InfoType = SysInfoType.AddOrUpdateSingleBusServerInfo;
            sSysInfo.SetInfo(singleBusServerInfo);
            return SendSSysInfoToAllSLB(sSysInfo);
        }




        /// <summary>
        /// 启动所有BusServer的服务 （建议只有在 Businesss 由offline变为publish时调用）
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool AddAllSingleBusBusiness(string BusID)
        { //启动所有BusServer的服务
            DataTable dataTable = DbHelper.BusServerInfoByBusID(BusID, true);
            foreach (DataRow dr in dataTable.Rows)
            {
                SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
                singleBusServerInfo.BusServerID = dr["BusServerID"].ToString();
                singleBusServerInfo.BusID = BusID;
                if (singleBusServerInfo != null)
                {
                    AddOrUpdateSingleBusBusiness(singleBusServerInfo);
                }
                DbHelper.BusServerBusInfoStatus(BusID, true);
            }
            return true;
        }

        /// <summary>
        /// 移除单个BusServer上的单个业务BusName
        /// </summary>
        /// <param name="singleBusServerInfo"></param>
        /// <returns></returns>
        public bool RemoveSingleBusBusiness(SingleBusServerInfo singleBusServerInfo)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.RemoveSingleBusServerInfo);
            sSysInfo.SetInfo(singleBusServerInfo);
            return SendSSysInfoToAllSLB(sSysInfo);
        }

        /// <summary>
        /// BusinessInfo 更新或版本更新<see cref="BusID"/> 需要更新到每一个BusServer
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool UpdateBusServerBusinessOrVersion(string BusID)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.UpdateBusinessInfo);
            sSysInfo.Info = BusID;
            return SendSSysInfoToAllSLB(sSysInfo);
        }

        /// <summary>
        /// BusinessInfo配置内容更新<see cref="BusID"/> 需要更新到每一个SLB
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool UpdateBusBusinessInfo(BusinessInfo businessInfo, BusStatus busStatus)
        {
            SSysInfo sSysInfo;
            if (busStatus == BusStatus.Offline)
            {
                //停止服务所有BusServer的服务
                DataTable dataTable = DbHelper.BusServerIDByBusID(businessInfo.BusID, 1);
                foreach (DataRow dr in dataTable.Rows)
                {
                    SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
                    singleBusServerInfo.BusServerID = dr["BusServerID"].ToString();
                    singleBusServerInfo.BusID = businessInfo.BusID;
                    if (singleBusServerInfo != null)
                    {
                        RemoveSingleBusBusiness(singleBusServerInfo);
                    }
                }
                DbHelper.BusServerBusInfoStatus(businessInfo.BusID, false);
              
                sSysInfo = new SSysInfo(SysInfoType.RemoveBusinessInfoOnline);
                sSysInfo.Info = businessInfo.BusID;
                SendSSysInfoToAllSLB(sSysInfo);
            }
            else
            {
                BusinessInfoOnline BusInfoOnline = new BusinessInfoOnline(businessInfo);
                if (busStatus == BusStatus.Publish)
                {
                    BusInfoOnline.busStatus = BusOnlineStatus.READY;
                    sSysInfo = new SSysInfo(SysInfoType.AddOrUpdateBusinessInfoOnline);
                    sSysInfo.SetInfo(BusInfoOnline);
                    SendSSysInfoToAllSLB(sSysInfo);

                    DataTable dataTable = DbHelper.BusServerInfoByBusID(businessInfo.BusID, -2);
                    if (dataTable.Rows.Count > 0)//有业务服务器
                    {
                        List<BusinessInfoBusVersion> listver = DbHelper.BusinessInfoVerListGet(string.Format(" busid ='{0}' and status >0 ", businessInfo.BusID));
                        foreach (BusinessInfoBusVersion busver in listver)
                        {
                            BusinessInfoVerOnline businessInfoVer = new BusinessInfoVerOnline(busver);
                            SSysInfo sSysInfo1 = new SSysInfo(SysInfoType.AddOrUpdateBusinessInfoVerOnline);
                            sSysInfo1.SetInfo(businessInfoVer);
                            SendSSysInfoToAllSLB(sSysInfo1);
                        }
                    }

                    
                    //启动所有因业务停止触发的停止的BusServer服务
                    DataTable dataTable0 = DbHelper.BusServerIDByBusID(businessInfo.BusID, -2);
                    foreach (DataRow dr in dataTable0.Rows)
                    {
                        SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
                        singleBusServerInfo.BusServerID = dr["BusServerID"].ToString();
                        singleBusServerInfo.BusID = businessInfo.BusID;
                        if (singleBusServerInfo != null)
                        {
                            AddOrUpdateSingleBusBusiness(singleBusServerInfo);
                        }
                    }

                   //添加所有业务服务器业务版本
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        BusinessInfoBusVersion businessInfoT = JsonConvert.DeserializeObject<BusinessInfoBusVersion>(dr["Value"].ToString());
                        SingleBusinessInfoVer singleBusServerInfoVer = new SingleBusinessInfoVer();
                        if (((BusDDLBS)dr["busDDLBS"]) == BusDDLBS.LimitServerMaxConcurrentRandomRule)
                        {
                            singleBusServerInfoVer.Maxconcurrent = (int)dr["Maxconcurrent"];
                        }
                        singleBusServerInfoVer.BusServerID = dr["BusServerID"].ToString();
                        singleBusServerInfoVer.BusID = businessInfo.BusID;
                        singleBusServerInfoVer.BusVersion = businessInfoT.BusVersion;
                        singleBusServerInfoVer.TheConcurrent = 0;
                        if (singleBusServerInfoVer != null)
                        {
                            AddOrUpdateSingleBusBusinessVer(singleBusServerInfoVer);
                        }
                    }

                    DbHelper.BusServerBusInfoStatus(businessInfo.BusID, true);
                }
                else if (busStatus == BusStatus.Suspend)
                {
                    BusInfoOnline.busStatus = BusOnlineStatus.SILENCE;
                    sSysInfo = new SSysInfo(SysInfoType.AddOrUpdateBusinessInfoOnline);
                    sSysInfo.SetInfo(BusInfoOnline);
                    SendSSysInfoToAllSLB(sSysInfo);
                }
            
            }


            return true;
        }
    
        /// <summary>
        /// BusinessInfoBusVersion配置内容更新<see cref="BusID_BusVersion"/> 需要更新到每一个SLB  add by wanglei 20200211
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool UpdateBusBusinessInfoBusVer(BusinessInfoBusVersion businessInfoVersion, BusStatus busStatus)
        {
            SSysInfo sSysInfo=null ;
            if (busStatus == BusStatus.Offline)
            {
                //停止服务所有BusServer的服务
                DataTable dataTable = DbHelper.BusServerBusInfoVerByBusIDBusVer(businessInfoVersion.BusID, businessInfoVersion.BusVersion, false);
                foreach (DataRow dr in dataTable.Rows)
                {
                    SingleBusinessInfoVer singleBusServerInfo = new SingleBusinessInfoVer();
                    singleBusServerInfo.BusServerID = dr["BusServerID"].ToString();
                    singleBusServerInfo.BusID = businessInfoVersion.BusID;
                    singleBusServerInfo.BusVersion = businessInfoVersion.BusVersion;
                    if (singleBusServerInfo != null)
                    {
                        RemoveSingleBusBusinessVer(singleBusServerInfo);
                    }
                }
                DbHelper.BusServerBusInfoVerStatus(businessInfoVersion.BusID, businessInfoVersion.BusVersion, -1);
             
            }
            else
            {
                BusinessInfoVerOnline BusInfoOnline = new BusinessInfoVerOnline(businessInfoVersion);
                if (busStatus == BusStatus.Publish)
                {
                    BusInfoOnline.busStatus = BusOnlineStatus.READY;

                    //启动所有由业务版本停止时触发的停止服务器
                    DataTable dataTable = DbHelper.BusServerBusInfoVerByBusIDBusVer(businessInfoVersion.BusID, businessInfoVersion.BusVersion, -1);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        SingleBusinessInfoVer infoVer = new SingleBusinessInfoVer();

                        infoVer.BusServerID = dr["BusServerID"].ToString();
                        if (((BusDDLBS)dr["busDDLBS"]) == BusDDLBS.LimitServerMaxConcurrentRandomRule)
                        {
                            infoVer.Maxconcurrent = (int)dr["Maxconcurrent"];
                        }
                        infoVer.BusID = businessInfoVersion.BusID;
                        infoVer.BusVersion = businessInfoVersion.BusVersion;
                        if (infoVer != null)
                        {
                            AddOrUpdateSingleBusBusinessVer(infoVer);
                        }
                        DbHelper.BusServerBusInfoVerStatus(businessInfoVersion.BusID, businessInfoVersion.BusVersion, 1);
                    }

                }
                else if (busStatus == BusStatus.Suspend)
                {
                    BusInfoOnline.busStatus = BusOnlineStatus.SILENCE;
                }
                sSysInfo = new SSysInfo(SysInfoType.AddOrUpdateBusinessInfoVerOnline);
                sSysInfo.SetInfo(BusInfoOnline);

            }
            if (sSysInfo != null)
            {
                foreach (SSLBLoadInfo slbload in cdSLB.Values)
                {
                    try
                    {
                        if (slbload.SLBLTcpClient.Connected)
                            slbload.SLBLTcpClient.SendSysInfo(sSysInfo);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 删除BusinessInfoBusVersio 
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool RemoveBusinessInfoVerOnline (string BusID, string BusVersion)
        {
            SSysInfo sSysInfo = null;
            sSysInfo = new SSysInfo(SysInfoType.RemoveBusinessInfoVerOnline);
            sSysInfo.SetInfo(new BusinessInfoVerOnline() { BusID = BusID, BusVersion = BusVersion });
          return   SendSSysInfoToAllSLB(sSysInfo);
        }

            /// <summary>
            /// 移除单个BusServer上的单个业务BusName 对应业务版本  add by wanglei 20200211 OK 
            /// </summary>
            /// <param name="singleBusServerInfo"></param>
            /// <returns></returns>
            public bool RemoveSingleBusBusinessVer(SingleBusinessInfoVer singleBusServerInfoVer)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.StopSingleBusServerInfoVer);
            sSysInfo.SetInfo(singleBusServerInfoVer);
            return SendSSysInfoToAllSLB(sSysInfo);
        }

        /// <summary>
        /// 添加或者更新此单个BusServer上的单个业务对应业务版本 add by wanglei 20200213
        /// </summary>
        /// <param name="singleServerTakeInfo"></param>
        /// <returns></returns>
        public bool AddOrUpdateSingleBusBusinessVer(SingleBusinessInfoVer singleBusServerInfo)
        {
            SSysInfo sSysInfo = new SSysInfo();
            sSysInfo.InfoType = SysInfoType.AddOrUpdateSingleBusServerInfoVer;
            sSysInfo.SetInfo(singleBusServerInfo);
            return SendSSysInfoToAllSLB(sSysInfo);
        }
        /// <summary>
        /// 启动所有BusServer的服务 （建议只有在 Businesss 由offline变为publish时调用）
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool AddAllSingleBusBusinessVer(SingleBusinessInfoVer singleBusinessInfoVer)
        { //启动所有BusServer的服务
            //此处只启动由业务版本停止时触发的停止服务器
            DataTable dataTable = DbHelper.BusServerBusInfoVerByBusIDBusVer(singleBusinessInfoVer.BusID, singleBusinessInfoVer.BusVersion, 0 );
            foreach (DataRow dr in dataTable.Rows)
            {
                SingleBusinessInfoVer infoVer = new SingleBusinessInfoVer();
                infoVer.BusServerID = dr["BusServerID"].ToString();
                infoVer.BusID = singleBusinessInfoVer.BusID;
                infoVer.BusVersion = singleBusinessInfoVer.BusVersion;
                if (infoVer != null)
                {
                    AddOrUpdateSingleBusBusinessVer(infoVer);
                }
                DbHelper.BusServerBusInfoVerStatus(singleBusinessInfoVer.BusID, singleBusinessInfoVer.BusVersion, 1);
            }
            return true;
        }

      


        /// <summary>
        /// BusinessInfo更新业务版本信息或版本更新<see cref="BusID"/> 需要更新到每一个BusServer   add by wanglei 20200211
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool UpdateBusServerBusinessVerOrVersion(string BusID,string BusVersion)
        {
            SingleBusinessInfoVer singleBusServerInfo = new SingleBusinessInfoVer();

            singleBusServerInfo.BusID = BusID;
            singleBusServerInfo.BusVersion = BusVersion;
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.UpdateBusinessInfoVer);
            sSysInfo.SetInfo(singleBusServerInfo);
            return SendSSysInfoToAllSLB(sSysInfo);
        }


        /// <summary>
        ///  BusinessInfo更新已经存在的业务服务的测试版本<see cref="SBusIDBusVersion"/> 需要更新到每一个BusServer
        /// </summary>
        /// <param name="sBusIDBus"></param>
        /// <returns></returns>
        public bool AddOrUpdateBusinessTestVersion(string BusID,string BusVersion)
        {
            SBusIDBusVersion sBusIDBus = new SBusIDBusVersion();
            sBusIDBus.BusID = BusID;
            sBusIDBus.BusVersion = BusVersion;
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.AddOrUpdateBusinessTestVersion);
            sSysInfo.SetInfo(sBusIDBus);
            return SendSSysInfoToAllSLB(sSysInfo);
        }

        /// <summary>
        /// 加载默认测试标记
        /// </summary>
        public bool IinitializeDefaultTestMark()
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.IinitializeDefaultTestMark);
            sSysInfo.Info = "";
            return SendSSysInfo(sSysInfo);
        }

        /// <summary>
        /// 更新指定BusID BusVersion 测试标记
        /// </summary>
        /// <param name="sBusIDBus"></param>
        public bool AddorUpdateBusStatusAndTestMark(SBusIDBusVersion sBusIDBus  )
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.AddorUpdateBusStatusAndTestMark);
            sSysInfo.SetInfo(sBusIDBus);
            return SendSSysInfoToAllSLB(sSysInfo);
        }
        /// <summary>
        /// 获取自助机状态
        /// </summary>
        /// <param name="ContainClientSLB">是否包含在医院的自助机</param>
        /// <param name="Hos_ID">医院ID，空则表示全部</param>
        /// <returns></returns>
        public bool GetClientSystemAndProcessInfo(bool FromClientSLB=false,string Hos_ID ="")
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.ClientSystemAndProcessInfo);
            GetSClientSystemAndProcessInfo getSClient = new GetSClientSystemAndProcessInfo();
            getSClient.FromClientSLB = FromClientSLB;
            getSClient.HOS_ID  = Hos_ID;
            OMCandZZJInfoHead header = new OMCandZZJInfoHead();
            header.OMCID = MyPubConstant.OperationMaintenanceID;
            if (FromClientSLB)
            {
                if (string.IsNullOrEmpty(Hos_ID))
                {
                    header.OrgID = HCCNValye.SendToALLTag;
                }
                else
                {
                    header.OrgID = Hos_ID;
                }
            }
          

            sSysInfo.SetInfo(getSClient);
            return SendSSysInfoToAllSLB(sSysInfo, header);
        }
        
        /// <summary>
        /// 通知指定BusServer当前在线SLB列表
        /// </summary>
        /// <returns></returns>
        public bool SendOnLineSLBlistToBusServer(SOnLineSLBlistToBusServer sOnLineSLBlistToBusServer, List<string> SlbList)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.OnLineSLBlistToBusServer);
            sSysInfo.SetInfo(sOnLineSLBlistToBusServer);
            return SendSSysInfo(sSysInfo, SlbList);
        }


        /// <summary>
        /// 通知指定BusServer当前在线SLB列表
        /// </summary>
        /// <returns></returns>
        public bool OMC_SLB_Client(SOnLineSLBlistToBusServer sOnLineSLBlistToBusServer, List<string> SlbList)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.OnLineSLBlistToBusServer);
            sSysInfo.SetInfo(sOnLineSLBlistToBusServer);
            return SendSSysInfo(sSysInfo, SlbList);
        }

        /// <summary>
        ///  OMC 主动发给SLB或者自助机
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool OMC_SLB_Client(string SLBID,OMCandZZJInfoHead oMCandZZJInfo, byte[] bInfo,int LCCN =0)
        {
            if (cdSLB.ContainsKey(SLBID) && cdSLB[SLBID].SLBLTcpClient.Connected)
            {
                cdSLB[SLBID].SLBLTcpClient.OMC_SLB_Client(oMCandZZJInfo, bInfo, LCCN);
                return true;
            }
            return false;
        }
        /// <summary>
        ///  OMC 返回给自助机 
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool OMC_SLBR_Client(string SLBID, OMCandZZJInfoHead oMCandZZJInfo, byte[] bInfo, int LCCN = 0)
        {
            if (cdSLB.ContainsKey(SLBID) && cdSLB[SLBID].SLBLTcpClient.Connected)
            {
                cdSLB[SLBID].SLBLTcpClient.OMC_SLBR_Client(oMCandZZJInfo, bInfo, LCCN);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送跟踪条件请求
        /// </summary>
        /// <param name="sTrackConditions"></param>
        /// <returns></returns>
        public bool TrackConditions(STrackConditions sTrackConditions)
        {
            SSysInfo sSysInfo = new SSysInfo(SysInfoType.TrackConditions);
            sSysInfo.SetInfo(sTrackConditions);
            if (string.IsNullOrEmpty(sTrackConditions.SLBID))
            {
                foreach (SSLBLoadInfo slbload in cdSLB.Values)
                {
                    try
                    {
                        if (slbload.SLBLTcpClient.Connected)
                            slbload.SLBLTcpClient.SendSysInfo(sSysInfo);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return true;
            }
            else
            {
                if (cdSLB.ContainsKey(sTrackConditions.SLBID))
                    return cdSLB[sTrackConditions.SLBID].SLBLTcpClient.SendSysInfo(sSysInfo);
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 更新指定BusServer的HTTP服务允许接收的业务
        /// </summary>
        /// <param name="upSBusServerHttpBus"></param>
        /// <returns></returns>
        public bool UpdateSBusServerHttpBus(UpSBusServerHttpBus upSBusServerHttpBus)
        {
            SSysInfo sSysInfo = new SSysInfo();
            sSysInfo.InfoType = SysInfoType.UpdateSingleBusServerHTTPBus;
            sSysInfo.SetInfo(upSBusServerHttpBus);
            return SendSSysInfoToAllSLB(sSysInfo);
        }
        /// <summary>
        /// 负载均衡SLB的信息 包含
        /// </summary>
        class SSLBLoadInfo
        {
            public SSLBLoadInfo(WebSocket4NetSpring sLBLTcpClient, SLBInfo slBInfo)
            {
                SLBLTcpClient = sLBLTcpClient;
                sLBInfo = slBInfo;
            }
            public WebSocket4NetSpring SLBLTcpClient;
            public SLBInfo sLBInfo;
            /// <summary>
            /// SLB当前负载及系统状态
            /// </summary>
            public SLBCurrentStatus sLBCurrentStatus;
            /// <summary>
            /// 负载
            /// </summary>
            public int LoadFactor;
            /// <summary>
            /// 接收到SLB 发来的负载
            /// </summary>
            public int LoadFactorRec;

            /// <summary>
            /// 尝试重新连接次数，连接成功后请置零
            /// </summary>
            public int RetryConnectCount = 0;
            /// <summary>
            ///最后一次收到SLB发来的状态信息的时间
            /// </summary>
            public DateTime  LastReciveTime =DateTime.Now  ;

        }
    }
}
