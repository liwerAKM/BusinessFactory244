using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PasS.Base.Lib;
using PasS.Base.Lib.DAL;
using PasS.Base.Lib.Model;

namespace BusinessInterface
{
    /// <summary>
    /// 业务处理适配器
    /// </summary>
    public class ProcessingBusinessAccess
    {

        //LoadFrom Load 区别 https://www.cnblogs.com/zagelover/articles/2726034.html

        public static bool UseVersion = true;
        /// <summary>
        /// 本程序运行的路径，不包括可执行文件名
        /// </summary>
      //  public static string StartupPath = Environment.CurrentDirectory;
        public static string StartupPath =     AppContext.BaseDirectory.TrimEnd('\\').TrimEnd('/');
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Task_info"></param>
        /// <returns></returns>
        public static ProcessingBusinessBase CreateProcessingBusiness(BusinessInfoBusVersion busnessInfo)
        {
            Version version = null;
            Version.TryParse(busnessInfo.version, out version);
            object objType = CreateInstanceNew(busnessInfo, version);
            if (objType != null)
                return (ProcessingBusinessBase)objType;
            else
                return null;

        }
        private static Hashtable _transmit_tb = new Hashtable();

        private static ConcurrentDictionary<string, Assembly> cdAss = new ConcurrentDictionary<string, Assembly>();


