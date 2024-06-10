using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics.Tracing;
using System.Linq;

namespace PasS.Base.Lib
{

  
    /// <summary>
    /// 系统信息类 - 获取CPU、内存、磁盘、进程信息 
    /// </summary>
    public class SystemInfo
    {
        private int m_ProcessorCount = 0;   //CPU个数 
      //  private PerformanceCounter pcCpuLoad;   //CPU计数器 
        private long m_PhysicalMemory = 0;   //物理内存 

        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;

      //  private RegistryKey CPUregistryKey;

       

        #region 构造函数 
        ///  
        /// 构造函数，初始化计数器等 
        ///  
        public SystemInfo()
        {
          
           

            //CPU个数 
            m_ProcessorCount = Environment.ProcessorCount;

            
        }
        #endregion

        public SystemAndProcessInfo GetCurrentProcess()
        {
            Process instance = Process.GetCurrentProcess();
            //SystemAndProcessInfo sysandProcessInfo = new SystemAndProcessInfo(instance, ProcessorName, ProcessorCount, CpuLoad, MemoryAvailable, PhysicalMemory, MachineName, OSVersion, IPAddress);

            MEMORY_INFO mEMORY_INFO= SystemInfoWindowsLinux.MEMORY;
            SystemAndProcessInfo sysandProcessInfo = new SystemAndProcessInfo(instance, ProcessorName, ProcessorCount, CpuLoad, mEMORY_INFO, MachineName, OSVersion, IPAddress, GetLogicalDrives());
            return sysandProcessInfo;
        }
        /// <summary>
        /// 获取CPU名称
        /// </summary>
        public string ProcessorName
        {
            get
            {
                return "";// CPUregistryKey.GetValue("ProcessorNameString").ToString();
            }
        }
        /// <summary>
        /// 获取CPU频率
        /// </summary>
        public string ProcessorMHz
        {
            get
            {
                return "";// CPUregistryKey.GetValue("~MHz").ToString();
            }
        }
        /// <summary>
        /// 获取CPU标识
        /// </summary>
        public string Identifier
        {
            get
            {
                return "";// CPUregistryKey.GetValue("~Identifier").ToString();
            }
        }


        #region CPU个数 
        /// <summary>
        /// 获取CPU个数 
        /// </summary>
        public int ProcessorCount
        {
            get
            {
                return m_ProcessorCount;
            }
        }
        #endregion

        #region CPU占用率 
        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad
        {
            get
            {
                return (float)CPULOAD;// pcCpuLoad.NextValue();
            }
        }
        public static double CPULOAD
        {
            get
            {
              return  SystemInfoWindowsLinux.CPULoad;
            }
        }

 
        #endregion

