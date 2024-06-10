using Newtonsoft.Json;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;
using SuperSocket.WebSocket.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BusinessInterface;
using PasS.Base.Lib;
using static BusinessInterface.BusServiceAdapter;
using Microsoft.Extensions.Configuration;
using System.Buffers;
using static PasS.Base.Lib.SSysRegisterInfo;
using Microsoft.Extensions.Hosting;

namespace PaaS.Comm
{  /// <summary>
   /// 重启服务
   /// </summary>
    public delegate void ReStartSLBEventHandler();
     public partial class PaaSCenter : IDisposable
    {

        public PaaSCenter()
        {
        }
        public PaaSCenter(SLBInfo  sLB_Info   )
        {
            sLBInfo = sLB_Info;
            WebSocketBusSPort = sLBInfo.WebSocketPort ;
        }
        /// <summary>
        ///  重启服务
        /// </summary>
        public event ReStartSLBEventHandler ReStartSLB_EventHandler;


        SLBInfo sLBInfo = null;
        /// <summary>
        /// 为BusServe提供连接的WebSocketServer
        /// </summary>
       // WebSocketServer WebSocketserveBusS = null;
        /// <summary>
        /// 监听的端口
        /// </summary>
        public int WebSocketBusSPort { get; private set; } = 8910;

        public bool IsRunning = false;

        //定义Timer类
        System.Threading.Timer threadTimer;
     
        WebSocket4NetSpring webSocket4NetSpring;

        int ClientToSLBRates = 0;

        int SLBToClientRates = 0;

        /// <summary>
        /// 缓存直接返回客户端
        /// </summary>
        int SLBCacheToClientRates = 0;
        /// <summary>
        /// 未处理直接返回客户端
        /// </summary>
        int SLBUTToClientRates = 0;

        /// <summary>
        /// BusServer到SLB请求调用其他BusServer服务
        /// </summary>
        int ServerCallOtherToSLBRates = 0;
        /// <summary>
        /// SLB返回 BusServer请求调用其他BusServer服务的结果
        /// </summary>
        int SLBToServerCallOtherBackRates = 0;
        /// <summary>
        /// SLB返回: BusServer请求调用其他BusServer服务，但无可用服务
        /// </summary>
        int SLBToServerCallOtherNoServerRates = 0;
        /// <summary>
        /// Http 发送消息到SLB
        /// </summary>
        int HttpRequestRates = 0;
        /// <summary>
        /// WebSocket 发送消息到SLB
        /// </summary>
        int WebSocketToSLBRates = 0;
        /// <summary>
        ///  SLB发送消息到WebSocket
        /// </summary>
        int SLBToSLBWebSocketRates = 0;

        /// <summary>
        ///  SLB接收到的系统信息
        /// </summary>
        int SLBSysInofRates = 0;

        SystemInfo System_Info = new SystemInfo();
        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <returns>异步TCP服务器</returns>
        public PaaSCenter Start()
        {
            if (!IsRunning)
            {
                Console.WriteLine("Start PaaSCenter...");
                System.Net.ServicePointManager.DefaultConnectionLimit = 512;
                SLBID = MyPubConstant.SLBID;
                IsRunning = true;
                DbHelper.SLBInfoSetStatus(MyPubConstant.SLBID, 1);
                SpringAPI.BusServerSetStatus(MyPubConstant.BusServerID, 1);
                BusServiceAdapter.BusServerID = MyPubConstant.BusServerID;
                AddOrUpdateSingleBusBusinessVer();
                WebSocletSStart();

              

                InitTimer();

                if (MyPubConstant.SLBisHosClient)
                {
                    StartClientSLB();
                    Console.WriteLine("StartClientSLB");
                }
                Httpport = MyPubConstant.HttpLocalPort;
                HttpStart();
            }
            else
            {
                Console.WriteLine("Start PaaSCenter IsRunning");
            }
            return this;
        }

 

        public void Dispose()
        {
            DbHelper.SLBInfoSetStatus(MyPubConstant.SLBID, 0);
            SpringAPI.BusServerSetStatus(MyPubConstant.BusServerID, 0);
        }
        /// <summary>
        /// 初始化Timer类
        /// </summary>
        private void InitTimer()
        {
            threadTimer = new System.Threading.Timer(new TimerCallback(TimerUp), null, 0, 1000);
             
        }
       
