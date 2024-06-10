using BusinessInterface;
using Newtonsoft.Json;
using PasS.Base.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PaaS.Comm
{
    public partial class PaaSCenter : IDisposable
    {
        /// <summary>
     /// HTTP请求的的Header 中的 TID开头标识 ，用以区分是HTTP的请求
     /// </summary>
        const string HttpTIDStart = "HT:P";
        private HttpListener httplistener;   //其他超做请求监听
        public static int Rate = 0;

        string StartURL = "/SLB";//"/SLB";
        /// <summary>  
        /// HTTP服务器是否在运行  
        /// </summary>  
        public bool HttpIsRunning
        {
            get { return (httplistener == null) ? false : httplistener.IsListening; }

        }
        List<string> HttpIPList = new List<string>();
        public bool HttpIPAdd(string IP)
        {

            if (IP.StartsWith("169.254."))//window自行分配
            {
                return false;
            }

            else if (HttpIPList.Contains(IP))
            {
                return false;
            }
            else
            {
                HttpIPList.Add(IP);
                return true;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public int Httpport { get; set; }

        /// <summary>  
        /// 启动服务  
        /// </summary>  
        public void HttpStart()
        {

            // Httpport = MyPubConstant.HttpPort;
            if (httplistener != null && httplistener.IsListening)
                return;
            if (httplistener == null)
                httplistener = new HttpListener();


            if (HttpIPList.Count == 0)//如果事先没有指定IP则添加所有网卡IP
            {
                NetworkInterface[] interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface NetworkIntf in interfaces)
                {
                    if (NetworkIntf.OperationalStatus == OperationalStatus.Up)
                    {
                        IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
                        UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                        foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                        {
                            if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                HttpIPAdd(UnicastIPAddressInformation.Address.ToString());
                            }
                        }
                    }
                }
            }
            if (HttpIPList.Count > 0)
            {
                //listener.Prefixes.Add("http://" + Host + ":" + this.port + "/");
                //AddPrefixes("/BehaviorApi/EmailSend/");

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    AddPrefixes("/");
                }
                else
                {
                    //if (MyPubConstant.SLBHttpMyspringClear)
                    {
                        //AddPrefixes("/MyspringClear/");//未加密
                        //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        //{
                        //    AddPrefixes("/MySpringClear/");//未加密
                        //    AddPrefixes("/myspringClear/");//未加密
                        //    AddPrefixes("/myspringclear/");//未加密
                        //}
                    }
                    AddPrefixes("/Notify/");//支付、等通知 

                    AddPrefixes("/MyspringAES/");//加密
                    //if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    //{
                    //    AddPrefixes("/notify/");//支付、等通知 
                    //    AddPrefixes("/myspringAES/");//加密
                    //    AddPrefixes("/MySpringAES/");//加密
                    //    AddPrefixes("/myspringaes/");//加密
                    //}
                    AddPrefixes("/SpringAPI/");//内部函数加密访问
                    AddPrefixes("/Helper/");//帮助文档
                    AddPrefixes("/healthAES/");//y医保支付
                    AddPrefixes("/healthClear/");//y医保支付
                }
            }

            httplistener.Start();
            // GethttRequest();
            Thread listenThread = new Thread(GethttRequest);
            listenThread.Name = "httpserver";
            listenThread.Start();

            Console.WriteLine(string.Format("HttpServer start[Port:{0}] ...  ", Httpport));
            // PasSLog.Info("HttpStart", string.Format("HttpServer start[Port:{0}] ...  ", Httpport));
        }


        /// <summary>  
        /// 停止服务  
        /// </summary>  
        public void HTTPStop()
        {
            try
            {
                if (httplistener != null)
                {
                    //listener.Close();
                    //listener.Abort();
                    httplistener.Stop();
                    Console.WriteLine(string.Format("HttpServer Stop  ", Httpport));
                    //PasSLog.Info("HTTPStop", string.Format("HttpServer Stop  ", Httpport));
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool AddPrefixes(string uriPrefix)
        {
            foreach (string IP in HttpIPList)
            {
                //string uri = "http://" + IP + ":" + this.Httpport + StartURL + uriPrefix;
                string uri = "http://" + IP + ":" + this.Httpport + uriPrefix;
                if (httplistener.Prefixes.Contains(uri)) return false;
                httplistener.Prefixes.Add(uri);


                Console.WriteLine(string.Format("{0} HTT AddPrefixes :{1}  ", DateTime.Now, uri));
            }
            return true;
        }

        /// <summary>
        /// 执行请求监听行为
        /// </summary>
        void GethttRequest()
        {
            int maxThreadNum, portThreadNum;
            //线程池
            int minThreadNum;
            ThreadPool.GetMaxThreads(out maxThreadNum, out portThreadNum);
            ThreadPool.GetMinThreads(out minThreadNum, out portThreadNum);
            Console.WriteLine("最大线程数：{0} 最小空闲线程数：{1}", maxThreadNum, minThreadNum);


            Console.WriteLine("http等待客户连接中。。。。");
            while (httplistener.IsListening)
            {
                try
                {
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    HttpListenerContext ctx = httplistener.GetContext();
                    // Console.WriteLine("\n连接。。。。");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(TaskProc), ctx);
                }
                catch (Exception ex)
                {
                }
            }
        }
        
        void TaskProc(object o)
        {
            using (SUsingDisposable sUsingDisposable = new SUsingDisposable())
            {
                if (o == null)
                    return;
                HttpRequestRates++;
                ClientToSLBRates++;
                Stopwatch watch = new Stopwatch();
                watch.Start();
                HttpListenerContext ctx = (HttpListenerContext)o;
                string Strlog = "";
                try
                {

                    //Headers
                    WebRequestHeaders webRequestHeaders = new WebRequestHeaders(ctx.Request.Headers);
                    var endPoint = ctx.Request.RemoteEndPoint;//客户端IP

                    try
                    {
                        if (!string.IsNullOrEmpty(webRequestHeaders.X_Real_IP))
                        {
                            if (webRequestHeaders.X_Real_IP != endPoint.Address.ToString())
                                endPoint = new IPEndPoint(IPAddress.Parse(webRequestHeaders.X_Real_IP), endPoint.Port);
                        }
                        else if (!string.IsNullOrEmpty(webRequestHeaders.X_Forwarded_For))
                        {
                            try
                            {
                                string X_Forwarded = "";
                                if (webRequestHeaders.X_Forwarded_For.Contains(","))
                                {
                                    X_Forwarded = webRequestHeaders.X_Forwarded_For.Split(',')[0];
                                }
                                else
                                {
                                    X_Forwarded = webRequestHeaders.X_Forwarded_For;
                                }
                                endPoint = new IPEndPoint(IPAddress.Parse(X_Forwarded), endPoint.Port);
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch { }
                    Strlog += endPoint.Address.ToString() + ":" + endPoint.Port.ToString() + " ";
                    Strlog += ctx.Request.HttpMethod + " ";
                    Strlog += ctx.Request.RawUrl + " ";
                    Strlog += ctx.Request.Headers.ToString() + " ";

                    string sContentType = "";
                    if (webRequestHeaders.ContentType == null || webRequestHeaders.ContentType.StartsWith(ContentType.EnctypeDefault, true, null))
                    {
                        sContentType = ContentType.EnctypeDefault;
                    }
                    else if (webRequestHeaders.ContentType.StartsWith(ContentType.Json, true, null))
                    {
                        sContentType = ContentType.Json;
                    }
                    else
                    {
                        sContentType = webRequestHeaders.ContentType;
                    }
                    //获取rawUrl
                    string rawUrl = ctx.Request.RawUrl;
                    int paramStartIndex = rawUrl.IndexOf('?');
                    if (paramStartIndex > 0)
                        rawUrl = rawUrl.Substring(0, paramStartIndex);
                    else if (paramStartIndex == 0)
                        rawUrl = "";

                    string InParam = "";//字符串 入参数 

                    ConvertHttpParamToDic conToDic = new ConvertHttpParamToDic("");
                    // NameValueCollection NVCQuery = null;
                    string Json = "";

                    if (ctx.Request.HttpMethod == "POST")
                    {
                        //读取客户端发送过来的数据
                        using (Stream inputStream = ctx.Request.InputStream)
                        {
                            using (StreamReader reader = new StreamReader(inputStream, Encoding.UTF8))
                            {
                                InParam = reader.ReadToEnd();
                                // data = System.Web.HttpUtility.UrlDecode(data);//解码字符串
                                if (sContentType == ContentType.EnctypeDefault)
                                {
                                    conToDic = new ConvertHttpParamToDic(InParam);
                                    //   NVCQuery = HttpUtility.ParseQueryString(data);
                                }
                                else if (sContentType == ContentType.Json)
                                {
                                    Json = InParam;
                                }
                                else
                                {
                                    Json = InParam;
                                }
                                // Dictionary<string, string> dictionary = CoverstringToDictionary(data);
                            }
                        }
                    }
                    else//get
                    {
                        if (paramStartIndex >= 0 && ctx.Request.RawUrl.Length >= paramStartIndex)
                        {
                            InParam = ctx.Request.RawUrl.Substring(paramStartIndex);
                            InParam = Encoding.UTF8.GetString(Encoding.Default.GetBytes(InParam));
                            string QueryString = System.Web.HttpUtility.UrlDecode(InParam);//解码字符串
                            conToDic = new ConvertHttpParamToDic(QueryString);
                            if (sContentType == ContentType.Json)
                            {
                                Json = conToDic.GetValue(0);
                                // Json = NVCQuery.Get(0);
                            }
                        }
                        //var msg = NVCQuery["BusData"]; //接受GET请求过来的参数；
                        //Console.WriteLine(msg);
                        //在此处执行你需要进行的操作>>比如什么缓存数据读取，队列消息处理，邮件消息队列添加等等。
                        // string filename = Path.GetFileName(ctx.Request.RawUrl);
                        // NVCQuery = HttpUtility.ParseQueryString(filename); //避免中文乱码

                    }

                    string Outdata = "";
                    if (string.Compare(rawUrl, StartURL + "/Helper/", true) == 0)
                    {//说明文档
                        Outdata = PasSLog.ReadAllText("httpreadme.txt");
                        Outdata = Outdata.Replace("(0}", MyPubConstant.SLBHttpMyspringClear ? "[当前服务已经开启非加密接口!]" : "[当前服务已经关闭非加密接口!]");
                        ctx.Response.ContentType = "text/html;charset=UTF-8";
                    }
                    else if (rawUrl.StartsWith(StartURL + "/myspring", true, null))
                    {
                        #region myspring 处理主要程序
                        SpringHttpData springHttpData = new SpringHttpData();
                        if (sContentType == ContentType.EnctypeDefault)
                        {//默认格式
                            if (rawUrl.StartsWith(StartURL + "/myspringAES/", true, null))
                            {
                                springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringAES/").Length).TrimEnd('/');
                                springHttpData.ParamType = 1;
                                springHttpData.Param = conToDic.GetValue("Param"); //加密的字符串~~
                                springHttpData.user_id = conToDic.GetValue("user_id");   // 
                                springHttpData.sign = conToDic.GetValue("sign");  //签名
                                springHttpData.SubBusID = conToDic.GetValue("SubBusID"); //子业务ID.（根据实际业务定义，可空）
                                springHttpData.CTag = conToDic.GetValue("CTag");   //灰度发布标记.可空
                                springHttpData.TID = conToDic.GetValue("TID"); //业务唯一流水号.可空
                                string sGzip = conToDic.GetValue("Gzip"); //是否压缩 0 未压缩 1 压缩.可空
                                if (!string.IsNullOrWhiteSpace(sGzip) && sGzip == "1")
                                    springHttpData.Gzip = 1;
                                //Console.WriteLine(JsonConvert.SerializeObject(springHttpData));
                            }
                            else if (rawUrl.StartsWith(StartURL + "/myspringClear/", true, null))
                            {//myspringClear 明文入参
                                springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringClear/").Length).TrimEnd('/');
                                springHttpData.ParamType = 13;
                                springHttpData.Param = conToDic.GetValue("Param"); //字符串~~
                                springHttpData.user_id = conToDic.GetValue("user_id");   // 
                                springHttpData.sign = conToDic.GetValue("sign");  //签名
                                springHttpData.SubBusID = conToDic.GetValue("SubBusID"); //子业务ID.（根据实际业务定义，可空）
                                springHttpData.CTag = conToDic.GetValue("CTag");   //灰度发布标记.可空
                                springHttpData.TID = conToDic.GetValue("TID"); //业务唯一流水号.可空
                                string sGzip = conToDic.GetValue("Gzip"); //是否压缩 0 未压缩 1 压缩.可空
                                if (!string.IsNullOrWhiteSpace(sGzip) && sGzip == "1")
                                    springHttpData.Gzip = 1;
                                //  Console.WriteLine(JsonConvert.SerializeObject(springHttpData));
                            }
                        }
                        else if (sContentType == ContentType.Json)
                        {//Json格式
                            if (rawUrl.StartsWith(StartURL + "/myspringAES/", true, null))
                            {
                                try
                                {
                                    springHttpData = JsonConvert.DeserializeObject<SpringHttpData>(Json);
                                }
                                catch
                                {
                                    HttpResponseError(ctx, "请检查Json格式,格式为不正确.", Strlog + "\r\n" + Json);
                                    return;
                                }
                                // Console.WriteLine(Json);
                                springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringAES/").Length).TrimEnd('/');
                                springHttpData.ParamType = 1;
                            }
                            else if (rawUrl.StartsWith(StartURL + "/myspringClear/", true, null))
                            {// myspringClear 明文入参
                                try
                                {
                                    springHttpData = JsonConvert.DeserializeObject<SpringHttpData>(Json);
                                }
                                catch
                                {
                                    HttpResponseError(ctx, "请检查Json格式,格式为不正确.", Strlog + "\r\n" + Json);
                                    return;
                                }
                                //  Console.WriteLine(Json);
                                springHttpData.BusID = rawUrl.Substring((StartURL + "/myspringClear/").Length).TrimEnd('/');
                                springHttpData.ParamType = 13;
                            }

                        }
                        if (string.IsNullOrWhiteSpace(springHttpData.BusID))
                        {
                            HttpResponseSpringError(ctx, -1, "请检查URL,末节点不正确.", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(springHttpData.Param))
                        {
                            HttpResponseSpringError(ctx, -1, "缺少必填参数Param!", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(springHttpData.user_id) && springHttpData.ParamType != 13)
                        {
                            HttpResponseSpringError(ctx, -1, "缺少必填参数user_id!", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(springHttpData.sign) && springHttpData.ParamType != 13)
                        {
                            HttpResponseSpringError(ctx, -1, "缺少必填参数sign!", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                            return;
                        }

                        int EncryptType = 1;//加密类型，HTTP为1
                        string SEID = springHttpData.user_id;//用户加密ID
                        SLBBusinessInfo sLBBusinessInfo = new SLBBusinessInfo();
                        string Paramdecode = "";



                        if (springHttpData.ParamType == 1)
                        {
                            //if (!SignEncryptHelper.SignCheck(springHttpData.Param, springHttpData.sign, EncryptType,springHttpData.user_id))
                            if (!KeyData.AESKEYCheck(springHttpData.user_id, springHttpData.Param, springHttpData.sign))
                            {
                                HttpResponseSpringError(ctx, -1, "签名错误！", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                                return;
                            }
                            try
                            {
                                Paramdecode = SignEncryptHelper.DecryptJave(springHttpData.Param, EncryptType, springHttpData.user_id);
                            }
                            catch (Exception ex)
                            {
                                HttpResponseSpringError(ctx, -1, "解密失败！", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                                return;
                            }
                            sLBBusinessInfo.BusData = Paramdecode;
                        }

                        else if (springHttpData.ParamType == 13)
                        {
                            if (!string.IsNullOrEmpty(springHttpData.user_id) && !string.IsNullOrEmpty(springHttpData.sign))
                            {
                                if (!KeyData.AESKEYCheck(springHttpData.user_id, springHttpData.Param, springHttpData.sign))
                                {
                                    HttpResponseSpringError(ctx, -1, "签名错误！", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                                    return;
                                }
                            }
                            EncryptType = 0;

                            sLBBusinessInfo.BusData = springHttpData.Param;

                        }
                        if (springHttpData.Gzip == 1)//如果压缩了 要先解压
                        {
                            sLBBusinessInfo.BusData = GZipHelper.DecompressBase64(sLBBusinessInfo.BusData);
                        }
                        sLBBusinessInfo.BusID = springHttpData.BusID;
                        sLBBusinessInfo.SubBusID = springHttpData.SubBusID;
                        sLBBusinessInfo.CTag = springHttpData.CTag;
                        sLBBusinessInfo.TID = springHttpData.TID;


                        #region 调用SLB进行数据处理
                        Outdata = HttpSpringCallSLB(springHttpData, sLBBusinessInfo, EncryptType, endPoint);

                        #endregion

                        #endregion
                        ctx.Response.ContentType = "application/json;charset=UTF-8";
                    }
                    else if (rawUrl.StartsWith(StartURL + "/health", true, null))
                    {
                        BusinessInterface.InputRoot platInput = new BusinessInterface.InputRoot();

                        BusinessInterface.OutputRoot platOutput = new BusinessInterface.OutputRoot();
                        string BusID = "";
                        string InputData = conToDic.GetValue("InputData");



                        #region myspring 处理主要程序
                        SpringHttpData springHttpData = new SpringHttpData();
                        if (sContentType == ContentType.EnctypeDefault)
                        {//默认格式
                            string HOS_ID = conToDic.GetValue("HOS_ID");
                            string CTag = conToDic.GetValue("CTag"); //灰度发布标记.可空
                            InputData = conToDic.GetValue("InputData");//字符串~~
                            HOS_ID = conToDic.GetValue("HOS_ID");
                            //  springHttpData.sign = conToDic.GetValue("sign");  //签名

                            // platInput.TID = conToDic.GetValue("TID"); //业务唯一流水号.可空
                            string sGzip = conToDic.GetValue("Gzip"); //是否压缩 0 未压缩 1 压缩.可空

                            if (rawUrl.StartsWith(StartURL + "/healthAES/", true, null))
                            {
                                BusID = rawUrl.Substring((StartURL + "/healthAES/").Length).TrimEnd('/');
                                springHttpData.ParamType = 1;
                            }
                            else if (rawUrl.StartsWith(StartURL + "/healthClear/", true, null))
                            {//healthClear 明文入参
                                BusID = rawUrl.Substring((StartURL + "/healthClear/").Length).TrimEnd('/');
                                springHttpData.ParamType = 13;
                            }
                            try
                            {
                                platInput = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessInterface.InputRoot>(InputData);

                            }
                            catch (Exception ex)
                            {
                                HttpResponseError(ctx, "请检查InputData Json格式,格式为不正确.", Strlog + "\r\n" + Json);
                                return;
                            }
                            if (!string.IsNullOrWhiteSpace(sGzip) && sGzip == "1")
                                platInput.Gzip = 1;
                            if (!string.IsNullOrWhiteSpace(CTag))
                                platInput.CTag = CTag;
                            platInput.HOS_ID = HOS_ID;
                        }
                        else if (sContentType == ContentType.Json)
                        {//Json格式
                            if (rawUrl.StartsWith(StartURL + "/healthAES/", true, null))
                            {
                                try
                                {
                                    platInput = JsonConvert.DeserializeObject<InputRoot>(Json);
                                }
                                catch
                                {
                                    HttpResponseError(ctx, "请检查Json格式,格式为不正确.", Strlog + "\r\n" + Json);
                                    return;
                                }

                                BusID = rawUrl.Substring((StartURL + "/healthAES/").Length).TrimEnd('/');
                                springHttpData.ParamType = 1;
                            }
                            else if (rawUrl.StartsWith(StartURL + "/healthClear/", true, null))
                            {// myspringClear 明文入参
                                try
                                {
                                    platInput = JsonConvert.DeserializeObject<InputRoot>(Json);
                                }
                                catch
                                {
                                    HttpResponseError(ctx, "请检查Json格式,格式为不正确.", Strlog + "\r\n" + Json);
                                    return;
                                }
                                //  Console.WriteLine(Json);
                                BusID = rawUrl.Substring((StartURL + "/healthClear/").Length).TrimEnd('/');
                                springHttpData.ParamType = 13;
                            }

                        }

                        platInput.BusID = BusID;
                        if (string.IsNullOrWhiteSpace(platInput.HOS_ID))
                        {
                            HttpResponseOutputRootError(ctx, 9999, "请检查URL,末节点不正确.", springHttpData.TID, Strlog + "\r\n" + InputData);
                            return;
                        }
                        if (string.IsNullOrWhiteSpace((string)platInput.InData))
                        {
                            HttpResponseOutputRootError(ctx, 9999, "缺少必填参数InData!", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(platInput));
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(platInput.HOS_ID) && springHttpData.ParamType != 13)
                        {
                            HttpResponseOutputRootError(ctx, 9999, "缺少必填参数HOS_ID!", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                            return;
                        }
                        if (string.IsNullOrWhiteSpace(platInput.sign) && springHttpData.ParamType != 13)
                        {
                            HttpResponseOutputRootError(ctx, 9999, "缺少必填参数sign!", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                            return;
                        }

                        int EncryptType = 1;//加密类型，HTTP为1
                        string SEID = springHttpData.user_id;//用户加密ID


                        if (springHttpData.ParamType == 1)
                        {
                            //if (!SignEncryptHelper.SignCheck(springHttpData.Param, springHttpData.sign, EncryptType, springHttpData.user_id))
                            if (!KeyData.AESKEYCheck(platInput.HOS_ID, (string)platInput.InData, platInput.sign))
                            {
                                HttpResponseOutputRootError(ctx, 9999, "签名错误！", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                                return;
                            }
                            try
                            {
                                platInput.InData = SignEncryptHelper.DecryptJave((string)platInput.InData, EncryptType, platInput.HOS_ID);
                            }
                            catch (Exception ex)
                            {
                                HttpResponseOutputRootError(ctx, 9999, "解密失败！", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(platInput));
                                return;
                            }
                        }

                        else if (springHttpData.ParamType == 13)
                        {
                            if (!string.IsNullOrEmpty(platInput.HOS_ID) && !string.IsNullOrEmpty(springHttpData.sign))
                            {
                                if (!KeyData.AESKEYCheck(platInput.HOS_ID, springHttpData.Param, springHttpData.sign))
                                {
                                    HttpResponseSpringError(ctx, -1, "签名错误！", springHttpData.TID, Strlog + "\r\n" + JsonConvert.SerializeObject(springHttpData));
                                    return;
                                }
                            }
                            EncryptType = 0;
                        }
                        if (platInput.Gzip == 1)//如果压缩了 要先解压
                        {
                            platInput.InData = GZipHelper.DecompressBase64((string)platInput.InData);
                        }


                        #region 调用SLB进行数据处理

                        try
                        {
                            platOutput =HttpCallSLB(platInput);
                            if (platInput.Gzip == 1)// 要先压缩
                            {
                                platOutput.OutData = GZipHelper.CompressToBase64((string)platOutput.OutData);
                            }
                            if (EncryptType > 0)
                            {
                                platOutput.OutData = SignEncryptHelper.EncryptJava((string)platOutput.OutData, EncryptType, platInput.HOS_ID);
                                //  springHttpDataOut.sign = SignEncryptHelper.Sign(springHttpDataOut.Param, EncryptType, springHttpData.user_id);
                            }



                        }
                        catch (Exception ex)
                        {
                            platOutput.RspCode = "9999";
                            platOutput.RspMsg = "入参解析失败，请检查参数格式是否正确";
                        }
                        Outdata = JsonConvert.SerializeObject(platOutput);
                        #endregion

                        #endregion
                    }
                    else
                    {

                        Outdata = "未处理的请求:" + ctx.Request.RawUrl;
                    }
                    ctx.Response.ContentType = "application/json;charset=UTF-8";


                    ctx.Response.StatusCode = 200;//设置返回给客服端http状态代码
                    ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    ctx.Response.ContentEncoding = Encoding.UTF8;

                    //使用Writer输出http响应代码
                    using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
                    {
                        writer.Write(Outdata);
                        //  Console.WriteLine(Outdata);
                        writer.Close();
                        ctx.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        HttpResponseError(ctx, "服务器错误:" + ex.ToString(), Strlog);
                        Console.WriteLine(string.Format("{0} HTTP Errpr :{1}  ", DateTime.Now, ex.Message));
                    }
                    catch { }
                }
                Rate++;
                watch.Stop();
                PasSLog.HTTPLog(watch.ElapsedMilliseconds.ToString() + "毫秒", Strlog);
                //  Console.WriteLine(watch.ElapsedMilliseconds.ToString() + "毫秒");
            }
        }
        private bool HttpResponseSpringError(HttpListenerContext ctx, int ReslutCode, string ResultMessage, string TID, string InInfo)
        {
            try
            {
                Rate++;
                SpringHttpDataOut springHttpDataOut = new SpringHttpDataOut();
                springHttpDataOut.TID = TID;
                springHttpDataOut.ReslutCode = ReslutCode;
                springHttpDataOut.ResultMessage = ResultMessage;
                ctx.Response.StatusCode = 200;
                if (ctx.Request.HttpMethod == "POST")
                    ctx.Response.ContentType = "application/text;charset=UTF-8";
                else
                    ctx.Response.ContentType = "text/html;charset=UTF-8";

                ctx.Response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(springHttpDataOut));
                //对客户端输出相应信息.
                ctx.Response.ContentLength64 = buffer.Length;
                System.IO.Stream output = ctx.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                //关闭输出流，释放相应资源
                output.Close();

                PasSLog.HTTPLog("SpringError", string.Format(@"InInfo: {0}  
                               return:{1} {2} ", InInfo, ReslutCode, ResultMessage));
                return true;
            }
            catch { }
            return true;
        }
        private bool HttpResponseOutputRootError(HttpListenerContext ctx, int ReslutCode, string ResultMessage, string TID, string InInfo)
        {
            try
            {
                Rate++;
                OutputRoot springHttpDataOut = new OutputRoot();

                springHttpDataOut.RspCode = ReslutCode.ToString();
                springHttpDataOut.RspMsg = ResultMessage;
                ctx.Response.StatusCode = 200;
                if (ctx.Request.HttpMethod == "POST")
                    ctx.Response.ContentType = "application/text;charset=UTF-8";
                else
                    ctx.Response.ContentType = "text/html;charset=UTF-8";

                ctx.Response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(springHttpDataOut));
                //对客户端输出相应信息.
                ctx.Response.ContentLength64 = buffer.Length;
                System.IO.Stream output = ctx.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                //关闭输出流，释放相应资源
                output.Close();

                PasSLog.HTTPLog("SpringError", string.Format(@"InInfo: {0}  
                               return:{1} {2} ", InInfo, ReslutCode, ResultMessage));
                return true;
            }
            catch { }
            return true;
        }
        private bool HttpResponseError(HttpListenerContext ctx, string Outdata, string Ininfo)
        {
            try
            {
                Rate++;
                ctx.Response.StatusCode = 202;
                if (ctx.Request.HttpMethod == "POST")
                    ctx.Response.ContentType = "application/text;charset=UTF-8";
                else
                    ctx.Response.ContentType = "text/html;charset=UTF-8";
                ctx.Response.ContentEncoding = Encoding.UTF8;
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Outdata);
                //对客户端输出相应信息.
                ctx.Response.ContentLength64 = buffer.Length;
                System.IO.Stream output = ctx.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                //关闭输出流，释放相应资源
                output.Close();
                PasSLog.HTTPLog("HTTPError", string.Format(@"InInfo: {0}  
                               return:{1} {2} ", Ininfo, 202, Outdata));
                return true;
            }
            catch { }
            return true;
        }

        #region HTTP调用SLB进行数据处理
        string HttpSpringCallSLB(SpringHttpData springHttpData, SLBBusinessInfo sLBBusinessInfo, int EncryptType, IPEndPoint endPoint)
        {
            string Outdata = "";
            SpringHttpDataOut springHttpDataOut = HttpSpringCallSLB2(springHttpData, sLBBusinessInfo, EncryptType, endPoint);
            Outdata = JsonConvert.SerializeObject(springHttpDataOut);

            return Outdata;
        }


        #region HTTP调用BusServiceAdapter进行数据处理
        public SpringHttpDataOut HttpSpringCallSLB2(SpringHttpData springHttpData, SLBBusinessInfo sLBBusinessInfo, int EncryptType, IPEndPoint endPoint)
        {
            string Outdata = "";
            #region 调用SLB进行数据处理


            SLBBusinessInfo sLBInfoOut = HttpCallSLB(sLBBusinessInfo, endPoint);
            SpringHttpDataOut springHttpDataOut = new SpringHttpDataOut();

            springHttpDataOut.TID = springHttpData.TID;
            springHttpDataOut.ReslutCode = sLBInfoOut.ReslutCode;
            springHttpDataOut.ResultMessage = sLBInfoOut.ResultMessage;

            if (springHttpData.Gzip == 1)
            {
                springHttpDataOut.Gzip = 1;
                sLBInfoOut.BusData = GZipHelper.CompressToBase64(sLBInfoOut.BusData);
            }
            if (EncryptType > 0)
            {
                springHttpDataOut.Param = SignEncryptHelper.EncryptJava(sLBInfoOut.BusData, EncryptType, springHttpData.user_id);
                //  springHttpDataOut.sign = SignEncryptHelper.Sign(springHttpDataOut.Param, EncryptType, springHttpData.user_id);
            }
            else
            {
                springHttpDataOut.Param = sLBInfoOut.BusData;
                if (!string.IsNullOrEmpty(springHttpData.user_id))
                {
                    springHttpDataOut.sign = KeyData.Sign(springHttpData.user_id, springHttpDataOut.Param);
                }
            }
            //   Outdata = JsonConvert.SerializeObject(springHttpDataOut);

            #endregion
            return springHttpDataOut;
        }
        SLBBusinessInfo HttpCallSLB(SLBBusinessInfo sLBBusinessInfo, IPEndPoint endPoint)
        {
            SLBBusinessInfo OutBusinessInfo = null;

            #region 调用SLB进行数据处理
            string inBusInfo = JsonConvert.SerializeObject(sLBBusinessInfo);
            SLBInfoHead sLBInfoHead = new SLBInfoHead(sLBBusinessInfo);

            string Key = HttpTIDStart + NewIdHelper.NewOrderId24;
            sLBInfoHead.TID = Key;
            if (sLBBusinessInfo.BusID.ToUpper() == "EBPP2")
            {
                //  new EBPPAPI().ProcessingBusiness(sLBBusinessInfo, out OutBusinessInfo);
            }
            else
            {
                if (BusServiceAdapter.Ipb_CallOtherBusiness(sLBBusinessInfo, out OutBusinessInfo))
                {

                }
                else if (OutBusinessInfo != null)
                {

                }
                else
                {
                    OutBusinessInfo = new SLBBusinessInfo(sLBBusinessInfo);
                    OutBusinessInfo.ReslutCode = -1;
                    OutBusinessInfo.ResultMessage = "系统处理失败：Ipb_CallOtherBusiness";

                }

            }
            #endregion
            return OutBusinessInfo;
        }


        public OutputRoot HttpCallSLB(InputRoot inputRoot)
        {
            OutputRoot outputRoot = new OutputRoot();

            #region 调用SLB进行数据处理

            if (BusServiceAdapter.Ipb_CallOtherBusiness(inputRoot, out outputRoot))
            {

            }
            else if (outputRoot != null)
            {

            }
            else
            {
                outputRoot = new OutputRoot(inputRoot);
                outputRoot.RspCode = "9990";
                outputRoot.RspMsg = "系统处理失败：Ipb_CallOtherBusiness";



            }
            #endregion
            return outputRoot;
        }
        #endregion
        #endregion
    }
    /// <summary>
    /// 简单的UsingDisposable
    /// </summary>
    public class SUsingDisposable : IDisposable
    {
        public void Dispose()
        {

        }
    }
}