        /// <summary>
        /// 计算机名
        /// </summary>
        public string MachineName
        {
            get
            {
                try
                {
                    return System.Environment.GetEnvironmentVariable("ComputerName");
                }
                catch
                {

                    return "unknowName";
                }

            }
        }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string OSVersion
        {
            get
            {
                return     RuntimeInformation.OSDescription;
            }
        }
        /// <summary>
        /// IP列表
        /// </summary>
        public string IPAddress
        {
            get
            {
                try
                {
                    //获取IP地址
                    string st = "";
                    NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface NetworkIntf in interfaces)
                    {
                        if (NetworkIntf.OperationalStatus == OperationalStatus.Up)
                        {
                            IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                            UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                            foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                            {
                                if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    string  IP = UnicastIPAddressInformation.Address.ToString();
                                    if (!IP.StartsWith("169.254."))//window自行分配
                                        st += st == "" ? IP : ", " + IP;
                                }
                            }
                        }
                    }
                   
                    return st;
                }
                catch
                {
                    return "unknow";
                }
            }
        }

        public static bool IsWindows()
        {
            var platform = Environment.OSVersion.Platform;
            return platform == PlatformID.Win32NT;
        }

 

        #region 可用内存 
        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable
        {
            get
            {
                long availablebytes = 0;
                //GetMemoryStatus().ullAvailPhys
                ////ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_PerfOS_Memory"); 
                ////foreach (ManagementObject mo in mos.Get()) 
                ////{ 
                ////    availablebytes = long.Parse(mo["Availablebytes"].ToString()); 
                ////} 
                //ManagementClass mos = new ManagementClass("Win32_OperatingSystem");
                //foreach (ManagementObject mo in mos.GetInstances())
                //{
                //    if (mo["FreePhysicalMemory"] != null)
                //    {
                //        availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                //    }
               // }
                return availablebytes;
            }
        }
        #endregion

        #region 物理内存 

        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public long PhysicalMemory
        {
            get
            {
                return m_PhysicalMemory;
            }
        }
        #endregion

        #region 获得分区信息 
        /// <summary>
        /// 获取分区信息 
        /// </summary>
        /// <returns></returns>
        public List<DiskInfo> GetLogicalDrives()
        {
            List<DiskInfo> drives = new List<DiskInfo>();
            DriveInfo[] drive = DriveInfo.GetDrives();//获取所有驱动器
            foreach (DriveInfo df in drive)//遍历驱动器
            {
                if (df.IsReady && df.DriveType == DriveType.Fixed)
                {
                    drives.Add(new DiskInfo(df.Name, df.TotalSize, df.AvailableFreeSpace));
                }
            }
            return drives;
        }
        /// <summary>
        /// 获取特定分区信息 
        /// </summary>
        /// <param name="DriverID">盘符</param>
        /// <returns></returns>
        public List<DiskInfo> GetLogicalDrives(char DriverID)
        {
            List<DiskInfo> drives = new List<DiskInfo>();
            DriveInfo[] drive = DriveInfo.GetDrives();//获取所有驱动器
            foreach (DriveInfo df in drive)//遍历驱动器
            {
                if (df.IsReady && df.DriveType == DriveType.Fixed)
                {
                    drives.Add(new DiskInfo(df.Name, df.TotalSize, df.AvailableFreeSpace));
                }
            }

            return drives;
        }


        #endregion

        #region 获得进程列表 
        ///  
        /// 获得进程列表 
        ///  
        public List<ProcessInfo> GetProcessInfo()
        {
            List<ProcessInfo> pInfo = new List<ProcessInfo>();
            Process[] processes = Process.GetProcesses();
            foreach (Process instance in processes)
            {
                try
                {
                    pInfo.Add(new ProcessInfo(instance.Id,
                        instance.ProcessName,
                        instance.TotalProcessorTime.TotalMilliseconds,
                        instance.WorkingSet64,
                        instance.MainModule.FileName));
                }
                catch { }
            }
            return pInfo;
        }
        /// <summary>
        /// 获得特定进程信息 
        /// </summary>
        /// <param name="ProcessName">进程名称</param>
        /// <returns></returns>
        public List<ProcessInfo> GetProcessInfo(string ProcessName)
        {
            List<ProcessInfo> pInfo = new List<ProcessInfo>();
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach (Process instance in processes)
            {
                try
                {
                    pInfo.Add(new ProcessInfo(instance.Id,
                        instance.ProcessName,
                        instance.TotalProcessorTime.TotalMilliseconds,
                        instance.WorkingSet64,
                        instance.MainModule.FileName));
                }
                catch { }
            }
            return pInfo;
        }
        public struct ProcessInfo
        {
            public ProcessInfo(Process instance)
            {
                this.Id = instance.Id;
                this.ProcessName = instance.ProcessName;
                this.TotalProcessorTime = instance.TotalProcessorTime.TotalMilliseconds;
                this.WorkingSet64 = instance.WorkingSet64;
                this.FileName = instance.MainModule.FileName;
            }
            public ProcessInfo(int Id,
                 string ProcessName,
                 double TotalProcessorTime,
                 long WorkingSet64,
                 string FileName)
            {
                this.Id = Id;
                this.ProcessName = ProcessName;
                this.TotalProcessorTime = TotalProcessorTime;
                this.WorkingSet64 = WorkingSet64;
                this.FileName = FileName;
            }
            /// <summary>
            /// 进程唯一标识
            /// </summary>
            public int Id;
            /// <summary>
            /// 进程名称
            /// </summary>
            public string ProcessName;
            /// <summary>
            /// 总处理器处理时间(毫秒)
            /// </summary>
            public double TotalProcessorTime;
            /// <summary>
            /// 关联的内存分配的物理内存量
            /// </summary>
            public long WorkingSet64;
            /// <summary>
            /// 关联的内存分配的物理内存量
            /// </summary>
            public decimal WorkingSet64MB
            {
                get
                {
                    return WorkingSet64 / 1024m / 1024m;
                }
            }
            /// <summary>
            /// 模块的完整路径
            /// </summary>
            public string FileName;
        }

        #endregion

        #region 结束指定进程 
        ///  
        /// 结束指定进程 
        ///  
        /// 进程的 Process ID 
        public static void EndProcess(int pid)
        {
            try
            {
                Process process = Process.GetProcessById(pid);
                process.Kill();
            }
            catch { }
        }
        #endregion


        #region 查找所有应用程序标题 
        ///  
        /// 查找所有应用程序标题 
        ///  
        /// 应用程序标题范型 
        public static List<string> FindAllApps(int Handle)
        {
            List<string> Apps = new List<string>();
            /*
            int hwCurr;
            hwCurr = GetWindow(Handle, GW_HWNDFIRST);

            while (hwCurr > 0)
            {
                int IsTask = (WS_VISIBLE | WS_BORDER);
                int lngStyle = GetWindowLongA(hwCurr, GWL_STYLE);
                bool TaskWindow = ((lngStyle & IsTask) == IsTask);
                if (TaskWindow)
                {
                    int length = GetWindowTextLength(new IntPtr(hwCurr));
                    StringBuilder sb = new StringBuilder(2 * length + 1);
                    GetWindowText(hwCurr, sb, sb.Capacity);
                    string strTitle = sb.ToString();
                    if (!string.IsNullOrEmpty(strTitle))
                    {
                        Apps.Add(strTitle);
                    }
                }
                hwCurr = GetWindow(hwCurr, GW_HWNDNEXT);
            }
            */
            return Apps;
        }
        #endregion
    }

    public class DiskInfo
    {
        public DiskInfo()
        {
        }
        public DiskInfo(string Name,
         long Size,
         long FreeSpace)
        {
            this.Name = Name;
            this.SizeGB = decimal.Parse((Size / 1024m / 1024m / 1024m).ToString("0.00"));
            this.FreeSpaceGB = decimal.Parse((FreeSpace / 1024m / 1024m / 1024m).ToString("0.00"));
        }
        public string Name;
        public decimal SizeGB;
        public decimal FreeSpaceGB;

    }
    /// <summary>
    /// 系统信息结构 - 获取CPU、内存、磁盘、进程信息 
    /// </summary>
    public class SystemAndProcessDynamicInfo
    {

        public SystemAndProcessDynamicInfo()
        {
        }

        public SystemAndProcessDynamicInfo(Process instance, float CpuLoad, long MemoryAvailable)
        {

            this.TotalProcessorTime = instance.TotalProcessorTime.TotalMilliseconds;
            this.WorkingSet64 = instance.WorkingSet64;
            this.CpuLoad = CpuLoad;
            this.MemoryAvailable = MemoryAvailable;

        }


        /// <summary>
        /// 总处理器处理时间(毫秒)
        /// </summary>
        public double TotalProcessorTime;
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public long WorkingSet64;
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public decimal WorkingSet64MB()
        {
            return WorkingSet64 / 1024m / 1024m;
        }

        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad;
        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable;



        /// <summary>
        /// 获取可用内存 
        /// </summary>

        public decimal MemoryAvailableMB()
        {
            return MemoryAvailable / 1024m / 1024m;
        }


    }
    /// <summary>
    /// 系统信息结构 - 获取CPU、内存、磁盘、进程信息 
    /// </summary>
    public class SystemAndProcessInfo
    {

        public SystemAndProcessInfo()
        {
        }
        public SystemAndProcessInfo(Process instance, string ProcessorName, int ProcessorCount, float CpuLoad, MEMORY_INFO mEMORY_INFO, string MachineName, string OSVersion, string IPAddress, List<DiskInfo> Drivesinfo)
        {

            this.Id = instance.Id;
            this.ProcessName = instance.ProcessName;
            this.TotalProcessorTime = instance.TotalProcessorTime.TotalMilliseconds;
            this.WorkingSet64 = instance.WorkingSet64;
            this.FileName = instance.MainModule.FileName;
            this.ProcessorName = ProcessorName;
            this.ProcessorCount = ProcessorCount;
            this.CpuLoad = CpuLoad;
            this.MemoryAvailable = mEMORY_INFO.MemAvailable;
            this.PhysicalMemory = mEMORY_INFO.MemTotal;
            this.MachineName = MachineName;
            this.OSVersion = OSVersion;
            this.IPAddress = IPAddress;
            this.Drives = Drivesinfo;
        }
        public List<DiskInfo> Drives;
        public SystemAndProcessInfo(Process instance, string ProcessorName, int ProcessorCount, float CpuLoad, long MemoryAvailable, long PhysicalMemory, string MachineName, string OSVersion, string IPAddress)
        {

            this.Id = instance.Id;
            this.ProcessName = instance.ProcessName;
            this.TotalProcessorTime = instance.TotalProcessorTime.TotalMilliseconds;
            this.WorkingSet64 = instance.WorkingSet64;
            this.FileName = instance.MainModule.FileName;
            this.ProcessorName = ProcessorName;
            this.ProcessorCount = ProcessorCount;
            this.CpuLoad = CpuLoad;
            this.MemoryAvailable = MemoryAvailable;
            this.PhysicalMemory = PhysicalMemory;
            this.MachineName = MachineName;
            this.OSVersion = OSVersion;
            this.IPAddress = IPAddress;

        }
        public void Update(SystemAndProcessDynamicInfo DynamicInfo)
        {if (DynamicInfo != null)
            {
                this.CpuLoad = DynamicInfo.CpuLoad;
                this.MemoryAvailable = DynamicInfo.MemoryAvailable;
                this.TotalProcessorTime = DynamicInfo.TotalProcessorTime;
                this.WorkingSet64 = DynamicInfo.WorkingSet64;
            }

        }
        /// <summary>
        /// 进程唯一标识
        /// </summary>
        public int Id;
        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName;
        /// <summary>
        /// 总处理器处理时间(毫秒)
        /// </summary>
        public double TotalProcessorTime;
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public long WorkingSet64;
        /// <summary>
        /// 关联的内存分配的物理内存量
        /// </summary>
        public decimal WorkingSet64MB()
        {
            return WorkingSet64 / 1024m / 1024m;
        }
        /// <summary>
        /// 模块的完整路径
        /// </summary>
        public string FileName;
        /// <summary>
        /// 获取CPU名称
        /// </summary>
        public string ProcessorName;
        /// <summary>
        /// 获取CPU个数 
        /// </summary>
        public int ProcessorCount;
        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad;
        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable;

        /// <summary>
        /// 计算机名
        /// </summary>
        public string MachineName;
        /// <summary>
        /// 系统名称
        /// </summary>
        public string OSVersion;


        /// <summary>
        /// IP列表
        /// </summary>
        public string IPAddress;

        /// <summary>
        /// 获取可用内存 
        /// </summary>

        public decimal MemoryAvailableMB()
        {
            return MemoryAvailable / 1024m / 1024m;
        }
        /// <summary>
        ///  获取物理内存 
        /// </summary>
        public long PhysicalMemory;

        /// <summary>
        /// 获取物理内存 
        /// </summary>
        /// <returns></returns>

        public decimal PhysicalMemoryMB()
        {
            return PhysicalMemory / 1024m / 1024m;
        }

    }


    /// <summary>
    /// <para>
    /// This utility is used by <see cref="PrerequisiteSoftwareValidation"/> to detect the Microsoft Windows
    /// version currently running on the machine.
    /// </para>
    /// <para>
    /// This class uses Win API function GetVersionEx() from kernel32 library to retrieve additional parameters
    /// about the installed Windows version.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// <para>
    /// This class is immutable and thread safe.
    /// </para>
    /// </threadsafety>
    ///
    /// <author>saarixx</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
    public static class WindowsVersionDetector
    {
        /// <summary>
        /// <para>
        /// Represents Windows 95.
        /// </para>
        /// </summary>
        private const string Windows95 = "Windows 95";

        /// <summary>
        /// <para>
        /// Represents Windows 98.
        /// </para>
        /// </summary>
        private const string Windows98 = "Windows 98";

        /// <summary>
        /// <para>
        /// Represents Windows ME.
        /// </para>
        /// </summary>
        private const string WindowsME = "Windows ME";

        /// <summary>
        /// <para>
        /// Represents Windows Vista.
        /// </para>
        /// </summary>
        private const string WindowsVista = "Windows Vista";

        /// <summary>
        /// <para>
        /// Represents Windows Server 2008.
        /// </para>
        /// </summary>
        private const string WindowsServer2008 = "Windows Server 2008";
        private const string WindowsServer2008R2 = "Windows Server 2008 R2";

        /// <summary>
        /// Represents windows 10, windows server 2016 technical preview
        /// </summary>
        private const string Windows10 = "Windows 10";
        private const string WindowsServer2016TP = "Windows Server 2016 Technical Preview";

        #region Represents Windows8,8.1,server 2012,server 2012 R2 #CJF 11/06/2013
        /// <summary>
        /// Represents windows 8,8.1
        /// </summary>
        private const string Windows8 = "Windows 8";
        private const string Windows8_1 = "Windows 8.1";

        /// <summary>
        /// Represents windows server 2012
        /// </summary>
        private const string WindowsServer2012 = "Windows Server 2012";
        private const string WindowsServer2012R2 = "Windows Server 2012 R2";
        #endregion

        /// <summary>
        /// <para>
        /// Represents Windows 7.
        /// </para>
        /// </summary>
        private const string Windows7 = "Windows 7";

        /// <summary>
        /// <para>
        /// Represents Windows Server 2003, Storage.
        /// </para>
        /// </summary>
        private const string WindowsServer2003Storage = "Windows Server 2003, Storage";

        /// <summary>
        /// <para>
        /// Represents Windows Server 2003, Compute Cluster Edition.
        /// </para>
        /// </summary>
        private const string WindowsServer2003ComputeClusterEdition = "Windows Server 2003, Compute Cluster Edition";

        /// <summary>
        /// <para>
        /// Represents Windows Server 2003, Datacenter Edition.
        /// </para>
        /// </summary>
        private const string WindowsServer2003DatacenterEdition = "Windows Server 2003, Datacenter Edition";

        /// <summary>
        /// <para>
        /// Represents Windows Server 2003, Enterprise Edition.
        /// </para>
        /// </summary>
        private const string WindowsServer2003EnterpriseEdition = "Windows Server 2003, Enterprise Edition";

        /// <summary>
        /// <para>
        /// Represents Windows Server 2003, Web Edition.
        /// </para>
        /// </summary>
        private const string WindowsServer2003WebEdition = "Windows Server 2003, Web Edition";

        /// <summary>
        /// <para>
        /// Represents Windows Server 2003, Standard Edition.
        /// </para>
        /// </summary>
        private const string WindowsServer2003StandardEdition = "Windows Server 2003, Standard Edition";

        /// <summary>
        /// <para>
        /// Represents Windows XP Home Edition.
        /// </para>
        /// </summary>
        private const string WindowsXPHomeEdition = "Windows XP Home Edition";

        /// <summary>
        /// <para>
        /// Represents Windows XP Professional.
        /// </para>
        /// </summary>
        private const string WindowsXPProfessional = "Windows XP Professional";

        /// <summary>
        /// <para>
        /// Represents Windows 2000 Professional.
        /// </para>
        /// </summary>
        private const string Windows2000Professional = "Windows 2000 Professional";

        /// <summary>
        /// <para>
        /// Represents Windows 2000 Datacenter Server.
        /// </para>
        /// </summary>
        private const string Windows2000DatacenterServer = "Windows 2000 Datacenter Server";

        /// <summary>
        /// <para>
        /// Represents Windows 2000 Advanced Server.
        /// </para>
        /// </summary>
        private const string Windows2000AdvancedServer = "Windows 2000 Advanced Server";

        /// <summary>
        /// <para>
        /// Represents Windows 2000 Server.
        /// </para>
        /// </summary>
        private const string Windows2000Server = "Windows 2000 Server";

        /// <summary>
        /// <para>
        /// Represents Windows NT 4.0.
        /// </para>
        /// </summary>
        private const string WindowsNT40 = "Windows NT 4.0";

        /// <summary>
        /// <para>
        /// Represents Windows CE.
        /// </para>
        /// </summary>
        private const string WindowsCE = "Windows CE";

        /// <summary>
        /// <para>
        /// Represents Unknown.
        /// </para>
        /// </summary>
        private const string Unknown = "Unknown";

        /// <summary>
        /// <para>
        /// Represents VER_NT_WORKSTATION from kernel32 library.
        /// </para>
        /// </summary>
        private const byte VER_NT_WORKSTATION = 0x01;

        /// <summary>
        /// <para>
        /// Represents VER_SUITE_ENTERPRISE from kernel32 library.
        /// </para>
        /// </summary>
        private const short VER_SUITE_ENTERPRISE = 0x0002;

        /// <summary>
        /// <para>
        /// Represents VER_SUITE_DATACENTER from kernel32 library.
        /// </para>
        /// </summary>
        private const short VER_SUITE_DATACENTER = 0x0080;

        /// <summary>
        /// <para>
        /// Represents VER_SUITE_PERSONAL from kernel32 library.
        /// </para>
        /// </summary>
        private const short VER_SUITE_PERSONAL = 0x0200;

        /// <summary>
        /// <para>
        /// Represents VER_SUITE_BLADE from kernel32 library.
        /// </para>
        /// </summary>
        private const short VER_SUITE_BLADE = 0x0400;

        /// <summary>
        /// <para>
        /// Represents VER_SUITE_STORAGE_SERVER from kernel32 library.
        /// </para>
        /// </summary>
        private const short VER_SUITE_STORAGE_SERVER = 0x2000;

        /// <summary>
        /// <para>
        /// Represents VER_SUITE_COMPUTE_SERVER from kernel32 library.
        /// </para>
        /// </summary>
        private const short VER_SUITE_COMPUTE_SERVER = 0x4000;

        /// <summary>
        /// <para>
        /// Contains operating system version information. The information includes major and minor version
        /// numbers, a build number, a platform identifier, and information about product suites and the
        /// latest Service Pack installed on the system.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Please refer to WIN API document for more detailed information.
        /// </para>
        /// </remarks>
        ///
        /// <author>saarixx</author>
        /// <author>TCSDEVELOPER</author>
        /// <version>1.0</version>
        /// <copyright>Copyright (c) 2008, TopCoder, Inc. All rights reserved.</copyright>
        [StructLayout(LayoutKind.Sequential)]
        private struct OSVersionInfoEx
        {
            /// <summary>
            /// <para>
            /// The size of this data structure, in bytes.
            /// </para>
            /// </summary>
            public int dwOSVersionInfoSize;

            /// <summary>
            /// <para>
            /// The major version number of the operating system.
            /// </para>
            /// </summary>
            public int dwMajorVersion;

            /// <summary>
            /// <para>
            /// The minor version number of the operating system.
            /// </para>
            /// </summary>
            public int dwMinorVersion;

            /// <summary>
            /// <para>
            /// The build number of the operating system.
            /// </para>
            /// </summary>
            public int dwBuildNumber;

            /// <summary>
            /// <para>
            /// The operating system platform.
            /// </para>
            /// </summary>
            public int dwPlatformId;

            /// <summary>
            /// <para>
            /// A null-terminated string, such as "Service Pack 3", that indicates the latest Service Pack
            /// installed on the system. If no Service Pack has been installed, the string is empty.
            /// </para>
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;

            /// <summary>
            /// <para>
            /// The major version number of the latest Service Pack installed on the system. For example,
            /// for Service Pack 3, the major version number is 3. If no Service Pack has been installed, the
            /// value is zero.
            /// </para>
            /// </summary>
            public short wServicePackMajor;

            /// <summary>
            /// <para>
            /// The minor version number of the latest Service Pack installed on the system. For example,
            /// for Service Pack 3, the minor version number is 0.
            /// </para>
            /// </summary>
            public short wServicePackMinor;

            /// <summary>
            /// <para>
            /// A bit mask that identifies the product suites available on the system.
            /// </para>
            /// </summary>
            public short wSuiteMask;

            /// <summary>
            /// <para>
            /// Any additional information about the system.
            /// </para>
            /// </summary>
            public byte wProductType;

            /// <summary>
            /// <para>
            /// Reserved for future use.
            /// </para>
            /// </summary>
            public byte wReserved;
        }

        /// <summary>
        /// <para>
        /// Retrieves information about the current operating system.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Please refer to WIN API document for more detailed information.
        /// </para>
        /// </remarks>
        ///
        /// <param name="osVersionInfoEx">
        /// A structure that receives the operating system information.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a nonzero value. If the function fails, the
        /// return value is zero.
        /// </returns>
        [DllImport("kernel32.Dll")]
        private static extern short GetVersionEx(ref OSVersionInfoEx osVersionInfoEx);

        /// <summary>
        /// <para>
        /// Retrieves the Windows version name.
        /// </para>
        /// <para>
        /// Currently the result can be one of:
        /// <list type="bullet">
        /// <item>Windows 95</item>
        /// <item>Windows 98</item>
        /// <item>Windows ME</item>
        /// <item>Windows Vista</item>
        /// <item>Windows Server 2008</item>
        /// <item>Windows 7</item>
        /// <item>Windows Server 2003, Storage</item>
        /// <item>Windows Server 2003, Compute Cluster Edition</item>
        /// <item>Windows Server 2003, Datacenter Edition</item>
        /// <item>Windows Server 2003, Enterprise Edition</item>
        /// <item>Windows Server 2003, Web Edition</item>
        /// <item>Windows Server 2003, Standard Edition</item>
        /// <item>Windows XP Home Edition</item>
        /// <item>Windows XP Professional</item>
        /// <item>Windows 2000 Professional</item>
        /// <item>Windows 2000 Datacenter Server</item>
        /// <item>Windows 2000 Advanced Server</item>
        /// <item>Windows 2000 Server</item>
        /// <item>Windows NT 4.0</item>
        /// <item>Windows CE</item>
        /// <item>Unknown</item>
        /// </list>
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The Windows version name (e.g. "Windows Vista"), can not be null or empty.
        /// </returns>
        ///
        /// <exception cref="PrerequisiteSoftwareValidationException">
        /// If an error occurred while getting the version.
        /// </exception>
        public static string GetVersion()
        {
            try
            {
                // Get platform
                PlatformID platform = Environment.OSVersion.Platform;

                // Get Windows major version
                int majorVersion = Environment.OSVersion.Version.Major;

                // Get Windows minor version
                int minorVersion = Environment.OSVersion.Version.Minor;

                // Get OS Version Info
                OSVersionInfoEx os = new OSVersionInfoEx();

                if (platform == PlatformID.Win32NT)
                {
                    os.dwOSVersionInfoSize = Marshal.SizeOf(os);

                    // Call GetVersionEx
                    if (GetVersionEx(ref os) == 0)
                    {
                        return Unknown;
                    }
                }

                // Default is Unknown
                string versionName = Unknown;

                // Handle Win95, 98, ME
                if ((platform == PlatformID.Win32Windows) && (majorVersion == 4))
                {
                    switch (minorVersion)
                    {
                        case 0:
                            versionName = Windows95;
                            break;
                        case 10:
                            versionName = Windows98;
                            break;
                        case 90:
                            versionName = WindowsME;
                            break;
                    }
                }
                else if (platform == PlatformID.Win32NT)
                {
                    switch (majorVersion)
                    {
                        //Handle Windows 10, 
                        //add below support Win10 info to IPSClient.exe->IPS.manifest
                        //http://www.itdadao.com/article/227158/
                        //https://msdn.microsoft.com/en-us/library/dn481241(v=vs.85).aspx
                        //<supportedOS Id="{8e0f7a12-bfb3-4fe8-b9a5-48fd50a15a9a}"/>
                        case 10:
                            if (minorVersion == 0)
                            {
                                if (os.wProductType == VER_NT_WORKSTATION)
                                {
                                    versionName = Windows10;
                                }
                                else
                                {
                                    versionName = WindowsServer2016TP;
                                }
                            }
                            break;

                        // Handle Vista, Win2008,Windows 7,Windows 8/8.1, windows server 2012/2012R2
                        //https://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx

                        case 6:
                            #region Handle Windows8, 8.1, server 2012, server 2012 R2  #CJF 11/06/2013
                            if (minorVersion == 2)
                            {
                                if (os.wProductType == VER_NT_WORKSTATION)
                                {
                                    versionName = Windows8;
                                }
                                else
                                {
                                    versionName = WindowsServer2012;
                                }
                            }

                            if (minorVersion == 3)
                            {
                                if (os.wProductType == VER_NT_WORKSTATION)
                                {
                                    versionName = Windows8_1;
                                }
                                else
                                {
                                    versionName = WindowsServer2012R2;
                                }
                            }
                            #endregion

                            if (minorVersion == 1)
                            {  // Windows 7 or windows Server 2008 R2

                                if (os.wProductType == VER_NT_WORKSTATION)
                                {
                                    versionName = Windows7;
                                }
                                else
                                {
                                    versionName = WindowsServer2008R2;
                                }
                            }
                            else
                            if (minorVersion == 0)
                            {   // Vista or windows server 2008 
                                if (os.wProductType == VER_NT_WORKSTATION)
                                {
                                    versionName = WindowsVista;
                                }
                                else
                                {
                                    versionName = WindowsServer2008;
                                }
                            }

                            break;
                        // Handle Win2003, XP, Win2000
                        case 5:
                            switch (minorVersion)
                            {
                                case 2:
                                    if ((os.wSuiteMask & VER_SUITE_STORAGE_SERVER) == VER_SUITE_STORAGE_SERVER)
                                    {
                                        versionName = WindowsServer2003Storage;
                                    }
                                    else if ((os.wSuiteMask & VER_SUITE_COMPUTE_SERVER) == VER_SUITE_COMPUTE_SERVER)
                                    {
                                        versionName = WindowsServer2003ComputeClusterEdition;
                                    }
                                    else if ((os.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                                    {
                                        versionName = WindowsServer2003DatacenterEdition;
                                    }
                                    else if ((os.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                                    {
                                        versionName = WindowsServer2003EnterpriseEdition;
                                    }
                                    else if ((os.wSuiteMask & VER_SUITE_BLADE) == VER_SUITE_BLADE)
                                    {
                                        versionName = WindowsServer2003WebEdition;
                                    }
                                    else
                                    {
                                        versionName = WindowsServer2003StandardEdition;
                                    }
                                    break;
                                case 1:
                                    if ((os.wSuiteMask & VER_SUITE_PERSONAL) == VER_SUITE_PERSONAL)
                                    {
                                        versionName = WindowsXPHomeEdition;
                                    }
                                    else
                                    {
                                        versionName = WindowsXPProfessional;
                                    }
                                    break;
                                case 0:
                                    if ((os.wProductType & VER_NT_WORKSTATION) == VER_NT_WORKSTATION)
                                    {
                                        versionName = Windows2000Professional;
                                    }
                                    else if ((os.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                                    {
                                        versionName = Windows2000DatacenterServer;
                                    }
                                    else if ((os.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                                    {
                                        versionName = Windows2000AdvancedServer;
                                    }
                                    else
                                    {
                                        versionName = Windows2000Server;
                                    }
                                    break;
                            }
                            break;
                        // Handle WinNT
                        case 4:
                            if (majorVersion == 0)
                            {
                                versionName = WindowsNT40;
                            }
                            break;
                    }
                }
                // Handle WinCE
                else if (platform == PlatformID.WinCE)
                {
                    versionName = WindowsCE;
                }

                return versionName;
            }
            catch
            {
                return Unknown;
            }
        }

        /// <summary>
        /// <para>
        /// Retrieves the Windows service pack.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The Windows service pack (can not be null, empty if no service packs are installed)
        /// </returns>
        ///
        /// <exception cref="PrerequisiteSoftwareValidationException">
        /// If an error occurred while getting the version of the service pack.
        /// </exception>
        public static string GetServicePack()
        {
            try
            {
                return Environment.OSVersion.ServicePack;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

}
