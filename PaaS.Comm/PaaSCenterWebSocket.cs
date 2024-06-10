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
{  
     public partial class PaaSCenter : IDisposable
    {

       
      
   
 
        /// <summary>
        ///待验证BusServer客户端列表
        ///key:SessionID
        /// </summary>
        static ConcurrentDictionary<string, WebSocketClientWaitAuth> cdWebSocketBusS_WaitAuth = new ConcurrentDictionary<string, WebSocketClientWaitAuth>();

        /// <summary>
        /// 在线Session列表
        ///Key：Sessionid WebSocketSessionSLB
        /// </summary>
        internal static ConcurrentDictionary<string, WebSocketSessionSLB> cdBusServerWebSocket = new ConcurrentDictionary<string, WebSocketSessionSLB>();
        /// <summary>
        /// 在线自助机客户端列表
        ///Key：ClientID WebSocketSessionSLB
        /// </summary>
        internal static ConcurrentDictionary<string, WebSocketSessionSLB> cdClientWebSocket = new ConcurrentDictionary<string, WebSocketSessionSLB>();

        /// <summary>
        /// 在线OMS列表
        ///Key：ClientID WebSocketSessionSLB
        /// </summary>
        internal static ConcurrentDictionary<string, WebSocketSessionSLB> cdOMSWebSocket = new ConcurrentDictionary<string, WebSocketSessionSLB>();

        /// <summary>
        /// 在线医院PaaS列表
        ///Key：HosID:ClientID WebSocketSessionSLB
        /// </summary>
        internal static ConcurrentDictionary<string , ConcurrentDictionary<string, WebSocketSessionSLB>> cdCPaaSWebSocket = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocketSessionSLB>>();
        string SLBID="";
       
        /// <summary>
        /// 使用自定义的数据包和过滤器创建的服务宿主
        /// </summary>
        private WebSocketHostBuilder host = null;
       
        

        /// <summary>
        /// WebSoclet服务启动
        /// </summary>
        /// <returns></returns>
        public bool WebSocletSStart()
        {
            if (host == null)
            {
                host = WebSocketHostBuilder.Create();
            }
            else
            {
                return false;
            }
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;

                host.ConfigureAppConfiguration((hostCtx, configApp) =>
                {
                    configApp.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"serverOptions:name", "PxIotServer"},
                        {"serverOptions:listeners:0:ip", "Any"},
                        {"serverOptions:listeners:0:port", WebSocketBusSPort.ToString()}
                    });
                });

                host.UseWebSocketMessageHandler(async (session, package) =>
                {
                    OnPackageAsync(session, package);
                    // string str = Encoding.UTF8.GetString(package.Data.ToArray());
                    //  Console.WriteLine($@"Receive message===>{str}.");
                });
                //配置参数
                //host.ConfigureSuperSocket(options =>
                //{
                //    options.Name = "PxIotServer";
                //    options.MaxPackageLength = WebSocketSessionClientBase.MaxRequestLength; //最大包长度
                //    options.ReceiveBufferSize = 4 * 1024;
                //    options.SendBufferSize = 4 * 1024;
                //    options.ReceiveTimeout = 0;
                //    options.SendTimeout = 500 * 1000;

                //    options.Listeners = new[] { new ListenOptions { Ip = "Any", Port = WebSocketBusSPort, BackLog = 1024 } }.ToList();
                //});
                //使用会话的连接和断开事件
                host.UseSessionHandler(OnConnectedAsync, OnClosedAsync);

                //使用会话的数据接收时间
                // host.UsePackageHandler(OnPackageAsync);
                //启动
                host.RunConsoleAsync();
                Console.WriteLine(string.Format($"WebSocket设置{WebSocketBusSPort}端口成功!"));
                Console.WriteLine(string.Format($"WebSocke监听{WebSocketBusSPort}端口成功!"));
              
          
                return true;


            }
            catch (Exception ex)
            {
                Console.WriteLine("WebSocletSStart:" + ex.Message);
                return false;
            }


        }

        /// <summary>
        /// WebSoclet服务启动
        /// </summary>
        /// <returns></returns>
        public bool WebSocletSStartOld()
        {/*

            if (WebSocketserveBusS == null)
            {
                WebSocketserveBusS = new WebSocketServer();
            }
            else
            {
                return false;
            }

            WebSocketserveBusS.NewSessionConnected += WebSocketserve_NewSessionConnected; ;
            WebSocketserveBusS.SessionClosed += WebSocketserve_SessionClosed;
            WebSocketserveBusS.NewMessageReceived += WebSocketserveBusS_NewMessageReceived;
            WebSocketserveBusS.NewDataReceived += WebSocketserveBusS_NewDataReceived;

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                ServerConfig serverConfig = new ServerConfig
                {
                    Name = "WebSServer",
                    ServerTypeName = "WebSServer",
                    ClearIdleSession = true, //60秒执行一次清理90秒没数据传送的连接
                    ClearIdleSessionInterval = 60,
                    IdleSessionTimeOut = 300,
                    MaxRequestLength = WebSocketSessionClientBase.MaxRequestLength, //最大包长度
                    Ip = "Any",
                    Port = WebSocketBusSPort,
                    MaxConnectionNumber = 1000,
                };

                WebSocketserveBusS.Setup(serverConfig);//设置端口
                Console.WriteLine(string.Format($"WebSocket设置{WebSocketBusSPort}端口成功!"));

                WebSocketserveBusS.Start();//开启监听
                Console.WriteLine(string.Format($"WebSocke监听{WebSocketBusSPort}端口成功!"));
               
                return true;

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("WebSocletSStart:" + ex.Message);
                return false;
            }
            */
            return true;
        }

      
       
       

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        public bool WebSocletSStop()
        {
            if (host != null)
            {
                bool isSuccess = false;
                foreach (var v in cdBusServerWebSocket)
                {
                    v.Value.SocketSession.CloseAsync(CloseReason.ServerShutdown);
                }
                cdBusServerWebSocket.Clear();
                SpringAPI.BusServerSetStatus(MyPubConstant.BusServerID, 0);
               
            }
            
            SpringAPI.BusServerSetStatus(MyPubConstant.BusServerID, 0);
            cdBusServerWebSocket.Clear();
            cdClientWebSocket.Clear();
            cdOMSWebSocket.Clear();
            return true;
        }
      

        /// <summary>
        /// 会话的连接事件
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        private async ValueTask OnConnectedAsync(IAppSession session)
        {
            await Task.Run(() =>
            {
                WebSocketSession webSocketSession = (WebSocketSession)session;
                string SessionID = session.SessionID;
                // session.LastActiveTime = DateTime.Now;
                WebSocketSessionSLB sessionSLB = new WebSocketSessionSLB(webSocketSession);
                sessionSLB.RegisterEvent += SessionSLB_RegisterEvent;
                cdWebSocketBusS_WaitAuth.TryAdd(SessionID, new WebSocketClientWaitAuth(sessionSLB));
                string Message = string.Format("{0}:客户端IP{1}({2}),ID {3} 接入WebSocketS", DateTime.Now.ToShortDateString() + " "
                    + DateTime.Now.ToShortTimeString(), session.RemoteEndPoint.ToString(), "", session.SessionID);

                SessionState sessionState = session.State;
                ConsoleColor colorold = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Message);
                Console.ForegroundColor = colorold;
                PasSLog.WebSocketLog("NewSessionConnected", Message);

            });
        }
       

        /// <summary>
        /// 会话的断开事件
        /// </summary>
        /// <param name="session"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private async ValueTask OnClosedAsync(IAppSession session, CloseEventArgs e)
        {
            await Task.Run(() =>
            {
                EndPoint iPEndPoint = session.RemoteEndPoint;
                // string IP_Port = iPEndPoint.Address.ToString() + ":" + iPEndPoint.Port.ToString();
                if (cdBusServerWebSocket.ContainsKey(session.SessionID))
                    BusServerExceptionStop(cdBusServerWebSocket[session.SessionID]);
                else if (cdWebSocketBusS_WaitAuth.ContainsKey(session.SessionID))
                {
                    WebSocketClientWaitAuth webSocketw;
                    cdWebSocketBusS_WaitAuth.TryRemove(session.SessionID, out webSocketw);
                    webSocketw = null;
                }

                string Message = string.Format("{0}:客户端IP{1}({2}),ID {3} 关闭", DateTime.Now.ToShortDateString() + " "
                 + DateTime.Now.ToShortTimeString(), session.RemoteEndPoint.ToString(), "", session.SessionID);
                ConsoleColor colorold = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Message);
                Console.ForegroundColor = colorold;
            });
        }
      
        private void OnPackageAsync(WebSocketSession session, SuperSocket.WebSocket.WebSocketPackage package)
        {
            Task.Run(() =>
            {

                string str = Encoding.UTF8.GetString(package.Data.ToArray());

                if (cdBusServerWebSocket.ContainsKey(session.SessionID))
                {
                    //byte[] bys = WebSocketSeverHelper.DecodeClientData(package.Data, package.Data.Length);
                    TaskWebSocketserveBusS_NewDataReceived(cdBusServerWebSocket[session.SessionID], package.Data.ToArray());
                }
                else if (cdWebSocketBusS_WaitAuth.ContainsKey(session.SessionID))
                {
                    // byte[] bys = WebSocketSeverHelper.DecodeClientData(package.Datas, package.Datas.Length);
                    TaskWebSocketserveBusS_NewDataReceived(cdWebSocketBusS_WaitAuth[session.SessionID].WebSocketSLB, package.Data.ToArray());
                }
            });

        }
      
        /// <summary>
        /// 收到数据
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void WebSocketserveBusS_NewDataReceived(WebSocketSession session, byte[] value)
        {
            if (cdBusServerWebSocket.ContainsKey(session.SessionID))
            {
                TaskWebSocketserveBusS_NewDataReceived(cdBusServerWebSocket[session.SessionID], value);
            }
            else if (cdWebSocketBusS_WaitAuth.ContainsKey(session.SessionID))
            {
                TaskWebSocketserveBusS_NewDataReceived(cdWebSocketBusS_WaitAuth[session.SessionID].WebSocketSLB, value);
            }
        }
        private async Task<bool> TaskWebSocketserveBusS_NewDataReceived(WebSocketSessionSLB webSocket, byte[] value)
        {
            var result = await Task.Run(() =>
            {
                WebSocketToSLBRates++;
                webSocket.DataReceived(value);
                return true;
            });
            return result;
        }
        private void WebSocketserveBusS_NewMessageReceived(WebSocketSession session, string value)
        {
            //throw new NotImplementedException();
        }

       
    


        /// <summary>
        /// 连接客户端关闭
        /// </summary>
        /// <param name="internalClient"></param>
        /// <param name="isCheckStop">是否心跳通讯不答强行关闭</param>
        void BusServerExceptionStop(WebSocketSessionSLB internalClient, bool isCheckStop = false)
        {
            //连接已经关闭
            try
            {//add by wanglei 20190517 强制关闭

                internalClient.SocketSession.CloseAsync(CloseReason.LocalClosing);

            }
            catch
            {
            }
            cdBusServerWebSocket.TryRemove(internalClient.SessionID, out internalClient);
            if (cdClientWebSocket.ContainsKey(internalClient.UDID))
            {
                if (cdClientWebSocket[internalClient.UDID].SessionID == internalClient.SessionID)
                {
                    WebSocketSessionSLB sessionSLB;
                    cdClientWebSocket.TryRemove(internalClient.UDID, out sessionSLB);
                    string Message = string.Format("{0}:ZZJClient ID {3} , {1}({2}) 关闭", DateTime.Now.ToShortDateString() + " "
      + DateTime.Now.ToShortTimeString(), sessionSLB.SocketSession.RemoteEndPoint.ToString(),"", sessionSLB.UDID );
                    ConsoleColor colorold = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(Message);
                    Console.ForegroundColor = colorold;
                }
            }
            else if (cdOMSWebSocket.ContainsKey(internalClient.UDID))
            {
                if (cdOMSWebSocket[internalClient.UDID].SessionID == internalClient.SessionID)
                {
                    WebSocketSessionSLB sessionSLB;
                    cdOMSWebSocket.TryRemove(internalClient.UDID, out sessionSLB);
                    string Message = string.Format("{0}:管理控制端 ID {3} , {1}({2})  关闭", DateTime.Now.ToShortDateString() + " "
      + DateTime.Now.ToShortTimeString(), sessionSLB.SocketSession.RemoteEndPoint.ToString(), "", sessionSLB.UDID);
                    ConsoleColor colorold = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(Message);
                    Console.ForegroundColor = colorold;
                   
                }
            }
            else
            {
                Console.WriteLine(string.Format("unknow Client {0} {1} remove \r\n", internalClient.UDID, internalClient.SessionID));
            }
            return;

        }

        /// <summary>
        /// 检查WebSocket是否在线
        /// </summary>
        void CheckWebSocketForClientConnect()
        {
            if (host == null)
            {
                return;
            }
            //  if (host. == ServerState.Started)
            {
                foreach (string key in cdBusServerWebSocket.Keys)
                {
                    WebSocketSessionSLB socketInfo = cdBusServerWebSocket[key];
                    if (socketInfo.Connected)
                    {
                        if (socketInfo.TrySendHeartbeat())
                            break;
                        if (socketInfo.TrySendHeartbeat())//如果一次不成功再执行一次
                            break;
                    }
                    BusServerExceptionStop(socketInfo);
                }

                foreach (string key in cdWebSocketBusS_WaitAuth.Keys)
                {
                    try
                    {
                        WebSocketClientWaitAuth socketInfo = cdWebSocketBusS_WaitAuth[key];
                        if (socketInfo.WebSocketSLB != null && socketInfo.WebSocketSLB.Connected)
                        {
                            if ((DateTime.Now - socketInfo.WebSocketSLB.SocketSession.LastActiveTime).TotalMinutes >= 1)
                            {
                                socketInfo.WebSocketSLB.Close();
                            }
                            else
                            {
                                break;
                            }
                        }
                        cdWebSocketBusS_WaitAuth.TryRemove(key, out socketInfo);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
        }


        /// <summary>
        /// 注册事件例子，可以自己重写
        /// </summary>
        /// <param name="sSysRegisterInfo"></param>
        private void SessionSLB_RegisterEvent(SSysRegisterInfo sSysRegisterInfo, WebSocketSessionSLB socketSessionSLB)
        {
            if (sSysRegisterInfo.Register_Identity == RegisterIdentity.ZZJClient)
            {
                if (sSysRegisterInfo.Info == "WebSocketClient")
                {
                    WebSocketClinetPassLgoin(socketSessionSLB, sSysRegisterInfo);
                }
            }
            else if (sSysRegisterInfo.Register_Identity == RegisterIdentity.OperationMaintenance|| sSysRegisterInfo.Register_Identity == RegisterIdentity.WebManage )
            {

                WebSocketOperationMaintenancePassLgoin(socketSessionSLB, sSysRegisterInfo);

            }
            else if (sSysRegisterInfo.Register_Identity == RegisterIdentity.ClientSLB)//在医院的SLB
            {

                WebSocketClientSLBPassLgoin(socketSessionSLB, sSysRegisterInfo);

            }
        }

        #region   医院ClientSLB客户端操作
        /// <summary>
        /// 登录验证通过
        /// </summary>
        void WebSocketClientSLBPassLgoin(WebSocketSessionSLB sSLBWebSocket,  SSysRegisterInfo sSysRegister)
        {
            
            sSLBWebSocket.DataInteriorSysInfoEvent += ClientSLBWebSocket_DataInteriorSysInfoEvent;
            sSLBWebSocket.OMC_and_ClientEvent += SSLBWebSocketClientSLB_OMC_and_ClientEvent;
            sSLBWebSocket.UDID = sSysRegister.ID;
            SSysRegisterInfoClientSLB infoClientSLB = sSysRegister.GetInfo<SSysRegisterInfoClientSLB>();
            string Hos_id = infoClientSLB.OrgID ;
            if (cdCPaaSWebSocket.ContainsKey(Hos_id) && cdCPaaSWebSocket[Hos_id].ContainsKey(sSysRegister.ID)&& cdCPaaSWebSocket[Hos_id][sSysRegister.ID].SessionID != sSLBWebSocket.SessionID)//是不同连接
            {
                WebSocketSessionSLB sSLB = cdCPaaSWebSocket[Hos_id][sSysRegister.ID] ;
                sSLB.OrgID = Hos_id;
                string oldSessionID = sSLB.SessionID;
                if (cdBusServerWebSocket.ContainsKey(oldSessionID))
                {
                    WebSocketSessionSLB sSLBWebSockeold2;
                    cdBusServerWebSocket.TryRemove(oldSessionID, out sSLBWebSockeold2);
                }

                string Message = string.Format("{0}:ClientPaaS ID {1} IP{5},ID {6} WebSocket 重复上线，删除并关闭之前连接 IP{2},ID {3} ;当前在线客户数：{4}", DateTime.Now.ToShortDateString() + " "
    + DateTime.Now.ToShortTimeString(), sSysRegister.ID, sSLB.SocketSession.RemoteEndPoint.ToString(), sSLB.SocketSession.SessionID, cdCPaaSWebSocket.Count
    , sSLBWebSocket.SocketSession.RemoteEndPoint.ToString(), sSLBWebSocket.SocketSession.SessionID);
                ConsoleColor colorold2 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Message);
                Console.ForegroundColor = colorold2;
                PasSLog.WebSocketLog("WebSocketClientSLBPassLgoin", Message);
                sSLB.SocketSession.CloseAsync(CloseReason.RemoteClosing);

                sSLB.SocketSession = sSLBWebSocket.SocketSession;
                if (!cdBusServerWebSocket.ContainsKey(sSLB.SessionID))
                {
                    cdBusServerWebSocket.TryAdd(sSLB.SessionID, sSLB);
                }


            }
            else
            {
                sSLBWebSocket.OrgID = Hos_id;
                if (!cdBusServerWebSocket.ContainsKey(sSLBWebSocket.SessionID))
                {
                    cdBusServerWebSocket.TryAdd(sSLBWebSocket.SessionID, sSLBWebSocket);
                }

                if (!cdCPaaSWebSocket.ContainsKey(Hos_id ))
                {
                    cdCPaaSWebSocket.TryAdd(Hos_id, new ConcurrentDictionary<string, WebSocketSessionSLB>() );
                }
                
                if (!cdCPaaSWebSocket[Hos_id].ContainsKey(sSysRegister.ID))
                {
                    cdCPaaSWebSocket[Hos_id].TryAdd(sSysRegister.ID, sSLBWebSocket);
                }
                else
                {
                    cdCPaaSWebSocket[Hos_id][sSysRegister.ID].SocketSession = sSLBWebSocket.SocketSession;
                }
            }
            //发送消息给客户端
            SSysInfo sSysInfoRet = new SSysInfo(SysInfoType.RegisterRet);
            SSysRegisterRetInfo sSysRegisterRetInfo = new SSysRegisterRetInfo(MyPubConstant.SLBID, sSysRegister.ID, sSLBWebSocket.SocketSession.RemoteEndPoint.ToString().Split(':')[0],int.Parse( sSLBWebSocket.SocketSession.RemoteEndPoint.ToString().Split(':')[1]));
            sSysRegisterRetInfo.Register_Identity = sSysRegister.Register_Identity;
            sSysRegisterRetInfo.SessionID = sSLBWebSocket.SessionID;
            sSysRegisterRetInfo.ReslutCode = 1;

            int PaaScount = 0;
            foreach (ConcurrentDictionary<string, WebSocketSessionSLB> cd in cdCPaaSWebSocket.Values)
            {
                PaaScount += cd.Count;
            }

            sSysInfoRet.SetInfo(sSysRegisterRetInfo);
            sSLBWebSocket.SendSysInfo(sSysInfoRet);
            ConsoleColor colorold = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{ DateTime.Now.ToShortTimeString()}:客户PaaS ID:[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{sSysRegister.ID}");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"]IP:{sSLBWebSocket.SocketSession.RemoteEndPoint.ToString()} ");
            Console.WriteLine($" 连接成功,当前连接客户数量{cdCPaaSWebSocket.Count} PaaS数量:{PaaScount} ");

            Console.ForegroundColor = colorold;

        }
        /// <summary>
        /// 发送给上级PaaS
        /// </summary>
        /// <param name="sSysInfo"></param>
        private void SendSysInfoToFBOSLB(SSysInfo sSysInfo)
        {
            Task.Run(() =>
            {
                try
                {if(webSocket4NetSpring!=null&& webSocket4NetSpring.Connected )
                    webSocket4NetSpring.SendSysInfo(sSysInfo);
                }
                catch
                {
                }
                return true;
            });
        }
        private void ClientSLBWebSocket_DataInteriorSysInfoEvent(WebSocketSessionSLB socketSession, OMCandZZJInfoHead sLBInfoHead, SSysInfo sSysInfo)
        {
            if (!string.IsNullOrEmpty(sLBInfoHead.OMCID))
            {

                if (cdOMSWebSocket.ContainsKey(sLBInfoHead.OMCID))
                {
                    string FBOOMCID = sLBInfoHead.OMCID;

                    cdOMSWebSocket[FBOOMCID].SendSysInfo(sSysInfo, sLBInfoHead);
                }

            }
            else
            {
                if (sSysInfo.InfoType == SysInfoType.SLBRatesStatus)
                {
                    SLBCurrentStatus sLBCurrentStatus = sSysInfo.GetInfo<SLBCurrentStatus>();
                    sLBCurrentStatus.ClientSLBID = sLBCurrentStatus.SLBID;
                    sLBCurrentStatus.SLBID = this.SLBID;
                    sLBCurrentStatus.OrgID = socketSession.OrgID;
                    SSysInfo sSysInfo1 = new SSysInfo(SysInfoType.SLBRatesStatus);
                    sSysInfo1.SetInfo(sLBCurrentStatus);
                    SendSysInfoToOM(sSysInfo1);
                }
            }
        }
        /// <summary>
        /// 将ClientSLB发给自助机的 转发给OMC
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        private void SSLBWebSocketClientSLB_OMC_and_ClientEvent(WebSocketSessionSLB socketSession,int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            if (cdOMSWebSocket.ContainsKey(Head.OMCID))
            {
                cdOMSWebSocket[Head.OMCID].Send(CCN, Head, bInfo);
            }

        }
        #endregion  医院ClientSLB客户端操作End
     
        #region 管理端相关操作 OperationMaintenance
        /// <summary>
        /// 登录验证通过
        /// </summary>
        void WebSocketOperationMaintenancePassLgoin(WebSocketSessionSLB sSLBWebSocket, SSysRegisterInfo sSysRegister)
        {
            sSLBWebSocket.DataInteriorSysInfoEvent += SSLBOMCWebSocket_DataInteriorSysInfoEvent;
            sSLBWebSocket.OMC_and_ClientEvent += SSLBWebSocket_OMC_and_ClientEvent;
            sSLBWebSocket.DatasSysInfoEvent += SSLBWebSocket_DatasSysInfoEvent;
            sSLBWebSocket.ReciveManageRequestSLBInfoEvent += SSLBWebSocket_ReciveManageRequestSLBInfoEvent;
            sSLBWebSocket.UDID = sSysRegister.ID;

            if (cdOMSWebSocket.ContainsKey(sSysRegister.ID) && cdOMSWebSocket[sSysRegister.ID].SessionID != sSLBWebSocket.SessionID)//是不同连接
            {
                WebSocketSessionSLB sSLB = cdOMSWebSocket[sSysRegister.ID];
                string oldSessionID = cdOMSWebSocket[sSysRegister.ID].SessionID;
                if (cdBusServerWebSocket.ContainsKey(oldSessionID))
                {
                    WebSocketSessionSLB sSLBWebSockeold2;
                    cdBusServerWebSocket.TryRemove(oldSessionID, out sSLBWebSockeold2);
                }

                string Message = string.Format("{0}:OMS ID {1} IP{5},ID {6} WebSocket 重复上线，删除并关闭之前连接 IP{2},ID {3} ;当前在线数：{4}", DateTime.Now.ToShortDateString() + " "
    + DateTime.Now.ToShortTimeString(), sSysRegister.ID, sSLB.SocketSession.RemoteEndPoint.ToString(), sSLB.SocketSession.SessionID, cdOMSWebSocket.Count
    , sSLBWebSocket.SocketSession.RemoteEndPoint.ToString(), sSLBWebSocket.SocketSession.SessionID);
                ConsoleColor colorold2 = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Message);
                Console.ForegroundColor = colorold2;
                PasSLog.WebSocketLog("WebSocketPassLgoin", Message);
                sSLB.SocketSession.CloseAsync(CloseReason.RemoteClosing);
               
                sSLB.SocketSession = sSLBWebSocket.SocketSession;
                if (!cdBusServerWebSocket.ContainsKey(sSLB.SessionID))
                {
                    cdBusServerWebSocket.TryAdd(sSLB.SessionID, sSLB);
                }

              
            }
            else
            {
                if (!cdBusServerWebSocket.ContainsKey(sSLBWebSocket.SessionID))
                {
                    cdBusServerWebSocket.TryAdd(sSLBWebSocket.SessionID, sSLBWebSocket);
                }

                if (!cdOMSWebSocket.ContainsKey(sSysRegister.ID))
                {
                    cdOMSWebSocket.TryAdd(sSysRegister.ID, sSLBWebSocket);
                }
                else
                {
                    cdOMSWebSocket[sSysRegister.ID].SocketSession = sSLBWebSocket.SocketSession;
                }
            }
            //发送消息给客户端
            SSysInfo sSysInfoRet = new SSysInfo(SysInfoType.RegisterRet);
            SSysRegisterRetInfo sSysRegisterRetInfo = new SSysRegisterRetInfo(MyPubConstant.SLBID, sSysRegister.ID, sSLBWebSocket.SocketSession.RemoteEndPoint.ToString().Split(':')[0], int.Parse(sSLBWebSocket.SocketSession.RemoteEndPoint.ToString().Split(':')[1]));
            sSysRegisterRetInfo.Register_Identity = sSysRegister.Register_Identity;
            sSysRegisterRetInfo.SessionID = sSLBWebSocket.SessionID;
            sSysRegisterRetInfo.ReslutCode = 1;

            sSysInfoRet.SetInfo(sSysRegisterRetInfo);
            sSLBWebSocket.SendSysInfo(sSysInfoRet);
            ConsoleColor colorold = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{ DateTime.Now.ToShortTimeString()}:管理控制台 ID:[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{sSysRegister.ID}");
            Console.ForegroundColor = ConsoleColor.White ;
            Console.Write($"]IP:{sSLBWebSocket.SocketSession.RemoteEndPoint.ToString()} ");
            Console.WriteLine($" 连接成,当前管理台数量:{cdOMSWebSocket.Count} ");
             
            Console.ForegroundColor = colorold;
          
        }
        



        /// <summary>
        /// 将OMC发给自助机的 转发给自助机或者医院SLB
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        private void SSLBWebSocket_OMC_and_ClientEvent(WebSocketSessionSLB socketSession, int CCN, OMCandZZJInfoHead zHead, byte[] bInfo)
        {

            if (!string.IsNullOrEmpty(zHead.OrgID))
            {//发给医院的SLB的
                zHead.OMCID = socketSession.UDID;
                 
                if (zHead.OrgID == HCCNValye.SendToALLTag)//发给所有医院
                {
                    foreach (string hos_id in cdCPaaSWebSocket.Keys)
                    {
                        ConcurrentDictionary<string, WebSocketSessionSLB> keyValues = cdCPaaSWebSocket[hos_id];
                        zHead.OrgID = hos_id;
                        foreach (WebSocketSessionSLB sessionSLB in keyValues.Values)
                        {
                            sessionSLB.Transpond(CCN, JsonConvert.SerializeObject(zHead), bInfo);
                        }
                    }
                }
                else if (cdCPaaSWebSocket.ContainsKey(zHead.OrgID))//发给单个医院
                {
                    if (!string.IsNullOrEmpty(zHead.SLBID))//发给指定SLB
                    {
                        if (cdCPaaSWebSocket[zHead.OrgID].ContainsKey(zHead.SLBID))
                            cdCPaaSWebSocket[zHead.OrgID][zHead.SLBID].Send(CCN, zHead, bInfo); 
                    }
                    else
                    {
                        foreach (WebSocketSessionSLB sessionSLB in cdCPaaSWebSocket[zHead.OrgID].Values)
                        {
                            sessionSLB.Send(CCN, zHead, bInfo);
                        }
                    }
                }

                return;
            }
       
            else if (!string.IsNullOrEmpty(zHead.ZZJID) && cdClientWebSocket.ContainsKey(zHead.ZZJID))
            {
                cdClientWebSocket[zHead.ZZJID].Send(CCN, zHead, bInfo);
            }

        }
        /// <summary>
        /// 发送消息给OperationMaintenance
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        private void SendSysInfoToOM(SSysInfo sSysInfo)
        {
            Task.Run(() =>
            {
                try
                {
                    foreach (WebSocketSessionSLB sessionSLB   in cdOMSWebSocket.Values)
                    {
                        try
                        {
                            if (sessionSLB != null && sessionSLB.Connected) ;
                            sessionSLB.SendSysInfo(sSysInfo);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
                return true;
            });
        }

        private void SSLBWebSocket_DatasSysInfoEvent(WebSocketSessionSLB socketSession, int CCN, string Head, byte[] bInfo)
        {
            SLBSysInofRates++;
               OMCandZZJInfoHead zHead = new OMCandZZJInfoHead(Head);
           
            if (!string.IsNullOrEmpty(zHead.OrgID))
            {//发给医院的SLB的
                zHead.OMCID = socketSession.UDID;
                if (zHead.OrgID == HCCNValye.SendToALLTag )//发给所有医院
                {
                    foreach (string hos_id in cdCPaaSWebSocket.Keys)
                    {
                        ConcurrentDictionary<string, WebSocketSessionSLB> keyValues = cdCPaaSWebSocket[hos_id];
                        zHead.OrgID = hos_id;
                        foreach (WebSocketSessionSLB sessionSLB in keyValues.Values)
                        {
                            sessionSLB.Transpond(CCN, JsonConvert.SerializeObject(zHead), bInfo);
                        }
                    }
                }
                else if (cdCPaaSWebSocket.ContainsKey(zHead.OrgID))//发给单个医院
                {
                    if (!string.IsNullOrEmpty(zHead.SLBID))//发给指定SLB
                    {
                        if (cdCPaaSWebSocket[zHead.OrgID].ContainsKey(zHead.SLBID))
                            cdCPaaSWebSocket[zHead.OrgID][zHead.SLBID].Transpond(CCN, Head, bInfo);
                    }
                    else
                    {
                        foreach (WebSocketSessionSLB sessionSLB in cdCPaaSWebSocket[zHead.OrgID].Values)
                        {
                            sessionSLB.Transpond(CCN, Head, bInfo);
                        }
                    }
                }


                return;
            }
        }

        private void DealOMCSysInfo(OMCandZZJInfoHead zHead, SSysInfo sSysInfo, bool FromSLB, WebSocketSessionSLB socketSession)
        {
            if (sSysInfo.InfoType == SysInfoType.OtherSLBChnage)
            {//  通知在线BusServer 连接到对应SLB 
                //Console.ForegroundColor = ConsoleColor.DarkCyan;
                //Console.WriteLine(string.Format("{0}Recive OMC OtherSLBChnage  {1}     \r\n", DateTime.Now, sSysInfo.Get_Info()));
                //if (cdBusServer != null)
                //{
                //    foreach (TcpBusServerInfo ts in cdBusServer.Values)
                //    {
                //        BusSendSysInfo(ts, sSysInfo);
                //        Random rnd = new Random(Guid.NewGuid().GetHashCode());
                //        Thread.Sleep(rnd.Next(100));
                //    }
                //}
            }
            else if (sSysInfo.InfoType == SysInfoType.SLBOffline)
            {//让当前SLB下线
                Stop();
                if (sSysInfo.Info == "ReStart")
                {
                    //ReStartSLB_EventHandler?.Invoke();
                }
            }

            else if (sSysInfo.InfoType == SysInfoType.AddOrUpdateSingleBusServerInfo || sSysInfo.InfoType == SysInfoType.RemoveSingleBusServerInfo)
            { //添加、更新或移除单个BusServer上的BusInfo

                SingleBusServerInfo singleBusServerInfo = sSysInfo.GetInfo<SingleBusServerInfo>();

                if (singleBusServerInfo.BusServerID == MyPubConstant.BusServerID)
                {
                    if (sSysInfo.InfoType == SysInfoType.AddOrUpdateSingleBusServerInfo)
                    {
                        AddOrUpdateSingleBusBusiness(singleBusServerInfo);
                    }
                    else if (sSysInfo.InfoType == SysInfoType.RemoveSingleBusServerInfo)
                    {
                        RemoveSingleBusBusiness(singleBusServerInfo);
                    }
                }
            }
            else if (sSysInfo.InfoType == SysInfoType.UpdateBusinessInfo || sSysInfo.InfoType == SysInfoType.AddOrUpdateBusinessTestVersion)
            {//更新单个BusInfo 的版本或测试版本

                UpdateBusBusinessInfoOrVersion(sSysInfo.Info);
            }
            else if (sSysInfo.InfoType == SysInfoType.AddOrUpdateBusinessInfoVerOnline)//增加或修改某一业务版本
            {
                BusinessInfoVerOnline businessInfoOnline = sSysInfo.GetInfo<BusinessInfoVerOnline>();

                //GSLBS.AddOrUpdateBusInfoVer(businessInfoOnline);

                //  AddOrUpdateCacheData(businessInfoOnline);--------------------------------------------------------------------------------------------------------------------------
            }
            else if (sSysInfo.InfoType == SysInfoType.RemoveBusinessInfoVerOnline)//停止某一业务版本
            {
                //BusinessInfoVerOnline businessInfoOnline = JsonConvert.DeserializeObject<BusinessInfoVerOnline>(sSysInfo.Info);

                //GSLBS.RemoveBusInfoVer(businessInfoOnline);

                //  AddOrUpdateCacheData(businessInfoOnline);--------------------------------------------------------------------------------------------------------------------------
            }
            else if (sSysInfo.InfoType == SysInfoType.AddOrUpdateSingleBusServerInfoVer || sSysInfo.InfoType == SysInfoType.StopSingleBusServerInfoVer)
            { //添加、更新或移除单个BusServer上的BusInfo 下的BusVersion add by wanglei  20200211  
                SingleBusinessInfoVer singleBusServerInfo = sSysInfo.GetInfo<SingleBusinessInfoVer>();
                if (singleBusServerInfo.BusServerID == MyPubConstant.BusServerID)
                {
                    if (sSysInfo.InfoType == SysInfoType.AddOrUpdateSingleBusServerInfoVer)
                    {
                        AddOrUpdateSingleBusBusinessVer(singleBusServerInfo);
                    }
                    else if (sSysInfo.InfoType == SysInfoType.StopSingleBusServerInfoVer)
                    {
                        RemoveSingleBusBusinessVer(singleBusServerInfo);
                    }
                }
            }
            else if (sSysInfo.InfoType == SysInfoType.UpdateBusinessInfoVer)
            {//更新单个BusInfo 的BusVersion 版本或测试版本 add by wanglei 20200211
                SingleBusinessInfoVer singleBusServerInfo = sSysInfo.GetInfo<SingleBusinessInfoVer>();
                UpdateBusBusinessInfoOrVersion(singleBusServerInfo.BusID, singleBusServerInfo.BusVersion);
            }
            else if (sSysInfo.InfoType == SysInfoType.AddOrUpdateBusinessTestVersion)
            {
                SBusIDBusVersion sBusIDBus = sSysInfo.GetInfo<SBusIDBusVersion>();
                AddOrUpdateBusinessTestVersion(sBusIDBus.BusID, sBusIDBus.BusVersion);
            }

            else if (sSysInfo.InfoType == SysInfoType.IinitializeDefaultTestMark || sSysInfo.InfoType == SysInfoType.AddorUpdateBusStatusAndTestMark)
            {//更新测试组标记 ，所有BusServer 都进行
                //if (cdBusServer != null)
                //{
                //    foreach (TcpBusServerInfo ts in cdBusServer.Values)
                //    {
                //        SendSysInfo(ts.TcpClient, sSysInfo);
                //        Random rnd = new Random(Guid.NewGuid().GetHashCode());
                //        Thread.Sleep(rnd.Next(100));
                //    }
                //}
                if (sSysInfo.InfoType == SysInfoType.IinitializeDefaultTestMark)
                {//更新默认测试组标记
                    IinitializeDefaultTestMark();
                }
                else if (sSysInfo.InfoType == SysInfoType.AddorUpdateBusStatusAndTestMark)
                {//更新指定BusID测试组标记 

                    SBusIDBusVersion sBusIDBus = sSysInfo.GetInfo<SBusIDBusVersion>();
                    AddorUpdateBusStatusAndTestMark(sBusIDBus);
                }
            }
            else if (sSysInfo.InfoType == SysInfoType.AddOrUpdateBusinessInfoOnline)
            {
                //BusinessInfoOnline businessInfoOnline = JsonConvert.DeserializeObject<BusinessInfoOnline>(sSysInfo.Info);

                //GSLBS.AddOrUpdateBusInfo(businessInfoOnline);

                //AddOrUpdateCacheData(businessInfoOnline);
            }
            else if (sSysInfo.InfoType == SysInfoType.RemoveBusinessInfoOnline)
            {
                //GSLBS.RemoveBusNameVer(sSysInfo.Info);
                //RemoveCacheData(sSysInfo.Info);
            }
            else if (sSysInfo.InfoType == SysInfoType.UpdateSingleBusServerHTTPBus)
            {
                //更新指定BusServer的HTTP服务允许接收的业务 < see cref = "UpSBusServerHttpBus" />
                //UpSBusServerHttpBus upSBusServerHttpBus = sSysInfo.GetInfo<UpSBusServerHttpBus>();
                //if (cdBusServer != null)
                //{
                //    if (cdBusServer.ContainsKey(upSBusServerHttpBus.BusServerID))
                //        SendSysInfo(cdBusServer[upSBusServerHttpBus.BusServerID].TcpClient, sSysInfo);
                //}
            }
            else if (sSysInfo.InfoType == SysInfoType.OnLineSLBlistToBusServer)
            {// 通知指定BusServer当前在线SLB列表 < see cref = "SOnLineSLBlistToBusServer" />
                //SOnLineSLBlistToBusServer sOnLineSLBlistToBusServer
                //= sSysInfo.GetInfo<SOnLineSLBlistToBusServer>();

                //if (cdBusServer != null)
                //{
                //    if (cdBusServer.ContainsKey(sOnLineSLBlistToBusServer.BusServerID))
                //        SendSysInfo(cdBusServer[sOnLineSLBlistToBusServer.BusServerID].TcpClient, sSysInfo);
                //}
            }
            else if (sSysInfo.InfoType == SysInfoType.TrackConditions)
            {
                //STrackConditions sTC = sSysInfo.GetInfo<STrackConditions>();
                //if (sTC.Status == 1)
                //{
                //    cdTrackConditions.TryAdd(sTC.TCID, sTC);

                //    cdTrackAndOM.TryAdd(sTC.TCID, internalClient.UDID);
                //}
                //else if (cdTrackConditions.ContainsKey(sTC.TCID))
                //{
                //    cdTrackConditions.TryRemove(sTC.TCID, out sTC);
                //    if (cdTrackResult.ContainsKey(sTC.TCID))
                //    {
                //        ConcurrentDictionary<string, STrackResult> cdr;
                //        cdTrackResult.TryRemove(sTC.TCID, out cdr);
                //        cdr.Clear();
                //        cdr = null;

                //    }
                //    if (cdTrackAndOM.ContainsKey(sTC.TCID))
                //    {
                //        string OMid;
                //        cdTrackAndOM.TryRemove(sTC.TCID, out OMid);
                //    }
                //    sTC = null;
                //}

            }
            /// 添加 修改 或者移除APIinfo<see cref="SAPIList"/>
            else if (sSysInfo.InfoType == SysInfoType.UpdateSingleAPIInfo && OpenAPIACF)
            {
                //SAPIList sAPIList = sSysInfo.GetInfo<SAPIList>();
                //if (sAPIList.Mark_Stop)
                //{
                //    if (cdAPIinfo.ContainsKey(sAPIList.API_ID))
                //    {
                //        SAPIList sAPIListrem;
                //        cdAPIinfo.TryRemove(sAPIList.API_ID, out sAPIListrem);
                //        sAPIListrem = null;
                //        Console.ForegroundColor = ConsoleColor.White;
                //        Console.WriteLine(string.Format("{0} cdAPIinfo API授权列表 停用API {1} {2}   \r\n", DateTime.Now, sAPIList.API_ID, sAPIList.API_Name));
                //    }
                //    if (cdAPINameToID.ContainsKey(sAPIList.API_Name))
                //    {
                //        int idout;
                //        cdAPINameToID.TryRemove(sAPIList.API_Name, out idout);
                //    }
                //}
                //else
                //{
                //    if (cdAPIinfo.ContainsKey(sAPIList.API_ID))
                //    {
                //        cdAPIinfo[sAPIList.API_ID] = sAPIList;
                //    }
                //    else
                //    {
                //        cdAPIinfo.TryAdd(sAPIList.API_ID, sAPIList);
                //        Console.ForegroundColor = ConsoleColor.White;
                //        Console.WriteLine(string.Format("{0} cdAPIinfo API授权列表 增加API {1} {2}   \r\n", DateTime.Now, sAPIList.API_ID, sAPIList.API_Name));
                //    }
                //    if (!cdAPINameToID.ContainsKey(sAPIList.API_Name))
                //    {
                //        cdAPINameToID.TryAdd(sAPIList.API_Name, sAPIList.API_ID);
                //    }
                //}
            }
            /// 添加 修改 或者移除APIUserinfo<see cref="SAPIUserInfo"/>
            else if (sSysInfo.InfoType == SysInfoType.UpdateSingleAPIUserInfo)
            {
                //SAPIUserInfo sAPIUserInfo = sSysInfo.GetInfo<SAPIUserInfo>();
                //if (sAPIUserInfo.Mark_Stop)
                //{
                //    if (sAPIUserInfo.AType == 1)
                //    {
                //        if (cdAPIUser.ContainsKey(sAPIUserInfo.APIU_ID))
                //        {
                //            SAPIUserInfoCKey sAPIUserInfodel;
                //            cdAPIUser.TryRemove(sAPIUserInfo.APIU_ID, out sAPIUserInfodel);
                //            Console.ForegroundColor = ConsoleColor.White;
                //            Console.WriteLine(string.Format("{0} cdAPIUser 移除API用户 {1} {2}   \r\n", DateTime.Now, sAPIUserInfodel.APIU_ID, sAPIUserInfodel.APIU_Name));
                //            sAPIUserInfodel = null;
                //        }
                //    }
                //    else if (sAPIUserInfo.AType == 2)
                //    {
                //        if (cdAPIUserSocketBase.ContainsKey(sAPIUserInfo.APIU_ID))
                //        {
                //            SAPIUserInfoCKey sAPIUserInfodel;
                //            cdAPIUserSocketBase.TryRemove(sAPIUserInfo.APIU_ID, out sAPIUserInfodel);
                //            Console.ForegroundColor = ConsoleColor.White;
                //            Console.WriteLine(string.Format("{0} cdAPIUserSocketBase 移除API用户 {1} {2}   \r\n", DateTime.Now, sAPIUserInfodel.APIU_ID, sAPIUserInfodel.APIU_Name));
                //            sAPIUserInfodel = null;
                //        }
                //    }
                //}
                //else
                //{
                //    if (sAPIUserInfo.AType == 1)
                //    {
                //        AddOrUpdateAPIUserInfo(sAPIUserInfo, ref cdAPIUser);
                //        Console.ForegroundColor = ConsoleColor.White;
                //        Console.WriteLine(string.Format("{0} cdAPIUser 增加或更新API用户 {1} {2}   \r\n", DateTime.Now, sAPIUserInfo.APIU_ID, sAPIUserInfo.APIU_Name));
                //    }
                //    else if (sAPIUserInfo.AType == 2)
                //    {
                //        AddOrUpdateAPIUserInfo(sAPIUserInfo, ref cdAPIUserSocketBase);
                //        Console.ForegroundColor = ConsoleColor.White;
                //        Console.WriteLine(string.Format("{0} cdAPIUserSocketBase 增加或更新API用户 {1} {2}   \r\n", DateTime.Now, sAPIUserInfo.APIU_ID, sAPIUserInfo.APIU_Name));
                //    }

                //}
            }
            ///API用户增加或者重新加载访问API权限<see cref="SUpdateAPIUserSaddAPI"/>
            else if (sSysInfo.InfoType == SysInfoType.UpdateAPIUserSaddAPI && OpenAPIACF)
            {
                //SUpdateAPIUserSaddAPI sAPIUserSaddAPI = sSysInfo.GetInfo<SUpdateAPIUserSaddAPI>();

                //if (sAPIUserSaddAPI.Type == 1)
                //{
                //    foreach (string APIU_ID in sAPIUserSaddAPI.UserList)
                //    {
                //        if (cdAPIUserToApi.ContainsKey(APIU_ID))
                //        {
                //            List<int> listAPI = cdAPIUserToApi[APIU_ID];
                //            foreach (int API_ID in sAPIUserSaddAPI.APIList)
                //            {
                //                if (!listAPI.Contains(API_ID))
                //                { listAPI.Add(API_ID); }
                //                Console.ForegroundColor = ConsoleColor.White;
                //                Console.WriteLine(string.Format("{0} cdAPIUserToApi 增加API用户 {1} 权限 {2}   \r\n", DateTime.Now, APIU_ID, API_ID));
                //            }
                //        }
                //        else
                //        {
                //            List<int> listAPI = new List<int>();
                //            foreach (int API_ID in sAPIUserSaddAPI.APIList)
                //            {
                //                if (!listAPI.Contains(API_ID))
                //                { listAPI.Add(API_ID); }
                //                Console.ForegroundColor = ConsoleColor.White;
                //                Console.WriteLine(string.Format("{0} cdAPIUserToApi 增加API用户 {1} 权限 {2}   \r\n", DateTime.Now, APIU_ID, API_ID));
                //            }
                //            cdAPIUserToApi.TryAdd(APIU_ID, listAPI);
                //        }
                //    }
                // }
                // //else
                //{
                //    Apilist apilist = new Apilist();
                //    foreach (string APIU_ID in sAPIUserSaddAPI.UserList)
                //    {
                //        List<int> listAPI = apilist.GetAPIUserAPIList(APIU_ID);
                //        if (cdAPIUserToApi.ContainsKey(APIU_ID))
                //        {
                //            cdAPIUserToApi[APIU_ID] = listAPI;
                //        }
                //        else
                //        {
                //            cdAPIUserToApi.TryAdd(APIU_ID, listAPI);
                //        }
                //        Console.ForegroundColor = ConsoleColor.White;
                //        Console.WriteLine(string.Format("{0} cdAPIUserToApi 更新API用户 {1} 权限   \r\n", DateTime.Now, APIU_ID));
                //    }
                //}


                /// <summary> 
                /// API用户对应的API授权表
                /// key:APIU_ID      APIID 
                /// </summary>
                //   ConcurrentDictionary<string, List<int>> cdAPIUserToApi = new ConcurrentDictionary<string, List<int>>();
            }
            else if (sSysInfo.InfoType == SysInfoType.UpdateSysInfo)
            {
                //SUpdateSysInfo sUpdateSysInfo = sSysInfo.GetInfo<SUpdateSysInfo>();

                //if (sUpdateSysInfo != null)
                //{
                //    if (string.Compare(sUpdateSysInfo.SysID, "RSAPrivateKey", true) == 0)
                //    {
                //        SignEncryptHelper.RSAPrivateKey = sUpdateSysInfo.SysValue;
                //    }
                //    else if (string.Compare(sUpdateSysInfo.SysID, "RSAPublicKey", true) == 0)
                //    {
                //        SignEncryptHelper.RSAPublicKey = sUpdateSysInfo.SysValue;
                //    }
                //    else if (string.Compare(sUpdateSysInfo.SysID, "RSAPriDeprecated", true) == 0)
                //    {
                //        SignEncryptHelper.RSAPriDeprecated = sUpdateSysInfo.SysValue;
                //    }
                //}
            }
            else if (sSysInfo.InfoType == SysInfoType.ClientSystemAndProcessInfo)
            {
                //GetSClientSystemAndProcessInfo getSClient = sSysInfo.GetInfo<GetSClientSystemAndProcessInfo>();
                //if (getSClient.FromClientSLB == false)
                //{
                    List<ClientSystemAndProcessInfo> list = new List<ClientSystemAndProcessInfo>();
                    
                       
                    foreach (WebSocketSessionSLB sessionSLB in cdClientWebSocket.Values)
                    {
                    if (sessionSLB.SystemAnd_ProcessInfo != null)
                    {
                        ClientSystemAndProcessInfo clientSystemAndProcessInfo = new ClientSystemAndProcessInfo(sessionSLB.SystemAnd_ProcessInfo);
                        clientSystemAndProcessInfo.UDID = sessionSLB.UDID;
                        clientSystemAndProcessInfo.UDName = sessionSLB.UDID;
                        clientSystemAndProcessInfo.SLBID = SLBID;

                        clientSystemAndProcessInfo.IPAddress = sessionSLB.SocketSession.RemoteEndPoint.ToString().Split(':')[0];

                            list.Add(clientSystemAndProcessInfo);
                        }
                    }
                    if (list.Count > 0)
                    {
                        sSysInfo.SetInfoEncrypt(list);
                        if (FromSLB)
                            webSocket4NetSpring.SendSysInfo(sSysInfo, zHead);
                        else
                            socketSession.SendSysInfo(sSysInfo, zHead);
                    }
                //}
                //else
                //{
                //    if (cdCSLBWebSocket.Count > 0)
                //    {
                //        SSysInfo sSysInfo1 = new SSysInfo(SysInfoType.ClientSystemAndProcessInfo);
                //        getSClient.FBOSLBID = this.SLBID;
                //        getSClient.FBOOMCID = socketSession.UDID;
                //        sSysInfo.SetInfoEncrypt(getSClient);
                //        if (string.IsNullOrEmpty(getSClient.HOS_ID))
                //        {
                //            foreach (string hos_id in cdCSLBWebSocket.Keys)
                //            {
                //                getSClient.HOS_ID = hos_id;
                //                ConcurrentDictionary<string, WebSocketSessionSLB> keyValues = cdCSLBWebSocket[hos_id];
                //                foreach (WebSocketSessionSLB sessionSLB in keyValues.Values)
                //                {
                //                    sSysInfo1.SetInfoEncrypt(getSClient);
                //                    if (FromSLB)
                //                        webSocket4NetSpring.SendSysInfo(sSysInfo1, zHead);
                //                    else
                //                        socketSession.SendSysInfo(sSysInfo1, zHead);
                                    
                //                }
                //            }
                //        }
                //    }
                //}

            }

        }
        private void SSLBOMCWebSocket_DataInteriorSysInfoEvent(WebSocketSessionSLB socketSession, OMCandZZJInfoHead zHead, SSysInfo sSysInfo)
        {

            if (!string.IsNullOrEmpty(zHead.OrgID) )
            {//发给医院的SLB的
                zHead.OMCID = socketSession.UDID;
                if (zHead.OrgID == HCCNValye.SendToALLTag)//发给所有医院
                {
                    foreach (string hos_id in cdCPaaSWebSocket.Keys)
                    {
                       
                        ConcurrentDictionary<string, WebSocketSessionSLB> keyValues = cdCPaaSWebSocket[hos_id];
                        zHead.OrgID = hos_id;
                        foreach (WebSocketSessionSLB sessionSLB in keyValues.Values)
                        {
                            sessionSLB.SendSysInfo(sSysInfo, zHead);
                        }
                    }
                }
                else if(cdCPaaSWebSocket.ContainsKey(zHead.OrgID))//发给单个医院
                {
                    if (!string.IsNullOrEmpty(zHead.SLBID))//发给指定SLB
                    {
                        if (cdCPaaSWebSocket[zHead.OrgID].ContainsKey(zHead.SLBID))
                            cdCPaaSWebSocket[zHead.OrgID][zHead.SLBID].SendSysInfo(sSysInfo, zHead);
                    }
                    else
                    {
                        foreach (WebSocketSessionSLB sessionSLB in cdCPaaSWebSocket[zHead.OrgID].Values)
                        {
                            sessionSLB.SendSysInfo(sSysInfo, zHead);
                        }
                    }
                }
               

                return;
            }
           
            else
            {
                DealOMCSysInfo(zHead, sSysInfo,false , socketSession);
            }
        }


        /// <summary>
        /// 管理端调用SLB的常规业务
        /// </summary>
        /// <param name="socketSession"></param>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        private void SSLBWebSocket_ReciveManageRequestSLBInfoEvent(WebSocketSessionSLB socketSession, int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            if (CCN == 5009999)//测试
            {
                string messagerec = socketSession.Encoding.GetString(bInfo);
                Console.WriteLine("recive：  " + messagerec);
                string messageret = "服务端返回500数据" + DateTime.Now;
                for (int i = 0; i < 1000; i++)
                {
                    messageret += i.ToString() + "服务端返回500数据";
                }
                if (socketSession != null && socketSession.Connected)
                {
                    socketSession.Send(CCN, Head, messageret);//Header 和CCN 不变
                }
            }
            else
            {//和管理端之间业务处理处理业务
                ProcessingSLB_DataInfo(socketSession, CCN, Head, bInfo);
            }
        }
        #endregion

    

        #region 自助机客户端交互相关代码
        /// <summary>
        /// 登录验证通过
        /// </summary>
        void WebSocketClinetPassLgoin(WebSocketSessionSLB sSLBWebSocket, SSysRegisterInfo sSysRegister)
        {
            sSLBWebSocket.DataClientCallServerHeadEvent += SSLBWebSocket_DataClientCallServerHeadEvent;
            sSLBWebSocket.OMC_and_ClientEvent += SSLBWebSocket_Client_and_OMCEvent;
            sSLBWebSocket.UDID = sSysRegister.ID;
            
            if (cdClientWebSocket.ContainsKey(sSysRegister.ID) && cdClientWebSocket[sSysRegister.ID].SessionID != sSLBWebSocket.SessionID)//是不同连接
            {
                WebSocketSessionSLB sSLB = cdClientWebSocket[sSysRegister.ID];
                string oldSessionID = cdClientWebSocket[sSysRegister.ID].SessionID;
                if (cdBusServerWebSocket.ContainsKey(oldSessionID))
                {
                    WebSocketSessionSLB sSLBWebSockeold2;
                    cdBusServerWebSocket.TryRemove(oldSessionID, out sSLBWebSockeold2);
                }
                sSLB.SocketSession = sSLBWebSocket.SocketSession;
                if (!cdBusServerWebSocket.ContainsKey(sSLB.SessionID))
                {
                    cdBusServerWebSocket.TryAdd(sSLB.SessionID, sSLB);
                }

                string Message = string.Format("{0}:客户端用户ID {1} IP{5},ID {6} WebSocket 重复上线，删除并关闭之前连接 IP{2},ID {3} ;当前在线数：{4}", DateTime.Now.ToShortDateString() + " "
      + DateTime.Now.ToShortTimeString(), sSysRegister.ID, sSLB.SocketSession.RemoteEndPoint.ToString(), sSLB.SocketSession.SessionID, cdClientWebSocket.Count + 1
      , sSLBWebSocket.SocketSession.RemoteEndPoint.ToString(), sSLBWebSocket.SocketSession.SessionID);
                ConsoleColor colorold = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Message);
                Console.ForegroundColor = colorold;
                PasSLog.WebSocketLog("WebSocketPassLgoin", Message);
                sSLB.SocketSession.CloseAsync(CloseReason.LocalClosing);
                sSLB = null;
            }
            else
            {
                if (!cdBusServerWebSocket.ContainsKey(sSLBWebSocket.SessionID))
                {
                    cdBusServerWebSocket.TryAdd(sSLBWebSocket.SessionID, sSLBWebSocket);
                }

                if (!cdClientWebSocket.ContainsKey(sSysRegister.ID))
                {
                    cdClientWebSocket.TryAdd(sSysRegister.ID, sSLBWebSocket);
                }
                else
                {
                    cdClientWebSocket[sSysRegister.ID].SocketSession = sSLBWebSocket.SocketSession;
                }
            }
            //发送消息给客户端
            SSysInfo sSysInfoRet = new SSysInfo(SysInfoType.RegisterRet);
            SSysRegisterRetInfo sSysRegisterRetInfo = new SSysRegisterRetInfo(MyPubConstant.SLBID, sSysRegister.ID, sSLBWebSocket.SocketSession.RemoteEndPoint.ToString().Split(':')[0], int.Parse(sSLBWebSocket.SocketSession.RemoteEndPoint.ToString().Split(':')[1]));
            sSysRegisterRetInfo.Register_Identity = sSysRegister.Register_Identity;
          sSysRegisterRetInfo.SessionID = sSLBWebSocket.SessionID;
            sSysRegisterRetInfo.ReslutCode = 1;
          
            sSysInfoRet.SetInfo(sSysRegisterRetInfo);
            sSLBWebSocket.SendSysInfo(sSysInfoRet);

            string Message2 = string.Format("{0}:客户端用户ID {1} IP{2},ID {3} WebSocket ;当前在线数：{4}", DateTime.Now.ToShortTimeString(), sSysRegister.ID, sSLBWebSocket.SocketSession.RemoteEndPoint.ToString(), sSLBWebSocket.SocketSession.SessionID, cdClientWebSocket.Count);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Message2);

        }
        /// <summary>
        /// 将自助机发给OMC的信息转发给OMC 或公司SLB
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>

        private void SSLBWebSocket_Client_and_OMCEvent(WebSocketSessionSLB socketSession,int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            if (!string.IsNullOrEmpty(Head.OrgID)  &&webSocket4NetSpring.Connected )
            {//发给医院的SLB的
                webSocket4NetSpring.Send(CCN, Head, bInfo);
            }
            
         else   if (cdOMSWebSocket.ContainsKey(Head.OMCID))
            {
                cdOMSWebSocket[Head.OMCID].Send(CCN, Head,bInfo);
            }
        }

        /// <summary>
        /// 处理客户端发来的消息
        /// </summary>
        /// <param name="socketSession"></param>
        /// <param name="CCN"></param>
        /// <param name="sLBInfoHead"></param>
        /// <param name="bInfo"></param>
        private void SSLBWebSocket_DataClientCallServerHeadEvent(WebSocketSessionSLB socketSession, int CCN, OMCandZZJInfoHead sLBInfoHead, byte[] bInfo)
        {
            ClientToSLBRates++;
            if (CCN ==20000000)//测试
            {
                string messagerec = socketSession.Encoding.GetString(bInfo);
                Console.WriteLine("recive：  " + messagerec);
                string messageret = "服务端返回数据" + DateTime.Now;
                if (socketSession != null && socketSession.Connected)
                {
                    socketSession.Send(CCN,sLBInfoHead, messageret );//Header 和CCN 不变
                }
            }
            else
            {//和自助机之间业务处理处理业务
                ProcessingSLB_DataInfo(socketSession,   CCN,   sLBInfoHead,  bInfo);
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        public void CallTest()
        {
            int ccn = 10000000;
            SLBInfoHead sLBInfoHead = new SLBInfoHead();
            foreach (WebSocketSessionSLB sessionSLB in cdBusServerWebSocket.Values)
            {
                string sendMessage = "服务端发给发给客户端" + DateTime.Now;
                Console.WriteLine("Send:" + sendMessage);

                string result = sessionSLB.Call(ccn, sLBInfoHead, sendMessage);
                Console.WriteLine("Recive:" + sendMessage);
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        public void SendTest()
        {
            int ccn = 10000000;
            foreach (WebSocketSessionSLB sessionSLB in cdBusServerWebSocket.Values)
            {
                OMCandZZJInfoHead sLBInfoHead = new OMCandZZJInfoHead();
                string sendMessage = "服务端发给发给客户端" + DateTime.Now;
                Console.WriteLine("Send:" + sendMessage);
                sessionSLB.Send(ccn, sLBInfoHead, sendMessage);
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        public void SendFileTest()
        {
            string path = "FileS";
            //获得当前目录下的所有文件 
            DirectoryInfo directoryInfo = new DirectoryInfo( Path.Combine ( System.AppDomain.CurrentDomain.BaseDirectory, path));//创建目录对象。
            FileInfo[] files;
            try
            {
                files = directoryInfo.GetFiles();
            }
            catch
            {
                return;
            }
         
            //文件的cTime和访问时间。
            foreach (FileInfo fileInfo in files)
            {
                FileInfoHead fileInfoHead = new FileInfoHead();
                string name = fileInfo.Name;
               
                   // FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(directoryInfo.FullName , name));
                fileInfoHead.FileName = fileInfo.Name;
                fileInfoHead.FilePath = path;
                fileInfoHead.LastWriteTime = fileInfo.LastWriteTime;
                fileInfoHead.CreateTime = fileInfo.CreationTime;
                using (FileStream fs = new FileStream(Path.Combine(directoryInfo.FullName, name), FileMode.Open, FileAccess.Read))
                {
                    BinaryReader r = new BinaryReader(fs);
                    byte[] fileArray = r.ReadBytes((int)fs.Length);
                    fs.Dispose();
                    foreach (WebSocketSessionSLB sessionSLB in cdBusServerWebSocket.Values)
                    {
                         
                        Console.WriteLine($"Send file:{name}" );
                        sessionSLB.SendFile(fileInfoHead, fileArray);
                    }
                }
            }

            
        }

      
        #endregion 客户端交互相关代码





    }

  
}
