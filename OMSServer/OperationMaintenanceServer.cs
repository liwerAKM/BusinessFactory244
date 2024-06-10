
using Newtonsoft.Json;
using PaaS.Comm;
using PasS.Base.Lib;
using PasS.Base.Lib.DAL;
using PasS.Base.Lib.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace OMSServer
{ 
    //////////////////////////////////////////////////////////
    /*
     OperationMaintenanceServer.cs
     Function：管理及监控的服务端。核心为perationMaintenanceConnect  ，并记录SLB、 BusServer以及BusInfo的当前状况 
     Created on:  2018-07-18     
     Original author: 王磊
    */
    ////////////////////////////////////////////////////////
    /// <summary>
    /// 钉钉消息提示
    /// </summary>
    /// <param name="MessageJson"></param>
    public delegate void DingMessageEventHandler(string MessageJson);
    /// <summary>
    /// 集成各系统SLB、 BusServer以及BusInfo的当前状况及<see cref="OperationMaintenanceServer"/>的类
    /// 主要作为管理及监控的服务端
    /// 核心为<see cref="OperationMaintenanceServer"/> ，并记录SLB、 BusServer以及BusInfo的当前状况 
    /// </summary>
    public class OperationMaintenanceServer
    {
        /// <summary>
        /// 是否是监控服务，对于监控服务,系统会对<see cref="AccessServer"/>、<see cref="ServerLoadBalancing"/>等采取重连策略,以最大可能保证能连接到各系统
        /// </summary>
        bool isMonitorService = false;

        OperationMaintenanceConnect OMConnect = null;

        string AccessServerStatus = "未知";
        /// <summary>
        ///SLBInfo的信息
        /// key:SLBID
        /// </summary>
        Dictionary<string, SLBInfo> dicSLBInfo = new Dictionary<string, SLBInfo>();

        /// <summary>
        /// BusServer的信息
        /// key:BusServerID
        /// </summary>
        Dictionary<string, BusServerInfo> dicBusServerInfo = new Dictionary<string, BusServerInfo>();

        /// <summary>
        ///数据库BusinessInfo信息
        /// key:BusID
        /// </summary>
        Dictionary<string, BusinessInfo> dicBusinessInfo = new Dictionary<string, BusinessInfo>();

        /// <summary>
        /// BusInfo 以及对应状态
        /// </summary>
        DataTable dtBusInfoStatusAndRate = null;

        /// <summary>
        /// 在线SLBRates
        /// Key SLBID
        /// </summary>
        ConcurrentDictionary<string, SDataSLBRates> cdSDataSLBRates = new ConcurrentDictionary<string, SDataSLBRates>();

        /// <summary>
        /// 在线BusServerRates
        /// Key BusServerID
        /// </summary>
        ConcurrentDictionary<string, SBusServerRates> cdSBusServerRates = new ConcurrentDictionary<string, SBusServerRates>();

        /// <summary>
        /// 系统基本配置数量
        /// </summary>
        SSysBaseCount BaseCount = new SSysBaseCount();

        const int QCharData_MaxNum = 1800;
        /// <summary>
        /// 图标数据队列
        /// </summary>
        private Queue<SCharData> QCharData = new Queue<SCharData>(QCharData_MaxNum);
        /// <summary>
        /// 图表数据当前接收值
        /// </summary>
        SCharData sCharData = new SCharData();
        /// <summary>
        ///  图表数据当前接收值 ,当某一值在<see cref="sCharData"/>中存才是就存在此
        /// </summary>
        SCharData sCharData2 = new SCharData();

        const int SysInfo_Maxnum = 100;
        /// <summary>
        /// 系统消息队列
        /// </summary>
        private Queue<SSysinfoLog> QSysInfo = new Queue<SSysinfoLog>(SysInfo_Maxnum);

        //定义Timer类
        System.Threading.Timer timerBaseInfo;

        //定义Timer类
        System.Threading.Timer timerSpringInfo;

        /// <summary>
        /// 跟踪结果
        /// Key TCID
        /// </summary>
        ConcurrentDictionary<string, ConcurrentQueue<STrackResult>> cdSTrackResult = new ConcurrentDictionary<string, ConcurrentQueue<STrackResult>>();
        /// <summary>
        /// 优化SLB上线下机制 用于发送SLB上线事件消息
        /// Key: SLB_Status
        /// </summary>
        ConcurrentDictionary<string, SLBUpDownOptimize> cdSLBUp = new ConcurrentDictionary<string, SLBUpDownOptimize>();
        /// <summary>
        /// 优化BusServer上线下线机制 用于发送BusServer上下事件消息
        /// Key: BusServer_Status
        /// </summary>
        ConcurrentDictionary<string, BusServerUpDownOptimize> cdBusServerUpDown = new ConcurrentDictionary<string, BusServerUpDownOptimize>();
        /// <summary>
        /// 业务执行异常记录基点
        /// </summary>
        Dictionary<EHandingBusinessResult, List<BusAbnormalPoint>> dicRecordPoint = new Dictionary<EHandingBusinessResult, List<BusAbnormalPoint>>();

        /// <summary>
        /// 业务执行异常统计及通知
        ///  Key: <BusServer_ID, Dictionary<BusID, Dictionary<EHandingBusinessResult,
        /// </summary>
        ConcurrentDictionary<string, Dictionary<string, Dictionary<EHandingBusinessResult, BusAbnormal>>> cdBusAbnormal
            = new ConcurrentDictionary<string, Dictionary<string, Dictionary<EHandingBusinessResult, BusAbnormal>>>();
        /// <summary>
        /// 记录基点
        /// </summary>
        public class BusAbnormalPoint
        {
            public BusAbnormalPoint()
            {
            }
            public BusAbnormalPoint(int Count, decimal Percent)
            {
                this.Count = Count;
                this.Percent = Percent;
            }
            /// <summary>
            ///异常数量
            /// </summary>
            public int Count;
            /// <summary>
            ///异常比例
            /// </summary>
            public decimal Percent;
        }
        public class BusAbnormal
        { /// <summary>
          /// 当前统计范围总数量
          /// </summary>
            public int AllCount;
            /// <summary>
            ///异常数量
            /// </summary>
            public int AbnormalCount;
            /// <summary>
            ///异常比例
            /// </summary>
            public decimal AbnormalPercent;
            /// <summary>
            /// 记录时间
            /// </summary>
            public DateTime RecordTime;

            /// <summary>
            /// 最后异常消息
            /// </summary>
            public string LastMessagge;
            /// <summary>
            /// 下一个记录比较基点的索引，-1时表示当前记录已经达到最大范围，不需要再处理
            /// </summary>
            public int NextReportPoint;
        }

        /// <summary>
        /// 钉钉通知消息
        /// </summary>
        public event DingMessageEventHandler DingMessageEvent;
        public event ShowInfoEventHandler ShowInfoEvent;
        /// <summary>
        /// 收到 控制端和自助机之间通讯
        /// </summary>
        public event ReciveOMC_and_ClientEventHandler ReciveOMC_and_ClientEvent;
        /// <summary>
        /// 其他系统消息
        /// </summary>
        public event OtherSysINofEventHandler OtherSysINofEvent;
        public OperationMaintenanceServer()
        {
            InitializationdicRecordPoint();
            isMonitorService = MyPubConstant.IsMonitorService();
            InitializationBusinessInfoandStatus();
            LoadSLBInfo();
            LoadBusServerInfo();

            OMConnect = new OperationMaintenanceConnect(isMonitorService);
            OMConnect.DataSLBRatesEvent += OMClient_DataSLBRatesEvent;
            OMConnect.DataBusServerRatesEvent += OMClient_DataBusServerRatesEvent;
            OMConnect.DataTrackResultEvent += OMClient_DataTrackResultEvent;
            OMConnect.SLBInfoUpDownEvent += OMClient_SLBInfoUpDownEvent;
            OMConnect.BusServerUpDownEvent += OMClient_BusServerUpDownEvent;
            OMConnect.AccessServerConnctEven += OMConnect_AccessServerConnctEven;
            OMConnect.HeartbeatBusServerInfoEvent += OMConnect_HeartbeatBusServerInfoEvent;
            OMConnect.ShowInfoEvent += OMConnect_ShowInfoEvent;
            OMConnect.ReciveOMC_and_ClientEvent += OMConnect_ReciveOMC_and_ClientEvent;
            OMConnect.OtherSysINofEvent += OMConnect_OtherSysINofEvent;
            OMConnect.ConnectAccessServerTcpClient();

            ConnectSLB();
            InitTimer();
            if (isMonitorService)
            {
                if (!string.IsNullOrEmpty(MyPubConstant.HttpLocalIP))
                    HttpIPAdd(MyPubConstant.HttpLocalIP);
                HttpStart();
            }
        }

        private void OMConnect_ReciveOMC_and_ClientEvent(WebSocket4NetSpring webSocket4NetClent, int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            ReciveOMC_and_ClientEvent?.Invoke(webSocket4NetClent, CCN, Head, bInfo);
        }

        private void OMConnect_OtherSysINofEvent(WebSocket4NetSpring webSocket4Net, SSysInfo sysInfo)
        {
            OtherSysINofEvent?.Invoke(webSocket4Net, sysInfo);
        }

        private async Task<bool> ConnectSLB()
        {

            var result = await Task.Run(() =>
            {
                List<SLBInfo> listSLB = DbHelper.SLBInfoListGet("Status =1 and OrgID ='" + MyPubConstant.OMSMainOrgID + "'");
                if (listSLB != null && listSLB.Count > 0)
                {
                    foreach (SLBInfo slbinfo in listSLB)
                    {
                        OMConnect.ConnectSLB(slbinfo);
                    }
                }
                return true;

            });

            return result;
        }

        private void OMConnect_ShowInfoEvent(string Showinfo)
        {
            ShowInfoEvent?.Invoke(Showinfo);
        }


        /// <summary>
        /// 初始化异常业务记录基点
        /// </summary>
        void InitializationdicRecordPoint()
        {
            dicRecordPoint = new Dictionary<EHandingBusinessResult, List<BusAbnormalPoint>>();
            List<BusAbnormalPoint> listSysE = new List<BusAbnormalPoint>();
            listSysE.Add(new BusAbnormalPoint(1, 0m));
            listSysE.Add(new BusAbnormalPoint(2, 0.2m));
            listSysE.Add(new BusAbnormalPoint(5, 0.5m));
            listSysE.Add(new BusAbnormalPoint(10, 1m));
            dicRecordPoint.Add(EHandingBusinessResult.SysError, listSysE);

            List<BusAbnormalPoint> listBusE = new List<BusAbnormalPoint>();
            listBusE.Add(new BusAbnormalPoint(1, 0.05m));
            listBusE.Add(new BusAbnormalPoint(3, 0.3m));
            listBusE.Add(new BusAbnormalPoint(5, 0.6m));
            listBusE.Add(new BusAbnormalPoint(10, 1m));
            dicRecordPoint.Add(EHandingBusinessResult.BusinessError, listBusE);

            List<BusAbnormalPoint> listBusA = new List<BusAbnormalPoint>();
            listBusA.Add(new BusAbnormalPoint(5, 0.2m));
            listBusA.Add(new BusAbnormalPoint(10, 0.5m));
            listBusA.Add(new BusAbnormalPoint(20, 1m));
            dicRecordPoint.Add(EHandingBusinessResult.BusinessAbnormal, listBusA);
        }


        /// <summary>
        /// 管理跟踪连接,用来发送系统配置管理信息
        /// </summary>
        public OperationMaintenanceConnect GOMConnect
        {
            get
            {
                return OMConnect;
            }
        }


        #region BaseandEvent

        /// <summary>
        /// 初始化Timer类
        /// </summary>
        private void InitTimer()
        {
            timerBaseInfo = new System.Threading.Timer(new TimerCallback(TimerUpBaseInfo), null, 0, 1000);
            if (isMonitorService)
                timerSpringInfo = new System.Threading.Timer(new TimerCallback(TimerSpringInfo), null, 0, 60000);
        }
        /// <summary>
        /// 定期将SLB发送给BusServer, isMonitorService 才执行
        /// </summary>
        /// <param name="value"></param>
        private void TimerSpringInfo(object value)
        {
            SOnLineSLBlistToBusServer sOnLineSLBToBusS = new SOnLineSLBlistToBusServer();
            sOnLineSLBToBusS.SLBList = new List<SLBInfo>();
            foreach (string SLBid in cdSDataSLBRates.Keys)
            {
                sOnLineSLBToBusS.SLBList.Add(cdSDataSLBRates[SLBid].sLBInfo);
            }
            foreach (string BusServerID in cdSBusServerRates.Keys)
            {
                List<string> SLBlist = cdSBusServerRates[BusServerID].SLBList;
                if (SLBlist != null && SLBlist.Count > 0)
                {
                    sOnLineSLBToBusS.BusServerID = BusServerID;
                    OMConnect.SendOnLineSLBlistToBusServer(sOnLineSLBToBusS, SLBlist);
                }
                else
                {
                    sOnLineSLBToBusS.BusServerID = BusServerID;
                    foreach (SLBInfo slbinfo in sOnLineSLBToBusS.SLBList)
                    {
                        List<string> SLBlistt = new List<string>();
                        SLBlistt.Add(slbinfo.SLBID);
                        OMConnect.SendOnLineSLBlistToBusServer(sOnLineSLBToBusS, SLBlistt);
                    }
                }
            }
            InitializationBusinessInfoandStatus();
        }
        private void TimerUpBaseInfo(object value)
        {
            if (dtBusInfoStatusAndRate != null)
            {
                lock (dtBusInfoStatusAndRate)
                {
                    foreach (DataRow drbs in dtBusInfoStatusAndRate.Rows)
                    {
                        SCharBusRatesandStatus sCharBusRate = null;
                        string busID = drbs["BusID"].ToString();
                        foreach (SBusServerRates sbsr in cdSBusServerRates.Values)
                        {
                            if (sbsr.sdBusRates.ContainsKey(busID))
                            {
                                if (sCharBusRate == null)
                                {
                                    sCharBusRate = new SCharBusRatesandStatus();
                                    sCharBusRate.BusID = busID;
                                    sCharBusRate.BusName = drbs["BusName"].ToString();
                                }
                                sCharBusRate.AllCount += sbsr.sdBusRates[busID].AllCount;
                                sCharBusRate.InRates += sbsr.sdBusRates[busID].InRates;
                                sCharBusRate.OutRates += sbsr.sdBusRates[busID].OutRates;
                                sCharBusRate.TheConcurrent += sbsr.sdBusRates[busID].TheConcurrent;
                                sCharBusRate.RunServerCount++;
                            }
                        }
                        if (sCharBusRate != null)
                        {
                            drbs["RunServerCount"] = sCharBusRate.RunServerCount;
                            drbs["InRate"] = sCharBusRate.InRates;
                            sCharData.cdBusiness.TryAdd(busID, sCharBusRate);
                        }
                        else
                        {
                            drbs["RunServerCount"] = 0;
                            drbs["InRate"] = 0;
                        }
                    }
                }
            }

            if (dtBusInfoStatusAndRate != null)
            {
                BaseCount.BusRun = dtBusInfoStatusAndRate.Select("RunServerCount >0").Length;

                BaseCount.NoServerBus = dtBusInfoStatusAndRate.Select("RunServerCount =0 and Status=1").Length;
            }

            int num = 1;
            if (QCharData.Count > QCharData_MaxNum)
            {
                lock (QCharData)
                    //先出列
                    for (int i = 0; i < num; i++)
                    {
                        QCharData.Dequeue();
                    }
            }
            for (int i = 0; i < num; i++)
            {
                lock (QCharData)
                {
                    QCharData.Enqueue(sCharData);
                    sCharData = sCharData2;
                    sCharData2 = new SCharData();
                }
            }
            if (cdSLBUp.Count > 0)
            {
                foreach (SLBUpDownOptimize bso in cdSLBUp.Values)
                {
                    bso.SencondTime++;
                    NeedSendSLBUpDown(bso);
                }
            }

            if (cdBusServerUpDown.Count > 0)
            {
                foreach (BusServerUpDownOptimize bso in cdBusServerUpDown.Values)
                {
                    bso.SencondTime++;
                    NeedSendBusServerUpDown(bso);
                }
            }

        }

        /// <summary>
        ///AccessServer连接事件
        /// </summary>
        /// <param name="Status">0 连接失败； 1连接成功；2 多次连接失败，3 失败后重连成功</param>
        /// <param name="Note"></param>
        private void OMConnect_AccessServerConnctEven(int Status, string Note)
        {
            if (Status == 1 || Status == 3)
            {
                AccessServerStatus = "在线";
                BaseCount.AccessServerOnline = true;
            }
            else
            {
                AccessServerStatus = "离线";
            }
            string info = string.Format("AccessServer:{0}   ", Note);
            SSysinfoLog sSysinfoLog = new SSysinfoLog(info);
            if (QSysInfo.Count > SysInfo_Maxnum)
            {
                QSysInfo.Dequeue();
            }
            QSysInfo.Enqueue(sSysinfoLog);
            int Msgkind = 0;
            if (Status == 2)
            {
                Msgkind = 3;
            }
            else if (Status == 3)
            {
                Msgkind = 1;
            }
            SendSysRunMessage("AccessServer" + AccessServerStatus.Replace("在线", "上线"), Note, Msgkind);
            //Showrich_SysInfo(info);
        }



        int InitializationBusinessInfotag = -1;
        void InitializationBusinessInfoandStatus()
        {
            if (InitializationBusinessInfotag == -1 || InitializationBusinessInfotag > 30)
            {
                InitializationBusinessInfotag = 0;
                dtBusInfoStatusAndRate = DbHelper.BusinessInfoStatus();
                dtBusInfoStatusAndRate.Columns.Add(new DataColumn("InRate", typeof(int)));
                BaseCount.BusCount = dtBusInfoStatusAndRate.Rows.Count;

                List<BusinessInfo> listBusInfo = DbHelper.BusinessInfoListGet(" Status >=0 ");
                if (listBusInfo != null && listBusInfo.Count > 0)
                {
                    lock (dicBusinessInfo)
                    {
                        dicBusinessInfo = new Dictionary<string, BusinessInfo>();
                        foreach (BusinessInfo businfo in listBusInfo)
                        {
                            dicBusinessInfo.Add(businfo.BusID, businfo);
                        }
                    }
                }
            }
            InitializationBusinessInfotag++;
        }

        /// <summary>
        /// 加载 或更新SLB信息
        /// </summary>
        public void LoadSLBInfo()
        {
            List<SLBInfo> listSLB = DbHelper.SLBInfoListGet("Status >=0");
            dicSLBInfo = new Dictionary<string, SLBInfo>();
            foreach (SLBInfo bs in listSLB)
            {
                dicSLBInfo.Add(bs.SLBID, bs);
            }
            BaseCount.SLBCount = dicSLBInfo.Count;
        }

        /// <summary>
        /// 加载 或更新BusServer信息
        /// </summary>
        public void LoadBusServerInfo()
        {
            List<BusServerInfo> list = DbHelper.BusServerListGet("Status>=0");
            dicBusServerInfo = new Dictionary<string, BusServerInfo>();
            foreach (BusServerInfo bs in list)
            {
                dicBusServerInfo.Add(bs.BusServerID, bs);
            }
            BaseCount.BusSCount = dicBusServerInfo.Count;
        }

        /// <summary>
        /// SLB承载情况事件 对<see cref="cdSDataSLBRates"/>、<see cref="sCharData"/>进行更新操作
        /// </summary>
        /// <param name="sLBInfo"></param>
        /// <param name="sLBCurrentStatus"></param>
        /// <param name="LoadFactor">当前总状况值</param>
        /// <param name="Online">是否在线 ，false时为下线</param>
        private void OMClient_DataSLBRatesEvent(SLBInfo sLBInfo, SLBCurrentStatus sLBCurrentStatus, int LoadFactor, bool Online)
        {
            if (!Online)
            {//不在线 移除
                if (cdSDataSLBRates.ContainsKey(sLBInfo.SLBID))
                {
                    SDataSLBRates sDataSLBRates;
                    cdSDataSLBRates.TryRemove(sLBInfo.SLBID, out sDataSLBRates);
                    sDataSLBRates = null;
                }
                BaseCount.SLBRun = cdSDataSLBRates.Count;
                return;
            }
            //更新或新增
            if (cdSDataSLBRates.ContainsKey(sLBInfo.SLBID))
            {
                cdSDataSLBRates[sLBInfo.SLBID].sLBInfo = sLBInfo;
                cdSDataSLBRates[sLBInfo.SLBID].sLBCurrentStatus = sLBCurrentStatus;
                cdSDataSLBRates[sLBInfo.SLBID].LoadFactor = LoadFactor;
            }
            else
            {
                SDataSLBRates sDataSLBRates = new SDataSLBRates();
                sDataSLBRates.sLBInfo = sLBInfo;
                sDataSLBRates.sLBCurrentStatus = sLBCurrentStatus;
                sDataSLBRates.LoadFactor = LoadFactor;
                cdSDataSLBRates.TryAdd(sLBInfo.SLBID, sDataSLBRates);
                BaseCount.SLBRun = cdSDataSLBRates.Count;
            }

            //更新sCharData或sCharData2
            SCharSLBRate sCharSLBRate = new SCharSLBRate(sLBInfo, sLBCurrentStatus.sMessageRates, sLBCurrentStatus.SystemandProcessInfo, LoadFactor);

            Dictionary<string, SSLBBusRateStatus> dictionary = new Dictionary<string, SSLBBusRateStatus>();
            if (sLBCurrentStatus.cdBusRates != null)
            {
                foreach (BusRates brs in sLBCurrentStatus.cdBusRates.Values)
                {
                    SSLBBusRateStatus sSLBBusRateStatus = new SSLBBusRateStatus(sLBInfo, brs);
                    dictionary.Add(sSLBBusRateStatus.BusID, sSLBBusRateStatus);
                }
            }

            if (!sCharData.cdCharSLBRate.ContainsKey(sLBInfo.SLBID))
            {
                sCharData.cdCharSLBRate.TryAdd(sLBInfo.SLBID, sCharSLBRate);
                sCharData.cdCharSLBbusRate.TryAdd(sLBInfo.SLBID, dictionary);
            }
            else
            {
                sCharData2.cdCharSLBRate.TryAdd(sLBInfo.SLBID, sCharSLBRate);
                sCharData2.cdCharSLBbusRate.TryAdd(sLBInfo.SLBID, dictionary);
            }
        }

        /// <summary>
        /// aBusServer承载情况事件 对<see cref="cdSBusServerRates"/>、<see cref="sCharData"/>进行更新操作
        /// </summary>
        /// <param name="sBusServerRates"></param>
        /// <param name="Online"></param>
        private void OMClient_DataBusServerRatesEvent(SBusServerRates sBusServerRates, bool Online)
        {
            if (!Online)
            {//不在线 移除
                if (cdSBusServerRates.ContainsKey(sBusServerRates.BusServerID))
                {
                    SBusServerRates sBusServerRRates;
                    cdSBusServerRates.TryRemove(sBusServerRates.BusServerID, out sBusServerRRates);
                    sBusServerRRates = null;
                }
                return;
            }
            //更新或新增
            if (cdSBusServerRates.ContainsKey(sBusServerRates.BusServerID))
            {
                cdSBusServerRates[sBusServerRates.BusServerID] = sBusServerRates;
            }
            else
            {
                cdSBusServerRates.TryAdd(sBusServerRates.BusServerID, sBusServerRates);
            }
            string BusServerName = "";
            if (dicBusServerInfo.ContainsKey(sBusServerRates.BusServerID))
            {
                BusServerName = dicBusServerInfo[sBusServerRates.BusServerID].BusServerName;
            }
            UpdateAbnormalBus(sBusServerRates, BusServerName);
            //更新sCharData或sCharData2
            SCharBusServerRates sCharBusServerRateandStatus = new SCharBusServerRates(sBusServerRates, sBusServerRates.SystemandProcessInfo);
            if (dicBusServerInfo.ContainsKey(sBusServerRates.BusServerID))
            {
                sCharBusServerRateandStatus.BusServerName = dicBusServerInfo[sBusServerRates.BusServerID].BusServerName;
            }
            Dictionary<string, SBusServerBusRatesandStatus> dictionary = new Dictionary<string, SBusServerBusRatesandStatus>();

            foreach (SBusServerBusRates brs in sBusServerRates.sdBusRates.Values)
            {
                SBusServerBusRatesandStatus sBusServerBusRatesandStatus = new SBusServerBusRatesandStatus(brs, sBusServerRates.BusServerID);
                sBusServerBusRatesandStatus.BusServerName = BusServerName;
                if (dicBusinessInfo.ContainsKey(brs.BusID))
                    sBusServerBusRatesandStatus.BusName = dicBusinessInfo[brs.BusID].BusName;

                dictionary.Add(sBusServerBusRatesandStatus.BusID, sBusServerBusRatesandStatus);
                // brs.RangeAbnorma
            }
            if (!sCharData.cdCharBusServerRate.ContainsKey(sBusServerRates.BusServerID))
            {
                sCharData.cdCharBusServerRate.TryAdd(sBusServerRates.BusServerID, sCharBusServerRateandStatus);
                sCharData.cdCharBusSeBusRate.TryAdd(sBusServerRates.BusServerID, dictionary);
            }
            else
            {
                sCharData2.cdCharBusServerRate.TryAdd(sBusServerRates.BusServerID, sCharBusServerRateandStatus);
                sCharData2.cdCharBusSeBusRate.TryAdd(sBusServerRates.BusServerID, dictionary);
            }
        }

        #region 检查并更新业务异常
        /// <summary>
        /// 检查并更新业务异常
        /// </summary>
        /// <param name="sBusServerRates"></param>
        void UpdateAbnormalBus(SBusServerRates sBusServerRates, string BusServerName)
        {
            foreach (SBusServerBusRates brs in sBusServerRates.sdBusRates.Values)
            {
                if (brs.StatRangeData.SysErrorCount > 0)
                {
                    BusAbnormal busAbnormal = new BusAbnormal();
                    busAbnormal.AbnormalCount = brs.StatRangeData.SysErrorCount;
                    busAbnormal.LastMessagge = brs.StatRangeData.LSysEM;
                    UpdateAbnormalBus(busAbnormal, EHandingBusinessResult.SysError, sBusServerRates, brs, BusServerName);
                }
                if (brs.StatRangeData.BusErrorCount > 0)
                {
                    BusAbnormal busAbnormal = new BusAbnormal();
                    busAbnormal.AbnormalCount = brs.StatRangeData.BusErrorCount;
                    busAbnormal.LastMessagge = brs.StatRangeData.LBusEM;
                    UpdateAbnormalBus(busAbnormal, EHandingBusinessResult.BusinessError, sBusServerRates, brs, BusServerName);
                }
                if (brs.StatRangeData.BusAbnormalCount > 0)
                {
                    BusAbnormal busAbnormal = new BusAbnormal();
                    busAbnormal.AbnormalCount = brs.StatRangeData.BusAbnormalCount;
                    busAbnormal.LastMessagge = brs.StatRangeData.LBusAM;
                    UpdateAbnormalBus(busAbnormal, EHandingBusinessResult.BusinessAbnormal, sBusServerRates, brs, BusServerName);
                }
            }
        }
        void UpdateAbnormalBus(BusAbnormal busAbnormal, EHandingBusinessResult eHResult, SBusServerRates sBusServerRates, SBusServerBusRates brs, string BusServerName)
        {
            busAbnormal.AllCount = brs.StatRangeData.AllCount;
            busAbnormal.RecordTime = DateTime.Now;
            busAbnormal.AbnormalPercent = (decimal)busAbnormal.AbnormalCount / (decimal)busAbnormal.AllCount;

            if (busAbnormal.AbnormalCount >= dicRecordPoint[eHResult][0].Count && busAbnormal.AbnormalPercent >= dicRecordPoint[eHResult][0].Percent)
            {//大于第一个记录基准点才处理

                if (!cdBusAbnormal.ContainsKey(sBusServerRates.BusServerID))
                {
                    cdBusAbnormal.TryAdd(sBusServerRates.BusServerID, new Dictionary<string, Dictionary<EHandingBusinessResult, BusAbnormal>>());
                }
                if (!cdBusAbnormal[sBusServerRates.BusServerID].ContainsKey(brs.BusID))
                {
                    cdBusAbnormal[sBusServerRates.BusServerID].Add(brs.BusID, new Dictionary<EHandingBusinessResult, BusAbnormal>());
                }
                bool ContaineRecord = false;//是否包含有效记录
                if (cdBusAbnormal[sBusServerRates.BusServerID][brs.BusID].ContainsKey(eHResult))
                {
                    if ((busAbnormal.RecordTime - cdBusAbnormal[sBusServerRates.BusServerID][brs.BusID][eHResult].RecordTime).Hours >= GetHourMin1BySecond(brs.StatPeriod))
                    {//超时，删除
                        cdBusAbnormal[sBusServerRates.BusServerID][brs.BusID].Remove(eHResult);
                        ContaineRecord = false;
                    }
                    else if (cdBusAbnormal[sBusServerRates.BusServerID][brs.BusID][eHResult].NextReportPoint == -1)
                    {//已经达到最大值范围
                        return;
                    }
                    {//有效
                        ContaineRecord = true;
                    }
                }
                if (ContaineRecord == false)
                {
                    //没有历史有效记录点，直接增加 记得发消息OOOOOOOOOOOOOOOOOOOOO
                    busAbnormal.NextReportPoint = GetNextPointIndex(busAbnormal, eHResult);
                    cdBusAbnormal[sBusServerRates.BusServerID][brs.BusID].Add(eHResult, busAbnormal);
                    SendBusAbnormalMessage(busAbnormal, eHResult, sBusServerRates, brs, BusServerName);
                }
                else
                {  // 有历史有效记录点进行对比
                    int Poinindex = cdBusAbnormal[sBusServerRates.BusServerID][brs.BusID][eHResult].NextReportPoint;
                    if (busAbnormal.AbnormalCount >= dicRecordPoint[eHResult][Poinindex].Count && busAbnormal.AbnormalPercent >= dicRecordPoint[eHResult][Poinindex].Percent)
                    {//当前记录大于上次标记的【下次记录基点】
                     //有效当前记录更新记得发消息OOOOOOOOOOOOOOOOOOOOO
                        busAbnormal.NextReportPoint = GetNextPointIndex(busAbnormal, eHResult);
                        cdBusAbnormal[sBusServerRates.BusServerID][brs.BusID][eHResult] = busAbnormal;
                        SendBusAbnormalMessage(busAbnormal, eHResult, sBusServerRates, brs, BusServerName);
                    }
                }
            }
        }
        /// <summary>
        /// 获取下一个记录基点的索引值 -1为已经最大;
        /// </summary>
        /// <param name="busAbnormal"></param>
        /// <param name="eHResult"></param>
        /// <returns></returns>
        private int GetNextPointIndex(BusAbnormal busAbnormal, EHandingBusinessResult eHResult)
        {
            int NextPoinindex = -1;
            for (int i = 1; i < dicRecordPoint[eHResult].Count; i++)
            {
                if (busAbnormal.AbnormalCount >= dicRecordPoint[eHResult][i].Count && busAbnormal.AbnormalPercent >= dicRecordPoint[eHResult][i].Percent)
                {
                }
                else
                {
                    NextPoinindex = i;
                    if (NextPoinindex >= dicRecordPoint[eHResult].Count)
                    {//超过上限不处理
                        NextPoinindex = -1;
                    }
                    break;
                }
            }
            return NextPoinindex;
        }
        private void SendBusAbnormalMessage(BusAbnormal busAbnormal, EHandingBusinessResult eHResult, SBusServerRates sBusServerRates, SBusServerBusRates brs, string BusServerName)
        {
            if (!isMonitorService)
            {
                return;
            }
            /*
            string BusName = "";
            if (dicBusinessInfo.ContainsKey(brs.BusID))
                BusName = dicBusinessInfo[brs.BusID].BusName;
            DingMessage.SDingMessage dingMessage = new DingMessage.SDingMessage(1, "oa");
            if (dicBusinessInfo != null && dicBusinessInfo.ContainsKey(brs.BusID) && dicBusinessInfo[brs.BusID].MSGID > 0)
            {
                dingMessage.MGID = dicBusinessInfo[brs.BusID].MSGID;
            }
            string ErrorType = "";
            if (eHResult == EHandingBusinessResult.SysError)
            {
                ErrorType = "业务Sys异常";
            }
            else if (eHResult == EHandingBusinessResult.BusinessError)
            {
                ErrorType = "业务Bus错误";
            }
            else if (eHResult == EHandingBusinessResult.BusinessAbnormal)
            {
                ErrorType = "业务Bus异常";
            }
            dingMessage.Content = busAbnormal.LastMessagge;

            dingMessage.oa = new DingMessage.OA();
            dingMessage.oa.Msgkind = 2;
            dingMessage.oa.Title = BusServerName + "[" + brs.BusID + "]" + ErrorType;
            dingMessage.oa.form.Add(new DingMessage.KeyValue("BusServer:", BusServerName + "[" + sBusServerRates.BusServerID + "]"));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("业务:", BusName + "[" + brs.BusID + "]"));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("异常量:", busAbnormal.AbnormalCount.ToString() + " 异常率 " + ((int)(busAbnormal.AbnormalPercent * 100)).ToString() + "%"));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("总量:", busAbnormal.AllCount.ToString()));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("统计周期:", ConvertSecond(brs.StatPeriod)));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("时间:", DateTime.Now.ToString()));
            //   dingMessage.oa.form.Add(new KeyValue("URL ", MyPubConstant.HttpServerURL + "/OMS/myspringClear/sysstatus?Param=1"));
            //dingMessage.oa.MessageUrl = MyPubConstant.HttpServerURLOMS + "/OMS/myspringClear/sysstatus?Param=1";
            //dingMessage.oa.PcMessageUrl = MyPubConstant.HttpServerURLOMS + "/OMS/myspringClear/sysstatus?Param=1";
            //dingMessage.oa.Author = "" + MyPubConstant.OperationMaintenanceID;

            try
            {
                DingMessageEvent?.Invoke(JsonConvert.SerializeObject(dingMessage));
            }
            catch
            { }
            */
        }

        #endregion

        /// <summary>
        /// 跟踪结果事件,保存到<see cref="cdSTrackResult"/>
        /// </summary>
        /// <param name="list"></param>
        private void OMClient_DataTrackResultEvent(List<STrackResult> list)
        {
            if (list != null && list.Count > 0)
            {
                foreach (STrackResult sTRt in list)
                {
                    if (cdSTrackResult.ContainsKey(sTRt.TCID))
                    {
                        cdSTrackResult[sTRt.TCID].Enqueue(sTRt);
                    }
                }
                //STrackResult sTR = list[0];
                //string inData = "", outData = "";
                //if (sTR.InData != null)
                //{
                //    if (sTR.InZip == 1) //GZip压缩
                //        sTR.InData = GZipHelper.Decompress(sTR.InData);
                //    inData = Encoding.UTF8.GetString(sTR.InData, 0, sTR.InData.Length);
                //}
                //if (sTR.OutData != null)
                //{

                //    if (sTR.OutZip == 1)//GZip压缩)
                //        sTR.OutData = GZipHelper.Decompress(sTR.OutData);
                //    outData = Encoding.UTF8.GetString(sTR.OutData, 0, sTR.OutData.Length);
                //}
                //// ShowInfo(sTR, inData, outData);
            }
        }

        /// <summary>
        /// SLB 上下事件 存到<see cref="QSysInfo"></see>中
        /// </summary>
        /// <param name="sLBInfo"></param>
        private void OMClient_SLBInfoUpDownEvent(SLBInfo sLBInfo, string Note)
        {
            if (sLBInfo.Status == 1 && !dicSLBInfo.ContainsKey(sLBInfo.SLBID))
            {
                dicSLBInfo.Add(sLBInfo.SLBID, sLBInfo);
            }
            if (cdSDataSLBRates.ContainsKey(sLBInfo.SLBID) && sLBInfo.Status == 0)
            {
                SDataSLBRates sDataSLBRates;
                cdSDataSLBRates.TryRemove(sLBInfo.SLBID, out sDataSLBRates);
            }
            else if (!cdSDataSLBRates.ContainsKey(sLBInfo.SLBID) && sLBInfo.Status == 1)
            {
                SDataSLBRates sDataSLBRates = new SDataSLBRates();
                sDataSLBRates.sLBInfo = sLBInfo;
                sDataSLBRates.sLBCurrentStatus = new SLBCurrentStatus(true);
                sDataSLBRates.LoadFactor = 0;
                cdSDataSLBRates.TryAdd(sLBInfo.SLBID, sDataSLBRates);
                BaseCount.SLBRun = cdSDataSLBRates.Count;
            }
            BaseCount.SLBRun = cdSDataSLBRates.Count;
            string info = string.Format("SLB:{0} {1}  {2}  {3}", sLBInfo.SLBName, sLBInfo.SLBID, sLBInfo.Status == 1 ? "上线" : "下线", Note);
            SSysinfoLog sSysinfoLog = new SSysinfoLog(info);
            if (QSysInfo.Count > SysInfo_Maxnum)
            {
                QSysInfo.Dequeue();
            }
            QSysInfo.Enqueue(sSysinfoLog);
            if (sLBInfo.Status == 1)//上线优化 与随之上线的BusServer一起通知
            {
                SLBUpDownOptimize sLBUpDownO = new SLBUpDownOptimize();
                sLBUpDownO.sLBInfo = new SLBInfo(sLBInfo);
                sLBUpDownO.Status = 1;
                sLBUpDownO.Note = "上线";
                cdSLBUp.TryAdd(sLBInfo.SLBID + "_1", sLBUpDownO);
            }
            else
            { SendSysRunMessage(sLBInfo.SLBName + "[" + sLBInfo.SLBID + "]" + (sLBInfo.Status == 1 ? "上线" : "下线"), info, sLBInfo.Status == 1 ? 1 : 2); }

            //  Showrich_SysInfo(info);
        }

        /// <summary>
        /// 判断并发送SLB上线事件消息
        /// </summary>
        /// <param name="bso"></param>
        /// <returns></returns>
        bool NeedSendSLBUpDown(SLBUpDownOptimize bso)
        {
            bool canSend = false;

            if (bso.SencondTime >= 10)
                canSend = true;
            if (bso.ListBusServer.Count == cdSBusServerRates.Count)
                canSend = true;

            if (canSend)
            {
                SLBUpDownOptimize bso2;
                cdSLBUp.TryRemove(bso.sLBInfo.SLBID + "_" + bso.Status.ToString(), out bso2);
                string info = "";

                if (bso.Status == 1)
                {
                    if (cdSDataSLBRates.ContainsKey(bso.sLBInfo.SLBID) && cdSDataSLBRates[bso.sLBInfo.SLBID].sLBCurrentStatus != null && cdSDataSLBRates[bso.sLBInfo.SLBID].sLBCurrentStatus.BuServerList != null)
                    {
                        foreach (string BusServerID in cdSDataSLBRates[bso.sLBInfo.SLBID].sLBCurrentStatus.BuServerList)
                        {
                            if (!bso.ListBusServer.Contains(BusServerID))
                            {
                                bso.ListBusServer.Add(BusServerID);
                            }
                        }
                    }
                    info = "已连接业务服务器: ";
                    foreach (string BusServerID in bso.ListBusServer)
                    {
                        info += BusServerID + ",";
                    }
                    info += bso.Note;
                }
                SendSysRunMessage(bso.sLBInfo.SLBName + "[" + bso.sLBInfo.SLBID + "]" + (bso.Status == 1 ? "上线" : "下线"), info, bso.Status == 1 ? 1 : 0);
            }
            return canSend;
        }


        /// <summary>
        /// BusServer 上下事件 存到<see cref="QSysInfo"></see>中
        /// </summary>
        /// <param name="fromsLBInfo"></param>
        /// <param name="BusServerID"></param>
        /// <param name="Status"></param>
        private void OMClient_BusServerUpDownEvent(SLBInfo fromsLBInfo, string BusServerID, int Status, string Note)
        {
            if (true)
            {
                if (cdSLBUp.ContainsKey(fromsLBInfo.SLBID + "_1") && Status == 1)//随SLB上线而上线的处理
                {
                    cdSLBUp[fromsLBInfo.SLBID + "_1"].ListBusServer.Add(BusServerID);
                    NeedSendSLBUpDown(cdSLBUp[fromsLBInfo.SLBID + "_1"]);
                }
                else
                {
                    string cdkey = BusServerID + "_" + Status.ToString();
                    BusServerUpDownOptimize bso;
                    if (cdBusServerUpDown.ContainsKey(cdkey))
                    {
                        bso = cdBusServerUpDown[cdkey];
                        bso.ListSLB.Add(new SLBInfo(fromsLBInfo));
                    }
                    else
                    {
                        bso = new BusServerUpDownOptimize();
                        bso.BusServerID = BusServerID;
                        bso.Note = Note;
                        bso.Status = Status;
                        bso.ListSLB.Add(new SLBInfo(fromsLBInfo));
                        cdBusServerUpDown.TryAdd(cdkey, bso);
                    }
                    NeedSendBusServerUpDown(bso);
                }
                if (cdSBusServerRates.ContainsKey(BusServerID) && Status == 0)
                {
                    SBusServerRates sBusServerRates;
                    cdSBusServerRates.TryRemove(BusServerID, out sBusServerRates);
                    sBusServerRates = null;

                }
                else if (!cdSBusServerRates.ContainsKey(BusServerID) && Status == 1)
                {
                    SBusServerRates sBusServerRates = new SBusServerRates();
                    sBusServerRates.BusServerID = BusServerID;
                    cdSBusServerRates.TryAdd(BusServerID, sBusServerRates);

                }
                BaseCount.BusSRun = cdSBusServerRates.Count;
            }

            string info = string.Format("BusServer:{0}   {1}  from {2} {3} {4}", BusServerID, Status == 1 ? "上线" : "下线", fromsLBInfo.SLBName, fromsLBInfo.SLBID, Note);
            SSysinfoLog sSysinfoLog = new SSysinfoLog(info);
            if (QSysInfo.Count > SysInfo_Maxnum)
            {
                QSysInfo.Dequeue();
            }

            QSysInfo.Enqueue(sSysinfoLog);
            // SendMessage(BusServerID + (Status == 1 ? "上线" : "下线"), info, Status == 1 ? 1 : 0);
            //Showrich_SysInfo(info);
        }
        /// <summary>
        /// 判断并发送BusServer上下事件消息
        /// </summary>
        /// <param name="bso"></param>
        /// <returns></returns>
        bool NeedSendBusServerUpDown(BusServerUpDownOptimize bso)
        {
            bool canSend = false;
            if (bso.SencondTime >= 5)
                canSend = true;
            if (bso.ListSLB.Count == cdSDataSLBRates.Count)
                canSend = true;

            if (canSend)
            {
                BusServerUpDownOptimize bso2;
                cdBusServerUpDown.TryRemove(bso.BusServerID + "_" + bso.Status.ToString(), out bso2);
                string info = "from";
                foreach (SLBInfo fromsLBInfo in bso.ListSLB)
                {
                    info += fromsLBInfo.SLBName + "[" + fromsLBInfo.SLBID + "],";
                }
                info += bso.Note;
                string BusServerName = "";
                if (dicBusServerInfo.ContainsKey(bso.BusServerID))
                {
                    BusServerName = dicBusServerInfo[bso.BusServerID].BusServerName;
                }
                SendSysRunMessage(BusServerName + "[" + bso.BusServerID + "]" + (bso.Status == 1 ? "上线" : "下线"), info, bso.Status == 1 ? 1 : 0);
            }
            return canSend;
        }

        /// <summary>
        /// SLB通过心跳检测BusServer服务器停启业务服务消息
        /// </summary>
        /// <param name="sLBInfo"></param>
        /// <param name="sHeartbeatBusServerInfo"></param>
        private void OMConnect_HeartbeatBusServerInfoEvent(SLBInfo sLBInfo, SHeartbeatBusServerInfo sHearInfo)
        {
            string BusServerName = "";
            if (dicBusServerInfo.ContainsKey(sHearInfo.BusServerID))
            {
                BusServerName = dicBusServerInfo[sHearInfo.BusServerID].BusServerName;
            }
            string info = "SLB[" + sLBInfo.SLBID + "]通过心跳检测业务服务器[" + BusServerName + "][" + sHearInfo.BusServerID + "]：" + sHearInfo.Note;
            SSysinfoLog sSysinfoLog = new SSysinfoLog(info);
            if (QSysInfo.Count > SysInfo_Maxnum)
            {
                QSysInfo.Dequeue();
            }

            QSysInfo.Enqueue(sSysinfoLog);
            SendSysRunMessage("[" + sLBInfo.SLBID + "]心跳" + (sHearInfo.StopServices ? "暂停" : "重启") + "[" + sHearInfo.BusServerID + "]业务"
                , info, sHearInfo.StopServices ? 3 : 1);
        }

        #endregion

        #region 对外ServerStatus Function

        /// <summary>
        ///系统基本配置数量
        /// </summary>
        /// <returns></returns>
        public SSysBaseCount GetSysBaseCount()
        {
            BaseCount.SLBRun = cdSDataSLBRates.Count;
            BaseCount.BusSRun = cdSBusServerRates.Count;
            return BaseCount;
        }

        #region----------------------BusInfo---------------------------------

        /// <summary>
        /// 获取系统业务基本信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetBusInfoStatusTable()
        {
            return dtBusInfoStatusAndRate.Copy();
        }
        /// <summary>
        /// 获取系统业务基本信息 (转换为DataTable)
        /// </summary>
        /// <param name="dtref"></param>
        /// <returns></returns>
        public bool GetBusInfoStatusTable(ref DataTable dtref)
        {
            List<string> listColumnsKey = new List<string>();
            listColumnsKey.Add("BusID");
            return CopyTable(GetBusInfoStatusTable(), ref dtref, listColumnsKey);
        }



        #endregion

        #region----------------SLBRates---------------------------------
        /// <summary>
        /// 获取<see cref="ServerLoadBalancing"/>整体情况
        /// </summary>
        /// <returns></returns>
        public List<SSLBRateandStatus> GetSLBRates()
        {
            List<SSLBRateandStatus> list = new List<SSLBRateandStatus>();
            foreach (SDataSLBRates sdr in cdSDataSLBRates.Values)
            {
                SSLBRateandStatus sSLBRateStatus = new SSLBRateandStatus(sdr.sLBInfo, sdr.sLBCurrentStatus.sMessageRates, sdr.sLBCurrentStatus.SystemandProcessInfo, sdr.sLBCurrentStatus, sdr.LoadFactor);
                list.Add(sSLBRateStatus);
            }
            return list;
        }

        /// <summary>
        ///  获取<see cref="ServerLoadBalancing"/>整体情况(转换为DataTable)
        /// </summary>
        /// <returns></returns>
        public DataTable GetSLBRatesTable()
        {
            List<SSLBRateandStatus> list = GetSLBRates();
            return ListToTableHelper.ToDataTable(list);
        }

        /// <summary>
        ///  将<see cref="ServerLoadBalancing"/> 整体情况更新到指定表中
        /// </summary>
        /// <param name="dtref">指定的表</param>
        /// <returns></returns>
        public bool GetSLBRatesTable(ref DataTable dtref)
        {
            List<string> listColumnsKey = new List<string>();
            listColumnsKey.Add("SLBID");
            return CopyTable(GetSLBRatesTable(), ref dtref, listColumnsKey);
        }

        /// <summary>
        ///  获取<see cref="ServerLoadBalancing"/>中的业务情况
        /// </summary>
        /// <returns></returns>
        public List<SSLBBusRateStatus> GetSLBBusRates()
        {
            List<SSLBBusRateStatus> list = new List<SSLBBusRateStatus>();
            foreach (SDataSLBRates sdr in cdSDataSLBRates.Values)
            {
                if (sdr.sLBCurrentStatus.cdBusRates != null)
                    foreach (BusRates brs in sdr.sLBCurrentStatus.cdBusRates.Values)
                    {
                        SSLBBusRateStatus sSLBBusRateStatus = new SSLBBusRateStatus(sdr.sLBInfo, brs);
                        list.Add(sSLBBusRateStatus);
                    }
            }
            return list;
        }

        /// <summary>
        /// 获取<see cref="ServerLoadBalancing"/>中的业务情况(转换为DataTable)
        /// </summary>
        /// <returns></returns>
        public DataTable GetSLBBusRatesTable()
        {
            List<SSLBBusRateStatus> list = GetSLBBusRates();
            return ListToTableHelper.ToDataTable(list);
        }
        /// <summary>
        ///  将<see cref="ServerLoadBalancing"/>中的业务情况更新到指定表中
        /// </summary>
        /// <param name="dtref">指定的表</param>
        /// <returns></returns>
        public bool GetSLBBusRatesTable(ref DataTable dtref)
        {
            List<string> listColumnsKey = new List<string>();
            listColumnsKey.Add("SLBID");
            listColumnsKey.Add("BusID");
            return CopyTable(GetSLBBusRatesTable(), ref dtref, listColumnsKey);
        }

        /// <summary>
        ///  获取指定<see cref="ServerLoadBalancing"/>中的业务情况
        /// </summary>
        /// <param name="SLBID"></param>
        /// <returns></returns>
        public List<SSLBBusRateStatus> GetSLBBusRates(string SLBID)
        {
            List<SSLBBusRateStatus> list = new List<SSLBBusRateStatus>();
            if (cdSDataSLBRates.ContainsKey(SLBID))
            {
                SDataSLBRates sdr = cdSDataSLBRates[SLBID];
                foreach (BusRates brs in sdr.sLBCurrentStatus.cdBusRates.Values)
                {
                    SSLBBusRateStatus sSLBBusRateStatus = new SSLBBusRateStatus(sdr.sLBInfo, brs);
                    list.Add(sSLBBusRateStatus);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定<see cref="ServerLoadBalancing"/>中的业务情况(转换为DataTable)
        /// </summary>
        /// <param name="SLBID"></param>
        /// <returns></returns>
        public DataTable GetSLBBusRatesTable(string SLBID)
        {
            List<SSLBBusRateStatus> list = GetSLBBusRates(SLBID);
            return ListToTableHelper.ToDataTable(list);
        }

        /// <summary>
        ///   将指定<see cref="ServerLoadBalancing"/>中的业务情况更新到指定表中
        /// </summary>
        /// <param name="dtref">指定的表</param>
        /// <param name="SLBID"></param>
        /// <returns></returns>
        public bool GetSLBBusRatesTable(ref DataTable dtref, string SLBID)
        {
            List<string> listColumnsKey = new List<string>();
            listColumnsKey.Add("SLBID");
            listColumnsKey.Add("BusID");
            return CopyTable(GetSLBBusRatesTable(SLBID), ref dtref, listColumnsKey);
        }
        #endregion

        #region-------------------BusServerRate----------------------
        /// <summary>
        ///  获取<see cref="SLBBusServer"/>整体情况
        /// </summary>
        /// <returns></returns>
        public List<SBusServerRateandStatus> GetBusServerRates()
        {
            List<SBusServerRateandStatus> list = new List<SBusServerRateandStatus>();

            foreach (SBusServerRates sdr in cdSBusServerRates.Values)
            {
                SBusServerRateandStatus sBusServerRateandStatus = new SBusServerRateandStatus(sdr, sdr.SystemandProcessInfo);
                if (dicBusServerInfo.ContainsKey(sdr.BusServerID))
                {
                    sBusServerRateandStatus.BusServerName = dicBusServerInfo[sdr.BusServerID].BusServerName;
                }
                list.Add(sBusServerRateandStatus);
            }
            return list;
        }
        /// <summary>
        /// 获取在线<see cref="SLBBusServer"/>ID及名称
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetBusServerIDOnLine()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (SBusServerRates sdr in cdSBusServerRates.Values)
            {
                SBusServerRateandStatus sBusServerRateandStatus = new SBusServerRateandStatus(sdr, sdr.SystemandProcessInfo);
                string BusServerName = "";
                if (dicBusServerInfo.ContainsKey(sdr.BusServerID))
                {
                    BusServerName = dicBusServerInfo[sdr.BusServerID].BusServerName;
                }
                dic.Add(sBusServerRateandStatus.BusServerID, BusServerName);
            }
            return dic;
        }
        /// <summary>
        ///   获取<see cref="SLBBusServer"/>整体情况(转换为DataTable)
        /// </summary>
        /// <returns></returns>
        public DataTable GetBusServerRatesTable()
        {
            List<SBusServerRateandStatus> list = GetBusServerRates();
            return ListToTableHelper.ToDataTable(list);
        }

        /// <summary>
        ///  将<see cref="SLBBusServer"/>整体情况 更新到指定表中
        /// </summary>
        /// <param name="dtref">指定表</param>
        /// <returns></returns>
        public bool GetBusServerRatesTable(ref DataTable dtref)
        {
            List<string> listColumnsKey = new List<string>();
            listColumnsKey.Add("BusServerID");
            return CopyTable(GetBusServerRatesTable(), ref dtref, listColumnsKey);
        }
        /// <summary>
        ///获取<see cref="SLBBusServer"/>中的业务情况
        /// </summary>
        /// <returns></returns>
        public List<SBusServerBusRatesandStatus> GetBusServerBusRates()
        {
            List<SBusServerBusRatesandStatus> list = new List<SBusServerBusRatesandStatus>();

            foreach (SBusServerRates sdr in cdSBusServerRates.Values)
            {
                string BusServerName = "";
                if (dicBusServerInfo.ContainsKey(sdr.BusServerID))
                {
                    BusServerName = dicBusServerInfo[sdr.BusServerID].BusServerName;
                }
                foreach (SBusServerBusRates brs in sdr.sdBusRates.Values)
                {
                    SBusServerBusRatesandStatus sBusServerBusRatesandStatus = new SBusServerBusRatesandStatus(brs, sdr.BusServerID);
                    sBusServerBusRatesandStatus.BusServerName = BusServerName;
                    if (dicBusinessInfo.ContainsKey(brs.BusID))
                        sBusServerBusRatesandStatus.BusName = dicBusinessInfo[brs.BusID].BusName;
                    list.Add(sBusServerBusRatesandStatus);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取<see cref="SLBBusServer"/>中的业务情况(转换为DataTable)
        /// </summary>
        /// <returns></returns>
        public DataTable GetBusServerBusRatesTable()
        {
            List<SBusServerBusRatesandStatus> list = GetBusServerBusRates();
            return ListToTableHelper.ToDataTable(list);
        }

        /// <summary>
        ///  将<see cref="SLBBusServer"/>中的业务情况更新到指定表中
        /// </summary>
        /// <param name="dtref">指定的表</param>
        /// <returns></returns>
        public bool GetBusServerBusRatesTable(ref DataTable dtref)
        {
            List<string> listColumnsKey = new List<string>();
            listColumnsKey.Add("BusServerID");
            listColumnsKey.Add("BusID");
            return CopyTable(GetBusServerBusRatesTable(), ref dtref, listColumnsKey);
        }


        /// <summary>
        /// 获取指定<see cref="SLBBusServer"/>中的业务情况
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public List<SBusServerBusRatesandStatus> GetBusServerBusRates(string BusServerID)
        {
            List<SBusServerBusRatesandStatus> list = new List<SBusServerBusRatesandStatus>();
            if (cdSBusServerRates.ContainsKey(BusServerID))
            {
                string BusServerName = "";
                if (dicBusServerInfo.ContainsKey(BusServerID))
                {
                    BusServerName = dicBusServerInfo[BusServerID].BusServerName;
                }
                SBusServerRates sdr = cdSBusServerRates[BusServerID];
                foreach (SBusServerBusRates brs in sdr.sdBusRates.Values)
                {
                    SBusServerBusRatesandStatus sBusServerBusRatesandStatus = new SBusServerBusRatesandStatus(brs, sdr.BusServerID);
                    sBusServerBusRatesandStatus.BusServerName = BusServerName;
                    if (dicBusinessInfo.ContainsKey(brs.BusID))
                        sBusServerBusRatesandStatus.BusName = dicBusinessInfo[brs.BusID].BusName;
                    list.Add(sBusServerBusRatesandStatus);
                }
            }
            return list;
        }

        /// <summary>
        ///  获取指定<see cref="SLBBusServer"/>中的业务情况(转换为DataTable)
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public DataTable GetBusServerBusRatesTable(string BusServerID)
        {
            List<SBusServerBusRatesandStatus> list = GetBusServerBusRates(BusServerID);
            return ListToTableHelper.ToDataTable(list);
        }

        /// <summary>
        ///  将指定<see cref="SLBBusServer"/>中的业务情况更新到指定表中
        /// </summary>
        /// <param name="dtref">指定的表</param>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public bool GetBusServerBusRatesTable(ref DataTable dtref, string BusServerID)
        {
            List<string> listColumnsKey = new List<string>();
            listColumnsKey.Add("BusServerID");
            listColumnsKey.Add("BusID");
            return CopyTable(GetBusServerBusRatesTable(BusServerID), ref dtref, listColumnsKey);
        }





        /// <summary>
        /// 获取一条信息日志信息并从队列中删除
        /// </summary>
        /// <returns></returns>
        public SSysinfoLog GetSSysinfoLogAndDel()
        {
            if (QSysInfo.Count > 0)
            {
                return QSysInfo.Dequeue();

            }
            return null;
        }
        /// <summary>
        /// 获取信息日志信息
        /// </summary>
        /// <returns></returns>
        public List<SSysinfoLog> GetSSysinfoLog()
        {
            if (QSysInfo.Count > 0)
            {
                return QSysInfo.ToList();

            }
            return null;
        }


        #endregion

        #region 跟踪
        /// <summary>
        /// 开始跟踪
        /// </summary>
        /// <param name="sTC"></param>
        /// <returns></returns>
        public bool StartTrack(STrackConditions sTC)
        {
            if (string.IsNullOrEmpty(sTC.TCID))
            {
                return false;
            }
            if (cdSTrackResult.ContainsKey(sTC.TCID))
            {
                return false;
            }
            ConcurrentQueue<STrackResult> concurrentQueue = new ConcurrentQueue<STrackResult>();
            cdSTrackResult.TryAdd(sTC.TCID, concurrentQueue);
            sTC.Status = 1;
            if (!OMConnect.TrackConditions(sTC))
            {
                cdSTrackResult.TryRemove(sTC.TCID, out concurrentQueue);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 停止跟踪
        /// </summary>
        /// <param name="sTC"></param>
        /// <returns></returns>
        public bool StopTrack(STrackConditions sTC)
        {
            if (string.IsNullOrEmpty(sTC.TCID))
            {
                return false;
            }
            ConcurrentQueue<STrackResult> concurrentQueue;
            if (cdSTrackResult.ContainsKey(sTC.TCID))
            {
                cdSTrackResult.TryRemove(sTC.TCID, out concurrentQueue);
            }

            sTC.Status = 0;
            return OMConnect.TrackConditions(sTC);

        }


        /// <summary>
        /// 获取跟踪结果
        /// </summary>
        /// <param name="TCID">此跟踪唯一ID</param>
        /// <returns></returns>
        public List<STrackResultTest> GetTrackResultTest(string TCID)
        {
            List<STrackResultTest> list = new List<STrackResultTest>();
            if (cdSTrackResult.ContainsKey(TCID))
            {
                while (cdSTrackResult[TCID].Count > 0)
                {
                    STrackResult sTrackResult;
                    if (cdSTrackResult[TCID].TryDequeue(out sTrackResult))
                    {
                        STrackResultTest sTrackResultTest = new STrackResultTest(sTrackResult);
                        list.Add(sTrackResultTest);
                        sTrackResult = null;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取跟踪结果
        /// </summary>
        /// <param name="TCID">此跟踪唯一ID</param>
        /// <param name="MaxCount">本次获取最大条数</param>
        /// <param name="AbandonOther">是否抛弃剩余的结果</param>
        /// <returns></returns>
        public List<STrackResultTest> GetTrackResultTest(string TCID, int MaxCount, bool AbandonOther)
        {
            List<STrackResultTest> list = new List<STrackResultTest>();
            if (cdSTrackResult.ContainsKey(TCID))
            {
                int GetCount = 0;
                while (cdSTrackResult[TCID].Count > 0 && GetCount < MaxCount)
                {
                    STrackResult sTrackResult;
                    if (cdSTrackResult[TCID].TryDequeue(out sTrackResult))
                    {
                        GetCount++;
                        STrackResultTest sTrackResultTest = new STrackResultTest(sTrackResult);
                        list.Add(sTrackResultTest);
                        sTrackResult = null;
                    }
                }
                if (AbandonOther)
                {
                    lock (cdSTrackResult[TCID])
                    {
                        ConcurrentQueue<STrackResult> concurrentQueue = new ConcurrentQueue<STrackResult>();
                        cdSTrackResult[TCID] = concurrentQueue;
                    }
                }
            }
            return list;
        }

        #endregion

        #region --------------------------图表数据----------------------------

        /// <summary>
        /// 获取业务信息的图表数据
        /// key: DateTime BusID
        /// </summary>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<string, SCharBusRatesandStatus>> GetQCharBusiness()
        {
            Dictionary<DateTime, Dictionary<string, SCharBusRatesandStatus>> dic = new Dictionary<DateTime, Dictionary<string, SCharBusRatesandStatus>>();
            lock (QCharData)
            {
                for (int i = 0; i < QCharData.Count; i++)
                {
                    if (!dic.ContainsKey(QCharData.ElementAt(i).Time))
                    {
                        Dictionary<string, SCharBusRatesandStatus> dictionary = new Dictionary<string, SCharBusRatesandStatus>();
                        SCharData sCharData = QCharData.ElementAt(i);
                        foreach (string BusID in sCharData.cdBusiness.Keys)
                        {
                            dictionary.Add(BusID, sCharData.cdBusiness[BusID]);
                        }
                        dic.Add(QCharData.ElementAt(i).Time, dictionary);
                    }
                }
            }
            return dic;
        }


        /// <summary>
        /// 获取BusServer的图表数据
        ///  key: DateTime BusServerID
        /// </summary>
        /// <returns></returns>
        public Dictionary<DateTime, ConcurrentDictionary<string, SCharBusServerRates>> GetQCharBusServer()
        {
            Dictionary<DateTime, ConcurrentDictionary<string, SCharBusServerRates>> dic = new Dictionary<DateTime, ConcurrentDictionary<string, SCharBusServerRates>>();
            lock (QCharData)
            {
                for (int i = 0; i < QCharData.Count; i++)
                {
                    if (!dic.ContainsKey(QCharData.ElementAt(i).Time))
                        dic.Add(QCharData.ElementAt(i).Time, QCharData.ElementAt(i).cdCharBusServerRate);
                }
            }
            return dic;
        }
        /// <summary>
        ///  获取指定BusServer中的业务的的图标数据
        ///   key: DateTime BusID
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<string, SBusServerBusRatesandStatus>> GetQCharBusServerBus(string BusServerID)
        {
            Dictionary<DateTime, Dictionary<string, SBusServerBusRatesandStatus>> dic = new Dictionary<DateTime, Dictionary<string, SBusServerBusRatesandStatus>>();
            lock (QCharData)
            {
                for (int i = 0; i < QCharData.Count; i++)
                {
                    if (!dic.ContainsKey(QCharData.ElementAt(i).Time))
                        if (QCharData.ElementAt(i).cdCharBusSeBusRate.ContainsKey(BusServerID))
                            dic.Add(QCharData.ElementAt(i).Time, QCharData.ElementAt(i).cdCharBusSeBusRate[BusServerID]);
                }
            }
            return dic;
        }

        /// <summary>
        ///获取SLB的图表数据
        ///key: DateTime SLBID
        /// </summary>
        /// <returns></returns>
        public Dictionary<DateTime, ConcurrentDictionary<string, SCharSLBRate>> GetQCharrSLB()
        {
            Dictionary<DateTime, ConcurrentDictionary<string, SCharSLBRate>> dic = new Dictionary<DateTime, ConcurrentDictionary<string, SCharSLBRate>>();
            lock (QCharData)
            {
                for (int i = 0; i < QCharData.Count; i++)
                {
                    if (!dic.ContainsKey(QCharData.ElementAt(i).Time))
                        dic.Add(QCharData.ElementAt(i).Time, QCharData.ElementAt(i).cdCharSLBRate);
                }
            }
            return dic;
        }
        /// <summary>
        ///获取指定SLB中的业务的的图标数据
        ///key: DateTime BusID
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public Dictionary<DateTime, Dictionary<string, SSLBBusRateStatus>> GetQCharSLBBus(string SLBID)
        {
            Dictionary<DateTime, Dictionary<string, SSLBBusRateStatus>> dic = new Dictionary<DateTime, Dictionary<string, SSLBBusRateStatus>>();
            lock (QCharData)
            {
                for (int i = 0; i < QCharData.Count; i++)
                {
                    if (!dic.ContainsKey(QCharData.ElementAt(i).Time))
                        if (QCharData.ElementAt(i).cdCharSLBbusRate.ContainsKey(SLBID))
                            dic.Add(QCharData.ElementAt(i).Time, QCharData.ElementAt(i).cdCharSLBbusRate[SLBID]);
                }
            }
            return dic;
        }


        #endregion

        #endregion
        private void SendSysRunMessage(string Title, string Content, int Msgkind)
        {
            if (!isMonitorService)
            {
                return;
            }
            /*
            if (BaseCount.SLBRun == 0 || BaseCount.BusSRun == 0 || AccessServerStatus == "离线")
            {
                Msgkind = 3;
            }
            else if (BaseCount.SLBRun < 3 || BaseCount.BusSRun < 2 || BaseCount.NoServerBus > 0)
            {
                if (Msgkind < 2)
                    Msgkind = 2;
            }
            DingMessage.SDingMessage dingMessage = new DingMessage.SDingMessage(2, "oa");
            dingMessage.Content = Content;
            dingMessage.oa = new DingMessage.OA();
            dingMessage.oa.Msgkind = Msgkind;
            dingMessage.oa.Title = Title;
            dingMessage.oa.form.Add(new DingMessage.KeyValue("SLB ", BaseCount.SLBCount.ToString() + "  运行:" + BaseCount.SLBRun));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("BusServer ", BaseCount.BusSCount.ToString() + "  运行:" + BaseCount.BusSRun));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("AccessServer ", AccessServerStatus));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("业务 ", BaseCount.BusCount.ToString() + "  运行:" + BaseCount.BusRun + (BaseCount.NoServerBus > 0 ? " 无服务:" + BaseCount.NoServerBus.ToString() : "")));
            dingMessage.oa.form.Add(new DingMessage.KeyValue("时间 ", DateTime.Now.ToString()));
            //   dingMessage.oa.form.Add(new KeyValue("URL ", MyPubConstant.HttpServerURL + "/OMS/myspringClear/sysstatus?Param=1"));
            //dingMessage.oa.MessageUrl = MyPubConstant.HttpServerURLOMS + "/OMS/myspringClear/sysstatus?Param=1";
            //dingMessage.oa.PcMessageUrl = MyPubConstant.HttpServerURLOMS + "/OMS/myspringClear/sysstatus?Param=1";
            //dingMessage.oa.Author = "OMS:" + MyPubConstant.OperationMaintenanceID;

            try
            {
                DingMessageEvent?.Invoke(JsonConvert.SerializeObject(dingMessage));
            }
            catch
            { }
            */
        }
        #region HTTPServerAPI
        string projectID = "default";
        /// <summary>
        /// 获取系统状态HTML页面
        /// </summary>
        /// <returns></returns>
        string GetHttpSysStatus()
        {
            string strHttp = PasSLog.ReadAllText("sysstatus.txt");

            /// <summary>
            /// 消息种类：0 普通消息 蓝色；1安全消息 绿色；2 警告消息 黄色 ；3 错误消息 红色
            /// </summary>
            int SystemStatuasind = 0;
            string SystemStatuas = "";

            DataTable dtBusInfoStatus = GetBusInfoStatusTable();

            StringBuilder strtBusInfoStatus2 = new StringBuilder();
            strtBusInfoStatus2.Append("<tr>");
            if (dtBusInfoStatus.Columns.Count > 0)
            {
                for (int i = 0; i < dtBusInfoStatus.Columns.Count; i++)
                {
                    if (string.Compare(dtBusInfoStatus.Columns[i].ColumnName, "Status", true) != 0)
                        strtBusInfoStatus2.Append("<td><span style=\"font-size:18px;\" >" + dtBusInfoStatus.Columns[i].ColumnName + " </span></td>");
                }
            }
            strtBusInfoStatus2.Append("</tr>");
            foreach (DataRow dr in dtBusInfoStatus.Rows)
            {
                strtBusInfoStatus2.Append("<tr>");
                for (int i = 0; i < dtBusInfoStatus.Columns.Count; i++)
                {
                    if (string.Compare(dtBusInfoStatus.Columns[i].ColumnName, "Status", true) != 0)
                    {
                        string color = "";
                        if (string.Compare(dtBusInfoStatus.Columns[i].ColumnName, "RunServerCount", true) == 0)
                        {
                            if (int.Parse(dr["Status"].ToString()) == 1)
                            {
                                int RunServCount = int.Parse(dr[dtBusInfoStatus.Columns[i].ColumnName].ToString());
                                if (RunServCount == 0)
                                {
                                    color = HttpColor.Red;//红色
                                }
                                else if (RunServCount > 1)
                                {
                                    color = HttpColor.Green;//绿色
                                }
                                else
                                {
                                    color = HttpColor.Orange;//绿色
                                }
                            }
                        }
                        strtBusInfoStatus2.Append("<td><span style=\"font-size:18px;" + color + "\" >" + dr[dtBusInfoStatus.Columns[i].ColumnName].ToString() + " </span></td>");
                    }
                }
                strtBusInfoStatus2.Append("</tr>");
            }
            //SLB 状态
            DataTable dtSLBRates = GetSLBRatesTable();
            StringBuilder strSLBtable3 = new StringBuilder();
            foreach (DataColumn dc in dtSLBRates.Columns)
            {
                if (dc.ColumnName.Equals("FileName") || dc.ColumnName.Equals("WorkingSet64"))
                {
                    continue;
                }
                if (dc.ColumnName.Length > 20)
                    strSLBtable3.Append("<tr><td><span style=\"font-size:14px;\" >" + dc.ColumnName + " </span></td>");
                else
                    strSLBtable3.Append("<tr><td><span style=\"font-size:18px;\" >" + dc.ColumnName + " </span></td>");
                if (dtSLBRates.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSLBRates.Rows.Count; i++)
                    {
                        string color = "";
                        if (string.Compare(dc.ColumnName, "CpuLoadS", true) == 0)
                        {
                            int SystemStatuasi = 1;
                            color = GetCpuColor(dtSLBRates.Rows[i][dc.ColumnName], ref SystemStatuasi);
                            if (SystemStatuasi > 1)
                            {
                                SystemStatuasind = SystemStatuasind < SystemStatuasi ? SystemStatuasi : SystemStatuasind;
                                SystemStatuas += SystemStatuas == "" ? dtSLBRates.Rows[i]["SLBName"].ToString() + "CPU过高" : ";  " + dtSLBRates.Rows[i]["SLBName"].ToString() + "CPU过高";
                            }
                        }
                        else if (string.Compare(dc.ColumnName, "ServerCount", true) == 0)
                        {
                            if (int.Parse(dtSLBRates.Rows[i][dc.ColumnName].ToString()) == BaseCount.BusSRun)
                            {
                                color = HttpColor.Green;
                            }
                            else
                            {
                                color = HttpColor.Orange;
                                SystemStatuasind = SystemStatuasind < 2 ? 2 : SystemStatuasind;
                                SystemStatuas += SystemStatuas == "" ? dtSLBRates.Rows[i]["SLBName"].ToString() + "连接业务服务器异常" : ";  " + dtSLBRates.Rows[i]["SLBName"].ToString() + "连接业务服务器异常";
                            }

                        }

                        strSLBtable3.Append("<td><span style=\"font-size:18px;" + color + "\" >" + dtSLBRates.Rows[i][dc.ColumnName].ToString() + " </span></td>");
                    }
                }
                strSLBtable3.Append("</tr>");
            }

            //BusServer 状态
            DataTable dtBusSRates = GetBusServerRatesTable();
            StringBuilder strBusStable4 = new StringBuilder();

            foreach (DataColumn dc in dtBusSRates.Columns)
            {
                if (dc.ColumnName.Equals("FileName"))
                {
                    continue;
                }
                if (dc.ColumnName.Length > 20)
                    strBusStable4.Append("<tr><td><span style=\"font-size:14px;\" >" + dc.ColumnName + " </span></td>");
                else
                    strBusStable4.Append("<tr><td><span style=\"font-size:18px;\" >" + dc.ColumnName + " </span></td>");
                if (dtBusSRates.Rows.Count > 0)
                {
                    for (int i = 0; i < dtBusSRates.Rows.Count; i++)
                    {
                        string color = "";
                        if (string.Compare(dc.ColumnName, "IPAddress", true) == 0)
                        {
                            dtBusSRates.Rows[i][dc.ColumnName] = dtBusSRates.Rows[i][dc.ColumnName].ToString().Replace(",", ", ");
                        }
                        else if (string.Compare(dc.ColumnName, "CpuLoadS", true) == 0)
                        {

                            int SystemStatuasi = 1;
                            color = GetCpuColor(dtBusSRates.Rows[i][dc.ColumnName], ref SystemStatuasi);
                            if (SystemStatuasi > 1)
                            {
                                SystemStatuasind = SystemStatuasind < SystemStatuasi ? SystemStatuasi : SystemStatuasind;
                                SystemStatuas += SystemStatuas == "" ? "BusServer[" + dtBusSRates.Rows[i]["BusServerName"].ToString() + "]CPU过高" : ";  BusServer[" + dtBusSRates.Rows[i]["BusServerName"].ToString() + "]CPU过高";
                            }
                        }
                        else if (string.Compare(dc.ColumnName, "ConnectSLB", true) == 0)
                        {
                            if (int.Parse(dtBusSRates.Rows[i][dc.ColumnName].ToString()) == BaseCount.SLBRun)
                            {
                                color = HttpColor.Green;
                            }
                            else
                            {
                                color = HttpColor.Orange;
                                SystemStatuasind = SystemStatuasind < 3 ? 3 : SystemStatuasind;
                                SystemStatuas += SystemStatuas == "" ? "BusServer[" + dtBusSRates.Rows[i]["BusServerName"].ToString() + "]连接SLB异常" : ";  " + "BusServer[" + dtBusSRates.Rows[i]["BusServerName"].ToString() + "]连接SLB异常";
                            }

                        }
                        strBusStable4.Append("<td><span style=\"font-size:18px;" + color + "\" >" + dtBusSRates.Rows[i][dc.ColumnName].ToString() + " </span></td>");
                    }
                }
                strBusStable4.Append("</tr>");
            }


            DataTable dtBusSbusRates = GetBusServerBusRatesTable();
            if (dtBusSbusRates != null && dtBusSbusRates.Columns.Contains("RAbnorma"))
            {
                //if (dtBusSbusRates.Columns.Contains("RAbnorma"))
                //    dtBusSbusRates.Columns.Remove("RAbnorma");
                if (dtBusSbusRates.Columns.Contains("RAllCount"))
                    dtBusSbusRates.Columns.Remove("RAllCount");
                if (dtBusSbusRates.Columns.Contains("RAbnormaRate"))
                    dtBusSbusRates.Columns.Remove("RAbnormaRate");
            }
            StringBuilder strBusSbustable5 = new StringBuilder();
            strBusSbustable5.Append("<tr>");
            if (dtBusSbusRates.Columns.Count > 0)
            {
                for (int i = 0; i < dtBusSbusRates.Columns.Count; i++)
                {
                    strBusSbustable5.Append("<td><span style=\"font-size:18px;\" >" + dtBusSbusRates.Columns[i].ColumnName + " </span></td>");
                }
            }
            strBusSbustable5.Append("</tr>");
            foreach (DataRow dr in dtBusSbusRates.Rows)
            {
                strBusSbustable5.Append("<tr>");
                for (int i = 0; i < dtBusSbusRates.Columns.Count; i++)
                {
                    strBusSbustable5.Append("<td><span style=\"font-size:18px;\" >" + dr[dtBusSbusRates.Columns[i].ColumnName].ToString() + " </span></td>");
                }
                strBusSbustable5.Append("</tr>");
            }


            //系统状态
            string SystemStatuas1 = "</span><span style=\"color:#009900;font-size:24px;\">正常</span> ";
            if (BaseCount.SLBRun == 0 || BaseCount.BusSRun == 0 || AccessServerStatus == "离线" || SystemStatuasind == 3)
            {
                SystemStatuas1 =
                    string.Format("</span><span style=\"color:#FF0000;font-size:24px;\">严重错误 {0}</span> ",
                    (AccessServerStatus == "离线" ? ",AccessServer离线" : "")
                  + (BaseCount.SLBRun == 0 ? ",SLB在线数为0" : "")
                    + (BaseCount.BusSRun == 0 ? ",BusServer在线数为0" : "")
                    + " " + SystemStatuas);
            }
            else if (BaseCount.SLBRun < 3 || BaseCount.BusSRun < 2 || BaseCount.NoServerBus > 0 || SystemStatuasind == 2)
            {
                SystemStatuas1 = string.Format("</span><span style=\"color:#FFE500;font-size:24px;\">不健康{0}</span> ",
                  (BaseCount.SLBRun < 3 ? ",SLB在线数小于3" : "")
                    + (BaseCount.BusSRun < 2 ? ",BusServer在线数小于2" : "")
                     + (BaseCount.NoServerBus > 0 ? ",无承载服务" + BaseCount.NoServerBus.ToString() : "")
                        + " " + SystemStatuas);
            }
            SystemStatuas1 += string.Format("<p style=\"text-indent:2em;\"><span style=\"color:#000000;font-size:24px;\"> AccessServer状态:<span style=\"color:#{1};\"> {0} </span></p>"
                , AccessServerStatus, AccessServerStatus == "离线" ? "FF0000" : "009900");
            SystemStatuas1 += string.Format("<p style=\"text-indent:2em;\"><span style=\"color:#000000;font-size:24px;\"> SLB配置数量:<span style=\"color:#003399;\"> {0} </span>,在线数 <span style=\"color:#009900;\"> {1} </span></p>"
                , BaseCount.SLBCount, BaseCount.SLBRun);
            SystemStatuas1 += string.Format("<p style=\"text-indent:2em;\"><span style=\"color:#000000;font-size:24px;\"> BusServer配置数量:<span style=\"color:#003399;\"> {0} </span>,在线数 <span style=\"color:#009900;\"> {1} </span></p>"
             , BaseCount.BusSCount, BaseCount.BusSRun);
            SystemStatuas1 += string.Format("<p style=\"text-indent:2em;\"><span style=\"color:#000000;font-size:24px;\">业务配置数量:<span style=\"color:#003399;\"> {0} </span>,运行 <span style=\"color:#009900;\"> {1} <span style=\"color:#FF0000;\"> {2} </span</span></p>"
            , BaseCount.BusCount, BaseCount.BusRun, (BaseCount.NoServerBus > 0 ? " 无承载服务:" + BaseCount.NoServerBus.ToString() : ""));

            strHttp = string.Format(strHttp, DateTime.Now.ToString(), SystemStatuas1, strtBusInfoStatus2, strSLBtable3.ToString(), strBusStable4, strBusSbustable5);
            return strHttp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CpuLoads"></param>
        /// <param name="SystemStatuasind">消息种类：0 普通消息 蓝色；1安全消息 绿色；2 警告消息 黄色 ；3 错误消息 红色</param>
        /// <returns></returns>
        private string GetCpuColor(object CpuLoads, ref int SystemStatuasind)
        {
            decimal CpuLoad;
            string color = "";
            if (decimal.TryParse(CpuLoads.ToString(), out CpuLoad))
            {

                if (CpuLoad <= 70)
                {
                    color = HttpColor.Green;
                }
                else if (CpuLoad > 70 && CpuLoad <= 85)
                {
                    SystemStatuasind = SystemStatuasind < 2 ? 2 : SystemStatuasind;
                    color = HttpColor.Yellow;
                }
                else
                {
                    SystemStatuasind = SystemStatuasind < 2 ? 2 : SystemStatuasind;
                    color = HttpColor.Red;
                }
            }
            return color;
        }
        class HttpColor
        {
            public const string Green = "color:#009900;";//绿色
            public const string Red = "color:#FF0000;";//红色
            public const string Yellow = "color:#FFE500;";//黄色
            public const string Orange = "color:#FF9900;";//橙色

        }

        int HTTPServerAPI(string ApiID, string Param, out string ApiResult, out string APiMessage)
        {
            ApiResult = "";
            APiMessage = "";
            switch (ApiID)
            {
                case "GetSysBaseCount"://系统基本配置数量
                    {
                        ApiResult = JsonConvert.SerializeObject(GetSysBaseCount());
                        break;
                    }
                case "BusInfoStatus"://业务整体状态
                    {
                        DataTable dtBusInfoStatus = GetBusInfoStatusTable();
                        ApiResult = JsonConvert.SerializeObject(dtBusInfoStatus);
                        break;
                    }
                case "GetSysinfoLog":// 获取系统信息
                    {
                        ApiResult = JsonConvert.SerializeObject(GetSSysinfoLog());
                        break;
                    }

                #region  //--------------------业务信息------------------------------------------
                case "BusinessInfo"://业务信息
                    {
                        DataTable dtBusStatus = DbHelper.AllCodeInt("BusinessStatus");
                        DataTable dtBusType2 = DbHelper.AllCodeInt("BusType");
                        DataTable dt = DbHelper.BusinessInfoTableGet("projectID='" + projectID + "' ");
                        DataColumn[] FJColumns = new DataColumn[1];
                        FJColumns[0] = dt.Columns["busType"];
                        DataColumn[] SJColumns = new DataColumn[1];
                        SJColumns[0] = dtBusType2.Columns["Code_ID"];
                        DataTable dtBusinessInfo = DataTableHelper.LeftOuterJoin(dt, dtBusType2, FJColumns, SJColumns);

                        FJColumns[0] = dt.Columns["status"];
                        SJColumns[0] = dtBusStatus.Columns["Code_ID"];
                        dtBusinessInfo = DataTableHelper.LeftOuterJoin(dtBusinessInfo, dtBusStatus, FJColumns, SJColumns);

                        ApiResult = JsonConvert.SerializeObject(dtBusinessInfo);
                        break;
                    }
                case "BusinessInfo.ALL"://业务整体数据
                    {
                        SBusinessInfoALL businessInfoALL = new SBusinessInfoALL();
                        businessInfoALL.BusDDLBS = DbHelper.AllCodeInt("BusDDLBS");
                        businessInfoALL.BusLogType = DbHelper.AllCodeInt("BusLogType");

                        DataTable dtBusStatus = DbHelper.AllCodeInt("BusinessStatus");
                        businessInfoALL.BusinessStatus = dtBusStatus;
                        DataTable dtBusType2 = DbHelper.AllCodeInt("BusType");
                        businessInfoALL.BusType = dtBusType2;
                        DataTable dt = DbHelper.BusinessInfoTableGet("projectID='" + projectID + "' ");
                        DataColumn[] FJColumns = new DataColumn[1];
                        FJColumns[0] = dt.Columns["busType"];
                        DataColumn[] SJColumns = new DataColumn[1];
                        SJColumns[0] = dtBusType2.Columns["Code_ID"];
                        DataTable dtBusinessInfo = DataTableHelper.LeftOuterJoin(dt, dtBusType2, FJColumns, SJColumns);

                        FJColumns[0] = dt.Columns["status"];
                        SJColumns[0] = dtBusStatus.Columns["Code_ID"];
                        dtBusinessInfo = DataTableHelper.LeftOuterJoin(dtBusinessInfo, dtBusStatus, FJColumns, SJColumns);
                        businessInfoALL.BusinessInfo = dtBusinessInfo;
                        businessInfoALL.BusVStatus = MyPubConstant.EnumToDataTable(typeof(BusinessInfoVersionStatus), "StatusName", "StatusType");
                        ApiResult = JsonConvert.SerializeObject(businessInfoALL);
                        break;
                    }
                case "BusnessinfoVersion"://业务对应的版本
                    {
                        BusinessinfoVersion dalbusinessinfoVersion = new BusinessinfoVersion();
                        DataTable dtBusinessInfoVersion = dalbusinessinfoVersion.GetList("BusID='" + Param + "'");
                        DataColumn[] FJColumns = new DataColumn[1];
                        DataColumn[] SJColumns = new DataColumn[1];
                        DataTable dtBusVStatus = MyPubConstant.EnumToDataTable(typeof(BusinessInfoVersionStatus), "StatusName", "StatusType");
                        FJColumns[0] = dtBusinessInfoVersion.Columns["status"];
                        SJColumns[0] = dtBusVStatus.Columns["StatusType"];

                        dtBusinessInfoVersion = DataTableHelper.LeftOuterJoin(dtBusinessInfoVersion, dtBusVStatus, FJColumns, SJColumns);
                        dtBusinessInfoVersion.Columns.Add("isNew", typeof(Boolean));
                        dtBusinessInfoVersion.Columns.Add("FileIDBack", typeof(string));
                        dtBusinessInfoVersion.Columns.Add("StatusBack", typeof(int));
                        dtBusinessInfoVersion.Columns.Add("VersionNBack", typeof(decimal));
                        ApiResult = JsonConvert.SerializeObject(dtBusinessInfoVersion);
                        break;
                    }
                case "BusFileInfo"://获取直接提供业务服务的DLL
                    {
                        DataTable dtbusDllinfo = new Fileinfo().GetDirect("default", Param);

                        ApiResult = JsonConvert.SerializeObject(dtbusDllinfo);
                        break;
                    }
                case "DependentFile"://获取Bus对应依赖
                    {
                        SGetDependentFile sGetDependentFile = JsonConvert.DeserializeObject<SGetDependentFile>(Param);
                        DataTable dtDependentFile = DbHelper.DependentFileGet(sGetDependentFile.BusID, sGetDependentFile.VersionN);

                        dtDependentFile.Columns.Add("isNew", typeof(Boolean));
                        foreach (DataRow dr in dtDependentFile.Rows)
                        {
                            dr["isNew"] = false;
                        }
                        ApiResult = JsonConvert.SerializeObject(dtDependentFile);
                        break;
                    }
                case "SaveBusinessInfo"://保存BusinessInfo
                    {
                        //SBusinessInfoSave sBusinessInfoSaveT = new SBusinessInfoSave();
                        //sBusinessInfoSaveT.businessInfo = DbHelper.BusinessInfoGet("AdditionTest");
                        //sBusinessInfoSaveT.busStatusNew = (sBusinessInfoSaveT.busStatusNew==BusStatus.Publish? BusStatus.Offline: BusStatus.Publish);
                        //Param = JsonConvert.SerializeObject(sBusinessInfoSaveT);

                        SBusinessInfoSave sBusinessInfoSave = JsonConvert.DeserializeObject<SBusinessInfoSave>(Param);
                        return SaveBusinessInfo(sBusinessInfoSave.businessInfo, sBusinessInfoSave.busStatusNew, out ApiResult, out APiMessage);
                        break;
                    }
                case "SaveBusnessVersion"://保存业务版本对应DLL
                    {
                        //BusinessinfoVersion dalbusinessinfoVersion = new BusinessinfoVersion();
                        //DataTable dtBusinessInfoVersion = dalbusinessinfoVersion.GetList("BusID='AdditionTest'");
                        //SBusnessVersionSave sBusnessVersionSaveT = new SBusnessVersionSave();
                        //sBusnessVersionSaveT.busversion = dalbusinessinfoVersion.DataRowToModel(dtBusinessInfoVersion.Rows[0]);
                        //sBusnessVersionSaveT.VersionOld = "1.0.0.0";
                        //Param = JsonConvert.SerializeObject(sBusnessVersionSaveT);

                        SBusnessVersionSave sBusnessVersionSave = JsonConvert.DeserializeObject<SBusnessVersionSave>(Param);
                        return SaveBusnessVersion(sBusnessVersionSave.busversion, sBusnessVersionSave.VersionOld, out ApiResult, out APiMessage);
                        break;
                    }
                case "SaveDependentFile"://保存业务版本依赖文件
                    {
                        //SDependentFile sDependentFileT = new SDependentFile();
                        //Param = JsonConvert.SerializeObject(sDependentFileT);

                        SDependentFile sDependentFile = JsonConvert.DeserializeObject<SDependentFile>(Param);
                        return SaveDependentFile(sDependentFile, out ApiResult, out APiMessage);
                        break;
                    }
                #endregion    //--------------------业务信息------------------------------------------

                #region  //--------------------业务服务器信息------------------------------------------
                case "BusServer.ALL"://业务服务器整体数据
                    {
                        DataTable dtBusServerStatus = DbHelper.AllCodeInt("BusServerStatus");
                        DataTable dtBusInfoStatus = DbHelper.AllCodeInt("BusServerBusStatus");
                        DataTable dtBusServerSEType = DbHelper.AllCodeInt("SEType");
                        DataTable dtBusServer = DbHelper.BusServerTableGet("Status >=0");
                        dtBusServer.Columns.Add("isNew", typeof(bool));
                        foreach (DataRow dr in dtBusServer.Rows)
                        {
                            dr["isNew"] = false;
                        }
                        SBusServerALL sBusServerALL = new SBusServerALL();
                        sBusServerALL.BusServer = dtBusServer;
                        sBusServerALL.BusServerStatus = dtBusServerStatus;
                        sBusServerALL.BusInfoStatus = dtBusInfoStatus;
                        sBusServerALL.dtBusServerSEType = dtBusServerSEType;
                        ApiResult = JsonConvert.SerializeObject(sBusServerALL);
                        break;
                    }
                case "SaveBusServer"://保存业务服务器
                    {
                        SSaveBusServer SsaveBusServer = JsonConvert.DeserializeObject<SSaveBusServer>(Param);

                        return SaveBusServer(SsaveBusServer.busServerInfo, SsaveBusServer.BusServerStatus, out ApiResult, out APiMessage);
                        break;
                    }
                case "BusServerBusInfo"://业务服务器对应业务
                    {
                        string BusServerID = Param;
                        DataTable dtBusServerBusInfo = DbHelper.BusServerBusInfoGet(BusServerID);
                        ApiResult = JsonConvert.SerializeObject(dtBusServerBusInfo);
                        break;
                    }
                case "SaveBusServerBusInfo"://业务服务器对应业务保存
                    {
                        //SBusServerBusInfo sBusServerBusInfoT = new SBusServerBusInfo();
                        //sBusServerBusInfoT.BusServerID = "BusST01";
                        //sBusServerBusInfoT.BusID = "AdditionTest";
                        //sBusServerBusInfoT.Maxconcurrent = 0;
                        //sBusServerBusInfoT.Status = 1;
                        //Param=  JsonConvert.SerializeObject(sBusServerBusInfoT);


                        SBusServerBusInfo sBusServerBusInfo = JsonConvert.DeserializeObject<SBusServerBusInfo>(Param);
                        return SaveBusServerBusInfo(sBusServerBusInfo, out ApiResult, out APiMessage);
                        break;
                    }
                case "DelBusServerBusInfo"://业务服务器对应业务删除
                    {
                        SBusServerBusInfo sBusServerBusInfo = JsonConvert.DeserializeObject<SBusServerBusInfo>(Param);
                        return DelBusServerBusInfo(sBusServerBusInfo, out ApiResult, out APiMessage);
                        break;
                    }
                case "BusServerBusHTTP"://业务服务器允许执行的http业务
                    {
                        string BusServerID = Param;
                        DataTable dtBusServerBusHTTP = DbHelper.BusServerBusHTTPGet(BusServerID);
                        ApiResult = JsonConvert.SerializeObject(dtBusServerBusHTTP);
                        break;
                    }
                case "SaveBusServerBusHTTP"://业务服务器允许执行的http业务保存或者删除
                    {
                        //SBusServerBusInfohttp sBusServerBusInfohttpT = new SBusServerBusInfohttp();
                        //sBusServerBusInfohttpT.BusID = "PBusGetDocInfoHelper";
                        //sBusServerBusInfohttpT.BusServerID = "BusS19";
                        //sBusServerBusInfohttpT.Status = -1;
                        // Param = JsonConvert.SerializeObject(sBusServerBusInfohttpT);

                        SBusServerBusInfohttp sBusServerBusInfohttp = JsonConvert.DeserializeObject<SBusServerBusInfohttp>(Param);
                        return SaveBusServerBusHTTP(sBusServerBusInfohttp, out ApiResult, out APiMessage);
                        break;
                    }
                #endregion
                case "SLBInfo.ALL"://SLB服务器整体数据
                    {
                        DataTable dtBusServerStatus = DbHelper.AllCodeInt("BusServerStatus");

                        DataTable dtSLBinfo = DbHelper.SLBInfoTableGet("Status >=0");
                        SSLBInfoALL sSLBInfoALL = new SSLBInfoALL();
                        sSLBInfoALL.SLBStatus = dtBusServerStatus;
                        sSLBInfoALL.SLBInfo = dtSLBinfo;
                        ApiResult = JsonConvert.SerializeObject(sSLBInfoALL);
                        break;
                    }

                case "SysStatus.ALL"://系统状态
                    {
                        SysStatusALL sysStatusALL = new SysStatusALL();
                        List<SSLBRateandStatus> listSSLBRateandStatus = GetSLBRates();// 获取<see cref="ServerLoadBalancing"/>整体情况
                        List<SSLBBusRateStatus> listSSLBBusRateStatuss = GetSLBBusRates();//获取<see cref="ServerLoadBalancing"/>中的业务情况
                        List<SBusServerRateandStatus> listSLBBusServer = GetBusServerRates();// 获取<see cref="SLBBusServer"/>整体情况
                        List<SBusServerBusRatesandStatus> listSBusServerBusRatesandStatus = GetBusServerBusRates();//获取<see cref="SLBBusServer"/>中的业务情况

                        sysStatusALL.listSSLBRateandStatus = listSSLBRateandStatus;
                        sysStatusALL.listSSLBBusRateStatuss = listSSLBBusRateStatuss;
                        sysStatusALL.listSLBBusServer = listSLBBusServer;
                        sysStatusALL.listSBusServerBusRatesandStatus = listSBusServerBusRatesandStatus;
                        ApiResult = JsonConvert.SerializeObject(sysStatusALL);
                        break;
                    }
                default:
                    {
                        ApiResult = "";
                        APiMessage = "无此业务函数:" + ApiID;
                        break;
                    }
            }
            return 1;
        }

        int SaveBusinessInfo(BusinessInfo busCurrent, BusStatus busStatusNew, out string ApiResult, out string APiMessage)
        {
            ApiResult = ""; APiMessage = "";
            bool NeedinfoBusServerOrVersion = false;//是否需要通知服务器更新版本
            bool NeedinfoServerBusInfo = false;//是否需要通知服务器更新业务配置
            bool AutoPublishBusServer = false;//是否需要通知BusServer服务器自动启动业务

            if (busCurrent.busDDLBS == BusDDLBS.LimitMaxConcurrentRandomRule)
            {
            }
            else
            {
                busCurrent.Maxconcurrent = 0;
            }

            if (busCurrent.busType == BusType.CacheData)
            {
                if (busCurrent.TTL == 0)
                {
                    busCurrent.TTL = 300;
                }
            }
            else
            {
                busCurrent.TTL = 0;
            }
            BusinessinfoVersion dalbusinessinfoVersion = new BusinessinfoVersion();
            if (dalbusinessinfoVersion.Exists(busCurrent.BusID, DbHelper.VersionTodecimal(busCurrent.version)))
            {
                businessinfoversion busversion = dalbusinessinfoVersion.GetModel(busCurrent.BusID, DbHelper.VersionTodecimal(busCurrent.version));
                BusinessInfoVersionStatus busStatusV = (BusinessInfoVersionStatus)busversion.Status;
                if (busStatusNew == BusStatus.Publish)
                {//在使用的业务只允许更新测试后的
                    if (busStatusV != BusinessInfoVersionStatus.Publish && busStatusV != BusinessInfoVersionStatus.Releasable)
                    {
                        APiMessage = "在用的业务只允许指定为状态是Publish、Releasable的版本!";
                        return -1;
                    }
                }
            }
            BusStatus busStatusOld = BusStatus.Offline;
            BusinessInfo busInfoOld = new BusinessInfo();
            DataTable dtOld = DbHelper.BusinessInfoTableGet(string.Format("BusID ='{0}'", busCurrent.BusID));
            if (dtOld != null && dtOld.Rows.Count > 0)
            {
                busStatusOld = (BusStatus)int.Parse(dtOld.Rows[0]["Status"].ToString());
                busInfoOld = DbHelper.BusinessInfoGet(busCurrent.BusID);
            }

            if (busInfoOld.BMODID == busCurrent.BMODID
                && busInfoOld.busDDLBS == busCurrent.busDDLBS
                 && busInfoOld.BusName == busCurrent.BusName
                  && busInfoOld.busType == busCurrent.busType
                   && busInfoOld.fileID == busCurrent.fileID
                   && busInfoOld.NamespaceClass == busCurrent.NamespaceClass
                   && busInfoOld.Maxconcurrent == busCurrent.Maxconcurrent
                     && busInfoOld.TTL == busCurrent.TTL
                   && busStatusOld == busStatusNew
                    && busInfoOld.LogType == busCurrent.LogType
                       && busInfoOld.BLogSeparate == busCurrent.BLogSeparate
                          && busInfoOld.RecordSlowExecute == busCurrent.RecordSlowExecute
                )
            {
                APiMessage = "设置未改变，无需保存!";
                return -2;

            }


            if ((busInfoOld == null || busInfoOld.BLogSeparate != busCurrent.BLogSeparate) && busCurrent.BLogSeparate == true)
            {
                string strSQL = string.Format(@" CREATE TABLE `log{0}` (
  `BusID` varchar(30) NOT NULL,
  `TID` varchar(40) NOT NULL DEFAULT '',
  `ExecuteTime` int(11) NOT NULL DEFAULT '0',
  `InTime` datetime NOT NULL,
  `OutTime` datetime NOT NULL,
`ResultStatus` int(2) NOT NULL DEFAULT '1',
  `ClientID` varchar(30) NOT NULL DEFAULT '',
  `SLBID` varchar(25) NOT NULL,
  `BusServerID` varchar(20) NOT NULL,
  `InData` longtext NOT NULL,
  `OutData` longtext NOT NULL,
  `Version` varchar(20) NOT NULL,
  `VersionStatus` int(2) NOT NULL DEFAULT '1',
  `Header` varchar(200) NOT NULL,
  KEY `indexlog{0}` (`InTime`,`ExecuteTime`,`TID`,`ResultStatus`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8;", busCurrent.BusID);
                DbHelper.CreateLogTable(strSQL);
            }

            if (DbHelper.BusinessInfoSet(busCurrent, (int)(busStatusNew)))
            {
                if (busInfoOld != null)
                {
                    if (busInfoOld.busType != busCurrent.busType
                          || busInfoOld.fileID != busCurrent.fileID
                          || busInfoOld.NamespaceClass != busCurrent.NamespaceClass
                           || busInfoOld.LogType != busCurrent.LogType
                            || busInfoOld.BLogSeparate != busCurrent.BLogSeparate
                             || busInfoOld.RecordSlowExecute != busCurrent.RecordSlowExecute
                       )
                    {
                        if (!(busStatusOld == BusStatus.Offline && busStatusNew == BusStatus.Offline))
                        {
                            NeedinfoBusServerOrVersion = true;
                        }
                    }
                    if (busInfoOld.busDDLBS != busCurrent.busDDLBS
                         || busInfoOld.busType != busCurrent.busType
                          || busInfoOld.Maxconcurrent != busCurrent.Maxconcurrent
                          || busInfoOld.TTL != busCurrent.TTL)
                    {
                        if (!(busStatusOld == BusStatus.Offline && busStatusNew == BusStatus.Offline))
                        {
                            NeedinfoServerBusInfo = true;
                        }
                    }
                    if (busStatusOld != busStatusNew)
                    {
                        if (busStatusOld == BusStatus.Offline || busStatusNew == BusStatus.Offline)
                        {
                            NeedinfoBusServerOrVersion = true;
                        }
                        NeedinfoServerBusInfo = true;
                        if (busStatusOld == BusStatus.Offline && busStatusNew != BusStatus.Offline)
                        {

                            AutoPublishBusServer = true;
                            NeedinfoBusServerOrVersion = false;

                        }
                        if (busStatusOld != BusStatus.Offline && busStatusNew == BusStatus.Offline)
                        {

                            NeedinfoServerBusInfo = true;
                            NeedinfoBusServerOrVersion = false;

                        }
                    }
                    if (busStatusOld != BusStatus.Publish && busStatusNew == BusStatus.Publish)
                    {//新发布，默认各个BusServer都发布

                    }
                }

                if (!dalbusinessinfoVersion.Exists(busCurrent.BusID, DbHelper.VersionTodecimal(busCurrent.version)))
                {
                    businessinfoversion busversion = new businessinfoversion(busCurrent);
                    busversion.Status = (int)busStatusNew;
                    dalbusinessinfoVersion.Add(busversion);
                }
            }

            if (NeedinfoServerBusInfo && GOMConnect != null)
            {
                GOMConnect.UpdateBusBusinessInfo(busCurrent, busStatusNew);
                if (AutoPublishBusServer)
                {
                    GOMConnect.AddAllSingleBusBusiness(busCurrent.BusID);
                }
            }
            if (NeedinfoBusServerOrVersion && GOMConnect != null)
            {
                GOMConnect.UpdateBusServerBusinessOrVersion(busCurrent.BusID);
            }
            ApiResult = "success";
            return 1;
        }

        int SaveBusnessVersion(businessinfoversion busversion, string VersionOld, out string ApiResult, out string APiMessage)
        {
            ApiResult = ""; APiMessage = "";
            busversion.VersionN = DbHelper.VersionTodecimal(busversion.Version);
            BusinessinfoVersion dalbusinessinfoVersion = new BusinessinfoVersion();
            businessinfoversion busversionOld = dalbusinessinfoVersion.GetModel(busversion.BusID, DbHelper.VersionTodecimal(VersionOld));
            if (busversionOld != null)
            {
                if (busversion.BusID == busversionOld.BusID
                  && busversion.FilePath == busversionOld.FilePath
                  && busversion.DllName == busversionOld.DllName
                  && busversion.Status == busversionOld.Status
                  && busversion.Version == busversionOld.Version
               )
                {
                    APiMessage = "未检测到变化，无需保存!";
                    return -1;
                }
                try
                {
                    dalbusinessinfoVersion.Update(busversion, DbHelper.VersionTodecimal(VersionOld));
                    if (busversion.VersionN != DbHelper.VersionTodecimal(VersionOld))
                    {
                        DbHelper.DependentFileDelete(busversion.BusID, DbHelper.VersionTodecimal(VersionOld));
                    }
                }
                catch
                {
                    APiMessage = "更新后台失败，可能已经存在该版本!";
                    return -2;
                }
            }
            else
            {
                dalbusinessinfoVersion.Add(busversion);
            }
            if (GOMConnect != null)
            {
                GOMConnect.AddOrUpdateBusinessTestVersion(busversion.BusID, busversion.BusVersion);
            }
            ApiResult = "success";
            return 1;
        }
        int SaveDependentFile(SDependentFile sDependentFile, out string ApiResult, out string APiMessage)
        {
            ApiResult = ""; APiMessage = "";
            if (DbHelper.DependentFileInsert(sDependentFile.BusID, sDependentFile.VersionN, sDependentFile.projectID, sDependentFile.FilePath, sDependentFile.Filename, sDependentFile.version))
            {
                ApiResult = "success";
                return 1;
            }
            else
            {
                APiMessage = "保存失败";
                return -1;
            }
        }

        int SaveBusServer(BusServerInfo busServerInfo, int BusServerStatus, out string ApiResult, out string APiMessage)
        {
            ApiResult = ""; APiMessage = "";
            //是否需要通知服务器更新并更新  
            bool NeedinfoServer = false;
            bool AddOrUpdateBusServer = false;
            bool StatusChange = false;
            DataTable dtold = DbHelper.BusServerTableGet("ID ='" + busServerInfo.BusServerID + "'");
            bool isNew = (dtold == null || dtold.Rows.Count == 0);
            if (!isNew)
            {
                BusServerInfo busSerOld = DbHelper.BusServerGet(busServerInfo.BusServerID);
                if (busServerInfo.Weight == busSerOld.Weight
                    && busServerInfo.BusServerName == busSerOld.BusServerName
                    && busServerInfo.UseHttp == busSerOld.UseHttp
                     && busServerInfo.HttpPort == busSerOld.HttpPort
                     && BusServerStatus == int.Parse(dtold.Rows[0]["Status"].ToString())
                      && busServerInfo.EncryptType == busSerOld.EncryptType
                       && busServerInfo.SignType == busSerOld.SignType
                         && busServerInfo.PassGAP == busSerOld.PassGAP
                           && busServerInfo.BusLogAPISave == busSerOld.BusLogAPISave
                        )
                {//没有变化
                    APiMessage = "没有变化";
                    return -1;
                }
                if (busServerInfo.Weight != busSerOld.Weight)
                {
                    AddOrUpdateBusServer = true;
                    NeedinfoServer = true;
                    StatusChange = true;
                }
                if (BusServerStatus != int.Parse(dtold.Rows[0]["Status"].ToString()))
                {
                    NeedinfoServer = true; StatusChange = true;
                }
                if (busServerInfo.UseHttp != busSerOld.UseHttp || busServerInfo.UseHttp && busServerInfo.HttpPort != busSerOld.HttpPort)
                {
                    AddOrUpdateBusServer = true; StatusChange = true;
                }
            }
            if (DbHelper.BusServerSet(busServerInfo))
            {

            }
            else
            {
                APiMessage = "保存BusServer失败";
                return -2;
            }
            if (StatusChange && BusServerStatus == 0)
            {
                DbHelper.BusServerSetStatus(busServerInfo.BusServerID, 0);
            }
            if (NeedinfoServer)
            {//通知服务器
                if (GOMConnect != null)
                {
                    if (AddOrUpdateBusServer)
                        GOMConnect.AddOrUpdateBusServer(busServerInfo);
                    if (StatusChange)
                    {
                        if (BusServerStatus == 1)
                        {
                            GOMConnect.StartReceivingService(busServerInfo.BusServerID);
                        }
                        else if (BusServerStatus == 2)
                        {
                            GOMConnect.StopReceivingService(busServerInfo.BusServerID);
                        }
                        else if (BusServerStatus == 0)
                        {
                            bool reStart = false;
                            // if (MessageBox.Show("是否自动重启BusServer程序？", "提示!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                reStart = true;
                            }
                            GOMConnect.RemoveBusServer(busServerInfo.BusServerID, false, reStart);
                        }
                    }
                }
            }
            return 1;
        }
        int SaveBusServerBusInfo(SBusServerBusInfo sSaveBBSbi, out string ApiResult, out string APiMessage)
        {
            ApiResult = ""; APiMessage = "";
            SBusServerBusInfo sSaveBBSbOld = DbHelper.SBusServerBusInfoGet(sSaveBBSbi.BusServerID, sSaveBBSbi.BusID);
            bool isNew = sSaveBBSbOld == null;
            if (isNew || sSaveBBSbi.Status != sSaveBBSbOld.Status
                        || sSaveBBSbi.Maxconcurrent != sSaveBBSbOld.Maxconcurrent)
            {
                DbHelper.BusServerBusInfoSet(sSaveBBSbi.BusServerID, sSaveBBSbi.BusID, sSaveBBSbi.Status, sSaveBBSbi.Maxconcurrent);

                if (isNew && sSaveBBSbi.Status == 1
                    || sSaveBBSbi.Status == 1)
                {//新增并且是运行的 或者是旧的但是现在运行的 发给服务
                    SingleBusServerInfo singleBusServerInfo = DbHelper.BusServerBusInfoSingleByServerGet(sSaveBBSbi.BusServerID, sSaveBBSbi.BusID);
                    if (singleBusServerInfo != null)
                    {
                        if (GOMConnect != null)
                            GOMConnect.AddOrUpdateSingleBusBusiness(singleBusServerInfo);
                    }
                }
                else if (!isNew && sSaveBBSbi.Status != 1)
                {//停止服务
                    SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
                    singleBusServerInfo.BusServerID = sSaveBBSbi.BusServerID;
                    singleBusServerInfo.BusID = sSaveBBSbi.BusID;
                    if (singleBusServerInfo != null)
                    {
                        if (GOMConnect != null)
                            GOMConnect.RemoveSingleBusBusiness(singleBusServerInfo);
                    }
                }
            }
            else
            {
                APiMessage = "没有变化";
                return -1;
            }
            ApiResult = "success";
            return 1;
        }
        int DelBusServerBusInfo(SBusServerBusInfo sSaveBBSbi, out string ApiResult, out string APiMessage)
        {
            ApiResult = ""; APiMessage = "";
            if (DbHelper.BusServerBusInfoDel(sSaveBBSbi.BusServerID, sSaveBBSbi.BusID))
            {
                //停止服务
                SingleBusServerInfo singleBusServerInfo = new SingleBusServerInfo();
                singleBusServerInfo.BusServerID = sSaveBBSbi.BusServerID;
                singleBusServerInfo.BusID = sSaveBBSbi.BusID;
                if (GOMConnect != null)
                    GOMConnect.RemoveSingleBusBusiness(singleBusServerInfo);
            }
            else
            {
                APiMessage = "保存失败";
                return -1;
            }
            ApiResult = "success";
            return 1;
        }
        int SaveBusServerBusHTTP(SBusServerBusInfohttp BusHTTP, out string ApiResult, out string APiMessage)
        {
            List<string> newHttpBus = new List<string>();
            DbHelper.BusServerBusHTTPSet(BusHTTP.BusServerID, BusHTTP.BusID, BusHTTP.Status == 1);
            newHttpBus.Add(BusHTTP.BusID);
            if (newHttpBus.Count > 0)
            {
                UpSBusServerHttpBus upSBusServerHttpBus = new UpSBusServerHttpBus();
                upSBusServerHttpBus.BusServerID = BusHTTP.BusServerID;
                upSBusServerHttpBus.Enable = BusHTTP.Status == 1;
                upSBusServerHttpBus.BusIDS = newHttpBus;
                if (GOMConnect != null)
                    GOMConnect.UpdateSBusServerHttpBus(upSBusServerHttpBus);
            }
            ApiResult = ""; APiMessage = "";
            ApiResult = "success";
            return 1;
        }





        /// <summary>
        /// 更机构具有的系统权限
        /// </summary>
        /// <param name="sApiUserandGroup"></param>
        /// <param name="ApiResult"></param>
        /// <param name="APiMessage"></param>
        /// <returns></returns>
        public int SorgandSysUpdate(SSorgandSysUpdate sSorgandSys, out string ApiResult, out string APiMessage)
        {
            ApiResult = ""; APiMessage = "";

            if (string.IsNullOrEmpty(sSorgandSys.ORG_ID))
            {
                APiMessage = "参数ORG_ID不能为空!";
                return -1;
            }
            DataTable dtOld = DbHelper.SorgandSysTbGet(sSorgandSys.ORG_ID);
            if (dtOld != null && dtOld.Rows.Count == 0 && sSorgandSys.listSysID != null && sSorgandSys.listSysID.Count == 0)
            {
                APiMessage = "无变化,均未授权!";
                return -1;
            }
            if (dtOld != null && dtOld.Rows.Count > 0 || sSorgandSys.listSysID != null && sSorgandSys.listSysID.Count > 0)
            {
                List<int> addGroup = new List<int>();
                List<int> delGroup = new List<int>();
                foreach (DataRow dr in dtOld.Rows)
                {
                    if (!sSorgandSys.listSysID.Contains((int)dr["SYSID"]))
                    {
                        delGroup.Add((int)dr["SYSID"]);
                    }
                }
                foreach (int APIG_ID in sSorgandSys.listSysID)
                {
                    if (dtOld.Select("SYSID = " + APIG_ID.ToString()).Length == 0)
                    {
                        addGroup.Add(APIG_ID);
                    }
                }
                if (addGroup.Count == 0 && delGroup.Count == 0)
                {
                    APiMessage = "无变化,无需保存!";
                    return -1;
                }
                DbHelper.SorgandSysUpdate(sSorgandSys.ORG_ID, addGroup, delGroup, sSorgandSys.User_id);
            }
            ApiResult = ""; APiMessage = "";
            ApiResult = "success";
            return 1;
        }




        /// <summary>
        /// 更API用户组所含API
        /// </summary>
        public class SApiGroupandAPIUpdate
        {
            public int APIG_ID;
            public List<int> listAPI_ID;
            /// <summary>
            /// 当前操作员ID
            /// </summary>
            public string User_id;
        }
        /// <summary>
        /// 更API用户组所含用户 
        /// </summary>
        public class SApiGroupandUserUpdate
        {
            public int APIG_ID;
            public List<string> listAPIU_ID;
            /// <summary>
            /// 当前操作员ID
            /// </summary>
            public string User_id;
        }
        /// <summary>
        /// 更机构所具有的系统 
        /// </summary>
        public class SSorgandSysUpdate
        {
            public string ORG_ID;
            public List<int> listSysID;
            /// <summary>
            /// 当前操作员ID
            /// </summary>
            public string User_id;
        }
        /// <summary>
        /// 更API 所属用户组
        /// </summary>
        public class SAPIandGroupUpdate
        {
            public int API_ID;
            public List<int> listGroup;
            /// <summary>
            /// 当前操作员ID
            /// </summary>
            public string User_id;
        }

        /// <summary>
        /// 更API新用户所属用户组
        /// </summary>
        public class SApiUserandGroupUpdate
        {
            public string APIU_ID;
            public List<int> listGroup;
            /// <summary>
            /// 当前操作员ID
            /// </summary>
            public string User_id;
        }
        public class SysStatusALL
        {
            /// <summary>
            ///   <see cref="ServerLoadBalancing"/>整体情况
            /// </summary>
            public List<SSLBRateandStatus> listSSLBRateandStatus;
            /// <summary>
            /// // <see cref="ServerLoadBalancing"/>中的业务情况
            /// </summary>
            public List<SSLBBusRateStatus> listSSLBBusRateStatuss;
            /// <summary>
            /// / <see cref="SLBBusServer"/>整体情况
            /// </summary>
            public List<SBusServerRateandStatus> listSLBBusServer;
            /// <summary>
            ///  <see cref="SLBBusServer"/>中的业务情况
            /// </summary>
            public List<SBusServerBusRatesandStatus> listSBusServerBusRatesandStatus;


        }
        public class SSLBInfoALL
        {
            /// <summary>
            /// SLB数据
            /// </summary>
            public DataTable SLBInfo;
            /// <summary>
            /// SLB状态
            /// </summary>
            public DataTable SLBStatus;

        }

        public class SBusServerBusInfohttp
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
            /// 1 增加 -1删除
            /// </summary>
            public int Status;

        }
        public class SSaveBusServer
        {
            public int BusServerStatus;
            public BusServerInfo busServerInfo;
        }

        public class SBusServerALL
        {
            /// <summary>
            /// BusServer数据
            /// </summary>
            public DataTable BusServer;
            /// <summary>
            /// BusServer状态
            /// </summary>
            public DataTable BusServerStatus;
            /// <summary>
            ///  Bus业务状态
            /// </summary>
            public DataTable BusInfoStatus;
            /// <summary>
            /// 加密类型
            /// </summary>
            public DataTable dtBusServerSEType;


        }

        public class SGetDependentFile
        {
            public string BusID;
            public decimal VersionN;
        }
        public class SDependentFile
        {/// <summary>
         /// 业务ID
         /// </summary>
            public string BusID;
            /// <summary>
            /// 业务ID对应版本号数值
            /// </summary>
            public decimal VersionN;
            /// <summary>
            /// 依赖文件的projectID
            /// </summary>
            public string projectID;
            /// <summary>
            /// 依赖文件的路径
            /// </summary>
            public string FilePath;
            /// <summary>
            /// 依赖文件的名称
            /// </summary>
            public string Filename;
            /// <summary>
            /// 依赖文件的版本号
            /// </summary>
            public string version;
        }
        public class SBusnessVersionSave
        {
            public businessinfoversion busversion;
            public string VersionOld;
        }
        public class SBusinessInfoSave
        {
            public BusinessInfo businessInfo;
            public BusStatus busStatusNew;
        }

        public class SBusinessInfoALL
        {
            /// <summary>
            /// 业务类别
            /// </summary>
            public DataTable BusType;
            /// <summary>
            /// 业务状态
            /// </summary>
            public DataTable BusinessStatus;

            /// <summary>
            /// 负载策略 
            /// </summary>
            public DataTable BusDDLBS;

            /// <summary>
            /// 日志类别 
            /// </summary>
            public DataTable BusLogType;

            /// <summary>
            /// 业务版本状态
            /// </summary>
            public DataTable BusVStatus;
            public DataTable BusinessInfo;
        }



        #endregion HTTPServerAPI

        #region HttpListener


        Encoding Encoding = Encoding.UTF8;
        /// <summary>
        /// 调用服务器的EventWaitHandle队列
        /// </summary>
        ConcurrentDictionary<string, EventWaitHandle> sdhttpCallServerBackwh = new ConcurrentDictionary<string, EventWaitHandle>();
        /// <summary>
        /// 调用服务器返回的数据队列
        /// </summary>
        ConcurrentDictionary<string, ResultStore> sdhttpCallServerBack = new ConcurrentDictionary<string, ResultStore>();
        private HttpListener httplistener;   //其他超做请求监听

        /// <summary>  
        /// HTTP服务器是否在运行  
        /// </summary>  
        public bool HttpIsRunning
        {
            get { return (httplistener == null) ? false : httplistener.IsListening; }
        }
        List<string> HttpIPList = new List<string>();
        public bool HttpIPAdd(string IP)
        {
            if (!HttpIPList.Contains(IP))
            {
                HttpIPList.Add(IP);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Httpport { get; set; }

        /// <summary>  
        /// 启动服务  
        /// </summary>  
        public void HttpStart()
        {
            Httpport = MyPubConstant.HttpPort;
            if (httplistener != null && httplistener.IsListening)
                return;
            if (httplistener == null)
                httplistener = new HttpListener();
            if (HttpIPList.Count == 0)//如果事先没有指定IP则添加所有网卡IP
            {
                try
                {
                    NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface NetworkIntf in interfaces)
                    {
                        IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                        UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                        foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                        {
                            if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                HttpIPAdd(UnicastIPAddressInformation.Address.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (HttpIPList.Count > 0)
            {
                //   listener.Prefixes.Add("http://" + Host + ":" + this.port + "/");
                //AddPrefixes("/BehaviorApi/EmailSend/");
                AddPrefixes("/myspringClear/");//未加密
                AddPrefixes("/myspringAES/");//加密
                AddPrefixes("/Helper/");//帮助文档
            }

            // httplistener.Start();
            Thread listenThread = new Thread(GethttRequest);
            listenThread.Name = "httpserver";
            listenThread.Start();
            Console.WriteLine(string.Format("HttpServer start[Port:{0}] ...  ", Httpport));
            PasSLog.Info("HttpStart", string.Format("HttpServer start[Port:{0}] ...  ", Httpport));
        }


        /// <summary>  
        /// 停止服务  
        /// </summary>  
        public void HTTPStop()
        {
            try
            {
                if (httplistener != null)
                {
                    //listener.Close();
                    //listener.Abort();
                    httplistener.Stop();
                    Console.WriteLine(string.Format("HttpServer Stop  ", Httpport));
                    PasSLog.Info("HTTPStop", string.Format("HttpServer Stop  ", Httpport));
                }
            }
            catch (Exception ex)
            {

            }
        }
        string StartURL = "/OMS";
        public bool AddPrefixes(string uriPrefix)
        {
            foreach (string IP in HttpIPList)
            {
                string uri = "http://" + IP + ":" + this.Httpport + StartURL + uriPrefix;
                if (httplistener.Prefixes.Contains(uri)) return false;
                httplistener.Prefixes.Add(uri);

                string urihttps = "https://" + IP + ":" + 8908 + StartURL + uriPrefix;
                if (httplistener.Prefixes.Contains(urihttps)) return false;
                httplistener.Prefixes.Add(urihttps);
            }
            return true;
        }

        /// <summary>
        /// 执行请求监听行为
        /// </summary>
        void GethttRequest()
        {
            int maxThreadNum, portThreadNum;
            //线程池
            int minThreadNum;
            ThreadPool.GetMaxThreads(out maxThreadNum, out portThreadNum);
            ThreadPool.GetMinThreads(out minThreadNum, out portThreadNum);
            Console.WriteLine("最大线程数：{0}", maxThreadNum);
            Console.WriteLine("最小空闲线程数：{0}", minThreadNum);

            Console.WriteLine("\n\nhttp等待客户连接中。。。。");
            while (httplistener.IsListening)
            {
                try
                {
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    HttpListenerContext ctx = httplistener.GetContext();
                    // Console.WriteLine("\n连接。。。。");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc), ctx);
                }
                catch (Exception ex)
                {
                }
            }
        }

        void TaskProc(object o)
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            HttpListenerContext ctx = (HttpListenerContext)o;
            string Strlog = "";
            try
            {
                //Headers
                var endPoint = ctx.Request.RemoteEndPoint;//客户端IP
                Strlog += endPoint.Address.ToString() + ":" + endPoint.Port.ToString() + " ";
                Strlog += ctx.Request.HttpMethod + " ";
                Strlog += ctx.Request.RawUrl + " ";
                Strlog += ctx.Request.Headers.ToString() + " ";

                WebRequestHeaders webRequestHeaders = new WebRequestHeaders(ctx.Request.Headers);
                string sContentType = "";
                if (webRequestHeaders.ContentType == null || webRequestHeaders.ContentType.StartsWith(ContentType.EnctypeDefault, true, null))
                {
                    sContentType = ContentType.EnctypeDefault;
                }
                else if (webRequestHeaders.ContentType.StartsWith(ContentType.Json, true, null))
                {
                    sContentType = ContentType.Json;
                }
                else
                {
                    sContentType = webRequestHeaders.ContentType;
                }
                //获取rawUrl
                string rawUrl = ctx.Request.RawUrl;
                int paramStartIndex = rawUrl.IndexOf('?');
                if (paramStartIndex > 0)
                    rawUrl = rawUrl.Substring(0, paramStartIndex);
                else if (paramStartIndex == 0)
                    rawUrl = "";

                NameValueCollection NVCQuery = null;
                ConvertToDictionary conToDic = null;
                string Json = "";

                if (ctx.Request.HttpMethod == "POST")
                {
                    //读取客户端发送过来的数据
                    using (Stream inputStream = ctx.Request.InputStream)
                    {
                        using (StreamReader reader = new StreamReader(inputStream, Encoding.UTF8))
                        {
                            string data = reader.ReadToEnd();
                            //    data = System.Web.HttpUtility.UrlDecode(data);//解码字符串
                            if (sContentType == ContentType.EnctypeDefault)
                            {
                                conToDic = new ConvertToDictionary(data);
                                //   NVCQuery = HttpUtility.ParseQueryString(data);
                            }
                            else if (sContentType == ContentType.Json)
                            {
                                Json = data;
                            }
                            else
                            {
                                Json = data;
                            }
                            // Dictionary<string, string> dictionary = CoverstringToDictionary(data);
                        }
                    }
                }
                else//get
                {
                    if (paramStartIndex >= 0 && ctx.Request.RawUrl.Length >= paramStartIndex)
                    {
                        string QueryString = ctx.Request.RawUrl.Substring(paramStartIndex);
                        QueryString = Encoding.UTF8.GetString(Encoding.Default.GetBytes(QueryString));
                        QueryString = System.Web.HttpUtility.UrlDecode(QueryString);//解码字符串
                        conToDic = new ConvertToDictionary(QueryString);
                        NVCQuery = HttpUtility.ParseQueryString(QueryString);
                    }
                    else
                    {
                        NVCQuery = ctx.Request.QueryString;
                        conToDic = new ConvertToDictionary("");
                    }
                    //var msg = NVCQuery["BusData"]; //接受GET请求过来的参数；
                    //Console.WriteLine(msg);
                    //在此处执行你需要进行的操作>>比如什么缓存数据读取，队列消息处理，邮件消息队列添加等等。
                    // string filename = Path.GetFileName(ctx.Request.RawUrl);
                    // NVCQuery = HttpUtility.ParseQueryString(filename); //避免中文乱码
                    if (sContentType == ContentType.Json)
                    {
                        Json = conToDic.GetValue(0);
                        // Json = NVCQuery.Get(0);
                    }
                }

                string Outdata = "";
                int OutType = 0;//1直接返回展示页面
                if (string.Compare(rawUrl, StartURL + "/Helper/", true) == 0)
                {//说明文档
                    Outdata = PasSLog.ReadAllText("httpreadme.txt");

                    ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
                    ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    ctx.Response.ContentType = "text/html;charset=UTF-8";
                    ctx.Response.ContentEncoding = Encoding.UTF8;

                    //使用Writer输出http响应代码
                    using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
                    {

                        writer.Write(Outdata);
                        //  Console.WriteLine(Outdata);
                        writer.Close();
                        ctx.Response.Close();
                    }
                    return;
                }
                else if (rawUrl.StartsWith(StartURL + "/myspring", true, null))
                {
                    #region myspring 处理主要程序

                    SpringHttpData springHttpData = new SpringHttpData();

                    if (sContentType == ContentType.EnctypeDefault)
                    {//默认格式
                        if (rawUrl.StartsWith(StartURL + "/myspringAES/", true, null))
                        {
                            springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringAES/").Length).TrimEnd('/');
                            springHttpData.ParamType = 1;
                            springHttpData.Param = conToDic.GetValue("Param"); //加密的字符串~~
                            springHttpData.user_id = conToDic.GetValue("user_id");   // 
                            springHttpData.sign = conToDic.GetValue("sign");  //签名
                            springHttpData.CTag = conToDic.GetValue("CTag");   //灰度发布标记.可空
                            springHttpData.TID = conToDic.GetValue("TID"); //业务唯一流水号.可空

                            //springHttpData.Param = NVCQuery["Param"];//加密的字符串~~
                            //springHttpData.user_id  = NVCQuery["user_id"];// 
                            //springHttpData.sign  = NVCQuery["sign"];//签名
                            //springHttpData.CTag = NVCQuery["CTag"];//灰度发布标记.可空
                            //springHttpData.TID = NVCQuery["TID"];//业务唯一流水号.可空

                        }

                        else if (rawUrl.StartsWith(StartURL + "/myspringClear/", true, null))
                        {//myspringClear 明文入参
                            springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringClear/").Length).TrimEnd('/');
                            springHttpData.ParamType = 13;
                            springHttpData.Param = conToDic.GetValue("Param"); //加密的字符串~~
                            springHttpData.user_id = conToDic.GetValue("user_id");   // 
                            springHttpData.sign = conToDic.GetValue("sign");  //签名
                            springHttpData.CTag = conToDic.GetValue("CTag");   //灰度发布标记.可空
                            springHttpData.TID = conToDic.GetValue("TID"); //业务唯一流水号.可空
                        }
                    }
                    else if (sContentType == ContentType.Json)
                    {//Json格式
                        if (rawUrl.StartsWith(StartURL + "/myspringAES/", true, null))
                        {
                            try
                            {
                                springHttpData = JsonConvert.DeserializeObject<SpringHttpData>(Json);
                            }
                            catch
                            {
                                HttpResponseError(ctx, "请检查Json格式,格式为不正确.");
                                return;
                            }
                            springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringAES/").Length).TrimEnd('/');
                            springHttpData.ParamType = 1;


                        }
                        else if (rawUrl.StartsWith(StartURL + "/myspringClear/", true, null))
                        {// myspringClear 明文入参
                            try
                            {
                                springHttpData = JsonConvert.DeserializeObject<SpringHttpData>(Json);
                            }
                            catch
                            {
                                HttpResponseError(ctx, "请检查Json格式,格式为不正确.");
                                return;
                            }
                            springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringClear/").Length).TrimEnd('/');
                            springHttpData.ParamType = 13;
                        }

                    }
                    if (string.IsNullOrWhiteSpace(springHttpData.BusID))
                    {
                        HttpResponseSpringError(ctx, -1, "请检查URL,末节点不正确.", springHttpData.TID);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(springHttpData.Param))
                    {
                        HttpResponseSpringError(ctx, -1, "缺少必填参数Param!", springHttpData.TID);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(springHttpData.user_id) && springHttpData.ParamType != 13)
                    {
                        HttpResponseSpringError(ctx, -1, "缺少必填参数user_id!", springHttpData.TID);

                        return;
                    }
                    if (string.IsNullOrWhiteSpace(springHttpData.sign) && springHttpData.ParamType != 13)
                    {
                        HttpResponseSpringError(ctx, -1, "缺少必填参数sign!", springHttpData.TID);

                        return;
                    }

                    int EncryptType = 1;//加密类型，HTTP为1
                    string SEID = springHttpData.user_id;//用户加密ID

                    string Paramdecode = "";//调用参数
                    if (springHttpData.ParamType == 1)
                    {
                        if (!KeyData.AESKEYCheck(springHttpData.user_id, springHttpData.Param, springHttpData.sign))
                        {
                            HttpResponseSpringError(ctx, -1, "签名错误！", springHttpData.TID);

                            return;
                        }
                        try
                        {
                            Paramdecode = SignEncryptHelper.DecryptJave(springHttpData.Param, EncryptType, springHttpData.user_id);
                        }
                        catch (Exception ex)
                        {
                            HttpResponseSpringError(ctx, -1, "解密失败！", springHttpData.TID);

                            return;
                        }

                    }

                    else if (springHttpData.ParamType == 13)
                    {
                        if (!string.IsNullOrEmpty(springHttpData.user_id) && !string.IsNullOrEmpty(springHttpData.sign))
                        {
                            if (!KeyData.AESKEYCheck(springHttpData.user_id, springHttpData.Param, springHttpData.sign))
                            {
                                HttpResponseSpringError(ctx, -1, "签名错误！", springHttpData.TID);
                                return;
                            }
                        }
                        Paramdecode = springHttpData.Param;
                        EncryptType = 0;

                    }

                    #region 调用相关函数获取数据

                    string ApiResult = "";
                    string ResultMessage = "";
                    int ReslutCode = 0;
                    if (springHttpData.BusID.ToLower() == "sysstatus")
                    {
                        ApiResult = GetHttpSysStatus();
                        OutType = 1;
                    }
                    else
                    {
                        ReslutCode = HTTPServerAPI(springHttpData.BusID, Paramdecode, out ApiResult, out ResultMessage);
                        OutType = 0;
                    }


                    #endregion
                    if (OutType == 0)
                    {
                        SpringHttpDataOut springHttpDataOut = new SpringHttpDataOut();

                        springHttpDataOut.TID = springHttpData.TID;
                        springHttpDataOut.ReslutCode = ReslutCode;
                        springHttpDataOut.ResultMessage = ResultMessage;

                        if (EncryptType > 0)
                        {
                            springHttpDataOut.Param = SignEncryptHelper.EncryptJava(ApiResult, EncryptType, springHttpData.user_id);
                            springHttpDataOut.sign = KeyData.Sign(springHttpData.user_id, springHttpDataOut.Param);
                        }
                        else
                        {
                            springHttpDataOut.Param = ApiResult;
                            if (!string.IsNullOrEmpty(springHttpData.user_id))
                            {
                                springHttpDataOut.sign = KeyData.Sign(springHttpData.user_id, springHttpDataOut.Param);
                            }
                        }
                        Outdata = JsonConvert.SerializeObject(springHttpDataOut);
                    }
                    else if (OutType == 1)//1直接输出
                    {
                        Outdata = ApiResult;
                    }
                    #endregion
                }
                ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                ctx.Response.ContentType = "application/json;charset=UTF-8";
                if (OutType == 1)
                {
                    ctx.Response.ContentType = "text/html;charset=UTF-8";
                }

                ctx.Response.ContentEncoding = Encoding.UTF8;

                //使用Writer输出http响应代码
                using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
                {

                    writer.Write(Outdata);
                    //  Console.WriteLine(Outdata);
                    writer.Close();
                    ctx.Response.Close();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    HttpResponseError(ctx, "服务器错误:" + ex.Message);
                }
                catch { }
            }

            watch.Stop();
            PasSLog.HTTPLog(watch.ElapsedMilliseconds.ToString() + "毫秒", Strlog);
            //  Console.WriteLine(watch.ElapsedMilliseconds.ToString() + "毫秒");
        }

        private bool HttpResponseSpringError(HttpListenerContext ctx, int ReslutCode, string ResultMessage, string TID)
        {
            try
            {
                SpringHttpDataOut springHttpDataOut = new SpringHttpDataOut();
                springHttpDataOut.TID = TID;
                springHttpDataOut.ReslutCode = ReslutCode;
                springHttpDataOut.ResultMessage = ResultMessage;
                ctx.Response.StatusCode = 200;
                ctx.Response.ContentType = "application/html;charset=UTF-8";
                ctx.Response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(springHttpDataOut));
                //对客户端输出相应信息.
                ctx.Response.ContentLength64 = buffer.Length;
                System.IO.Stream output = ctx.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                //关闭输出流，释放相应资源
                output.Close();
                return true;
            }
            catch { }
            return true;
        }
        private bool HttpResponseError(HttpListenerContext ctx, string Outdata)
        {
            try
            {
                ctx.Response.StatusCode = 202;
                ctx.Response.ContentType = "application/text;charset=UTF-8";
                ctx.Response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Outdata);
                //对客户端输出相应信息.
                ctx.Response.ContentLength64 = buffer.Length;
                System.IO.Stream output = ctx.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                //关闭输出流，释放相应资源
                output.Close();
                return true;
            }
            catch { }
            return true;
        }
        /// <summary>
        /// 调用HTTP返回结果
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="retInfo"></param>
        private void RetHttp(SLBInfoHead sLBInfoHead, string retInfo)
        {
            RetHttp(sLBInfoHead, this.Encoding.GetBytes(retInfo));
        }
        /// <summary>
        /// 调用HTTP返回结果
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="bInfo"></param>
        private void RetHttp(SLBInfoHead sLBInfoHead, byte[] bInfo)
        {
            if (sdhttpCallServerBackwh.ContainsKey(sLBInfoHead.TID))
            {
                ResultStore resultStore = new ResultStore(CCNSysValye.BusServerToSCToClinet, sLBInfoHead, bInfo);
                sdhttpCallServerBack.TryAdd(sLBInfoHead.TID, resultStore);
                EventWaitHandle myHandleT;
                sdhttpCallServerBackwh.TryRemove(sLBInfoHead.TID, out myHandleT);
                myHandleT.Set();
            }
        }
        public class ConvertToDictionary
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            /// <summary>
            ///  将字符串key1=value1&key2=value2转换为Dictionary数据结构
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public ConvertToDictionary(string data)
            {
                if (null == data || 0 == data.Length)
                {
                    return;
                }
                if (data.StartsWith("?") && data.Length > 1)
                    data = data.Substring(1);
                string[] arrray = data.Split(new char[] { '&' });
                res = new Dictionary<string, string>();
                foreach (string s in arrray)
                {
                    int n = s.IndexOf("=");
                    string key = s.Substring(0, n);
                    string value = s.Substring(n + 1);
                    Console.WriteLine(key + "=" + value);
                    res.Add(key, value);
                }
            }
            public string GetValue(string Key)
            {
                if (res.ContainsKey(Key))
                {
                    return res[Key];
                }
                return "";
            }
            public string GetValue(int index)
            {
                if (res.Count > index)
                    return res.ElementAt(0).Value;
                return "";
            }
        }
        /// <summary>
        /// spring Http JSON入参结构
        /// </summary>
        class SpringHttpData
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
            /// 1 入参为业务数据AES加密 12入参为<see cref="SLBBusinessInfo"/>,13入参为业务明文(此模式只限制公司内部网络使用)
            /// </summary>
            internal int ParamType;

            internal string BusID;

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

        }
        /// <summary>
        /// 
        /// </summary>
        class SEncryptData
        {

            public int EncryptType;
            public string SEID;
            public string inBusInfo;
        }
        class ResultStore
        {
            public ResultStore(byte CcN, SLBInfoHead sLbInfoHead, byte[] b_Info)
            {
                CCN = CcN;
                sLBInfoHead = sLbInfoHead;
                bInfo = b_Info;
            }
            public byte CCN;
            public SLBInfoHead sLBInfoHead;
            public byte[] bInfo;
        }
        #endregion

        /// <summary>
        /// 将一个表的数据更新到另一个表中(内部使用)
        /// </summary>
        /// <param name="dtfrom">原数据表</param>
        /// <param name="dtTo">目标表</param>
        /// <param name="ColumnsKey">可以作为键的一个或多个列的名称, 因为用的的都是字符型的所以这里只处理字符型列</param>
        /// <returns></returns>
        private bool CopyTable(DataTable dtfrom, ref DataTable dtTo, List<string> ColumnsKey)
        {
            if (dtfrom == null)
            {
                dtfrom = null;
                return true;
            }
            if (dtTo == null || dtTo.Rows.Count != dtfrom.Rows.Count || ColumnsKey == null || ColumnsKey.Count == 0)
            {
                dtTo = dtfrom;
                return true;
            }
            for (int i = 0; i < ColumnsKey.Count; i++)
            {
                if (!dtfrom.Columns.Contains(ColumnsKey[i]) || !dtTo.Columns.Contains(ColumnsKey[i]))
                {
                    dtTo = dtfrom;
                    return true;
                }
            }
            foreach (DataRow dr in dtfrom.Rows)
            {
                string where = "";
                if (ColumnsKey.Count > 0)
                {
                    for (int i = 0; i < ColumnsKey.Count; i++)
                    {
                        if (i == 0)
                            where = ColumnsKey[i] + " = '" + dr[ColumnsKey[i]].ToString() + "'";
                        else
                            where = where + " and " + ColumnsKey[i] + " = '" + dr[ColumnsKey[i]].ToString() + "' ";
                    }
                    DataRow[] drs = dtTo.Select(where);
                    if (drs.Length == 0)
                    {
                        dtTo = dtfrom;
                        return true;
                    }
                    foreach (DataColumn Dc in dtTo.Columns)
                    {
                        if (!ColumnsKey.Contains(Dc.ColumnName))
                        {
                            drs[0][Dc.ColumnName] = dr[Dc.ColumnName];
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 将秒转换为小时 分钟 秒 
        /// </summary>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static string ConvertSecond(int Second)
        {
            string ssp = "";
            int statPeriodt = Second;
            if (statPeriodt >= 3600)
            {
                ssp = statPeriodt / 3600 + "小时";
                statPeriodt = statPeriodt % 3600;
            }
            if (statPeriodt >= 60)
            {
                ssp = ssp + statPeriodt / 60 + "分钟";
                statPeriodt = statPeriodt % 60;
            }
            if (statPeriodt > 0)
            {
                ssp = ssp + statPeriodt + "秒";
            }
            return ssp;
        }
        /// <summary>
        /// 获取秒转换成小时 不足一小时时 取1
        /// </summary>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static int GetHourMin1BySecond(int Second)
        {
            int hour = Second / 3600;
            return hour < 1 ? 1 : hour;

        }

        /// <summary>
        ///接收到SLB信息时保存的结构体(内部使用)
        /// </summary>
        class SDataSLBRates
        {
            public SLBInfo sLBInfo;
            public SLBCurrentStatus sLBCurrentStatus;
            /// <summary>
            /// 当前负载指标,是由其他数据综合而来
            /// </summary>
            public int LoadFactor;
        }

        class SCharData
        {
            public SCharData()
            {
                //  Time = DateTime.Now.ToString("HH:mm:ss");
                Time = DateTime.Now;
            }

            public DateTime Time;

            /// <summary>
            /// 业务信息，其统计和
            /// </summary>
            public ConcurrentDictionary<string, SCharBusRatesandStatus> cdBusiness = new ConcurrentDictionary<string, SCharBusRatesandStatus>();

            /// <summary>
            /// SLB
            /// Key SLBID
            /// </summary>
            public ConcurrentDictionary<string, SCharSLBRate> cdCharSLBRate = new ConcurrentDictionary<string, SCharSLBRate>();

            /// <summary>
            /// SLB 下的业务
            ///  Key SLBID  BusID
            /// </summary>
            public ConcurrentDictionary<string, Dictionary<string, SSLBBusRateStatus>> cdCharSLBbusRate = new ConcurrentDictionary<string, Dictionary<string, SSLBBusRateStatus>>();

            /// <summary>
            /// BusServer
            /// Key BusServerID
            /// </summary>
            public ConcurrentDictionary<string, SCharBusServerRates> cdCharBusServerRate = new ConcurrentDictionary<string, SCharBusServerRates>();

            /// <summary>
            ///BusServer下的业务
            ///Key BusServerID  BusID
            /// </summary>
            public ConcurrentDictionary<string, Dictionary<string, SBusServerBusRatesandStatus>> cdCharBusSeBusRate = new ConcurrentDictionary<string, Dictionary<string, SBusServerBusRatesandStatus>>();

        }


        /// <summary>
        /// 优化 SLB上线机制
        /// </summary>
        class SLBUpDownOptimize
        {/// <summary>
         /// 从第一个触发到现在秒数 超过10秒则不再等待跟随上线的BusServer
         /// </summary>
            public int SencondTime;
            /// <summary>
            /// 1 上线 0 下线
            /// </summary>
            public int Status;
            /// <summary>
            /// 上线的SLB
            /// </summary>
            public SLBInfo sLBInfo;

            public string Note;

            public List<string> ListBusServer = new List<string>();
        }
        /// <summary>
        /// 优化BusServer上线下线机制
        /// </summary>
        class BusServerUpDownOptimize
        {/// <summary>
         /// 从第一个触发到现在秒数 超过5秒则不再等待
         /// </summary>
            public int SencondTime;
            /// <summary>
            /// 1 上线 0 下线
            /// </summary>
            public int Status;
            public string BusServerID;
            public string Note;
            public List<SLBInfo> ListSLB = new List<SLBInfo>();
        }
    }
    /// <summary>
    ///系统基本配置数量
    /// </summary>
    public class SSysBaseCount
    {
        public bool AccessServerOnline;
        public int SLBCount;
        public int SLBRun;
        public int BusSCount;
        public int BusSRun;
        public int BusCount;
        public int BusRun;
        /// <summary>
        /// 没有在线BusServer的Bus
        /// </summary>
        public int NoServerBus = 0;
    }

    /// <summary>
    /// SLB的信息以及转发速度
    /// </summary>
    public class SSLBRateandStatus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sLBInfo"></param>
        /// <param name="sMessageRates"></param>
        /// <param name="sysInfo"></param>
        /// <param name="LoadFactor">当前负载指标,是由其他数据综合而来</param>
        public SSLBRateandStatus(SLBInfo sLBInfo, SMessageRates sMessageRates, SystemAndProcessInfo sysInfo, SLBCurrentStatus sLBCurrentStatus, int LoadFactor)
        {

            this.LoadFactor = LoadFactor;
            if (sLBInfo != null)
            {
                FBOSLBID = sLBInfo.SLBID;
                this.SLBID = sLBInfo.SLBID;
                this.SLBName = sLBInfo.SLBName;
                this.IP = sLBInfo.IP;
                this.Port = sLBInfo.Port;
                this.Status = sLBInfo.Status;
                this.IP2 = sLBInfo.IP2;
                this.Port2 = sLBInfo.Port2;
            }
            if (sMessageRates != null)
            {
                this.ClientCount = sMessageRates.ClientCount;
                //  this.ServerCount = sMessageRates.ServerCount;
                this.ClientToSLBRates = sMessageRates.ClientToPaaSRates;
                this.ServerToSLBRates = sMessageRates.ServerToSLBRates;
                this.SLBToServerRates = sMessageRates.SLBToServerRates;
                this.SLBToClientRates = sMessageRates.SLBToClientRates;
                this.SLBCacheToClientRates = sMessageRates.SLBCacheToClientRates;
                this.SLBUTToClientRates = sMessageRates.SLBUTToClientRates;
                this.ServerCallOtherToSLBRates = sMessageRates.ServerCallOtherToSLBRates;
                this.SLBToServerCallOtherBackRates = sMessageRates.SLBToServerCallOtherBackRates;
                this.SLBToServerCallOtherNoServerRates = sMessageRates.SLBToServerCallOtherNoServerRates;
                this.CacheDataCount = sMessageRates.CacheDataCount;
                this.WaitTakes = sMessageRates.WaitTakes;
                //   this.DevTestCount = sMessageRates.DevTestCount;
            }
            if (sysInfo != null)
            {
                this.Id = sysInfo.Id;
                this.ProcessName = sysInfo.ProcessName;
                this.TotalProcessorTime = sysInfo.TotalProcessorTime;
                this.WorkingSet64 = sysInfo.WorkingSet64;
                this.FileName = sysInfo.FileName;
                this.ProcessorCount = sysInfo.ProcessorCount;
                this.CpuLoad = sysInfo.CpuLoad;
                this.MemoryAvailable = sysInfo.MemoryAvailable;
                this.PhysicalMemory = sysInfo.PhysicalMemory;
                this.WorkingSet64MB = decimal.Parse(sysInfo.WorkingSet64MB().ToString("0.00"));

                this.MemoryAvailableMB = sysInfo.MemoryAvailableMB();
                this.ProcessorName = sysInfo.ProcessorName;
                this.MachineName = sysInfo.MachineName;
                this.OSVersion = sysInfo.OSVersion;
                this.IPAddress = sysInfo.IPAddress;
            }
            if (!string.IsNullOrEmpty(sLBCurrentStatus.OrgID) && !string.IsNullOrEmpty(sLBCurrentStatus.OrgID))
            {
                OrgID = sLBCurrentStatus.OrgID;
                FBOSLBID = sLBCurrentStatus.SLBID;
                ClientSLBID = sLBCurrentStatus.ClientSLBID;
            }
        }
        /// <summary>
        /// 负载均衡ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        ///  负载均衡名称
        /// </summary>
        public string SLBName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FBOSLBID { get; set; }
        /// <summary>
        ///医院SLBID  公司连接医院的客户端时需要，其他时为空
        /// </summary>
        public string ClientSLBID { get; set; }
        /// <summary>
        /// 医院客户ID，只有公司连接医院的客户端时非空，其他时为空
        /// </summary>
        public string OrgID { get; set; }


        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public decimal CpuLoadS
        {
            get
            {
                return decimal.Parse(this.CpuLoad.ToString("0.00"));
            }
        }
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public decimal WorkingSet64MB { get; set; }

        /// <summary>
        ///连接 客户端数量
        /// </summary>
        public int ClientCount { get; set; }
        /// <summary>
        /// 连接PTserver数量
        /// </summary>
        public int ServerCount { get; set; }
        /// <summary>
        ///客户端到SLB消息总频率 
        /// </summary>
        public int ClientToSLBRates { get; set; }
        /// <summary>
        ///BusServer到SLB消息总频率 
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
        /// 连接的开发测试终端数量
        /// </summary>
        public int DevTestCount { get; set; }

        /// <summary>
        /// 等待队列数量
        /// </summary>
        public int WaitTakes { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// 1 上线 ；0 下线
        /// </summary>
        public Int16 Status { get; set; }

        public string IP2 { get; set; }
        public int Port2 { get; set; }

        /// <summary>
        /// 当前负载指标,是由其他数据综合而来
        /// </summary>
        public int LoadFactor { get; set; }




        /// <summary>
        /// 进程唯一标识
        /// </summary>
        public int Id { get; set; }
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
        public long WorkingSet64 { get; set; }

        /// <summary>
        ///   /// <summary>
        /// 获取可用内存 
        /// </summary>
        /// </summary>
        public decimal MemoryAvailableMB { get; set; }
        /// <summary>
        /// 模块的完整路径
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad;
        /// <summary>
        /// 获取CPU名称
        /// </summary>
        public string ProcessorName { get; set; }
        /// <summary>
        /// 获取CPU个数 
        /// </summary>
        public int ProcessorCount { get; set; }
        ///// <summary>
        ///// 获取CPU占用率 
        ///// </summary>
        //public float CpuLoad;
        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable;



        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public long PhysicalMemory;


        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public decimal PhysicalMemoryGB
        {
            get { return decimal.Parse((PhysicalMemory / 1024m / 1024m / 1024m).ToString("0.00")); }
        }
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

    }
    /// <summary>
    /// 用于图表显示的 SLB的信息以及转发速度
    /// </summary>
    public class SCharSLBRate
    {
        public SCharSLBRate(SLBInfo sLBInfo, SMessageRates sMessageRates, SystemAndProcessInfo sysInfo, int LoadFactor)
        {

            this.LoadFactor = LoadFactor;
            if (sLBInfo != null)
            {
                this.SLBID = sLBInfo.SLBID;
                this.SLBName = sLBInfo.SLBName;

            }
            if (sMessageRates != null)
            {
                this.WebSocketCount = sMessageRates.WebSocketCount;
                this.ClientToPaaSRates = sMessageRates.ClientToPaaSRates;
                this.ServerToSLBRates = sMessageRates.ServerToSLBRates;
                this.SLBToServerRates = sMessageRates.SLBToServerRates;
                this.SLBToClientRates = sMessageRates.SLBToClientRates;
                this.SLBCacheToClientRates = sMessageRates.SLBCacheToClientRates;
                this.SLBUTToClientRates = sMessageRates.SLBUTToClientRates;
                this.ServerCallOtherToSLBRates = sMessageRates.ServerCallOtherToSLBRates;
                this.SLBToServerCallOtherBackRates = sMessageRates.SLBToServerCallOtherBackRates;
                this.SLBToServerCallOtherNoServerRates = sMessageRates.SLBToServerCallOtherNoServerRates;
                this.WaitTakes = sMessageRates.WaitTakes;
            }
            if (sysInfo != null)
            {
                this.TotalProcessorTime = sysInfo.TotalProcessorTime;
                this.WorkingSet64 = sysInfo.WorkingSet64;

                this.CpuLoad = sysInfo.CpuLoad;

                this.MemoryAvailable = sysInfo.MemoryAvailable;
                this.PhysicalMemory = sysInfo.PhysicalMemory;
                this.MemoryAvailableMB = sysInfo.MemoryAvailableMB();
            }


        }
        /// <summary>
        ///  <see cref="ServerLoadBalancing"/>运行端唯一ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        /// <see cref="ServerLoadBalancing"/>名称
        /// </summary>
        public string SLBName { get; set; }

        /// <summary>
        ///连接 客户端数量
        /// </summary>
        public int WebSocketCount { get; set; }
        /// <summary>
        /// 连接PTserver数量
        /// </summary>
        public int ServerCount { get; set; }
        /// <summary>
        ///客户端到SLB消息总频率 
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
        /// 等待队列数量
        /// </summary>
        public int WaitTakes { get; set; }

        /// <summary>
        ///  当前负载指标,是由其他数据综合而来
        /// </summary>
        public int LoadFactor { get; set; }




        /// <summary>
        /// 总处理器处理时间(毫秒)
        /// </summary>
        public double TotalProcessorTime { get; set; }
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public long WorkingSet64 { get; set; }



        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad { get; set; }


        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable { get; set; }
        /// <summary>
        ///   /// <summary>
        /// 获取可用内存 
        /// </summary>
        /// </summary>
        public decimal MemoryAvailableMB { get; set; }
        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public long PhysicalMemory { get; set; }

    }

    /// <summary>
    /// SLB中的BusInfo的信息及速度
    /// </summary>
    public class SSLBBusRateStatus
    {
        public SSLBBusRateStatus(SLBInfo sLBInfo, BusRates busInfoOnline)
        {

            UniqueConcurrentFlagList = new List<string>();
            if (sLBInfo != null)
            {
                this.SLBID = sLBInfo.SLBID;
                this.SLBName = sLBInfo.SLBName;
            }
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
                {
                    foreach (string str in busInfoOnline.UniqueConcurrentFlagList)
                    {
                        UniqueConcurrentFlagList.Add(str);
                    }
                }
                AllCount = busInfoOnline.AllCount;
                Rates = busInfoOnline.Rates;
            }
        }
        /// <summary>
        ///  <see cref="ServerLoadBalancing"/>运行端唯一ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        /// <see cref="ServerLoadBalancing"/>名称
        /// </summary>
        public string SLBName { get; set; }

        /// <summary>
        /// 业务(函数)唯一ID，与数据库中一致
        /// </summary>
        public string BusID { get; set; }




        public int Rates { get; set; }
        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent { get; set; }
        /// <summary>
        /// 总量
        /// </summary>
        public int AllCount { get; set; }
        /// <summary>
        /// 最大并发量 
        /// </summary>
        public int Maxconcurrent { get; set; }

        /// <summary>
        /// 业务状态
        /// </summary>
        public BusOnlineStatus busStatus { get; set; }
        /// <summary>
        /// 业务负载均衡策略
        /// </summary>
        public BusDDLBS busDDLBS { get; set; }

        /// <summary>
        /// 业务类别
        /// </summary>
        public BusType busType { get; set; }
        /// <summary>
        /// 唯一并发标记列表
        /// </summary>
        public List<String> UniqueConcurrentFlagList;


    }

    /// <summary>
    /// SLB中的BusInfo的信息及速度
    /// </summary>
    public class SCharSLBBusRateStatus
    {
        public SCharSLBBusRateStatus(SLBInfo sLBInfo, BusRates busInfoOnline)
        {
            if (sLBInfo != null)
            {
                this.SLBID = sLBInfo.SLBID;
                this.SLBName = sLBInfo.SLBName;
            }
            if (busInfoOnline != null)
            {
                BusID = busInfoOnline.BusID;

                Maxconcurrent = busInfoOnline.Maxconcurrent;
                TheConcurrent = busInfoOnline.TheConcurrent;
                AllCount = busInfoOnline.AllCount;
            }
        }
        /// <summary>
        ///  <see cref="ServerLoadBalancing"/>运行端唯一ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        /// <see cref="ServerLoadBalancing"/>名称
        /// </summary>
        public string SLBName { get; set; }

        /// <summary>
        /// 业务(函数)唯一ID，与数据库中一致
        /// </summary>
        public string BusID { get; set; }

        public int Rates { get; set; }
        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent { get; set; }
        /// <summary>
        /// 总量
        /// </summary>
        public int AllCount { get; set; }
        /// <summary>
        /// 最大并发量 
        /// </summary>
        public int Maxconcurrent { get; set; }


    }


    //-------------------------------------------
    public class SBusServerRateandStatus
    {
        public SBusServerRateandStatus(SBusServerRates sBusServerRates, SystemAndProcessInfo sysInfo)
        {
            this.BusServerID = sBusServerRates.BusServerID;
            this.ConnectSLB = sBusServerRates.ConnectSLB;
            this.IP = sBusServerRates.IP;
            this.RunningBusCount = sBusServerRates.runningBusCount;
            this.ServerToSLBRates = sBusServerRates.ServerToSLBRates;
            this.SLBToServerRates = sBusServerRates.SLBToServerRates;
            this.ServerCallOtherToSLBRates = sBusServerRates.ServerCallOtherToSLBRates;
            this.SLBToServerCallOtherBackRates = sBusServerRates.SLBToServerCallOtherBackRates;
            this.SLBToServerCallOtherNoServerRates = sBusServerRates.SLBToServerCallOtherNoServerRates;


            this.Id = sysInfo.Id;
            this.ProcessName = sysInfo.ProcessName;
            this.TotalProcessorTime = sysInfo.TotalProcessorTime;
            this.WorkingSet64 = sysInfo.WorkingSet64;
            this.FileName = sysInfo.FileName;
            this.ProcessorCount = sysInfo.ProcessorCount;
            this.CpuLoad = sysInfo.CpuLoad;
            this.MemoryAvailable = sysInfo.MemoryAvailable;
            this.PhysicalMemory = sysInfo.PhysicalMemory;
            this.WorkingSet64MB = decimal.Parse(sysInfo.WorkingSet64MB().ToString("0.00"));
            this.MemoryAvailableMB = (int)sysInfo.MemoryAvailableMB();
            this.PhysicalMemoryMB = (int)sysInfo.PhysicalMemoryMB();
            this.ProcessorName = sysInfo.ProcessorName;
            this.MachineName = sysInfo.MachineName;
            this.OSVersion = sysInfo.OSVersion;
            this.IPAddress = sysInfo.IPAddress;
            this.HttpIsRun = sBusServerRates.HttpIsRun;
            this.HttpPort = sBusServerRates.HttpPort;
            this.LogHttpAPISave = sBusServerRates.LogHttpAPISave;

            if (sysInfo.Drives != null && sysInfo.Drives.Count > 0)
            {
                this.Drives = sysInfo.Drives;
                this.SDrives = JsonConvert.SerializeObject(sysInfo.Drives);
            }

        }
        public string BusServerID { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string BusServerName { get; set; }

        /// <summary>
        /// 已经连接的SLB数量
        /// </summary>
        public int ConnectSLB { get; set; }



        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public decimal CpuLoadS
        {
            get
            {
                return decimal.Parse(this.CpuLoad.ToString("0.00"));
            }
        }

        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public decimal WorkingSet64MB { get; set; }


        /// <summary>
        /// 正在调用的业务数量
        /// </summary>
        public int RunningBusCount { get; set; }

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
        /// 在SLB上显示的IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 获取物理内存 MB
        /// </summary>
        /// <returns></returns>

        public int PhysicalMemoryMB;

        /// <summary>
        ///   /// <summary>
        /// 获取可用内存 MB
        /// </summary>
        /// </summary>
        public int MemoryAvailableMB { get; set; }


        /// <summary>
        /// 进程唯一标识
        /// </summary>
        public int Id { get; set; }
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
        /// 模块的完整路径
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad;
        /// <summary>
        /// 获取CPU名称
        /// </summary>
        public string ProcessorName { get; set; }
        /// <summary>
        /// 获取CPU个数 
        /// </summary>
        public int ProcessorCount { get; set; }
        ///// <summary>
        ///// 获取CPU占用率 
        ///// </summary>
        //public float CpuLoad;

        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public decimal PhysicalMemoryGB
        {
            get { return decimal.Parse((PhysicalMemory / 1024m / 1024m / 1024m).ToString("0.00")); }
        }
        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public long PhysicalMemory;

        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable;



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

        public List<DiskInfo> Drives;
        /// <summary>
        /// 磁盘信息s<see cref=" List<DiskInfo> "/>的JSON
        /// </summary>
        public string SDrives { get; set; }

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

    public class SCharBusServerRates
    {
        public SCharBusServerRates(SBusServerRates sBusServerRates, SystemAndProcessInfo sysInfo)
        {
            this.BusServerID = sBusServerRates.BusServerID;
            this.ConnnectSLB = sBusServerRates.ConnectSLB;
            this.IP = sBusServerRates.IP;
            this.RunningBusCount = sBusServerRates.runningBusCount;
            this.ServerToSLBRates = sBusServerRates.ServerToSLBRates;
            this.SLBToServerRates = sBusServerRates.SLBToServerRates;
            this.ServerCallOtherToSLBRates = sBusServerRates.ServerCallOtherToSLBRates;
            this.SLBToServerCallOtherBackRates = sBusServerRates.SLBToServerCallOtherBackRates;
            this.SLBToServerCallOtherNoServerRates = sBusServerRates.SLBToServerCallOtherNoServerRates;

            this.TotalProcessorTime = sysInfo.TotalProcessorTime;
            this.WorkingSet64 = sysInfo.WorkingSet64;

            this.CpuLoad = sysInfo.CpuLoad;
            this.MemoryAvailable = sysInfo.MemoryAvailable;
            this.PhysicalMemory = sysInfo.PhysicalMemory;
            Now = DateTime.Now;
        }
        public string BusServerID { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string BusServerName { get; set; }
        /// <summary>
        /// 已经连接的SLB数量
        /// </summary>
        public int ConnnectSLB { get; set; }

        /// <summary>
        /// 在SLB上显示的IP
        /// </summary>
        public string IP { get; set; }

        public DateTime Now { get; set; }


        /// <summary>
        /// 正在调用的业务数量
        /// </summary>
        public int RunningBusCount { get; set; }

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
        /// 总处理器处理时间(毫秒)
        /// </summary>
        public double TotalProcessorTime { get; set; }
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public long WorkingSet64 { get; set; }

        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad { get; set; }

        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable { get; set; }

        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public long PhysicalMemory { get; set; }



    }
    public class SBusServerBusRatesandStatus
    {

        public SBusServerBusRatesandStatus(SBusServerBusRates brs, string BusServerID)
        {
            BusID = brs.BusID;
            TheConcurrent = brs.TheConcurrent;
            AllCount = brs.AllCount;
            InRates = brs.InRates;
            OutRates = brs.OutRates;
            this.BusServerID = BusServerID;
            RAbnorma = brs.StatRangeData.AbnormaCount;
            RSysError = brs.StatRangeData.SysErrorCount;
            RBusError = brs.StatRangeData.BusErrorCount;
            RBusAbnormal = brs.StatRangeData.BusAbnormalCount;
            RAllCount = brs.StatRangeData.AllCount;
            StatPeriod_S = brs.StatPeriod;
            string ssp = "";
            int statPeriodt = brs.StatPeriod;
            if (statPeriodt >= 3600)
            {
                ssp = statPeriodt / 3600 + "小时";
                statPeriodt = statPeriodt % 3600;
            }
            if (statPeriodt >= 60)
            {
                ssp = ssp + statPeriodt / 60 + "分钟";
                statPeriodt = statPeriodt % 60;
            }
            if (statPeriodt > 0)
            {
                ssp = ssp + statPeriodt + "秒";
            }
            StatPeriod = ssp;

            /// <summary>
            /// 当前统计范围异常数率
            /// </summary>
            if (RAbnorma == 0 || RAllCount == 0)
            {
                RAbnormaRate = 0;
            }
            else
            {
                RAbnormaRate = RAbnorma * 100m / (RAllCount * 100m);
            }

            LBusAM = brs.StatRangeData.LBusAM;
            LBusEM = brs.StatRangeData.LBusEM;
            LSysEM = brs.StatRangeData.LSysEM;
        }

        public string BusServerID { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string BusServerName { get; set; }

        /// <summary>
        /// 业务(函数)唯一ID，与数据库中一致
        /// </summary>
        public string BusID { get; set; }

        /// <summary>
        /// 业务(函数)名称
        /// </summary>
        public string BusName { get; set; }

        /// <summary>
        /// 接收速率
        /// </summary>
        public int InRates { get; set; }

        /// <summary>
        /// 返回速率
        /// </summary>
        public int OutRates { get; set; }

        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent { get; set; }

        /// <summary> 
        /// 总量
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 当前统计范围异常数量RangeAbnorma
        /// </summary>
        public int RAbnorma { get; set; }

        /// <summary>
        /// 当前统计范围异常数率RangeAbnormaRate
        /// </summary>
        public decimal RAbnormaRate;// { get; set; }

        /// <summary>
        /// 当前统计范围业务异常数量
        /// </summary>
        public int RBusAbnormal { get; set; }

        /// <summary>
        /// 当前统计范围业务错误数量
        /// </summary>
        public int RBusError { get; set; }

        /// <summary>
        /// 当前统计范围系统异常数率RSysError
        /// </summary>
        public int RSysError { get; set; }

        /// <summary>
        /// 当前统计范围总数量RangeAllCount
        /// </summary>
        public int RAllCount { get; set; }

        /// <summary>
        /// 统计周期，秒
        /// </summary>
        public int StatPeriod_S;

        /// <summary>
        /// 统计周期，StatPeriod 
        /// </summary>
        public string StatPeriod { get; set; }
        /// <summary>
        /// 最后一次业务错误消息
        /// </summary>
        public string LBusEM { get; set; }
        /// <summary>
        /// 最后一次业务一次消息
        /// </summary>
        public string LBusAM { get; set; }
        /// <summary>
        /// 最后一次系统异常消息
        /// </summary>
        public string LSysEM { get; set; }
    }

    //-----------------------------------
    /// <summary>
    ///  用于图表显示的业务信息以及转发速度
    /// </summary>
    public class SCharBusRatesandStatus
    {
        /// <summary>
        /// 业务(函数)唯一ID，与数据库中一致
        /// </summary>
        public string BusID { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusName { get; set; }

        /// 接收速率
        /// </summary>
        public int InRates { get; set; }

        /// <summary>
        /// 返回速率
        /// </summary>
        public int OutRates { get; set; }

        /// <summary>
        /// 当前并发量
        /// </summary>
        public int TheConcurrent { get; set; }

        /// <summary>
        /// 总量
        /// </summary>
        public int AllCount { get; set; }
        /// <summary>
        /// 承载本业务的BusServer数量
        /// </summary>
        public int RunServerCount { get; set; }
    }

    //--------------------------------------

    /// <summary>
    /// 跟踪结果 InData、OutData已经解析为字符串
    /// </summary>
    public class STrackResultTest
    {

        public STrackResultTest(STrackResult sTrackResult)
        {
            TCID = sTrackResult.TCID;
            FTime = sTrackResult.FTime;

            BusID = sTrackResult.BusID;

            SLBID = sTrackResult.SLBID;
            BusServerID = sTrackResult.BusServerID;
            ClientID = sTrackResult.ClientID;
            CTag = sTrackResult.CTag;

            InCount = sTrackResult.InCount;
            OutCount = sTrackResult.OutCount;
            TID = sTrackResult.TID;
            InEncryptType = sTrackResult.InEncryptType;
            InSEID = sTrackResult.InSEID;
            OutEncryptType = sTrackResult.OutEncryptType;
            OutSEID = sTrackResult.OutSEID;
            OutTime = sTrackResult.OutTime;
            if (sTrackResult.InData != null)
            {
                if (sTrackResult.InEncryptType > 0)//加密
                {
                    sTrackResult.InData = SignEncryptHelper.Decrypt(sTrackResult.InData, sTrackResult.InEncryptType, sTrackResult.InSEID);
                }
                if (sTrackResult.InZip == 1) //GZip压缩
                    sTrackResult.InData = GZipHelper.Decompress(sTrackResult.InData);
                InData = Encoding.UTF8.GetString(sTrackResult.InData, 0, sTrackResult.InData.Length);
                if (BusID == "PBusF2FHelper")
                {
                    try
                    {
                        SLBBusinessInfo sLBBusinessInfo = JsonConvert.DeserializeObject<SLBBusinessInfo>(InData);
                        InteractiveData interactiveData = JsonConvert.DeserializeObject<InteractiveData>(sLBBusinessInfo.BusData);
                        string Paramdecode = SignEncryptHelper.Decrypt(interactiveData.Body, 1, interactiveData.HOSID);
                        InData += " ***业务明文:" + Paramdecode;
                    }
                    catch
                    {
                    }
                }
            }
            if (sTrackResult.OutData != null)
            {
                if (sTrackResult.OutEncryptType > 0)//加密
                {
                    sTrackResult.OutData = SignEncryptHelper.Decrypt(sTrackResult.OutData, sTrackResult.OutEncryptType, sTrackResult.OutSEID);
                }
                if (sTrackResult.OutZip == 1)//GZip压缩)
                    sTrackResult.OutData = GZipHelper.Decompress(sTrackResult.OutData);
                OutData = Encoding.UTF8.GetString(sTrackResult.OutData, 0, sTrackResult.OutData.Length);
                if (BusID == "PBusF2FHelper")
                {
                    try
                    {
                        SLBBusinessInfo sLBBusinessInfo = JsonConvert.DeserializeObject<SLBBusinessInfo>(OutData);
                        InteractiveData interactiveData = JsonConvert.DeserializeObject<InteractiveData>(sLBBusinessInfo.BusData);
                        string Paramdecode = SignEncryptHelper.Decrypt(interactiveData.Body, 1, interactiveData.HOSID);
                        OutData += "  ***业务明文:" + Paramdecode;
                    }
                    catch
                    {
                    }
                }
            }

        }

        class InteractiveData
        {
            /// <summary>
            /// 业务名称 唯一 
            /// </summary>
            public string ServiceName { get; set; }

            /// <summary>
            /// 响应码
            /// </summary>
            public Int32 Code { get; set; }

            /// <summary>
            /// 响应信息
            /// </summary>
            public string Msg { get; set; }

            /// <summary>
            /// 子错误码
            /// </summary>
            public string SubCode { get; set; }

            /// <summary>
            /// 子错误信息
            /// </summary>
            public string SubMsg { get; set; }

            /// <summary>
            /// 响应原始内容
            /// </summary>
            public string Body { get; set; }

            /// <summary>
            /// 签名
            /// </summary>
            public string Signature { get; set; }

            /// <summary>
            /// 医院代码
            /// </summary>
            public string HOSID { get; set; }


        }
        /// <summary>
        /// 记录保存时间
        /// </summary>
        public DateTime FTime;

        /// <summary>
        /// 获取出参时间
        /// </summary>
        public DateTime OutTime;

        /// <summary>
        /// 此跟踪唯一ID
        /// </summary>
        public string TCID { get; set; }
        /// <summary>
        /// <see cref="ServerLoadBalancing"/>运行端唯一ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        /// BusServerID
        /// </summary>
        public string BusServerID { get; set; }
        /// <summary>
        /// 业务(函数)唯一ID 
        /// </summary>
        public string BusID { get; set; }
        /// <summary>
        /// 客户端ID (IP:Port)
        /// </summary>
        public string ClientID { get; set; }
        /// <summary>
        ///  客户端灰度测试标记
        /// </summary>
        public string CTag { get; set; }
        /// <summary>
        /// 请求数量
        /// </summary>
        public int InCount { get; set; }
        /// <summary>
        /// 结果返回数量
        /// </summary>

        public int OutCount { get; set; }
        /// <summary>
        /// 此业务唯一ID
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        /// 入参
        /// </summary>
        public string InData { get; set; }
        /// <summary>
        /// 出参
        /// </summary>
        public string OutData { get; set; }


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

    }

    /// <summary>
    /// 系统信息内容，用来存储接收到的系统信息的结构体
    /// </summary>
    public class SSysinfoLog
    {
        public SSysinfoLog(string Info)
        {
            Time = DateTime.Now;
            this.Info = Info;
        }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 系统消息内容
        /// </summary>
        public string Info { get; set; }
    }

}

