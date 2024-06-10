using System;
using System.Collections.Generic;
using System.Text;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.WebSocket.Server;
using PasS.Base.Lib;
using System.Buffers;

namespace PaaS.Comm
{
    /// <summary>
    ///  接收到包含Head数据
    /// </summary>
    /// <param name="assignServer"></param>
    /// <param name="sLBInfoHeadBusS"></param>
    /// <param name="bInfo"></param>

    public delegate void DataClientCallServerHeadEventHandler(WebSocketSessionSLB  socketSession, Int32 CCN, OMCandZZJInfoHead sLBInfoHeadBusS, byte[] bInfo);

    /// <summary>
    /// 接收到包含系统消息 数据
    /// </summary>
    /// <param name="webSocket4NetClent"></param>
    /// <param name="CCN"></param>
    public delegate void DatasSysInfoEventWHandler(WebSocketSessionSLB socketSession, Int32 CCN, string Head, byte[] bInfo);

    /// <summary>
    /// 接收到包含系统消息<see cref="SSysInfo"/>数据
    /// </summary>
    /// <param name="webSocket4NetClent"></param>
    /// <param name="sSysInfo"></param>
    public delegate void DataInteriorSysInfoEventWHandler(WebSocketSessionSLB socketSession, OMCandZZJInfoHead sLBInfoHead  , SSysInfo sSysInfo);
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="Data"></param>
    public delegate void RegisterEventHandler(SSysRegisterInfo sSysRegisterInfo, WebSocketSessionSLB socketSessionSLB);

    /// <summary>
    ///  收到 控制端和自助机之间通讯
    /// </summary>
    /// <param name="CCN"></param>
    /// <param name="Head"></param>
    /// <param name="bInfo"></param>
    public delegate void OMC_and_ClientEventHandler(WebSocketSessionSLB socketSession,int CCN, OMCandZZJInfoHead Head, byte[] bInfo);

    /// <summary>
    ///  收到 自助机管理端调用SLB 500-599
    /// </summary>
    /// <param name="CCN"></param>
    /// <param name="Head"></param>
    /// <param name="bInfo"></param>
    public delegate void ReciveManageRequestSLBInfoEventHandler(WebSocketSessionSLB socketSession, int CCN, OMCandZZJInfoHead Head, byte[] bInfo);
    
    public class WebSocketSessionSLB : WebSocketSessionClientBase
    {
        public event RegisterEventHandler RegisterEvent;
        /// <summary>
        /// Server接收到客户端主动发送的数据
        /// </summary>

        public event DataClientCallServerHeadEventHandler DataClientCallServerHeadEvent;
        /// <summary>
        /// 接收到包含系统消息 数据
        /// </summary>
        public event DatasSysInfoEventWHandler DatasSysInfoEvent;
        /// <summary>
        ///接收到包含系统消息SysInfo数据
        /// </summary>
        public event DataInteriorSysInfoEventWHandler DataInteriorSysInfoEvent;

        /// <summary>
        /// 收到 控制端和自助机之间通讯
        /// </summary>
        public event OMC_and_ClientEventHandler OMC_and_ClientEvent ;
        /// <summary>
        /// 收到 自助机管理端调用SLB 500-599
        /// </summary>
        public event ReciveManageRequestSLBInfoEventHandler ReciveManageRequestSLBInfoEvent;

        
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string UDID = "";
        /// <summary>
        /// 客户端医院ID，只有ClientSLB 连接时才有
        /// </summary>
        public string OrgID = "";
        
         WebSocketSession _socketSession = null;

       public  SystemAndProcessInfo SystemAnd_ProcessInfo;
        public WebSocketSessionSLB(WebSocketSession websocketSession)
        {
            _socketSession = websocketSession;
        }



        public string SessionID
        {
            get
            {
                if (_socketSession != null)
                    return _socketSession.SessionID;
                else
                    return "";
            }
        }

        public bool Connected
        {
            get
            {
                if (_socketSession != null)
                    return _socketSession.State == SessionState.Connected;
                return false;
            }
        }

        public WebSocketSession SocketSession { get => _socketSession; set => _socketSession = value; }

