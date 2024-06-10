using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EBPP
{
   public  class URLCheck
    {

        /// <summary>
        /// 判断绝对路径是否合法。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsAbosolutePath(string path)
        {
            string pattern = @"^\s*([a-zA-Z]:\\|\\\\)([^\^\/:*?""<>|]+\\)*([^\^\/:*?""<>|]+)$";
            Regex regex1 = new Regex(pattern);
            return regex1.IsMatch(path);
        }
        /// <summary>
        /// 判断绝对路径是否有效
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool AbosolutePathIsValid(string path)
        {
            if (IsAbosolutePath(path))
            {
                if (File.Exists(path))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 判断url是否合法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            var pattern = "^((https|http|ftp|rtsp|mms|ws|wss)?://)?"

                          + "("
                          + "([0-9]{1,3}\\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184   
                          + "|"
                          + "([0-9a-zA-Z_!~*'()-]+\\.)*" // 域名- www.    
                          + "([0-9a-zA-Z][0-9a-zA-Z-]{0,61})?[0-9a-zA-Z]\\." // 二级域名     
                          + "[a-zA-Z]{2,6}" // first level domain- .com or .museum   
                          + ")"

                          + "(:[0-9]{1,4})?" // 端口- :80 

                          + "("
                          + "(/?)"
                          + "|"
                          + "(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-]+)"
                          + "+/?"
                          + ")$";
            Regex regex1 = new Regex(pattern);
            return regex1.IsMatch(url);
        }

        /// <summary>
        /// 判断url是否有效
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlIsValid(string url)
        {
            if (IsUrl(url))
            {
                if (EBPPConfig.EBPPApplyCloseCheck)
                {
                    try
                    {
                        HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                        myRequest.Method = "HEAD";               //设置提交方式可以为＂ｇｅｔ＂，＂ｈｅａｄ＂等
                        myRequest.Timeout = 10000;              //设置网页响应时间长度
                        myRequest.AllowAutoRedirect = false;//是否允许自动重定向
                        HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                        if (myResponse.StatusCode == HttpStatusCode.OK) ;//返回响应的状态
                        {
                            myResponse.Close();
                            myRequest.Abort();

                            HttpWebRequest myRequest2 = (HttpWebRequest)WebRequest.Create(url);
                            // myRequest2.Method = "HEAD";               //设置提交方式可以为＂ｇｅｔ＂，＂ｈｅａｄ＂等
                            myRequest2.Timeout = 10000;              //设置网页响应时间长度
                                                                     // myRequest2.AllowAutoRedirect = false;//是否允许自动重定向
                            HttpWebResponse myResponse2 = (HttpWebResponse)myRequest2.GetResponse();
                            if (myResponse2.StatusCode == HttpStatusCode.OK) ;//返回响应的状态
                            { //关闭请求
                                myResponse2.Close();
                                myRequest2.Abort();
                                return true;
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "基础连接已经关闭: 发送时发生错误。")
                        {
                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

      

    }

}
