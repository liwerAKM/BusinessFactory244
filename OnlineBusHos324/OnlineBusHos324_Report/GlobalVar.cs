using Log.Core.Model;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OnlineBusHos324_Report
{
    class GlobalVar
    {

        public static string DoBussiness = GetConfig("DoBussiness");

        public static string callmode = GetConfig("callmode");

        public static string posturl = GetConfig("url");

        public static string parameter = GetConfig("parameters");

        public static string use_encryption = GetConfig("use_encryption");

        public static string MethodName = GetConfig("MethodName");

        public static string GetConfig(string configname)
        {
            XmlDocument docini = new XmlDocument();
            docini.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "OnlineBusHos324_Report.dll.config"));
            DataSet ds = XMLHelper.X_GetXmlData(docini, "configuration/appSettings");//请求的数据包
            DataRow[] dr = ds.Tables[0].Select("key='" + configname + "'");
            if (dr.Length > 0)
            {
                return dr[0]["value"].ToString();
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// POST入参存入hashtable
        /// </summary>
        /// <param name="inxml">入参</param>
        /// <param name="hos_id">医院ID</param>
        /// <param name="para">POST参数</param>
        /// <returns></returns>
        public static Hashtable GetHashTable(string inxml, string hos_id, string para, string use_encryption)
        {
            try
            {
                Hashtable hashtable = new Hashtable();
                if (use_encryption == "1")
                {
                    string secretkey = "";
                    secretkey = EncryptionKeyCore.KeyData.AESKEY(hos_id);
                    string encryxml = AESExample.AESEncrypt(inxml, secretkey);
                    string signature = EncryptionKeyCore.MD5Helper.Md5(encryxml + secretkey);
                    string[] items = para.Split('^');
                    string[] _showids = items[0].Split('|');
                    string[] _shownames = items[1].Split('|');

                    if (_showids[0] == "1")
                    {
                        hashtable.Add(_shownames[0], encryxml);
                    }
                    if (_showids[1] == "1")
                    {
                        hashtable.Add(_shownames[1], hos_id);
                    }
                    if (_showids[2] == "1")
                    {
                        hashtable.Add(_shownames[2], signature);
                    }
                }
                else
                {
                    string[] items = para.Split('^');
                    string[] _showids = items[0].Split('|');
                    string[] _shownames = items[1].Split('|');
                    if (_showids[0] == "1")
                    {
                        hashtable.Add(_shownames[0], inxml);
                    }
                    if (_showids[1] == "1")
                    {
                        hashtable.Add(_shownames[1], hos_id);
                    }
                    if (_showids[2] == "1")
                    {
                        hashtable.Add(_shownames[2], "");
                    }
                }
                return hashtable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public class indata
        {
            public string xmlstr { get; set; }
            public string user_id { get; set; }
            public string signature { get; set; }
        }
        public class outdata
        {
            public string outxml { get; set; }
        }

        public static bool CALLSERVICE(string HOS_ID, string inxml, ref string his_rtnxml)
        {
            DateTime intime = DateTime.Now;
            try
            {
                if (callmode == "0")//webservice
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable = GlobalVar.GetHashTable(inxml, HOS_ID, parameter, use_encryption);
                    XmlDocument doc_sec = WebServiceHelper.QuerySoapWebService(posturl, GlobalVar.MethodName, hashtable);
                    his_rtnxml = doc_sec.InnerText;
                }
                else if (callmode == "1")//api
                {
                    string secretkey = EncryptionKeyCore.KeyData.AESKEY(HOS_ID);
                    string encryxml = AESExample.AESEncrypt(inxml, secretkey);
                    string signature = EncryptionKeyCore.MD5Helper.Md5(encryxml + secretkey);
                    indata apiin = new indata();
                    apiin.user_id = HOS_ID;
                    apiin.xmlstr = encryxml;
                    apiin.signature = signature;
                    var http = new HttpClient(posturl);
                    string out_data = "";
                    int status = http.SendJson(encryxml, Encoding.UTF8, out out_data);
                    if (status == 200)
                    {
                        outdata outdata = JSONSerializer.Deserialize<outdata>(out_data);
                        his_rtnxml = outdata.outxml;
                    }
                    else
                    {
                        ModLogHosError modLogHos = new ModLogHosError();
                        modLogHos.inTime = intime;
                        modLogHos.inXml = inxml;
                        modLogHos.outTime = DateTime.Now;
                        modLogHos.outXml = out_data;
                        new Log.Core.MySQLDAL.DalLogHosError().Add(modLogHos);
                        his_rtnxml = out_data;
                        return false;
                    }
                }
                if (use_encryption == "1")
                {
                    string secretkey = EncryptionKeyCore.KeyData.AESKEY(HOS_ID);
                    his_rtnxml = AESExample.AESDecrypt(his_rtnxml, secretkey);
                }
                if (DoBussiness == "1")
                {
                    ModLogHos modLogHos = new ModLogHos();
                    modLogHos.inTime = intime;
                    modLogHos.inXml = inxml;
                    modLogHos.outTime = DateTime.Now;
                    modLogHos.outXml = his_rtnxml;
                    new Log.Core.MySQLDAL.DalLogHos().Add(modLogHos);
                }
            }
            catch (Exception ex)
            {
                ModLogHosError modLogHos = new ModLogHosError();
                modLogHos.inTime = intime;
                modLogHos.inXml = inxml;
                modLogHos.outTime = DateTime.Now;
                modLogHos.outXml = his_rtnxml;
                new Log.Core.MySQLDAL.DalLogHosError().Add(modLogHos);
                his_rtnxml = ex.ToString();
                return false;
            }
            return true;
        }

        public static void WriteLog(string type, string className, string content)
        {
            string path = "";
            try
            {
                // path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\MySpringlog";
                path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "PasSLog", "ZzjLog");
            }
            catch (Exception ex)
            {
                //   path = HttpContent.Current.Server.MapPath("MySpringlog");
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

        public static string GETHISYLCARDTYPE(string YLCARD_TYPE)
        {
            switch (YLCARD_TYPE)
            {
                case "1":
                    YLCARD_TYPE = "6"; break;
                case "2":
                    YLCARD_TYPE = "2"; break;
                case "4":
                    YLCARD_TYPE = "4"; break;
                default:
                    YLCARD_TYPE = ""; break;
            }
            return YLCARD_TYPE;
        }
        //public static bool CallService_Core(string HOS_ID, string inxml, ref string his_rtnxml)
        //{
        //    DateTime InTime = DateTime.Now;
        //    try
        //    {
        //        string secretkey = "";
        //        secretkey = EncryptionKeyCore.KeyData.AESKEY(HOS_ID);
        //        string encryxml = AESExample.AESEncrypt(inxml, secretkey);
        //        string signature = EncryptionKeyCore.MD5Helper.Md5(encryxml + secretkey);

        //        string webServiceUrl = GlobalVar.posturl;
        //        // 创建 HTTP 绑定对象
        //        var binding = new BasicHttpBinding();
        //        //设置最大传输接受数量
        //        binding.MaxReceivedMessageSize = 2147483647;
        //        // 根据 WebService 的 URL 构建终端点对象
        //        var endpoint = new EndpointAddress(webServiceUrl);
        //        // 创建调用接口的工厂，注意这里泛型只能传入接口 添加服务引用时生成的 webservice的接口 一般是 (XXXSoap)
        //        var factory = new ChannelFactory<zzjserver.ServiceSoap>(binding, endpoint);
        //        // 从工厂获取具体的调用实例
        //        var callClient = factory.CreateChannel();
        //        //调用的对应webservice 服务类的函数生成对应的请求类Body (一般是webservice 中对应的方法+RequestBody  如GetInfoListRequestBody)
        //        //zzjserver.BusinessZZJRequest body = new zzjserver.BusinessZZJRequest();
        //        //body.signature = xmlstr;
        //        //获取请求对象 （一般是webservice 中对应的方法+tRequest  如GetInfoListRequest）
        //        zzjserver.MySoapHeader MySoapHeader = new zzjserver.MySoapHeader();
        //        MySoapHeader.PassWord = "";
        //        MySoapHeader.UserName = "";
        //        var request = new zzjserver.BusinessZZJRequest(MySoapHeader, encryxml, HOS_ID, signature);
        //        //发送请求
        //        var v = callClient.BusinessZZJAsync(request);
        //        //异步等待
        //        v.Wait();
        //        //获取数据
        //        his_rtnxml = v.Result.BusinessZZJResult;

        //        if (use_encryption == "1")
        //        {
        //            his_rtnxml = AESExample.AESDecrypt(his_rtnxml, secretkey);
        //        }
        //        Log.Core.Model.ModLogHos modLogHos = new Log.Core.Model.ModLogHos();
        //        modLogHos.inTime = InTime;
        //        modLogHos.outTime = DateTime.Now;
        //        modLogHos.inXml = inxml;
        //        modLogHos.outXml = his_rtnxml;
        //        Log.Core.LogHelper.Addlog(modLogHos);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Core.Model.ModLogHos modLogHos = new Log.Core.Model.ModLogHos();
        //        modLogHos.inTime = InTime;
        //        modLogHos.outTime = DateTime.Now;
        //        modLogHos.inXml = inxml;
        //        modLogHos.outXml = ex.ToString();
        //        Log.Core.LogHelper.Addlog(modLogHos);
        //        his_rtnxml = ex.ToString();
        //        return false;
        //    }
        //}


        public static bool CallService_Core(string HOS_ID, string inxml, ref string his_rtnxml)
        {
            DateTime InTime = DateTime.Now;
            try
            {
                /*
                string secretkey = "";
                secretkey = EncryptionKeyCore.KeyData.AESKEY(HOS_ID);
                string encryxml = AESExample.AESEncrypt(inxml, secretkey);
                string signature = EncryptionKeyCore.MD5Helper.Md5(encryxml + secretkey);

                string webServiceUrl = GlobalVar.posturl;
                // 创建 HTTP 绑定对象
                var binding = new BasicHttpBinding();
                //设置最大传输接受数量
                binding.MaxReceivedMessageSize = 2147483647;
                var endpoint = new EndpointAddress(webServiceUrl);
                WriteLog("webreponse","入参", "x"+ inxml);
                zzjserver.ServiceSoapClient client = new zzjserver.ServiceSoapClient(binding, endpoint);
                zzjserver.MySoapHeader MySoapHeader = new zzjserver.MySoapHeader();
                MySoapHeader.PassWord = "";
                MySoapHeader.UserName = "";
                WriteLog("webreponse", "入参", "xx"+ inxml);
                Task<zzjserver.BusinessZZJResponse> response = client.BusinessZZJAsync(MySoapHeader,encryxml, HOS_ID, signature);
                WriteLog("webreponse", "出参", "xxx"+ response.ToString());
                his_rtnxml = response.Result.BusinessZZJResult;
                WriteLog("webreponse", "出参", "xxxx"+ inxml + ";" + his_rtnxml);

                if (use_encryption == "1")
                {
                    his_rtnxml = AESExample.AESDecrypt(his_rtnxml, secretkey);
                }
                Log.Core.Model.ModLogHos modLogHos = new Log.Core.Model.ModLogHos();
                modLogHos.inTime = InTime;
                modLogHos.outTime = DateTime.Now;
                modLogHos.inXml = inxml;
                modLogHos.outXml = his_rtnxml;
                Log.Core.LogHelper.Addlog(modLogHos);
                */
                return true;
            }
            catch (Exception ex)
            {
                Log.Core.Model.ModLogHos modLogHos = new Log.Core.Model.ModLogHos();
                modLogHos.inTime = InTime;
                modLogHos.outTime = DateTime.Now;
                modLogHos.inXml = inxml;
                modLogHos.outXml = ex.ToString();
                Log.Core.LogHelper.Addlog(modLogHos);
                his_rtnxml = ex.ToString();
                return false;
            }
        }

    }


}