        /// <summary>
        /// 停止服务器
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                string showInfo = "";
                Console.ForegroundColor = ConsoleColor.Yellow;
                showInfo = string.Format("{0}SLB [{1}] Stoping...\r\n", DateTime.Now, MyPubConstant.SLBID);
                Console.WriteLine(showInfo);
                PasSLog.Info("ZZJServerLoadBalancing.Stop", showInfo);
                  WebSocletSStop();
                HTTPStop();
                threadTimer.Dispose();
                Console.ForegroundColor = ConsoleColor.Red;
                showInfo = string.Format("{0}SLB [{1}] Stop!!!!!!\r\n", DateTime.Now, MyPubConstant.SLBID);
                Console.WriteLine(showInfo);
                PasSLog.Info("ServerLoadBalancing.Stop", showInfo);
                DbHelper.SLBInfoSetStatus(MyPubConstant.SLBID, 0);
                SpringAPI.BusServerSetStatus(MyPubConstant.BusServerID, 0);
            }
            return true;
        }

        
        /// <summary>
        /// 定时到点执行的事件
        /// </summary>
        /// <param name="value"></param>
        private void TimerUp(object value)
        {
            try
            {
                //  using (SUsingDisposable usingDisposable = new SUsingDisposable())
                {
                    SLBCurrentStatus sLBCurrentStatus = new SLBCurrentStatus();
                    {
                      
                      
                        sLBCurrentStatus.SLBID = MyPubConstant.SLBID;
                        SMessageRates sMessageRates = new SMessageRates();
                        sMessageRates.ClientCount = cdClientWebSocket.Count;
                        sMessageRates.WebSocketCount = cdBusServerWebSocket.Count ;
                        sMessageRates.ClientToPaaSRates = ClientToSLBRates;
                        sMessageRates.HttpToSLBRates = HttpRequestRates;
                        if (HttpRequestRates + ClientToSLBRates > 0)
                            Console.WriteLine($"HttpRequestRates:{HttpRequestRates},ClientToPaaSRates:{ClientToSLBRates}");
                        sMessageRates.SLBToClientRates = SLBToClientRates;
                        // sMessageRates.ServerToSLBRates = ServerToSLBRates;
                        // sMessageRates.SLBToServerRates = SLBToServerRates;

                        sMessageRates.SLBCacheToClientRates = SLBCacheToClientRates;
                        sMessageRates.SLBUTToClientRates = SLBUTToClientRates;
                        sMessageRates.ServerCallOtherToSLBRates = ServerCallOtherToSLBRates;
                        sMessageRates.SLBToServerCallOtherBackRates = SLBToServerCallOtherBackRates;
                        sMessageRates.SLBToServerCallOtherNoServerRates = SLBToServerCallOtherNoServerRates;

                     
                        sMessageRates.SocketClientCount = cdClientWebSocket .Count;
                        sMessageRates.SocketWaitAuthCount = cdWebSocketBusS_WaitAuth.Count;
                        sMessageRates.WebSocketToSLBRates = WebSocketToSLBRates;
                        sMessageRates.SLBToSLBWebSocketRates = SLBToSLBWebSocketRates;
                        sMessageRates.SLBSysInofRates = SLBSysInofRates;

                        sMessageRates.Threads = Process.GetCurrentProcess().Threads.Count;

                        try
                        {
                            //sMessageRates.WaitTakes = 0;
                            //foreach (string key in cdWaitTakes.Keys)
                            //    sMessageRates.WaitTakes += cdWaitTakes[key].Count;
                        }
                        catch
                        {
                            sMessageRates.WaitTakes = 0;
                        }

                        ClientToSLBRates = 0;
                     //   ServerToSLBRates = 0;
                     //   SLBToServerRates = 0;
                        SLBToClientRates = 0;
                        SLBCacheToClientRates = 0;
                        SLBUTToClientRates = 0;
                        ServerCallOtherToSLBRates = 0;
                        SLBToServerCallOtherBackRates = 0;
                        SLBToServerCallOtherNoServerRates = 0;
                        HttpRequestRates = 0;
                        WebSocketToSLBRates = 0;
                        SLBToSLBWebSocketRates = 0;
                        SLBSysInofRates = 0;
                        //List<BusinessInfoOnline> list = GSLBS.GetBusOnlineInofo();
                        //foreach (BusinessInfoOnline busOnline in list)
                        //{
                        //    if (cdBusRates.ContainsKey(busOnline.BusID))
                        //    {
                        //        int AllcountLast = cdBusRates[busOnline.BusID].AllCount;
                        //        cdBusRates[busOnline.BusID] = new BusRates(busOnline);
                        //        cdBusRates[busOnline.BusID].Rates = busOnline.AllCount - AllcountLast;
                        //    }
                        //    else
                        //    {
                        //        cdBusRates.Add(busOnline.BusID, new BusRates(busOnline));
                        //        cdBusRates[busOnline.BusID].Rates = busOnline.AllCount;
                        //    }
                        //}
                        ////缓存数据数量
                        //if (UseSLBCacheData)
                        //{
                        //    if (cdBusCacheData.Count > 0)
                        //    {
                        //        int CacheDataCount = 0;
                        //        foreach (SSLBCacheDatacd sslBCacheData in cdBusCacheData.Values)
                        //        {
                        //            CacheDataCount += sslBCacheData.cdData.Count;
                        //        }
                        //        sMessageRates.CacheDataCount = CacheDataCount;
                        //    }
                        //}

                        sLBCurrentStatus.sMessageRates = sMessageRates;
                      //  sLBCurrentStatus.cdBusRates = cdBusRates;
                        sLBCurrentStatus.SystemandProcessInfo = System_Info.GetCurrentProcess();
                      //  Console.WriteLine(JsonConvert.SerializeObject( sLBCurrentStatus.SystemandProcessInfo));
                        //if (sMessageRates.ClientToSLBRates + sMessageRates.ServerToSLBRates + sMessageRates.WaitTakes > 0 && AsyncMessageRates != null)
                        //{
                        //    AsyncMessageRates(sLBCurrentStatus);
                        //}
                        SSysInfo sSysInfo = new SSysInfo(SysInfoType.SLBRatesStatus);
                        sSysInfo.SetInfo(sLBCurrentStatus);

                        //SendSysInfoToAccessServer(sSysInfo);

                        SendSysInfoToOM(sSysInfo);
                       SendSysInfoToFBOSLB(sSysInfo);
                        //decimal thisMload = (sLBCurrentStatus.SystemandProcessInfo.PhysicalMemoryMB() - sLBCurrentStatus.SystemandProcessInfo.MemoryAvailableMB()) * 100 / sLBCurrentStatus.SystemandProcessInfo.PhysicalMemoryMB();
                        //int SLBReStartMemoryHPointT = SLBReStartMemoryHPoint;
                        //if (sLBCurrentStatus.SystemandProcessInfo.PhysicalMemoryMB() > 2560)
                        //{
                        //    SLBReStartMemoryHPointT = SLBReStartMemoryHPoint - 10;
                        //}

                        //if (thisMload > SLBReStartMemoryHPointT && sLBCurrentStatus.SystemandProcessInfo.WorkingSet64MB() > SLBReStartWorkingSetMB)
                        //{//触发重启  add by wanglei 20201018

                        //    string Message = string.Format("{0} TimerUp SLBCurrentStatus  :SLB 占用内存达到 {1}MB,系统内存使用率达到{2}%,重启SLB  ", DateTime.Now, sLBCurrentStatus.SystemandProcessInfo.WorkingSet64MB(), thisMload);
                        //    Console.ForegroundColor = ConsoleColor.Red;
                        //    Console.WriteLine(Message);
                        //    Console.ForegroundColor = ConsoleColor.White;
                        //    MySpringLog.Error("TimerUp", Message);

                        //    Stop();
                        //    ReStartSLB_EventHandler();
                        //}
                        //else if (sLBCurrentStatus.sMessageRates.Threads > SLBReStartThreads)
                        //{//触发重启  add by wanglei 20210427

                        //    string Message = string.Format("{0} TimerUp SLBCurrentStatus  :SLB 线程数 {1} ,超过设定最大值{2} 重启SLB  ", DateTime.Now, sLBCurrentStatus.sMessageRates.Threads, SLBReStartThreads);
                        //    Console.ForegroundColor = ConsoleColor.Red;
                        //    Console.WriteLine(Message);
                        //    Console.ForegroundColor = ConsoleColor.White;
                        //    MySpringLog.Error("TimerUp", Message);

                        //    Stop();
                        //    ReStartSLB_EventHandler();
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("{0} TimerUp SLBCurrentStatus Error :{1}  ", DateTime.Now, ex.Message));
                Console.ForegroundColor = ConsoleColor.White;
                PasSLog.Error("TimerUp", string.Format("{0} SLBCurrentStatus :{1}  ", DateTime.Now, ex.ToString()));

            }
            try
            {
                //if (cdOMSWebSocket.Count > 0 && cdTrackResult.Count > 0)
                //{
                //    foreach (string TCID in cdTrackAndOM.Keys)
                //    {
                //        if (cdTrackResult.ContainsKey(TCID))
                //        {

                //            ConcurrentDictionary<string, STrackResult> cDSTR;
                //            cdTrackResult.TryRemove(TCID, out cDSTR);
                //            if (cDSTR.Count > 0)
                //            {
                //                SSysInfo sSysInfoTrack = new SSysInfo(SysInfoType.TrackResult);
                //                List<STrackResult> liststr = new List<STrackResult>();
                //                foreach (STrackResult sTR in cDSTR.Values)
                //                {
                //                    liststr.Add(sTR);
                //                }
                //                sSysInfoTrack.SetInfo(liststr);
                //                SendSysInfoToOM(sSysInfoTrack, cdTrackAndOM[TCID]);
                //            }
                //            cDSTR = null;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("{0} TimerUp TrackResult Error :{1}  ", DateTime.Now, ex.Message));
                Console.ForegroundColor = ConsoleColor.White;
                PasSLog.Error("TimerUp", string.Format("{0} TrackResult :{1}  ", DateTime.Now, ex.ToString()));
            }
        }

        
 

 
        #region  医院作为客户端连接公司端操作 
        private void StartClientSLB()
        {
            string WebSocketUrl = MyPubConstant.ClientSLBWebSocketUrl;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(string.Format("{0} StartClientSLB ，WebSocketUrl:{1} ", DateTime.Now, WebSocketUrl));
            Console.ForegroundColor = ConsoleColor.White;
            webSocket4NetSpring = new WebSocket4NetSpring(WebSocketUrl, "FBO183");
            webSocket4NetSpring.DataRegisterRetEvent += WebSocket4NetSpring_DataRegisterRetEvent; ;
            webSocket4NetSpring.DataServerSendClientHeadEvent += WebSocket4NetSpring_DataServerSendClientHeadEvent; ;
            webSocket4NetSpring.ReciveFileInfoEvent += WebSocket4NetSpring_ReciveFileInfoEvent; ;
            webSocket4NetSpring.ReciveOMC_and_ClientEvent += WebSocket4NetSpring_ReciveOMC_and_ClientEvent; ;
            webSocket4NetSpring.DataInteriorSysInfoEvent += WebSocket4NetSpring_DataInteriorSysInfoEvent;
            // webSocket4NetSpring.ConnecSLBBusServer();
            string zzjID = MyPubConstant.ZZJID;
            SSysRegisterInfo sSysRegisterInfo = new SSysRegisterInfo(RegisterIdentity.ClientSLB, SLBID);
            SSysRegisterInfoClientSLB infoClientSLB = new SSysRegisterInfoClientSLB();
            infoClientSLB.OrgID = sLBInfo.OrgID ;
            infoClientSLB.ClientIP = "";
            sSysRegisterInfo.SetInfo(infoClientSLB);
            webSocket4NetSpring.ConnectandLogin(sSysRegisterInfo);
        }

        private void WebSocket4NetSpring_DataInteriorSysInfoEvent(WebSocket4NetSpring webSocket4NetClent, OMCandZZJInfoHead head, SSysInfo sSysInfo)
        {
            if (!string.IsNullOrEmpty(head.ZZJID))//自助机不为空
            {
                if (cdClientWebSocket.ContainsKey(head.ZZJID))
                {
                    cdClientWebSocket[head.ZZJID].SendSysInfo(sSysInfo, head);
                }
            }
            else
            {
                if (sSysInfo.InfoType == SysInfoType.ClientSystemAndProcessInfo)
                {
                    GetSClientSystemAndProcessInfo getSClient = sSysInfo.GetInfo<GetSClientSystemAndProcessInfo>();
                    List<ClientSystemAndProcessInfo> list = new List<ClientSystemAndProcessInfo>();
                    foreach (WebSocketSessionSLB sessionSLB in cdClientWebSocket.Values)
                    {

                        ClientSystemAndProcessInfo clientSystemAndProcessInfo = new ClientSystemAndProcessInfo(sessionSLB.SystemAnd_ProcessInfo);
                        clientSystemAndProcessInfo.UDID = sessionSLB.UDID;
                        clientSystemAndProcessInfo.UDName = sessionSLB.UDID;
                        clientSystemAndProcessInfo.SLBID = webSocket4NetClent.SLBID;//. getSClient.FBOSLBID;
                        clientSystemAndProcessInfo.ClientSLBID = SLBID;
                        clientSystemAndProcessInfo.OrgID = head.OrgID;
                        clientSystemAndProcessInfo.IPAddress = sessionSLB.SocketSession.RemoteEndPoint.ToString();

                        list.Add(clientSystemAndProcessInfo);
                    }
                    if (list.Count > 0)
                    {
                        SSysInfo sSysInfo1 = new SSysInfo(SysInfoType.ClientSystemAndProcessInfo);
                        //sSysInfo.InfoType = SysInfoType.ClientSystemAndProcessDynamicInfo;
                        //ClientSystemAndProcessInfoClientSLB csaps = new ClientSystemAndProcessInfoClientSLB();
                        //csaps.FBOOMCID = getSClient.FBOOMCID;
                        //csaps.clients = list;
                        sSysInfo1.SetInfoEncrypt(list);
                        webSocket4NetClent.SendSysInfo(sSysInfo1, head);
                    }

                }
                else
                {
                    DealOMCSysInfo(head, sSysInfo, true, null);
                }
            }
        }

        private void WebSocket4NetSpring_ReciveOMC_and_ClientEvent(WebSocket4NetSpring webSocket4NetClent, int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            if (!string.IsNullOrEmpty(Head.ZZJID) && cdClientWebSocket.ContainsKey(Head.ZZJID))
            {
                cdClientWebSocket[Head.ZZJID].Send(CCN, Head, bInfo);
            }
        }

        private void WebSocket4NetSpring_ReciveFileInfoEvent(WebSocket4NetSpring webSocket4NetClent, SRecFileInfo sRecFileInfo)
        {
            throw new NotImplementedException();
        }

        private void WebSocket4NetSpring_DataServerSendClientHeadEvent(WebSocket4NetSpring webSocket4NetClent, int CCN, OMCandZZJInfoHead sLBInfoHead, byte[] bInfo)
        {
            throw new NotImplementedException();
        }

        private void WebSocket4NetSpring_DataRegisterRetEvent(WebSocket4NetSpring webSocket4NetClent, SSysRegisterRetInfo sSysRegisterInfo)
        {
            //throw new NotImplementedException();
        }

        #endregion 

     

        #region  业务管理

        public bool AddOrUpdateSingleBusBusinessVer()
        {
            if (MyPubConstant.LoadLocalBusinessInfoConfigInfo)
            {
                try
                {
                    List<BusinessInfoBusVersion> listlocal = new List<BusinessInfoBusVersion>();
                    string txtconfigInfo = PasSLog.ReadAllText(@"BusinessInfoConfigInfo.ini");
                    if (!string.IsNullOrWhiteSpace(txtconfigInfo))
                    {
                        listlocal = JsonConvert.DeserializeObject<List<BusinessInfoBusVersion>>(txtconfigInfo);
                    }
                    foreach (BusinessInfoBusVersion businessInfo in listlocal)
                    {
                        try
                        {
                            busServiceAdapter.AddOrUpdateBusiness(businessInfo);
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine($"加载本地业务{businessInfo.BusID},{businessInfo.DllName},异常:{ex2.Message }");
                        }
                    }
                    Console.WriteLine($"加载本地业务成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"加载本地业务失败:{ex.Message }");
                }

            }
            List<SingleBusinessInfoVer> list = SpringAPI.BusServerBusInfoSingleVerListByServerGet(MyPubConstant.BusServerID);
            if (list != null)
            {
                foreach (SingleBusinessInfoVer sbs in list)
                {
                    AddOrUpdateSingleBusBusinessVer(sbs);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 移除此BusServer上的单个业务BusName
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool RemoveSingleBusBusiness(string BusID)
        {
            SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
            singleBusServerInfo.BusID = BusID;
            return RemoveSingleBusBusiness(singleBusServerInfo);
        }
        /// <summary>
        /// 添加或者更新此BusServer上的单个业务BusName
        /// </summary>
        /// <param name="singleServerTakeInfo"></param>
        /// <returns></returns>
        public bool AddOrUpdateSingleBusBusiness(SingleBusServerInfo singleBusServerInfo)

        {
            return true;
        }
        /// <summary>
        /// 移除此BusServer上的单个业务BusName
        /// </summary>
        /// <param name="singleBusServerInfo"></param>
        /// <returns></returns>
        public bool RemoveSingleBusBusiness(SingleBusServerInfo singleBusServerInfo)
        {
            SSysInfo sSysInfo = new SSysInfo();
            sSysInfo.InfoType = SysInfoType.RemoveSingleBusServerInfo;


            sSysInfo.SetInfoEncrypt(singleBusServerInfo);


            //foreach (AsyncTcpClientAssign SSLc in cdSLB.Values)
            //{
            //    SSLc.SendSysInfo(sSysInfo);
            //}
            Thread.Sleep(1000);
            //busServiceAdapter.RemoveBusiness(singleBusServerInfo.BusID);
            //if (sdBusRates.ContainsKey(singleBusServerInfo.BusID))
            //{
            //    SBusServerBusRates busServerBusRates;
            //    sdBusRates.TryRemove(singleBusServerInfo.BusID, out busServerBusRates);
            //    busServerBusRates = null;
            //}
            return true;
        }

        /// <summary>
        ///  更新已经存在的业务服务配置或版本
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool UpdateBusBusinessInfoOrVersion(string BusID)
        {
            busServiceAdapter.UpdateBusBusinessInfoOrVersion(BusID);
            return true;
        }

        /// <summary>
        /// 添加或者更新此BusServer上的单个业务对应业务版本  add by wanglei 20200213
        /// </summary>
        /// <param name="singleServerTakeInfo"></param>
        /// <returns></returns>
        public bool AddOrUpdateSingleBusBusinessVer(SingleBusinessInfoVer singleBusinessInfoVer)
        {
            if (!busServiceAdapter.EXECBusiness(singleBusinessInfoVer.BusID, singleBusinessInfoVer.BusVersion))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("加载 " + singleBusinessInfoVer.BusID + "的业务版本" + singleBusinessInfoVer.BusVersion + "失败，请检查DLL及依赖是否存在，请检查配置命名空间及类名是否正确。");

                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            SSysInfo sSysInfo = new SSysInfo();
            sSysInfo.InfoType = SysInfoType.AddOrUpdateSingleBusServerInfoVer;

            sSysInfo.SetInfoEncrypt(singleBusinessInfoVer);


            busServiceAdapter.AddOrUpdateBusiness(singleBusinessInfoVer.BusID, singleBusinessInfoVer.BusVersion);


            return true;
        }
        /// <summary>
        /// 移除此BusServer上的单个业务版本 BusName
        /// </summary>
        /// <param name="singleBusServerInfo"></param>
        /// <returns></returns>
        public bool RemoveSingleBusBusinessVer(SingleBusinessInfoVer singleBusServerInfo)
        {
            SSysInfo sSysInfo = new SSysInfo();
            sSysInfo.InfoType = SysInfoType.StopSingleBusServerInfoVer;

            sSysInfo.SetInfoEncrypt(singleBusServerInfo);



            Thread.Sleep(1000);
            busServiceAdapter.RemoveBusinessVer(singleBusServerInfo.BusID, singleBusServerInfo.BusVersion);

            return true;
        }

        /// <summary>
        ///  更新已经存在的业务服务配置或版本
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool UpdateBusBusinessInfoOrVersion(string BusID, string BusVersion)
        {
            busServiceAdapter.UpdateBusBusinessInfoOrVersion(BusID, BusVersion);
            return true;
        }

        /// <summary>
        ///  更新业务测试版本版本
        /// </summary>
        /// <param name="BusID"></param>
        /// <param name="BusVersion"></param>
        /// <returns></returns>
        public bool AddOrUpdateBusinessTestVersion(string BusID, string BusVersion)
        {
            busServiceAdapter.AddOrUpdateBusinessTestVersion(BusID, BusVersion);
            return true;
        }
        public bool IinitializeDefaultTestMark()
        {
            busServiceAdapter.IinitializeDefaultTestMark();
            return true;
        }

        public bool AddorUpdateBusStatusAndTestMark(SBusIDBusVersion sBusIDBus)
        {
            busServiceAdapter.AddorUpdateBusStatusAndTestMark(sBusIDBus);
            return true;
        }
        #endregion 业务控制


        /// <summary>
        /// 根据前台版本获取后台版本
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="CCN"></param>
        /// <param name="sLBInfoHead"></param>
        /// <returns></returns>
        private string GetServerVersionByClientVersion(string ClientID, int CCN, OMCandZZJInfoHead sLBInfoHead)
        {
            int HCCN = CCN / 10000;
            string ClientVeriosn = sLBInfoHead.BusVersion;
            //处理对应版本代码

            return busServiceAdapter.GetBusinessExecDefVersion(HCCN.ToString());//获取业务第一个版本(仅仅是测试用)
        }

        BusServiceAdapter busServiceAdapter = new BusServiceAdapter();

        private async Task<bool> ProcessingSLB_DataInfo(WebSocketSessionSLB webSocket, int CCN, OMCandZZJInfoHead sLBInfoHead, byte[] bInfo)
        {



            //是否是异常结果
            HandingBusinessResult hanBusResult = new HandingBusinessResult();
            var result = await Task.Run(() =>
            {
                SLBInfoHeadBusS headBusS = new SLBInfoHeadBusS(sLBInfoHead);
                headBusS.RealBVer = GetServerVersionByClientVersion(webSocket.UDID, CCN, sLBInfoHead);
                IPEndPoint iPEndPoint = (IPEndPoint)webSocket.SocketSession.RemoteEndPoint;
                string strKey = iPEndPoint.Address.ToString() + ":" + iPEndPoint.Port.ToString();
                headBusS.SLBIP_Port = strKey;
                if (CCNSysValye.IsClientrRequestServer(CCN) || HCCNValye.IsManageRequestSLB(CCN / 10000)) //需要调用此服务的客户 hlw add 2020.07.15 增加SLB管理端调用业务
                {
                    bool InteractiveEnd;
                    byte[] bOutInfo;
                    hanBusResult = busServiceAdapter.HandingBusiness(CCN, headBusS, bInfo, out bOutInfo, out InteractiveEnd);
                    webSocket.Send(CCN, sLBInfoHead, bOutInfo);
                }
                return true;

            });

            return result;
        }

     



    }

    class WebSocketClientWaitAuth
    {
        public WebSocketClientWaitAuth(WebSocketSessionSLB webSocketSLB)
        {
            AuthCount = 0;
            WebSocketSLB = webSocketSLB;
        }
        public WebSocketSessionSLB WebSocketSLB;
        public int AuthCount;
        /// <summary>
        ///平台AESKey
        /// </summary>
        public string AESKey = "";
        /// <summary>
        /// 用户使用的RSA公钥 
        /// </summary>
        public string RSAPubUser = "";
        /// <summary>
        ///平台RSA私钥
        /// </summary>
        public string RSAPri = "";

    }
}
