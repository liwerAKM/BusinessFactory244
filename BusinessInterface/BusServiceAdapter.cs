using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PasS.Base.Lib;
using PasS.Base.Lib.Model;

namespace BusinessInterface
{
    public class BusServiceAdapter
    {
        public static string BusServerID = "";
        /// <summary>
        /// HTTP请求的的Header 中的 TID开头标识 ，用以区分是HTTP的请求
        /// </summary>
        const string HttpTIDStart = "HT:P";

        /// <summary>
        /// 是否开启API用户鉴权控制系统
        /// </summary>
        public static bool OpenAPIACF = false;

        /// <summary>
        /// TCP客户端是否开启API用户鉴权控制系统
        /// </summary>
        public static bool OpenAPIACF_TCP = false;

        /// <summary>
        /// HTTP客户端是否开启API用户鉴权控制系统
        /// </summary>
        public static bool OpenAPIACF_HTTP = false;




        Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 此BusServer的BusId  和BusID^BusVersion 之间关系列BusinessInfo列表
        /// Key:BusID,  value: ListBusID^BusVersion
        /// </summary>
        internal static ConcurrentDictionary<string, List<string>> sdBusandBusVer = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 此BusServer的<see cref="BusinessInfoBusVersion"/>列表
        /// Key:BusID^BusVersion,  value:<see cref="BusinessInfoBusVersion"/>
        /// </summary>
        internal static ConcurrentDictionary<string, BusinessInfoBusVersion> sdBusInfos = new ConcurrentDictionary<string, BusinessInfoBusVersion>();
        /// <summary>
        /// 测试版本
        ///  Key:BusID^BusVersion:  测试号BusinessInfoVersionStatus : value:<see cref="BusinessInfoBusVersion"/>
        /// </summary>
        static ConcurrentDictionary<string, ConcurrentDictionary<int, businessinfoversion>> sdBusInfoTest = new ConcurrentDictionary<string, ConcurrentDictionary<int, businessinfoversion>>();

        /// <summary>
        /// 默认测试组
        /// Status^MarkType  List<MarkValue>
        /// </summary>
        static ConcurrentDictionary<string, List<string>> sdDefaultTestMark = new ConcurrentDictionary<string, List<string>>();

        /// <summary>
        /// 设定的测试组
        /// busID^BusVersion ,  Status^MarkType  List MarkValue
        /// </summary>
        static ConcurrentDictionary<string, ConcurrentDictionary<string, List<string>>> sdTestMark = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<string>>>();

        /// <summary>
        /// 是否是开发测试
        /// </summary>
        public bool develop_test = false;

        /// <summary>
        ///  日志数据
        /// </summary>
        EventWaitHandle EWProcessingBusLog = new EventWaitHandle(false, EventResetMode.ManualReset);

        /// <summary>
        /// 日志数据的队列
        /// </summary>
        ConcurrentQueue<BusLogData> cQueueBusLog = new ConcurrentQueue<BusLogData>();
        /// <summary>
        /// 耗时业务时长记录点(毫秒)
        /// </summary>
        decimal LongExecuteTime = 1000;

        /// <summary>
        /// 日志是否通过HTTPAPI上传保存，否则通过配置的数据库保存
        /// </summary>
        public bool BusLogAPISave = false;

