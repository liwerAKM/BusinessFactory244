using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace PasS.Base.Lib
{
 
  // <summary>
  /// 写本地文件日志
  /// </summary>
    public class PasSLog
    {
        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            //if (MyPubConstant.LOG_LEVENL >= 3)
            {
                WriteLog("DEBUG", className, content);
            }
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            //if (MyPubConstant.LOG_LEVENL >= 2)
            {
                WriteLog("INFO", className, content);
            }
        }
        /// <summary>
        /// HTTP日志
        /// </summary>
        /// <param name="className"></param>
        /// <param name="content"></param>
        public static void HTTPLog(string className, string content)
        {
            WriteLog("HTTPLog", className, content);
        }
        /// <summary>
        ///WebSocket日志
        /// </summary>
        /// <param name="className"></param>
        /// <param name="content"></param>
        public static void WebSocketLog(string className, string content)
        {
            WriteLog("WebSocketLog", className, content);
        }
        /// <summary>
        /// 记录慢日志
        /// </summary>
        /// <param name="className"></param>
        /// <param name="content"></param>
        public static void SlowExecute(string className, string content)
        {
            WriteLog("SlowExecute", className, content);
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
          //  if (MyPubConstant.LOG_LEVENL >= 1)
            {
                WriteLog("ERROR", className, content);
            }
        }

        /**
        * 实际的写日志操作
        * @param type 日志记录类型
        * @param className 类名
        * @param content 写入内容
        */
        protected static void WriteLog(string type, string className, string content)
        {
            string path = "";
            try
            {
                // path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\MySpringlog";
                path = path = Path.Combine(AppContext.BaseDirectory, "PasSLog");
            }
            catch (Exception ex)
            {
                return;
            }

            if (!Directory.Exists(path))//如果日志目录不存在就创建
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");//获取当前系统时间
                string filename = path + "/" + DateTime.Now.ToString("yyyyMMdd") + type.Replace('|', ':') + ".log";//用日期对日志文件命名
                //创建或打开日志文件，向日志文件末尾追加记录
                StreamWriter mySw = File.AppendText(filename);

                //向日志文件写入内容
                string write_content = className + ": " + content;
                mySw.WriteLine(time + " " + type);
                mySw.WriteLine(write_content);
                mySw.WriteLine("");
                //关闭日志文件
                mySw.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public static string ReadAllText(string FileName)
        {
            try
            {
                string path = "";
                path = path = Path.Combine(AppContext.BaseDirectory, FileName);
                return File.ReadAllText(path, Encoding.Default);
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static bool WriteAllText(string FileName, string Text)
        {
            try
            {
                string path = "";
                path = Path.Combine(AppContext.BaseDirectory, FileName);
                File.WriteAllText(path, Text, Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
