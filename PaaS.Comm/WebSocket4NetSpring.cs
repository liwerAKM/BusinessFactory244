using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;
using PasS.Base.Lib;


namespace PaaS.Comm
{
    /// <summary>
    /// 客户端接收到服务端调用
    /// </summary>
    /// <param name="assignServer"></param>
    /// <param name="sLBInfoHeadBusS"></param>
    /// <param name="bInfo"></param>

    public delegate void DataServerSendClientHeadEventHandler(WebSocket4NetSpring webSocket4NetClent, Int32  CCN, OMCandZZJInfoHead sLBInfoHead, byte[] bInfo);

    /// <summary>
    /// 接收到包含系统消息
    /// </summary>
    /// <param name="webSocket4NetClent"></param>
    /// <param name="CCN"></param>
    public delegate void DataSysInfoEventHandler(WebSocket4NetSpring webSocket4NetClent, Int32 CCN, string  Head, byte[] bInfo);

    /// <summary>
    /// 接收到包含系统消息<see cref="SSysInfo"/>数据
    /// </summary>
    /// <param name="webSocket4NetClent"></param>
    /// <param name="sSysInfo"></param>
    public delegate void DataInteriorSysInfoEventHandler(WebSocket4NetSpring webSocket4NetClent, OMCandZZJInfoHead head, SSysInfo sSysInfo);

    /// <summary>
    /// 接收到文件信息
    /// </summary>
    /// <param name="webSocket4NetClent"></param>
    /// <param name="sSysInfo"></param>
    public delegate void ReciveFileInfoEventHandler(WebSocket4NetSpring webSocket4NetClent, SRecFileInfo sRecFileInfo);



    /// <summary>
    /// 注册返回数据
    /// </summary>
    /// <param name="webSocket4NetClent"></param>
    /// <param name="sSysRegisterInfo"></param>
    public delegate void DataRegisterRetEventHandler(WebSocket4NetSpring webSocket4NetClent, SSysRegisterRetInfo sSysRegisterInfo);

    /// <summary>
    ///  收到 控制端和自助机之间通讯
    /// </summary>
    /// <param name="CCN"></param>
    /// <param name="Head">Head 中 BusServerid为自助机id</param>
    /// <param name="bInfo"></param>
    public delegate void ReciveOMC_and_ClientEventHandler(WebSocket4NetSpring webSocket4NetClent,int CCN, OMCandZZJInfoHead Head, byte[] bInfo);


    /// <summary>
    ///  收到 SLB调用自助机管理端400-499
    /// </summary>
    /// <param name="CCN"></param>
    /// <param name="Head"> </param>
    /// <param name="bInfo"></param>
    public delegate void ReciveSLBRequestManageInfoEventHandler(WebSocket4NetSpring webSocket4NetClent, int CCN, OMCandZZJInfoHead Head, byte[] bInfo);


    

    /// <summary>
    /// 与Server交互的客户端
    /// </summary>
    public class WebSocket4NetSpring : WebSocketSessionClientBase
    {
        SystemInfo systemInfo = null;
        public WebSocket4NetSpring()
        {
            ConnectType = 1;
            systemInfo = new SystemInfo();
        }
        public WebSocket4NetSpring(RegisterIdentity register )
        {
            ConnectType = (int)register;
        }
        /// <summary>
        ///接收到包含系统消息SysInfo数据
        /// </summary>
        public event DataInteriorSysInfoEventHandler DataInteriorSysInfoEvent;
        /// <summary>
        /// 其他系统消息
        /// </summary>

        public event DataSysInfoEventHandler DatasSysInfoEvent;


        /// <summary>
        /// 客户端接收到包含Head数据
        /// </summary>
        public event DataServerSendClientHeadEventHandler DataServerSendClientHeadEvent;

