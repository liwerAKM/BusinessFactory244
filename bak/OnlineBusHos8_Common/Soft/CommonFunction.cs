using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace OnlineBusHos8_Common
{

    /// <summary>
    /// qrcode解析入参实体
    /// </summary>
    public class QrCodeAnalysisData
    {
        public string appSign { get; set; }
        public long timestamp { get; set; }
        public string pack { get; set; }
        public string qrCode { get; set; }

    }

    /// <summary>
    /// 病人基本信息
    /// </summary>
    public class HealthCardInfoData
    {
        public string virtualCardNum { get; set; }
        public string realname { get; set; }//姓名
        public string idNumber { get; set; }//身份证
        public string cellphone { get; set; }//手机号
        public string gender { get; set; }
        public string pin { get; set; }
        public string addr { get; set; }//地址



    }


    /// <summary>
    /// 接口返回（病人信息） 无法使用接口
    /// </summary>
    public class RtnModel
    {

        public string code { get; set; }
        public string message { get; set; }
        public HealthCardInfoData data { get; set; }
    }

    public class SLBRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SubBusID { get; set; }

        public string ReslutCode { get; set; }

        public string ResultMessage { get; set; }

    }

    public class GETSATISURL
    {
        public string GROUP_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string GUID { get; set; }
        public string MODULE_ID { get; set; }
        public string SFZ_NO { get; set; }
        public string PAT_NAME { get; set; }
        public string MOBILE_NO { get; set; }
    }

    public class GETSATISURL_Response
    {
        public string CLBZ { get; set; }
        public string CLJG { get; set; }
        public string URL { get; set; }
        public string EVAL { get; set; }
    }

    /// <summary>
    /// 公用方法
    /// </summary>
    public class CommonFunction
    {
        /// <summary>
        /// 获取电子健康卡信息
        /// </summary>
        /// <param name="qrcode">二维码信息</param>
        /// <returns></returns>
        public static RtnModel GetHealthCardInfo(string qrcode)
        {
            //计算时间戳，是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总秒数，能够唯一地标识某一刻的时间
            long iTmp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            //固定值
            string appKey = "07c59ecff78e4273b16d10564da717da";
            string pack = "cn.ac.sec.doctor";

            string[] arr = { appKey, pack, "" + iTmp };
            System.Array.Sort(arr);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (String param in arr)
            {
                sb.Append(param);
            }

            string cl = sb.ToString();


            //MD5转码
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(cl));
            string pwd = "";
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X2");
            }

            //qrcode = "B6B5BD9DF3714114894865A58863235697FE66A2DEC14E96A8454CA642AD8349:1";
            //qrcode = "BFFD6474929A473EA034FE70E15696E3F1F920FFAD274CBB82CDE8C5BB6AFE9E:1";//潘路强
            //qrcode = "6D65E91EAFE8444AAB213A2F53030E878B83345C16D94E50ABAABF1090B53DDA:1";//杜邦

            QrCodeAnalysisData data = new QrCodeAnalysisData();
            data.appSign = pwd;
            data.timestamp = iTmp;
            data.pack = "cn.ac.sec.doctor";
            data.qrCode = qrcode;
            //string RequestPara = JsonConvert.SerializeObject(data);
            string RequestPara = string.Format("appSign={0}&timestamp={1}&pack={2}&qrCode={3}", pwd, iTmp, "cn.ac.sec.doctor", qrcode);

            /*
{
    "data":{
        "virtualCardNum":"5420120170600000002",
        "realname":"李明",
        "idNumber":"110108188305065433",
        "cellphone":"15833064659",
        "gender":1,
        "pin":1234,
        "addr":"无锡?XX 区 XX 街道"
    },
    "code":0,
    "msg":""
}
             */
            // 创建WebRequest调用对象
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\bin\ServiceBUS.dll.config");
            DataSet ds = XMLHelper.X_GetXmlData(doc, "configuration/appSettings");//请求的数据包

            string Url = ds.Tables[0].Rows[5]["value"].ToString().Trim();
            //string Url = "http://192.168.10.48:13314/hospital/v2/user/qrCodeAnalysis";

            WebRequest request = HttpWebRequest.Create(Url);

            // 数据编码为键值对
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            //request.Timeout = 800;//请求超时时间
            // 将调用数据转换为字节数组
            byte[] buf = Encoding.GetEncoding("UTF-8").GetBytes(RequestPara);
            // 设置HTTP头，提交的数据长度
            request.ContentLength = buf.Length;

            // 写入参数内容
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(buf, 0, buf.Length);
                requestStream.Close();

            }



            // 调用返回内容
            string ReturnVal = "";
            // 发起调用操作
            WebResponse response = request.GetResponse();
            // 响应数据流
            Stream stream = response.GetResponseStream();
            // 以UTF-8编码转换为StreamReader
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8")))
            {
                ReturnVal = reader.ReadToEnd();
            }
            HealthCardInfoData healthCardInfoData = new HealthCardInfoData();

            //Newtonsoft.Json.JsonConvert.DeserializeObject<RtnModel>(ReturnVal);

            //ServiceBUS.Log.LogHelper.SaveLogZZJ(DateTime.Now, "1", DateTime.Now, ReturnVal);

            // 读取至结束
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RtnModel>(ReturnVal);

        }
    }
}
