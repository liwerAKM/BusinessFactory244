using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Soft.Lib;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Windows.Forms;
 

namespace QHSiInterface
{
    public class CSBHelper
    {


       static string uri = "http://10.72.3.14:8086/CSB";
       static string secretKey = "3ni3n+nWhGCsiu6SITGUqNfzvx8=";
       static string api_access_key = "a94971b5ecee4bb994bce25c9291ccbf";
       static string api_name = "hssServives";
       static string api_version = "1.0.0";
 
        /// <summary>
        /// 定点医药机构编号
        /// </summary>
        static string fixmedins_code = "H32130200063";
        /// <summary>
        /// 接口版本号
        /// </summary>
        static string infver = "V1.0";
        /// <summary>
        /// 经办人类别:1-经办人；2-自助终端；3-移动终端
        /// </summary>
        static string opter_typ = "2";

        //Logger log = new Logger();
        public static string CallService(string infno,string sign_no, object jsonstr)
        {
            string output = string.Empty;
            InputRoot input = new InputRoot()
            {
                infno = infno,// 交易编号
                msgid = fixmedins_code + DateTime.Now.ToString("yyyyMMddHHmmss0fff"),// 发送方报文ID
                mdtrtarea_admvs = "321324",// 就医地医保区划
                insuplc_admdvs = AppData.insuplc_admdvs,// 参保地医保区划
                recer_sys_code = "MBS",// 接收方系统代码
                dev_no = "",// 设备编号
                dev_safe_info = "",//   设备安全信息
                cainfo = "",// 数字签名信息
                signtype = "SM2",//   签名类型
                infver = infver,// 接口版本号
                opter_type = opter_typ,//  经办人类别
                opter = AppData.SiYB.gYBUserID,// 经办人
                opter_name ="ZZJ",// 经办人姓名
                inf_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),//  交易时间
                fixmedins_code = "H32130200090",//  定点医药机构编号
                fixmedins_name = "南京鼓楼医院集团宿迁医院",// 定点医药机构名称
                sign_no = sign_no,//交易签到流水号
                fixmedins_soft_fcty="启航开创软件有限公司",
                input =jsonstr
            };

            string inputjson = Newtonsoft.Json.JsonConvert.SerializeObject(input);
            LogHelper.BusinessLog("业务入参", inputjson);

            //log.Info("业务入参：",inputjson);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/json";
            long currentTicks= DateTime .Now.Ticks;
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string api_timestamp = ((currentTicks - dtFrom.Ticks) / 10000).ToString();
            request.Headers.Add("_api_timestamp", api_timestamp);
            request.Headers.Add("_api_name", api_name);
            //签名计算（按理说这里应该是参数排序）
            Dictionary<string, string> paramdic = new Dictionary<string, string>();
            paramdic.Add("_api_access_key", api_access_key);
            paramdic.Add("_api_name", api_name);
            paramdic.Add("_api_timestamp", api_timestamp);
            paramdic.Add("_api_version", api_version);
            Dictionary<string, string> dicSign = DictionarySort(paramdic);
            //加密串拼接
            string param = string.Join("&", dicSign.Select(x => x.Key + "=" + x.Value).ToArray());
            LogHelper.BusinessLog("签名明文", param);
            HMAC m = HMAC.Create("HMACSHA1");
            m.Key = Encoding.UTF8.GetBytes(secretKey);
            byte[] signData = Encoding.UTF8.GetBytes(param);
            byte[] finalData = m.ComputeHash(signData);
            string signature = Convert.ToBase64String(finalData);
            LogHelper.BusinessLog("签名密文", signature);
            request.Headers.Add("_api_signature", signature);
            request.Headers.Add("_api_version", api_version);
            request.Headers.Add("_api_access_key", api_access_key);

            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(inputjson);
                dataStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            output = reader.ReadToEnd();
            //var body = inputjson;
            //request.AddParameter("application/json", body, ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //output = response.Content;
            LogHelper.BusinessLog("出参", output);
            return output;
        }

        public static bool DoTrade(string infno, string sign_no, object input, ref object output, ref string msg)
        {
        
         
            //string jsonstr = JsonConvert.SerializeObject(input,Formatting.None) ;

            string response = CallService(infno, sign_no, input);

            if (string.IsNullOrEmpty(response)) return false;
            OutPutRoot outputroot = new OutPutRoot();
            try
            {
                 outputroot = JsonConvert.DeserializeObject<OutPutRoot>(response);
            }
            catch (Exception ex)
            { }


            if (outputroot.infcode != 0)
            {
                msg = outputroot.err_msg;
                return false;
            }
            output = response; //response;
            return true;
        }
        /// <summary>
        /// 字典 按 key排序
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static Dictionary<string, string> DictionarySort(Dictionary<string, string> dic)
        {
            return dic.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
        }
    }
}
