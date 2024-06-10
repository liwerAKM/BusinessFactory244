using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 系统信息
    /// </summary>

    public class SystemInfoWindowsLinux
    {
        private static readonly Stopwatch _getMemoryStatusWath = new Stopwatch();
        public static  double CPULoad 
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    CPUWin32LoadValue.Refresh();
                   return  CPUWin32LoadValue.CPULOAD;
                }
                else
                {
                    CPULinuxLoadValue.Refresh();
                    return CPULinuxLoadValue.CPULOAD ;
                }
            }
        }
        public static MEMORY_INFO MEMORY
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return MeminfoWindows.MemoryStatus();
                }
                else
                {
                   return  MeminfoLinux.MemoryStatus();
                }
            }
        }
    }

    public class MeminfoWindows
    {

        private static MEMORY_INFOsEx _memoryStatusInfo = new MEMORY_INFOsEx();

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct MEMORY_INFOsEx
        {
            public uint dwLength; // 当前结构体大小
            public uint dwMemoryLoad; // 当前内存使用率
            public long ullTotalPhys; // 总计物理内存大小
            public long ullAvailPhys; // 可用物理内存大小
            public long ullTotalPageFile; // 总计交换文件大小
            public long ullAvailPageFile; // 总计交换文件大小
            public long ullTotalVirtual; // 总计虚拟内存大小
            public long ullAvailVirtual; // 可用虚拟内存大小
            public long ullAvailExtendedVirtual; // 保留 这个值始终为0
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORY_INFOsEx mi);


        private unsafe static MEMORY_INFOsEx GetMemoryStatus()
        {

            _memoryStatusInfo.dwLength = (uint)sizeof(MEMORY_INFOsEx);
            GlobalMemoryStatusEx(ref _memoryStatusInfo);
            return _memoryStatusInfo;

        }
        public static MEMORY_INFO MemoryStatus()
        {
            MEMORY_INFOsEx mEMORY_INF = GetMemoryStatus();
            MEMORY_INFO mEMORY_INFO = new MEMORY_INFO();
            mEMORY_INFO.MemTotal = mEMORY_INF.ullTotalPhys;
            mEMORY_INFO.MemAvailable  = mEMORY_INF.ullAvailPhys;
            mEMORY_INFO.SwapFree =  mEMORY_INF.ullAvailVirtual;
            mEMORY_INFO.SwapTotal  = mEMORY_INF.ullTotalPageFile;
            return mEMORY_INFO;
        }
    }
        /// <summary>
        /// Linux内存信息
        /// </summary>
        public class MeminfoLinux
    {
      static   MEMORY_INFO mEMORY = new MEMORY_INFO();

        public static MEMORY_INFO  MemoryStatus()
        {
            GlobalMemoryStatus(ref mEMORY);
            return mEMORY;
        }
        /// <summary>
        /// 内存使用率
        /// </summary>
        /// <returns></returns>
        public static int MemoryUsageRate()
        {
            MEMORY_INFO mEMORY_ = new MEMORY_INFO();
            GlobalMemoryStatus(ref mEMORY_);
            return mEMORY_.UsageRate();
        }
        private  static bool GlobalMemoryStatus(ref MEMORY_INFO mi)
        {
            string path = "/proc/meminfo";
            if (!File.Exists(path))
            {
                return false;
            }
            string meminfo = File.ReadAllText(path, Encoding.Default);
            string[] meminfos = meminfo.Split('\n');
            foreach (string item in meminfos)
            {
                try
                {
                    string itenName = "";
                    long? value = call(item, ref itenName);
                    if (!string.IsNullOrWhiteSpace(itenName) && value != null)
                        switch (itenName)
                        {
                            case ("MemTotal"): mi.MemTotal = value.Value*1024; break;
                            case ("MemAvailable"): mi.MemAvailable = value.Value * 1024; break;
                            case ("MemFree"): mi.MemFree = value.Value * 1024; break;
                            case ("Buffer"): mi.Buffer = value.Value * 1024; break;
                            case ("Cached"): mi.Cached = value.Value * 1024; break;
                            case ("SwapTotal"): mi.SwapTotal = value.Value * 1024; break;
                            case ("SwapFree"): mi.SwapFree = value.Value * 1024; break;
                                //     case ("CommitLimit"): mi.CommitLimit = value.Value; break;
                                //  case ("Committed_AS"): mi.Committed_AS = value.Value; break;
                                //case ("Active(anon)"): mi.Active_anon = value.Value; break;
                                //case ("Inactive(anon)"): mi.Inactive_anon = value.Value; break;
                                //case ("Active(file)"): mi.Active_file = value.Value; break;
                                //case ("Inactive(file)"): mi.Inactive_file = value.Value; break;

                        }
                }
                catch
                {
                }
            }
            return true;
            long? call(string line, ref string itenName)
            {
                int i = line.IndexOf(':');
                if (i < 0)
                {
                    return null;
                }
                string lk = line.Substring(0, i);
                if (string.IsNullOrEmpty(lk))
                {
                    return null;
                }
                itenName = lk;
                string value = line.Substring(i + 1).TrimStart();
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }
                string[] sp = value.Split(' ');
                if (sp == null || sp.Length <= 0)
                {
                    return null;
                }
                line = sp[0];
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }
                long.TryParse(line, out long n);
                return n;
            }
        }
      
    }


    public class MEMORY_INFO
    {
        public uint dwLength; // 当前结构体大小

        /// <summary>
        /// 所有可用的内存大小，物理内存减去预留位和内核使用
        /// </summary>
        public long MemTotal;     //所有可用的内存大小，物理内存减去预留位和内核使用。系统从加电开始到引导完成，firmware/BIOS要预留一些内存，内核本身要占用一些内存，最后剩下可供内核支配的内存就是MemTotal。这个值在系统运行期间一般是固定不变的，重启会改变。
        /// <summary>
        /// 表示系统尚未使用的内存。
        /// </summary>
        public long MemFree;             //表示系统尚未使用的内存。
        /// <summary>
        /// 真正的系统可用内存，
        /// </summary>
        public long MemAvailable;   //真正的系统可用内存，系统中有些内存虽然已被使用但是可以回收的，比如cache/buffer、slab都有一部分可以回收，所以这部分可回收的内存加上MemFree才是系统可用的内存
        public long Buffer;  //用来给块设备做缓存的内存，(文件系统的 metadata、pages)
        public long Cached;  //分配给文件缓冲区的内存,例如vi一个文件，就会将未保存的内容写到该缓冲区
                             // public long SwapCached;   //被高速缓冲存储用的交换空间（硬盘的swap）的大小
                             // public long Active;   //经常使用的高速缓冲存储器页面文件大小
                             // public long Inactive;   //不经常使用的高速缓冲存储器文件大小
                             //  public long Active_anon;  //活跃的匿名内存 Active(anon);
                             //  public long Inactive_anon; //不活跃的匿名内存Inactive(anon);
                             //   public long Active_file;   //活跃的文件使用内存Active(file)
                             //     public long Inactive_file;   //不活跃的文件使用内存Inactive(file);
                             //  public long Unevictable; //不能被释放的内存页
                             //  public long Mlocked;   //系统调用 mlock 家族允许程序在物理内存上锁住它的部分或全部地址空间。这将阻止Linux 将这个内存页调度到交换空间（swap space），即使该程序已有一段时间没有访问这段空间
        /// <summary>
        /// 交换空间总内存
        /// </summary>
        public long SwapTotal;  //交换空间总内存
        /// <summary>
        /// 交换空间空闲内存
        /// </summary>
        public long SwapFree;   //交换空间空闲内存
                                //   public long Dirty;    //等待被写回到磁盘的
                                //  public long Writeback;   //正在被写回的
                                //   public long AnonPages;   //未映射页的内存/映射到用户空间的非文件页表大小
                                //   public long Mapped;  //映射文件内存
                                //  public long Shmem;   //已经被分配的共享内存
                                //   public long Slab;   //内核数据结构缓存
                                //   public long SReclaimable;   //可收回slab内存
                                //    public long SUnreclaim;   //不可收回slab内存
                                //    public long KernelStack;    //内核消耗的内存
                                //   public long PageTables;   //管理内存分页的索引表的大小
                                //   public long NFS_Unstable;   //不稳定页表的大小
                                // public long Bounce;   //在低端内存中分配一个临时buffer作为跳转，把位于高端内存的缓存数据复制到此处消耗的内存
                                //   public long WritebackTmp;   //FUSE用于临时写回缓冲区的内存
                                //   public long CommitLimit;    //系统实际可分配内存
        /// <summary>
        /// 系统当前已分配的内存
        /// </summary>
        //  public long Committed_AS;    //系统当前已分配的内存
        public long VmallocTotal;  //预留的虚拟内存总量
        public long VmallocUsed;    //已经被使用的虚拟内存
        public long VmallocChunk;   //可分配的最大的逻辑连续的虚拟内存
        /// <summary>
        ///内存使用率
        /// </summary>
        /// <returns></returns>
        public int UsageRate()
        {
            //MemAvailable= MemFree + Active_file + Inactive_file - (watermark + min(watermark, Active(file) + Inactive(file) / 2))
            if (MemTotal == 0)
                return 0;
            return (int)((MemTotal - MemAvailable) * 100 / MemTotal);
        }

        public int MemTotalMB()
        {
            return (int)(MemTotal / 1024/1024);
        }

        public override string ToString()
        {

            return $"总计物理内存大小:{MemTotal},可用物理内存大小:{MemAvailable},交换空间总内存:{SwapTotal},交换空间空闲内存:{SwapFree} 使用率:{UsageRate()}";
        }
    }

}
