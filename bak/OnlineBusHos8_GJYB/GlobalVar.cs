using Newtonsoft.Json.Linq;
using OnlineBusHos8_GJYB.BUS;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace OnlineBusHos8_GJYB
{
    class GlobalVar
    {

        public static string BusinessClass = ""; // GetConfig("BusinessClass");

        public static Type type = null;//Assembly.GetExecutingAssembly().GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace+".BUS." + BusinessClass);
        public static object obj = null;//Activator.CreateInstance(type);
        public static IBusiness business = null;//(IBusiness)obj;

        //public  IBusiness business = RegisterType(type).As<IBusiness>().SingleInstance();
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
            BusinessClass = GetConfig("BusinessClass");
            Type type = Assembly.GetExecutingAssembly().GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".BUS." + BusinessClass);
            obj = Activator.CreateInstance(type);
            business = (IBusiness)obj;
        }

        /// <summary>
        /// 医保接口入口
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <param name="inputRoot"></param>
        /// <returns></returns>
        public static Models.OutputRoot YBTrans(string HOS_ID, Models.InputRoot inputRoot)
        {
            Models.OutputRoot outputRoot = new Models.OutputRoot();
            //调用医保
            bool flag = CSBHelper.CallCSBService(inputRoot, out outputRoot);
            return outputRoot;
        }
 
        public static JObject GetVaildCard(string pat_card_out)
        {
            try
            {
                JObject jb = JObject.Parse(pat_card_out);
                //paus_insu_date 是null或大于今天 筛选条件
                //取列表第一个
                JArray insuinfo = JArray.Parse(jb["insuinfo"].ToString());
                insuinfo = new JArray(insuinfo.OrderBy(x => x["insutype"]));
                string insuplc_admdvs = "";
                foreach (var j in insuinfo)
                {
                    if (insuplc_admdvs == "" && (FormatHelper.GetStr(j["paus_insu_date"]) == "" || FormatHelper.GetStr(j["paus_insu_date"]).CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) > 0))
                    {
                        return JObject.Parse(j.ToString());
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