        /// <summary>
        ///注册返回数据
        /// </summary>
        public event DataRegisterRetEventHandler DataRegisterRetEvent;
        /// <summary>
        /// 接收到文件信息
        /// </summary>
        public event ReciveFileInfoEventHandler ReciveFileInfoEvent;


        /// <summary>
        /// 收到 控制端和自助机之间通讯
        /// </summary>
        public event ReciveOMC_and_ClientEventHandler ReciveOMC_and_ClientEvent;
        /// <summary>
        /// 收到 SLB调用自助机管理端400-499
        /// </summary>
        public event ReciveSLBRequestManageInfoEventHandler ReciveSLBRequestManageInfoEvent;

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 在服务端的IP
        /// </summary>
        public string OnServerIP { get; set; }
        /// <summary>
        /// 在服务端的端口
        /// </summary>
        public int OnServerPort { get; set; }
        /// <summary>
        ///WebSocketClient在服务端的SessionID
        /// </summary>
        public string OnServerSessid { get; set; }

        /// <summary>
        ///连接的 SLBID
        /// </summary>
        public string SLBID { get; set; }

        public SLBInfo slbinfo { get; set; }

         

        /// <summary>
        /// WebSocketServer地址
        /// </summary>
        string WebSocketUrl = "";
 

        WebSocket websocketT;

        SSysRegisterInfo sSysRegisterInfoT = null;
        //定义Timer类
        System.Threading.Timer threadTimerHeartbeat;

        public WebSocket4NetSpring(string WebSocketUrl, string SLB_ID)
        {
            this.WebSocketUrl = WebSocketUrl;
            this.SLBID = SLB_ID;
            // websocketT = new WebSocket(WebSocketUrl);

        }
        public bool ConnecSLBBusServer()
        {

            websocketT = new WebSocket(WebSocketUrl);
            websocketT.Opened += websocket_Opened;
            websocketT.Closed += websocket_Closed;
            websocketT.MessageReceived += websocket_MessageReceived;
            websocketT.DataReceived += WebsocketT_DataReceived;
            websocketT.Open();
            int WaitCount = 0;
            while (websocketT.State == WebSocketState.Connecting && WaitCount < 10)
            {
                WaitCount++;
                Thread.Sleep(500);
            }
            if (websocketT.State != WebSocketState.Open)
            {
                return false;
            }
            return true;
        }
        public void ConnectandLogin(SSysRegisterInfo sSysRegisterInfo)
        {
            ConnectType = (int)sSysRegisterInfo.Register_Identity ;
            sSysRegisterInfoT = sSysRegisterInfo;
            ConnectandLogin();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public bool Login(SSysRegisterInfo sSysRegisterInfo)
        {
            if (Connected)
            {
                SysRegister(sSysRegisterInfo);
                return true;
            }
            return false;

        }
        /// <summary>
        /// 重新连接并登录
        /// </summary>
        /// <returns></returns>
        bool ConnectandLogin()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} ConnectandLogin");
            bool Connecsucess = false;
            while (!Connecsucess)
            {
                Connecsucess = ConnecSLBBusServer();
            }
            SysRegister(sSysRegisterInfoT);
            return true;

        }
        private void websocket_Closed(object sender, EventArgs e)
        {
            //客户端 下线
        }

