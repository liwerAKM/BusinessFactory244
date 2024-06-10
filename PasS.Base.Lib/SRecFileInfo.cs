using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 接收到的文件及信息
    /// </summary>
    public class SRecFileInfo
    {
        public SRecFileInfo()
        {
        }
        public SRecFileInfo(FileInfoHead fileInfoHead)
        {
            this.TID = fileInfoHead.TID;
            this.CreateTime = fileInfoHead.CreateTime;
            this.LastWriteTime = fileInfoHead.LastWriteTime;
            this.FileName = fileInfoHead.FileName;
            this.FilePath = fileInfoHead.FilePath;
        }
        public SRecFileInfo(SOMCandClientFileInfo fileInfoHead)
        {
            this.CreateTime = fileInfoHead.CreateTime;
            this.LastWriteTime = fileInfoHead.LastWriteTime;
            this.FileName = fileInfoHead.FileName;
            this.FilePath = fileInfoHead.FilePath;
        }
        /// <summary>
        /// 请求唯一ID
        /// </summary>
        public string TID { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后写入时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        ///文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        ///文件相对路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件的二进制
        /// </summary>
        public byte[] images { get; set; }

        public bool SaveFile(string StartupPath, string CallFunction = "")
        {
            string VerFile = Path.Combine(StartupPath, FilePath, FileName);
            string VerFileBack = Path.Combine(StartupPath, "FileBack", FilePath, FileName);
            try
            {

                if (File.Exists(VerFile))
                {

                    if (!Directory.Exists(Path.Combine(StartupPath, "FileBack", FilePath)))
                    {
                        Directory.CreateDirectory(Path.Combine(StartupPath, "FileBack", FilePath));
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

                if (!Directory.Exists(Path.Combine(StartupPath, FilePath)))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(StartupPath, FilePath));
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("无法访问路径" + Path.Combine(StartupPath, FilePath) + ".");
                        PasSLog.Error(CallFunction + ".SRecFileInfo", "无法访问路径" + Path.Combine(StartupPath, FilePath) + ".");
                        return false;
                    }
                }

                FileStream fs = new FileStream(VerFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fs.Write(images, 0, images.Length);
                fs.Close();
                fs = null;
                FileInfo FileInf = new FileInfo(VerFile);
                FileInf.LastWriteTime = LastWriteTime;
                FileInf.Refresh();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("更新文件" + VerFile + "成功。");
                PasSLog.Info(CallFunction + ".SRecFileInfo", "更新文件" + VerFile + "成功。");
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("更新文件" + VerFile + "出错:" + ex.Message);
                PasSLog.Error(CallFunction + ".SRecFileInfo", "更新文件" + VerFile + "出错:" + ex.ToString());
                return false;
            }

        }
    }
}