        public static void FreeAssembly()
        {
            foreach (string key in cdAss.Keys)
            {
                Assembly assembly;
                cdAss.TryRemove(key, out assembly);
                assembly = null;
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(string.Format("{0} Remove All IProcessingBusiness!!!", DateTime.Now));
            Console.ForegroundColor = ConsoleColor.White;
            PasSLog.Info("ProcessingBusinessAccess.FreeAssembly", string.Format("{0} Remove All IProcessingBusiness!!!", DateTime.Now));
        }
        //public static void FreeAssembly(BusinessInfo busnessInfo)
        //{

        //    FreeAssembly(busnessInfo.BusID, busnessInfo.version);
        //}
        public static void FreeAssembly(BusinessInfoBusVersion busnessInfo)
        {

            FreeAssembly(busnessInfo.BusID + "^" + busnessInfo.BusVersion, busnessInfo.version);
        }
        public static void FreeAssembly(string BusID, string BusVersion, string version)
        {
            FreeAssembly(BusID + "^" + BusVersion, version);
        }
        public static void FreeAssembly(string BusKey, string version)
        {
            string Key = BusKey + "_" + version;
            if (cdAss.ContainsKey(Key))
            {
                lock (cdAss)
                {
                    Assembly assembly;
                    cdAss.TryRemove(Key, out assembly);
                    assembly = null;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(string.Format("{0} Remove IProcessingBusiness:{1}  {2}   ", DateTime.Now, BusKey, version));
                    Console.ForegroundColor = ConsoleColor.White;
                    PasSLog.Info("ProcessingBusinessAccess.FreeAssembly", string.Format("{0} Remove IProcessingBusiness:{1}  {2}   ", DateTime.Now, BusKey, version));
                }
            }
        }
        //public static void FreeAssembly(BusinessInfo busnessInfo,  Version version)
        //{
        //    FreeAssembly(busnessInfo.BusID, version.ToString());
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Task_info"></param>
        ///// <returns></returns>
        //public static object CreateInstance(BusinessInfo busnessInfo)
        //{

        //    if (_transmit_tb.Contains(busnessInfo.BusID))
        //    {
        //        Assembly assembly = (Assembly)_transmit_tb[busnessInfo.BusID];
        //        return assembly.CreateInstance(busnessInfo.NamespaceClass);
        //    }
        //    else
        //    {
        //        Assembly assembly = Assembly.LoadFrom(StartupPath+ "\\" + busnessInfo.DllName);

        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //        Console.WriteLine(string.Format("Create IProcessingBusiness:{0}  {1}   ", busnessInfo.DllName, busnessInfo.BusID));
        //        Console.ForegroundColor = ConsoleColor.White;
        //        MySpringLog.Info("ProcessingBusinessAccess.CreateInstance", string.Format("Create IProcessingBusiness:{0}  {1}   ", busnessInfo.DllName, busnessInfo.BusID));
        //        if (!_transmit_tb.Contains(busnessInfo.BusID))
        //        {
        //            _transmit_tb.Add(busnessInfo.BusID, assembly);
        //        }
        //        return assembly.CreateInstance(busnessInfo.NamespaceClass);
        //    }
        //}
        public static object CreateInstanceNew(BusinessInfoBusVersion busnessInfo, Version version)
        {
            if (UseVersion && version != null)
            {
                string Key = busnessInfo.BusID + "^" + busnessInfo.BusVersion + "_" + version.ToString();
                if (cdAss.ContainsKey(Key))
                {
                    Assembly assembly = cdAss[Key];
                    return assembly.CreateInstance(busnessInfo.NamespaceClass);
                }
                else
                {
                    lock (cdAss)
                    {
                        Assembly assembly = null;
                        string VerFile = StartupPath + busnessInfo.DllPath + busnessInfo.DllName;//.Replace(".dll", "_" + version.ToString() + ".dll");
                        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            VerFile = VerFile.Replace('\\', '/');
                        }
                        Console.WriteLine(string.Format("CreateInstance:{0}  {1} File:{2}  ", busnessInfo.BusName, Key  , VerFile));
                        if (File.Exists(VerFile))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(string.Format("CreateInstance:{0}  {1} File. Exists{2}  ", busnessInfo.DllName, Key, VerFile));
                            if (busnessInfo.DllPath.Trim() == "\\")
                            {
                                byte[] fileData = File.ReadAllBytes(VerFile);
                                assembly = Assembly.Load(fileData);
                            }
                            else
                            {
                                assembly = Assembly.LoadFrom(VerFile);
                            }

                        }
                        else if (DownLoadFile(busnessInfo.ProjectID, busnessInfo.DllPath, busnessInfo.DllName, version.ToString(), busnessInfo.BusID))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(string.Format("CreateInstance:{0}   {1} File. Not {2}  ", busnessInfo.DllName, Key, VerFile));

                            if (busnessInfo.DllPath.Trim() == "\\")
                            {
                                byte[] fileData = File.ReadAllBytes(VerFile);
                                assembly = Assembly.Load(fileData);
                            }
                            else
                            {
                                assembly = Assembly.LoadFrom(VerFile);
                            }
                        }


                        if (assembly != null)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine(string.Format("{0} Create IProcessingBusiness Assembly OK:{1}  {2}   ", DateTime.Now, busnessInfo.DllName.Replace(".dll", "_" + version.ToString() + ".dll"), Key));
                            Console.ForegroundColor = ConsoleColor.White;
                            PasSLog.Info("ProcessingBusinessAccess.CreateInstanceNew", string.Format("{0} Create IProcessingBusiness:{1}  {2}   ", DateTime.Now, busnessInfo.DllName.Replace(".dll", "_" + version.ToString() + ".dll"), Key));
                            if (!cdAss.ContainsKey(Key))
                            {
                                cdAss.TryAdd(Key, assembly);
                            }
                            return assembly.CreateInstance(busnessInfo.NamespaceClass);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            else
            {
                string Key = busnessInfo.BusID + "^" + busnessInfo.BusVersion;
                if (cdAss.ContainsKey(Key))
                {
                    Assembly assembly = cdAss[busnessInfo.BusID];
                    return assembly.CreateInstance(busnessInfo.NamespaceClass);
                }
                else
                {
                    Assembly assembly = Assembly.LoadFrom(StartupPath + busnessInfo.DllPath + busnessInfo.DllName);
                    var ver = assembly.GetName().Version;
                    if (version == null)
                    {
                        busnessInfo.version = ver.ToString();
                        //  DbHelper.BusinessInfoSet(busnessInfo);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(string.Format("Create IProcessingBusiness:{0}  {1}   ", busnessInfo.DllName, Key));
                    Console.ForegroundColor = ConsoleColor.White;
                    PasSLog.Info("ProcessingBusinessAccess.CreateInstanceNew", string.Format("Create IProcessingBusiness:{0}  {1}   ", busnessInfo.DllName, Key));
                    if (!cdAss.ContainsKey(Key))
                    {
                        cdAss.TryAdd(Key, assembly);
                    }
                    return assembly.CreateInstance(busnessInfo.NamespaceClass);
                }
            }
        }

        private static string GetPublicKeyTokenFromAssembly(Assembly assembly)
        {
            var bytes = assembly.GetName().GetPublicKeyToken();
            if (bytes == null || bytes.Length == 0)
                return string.Empty;
            var publicKeyToken = string.Empty;
            for (int i = 0; i < bytes.GetLength(0); i++)
                publicKeyToken += string.Format("{0:x2}", bytes[i]);
            return publicKeyToken;
        }

        public static bool DownLoadFile(BusinessInfo busnessInfo)
        {

            return DownLoadFile(busnessInfo.ProjectID, busnessInfo.DllPath, busnessInfo.DllName, busnessInfo.version, busnessInfo.BusID);

        }
        public static bool DownLoadFile(BusinessInfoBusVersion busnessInfo)
        {

            return DownLoadFile(busnessInfo.ProjectID, busnessInfo.DllPath, busnessInfo.DllName, busnessInfo.version, busnessInfo.BusID);

        }

        public static bool DownLoadFile(string where)
        {
            Fileinfo dalFileinfo = new Fileinfo();

            List<fileinfo> list = SpringAPI.FileinfoGetModelWhere(where, MyPubConstant.BusServerID);
            if (list != null)
            {
                foreach (fileinfo file in list)
                {
                    DownLoadFileR(file.projectID, file.FilePath, file.Filename, file.Version);
                }
            }
            return true;


        }
        public static bool DownLoadFile(string ProjectID, string FilePath, string FileName, string versio, string BusID)
        {
            //先下载依赖文件，以保证稍后下载的业务文件能访问正确的依赖
            decimal VersionN = DbHelper.VersionTodecimal(versio);
            DataTable dtDependentFile = SpringAPI.DependentFileGet(BusID, VersionN);
            if (dtDependentFile != null && dtDependentFile.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDependentFile.Rows)
                {
                    if (!DownLoadFileR(dr["ProjectID"].ToString(), dr["FilePath"].ToString(), dr["Filename"].ToString(), dr["version"].ToString()))
                    {
                        return false;
                    }
                }
            }
            return DownLoadFileR(ProjectID, FilePath, FileName, versio);//下载真正的业务文件
        }
        public static bool DownLoadFileR(string ProjectID, string FilePath, string FileName, string versio)
        {


            fileinfo filnfo = SpringAPI.FileinfoGetModel(ProjectID, FilePath, FileName, versio);
            if (filnfo == null)
                return false;
            string VerFile = StartupPath + filnfo.FilePath + filnfo.Filename;
            string VerFileBack = StartupPath + "\\FileBack" + filnfo.FilePath + filnfo.Filename;
            if (filnfo.isdirect)
            {
                VerFile = StartupPath + FilePath + FileName.Replace(".dll", "_" + filnfo.Version + ".dll");
                VerFileBack = StartupPath + "\\FileBack" + filnfo.FilePath + FileName.Replace(".dll", "_" + filnfo.Version + ".dll");
            }
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                VerFile = VerFile.Replace('\\', '/');
                VerFileBack = VerFileBack.Replace('\\', '/');
            }
            try
            {

                if (File.Exists(VerFile))
                {
                    //if (!Directory.Exists(StartupPath + "\\FileBack"))
                    //{
                    //    Directory.CreateDirectory(StartupPath + "\\FileBack");
                    //}
                    string FileBack = StartupPath + "\\FileBack" + filnfo.FilePath.TrimEnd('\\');
                    if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        FileBack = FileBack.Replace('\\', '/');  
                    }
                    if (!Directory.Exists(FileBack))
                    {
                        Directory.CreateDirectory(FileBack);
                    }

                    if (File.Exists(VerFileBack))
                    {
                        try
                        {
                            File.Delete(VerFileBack);
                            File.Move(VerFile, VerFileBack);
                        }
                        catch//备份文件也被使用，只能重新命名备份
                        {
                            File.Move(VerFile, VerFileBack + DateTime.Now.ToString());
                        }
                    }
                    else
                    {
                        File.Move(VerFile, VerFileBack);
                    }

                }
                string sFilePath = StartupPath + filnfo.FilePath.TrimEnd('\\');
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    sFilePath = sFilePath.Replace('\\', '/');
                }
                if (!Directory.Exists(sFilePath))
                {
                    try
                    {
                        Directory.CreateDirectory(sFilePath);
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("无法访问路径" + sFilePath + ".");
                        PasSLog.Info("ProcessingBusinessAccess.DownLoadFileR", "无法访问路径" + sFilePath + ".");

                        return false;
                    }
                }
                filnfo = SpringAPI.GetModelandImage(ProjectID, FilePath, FileName, versio);
                FileStream fs = new FileStream(VerFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(filnfo.images, 0, filnfo.images.Length);
                fs.Close();
                fs = null;
                FileInfo FileInf = new FileInfo(VerFile);
                FileInf.LastWriteTime = filnfo.MTime;
                FileInf.Refresh();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("更新文件" + VerFile + "成功。");
                PasSLog.Info("ProcessingBusinessAccess.DownLoadFileR", "更新文件" + VerFile + "成功。");
                if (filnfo.isdirect)//对于有版本标识直接被调用的，也尝试下载无版本号文件
                {
                    try
                    {
                        DownLoadFileNoVersion(ProjectID, FilePath, FileName, versio);
                    }
                    catch
                    {
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("更新文件" + VerFile + "出错:" + ex.Message);
                PasSLog.Info("ProcessingBusinessAccess.DownLoadFileR", "更新文件" + VerFile + "出错:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 下载指定文件（不会对文件重命名）
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <param name="versio"></param>
        /// <returns></returns>
        public static bool DownLoadFileNoVersion(string ProjectID, string FilePath, string FileName, string versio)
        {


            fileinfo filnfo = SpringAPI.FileinfoGetModel(ProjectID, FilePath, FileName, versio);
            if (filnfo == null)
                return false;
            string VerFile = StartupPath + filnfo.FilePath + filnfo.Filename;
            string VerFileBack = StartupPath + "\\FileBack" + filnfo.FilePath + filnfo.Filename;
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                VerFile = VerFile.Replace('\\', '/');
                VerFileBack = VerFileBack.Replace('\\', '/');
            }
            try
            {

                if (File.Exists(VerFile))
                {
                    string FileBack = StartupPath + "\\FileBack" + filnfo.FilePath.TrimEnd('\\');
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        FileBack = FileBack.Replace('\\', '/');
                    }
                    if (!Directory.Exists(FileBack))
                    {
                        Directory.CreateDirectory(FileBack);
                    }
                    if (File.Exists(VerFileBack))
                    {
                        try
                        {
                            File.Delete(VerFileBack);
                            File.Move(VerFile, VerFileBack);
                        }
                        catch//备份文件也被使用，只能重新命名备份
                        {
                            File.Move(VerFile, VerFileBack + DateTime.Now.ToString());
                        }
                    }
                    else
                    {
                        File.Move(VerFile, VerFileBack);
                    }

                }
                string sFilePath = StartupPath + filnfo.FilePath.TrimEnd('\\');
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    sFilePath = sFilePath.Replace('\\', '/');
                }
                if (!Directory.Exists(sFilePath))
                {
                    try
                    {
                        Directory.CreateDirectory(sFilePath);
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("无法访问路径" + sFilePath + ".");
                        PasSLog.Info("ProcessingBusinessAccess.DownLoadFileNoVersion", "无法访问路径" + sFilePath + ".");
                        return false;
                    }
                }
                filnfo = SpringAPI.GetModelandImage(ProjectID, FilePath, FileName, versio);
                FileStream fs = new FileStream(VerFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(filnfo.images, 0, filnfo.images.Length);
                fs.Close();
                fs = null;
                FileInfo FileInf = new FileInfo(VerFile);
                FileInf.LastWriteTime = filnfo.MTime;
                FileInf.Refresh();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("更新文件" + VerFile + "成功。");
                PasSLog.Info("ProcessingBusinessAccess.DownLoadFileNoVersion", "更新文件" + VerFile + "成功。");
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("更新文件" + VerFile + "出错:" + ex.Message);
                PasSLog.Info("ProcessingBusinessAccess.DownLoadFileNoVersion", "更新文件" + VerFile + "出错:" + ex.ToString());
                return false;
            }

        }

    }
}