        void websocket_Opened(object sender, EventArgs e)
        {
            //客户端 上线
        }
        /// <summary>
        /// 接收服务端发来的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            MessageReceivedEventArgs responseMsg = (MessageReceivedEventArgs)e; //接收服务端发来的消息
            string value = responseMsg.Message;
            //MessageReceivedEvent?.Invoke(responseMsg.Message);
            // MessageReceived(value);
        }
        private void WebsocketT_DataReceived(object sender, DataReceivedEventArgs e)
        {//接收服务端发来的二进制消息
            byte[] receivedBytes = GetNewDataReceived(e.Data);
            if (receivedBytes != null)
                RaiseReceived(receivedBytes);
        }

        public bool Connected
        {
            get
            {
                if (websocketT != null)
                    return websocketT.State == WebSocketState.Open;
                return false;
            }
        }
        public EndPoint EndPoint
        {
            get
            {
                if (websocketT != null)
                    return websocketT.LocalEndPoint; 
                return null ;
            }
        }

        public void Close()
        {
            if (websocketT != null)
                try
                {
                     
                    websocketT.Close();
                }
                catch (Exception ex)
                {
                }
        }
        
        protected override void SendData(byte[] datagram)
        {
            websocketT.Send(datagram, 0, datagram.Length);
        }
        #region Send 发送登录 心跳等
      
        public void SysRegister(SSysRegisterInfo sSysRegisterInfo)
        {
            
            while (!Connected)
            {
                Thread.Sleep(10);
            }

            SSysInfo sSysInfo = new SSysInfo();
            sSysInfo.InfoType = SysInfoType.Register;
            sSysInfo.SetInfoEncrypt(sSysRegisterInfo);
            SendSysInfo(sSysInfo);
            Thread.Sleep(100);
        }
        /// <summary>
        /// 心跳检查
        /// </summary>
        /// <param name="value"></param>
        private void TimerHeartbeat(object value)
        {
            if (websocketT != null && websocketT.State == WebSocketState.Open)
            {
                try
                {
                    TestCennect();
                    SendSystemAndProcessDynamicInfo();
                    return;
                }
                catch (Exception ex)
                {
                }
            }
            threadTimerHeartbeat.Dispose();
            ConnectandLogin();
        }

        private void SendSystemAndProcessInfo()
        {
            try
            {
                if (systemInfo == null)
                    systemInfo = new SystemInfo();
                SSysInfo sSysInfo = new SSysInfo(SysInfoType.SystemAndProcessInfo);
                sSysInfo.SetInfoEncrypt(systemInfo.GetCurrentProcess());
                
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(CCNSysValye.SysInfo, new OMCandZZJInfoHead(), sysinfo);

            }
            catch (Exception ex)
            {
            }
        }

        private void SendSystemAndProcessDynamicInfo()
        {
            try
            {
                if (systemInfo == null)
                    systemInfo = new SystemInfo();
                SSysInfo sSysInfo = new SSysInfo(SysInfoType.SystemAndProcessDynamicInfo);
                sSysInfo.ET = 0;
               // sSysInfo.SetInfo( systemInfo.GetCurrentProcessDynamic());
               
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(CCNSysValye.SysInfo, new OMCandZZJInfoHead(), sysinfo);
                
            }
            catch (Exception ex)
            {
            }
        }
        #endregion



        /// <summary>
        /// 接收到统消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveDatasSysInfo(Int32 CCN, string Head, byte[] bInfo)
        {
            DatasSysInfoEvent?.Invoke(this, CCN, Head, bInfo);
        }
        /// <summary>
        /// 接收到内部系统控制消息
        /// </summary>
        /// <param name="sSysInfo"></param>
        protected override void ReciveDataInteriorSysInf(string strHeadX, SSysInfo sSysInfo)
        {
            OMCandZZJInfoHead head = new OMCandZZJInfoHead(strHeadX);
            if (sSysInfo.InfoType == SysInfoType.RegisterRet)//注册返回
            {
                SSysRegisterRetInfo sSysRegisterInfo = sSysInfo.GetInfo<SSysRegisterRetInfo>();
                if (sSysRegisterInfo.ReslutCode == 1)//注册成功
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    SLBID = sSysRegisterInfo.ServerID;
                    string showmessage = string.Format("注册服务器成功! {0}  {1}   :{2} ", sSysRegisterInfo.ServerID, sSysRegisterInfo.SessionID, sSysRegisterInfo.ClientIP);
                    Console.WriteLine(showmessage);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (sSysRegisterInfo.Register_Identity != RegisterIdentity.OperationMaintenance)//OperationMaintenance自带心跳等操作
                        threadTimerHeartbeat = new System.Threading.Timer(new TimerCallback(TimerHeartbeat), null, 0, 5000);
                    SendSystemAndProcessInfo();
                }
                else//注册失败
                {
                }
                DataRegisterRetEvent?.Invoke(this, sSysRegisterInfo);
            }
            else
            {
                DataInteriorSysInfoEvent?.Invoke(this,   head,sSysInfo);
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
            RetWebSocketForCall(new OMCandZZJInfoHead(Head), bInfo);
        }

        /// <summary>
        /// 接收到服务端主动发给客户端消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveDataServerCallClientInfo(Int32 CCN, string Head, byte[] bInfo)
        {
            DataServerSendClientHeadEvent?.Invoke(this, CCN,new OMCandZZJInfoHead(Head), bInfo);
        }


        /// <summary>
        /// 收到二进制文件及信息
        /// </summary>
        /// <param name="sRecFileInfo"></param>
        protected override void ReciveFileInfo(SRecFileInfo sRecFileInfo)
        {
            ReciveFileInfoEvent?.Invoke(this, sRecFileInfo);
        }
        /// <summary>
        ///  收到 控制端和自助机之间通讯
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head">Head 中 BusServerid为自助机id</param>
        /// <param name="bInfo"></param>
        protected override void OMC_and_Client(int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            if (ConnectType == 1 || ConnectType == 8 && string.IsNullOrEmpty(Head.ZZJID))//自助机或者 查看医院端SLB时
            {

                int HccN = CCN / 10000;//高位CCN 
                int LccN = CCN % 10000;//低位CCN
                if (HccN == HCCNValye.OMC_SLB_Client && Head.BusID == OMCandZZJInfoHeadBusID.File)//客户端收到文件
                {
                    SOMCandClientFileInfo sOMCandClient = Head.GetExtend<SOMCandClientFileInfo>();
                    SRecFileInfo sRecFile = new SRecFileInfo(sOMCandClient);
                    sRecFile.images = bInfo;
                    sRecFile.SaveFile(System.AppDomain.CurrentDomain.BaseDirectory);
                    Console.WriteLine("reciveFile：  " + sRecFile.FileName);
                    return;
                }
                else if (HccN == HCCNValye.OMC_SLB_Client && LccN == OMCandZZJInfoHeadBusID.LCCNExplorer)
                {
                    DealExplorer(HccN, LccN, Head, bInfo);
                    return;
                }
                else if (HccN == HCCNValye.OMC_SLB_Client && Head.BusID.StartsWith(OMCandZZJInfoHeadBusID.Screen))
                {
                //    DealScreen(CCN, Head, bInfo);
                    return;
                }
            }
            ReciveOMC_and_ClientEvent?.Invoke(this, CCN, Head, bInfo);
        }
        /*
        private void DealScreen(int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            if (Head.BusID == OMCandZZJInfoHeadBusID.Screen)
            {
                if (Head.Extend == OMCandZZJInfoHeadBusID.ScreenBegin)
                {
                    string json = Encoding.GetString(bInfo);
                    SScreenBegin sScreenBegin = JsonConvert.DeserializeObject<SScreenBegin>(json);
                   
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ":DealScreen:ScreenBegin"  );
                    string sysinfo = JsonConvert.SerializeObject(ExplorerHelper.GetDriveInfo());
                    SendTaskScreen = true;
                    STaskProcScreen sTaskProcScreen = new STaskProcScreen();
                    sTaskProcScreen.sScreenBegin = sScreenBegin;
                    sTaskProcScreen.Head = Head;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProcScreen), sTaskProcScreen);
                }
               else if (Head.Extend == OMCandZZJInfoHeadBusID.ScreenEnd)
                {
                    SendTaskScreen = false;
                  
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ":DealScreen:ScreenEnd" );
                }
                else if (Head.Extend == OMCandZZJInfoHeadBusID.ScreenMOUSE)
                {
                    MouseEvent mouseEvent = new MouseEvent(bInfo);
                    ScreenCapture.DoMouseEvent(mouseEvent);

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ":DealScreen:MOUSE");
                }
                else if (Head.Extend == OMCandZZJInfoHeadBusID.ScreenKEYBOARD)
                {
                    string json = Encoding.GetString(bInfo);
                    KeyBoardEvent keyBoard = JsonConvert.DeserializeObject<KeyBoardEvent>(json);
                    ScreenCapture.doKeyBoardEvent(keyBoard);

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ":DealScreen:KEYBOARD");
                }

            }

        }
        class STaskProcScreen
        {
            public SScreenBegin sScreenBegin;
            public OMCandZZJInfoHead Head;
        }
        bool SendTaskScreen = true;
        void TaskProcScreen(object o)
        {
            STaskProcScreen  sTaskProc= (STaskProcScreen)o;
            OMCandZZJInfoHead Head = sTaskProc.Head;
            Head.BusID = OMCandZZJInfoHeadBusID.ScreenImage;
            Bitmap myImageLast = null;
            int Count = 0;
             while (SendTaskScreen)
            {
               
                Head.Extend = "Full";
                byte[] bytes = ScreenCapture.GetCapture2(sTaskProc.sScreenBegin.percent, sTaskProc.sScreenBegin.level);
                Client_SLBR_OMC(Head, bytes);
                Thread.Sleep(sTaskProc.sScreenBegin.Ttimeinterval);
             }
           
        }
        */
        private void DealExplorer(int HCCN,int LCCN  ,OMCandZZJInfoHead Head, byte[] bInfo)
        {
            if (Head.BusID == OMCandZZJInfoHeadBusID.Explorer)
            {
                if (Head.Extend == "GetDriveInfo")
                {
                    string sysinfo = JsonConvert.SerializeObject(ExplorerHelper.GetDriveInfo());
                    Client_SLBR_OMC(Head, this.Encoding.GetBytes(sysinfo), LCCN);
                }
                else if (Head.Extend == "GetDirectoryFileInfo")
                {

                    string CurrentPath = this.Encoding.GetString(bInfo);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(DateTime.Now.ToLongTimeString() + ":ExplorerGetDirectoryFileInfo:" + CurrentPath);
                    MyDirectoryFileInfoS mds = ExplorerHelper.GetDirectoryFileInfo(CurrentPath);
                    string sysinfo = JsonConvert.SerializeObject(mds);
                    Client_SLBR_OMC(Head, this.Encoding.GetBytes(sysinfo),LCCN);
                }

            }
            else if (Head.BusID == OMCandZZJInfoHeadBusID.ExplorerSendFile)
            {
                MyDirectoryFileInfo dragFileInfo = Head.GetExtend<MyDirectoryFileInfo>();
                ExplorerHelper.SaveFileInfo(dragFileInfo, bInfo);
            }
            else if (Head.BusID == OMCandZZJInfoHeadBusID.ExplorerGetFile)
            {
                MyDirectoryFileInfo fileInfo = Head.GetExtend<MyDirectoryFileInfo>();
               string  TargetPath= fileInfo.TP;
                string FullName = Path.Combine(TargetPath, fileInfo.Name);
                string sorcepath = fileInfo.FullName;
                if (fileInfo.isF == 1)
                { 
                    fileInfo.FullName = FullName;
                    Head.SetExtend(fileInfo);
                    Client_SLBR_OMC(Head, this.Encoding.GetBytes(OMCandZZJInfoHeadBusID.ExplorerGetFile), LCCN);
                    MyDirectoryFileInfoS fileInfoS = ExplorerHelper.GetDirectoryFileInfo(sorcepath);
                    if (fileInfoS.listMyDirFileInfo.Count > 0)
                    {
                        System.Threading.Thread.Sleep(100);
                        CopysyncDragFileToRemote(FullName, fileInfoS.listMyDirFileInfo,   Head, LCCN); 
                    }
                }
                else
                {
                    if (File.Exists(sorcepath))
                    {
                        fileInfo.FullName = FullName;
                        Head.SetExtend(fileInfo);
                        byte[] file = FileHelper.ReadFile(sorcepath);
                        Client_SLBR_OMC(Head, file, LCCN);
                    }
                }
            }
            else if (Head.BusID == OMCandZZJInfoHeadBusID.ExplorerCopyF|| Head.BusID == OMCandZZJInfoHeadBusID.ExplorerDelF|| Head.BusID == OMCandZZJInfoHeadBusID.ExplorerMoveF)
            {
                DragFileInfoS dragFileInfo = Head.GetExtend<DragFileInfoS>();
                ExplorerHelper.CopyMoveDelFileinfo(dragFileInfo, Head.BusID == OMCandZZJInfoHeadBusID.ExplorerCopyF?1: (Head.BusID == OMCandZZJInfoHeadBusID.ExplorerMoveF?2:3));
            }
            else if (Head.BusID == OMCandZZJInfoHeadBusID.ExplorerCreateF || Head.BusID == OMCandZZJInfoHeadBusID.ExplorerReNameF  )
            {
                CreateRenameFileInfo dragFileInfo = Head.GetExtend<CreateRenameFileInfo>();
                ExplorerHelper.CreateReName(dragFileInfo, Head.BusID == OMCandZZJInfoHeadBusID.ExplorerCreateF ? 1 :2);
            }
            else if (Head.BusID == OMCandZZJInfoHeadBusID.ExplorerOpenRunF  )
            {
                string Fullname = Head.Extend;
                ExplorerHelper.OpenRunF(Fullname);
            }

        }
        void CopysyncDragFileToRemote(string TargetPath, List<MyDirectoryFileInfo> dragFileInfos, OMCandZZJInfoHead Head,int  LCCN) 
        {
            foreach (MyDirectoryFileInfo fileInfo in dragFileInfos)
            {
                string FullName = Path.Combine(TargetPath, fileInfo.Name);

                if (fileInfo.isF == 1)
                {
                    string sorcepath = fileInfo.FullName;
                    fileInfo.FullName = FullName;
                    Head.SetExtend(fileInfo);
                    Client_SLBR_OMC(Head, this.Encoding.GetBytes(OMCandZZJInfoHeadBusID.ExplorerGetFile), LCCN);

                    MyDirectoryFileInfoS fileInfoS = ExplorerHelper.GetDirectoryFileInfo(sorcepath);
                    if (fileInfoS.listMyDirFileInfo.Count > 0)
                    {
                        System.Threading.Thread.Sleep(100);
                        CopysyncDragFileToRemote(FullName, fileInfoS.listMyDirFileInfo,Head, LCCN);
                    }
                }
                else
                {
                    string sorcepath = fileInfo.FullName;
                    if (File.Exists(sorcepath))
                    {
                        fileInfo.FullName = FullName;
                        Head.SetExtend(fileInfo);
                        byte[] file = FileHelper.ReadFile(sorcepath);
                        Client_SLBR_OMC(Head, file, LCCN);
                    }
                }
            }
        }

        /// <summary>
        ///  OMC 主动发给自助机
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool OMC_SLB_Client(OMCandZZJInfoHead oMCandZZJInfo,SSysInfo sSysInfo  )
        {
            try
            {
                oMCandZZJInfo.BusID = "sSysInfo";
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(HCCNValye.OMC_SLB_Client * 10000, oMCandZZJInfo, this.Encoding.GetBytes(sysinfo));
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.OMC_SLB_Client", ex.Message + sSysInfo.Get_Info());
                return false;
            }
        }
        /// <summary>
        ///  OMC 返回给自助机 
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool OMC_SLBR_Client(OMCandZZJInfoHead oMCandZZJInfo, SSysInfo sSysInfo)
        {
            try
            {
                oMCandZZJInfo.BusID = "sSysInfo";
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(HCCNValye.OMC_SLBR_Client*10000, oMCandZZJInfo, this.Encoding.GetBytes(sysinfo));
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.OMC_SLBR_Client", ex.Message + sSysInfo.Get_Info());
                return false;
            }
        }
        /// <summary>
        /// 自助机 发给  OMC 
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool Client_SLB_OMC(OMCandZZJInfoHead oMCandZZJInfo, SSysInfo sSysInfo)
        {
            try
            {
                oMCandZZJInfo.BusID = "sSysInfo";
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(HCCNValye.Client_SLB_OMC * 10000, oMCandZZJInfo, this.Encoding.GetBytes(sysinfo));
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.Client_SLB_OMC", ex.Message + sSysInfo.Get_Info());
                return false;
            }
        }
     
        /// <summary>
        /// 自助机 返回给  OMC 
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool Client_SLBR_OMC(OMCandZZJInfoHead oMCandZZJInfo, SSysInfo sSysInfo)
        {
            try
            {
                oMCandZZJInfo.BusID = "sSysInfo";
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(HCCNValye.Client_SLBR_OMC * 10000, oMCandZZJInfo, this.Encoding.GetBytes(sysinfo));
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.Client_SLBR_OMC", ex.Message + sSysInfo.Get_Info());
                return false;
            }
        }

        /// <summary>
        ///  OMC 主动发给自助机
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool OMC_SLB_Client(OMCandZZJInfoHead oMCandZZJInfo, byte[] bInfo,int LCCN=0)
        {
            try
            {
                Send(HCCNValye.OMC_SLB_Client*10000 + LCCN, oMCandZZJInfo, bInfo);
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.OMC_SLB_Client", ex.Message);
                return false;
            }
        }
        /// <summary>
        ///  OMC 返回给自助机 
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool OMC_SLBR_Client(OMCandZZJInfoHead oMCandZZJInfo, byte [] bInfo, int LCCN = 0)
        {
            try
            {
              
                Send(HCCNValye.OMC_SLBR_Client * 10000 + LCCN, oMCandZZJInfo, bInfo);
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.OMC_SLBR_Client", ex.Message  );
                return false;
            }
        }

       
        /// <summary>
        /// 自助机 发给  OMC 
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool Client_SLB_OMC(OMCandZZJInfoHead oMCandZZJInfo, byte[] bInfo, int LCCN = 0)
        {
            try
            {
               
                Send(HCCNValye.Client_SLB_OMC * 10000 + LCCN, oMCandZZJInfo , bInfo);
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.Client_SLB_OMC", ex.Message  );
                return false;
            }
        }
        /// <summary>
        /// 自助机 返回给  OMC 
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool Client_SLBR_OMC(OMCandZZJInfoHead oMCandZZJInfo, byte[] bInfo, int LCCN = 0)
        {
            try
            {
                
                Send(HCCNValye.Client_SLBR_OMC * 10000 + LCCN, oMCandZZJInfo,  bInfo);
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.Client_SLBR_OMC", ex.Message  );
                return false;
            }
        }

        /// <summary>
        /// 是SLB调用自助机管理端400-499
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveSLBRequestManageInfo(int CCN, string Head, byte[] bInfo)
        {
            ReciveSLBRequestManageInfoEvent?.Invoke(this, CCN, new OMCandZZJInfoHead(Head), bInfo);
        }
        /// <summary>
        /// 是自助机管理端调用SLB 500-599
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected override void ReciveManageRequestSLBInfo(int CCN, string Head, byte[] bInfo)
        {
            RetWebSocketForCall(new OMCandZZJInfoHead(Head), bInfo);
        }
    }
}
