using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{
    /// <summary>
    ///http   Headers 中的一些参数定义
    ///add by wanglei 20180904
    /// </summary>
    public class WebRequestHeaders
    {
        public WebRequestHeaders(NameValueCollection RequestHeaders)
        {
           
            foreach (string key in RequestHeaders)
            {
                switch (key)
                {
                    case "Accept": { Accept = RequestHeaders[key]; break; }
                    case "Connection": { Connection = RequestHeaders[key]; break; }
                    case "Content-Length": { ContentLength = long.Parse(RequestHeaders[key]); break; }
                    case "Content-Type": { ContentType = RequestHeaders[key]; break; }
                    case "Expect": { Expect = RequestHeaders[key]; break; }
                    case "If-Modified-Since": { IfModifiedSince = RequestHeaders[key]; break; }
                    case "Host": { Host = RequestHeaders[key]; break; }
                    case "Transfer-Encoding": { TransferEncoding = RequestHeaders[key]; break; }
                    case "User-Agent": { UserAgent = RequestHeaders[key]; break; }
                    case "X-Real-IP": { X_Real_IP = RequestHeaders[key]; break; }
                    case "X-Forwarded-For":{ X_Forwarded_For = RequestHeaders[key]; break; }
        
                }
            }
        }

        /// <summary>
        ///<see cref="System.Net.HttpWebRequest.Accept"/>   
        /// </summary>
        public string Accept { get; set; }
        /// <summary>
        ///<see cref="System.Net.HttpWebRequest.Connection"/>    
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// <see cref="System.Net.HttpWebRequest.ContentLength"/>   
        /// Content-Length
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>
        ///内容类型 <see cref=" System.Net.HttpWebRequest.ContentType"/>  
        ///  Content-Type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///<see cref="System.Net.HttpWebRequest.Date"/>    
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// <see cref="System.Net.HttpWebRequest.Expect"/>   
        /// </summary>
        public string Expect { get; set; }

        /// <summary>
        ///<see cref=" System.Net.HttpWebRequest.Host"/>   
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///<see cref="System.Net.HttpWebRequest.IfModifiedSince"/>    
        /// If-Modified-Since
        /// </summary>
        public string IfModifiedSince { get; set; }

        /// <summary>
        ///<see cref=" System.Net.HttpWebRequest.AddRange(System.Int32, System.Int32) "/>   
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// <see cref="System.Net.HttpWebRequest.Referer"/>   
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        ///<see cref="System.Net.HttpWebRequest.TransferEncoding"/>    
        /// Transfer-Encoding
        /// </summary>
        public string TransferEncoding { get; set; }

        /// <summary>
        ///客户端引擎<see cref=" System.Net.HttpWebRequest.UserAgent"/>   
        /// User-Agent
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// 客户端真实IP
        /// </summary>
        public string X_Real_IP { get; set; }
        /// <summary>
        ///  X-Forwarded-For 客户端及代理
        /// </summary>
        public string X_Forwarded_For { get; set; }
       

    }
}
