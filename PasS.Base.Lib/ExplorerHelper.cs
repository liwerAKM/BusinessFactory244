using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 资源管理器相关代码功能
    /// </summary>
    public class ExplorerHelper
    {


        /// <summary>
        /// 返回当前计算机中逻辑驱动器名称的数组
        /// </summary>
        /// <returns></returns>

        public static List<MyDriveInfo> GetDriveInfo()
        {  //返回当前计算机中逻辑驱动器名称的数组
            List<MyDriveInfo> lsitDriveInfo = new List<MyDriveInfo>();
            DriveInfo[] Drivers = DriveInfo.GetDrives();
            foreach (DriveInfo driver in Drivers)
            {
                try
                {
                    MyDriveInfo drinfo = new MyDriveInfo(driver);
                    lsitDriveInfo.Add(drinfo);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ExplorerHelper GetDriveInfo Error:" + ex.Message );
                }
            }
            return lsitDriveInfo;
        }
        /// <summary>
        /// 获取指定路径下的文件和文件夹信息
        /// </summary>
        /// <param name="newPath"></param>
        /// <returns></returns>
        public static MyDirectoryFileInfoS GetDirectoryFileInfo(string CurrentPath)
        {
            MyDirectoryFileInfoS dfs = new MyDirectoryFileInfoS();
            dfs.CurrentPath = CurrentPath;
            dfs.listMyDirFileInfo = new List<MyDirectoryFileInfo>();
            if (Directory.Exists(CurrentPath))
            {
                DirectoryInfo currentDir = new DirectoryInfo(CurrentPath);
                DirectoryInfo[] dirs = new DirectoryInfo[0];
                FileInfo[] files = new FileInfo[0];
                dirs = currentDir.GetDirectories(); //获取目录
                files = currentDir.GetFiles(); //获取文件
                List<MyDirectoryFileInfo> listMyDirFileInfo = new List<MyDirectoryFileInfo>();
                foreach (DirectoryInfo dir in dirs)
                {
                    try
                    {
                        MyDirectoryFileInfo myDirectoryFile = new MyDirectoryFileInfo(dir);
                        dfs.listMyDirFileInfo.Add(myDirectoryFile);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ExplorerHelper GetDirectoryFileInfo  DirectoryInfo Error:" + ex.Message);
                    }
                }
                foreach (FileInfo file in files)
                {
                    try
                    {
                        MyDirectoryFileInfo myDirectoryFile = new MyDirectoryFileInfo(file);
                    dfs.listMyDirFileInfo.Add(myDirectoryFile);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ExplorerHelper GetDirectoryFileInfo  FileInfo Error:" + ex.Message);
                    }
                }
                dirs = null;
                files = null;
            }
            return dfs;

        }
        /// <summary>
        /// 运行程序、打开文件
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        public static bool OpenRunF(string FullName)
        {
            if (File.Exists(FullName))
            {
                try
                {
                    Process.Start(FullName);
                    return true;
                }
                catch//备份文件也被使用，只能重新命名备份
                { 
                }
            }
            return false;
        }
        
            
        public static bool  SaveFileInfo(MyDirectoryFileInfo dragFileInfo, byte[] FileInfo)
        {
            if (dragFileInfo.isF == 1)
            {
                if (!Directory.Exists(dragFileInfo.FullName))
                {
                    Directory.CreateDirectory(dragFileInfo.FullName);
                }
            }
            else
            {
                string VerFile = dragFileInfo.FullName;
                try
                {

                    if (File.Exists(VerFile))
                    {
                        try
                        {
                            File.Delete(VerFile);

                        }
                        catch//备份文件也被使用，只能重新命名备份
                        {
                            File.Move(VerFile, VerFile + DateTime.Now.ToString());
                        }
                    }
                    FileStream fs = new FileStream(VerFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fs.Write(FileInfo, 0, FileInfo.Length);
                    fs.Close();
                    fs = null;
                    FileInfo FileInf = new FileInfo(VerFile);
                    FileInf.LastWriteTime = dragFileInfo.LastWriteTimeUtc;
                    FileInf.Refresh();

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("ExplorerHelper 保存文件 " + VerFile + "成功。");
                    PasSLog.Info("ExplorerHelper", "保存文件" + VerFile + "成功。");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("ExplorerHelper 保存文件" + VerFile + "出错:" + ex.Message);
                    PasSLog.Error("ExplorerHelper", "保存文件" + VerFile + "出错:" + ex.ToString());
                    return false;
                }


            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="DTYPE">1 创建 2;重命名</param>
        /// <returns></returns>
        public static bool CreateReName(CreateRenameFileInfo fileInfo,int DTYPE)
        {
            try
            {
                if (DTYPE == 2)
                {
                    if (fileInfo.isF == 1)
                    {
                        if (Directory.Exists(fileInfo.OldFullName))
                        {
                            Directory.Move(fileInfo.OldFullName, fileInfo.NewFullName);
                            return true;
                        }
                    }
                    else
                    {
                        if (File.Exists(fileInfo.OldFullName))
                        {
                            File.Move(fileInfo.OldFullName, fileInfo.NewFullName);
                            return true;
                        }
                    }
                }
                else if (DTYPE == 1)
                {
                    if (fileInfo.isF == 1)
                    {
                        if (!Directory.Exists(fileInfo.NewFullName))
                        {
                            Directory.CreateDirectory(fileInfo.NewFullName);
                            return true;
                        }
                    }
                    else
                    {
                        if (!File.Exists(fileInfo.NewFullName))
                        {
                            File.Create(fileInfo.NewFullName).Close(); ;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("CreateReName  " + fileInfo.NewFullName + "出错:" + ex.Message);
                PasSLog.Error("CreateReName", " " + fileInfo.NewFullName + "出错:" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dragFileInfoS"></param>
        /// <param name="DTYPE">1 复制 2 剪切 3 删除</param>
        /// <returns></returns>
        public static bool CopyMoveDelFileinfo(DragFileInfoS dragFileInfoS,int DTYPE )
        {
            if (dragFileInfoS != null && dragFileInfoS.dragFileInfos.Count > 0)
            {
                if (DTYPE == 1 || DTYPE == 2)
                {
                    if (dragFileInfoS.TargetPath == null || !Directory.Exists(dragFileInfoS.TargetPath))
                    {
                        return false;
                    }
                }
                foreach (MyDirectoryFileInfo fileInfo in dragFileInfoS.dragFileInfos)
                {
                    try
                    {
                        if (fileInfo.isF == 1)
                        {
                            if (Directory.Exists(fileInfo.FullName))
                            {
                                if (DTYPE == 1)
                                    CopyDirectory(fileInfo.FullName, Path.Combine(dragFileInfoS.TargetPath, fileInfo.Name));
                                else if (DTYPE == 2)
                                {
                                    CopyDirectory(fileInfo.FullName, Path.Combine(dragFileInfoS.TargetPath, fileInfo.Name));
                                    Directory.Delete(fileInfo.FullName, true);
                                }
                                else if (DTYPE == 3)
                                    Directory.Delete(fileInfo.FullName,true );
                            }
                        }
                        else
                        {
                            if (File.Exists(fileInfo.FullName))
                            {
                                if (DTYPE == 1)
                                    File.Copy (fileInfo.FullName, Path.Combine(dragFileInfoS.TargetPath, fileInfo.Name));
                                else if (DTYPE == 2)
                                    File.Move(fileInfo.FullName, Path.Combine(dragFileInfoS.TargetPath, fileInfo.Name));
                                else if (DTYPE == 3)
                                    File.Delete(fileInfo.FullName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("CopyMoveDelFileinfo  " + fileInfo.FullName + "出错:" + ex.Message);
                        PasSLog.Error("CopyMoveDelFileinfo", " " + fileInfo.FullName + "出错:" + ex.ToString());
                    }
                }
            }
            return true;

        }

        public static void CopyDirectory(string srcPath, string destPath)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
                string patfg = "\\";
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    patfg =  "/";
                }
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)     //判断是否文件夹
                    {
                        if (!Directory.Exists(destPath + patfg + i.Name))
                        {
                            Directory.CreateDirectory(destPath + patfg + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                        }
                        CopyDirectory(i.FullName, destPath + patfg + i.Name);    //递归调用复制子文件夹
                    }
                    else
                    {
                        File.Copy(i.FullName, destPath + patfg + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public static string FileSize(long size)
        {
            double newsize = double.Parse(size.ToString());
            string returnStr = newsize > 1024
                ? ((newsize / 1024) > 1024
                    ? ((newsize / 1024 / 1024) > 1024 ? (((newsize / 1024 / 1024 / 1024).ToString("F") + " G")) : ((newsize / 1024 / 1024).ToString("F") + " M"))
                    : ((newsize / 1024).ToString("F") + " KB"))
                : (newsize + " B");
            return returnStr;
        }

    }
    /// <summary>
    /// 磁盘信息
    /// </summary>
    public class MyDriveInfo
    {
        public MyDriveInfo()
        {
        }
        public MyDriveInfo(DriveInfo driveInfo)
        {
            Name = driveInfo.Name;
            RootDir_FullName = driveInfo.RootDirectory.FullName;
            DriveType = driveInfo.DriveType;
            IsReady = driveInfo.IsReady;
            if (driveInfo.IsReady)
            {
                TotalSize = driveInfo.TotalSize;
                TotalFreeSpace = driveInfo.TotalFreeSpace;
                VolumeLabel = driveInfo.VolumeLabel;
                DriveFormat = driveInfo.DriveFormat;
            }

        }
        public string Name;

        public string RootDir_FullName;

        public DriveType DriveType;
        public long TotalSize;
        public long TotalFreeSpace;
        public bool IsReady;
        public string VolumeLabel;
        public string DriveFormat;
    }

    public class  MyDirectoryFileInfoS
    {
       public  List<MyDirectoryFileInfo> listMyDirFileInfo = new List<MyDirectoryFileInfo>();
        /// <summary>
        /// 当前路径
        /// </summary>
        public string CurrentPath { get; set; }
    }
    /// <summary>
    /// 文件和文件夹信息
    /// </summary>
    public class MyDirectoryFileInfo
    {
        public MyDirectoryFileInfo()
        {
        }
        public MyDirectoryFileInfo(DirectoryInfo dir)
        {
            isF = 1;
            Name = dir.Name;
            Attributes = dir.Attributes;
            FullName = dir.FullName;
            LastWriteTimeUtc = dir.LastWriteTimeUtc;
            Attributes = dir.Attributes;
        }

        public MyDirectoryFileInfo(FileInfo file)
        {
            isF = 0;
            Name = file.Name;
            Attributes = file.Attributes;
            FullName = file.FullName;
            LastWriteTimeUtc = file.LastWriteTimeUtc;
            Extension = file.Extension;
            Length = file.Length;
        }
        /// <summary>
        /// 是否是文件夹 0 不是 1 是
        /// </summary>
        public int isF { get; set; }
        public string Name { get; set; }

        public FileAttributes Attributes { get; set; }
        /// <summary>
        /// 扩展名  
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FullName { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }

        public long Length;

        /// <summary>
        /// 目标路径(监控端从远处电脑获取文件或文件夹时必填)
        /// </summary>
        public string TP{ get; set; }
        
    }

    public class DragFileInfoS
    {
        /// <summary>
        /// 是否是从本地选择，否则是从远端选择
        /// </summary>
        public bool FromLocal = true;
        public List<MyDirectoryFileInfo> dragFileInfos = new List<MyDirectoryFileInfo>();
        /// <summary>
        /// 目标路径
        /// </summary>
        public string TargetPath { get; set; }
    }
    public class DragFileInfo
    {


        /// <summary>
        /// 是否是文件夹 0 不是 1 是
        /// </summary>
        public int isF { get; set; }
        public string Name;

        public string FullName { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }

        public long Length;

        /// <summary>
        /// 扩展名  
        /// </summary>
        public string Extension { get; set; }
    }
    /// <summary>
    /// 重命名或者新建文件(夹)
    /// </summary>
    public class CreateRenameFileInfo
    {
        /// <summary>
        /// 是否是文件夹 0 不是 1 是
        /// </summary>
        public int isF { get; set; }
        public string OldFullName;
        public string NewFullName { get; set; }

      
    }
}
