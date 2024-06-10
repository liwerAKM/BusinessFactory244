using BusinessInterface;
using CommonModel;
using Log.Core.Model;
using MySql.Data.MySqlClient;
using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OnlineBusHos8_OutHos
{
    class GlobalVar
    {

        public static string DoBussiness = ""; // GetConfig("DoBussiness");

        public static string callmode = ""; // GetConfig("callmode");

        public static string posturl = ""; // GetConfig("url");

        public static string parameter = ""; // GetConfig("parameters");

        public static string use_encryption = ""; // GetConfig("use_encryption");

        public static string MethodName = ""; // GetConfig("MethodName");

        public static string GetConfig(string configname)
        {
            string key = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "_" + configname;
            Config config = null;
            try
            {
                config = DictionaryCacheHelper.GetCache(key, () => GetConfigClass(configname));
            }
            catch
            {
                config = DictionaryCacheHelper.UpdateCache(key, () => GetConfigClass(configname));
            }
            if (config == null)
            {
                return "";
            }
            else
            {
                TimeSpan ts = new TimeSpan();
                ts = DateTime.Now - config.Time;
                if (ts.Minutes >= 5)
                {
                    config = DictionaryCacheHelper.UpdateCache(key, () => GetConfigClass(configname));
                }
            }
            return config.Value;
        }

        public static Config GetConfigClass(string configname)
        {
            XmlDocument docini = new XmlDocument();
            docini.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".dll.config"));
            DataSet ds = XMLHelper.X_GetXmlData(docini, "configuration/appSettings");//请求的数据包
            DataRow[] dr = ds.Tables[0].Select("key='" + configname + "'");
            if (dr.Length > 0)
            {
                Config C = new Config();
                C.Key = configname;
                C.Value = dr[0]["value"].ToString();
                C.Time = DateTime.Now;
                return C;
            }
            else
            {
                return null;
            }
        }

        public class Config
        {
            public string Key { get; set; }

            public string Value { get; set; }

            public DateTime Time { get; set; }


        }
        public static void Init()
        {
            DoBussiness = GetConfig("DoBussiness");
            callmode = GetConfig("callmode");
            posturl = GetConfig("url");
            parameter = GetConfig("parameters");
            use_encryption = GetConfig("use_encryption");
            MethodName = GetConfig("MethodName");
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
        public static Dictionary<string,string> Get_Filter(string filter)
        {
            Dictionary<string, string> dic_filter = new Dictionary<string, string>();
            try
            {
                if (Soft.Core.FormatHelper.GetStr(filter) != "")
                {
                    try
                    {
                        dic_filter = JSONSerializer.Deserialize<Dictionary<string, string>>(filter);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            { }
            return dic_filter;
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
                    var http = new Soft.Core.HttpClient(posturl);
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

        public static SLBBusinessInfo CallOtherBus(string data, string BUS_ID, string subSubId)
        {
            try
            {
                SLBBusinessInfo OutSLBBOtherBus = new SLBBusinessInfo();
                DateTime nowIn = DateTime.Now;
                SLBBusinessInfo SLBBOtherBus = new SLBBusinessInfo();
                SLBBOtherBus.BusID = BUS_ID;
                SLBBOtherBus.SubBusID = subSubId;
                SLBBOtherBus.BusData = data;

                bool result = BusServiceAdapter.Ipb_CallOtherBusiness(SLBBOtherBus, out OutSLBBOtherBus);

                Log.Core.Model.ModLogAPP modLogAPP = new Log.Core.Model.ModLogAPP();
                modLogAPP.inTime = nowIn;
                modLogAPP.outTime = DateTime.Now;
                modLogAPP.inXml = data;
                modLogAPP.outXml = OutSLBBOtherBus.BusData;
                Log.Core.LogHelper.Addlog(modLogAPP);
                return OutSLBBOtherBus;
            }
            catch (Exception ex)
            {
                Log.Core.Model.ModLogAPPError modLogAPPError = new Log.Core.Model.ModLogAPPError();
                modLogAPPError = new Log.Core.Model.ModLogAPPError();
                modLogAPPError.inTime = DateTime.Now;
                modLogAPPError.outTime = DateTime.Now;
                modLogAPPError.inXml = data;
                modLogAPPError.TYPE = "2020";
                modLogAPPError.outXml = ex.ToString();

                Log.Core.LogHelper.Addlog(modLogAPPError);
                return null;
            }
        }
    }
}
