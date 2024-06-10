using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;

namespace Soft.Core
{
    public class Base64Helper
    {

        //public static bool DecodeBase64ToImage(string dataURL, string pathname, out string path)
        //{
        //    path = string.Empty;
        //    String base64 = dataURL.Substring(dataURL.IndexOf(",") + 1);      //将‘，’以前的多余字符串删除
        //    Bitmap bitmap = null;//定义一个Bitmap对象，接收转换完成的图片
        //    try//会有异常抛出，try，catch一下
        //    {
        //        byte[] arr = Convert.FromBase64String(base64);//将纯净资源Base64转换成等效的8位无符号整形数组
        //        using (MemoryStream ms = new MemoryStream(arr))
        //        {
        //            Bitmap bmp = new Bitmap(ms);
        //            bitmap = bmp;
        //            bitmap.Save(pathname);
        //            string tmpRootDir = System.Web.HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()); //获取程序根目录 
        //            path = pathname.Substring(tmpRootDir.Length).Replace("\\", "/");
        //        }
        //        //转换成无法调整大小的MemoryStream对象
        //        //将MemoryStream对象转换成Bitmap对象
        //        //关闭当前流，并释放所有与之关联的资源          

        //        File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/") + "DecodeBase64ToImage_LOG.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + dataURL + "\r\n" + path+ "\r\n");
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/") + "DecodeBase64ToImage_LOG.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" + dataURL + "\r\n" + JsonConvert.SerializeObject(e, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })+ "\r\n");
        //        return false;
        //    }
        //}

        /// <summary>
        /// 文件转换为Base64二进制流
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string FileToBase64(string FilePath)
        {
            using (FileStream fileStream = File.Open(FilePath, FileMode.OpenOrCreate))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        /// <summary>
        /// Base64二进制流还原文件
        /// </summary>
        /// <param name="FilePath">存放文件的路径</param>
        /// <param name="StringBase64">文件Base64二进制流</param>
        public static bool Base64ToCode(string FilePath, string StringBase64)
        {
            try
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    byte[] buffer = Convert.FromBase64String(StringBase64);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
