﻿using System;
using System.IO;

namespace PasS.Base.Lib
{
    static class CPULinuxLoadValue
    {
        private static CPU_OCCUPY previous_cpu_occupy = null;
        private static readonly object syncobj = new object();

        private class CPU_OCCUPY
        {
            public string name;
            /// <summary>
            /// 用户态时间
            /// </summary>
            public long user;
            /// <summary>
            /// 漂亮的用户态时间
            /// </summary>
            public long nice;
            /// <summary>
            /// 系统 内核态时间
            /// </summary>
            public long system;
            /// <summary>
            /// 空闲时间(不包含IO等待时间)
            /// </summary>
            public long idle;
            /// <summary>
            /// I / O等待时间
            /// </summary>
            public long lowait;
            /// <summary>
            /// 硬中断时间
            /// </summary>
            public long irq;
            /// <summary>
            /// 软中断时间
            /// </summary>
            public long softirq;
            /// <summary>
            /// 虚拟化环境中运行其他操作系统上花费的时间
            /// </summary>
            public long steal;
            /// <summary>
            /// 操作系统运行虚拟CPU花费的时间
            /// </summary>
            public long guest;
            /// <summary>
            /// 运行一个带nice值的guest花费的时间
            /// </summary>
            public long guest_nice;
        }

        public static double CPULOAD { get; private set; }

        public static void Refresh()
        {
            CPULOAD = QUERY_CPULOAD();

        }

        private static double QUERY_CPULOAD(bool a = true)
        {

            lock (syncobj)
            {
                CPU_OCCUPY current_cpu_occupy = get_cpuoccupy();
                if (current_cpu_occupy == null || previous_cpu_occupy == null)
                {
                    previous_cpu_occupy = current_cpu_occupy;
                    return 0;
                }
                try
                {
                    long od = (previous_cpu_occupy.user + previous_cpu_occupy.nice + previous_cpu_occupy.system + previous_cpu_occupy.idle + previous_cpu_occupy.lowait + previous_cpu_occupy.irq + previous_cpu_occupy.softirq + previous_cpu_occupy.steal + previous_cpu_occupy.guest);//第一次(用户+优先级+系统+空闲)的时间再赋给od
                    long nd = (current_cpu_occupy.user + current_cpu_occupy.nice + current_cpu_occupy.system + current_cpu_occupy.idle + current_cpu_occupy.lowait + current_cpu_occupy.irq + current_cpu_occupy.softirq + current_cpu_occupy.steal + current_cpu_occupy.guest);//第二次(用户+优先级+系统+空闲)的时间再赋给od

                    double sum = nd - od;
                    double idle = current_cpu_occupy.idle - previous_cpu_occupy.idle;
                    double cpu_use = (sum - idle) / sum;

                    if (!a)
                    {
                        idle = current_cpu_occupy.user + current_cpu_occupy.system + current_cpu_occupy.nice - previous_cpu_occupy.user - previous_cpu_occupy.system - previous_cpu_occupy.nice;
                        cpu_use = (sum - idle) / sum;
                    }

                    cpu_use = (cpu_use * 100);/// Environment.ProcessorCount;
                    return cpu_use;
                }
                finally
                {
                    previous_cpu_occupy = current_cpu_occupy;
                }
            }
        }

        private static string ReadArgumentValue(StreamReader sr)
        {
            string values = null;
            if (sr != null)
            {
                while (!sr.EndOfStream)
                {
                    char ch = (char)sr.Read();
                    if (ch == ' ')
                    {
                        if (values == null)
                        {
                            continue;
                        }
                        break;
                    }
                    values += ch;
                }
            }
            return values;
        }

        private static long ReadArgumentValueInt64(StreamReader sr)
        {
            string s = ReadArgumentValue(sr);
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            long r;
            long.TryParse(s, out r);
            return r;
        }

        private static CPU_OCCUPY get_cpuoccupy()
        {
            string path = "/proc/stat";
            if (!File.Exists(path))
            {
                return null;
            }
            FileStream stat = null;
            try
            {
                stat = File.OpenRead(path);
                if (stat == null)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            using (StreamReader sr = new StreamReader(stat))
            {
                CPU_OCCUPY occupy = new CPU_OCCUPY();
                try
                {
                    occupy.name = ReadArgumentValue(sr);
                    occupy.user = ReadArgumentValueInt64(sr);
                    occupy.nice = ReadArgumentValueInt64(sr);
                    occupy.system = ReadArgumentValueInt64(sr);
                    occupy.idle = ReadArgumentValueInt64(sr);
                    occupy.lowait = ReadArgumentValueInt64(sr);
                    occupy.irq = ReadArgumentValueInt64(sr);
                    occupy.softirq = ReadArgumentValueInt64(sr);
                    occupy.steal = ReadArgumentValueInt64(sr);
                    occupy.guest = ReadArgumentValueInt64(sr);
                    occupy.guest_nice = ReadArgumentValueInt64(sr);
                }
                catch (Exception)
                {
                    return null;
                }
                return occupy;
            }
        }

        public static bool GlobalMemoryStatus(ref SystemEnvironment.MEMORY_INFO mi)
        {
            string path = "/proc/meminfo";
            if (!File.Exists(path))
            {
                return false;
            }
            FileStream stat = null;
            try
            {
                stat = File.OpenRead(path);
                if (stat == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            long? call(string line, string key)
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
                if (lk != key)
                {
                    return null;
                }
                line = line.Substring(i + 1).TrimStart();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }
                string[] sp = line.Split(' ');
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
                return n * 1024;
            }
            using (StreamReader sr = new StreamReader(stat))
            {
                try
                {
                    int counts = 0;
                    string line = string.Empty;
                    while (counts < 2 && !string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        long? value = call(line, "MemTotal");
                        if (value != null)
                        {
                            counts++;
                            mi.ullTotalPhys = value.Value;
                            continue;
                        }
                        value = call(line, "MemAvailable");
                        if (value != null)
                        {
                            counts++;
                            mi.ullAvailPhys = value.Value;
                            continue;
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }

 
}