        public static string UpdateAPIUserInfoTime;
        public BusServiceAdapter()
        {
            // LongExecuteTime = MyPubConstant.LongExecuteTime;
            ProcessingBusLog();

        }
        /// <summary>
        /// 业务日志处理线程
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ProcessingBusLog()
        {
            var result = await Task.Run(() =>
            {
                while (true)
                {
                    EWProcessingBusLog.Reset();
                    EWProcessingBusLog.WaitOne();
                    while (cQueueBusLog.Count > 0)
                    {
                        try
                        {
                            BusLogData busLogData;
                            cQueueBusLog.TryDequeue(out busLogData);
                            try
                            {
                                if (BusLogAPISave)
                                {
                                    SpringAPI.SaveBusLogData(busLogData);
                                }
                                else
                                {
                                    BusLogHelper.SaveLog(busLogData);
                                }
                            }
                            catch (Exception ex)
                            {
                                PasSLog.Error("BusServiceAdapter.ProcessingBusLog", ex.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            PasSLog.Error("BusServiceAdapter.ProcessingBusLog", ex.ToString());
                        }

                    }
                }
                return true;
            });
            return result;
        }


        /// <summary>
        /// 加载开发本地DLL业务，仅限于开发测试调用
        /// </summary>
        /// <param name="busnessInfo"></param>
        public void AddOrUpdateBusinessDevelop_test(BusinessInfoBusVersion busnessInfo)
        {
            if (develop_test == false)
            {
                return;
            }
            if (busnessInfo != null)
            {
                string BusID = busnessInfo.BusID;
                string Key = busnessInfo.BusID + "^" + busnessInfo.BusVersion;
                if (!sdBusInfos.ContainsKey(Key))
                {
                    sdBusInfos.TryAdd(Key, busnessInfo);
                    if (sdBusandBusVer.ContainsKey(BusID))
                    {
                        if (!sdBusandBusVer[BusID].Contains(Key))
                        {
                            sdBusandBusVer[BusID].Add(Key);
                        }
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        list.Add(Key);
                        sdBusandBusVer.TryAdd(BusID, list);
                    }
                }
                else
                {
                    BusinessInfoBusVersion busnessInfoOLd = sdBusInfos[busnessInfo.BusID];
                    lock (sdBusInfos)
                    {
                        sdBusInfos.TryRemove(Key, out busnessInfoOLd);
                        sdBusInfos.TryAdd(Key, busnessInfo);

                        if (busnessInfoOLd.fileID != busnessInfo.fileID)
                        {
                            ProcessingBusinessAccess.DownLoadFile(busnessInfo);
                            Thread.Sleep(100);
                            ProcessingBusinessAccess.FreeAssembly(busnessInfoOLd);
                        }
                    }
                }
            }

        }

        public void AddOrUpdateBusiness(string BusID, string BusVersion)
        {
            BusinessInfoBusVersion busnessInfo = SpringAPI.BusinessInfoVerGet(BusServerID, BusID, BusVersion);
            AddOrUpdateBusiness(busnessInfo);
            return;
            string Key = BusID + "^" + BusVersion;
            if (busnessInfo != null)
            {
                if (!sdBusInfos.ContainsKey(Key))
                {
                    sdBusInfos.TryAdd(Key, busnessInfo);
                    if (sdBusandBusVer.ContainsKey(BusID))
                    {
                        if (!sdBusandBusVer[BusID].Contains(Key))
                        {
                            sdBusandBusVer[BusID].Add(Key);
                        }
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        list.Add(Key);
                        sdBusandBusVer.TryAdd(BusID, list);
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(string.Format("{0} AddBusinessBusVersion:{1}", DateTime.Now, Key));
                    PasSLog.Info("BusServiceAdapter.AddOrUpdateBusiness", string.Format("{0} AddBusinessBusVersion :{1}", DateTime.Now, Key));
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    BusinessInfoBusVersion busnessInfoOLd = sdBusInfos[Key];
                    lock (sdBusInfos)
                    {
                        sdBusInfos.TryRemove(Key, out busnessInfoOLd);
                        sdBusInfos.TryAdd(Key, busnessInfo);

                        if (busnessInfoOLd.fileID != busnessInfo.fileID)
                        {
                            ProcessingBusinessAccess.DownLoadFile(busnessInfo);
                            Thread.Sleep(100);
                            ProcessingBusinessAccess.FreeAssembly(busnessInfoOLd);
                        }

                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(string.Format("{0} UpdateBusinessBusVersion:{1}", DateTime.Now, Key));
                    PasSLog.Info("BusServiceAdapter.AddOrUpdateBusiness", string.Format("{0} UpdateBusinessBusVersion :{1}", DateTime.Now, Key));
                    Console.ForegroundColor = ConsoleColor.White;
                }
                AddOrUpdateBusinessTestVersion(BusID, BusVersion);
            }

        }

        public void AddOrUpdateBusiness(BusinessInfoBusVersion busnessInfo)
        {
          
            if (busnessInfo != null)
            {
                string Key = busnessInfo.BusID + "^" + busnessInfo.BusVersion;
                bool test = false;
                try
                {
                    ProcessingBusinessBase ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
                    if (ipb == null)
                    {
                        test = false;
                    }
                    else
                    {
                        test = true;
                    }
                }
                catch (Exception ex)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("加载 " + Key + "失败:" + ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    return;

                }
                if (test == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("加载 " + Key + "失败，请检查DLL及依赖是否存在，请检查配置命名空间及类名是否正确。");

                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
              
                if (!sdBusInfos.ContainsKey(Key))
                {
                    sdBusInfos.TryAdd(Key, busnessInfo);
                    if (sdBusandBusVer.ContainsKey(busnessInfo.BusID))
                    {
                        if (!sdBusandBusVer[busnessInfo.BusID].Contains(Key))
                        {
                            sdBusandBusVer[busnessInfo.BusID].Add(Key);
                        }
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        list.Add(Key);
                        sdBusandBusVer.TryAdd(busnessInfo.BusID, list);
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(string.Format("{0} UpdateBusinessBusVersion:{1}", DateTime.Now, Key));
                    PasSLog.Info("BusServiceAdapter.AddOrUpdateBusiness", string.Format("{0} UpdateBusinessBusVersion :{1}", DateTime.Now, Key));
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    BusinessInfoBusVersion busnessInfoOLd = sdBusInfos[Key];
                    lock (sdBusInfos)
                    {
                        sdBusInfos.TryRemove(Key, out busnessInfoOLd);
                        sdBusInfos.TryAdd(Key, busnessInfo);

                        if (busnessInfoOLd.fileID != busnessInfo.fileID)
                        {
                            ProcessingBusinessAccess.DownLoadFile(busnessInfo);
                            Thread.Sleep(100);
                            ProcessingBusinessAccess.FreeAssembly(busnessInfoOLd);
                        }

                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(string.Format("{0} UpdateBusinessBusVersion:{1}", DateTime.Now, Key));
                    PasSLog.Info("BusServiceAdapter.AddOrUpdateBusiness", string.Format("{0} UpdateBusinessBusVersion :{1}", DateTime.Now, Key));
                    Console.ForegroundColor = ConsoleColor.White;
                }
                AddOrUpdateBusinessTestVersion(busnessInfo.BusID, busnessInfo.BusVersion);
            }
        }
        /// <summary>
        /// 更新已经存在的业务服务配置或版本（这里会下载业务）
        /// </summary>
        /// <param name="BusID"></param>
        public void UpdateBusBusinessInfoOrVersion(string BusID)
        {
            if (sdBusandBusVer.ContainsKey(BusID))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format("{0} UpdateBusBusiness:{1}", DateTime.Now, BusID));
                PasSLog.Info("BusServiceAdapter.UpdateBusBusinessInfo", string.Format("{0} UpdateBusBusinessVersion :{1}", DateTime.Now, BusID));
                Console.ForegroundColor = ConsoleColor.White;
                BusinessInfo businfo = SpringAPI.BusinessInfoGet(BusID);
                List<string> list = sdBusandBusVer[BusID];

                foreach (string key in list)
                {
                    if (sdBusInfos.ContainsKey(key))
                    {
                        sdBusInfos[key].Updateinfo(businfo);
                    }
                }
            }
        }
        /// <summary>
        /// 更新已经存在的业务服务配置或版本（这里会下载业务）
        /// </summary>
        /// <param name="BusID"></param>
        /// <param name="BusVersion"></param>
        public void UpdateBusBusinessInfoOrVersion(string BusID, string BusVersion)
        {
            string Key = BusID + "^" + BusVersion;
            BusinessInfoBusVersion busnessInfo = SpringAPI.BusinessInfoVerGet(MyPubConstant.BusServerID ,BusID, BusVersion);
            if (busnessInfo != null)
            {
                bool test = false;
                try
                {
                    ProcessingBusinessBase ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
                    if (ipb == null)
                    {
                        test = false;
                    }
                    else
                    {
                        test = true;
                    }
                }
                catch (Exception ex)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("加载 " + BusID + "的业务版本" + BusVersion + "失败:" + ex.Message);
                    Console.ForegroundColor = ConsoleColor.White;
                    return;

                }
                if (test = false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("加载 " + BusID + "的业务版本" + BusVersion + "失败，请检查DLL及依赖是否存在，请检查配置命名空间及类名是否正确。");

                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                if (!sdBusInfos.ContainsKey(Key))
                {
                    sdBusInfos.TryAdd(Key, busnessInfo);
                    if (sdBusandBusVer.ContainsKey(BusID))
                    {
                        if (!sdBusandBusVer[BusID].Contains(Key))
                        {
                            sdBusandBusVer[BusID].Add(Key);
                        }
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        list.Add(Key);
                        sdBusandBusVer.TryAdd(BusID, list);
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(string.Format("{0} AddBusinessBusVersion:{1}", DateTime.Now, Key));
                    PasSLog.Info("BusServiceAdapter.UpdateBusBusinessInfoOrVersion", string.Format("{0} AddBusinessBusVersion :{1}", DateTime.Now, Key));
                    Console.ForegroundColor = ConsoleColor.White;
                }

                else
                {
                    BusinessInfoBusVersion busnessInfoOLd = sdBusInfos[Key];
                    lock (sdBusInfos)
                    {
                        sdBusInfos.TryRemove(Key, out busnessInfoOLd);
                        sdBusInfos.TryAdd(Key, busnessInfo);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(string.Format("{0} UpdateBusBusinessVersion :{1} {2} -> {3}", DateTime.Now, Key, busnessInfoOLd.fileID, busnessInfo.fileID));
                        PasSLog.Info("BusServiceAdapter.UpdateBusBusinessInfoOrVersion", string.Format("{0} UpdateBusBusinessVersion :{1} {2} -> {3}", DateTime.Now, Key, busnessInfoOLd.fileID, busnessInfo.fileID));
                        Console.ForegroundColor = ConsoleColor.White;
                        if (busnessInfoOLd.fileID != busnessInfo.fileID)
                        {
                            //  ProcessingBusinessAccess.DownLoadFile(busnessInfo);
                            //  Thread.Sleep(100);
                            ProcessingBusinessAccess.FreeAssembly(busnessInfoOLd);
                            AddOrUpdateBusinessTestVersion(BusID, BusVersion);
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  更新已经存在的业务服务的测试版本,这里只会更新sdBusInfoTest，sdTestMark 释放ProcessingBusinessAccess中对应的版本，
        ///  相关DLL只有在在被调用时下载；
        /// </summary>
        /// <param name="BusID"></param>
        public void AddOrUpdateBusinessTestVersion(string BusID, string BusVersion)
        {
            string Key = BusID + "^" + BusVersion;
            ConcurrentDictionary<int, businessinfoversion> cdTest = SpringAPI.GetTestLevelListBV(BusID);
            lock (sdBusInfoTest)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(string.Format("{0} AddOrUpdateBusinessTestVersion :{1} ", DateTime.Now, Key));
                PasSLog.Info("BusServiceAdapter.AddOrUpdateBusinessTestVersion", string.Format("{0} AddOrUpdateBusinessTestVersion :{1} ", DateTime.Now, Key));

                Console.ForegroundColor = ConsoleColor.White;
                if (sdBusInfoTest.ContainsKey(Key))
                {
                    ConcurrentDictionary<int, businessinfoversion> cdTestold;
                    sdBusInfoTest.TryRemove(Key, out cdTestold);
                    foreach (businessinfoversion bv in cdTestold.Values)
                    {
                        ProcessingBusinessAccess.FreeAssembly(bv.BusID, bv.BusVersion, bv.Version);
                    }
                }
                if (cdTest != null)
                {
                    sdBusInfoTest.TryAdd(Key, cdTest);
                }
            }
            if (!sdTestMark.ContainsKey(Key))
            {
                AddorUpdateBusStatusAndTestMark(Key, BusVersion);
            }
        }

        /// <summary>
        /// 移除某一服务,移除前请确保已经不再接收对应服务
        /// </summary>
        /// <param name="BusID"></param>
        public void RemoveBusiness(string BusID)
        {

            if (sdBusandBusVer.ContainsKey(BusID))
            {
                List<string> list = new List<string>();
                sdBusandBusVer.TryRemove(BusID, out list);
                foreach (string key in list)
                {
                    if (sdBusInfos.ContainsKey(key))
                    {
                        BusinessInfoBusVersion businessInfo;
                        sdBusInfos.TryRemove(key, out businessInfo);
                        Thread.Sleep(100);
                        ProcessingBusinessAccess.FreeAssembly(businessInfo);
                    }
                    DeleteBusStatusAndTestMark(key);
                    RemoveBusinessTestVersion(key);
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(string.Format("{0} RemoveBusiness :{1} ", DateTime.Now, BusID));
                PasSLog.Info("BusServiceAdapter.RemoveBusiness", string.Format("{0} RemoveBusiness :{1} ", DateTime.Now, BusID));
            }
        }
        /// <summary>
        /// 移除某一服务,移除前请确保已经不再接收对应服务
        /// </summary>
        /// <param name="BusID"></param>
        public void RemoveBusinessVer(string BusID, string BusVersion)
        {
            string Key = BusID + "^" + BusVersion;
            if (sdBusInfos.ContainsKey(Key))
            {
                BusinessInfoBusVersion businessInfo;
                sdBusInfos.TryRemove(Key, out businessInfo);
                if (sdBusandBusVer.ContainsKey(BusID))
                {
                    if (sdBusandBusVer[BusID].Contains(Key))
                    {
                        sdBusandBusVer[BusID].Remove(Key);
                    }
                    if (sdBusandBusVer[BusID].Count == 0)
                    {
                        List<string> list = new List<string>();
                        sdBusandBusVer.TryRemove(BusID, out list);
                    }
                }
                Thread.Sleep(100);
                ProcessingBusinessAccess.FreeAssembly(businessInfo);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(string.Format("{0} RemoveBusinessVer :{1} ", DateTime.Now, Key));
                PasSLog.Info("BusServiceAdapter.RemoveBusinessVer", string.Format("{0} RemoveBusinessVer :{1} ", DateTime.Now, Key));
            }
            DeleteBusStatusAndTestMark(Key);
            RemoveBusinessTestVersion(Key);

        }
        /// <summary>
        /// 移除所有,移除前请确保已经不再接收对应服务
        /// </summary>
        public void RemoveBusiness()
        {
            foreach (string Key in sdBusInfos.Keys)
            {

                BusinessInfoBusVersion businessInfo;
                sdBusInfos.TryRemove(Key, out businessInfo);
                Thread.Sleep(100);
                ProcessingBusinessAccess.FreeAssembly(businessInfo);

                DeleteBusStatusAndTestMark(Key);
                RemoveBusinessTestVersion(Key);
            }
            sdBusandBusVer.Clear();
            sdBusInfos.Clear();
        }
        /// <summary>
        /// 移除某一业务版本服务对应测试版本
        /// </summary>
        /// <param name="Key">BusID^BusVersion</param>
        void RemoveBusinessTestVersion(string Key)
        {
            if (sdBusInfoTest.ContainsKey(Key))
            {
                lock (sdBusInfoTest)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(string.Format("{0} RemoveBusinessTestVersion :{1} ", DateTime.Now, Key));
                    PasSLog.Info("BusServiceAdapter.RemoveBusinessTestVersion", string.Format("{0} RemoveBusinessTestVersion :{1} ", DateTime.Now, Key));

                    Console.ForegroundColor = ConsoleColor.White;
                    ConcurrentDictionary<int, businessinfoversion> cdTestold;
                    sdBusInfoTest.TryRemove(Key, out cdTestold);
                    foreach (businessinfoversion bv in cdTestold.Values)
                    {
                        ProcessingBusinessAccess.FreeAssembly(bv.BusID, bv.Version);
                    }
                }
            }
        }
        /// <summary>
        /// 是否包含指定业务BusName
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool EXECBusiness(string BusID)
        {
            return sdBusandBusVer.ContainsKey(BusID);
        }
        /// <summary>
        /// 获取业务第一个版本(仅仅是测试用)
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public string GetBusinessExecDefVersion(string BusIDorHCCN)
        {
            if (sdBusandBusVer.ContainsKey(BusIDorHCCN))
            {
                return sdBusandBusVer[BusIDorHCCN][0].Split('^')[1];
            }
            return "1.0";
        }

        /// <summary>
        /// 是否包含指定业务BusID BusVersion
        /// </summary>
        /// <param name="BusID"></param>
        /// <returns></returns>
        public bool EXECBusiness(string BusID, string BusVersion)
        {
            BusinessInfoBusVersion busnessInfo = SpringAPI.BusinessInfoVerGet(BusServerID,BusID, BusVersion);
            if (busnessInfo == null)
            {
                return false;
            }
            try
            {
                ProcessingBusinessBase ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
                if (ipb == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("BusServiceAdapter.EXECBusiness", BusID + BusVersion + ex.ToString());
                return false;
            }
        }

        public class HandingBusinessResult : System.IDisposable
        {
            public HandingBusinessResult()
            {

            }
            public HandingBusinessResult(EHandingBusinessResult eResult)
            {
                this.eResult = eResult;
            }
            public EHandingBusinessResult eResult = EHandingBusinessResult.sucess;
            public string Message;
            public void Dispose()
            {

            }
        }

        //------------------------------------------------------------------------------------------

        /// <summary>
        /// 处理业务BusName
        /// </summary>
        /// <param name="sLBInfoHeadBusS"></param>
        /// <param name="bInfo"></param>
        /// <param name="bOutInfo"></param>
        /// <returns></returns>
        public HandingBusinessResult HandingBusiness(int CCN, SLBInfoHeadBusS sLBInfoHeadBusS, byte[] bInfo, out byte[] bOutInfo, out bool InteractiveEnd)
        {

            HandingBusinessResult hanBusResult = new HandingBusinessResult();

            InteractiveEnd = false;
            bOutInfo = null;

            bool Complete = false;
            //sLBInfoHeadBusS.BusName
            string strBusiness = this.Encoding.GetString(bInfo);


            int Hccn = CCN / 10000;//高位
            int Lccn = CCN % 10000;//低位
            string Key = Hccn.ToString() + "^" + sLBInfoHeadBusS.RealBVer;
            if (Hccn == 2000)//老的通过BusID交易的
            {
                Key = sLBInfoHeadBusS.BusID + "^" + sLBInfoHeadBusS.RealBVer;
            }

            if (Hccn == 2000)//老的通过BusID交易的
            {
                SLBBusinessInfo OutBusinessInfo = null;
                SLBBusinessInfo InBusinessInfo = JsonConvert.DeserializeObject<SLBBusinessInfo>(strBusiness);
                InBusinessInfo.AUID = sLBInfoHeadBusS.SEID;//add by wanglei 20181204 加入API用户ID
                if (sdBusInfos.ContainsKey(Key))
                {
                    DateTime dateTimeBegin = DateTime.Now;
                    string ErrorMessage = "";
                    BusinessInfoBusVersion busnessInfo = sdBusInfos[Key];
                    BusinessInfoVersionStatus busSstatus = BusinessInfoVersionStatus.Publish;
                    string version;


                    ProcessingBusinessBase ipb = GetProcessingBusiness(busnessInfo, InBusinessInfo, sLBInfoHeadBusS.RealBVer, out busSstatus, out version);//  ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);

                    dateTimeBegin = DateTime.Now;


                    if (busnessInfo.busType == BusType.AsyncResult || busnessInfo.busType == BusType.CacheData || busnessInfo.busType == BusType.GetData)
                    {
                        try
                        {

                            Complete = ipb.ProcessingBusiness(InBusinessInfo, out OutBusinessInfo);

                            if (Complete)
                            {
                                if (OutBusinessInfo.ReslutCode == 1)
                                {
                                    hanBusResult.eResult = EHandingBusinessResult.sucess;
                                }
                                else if (OutBusinessInfo.ReslutCode == 0)
                                {
                                    hanBusResult.eResult = EHandingBusinessResult.BusinessAbnormal;
                                    hanBusResult.Message = OutBusinessInfo.ResultMessage;
                                }
                                else
                                {
                                    hanBusResult.eResult = EHandingBusinessResult.BusinessError;
                                    hanBusResult.Message = OutBusinessInfo.ResultMessage;
                                }
                            }
                            else
                            {
                                hanBusResult.eResult = EHandingBusinessResult.BusinessError;
                                hanBusResult.Message = "PB返回False:" + OutBusinessInfo.ResultMessage;
                            }
                        }
                        catch (Exception ex)
                        {
                            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
                            OutBusinessInfo.ReslutCode = -1;
                            OutBusinessInfo.ResultMessage = "执行业务ProcessingBusiness出错:" + ex.Message;
                            ErrorMessage = ex.ToString();
                            hanBusResult.eResult = EHandingBusinessResult.SysError;
                            hanBusResult.Message = OutBusinessInfo.ResultMessage;
                        }
                    }
                    else if (busnessInfo.busType == BusType.Immediate)
                    {
                        OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
                        OutBusinessInfo.ReslutCode = 0;
                        try
                        {
                            Complete = ipb.ProcessingBusiness(InBusinessInfo);
                            if (Complete)
                            {
                                OutBusinessInfo.ReslutCode = 1;
                                OutBusinessInfo.BusData = "Success";
                                OutBusinessInfo.ResultMessage = "Success";
                                hanBusResult.eResult = EHandingBusinessResult.sucess;
                            }
                            else
                            {
                                OutBusinessInfo.ReslutCode = 0;
                                OutBusinessInfo.BusData = "Faile";
                                hanBusResult.eResult = EHandingBusinessResult.BusinessAbnormal;
                            }
                        }
                        catch (Exception ex)
                        {
                            OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
                            OutBusinessInfo.ReslutCode = -1;
                            OutBusinessInfo.BusData = "Faile";
                            OutBusinessInfo.ResultMessage = "执行业务ProcessingBusiness出错:" + ex.Message;
                            ErrorMessage = ex.ToString();
                            hanBusResult.eResult = EHandingBusinessResult.SysError;
                            hanBusResult.Message = OutBusinessInfo.ResultMessage;
                            Complete = false;
                        }
                    }
                    else if (busnessInfo.busType == BusType.Interactive)
                    {
                        Complete = ProcessBusinessInteractive(ipb, sLBInfoHeadBusS, InBusinessInfo, out OutBusinessInfo, out InteractiveEnd);
                        if (Complete)
                        {
                            if (OutBusinessInfo.ReslutCode == 1)
                            {
                                hanBusResult.eResult = EHandingBusinessResult.sucess;
                            }
                            else if (OutBusinessInfo.ReslutCode == 0)
                            {
                                hanBusResult.eResult = EHandingBusinessResult.BusinessAbnormal;
                                hanBusResult.Message = OutBusinessInfo.ResultMessage;
                            }
                            else
                            {
                                hanBusResult.eResult = EHandingBusinessResult.BusinessError;
                                hanBusResult.Message = OutBusinessInfo.ResultMessage;
                            }
                        }
                        else
                        {
                            hanBusResult.eResult = EHandingBusinessResult.BusinessError;
                            hanBusResult.Message = "PB返回False:" + OutBusinessInfo.ResultMessage;
                        }
                    }
                    string sOutBusiness = JsonConvert.SerializeObject(OutBusinessInfo);
                    bOutInfo = this.Encoding.GetBytes(sOutBusiness);


                    #region  日志处理
                    DateTime dateTimeend = DateTime.Now;
                    int ExecuteTime = (int)(dateTimeend - dateTimeBegin).TotalMilliseconds;
                    string TIDT = sLBInfoHeadBusS.TID;
                    if (!string.IsNullOrWhiteSpace(sLBInfoHeadBusS.TID))
                    {
                        TIDT = sLBInfoHeadBusS.TID;
                    }
                    if (string.IsNullOrEmpty(ErrorMessage))//非系统捕获错误日志
                    {
                        long RecordSlowExecuteT = busnessInfo.RecordSlowExecute == 0 ? (int)LongExecuteTime : busnessInfo.RecordSlowExecute;

                        if (ExecuteTime > RecordSlowExecuteT || busSstatus != BusinessInfoVersionStatus.Publish || hanBusResult.eResult == EHandingBusinessResult.BusinessError || hanBusResult.eResult == EHandingBusinessResult.BusinessAbnormal)//超时或业务错误或测试记录
                        {
                            BusLogData busLogData = new BusLogData(sLBInfoHeadBusS, strBusiness, dateTimeBegin, sOutBusiness, dateTimeend, busnessInfo.LogType, busnessInfo.BLogSeparate, busSstatus, version);
                            busLogData.ExecuteTime = ExecuteTime;
                            busLogData.TID = TIDT;
                            busLogData.ResultStatus = (int)hanBusResult.eResult;
                            cQueueBusLog.Enqueue(busLogData);
                            EWProcessingBusLog.Set();
                        }
                        else if (busnessInfo.LogType != 0)//普通不记录日志的不处理
                        {
                            BusLogData busLogData = new BusLogData(sLBInfoHeadBusS, busnessInfo.LogType == 2 ? "" : strBusiness, dateTimeBegin, busnessInfo.LogType == 1 ? "" : sOutBusiness, dateTimeend, busnessInfo.LogType, busnessInfo.BLogSeparate, busSstatus, version);
                            busLogData.ExecuteTime = ExecuteTime;
                            busLogData.TID = TIDT;
                            busLogData.ResultStatus = (int)hanBusResult.eResult;
                            cQueueBusLog.Enqueue(busLogData);
                            EWProcessingBusLog.Set();
                        }
                    }
                    else
                    {
                        BusLogData busLogData = new BusLogData(sLBInfoHeadBusS, strBusiness, dateTimeBegin, sOutBusiness, DateTime.Now, busnessInfo.LogType, busnessInfo.BLogSeparate, busSstatus, version);
                        busLogData.ExecuteTime = ExecuteTime;
                        busLogData.TID = TIDT;
                        busLogData.ErrorData = ErrorMessage;
                        busLogData.ResultStatus = (int)hanBusResult.eResult;
                        cQueueBusLog.Enqueue(busLogData);
                        EWProcessingBusLog.Set();
                    }
                    #endregion

                    return hanBusResult;
                }
                else
                {
                    OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
                    OutBusinessInfo.ReslutCode = -2;
                    OutBusinessInfo.BusData = "Faile";
                    OutBusinessInfo.ResultMessage = "BusServer[" + sLBInfoHeadBusS.BusServerID + "] not exist Bus:" + sLBInfoHeadBusS.BusID;

                    string sOutBusiness = JsonConvert.SerializeObject(OutBusinessInfo);
                    bOutInfo = this.Encoding.GetBytes(sOutBusiness);
                    #region  日志处理
                    string TIDT = sLBInfoHeadBusS.TID;
                    if (!string.IsNullOrWhiteSpace(InBusinessInfo.TID))
                    {
                        TIDT = InBusinessInfo.TID;
                    }
                    BusLogData busLogData = new BusLogData(sLBInfoHeadBusS, strBusiness, DateTime.Now, sOutBusiness, DateTime.Now, 0, false, sOutBusiness);
                    busLogData.TID = TIDT;
                    busLogData.ResultStatus = (int)EHandingBusinessResult.SysError;
                    cQueueBusLog.Enqueue(busLogData);
                    EWProcessingBusLog.Set();
                    #endregion

                    hanBusResult.Message = OutBusinessInfo.ResultMessage;
                    hanBusResult.eResult = EHandingBusinessResult.SysError;
                    return hanBusResult;
                }
            }
            else
            {
                if (sdBusInfos.ContainsKey(Key))
                {
                    DateTime dateTimeBegin = DateTime.Now;
                    string ErrorMessage = "";
                    BusinessInfoBusVersion busnessInfo = sdBusInfos[Key];
                    BusinessInfoVersionStatus busSstatus = BusinessInfoVersionStatus.Publish;
                    string version;

                    ProcessingBusinessBase ipb = GetProcessingBusiness(busnessInfo, sLBInfoHeadBusS.RealBVer, out busSstatus, out version);//  ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
                    if (busnessInfo.busType == BusType.AsyncResultByte || busnessInfo.busType == BusType.ImmediateByte)
                    {
                        try
                        {

                            Complete = ipb.ProcessingBusiness(CCN, sLBInfoHeadBusS, bInfo, out bOutInfo);

                            if (Complete)
                            {

                                hanBusResult.eResult = EHandingBusinessResult.sucess;

                            }
                            else
                            {
                                hanBusResult.eResult = EHandingBusinessResult.BusinessError;
                                hanBusResult.Message = "PB返回False:";
                            }
                        }
                        catch (Exception ex)
                        {
                            bOutInfo = ipb.DefErrotReturn(-1, ex.Message);
                            ErrorMessage = ex.ToString();
                            hanBusResult.eResult = EHandingBusinessResult.SysError;
                            hanBusResult.Message = ErrorMessage;
                        }
                    }
                    else
                    {
                        bOutInfo = Encoding.UTF8.GetBytes("BusServer[" + sLBInfoHeadBusS.BusServerID + "] not exist busType:" + busnessInfo.busType);
                    }
                }
                else
                {

                    bOutInfo = Encoding.UTF8.GetBytes("BusServer[" + sLBInfoHeadBusS.BusServerID + "] not exist bus:" + Key);

                    hanBusResult.Message = "BusServer[" + sLBInfoHeadBusS.BusServerID + "] not exist Bus:" + Hccn;
                    hanBusResult.eResult = EHandingBusinessResult.SysError;
                    return hanBusResult;
                }
                return hanBusResult;
            }


        }

        static ProcessingBusinessBase GetProcessingBusiness(BusinessInfoBusVersion busnessInfo, SLBBusinessInfo sLBBusinessInfoIn, string RealBVer, out BusinessInfoVersionStatus busSstatus, out string version)
        {
            ProcessingBusinessBase ipb = null;

            version = busnessInfo.version;
            try
            {
                ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BusServiceAdapter.GetProcessingBusiness " + busnessInfo.BusID + RealBVer + ex.Message);
                PasSLog.Error("BusServiceAdapter.GetProcessingBusiness", busnessInfo.BusID + RealBVer + ex.ToString());
            }
            busSstatus = BusinessInfoVersionStatus.Publish;
            try
            {

                #region 灰度测试
                if (busnessInfo.busType != BusType.CacheData)//为保证CacheData数据一致CacheData类型的不进行灰度测试
                {

                    if (!string.IsNullOrWhiteSpace(sLBBusinessInfoIn.CTag))
                    {
                        if (sLBBusinessInfoIn.CTag == "TestLevel1")
                        {
                            busSstatus = BusinessInfoVersionStatus.TestLevel1;
                        }
                        else if (sLBBusinessInfoIn.CTag == "TestLevel2")
                        {
                            busSstatus = BusinessInfoVersionStatus.TestLevel2;
                        }
                        else if (sLBBusinessInfoIn.CTag == "TestLevel3")
                        {
                            busSstatus = BusinessInfoVersionStatus.TestLevel3;
                        }
                        else if (sLBBusinessInfoIn.CTag.Contains('{') && sLBBusinessInfoIn.CTag.Contains('}'))
                        {
                            //根据配置处理
                            busSstatus = GetGrayLevelTest(sLBBusinessInfoIn.BusID, RealBVer, sLBBusinessInfoIn.CTag);
                        }
                    }
                    string Key = sLBBusinessInfoIn.BusID + "^" + RealBVer;
                    if (busSstatus != BusinessInfoVersionStatus.Publish && sdBusInfoTest.ContainsKey(Key))
                    {
                        if (sdBusInfoTest[Key].ContainsKey((int)busSstatus))
                        {
                            businessinfoversion bv = sdBusInfoTest[Key][(int)busSstatus];
                            BusinessInfoBusVersion busnessInfoTest = new BusinessInfoBusVersion(bv);
                            busnessInfoTest.BusVersion = RealBVer;
                            busnessInfoTest.NamespaceClass = busnessInfo.NamespaceClass;
                            version = busnessInfoTest.version;
                            ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfoTest);
                        }
                    }
                }
                #endregion
            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BusServiceAdapter.GetProcessingBusiness灰度测试" + ex.Message);
                PasSLog.Error("BusServiceAdapter.GetProcessingBusiness灰度测试", ex.ToString());
            }
            return ipb;

        }
        static ProcessingBusinessBase GetProcessingBusiness(BusinessInfoBusVersion busnessInfo, string RealBVer, out BusinessInfoVersionStatus busSstatus, out string version)
        {
            ProcessingBusinessBase ipb = null;

            version = busnessInfo.version;
            try
            {
                ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BusServiceAdapter.GetProcessingBusiness " + busnessInfo.BusID + RealBVer + ex.Message);
                PasSLog.Error("BusServiceAdapter.GetProcessingBusiness", busnessInfo.BusID + RealBVer + ex.ToString());
            }
            busSstatus = BusinessInfoVersionStatus.Publish;
            try
            {

                #region 灰度测试
                if (busnessInfo.busType != BusType.CacheData)//为保证CacheData数据一致CacheData类型的不进行灰度测试
                {


                    string Key = busnessInfo.BusID + "^" + RealBVer;
                    if (busSstatus != BusinessInfoVersionStatus.Publish && sdBusInfoTest.ContainsKey(Key))
                    {
                        if (sdBusInfoTest[Key].ContainsKey((int)busSstatus))
                        {
                            businessinfoversion bv = sdBusInfoTest[Key][(int)busSstatus];
                            BusinessInfoBusVersion busnessInfoTest = new BusinessInfoBusVersion(bv);
                            busnessInfoTest.BusVersion = RealBVer;
                            busnessInfoTest.NamespaceClass = busnessInfo.NamespaceClass;
                            version = busnessInfoTest.version;
                            ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfoTest);
                        }
                    }
                }
                #endregion
            }

            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("BusServiceAdapter.GetProcessingBusiness灰度测试" + ex.Message);
                PasSLog.Error("BusServiceAdapter.GetProcessingBusiness灰度测试", ex.ToString());
            }
            return ipb;

        }
        #region 灰度测试
        /// <summary>
        /// 加载默认测试标记
        /// </summary>
        public void IinitializeDefaultTestMark()
        {

            sdDefaultTestMark = SpringAPI.GetDefaultStatusAndMarkcd();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(string.Format("{0} IinitializeDefaultTestMark   ", DateTime.Now));
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// 更新指定BusID BusVersion测试标记
        /// </summary>
        /// <param name="BusID"></param>
        public void AddorUpdateBusStatusAndTestMark(string BUsID, string BUsVersion)
        {
            AddorUpdateBusStatusAndTestMark(new SBusIDBusVersion() { BusID = BUsID, BusVersion = BUsVersion });
        }
        /// <summary>
        /// 更新指定BusID  BusVersion测试标记 
        /// </summary>
        /// <param name="BusID"></param>
        public void AddorUpdateBusStatusAndTestMark(SBusIDBusVersion sBusIDBus)
        {
            string Key = sBusIDBus.BusID + "^" + sBusIDBus.BusVersion;
            if (sdBusInfoTest.ContainsKey(Key))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(string.Format("{0} AddorUpdateBusStatusAndTestMark :{1} ", DateTime.Now, Key));
                Console.ForegroundColor = ConsoleColor.White;
                PasSLog.Info("BusServiceAdapter.AddorUpdateBusStatusAndTestMark", string.Format("{0} AddorUpdateBusStatusAndTestMark :{1} ", DateTime.Now, Key));


                ConcurrentDictionary<string, List<string>> sdTestMarkT = SpringAPI.GetBusStatusAndMarkcd(sBusIDBus);
                lock (sdTestMark)
                {
                    if (sdTestMark.ContainsKey(Key))
                    {
                        ConcurrentDictionary<string, List<string>> sdTestMarkTold;
                        sdTestMark.TryRemove(Key, out sdTestMarkTold);
                    }
                    if (sdTestMarkT != null && sdTestMarkT.Count > 0)
                    {
                        sdTestMark.TryAdd(Key, sdTestMarkT);
                    }
                }
            }
        }

        /// <summary>
        ///  移除指定BusID^BusVersion测试标记
        /// </summary>
        /// <param name="BusID"></param>
        public void DeleteBusStatusAndTestMark(string BusID_BusVersion)
        {
            if (sdTestMark.ContainsKey(BusID_BusVersion))
            {
                lock (sdTestMark)
                {
                    ConcurrentDictionary<string, List<string>> sdTestMarkTold;
                    sdTestMark.TryRemove(BusID_BusVersion, out sdTestMarkTold);
                }
                PasSLog.Info("BusServiceAdapter.DeleteBusStatusAndTestMark", string.Format("{0} DeleteBusStatusAndTestMark :{1} ", DateTime.Now, BusID_BusVersion));

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(string.Format("{0} DeleteBusStatusAndTestMark :{1} ", DateTime.Now, BusID_BusVersion));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        /// <summary>
        /// 根据配置获取测试等级状态 
        /// </summary>
        /// <param name="BusID"></param>
        /// <param name="CTag"></param>
        /// <returns></returns>
        static BusinessInfoVersionStatus GetGrayLevelTest(string BusI2D, string BusVersion, string CTag)
        {
            string key = BusI2D + "^" + BusVersion;
            BusinessInfoVersionStatus busSstatus = BusinessInfoVersionStatus.Publish;
            if (sdBusInfoTest.ContainsKey(key))
            {
                SGrayLevelTest sGrayLevelTest;
                try
                {
                    sGrayLevelTest = JsonConvert.DeserializeObject<SGrayLevelTest>(CTag);
                }
                catch
                {
                    return busSstatus;
                }
                //默认分组
                if (sdBusInfoTest[key].ContainsKey((int)BusinessInfoVersionStatus.TestLevel3))
                {
                    if (GetGrayLevelTest((int)BusinessInfoVersionStatus.TestLevel3, sGrayLevelTest, sdDefaultTestMark))
                    {
                        busSstatus = BusinessInfoVersionStatus.TestLevel3;
                        return busSstatus;
                    }
                }
                if (sdBusInfoTest[key].ContainsKey((int)BusinessInfoVersionStatus.TestLevel2))
                {
                    if (GetGrayLevelTest((int)BusinessInfoVersionStatus.TestLevel2, sGrayLevelTest, sdDefaultTestMark))
                    {
                        busSstatus = BusinessInfoVersionStatus.TestLevel2;
                        return busSstatus;
                    }
                }

                if (sdBusInfoTest[key].ContainsKey((int)BusinessInfoVersionStatus.TestLevel1))
                {
                    if (GetGrayLevelTest((int)BusinessInfoVersionStatus.TestLevel1, sGrayLevelTest, sdDefaultTestMark))
                    {
                        busSstatus = BusinessInfoVersionStatus.TestLevel1;
                        return busSstatus;
                    }
                }

                //-----------//普通分组--------
                if (sdTestMark.ContainsKey(key))
                {
                    if (sdBusInfoTest[key].ContainsKey((int)BusinessInfoVersionStatus.TestLevel3))
                    {
                        if (GetGrayLevelTest((int)BusinessInfoVersionStatus.TestLevel3, sGrayLevelTest, sdTestMark[key]))
                        {
                            busSstatus = BusinessInfoVersionStatus.TestLevel3;
                            return busSstatus;
                        }
                    }
                    if (sdBusInfoTest[key].ContainsKey((int)BusinessInfoVersionStatus.TestLevel2))
                    {
                        if (GetGrayLevelTest((int)BusinessInfoVersionStatus.TestLevel2, sGrayLevelTest, sdTestMark[key]))
                        {
                            busSstatus = BusinessInfoVersionStatus.TestLevel2;
                            return busSstatus;
                        }
                    }
                    if (sdBusInfoTest[key].ContainsKey((int)BusinessInfoVersionStatus.TestLevel1))
                    {
                        if (GetGrayLevelTest((int)BusinessInfoVersionStatus.TestLevel1, sGrayLevelTest, sdTestMark[key]))
                        {
                            busSstatus = BusinessInfoVersionStatus.TestLevel1;
                            return busSstatus;
                        }
                    }
                }
            }
            return busSstatus;
        }
        static bool GetGrayLevelTest(int Status, SGrayLevelTest sGrayLevelTest, ConcurrentDictionary<string, List<string>> sdTMark)
        {
            if (!string.IsNullOrWhiteSpace(sGrayLevelTest.UserID) && GetGrayLevelTest(Status, "UserID", sGrayLevelTest.UserID, sdTMark))
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(sGrayLevelTest.DeviceModel) && GetGrayLevelTest(Status, "DeviceModel", sGrayLevelTest.DeviceModel, sdTMark))
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(sGrayLevelTest.OSversion) && GetGrayLevelTest(Status, "OSversion", sGrayLevelTest.OSversion, sdTMark))
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(sGrayLevelTest.Location) && GetGrayLevelTest(Status, "Location", sGrayLevelTest.Location, sdTMark))
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(sGrayLevelTest.UserDefined) && GetGrayLevelTest(Status, "UserDefined", sGrayLevelTest.UserDefined, sdTMark))
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(sGrayLevelTest.Sex) && GetGrayLevelTest(Status, "Sex", sGrayLevelTest.Sex, sdTMark))
            {
                return true;
            }
            return false;
        }
        static bool GetGrayLevelTest(int Status, string MarkType, string sMkakValue, ConcurrentDictionary<string, List<string>> sdTMark)
        {
            string sMkakkey = Status.ToString() + '^' + MarkType;
            if (sdTMark.ContainsKey(sMkakkey) && sdTMark[sMkakkey].Contains(sMkakValue))
            {
                return true;
            }
            return false;
        }
        #endregion


        //----------Interactive------------
        public delegate bool InteractiveCallClientBusinessEventHandler(SLBInfoHeadBusS sLBInfoHeadBusS, byte[] bInfo, out byte[] bOutInfo, bool isEndTag);
        public delegate bool InteractiveReturnClientBusServerIDHandler(SLBInfoHeadBusS sLBInfoHeadBusS, SLBBusinessInfo InBusinessInfoFirst);
        /// <summary>
        ///<see cref="BusType.Interactive"/>类型的,BusServer发送数据给Client
        /// </summary>
        public static event InteractiveCallClientBusinessEventHandler InteractiveCallClientBusinessEvent;
        public static event InteractiveReturnClientBusServerIDHandler InteractiveReturnClientBusServerIDEvent;
        /// <summary>
        /// 此BusServer的正在进行<see cref="ProcessingBusinessInteractive"/>列表
        /// Key:,  value:<see cref="ProcessingBusinessInteractive"/>
        /// </summary>
        static ConcurrentDictionary<string, SipbInteractiveInfo> sdipbInteractive = new ConcurrentDictionary<string, SipbInteractiveInfo>();

        bool ProcessBusinessInteractive(ProcessingBusinessBase ipb, SLBInfoHeadBusS sLBInfoHeadBusS, SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo, out bool isInteractive)
        {
            isInteractive = true;
            ProcessingBusinessInteractive ipbInteractive = (ProcessingBusinessInteractive)ipb;
            string Keyid = sLBInfoHeadBusS.BusID + InBusinessInfo.TID;

            SipbInteractiveInfo sipbInteractiveInfo = new SipbInteractiveInfo();
            sipbInteractiveInfo.sLBInfoHeadBusS = sLBInfoHeadBusS;
            sipbInteractiveInfo.ipbInteractive = ipbInteractive;
            sdipbInteractive.TryAdd(Keyid, sipbInteractiveInfo);

            SLBBusinessInfo InBusinessInfofirst = new SLBBusinessInfo(InBusinessInfo);
            InBusinessInfofirst.ReslutCode = 1;
            InBusinessInfofirst.ResultMessage = "服务器已经接收服务,正在处理!";
            InteractiveReturnClientBusServerIDEvent(new SLBInfoHeadBusS(sLBInfoHeadBusS), InBusinessInfofirst);

            bool ret = ipbInteractive.ProcessingBusiness(InBusinessInfo, out OutBusinessInfo, Keyid, out bool InteractiveEnd);
            if (!InteractiveEnd)
            {
                sdipbInteractive.TryRemove(Keyid, out sipbInteractiveInfo);
            }
            return ret;
        }
        /// <summary>
        /// <see cref="ProcessingBusinessInteractive"/>BusServer发给Client的数据
        /// </summary>
        /// <returns></returns>
        public static bool InteractiveContinueStoC(string Keyid, SLBBusinessInfo InsLBBusinessInfo, out SLBBusinessInfo OutLBBusinessInfo, bool isEndTag)
        {
            OutLBBusinessInfo = null;
            if (!sdipbInteractive.ContainsKey(Keyid))
            {
                return false;
            }
            byte[] bInfo = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(InsLBBusinessInfo));
            byte[] bOutInfo;
            return InteractiveCallClientBusinessEvent(new SLBInfoHeadBusS(sdipbInteractive[Keyid].sLBInfoHeadBusS), bInfo, out bOutInfo, isEndTag);
        }
        /// <summary>
        /// 接收Client的数据发给 BusServer的数据
        /// </summary>
        /// <returns></returns>
        public HandingBusinessResult InteractiveContinueCToS(SLBInfoHeadBusS sLBInfoHeadBusS, byte[] bInfo, out byte[] bOutInfo, out bool IsEndTag)
        {
            IsEndTag = false;
            HandingBusinessResult hanBusResult = new HandingBusinessResult();
            string strBusiness = this.Encoding.GetString(bInfo);
            SLBBusinessInfo InBusinessInfo = JsonConvert.DeserializeObject<SLBBusinessInfo>(strBusiness);
            string Keyid = sLBInfoHeadBusS.BusID + InBusinessInfo.TID;
            bool Complete = false;
            SLBBusinessInfo OutBusinessInfo = null;
            if (!sdipbInteractive.ContainsKey(Keyid))
            {
                hanBusResult.eResult = EHandingBusinessResult.SysError;
                hanBusResult.Message = "InteractiveContinueCToS not ContainsKey:" + Keyid;
                OutBusinessInfo = new SLBBusinessInfo(InBusinessInfo);
                OutBusinessInfo.ReslutCode = -2;
                OutBusinessInfo.BusData = "Faile";
                bOutInfo = this.Encoding.GetBytes(JsonConvert.SerializeObject(OutBusinessInfo));
                return hanBusResult;
            }
            else
            {
                Complete = sdipbInteractive[Keyid].ipbInteractive.InteractiveContinue(InBusinessInfo, out OutBusinessInfo, out IsEndTag);
                hanBusResult.eResult = Complete ? EHandingBusinessResult.sucess : EHandingBusinessResult.BusinessAbnormal;
                if (OutBusinessInfo != null && OutBusinessInfo.ReslutCode != 1)
                {
                    if (OutBusinessInfo.ReslutCode == 0)
                    {
                        hanBusResult.eResult = EHandingBusinessResult.BusinessAbnormal;
                    }
                    else
                    {
                        hanBusResult.eResult = EHandingBusinessResult.BusinessError;
                    }
                    hanBusResult.Message = "InteractiveContinueCToS 执行失败 Faile:" + OutBusinessInfo.ResultMessage;
                }
            }
            if (IsEndTag)
            {
                SipbInteractiveInfo sipbInteractiveInfo;
                sdipbInteractive.TryRemove(Keyid, out sipbInteractiveInfo);
            }
            bOutInfo = this.Encoding.GetBytes(JsonConvert.SerializeObject(OutBusinessInfo));
            return hanBusResult;
        }





        public static bool Ipb_CallOtherBusiness(SLBBusinessInfo sLBBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(sLBBusinessInfo.BusVersion))
                {
                    sLBBusinessInfo.BusVersion = "1.0";
                }
                string Key = sLBBusinessInfo.BusID + "^" + sLBBusinessInfo.BusVersion;
                if (sdBusInfos.ContainsKey(Key))//此服务器上有
                {
                    BusinessInfoBusVersion busnessInfo = sdBusInfos[Key];// BusTable.GetBusinessInfo(sLBBusinessInfo.BusID);
                                                                         //   ProcessingBusinessBase ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
                    BusinessInfoVersionStatus busSstatus;
                    string version;
                    ProcessingBusinessBase ipb = GetProcessingBusiness(busnessInfo, sLBBusinessInfo, sLBBusinessInfo.BusVersion, out busSstatus, out version);
                    ;
                    //if (!ipb.HasBandCallOtherBusiness)
                    //{
                    //    ipb.CallOtherBusinessEvent += Ipb_CallOtherBusiness;
                    //}
                    return ipb.ProcessingBusiness(sLBBusinessInfo, out OutBusinessInfo);
                }
                else//此服务器上无 需要转发其他服务器执行
                {
                    OutBusinessInfo = new SLBBusinessInfo(sLBBusinessInfo);
                    return false;
                    //  return CallOtherBusinessEvent(sLBBusinessInfo, out OutBusinessInfo);
                }
            }
            catch (Exception ex)
            {
                OutBusinessInfo = new SLBBusinessInfo(sLBBusinessInfo);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ipb_CallOtherBusiness" + ex.Message);
                PasSLog.Error("SLBBusServer.Ipb_CallOtherBusiness", ex.ToString());
                return false;
            }
        }

        public static bool Ipb_CallOtherBusiness(InputRoot sLBBusinessInfo, out OutputRoot OutBusinessInfo)
        {

            try
            {
               
                   string BusVersion = "1.0";
               
                string Key = sLBBusinessInfo.BusID + "^" + BusVersion;
                if (sdBusInfos.ContainsKey(Key))//此服务器上有
                {
                    using (HandingBusinessResult hanBusResult = new HandingBusinessResult())
                    {
                        bool Complete = false;
                        string ErrorMessage = "";



                        BusinessInfoBusVersion busnessInfo = sdBusInfos[Key];// BusTable.GetBusinessInfo(sLBBusinessInfo.BusID);
                                                                                     //   ProcessingBusinessBase ipb = ProcessingBusinessAccess.CreateProcessingBusiness(busnessInfo);
                        BusinessInfoVersionStatus busSstatus;
                        string version = "";
                        SLBBusinessInfo sLBBusinessInfo1 = new SLBBusinessInfo();
                        sLBBusinessInfo1.BusID = sLBBusinessInfo.BusID;
                        sLBBusinessInfo1.CTag = sLBBusinessInfo.CTag;
                        sLBBusinessInfo1.BusVersion = "1.0";
                        ProcessingBusinessBase ipb = GetProcessingBusiness(busnessInfo, sLBBusinessInfo1, sLBBusinessInfo1.BusVersion, out busSstatus, out version);
                        DateTime dateTimeBegin = DateTime.Now;
                        try
                        {
                            dateTimeBegin = DateTime.Now;
                            OutBusinessInfo = ipb.Trans(sLBBusinessInfo);

                        }
                        catch (Exception ex2)
                        {
                            OutBusinessInfo = new OutputRoot(sLBBusinessInfo);

                            OutBusinessInfo.RspCode = "9999";

                            OutBusinessInfo.RspMsg = ex2.Message;
                        }
                        return Complete;

                        return Complete;
                    }
                }
                else//此服务器上无 需要转发其他服务器执行
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    string message = $"Ipb_CallOtherBusiness 业务ID:{sLBBusinessInfo.BusID}不在此服务器上不能执行";
                    Console.WriteLine(message);
                    PasSLog.Error("SLBBusServer.Ipb_CallOtherBusiness", $" 业务ID:{sLBBusinessInfo.BusID}不在此服务器上不能执行");
                    OutBusinessInfo = new OutputRoot(sLBBusinessInfo);
                    OutBusinessInfo.RspMsg = message;
                    OutBusinessInfo.RspCode = "9998";
                    return false;

                }
            }
            catch (Exception ex)
            {
                OutBusinessInfo = new OutputRoot(sLBBusinessInfo);
                OutBusinessInfo.RspMsg = ex.Message;
                OutBusinessInfo.RspCode = "9997";

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ipb_CallOtherBusiness" + ex.Message);
                PasSLog.Error("SLBBusServer.Ipb_CallOtherBusiness", ex.ToString());
                return false;
            }



        }

        class SipbInteractiveInfo
        {
            public ProcessingBusinessInteractive ipbInteractive;
            public SLBInfoHeadBusS sLBInfoHeadBusS;
        }

    }
}