        public void Close()
        {
            if (_socketSession != null)
                _socketSession.CloseAsync(SuperSocket.Channel.CloseReason.LocalClosing);
        }
        public void Close(CloseReason closeReason)
        {
            if (_socketSession != null)
                _socketSession.CloseAsync(closeReason);
        }

        public bool TrySendHeartbeat()
        {
            byte[] data = GetTestCennect();
            SendData(data);
            //因为是异步 此处需要处理
            return true;
        }
        /// <summary>
        /// 接收到统消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveDatasSysInfo(Int32 CCN, string Head, byte[] bInfo)
        {
            DatasSysInfoEvent?.Invoke(this,CCN, Head, bInfo);
        }
        /// <summary>
        /// 接收到内部系统控制消息
        /// </summary>
        /// <param name="sSysInfo"></param>
        protected override void ReciveDataInteriorSysInf(string  head,SSysInfo sSysInfo)
        {
            OMCandZZJInfoHead sLBInfoHead = new OMCandZZJInfoHead(head);
            if (sSysInfo.InfoType == SysInfoType.Register)//注册例子
            {
                SSysRegisterInfo sSysRegisterInfo = sSysInfo.GetInfo<SSysRegisterInfo>();
                RegisterEvent?.Invoke(sSysRegisterInfo, this);
            }
            else if (sSysInfo.InfoType == SysInfoType.SystemAndProcessInfo || sSysInfo.InfoType == SysInfoType.SystemAndProcessDynamicInfo)
            {
                if (sSysInfo.InfoType == SysInfoType.SystemAndProcessInfo)
                {
                    SystemAnd_ProcessInfo = sSysInfo.GetInfo<SystemAndProcessInfo>();
                }
                else
                {
                    SystemAndProcessDynamicInfo DynamicInfo = sSysInfo.GetInfo<SystemAndProcessDynamicInfo>();
                    if (SystemAnd_ProcessInfo != null)
                    {
                        SystemAnd_ProcessInfo.Update(DynamicInfo);
                    }
                }
               
            }
            else
            {
                DataInteriorSysInfoEvent?.Invoke(this,   sLBInfoHead, sSysInfo);
            }
        }
        /// <summary>
        ///  接收到客户端主动发给服务端消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveDataClientCallServerInfo(Int32 CCN, string Head, byte[] bInfo)
        {
            DataClientCallServerHeadEvent?.Invoke(this, CCN, new OMCandZZJInfoHead(Head), bInfo);
           
        }
       
        /// <summary>
        /// 接收到服务端主动发给客户端消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveDataServerCallClientInfo(Int32 CCN, string Head, byte[] bInfo)
        {
            RetWebSocketForCall(new OMCandZZJInfoHead(Head), bInfo);
        }


        /// <summary>
        /// 收到二进制文件及信息
        /// </summary>
        /// <param name="sRecFileInfo"></param>
        protected override void ReciveFileInfo(SRecFileInfo sRecFileInfo)
        {
            throw new NotImplementedException();
        }

        protected override void SendData(byte[] datagram)
        {
            SuperSocket.WebSocket.WebSocketPackage webSocketPackage = new SuperSocket.WebSocket.WebSocketPackage();
            webSocketPackage.Data = new ReadOnlySequence<byte>(datagram);
            webSocketPackage.OpCode = SuperSocket.WebSocket.OpCode.Binary;
            _socketSession.SendAsync(webSocketPackage);
            return;
        }
        /// <summary>
        ///  收到 控制端和自助机之间通讯
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"> </param>
        /// <param name="bInfo"></param>
        protected override void OMC_and_Client(int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            OMC_and_ClientEvent?.Invoke(this,CCN, Head, bInfo);
        }
        /// <summary>
        /// 是SLB调用自助机管理端400-499
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveSLBRequestManageInfo(int CCN, string Head, byte[] bInfo)
        {
            RetWebSocketForCall(new OMCandZZJInfoHead(Head), bInfo);
        }
        /// <summary>
        /// 是自助机管理端调用SLB 500-599
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveManageRequestSLBInfo(int CCN, string Head, byte[] bInfo)
        {
            ReciveManageRequestSLBInfoEvent?.Invoke(this, CCN, new OMCandZZJInfoHead(Head), bInfo);
        }
    }
}
