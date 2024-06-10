using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 文件操作类
    /// </summary>
  public   class FileHelper
    {
        /// <summary>
        /// 读文件到二进制
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        public static   byte[] ReadFile(string FullName)
        {
            if (File.Exists(FullName))
            {
                using (FileStream fs = new FileStream(FullName, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader r = new BinaryReader(fs);
                    byte[] fileArray = r.ReadBytes((int)fs.Length);
                    fs.Dispose();
                    return fileArray;
                }
            }
            return null;
        }

        /// <summary>
        /// 读文件<see cref="FileInfo"/>
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string FullName)
        {
            if (File.Exists(FullName))
            {
                FileInfo fileInfo = new FileInfo(FullName);
                return fileInfo;
            }
            return null;
        }
        
    }
}
