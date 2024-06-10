using Newtonsoft.Json;
using PaaS.Comm;
using PasS.Base.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace PassCoreStarter
{
    class Program
    {
       static  PaaSCenter paaSCenter = null;
        static void Main(string[] args)
        {
            string sOSPlatform = "操作系统平台:";
            sOSPlatform += RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" : RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : "OSX";
            Console.WriteLine(sOSPlatform);
            Console.WriteLine($"系统架构：{RuntimeInformation.OSArchitecture}");
            Console.WriteLine($"系统名称：{RuntimeInformation.OSDescription}");
            Console.WriteLine($"进程架构：{RuntimeInformation.ProcessArchitecture}");
            Console.WriteLine($"是否64位操作系统：{Environment.Is64BitOperatingSystem}");

            PasSLog.Info("PassCore", string.Format("Path:{0}", AppContext.BaseDirectory));
            PasSLog.Info("PassCore", sOSPlatform);
            MEMORY_INFO mEMORY_ = MeminfoLinux.MemoryStatus();

            PasSLog.Info("PassCore", mEMORY_.ToString());
            string IPS = "";
            NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface NetworkIntf in interfaces)
            {
                if (NetworkIntf.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                    UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                    {

                        if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(UnicastIPAddressInformation.Address))
                        {

                            IPS += UnicastIPAddressInformation.Address.ToString();
                        }
                    }
                }
            }
            // Console.WriteLine(string.Format("IP:{0}", IPS));
            PasSLog.Info("PassCore", string.Format("IP:{0}", IPS));

            SLBStart(false);
            //System.Console.WriteLine("按 ctrl + c 结束运行");

            bool goon = true;
            while (goon)
            {
                string input = Console.ReadLine();
                if (input.ToUpper() == "Q")
                {
                    goon = false;
                }
                else if (input.ToUpper() == "B")
                {

                }
                else if (input.ToUpper() == "S")
                {

                }
            }
            PaaSStop();
            Exit(5);
        }

       static  void TestInfo()
        {
            try
            {
                List<BusinessInfoBusVersion> list = new List<BusinessInfoBusVersion>();
                string txtconfigInfo = PasSLog.ReadAllText(@"BusinessInfoConfigInfo.ini");
                if (!string.IsNullOrWhiteSpace(txtconfigInfo))
                {
                    list = JsonConvert.DeserializeObject<List<BusinessInfoBusVersion>>(txtconfigInfo);
                }
                else
                {
                    {
                        BusinessInfoBusVersion businessInfo = new BusinessInfoBusVersion();
                        businessInfo.BusID = "EBPP";
                        businessInfo.version = "1.0.0.5";
                        businessInfo.DllName = "CEBPP.dll";
                        businessInfo.DllPath = "\\";
                        businessInfo.busType = BusType.AsyncResult;
                        businessInfo.NamespaceClass = "EBPP.EBPPAPI";
                        businessInfo.BusVersion = "1.0";
                        list.Add(businessInfo);
                    }
                    {
                        BusinessInfoBusVersion businessInfo = new BusinessInfoBusVersion();
                        businessInfo.BusID = "BusinessTest";
                        businessInfo.version = "1.0.0.0";
                        businessInfo.DllName = "BusinessTest.dll";
                        businessInfo.DllPath = "\\";
                        businessInfo.busType = BusType.AsyncResult;
                        businessInfo.NamespaceClass = "BusinessTest.BusinessTestAPI";
                        businessInfo.BusVersion = "1.0";
                        list.Add(businessInfo);
                    }
                    PasSLog.WriteAllText(@"BusinessInfoConfigInfo.ini", JsonConvert.SerializeObject(list));
                }
                foreach (BusinessInfoBusVersion businessInfo in list)
                {
                    try
                    {
                        
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"加载业务{businessInfo.BusID},{businessInfo.DllName},异常:{ex2.Message }");
                    }
                }
                Console.WriteLine($"加载业务成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载业务失败:{ex.Message }");
            }
            InitTimer();
            // TRunWebHostBuilder();
            SLBStart(false);
        }

        //定义Timer类
        static System.Threading.Timer threadTimer;

        /// <summary>
        /// 初始化Timer类
        /// </summary>
        static private void InitTimer()
        {
            threadTimer = new System.Threading.Timer(new TimerCallback(TimerUp), null, 0, 1000);
        }

        /// <summary>
        /// 定时到点执行的事件
        /// </summary>
        /// <param name="value"></param>
        static private void TimerUp(object value)
        {
            int rage = 0;
            if (rage > 0)
            {

                double CPULOAD = SystemInfoWindowsLinux.CPULoad;
                //关联进程当前使用的物理内存总量
                long WorkingSet64 = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
                //为关联的进程分配的虚拟内存量
                // long VirtualMemorySize64 = Process.GetCurrentProcess().VirtualMemorySize64 / 1024 / 1024;
                MEMORY_INFO mEMORY_ = MeminfoLinux.MemoryStatus();

                Console.WriteLine(string.Format("{0} DellBusinessRate:{1}/s ,Cpu:{2}% ,Memory {3}MB ,{4}%, WorkingSet:{5}MB", DateTime.Now, rage, (int)CPULOAD, mEMORY_.MemTotalMB(), mEMORY_.UsageRate(), WorkingSet64));
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReStart">是否自动重启</param>
        internal static void SLBStart(bool ReStart)
        {
              SLBInfo sLBInfo;
            if (paaSCenter != null && paaSCenter.IsRunning)
            {
                PasSLog.Info("SLBStarterSLBStart", "SLB is runing.");
                Console.WriteLine($"当前服务  SLBID:{MyPubConstant.SLBID} , BusServerID:{MyPubConstant.BusServerID} is runing.  ");
                return;
            }
            Console.WriteLine($"当前服务  SLBID:{MyPubConstant.SLBID} , BusServerID:{MyPubConstant.BusServerID}   ");
            sLBInfo = SpringAPI.SLBInfoGet(MyPubConstant.SLBID);
            if (sLBInfo == null)
            {
                PasSLog.Info("SLBStarterSLBStart", "系统中没有此SLB参数，请检查配置文件中[SLBID]设置 ...");
                Console.WriteLine("系统中没有此SLB参数，请检查配置文件中[SLBID]设置 ...  ");
                Exit(5);
            }
            if (!ReStart)
            {
                //if (sLBInfo.Status == 1)
                //{
                //    Console.WriteLine("此[SLBID]对应的服务已经运行。  ");
                //    Exit(5);
                //}
            }
            try
            {
                paaSCenter = new  PaaSCenter(sLBInfo);
                paaSCenter.ReStartSLB_EventHandler += SLB_ReStartSLB_EventHandler1;


                PasSLog.Info("ZZJSLBStarter Start", "SLBStart...");
                Thread.Sleep(2000);
                paaSCenter.Start();
                //if (MyPubConstant.OpenSLBHttpServer)
                //{
                //    SLB.HttpStart();
                //}
                //  if (MyPubConstant.OpenSLBWebSocket)
                //{
                //    SLB.WebSocletSStart();
                //}

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
            }
        }

        private static void SLB_ReStartSLB_EventHandler1()
        {
            SLB_ReStartSLB_EventHandler();
        }

        internal static void SLB_ReStartSLB_EventHandler()
        {
            
            {
                Console.WriteLine("Procedure will ReStart in 10 second... \r\n");
                Thread.Sleep(10000);
                string ReStart = "ReStart";
                System.Diagnostics.Process.Start("ZZJSLBStarter.exe", "\"" + ReStart + "\"");
                Environment.Exit(0);
            }
        }

        internal static void PaaSStop()
        {
            if (paaSCenter != null)
            {
                paaSCenter.Stop();

                paaSCenter = null;
            }
        }

        internal static void Exit(int SleepSecond)
        {
           
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Procedure will stop in 5 second... \r\n");
                Thread.Sleep(1000 * SleepSecond);
                Environment.Exit(0);
            }
        }





        private static void SockTCPAssign_MessageRates1(int ClientCount, int ServerCount, int ClientRates, int ServerRates)
        {
            Console.WriteLine(string.Format("{0}  ClientCount:{1} Rates:{3}; ServerCount:{2} Rates:{4} \r\n", DateTime.Now.ToString("HH:mm:ss"), ClientCount.ToString().PadLeft(5, ' '), ServerCount.ToString().PadLeft(5, ' '), ClientRates.ToString().PadLeft(6, ' '), ServerRates.ToString().PadLeft(6, ' ')));
        }

    }
}
