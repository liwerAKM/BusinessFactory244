﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{

    public class HttpClient
    {

        // 请求的URL
        private string requestUrl = "";

        // 返回结果
        private string result;

        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        public HttpClient(string url)
        {
            requestUrl = url;
        }
        /// <summary>
        ///  Get
        /// </summary>
        /// <param name="GetData"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        public int GetHttpResponse(string GetData, Encoding encoder)
        {
            try
            {
                HttpWebRequest HttpWResp = (HttpWebRequest)WebRequest.Create(requestUrl + "?" + GetData);
                HttpWResp.Method = "GET";
                HttpWResp.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                HttpWResp.UserAgent = null;
                HttpWResp.Timeout = 30000;

                HttpWebResponse response = (HttpWebResponse)HttpWResp.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, encoder);
                result = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return (int)response.StatusCode;
            }
            catch (Exception ex)
            {
                //   throw new Exception(ex.Message);
                return 0;
            }

        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取银联的处理结果
        /// </summary>
        /// <param name="strRequestData">待请求参数数组字符串</param>
        /// <returns>银联处理结果</returns>
        public int Send(string strRequestData, Encoding encoder)
        {

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = encoder.GetBytes(strRequestData);
            HttpWebResponse HttpWResp = null;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                myReq.Timeout = 30 * 1000;
                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();  //获得请求流
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                //发送POST数据请求服务器   
                HttpWResp = (HttpWebResponse)myReq.GetResponse();
                StreamReader sr = new StreamReader(HttpWResp.GetResponseStream(), encoder);
                result = sr.ReadToEnd().Trim();
                sr.Close();


                return (int)HttpWResp.StatusCode;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);
                return 0;
            }

        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取银联的处理结果
        /// </summary>
        /// <param name="strRequestData">待请求参数数组字符串</param>
        /// <returns>银联处理结果</returns>
        public int SendJson(string strRequestData, Encoding encoder)
        {

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = encoder.GetBytes(strRequestData);
            HttpWebResponse HttpWResp = null;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/json;charset=utf-8";
                myReq.Timeout = 30 * 1000;
                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();  //获得请求流
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                //发送POST数据请求服务器   
                HttpWResp = (HttpWebResponse)myReq.GetResponse();
                StreamReader sr = new StreamReader(HttpWResp.GetResponseStream(), encoder);
                result = sr.ReadToEnd().Trim();
                sr.Close();


                return (int)HttpWResp.StatusCode;
            }
            catch (Exception exp)
            {
                result = exp.Message;
                // throw new Exception(exp.Message);
                return -1;
            }

        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取银联的处理结果
        /// </summary>
        /// <param name="strRequestData">待请求参数数组字符串</param>
        /// <returns>银联处理结果</returns>
        public int SendJson(string strRequestData, Encoding encoder, out string out_data)
        {
            out_data = "";
            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = encoder.GetBytes(strRequestData);
            HttpWebResponse HttpWResp = null;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/json;charset=utf-8";
                myReq.Timeout = 60 * 1000;
                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();  //获得请求流
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                //发送POST数据请求服务器   
                HttpWResp = (HttpWebResponse)myReq.GetResponse();
                StreamReader sr = new StreamReader(HttpWResp.GetResponseStream(), encoder);
                result = sr.ReadToEnd().Trim();
                sr.Close();
                out_data = result;
                return (int)HttpWResp.StatusCode;
            }
            catch (Exception exp)
            {
                // throw new Exception(exp.Message);
                return 0;
            }

        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取银联的处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <returns>银联处理结果</returns>
        public int Send(Dictionary<string, string> sParaTemp, Encoding encoder)
        {
            // System.Net.ServicePointManager.Expect100Continue = false;
            //待请求参数数组字符串
            string strRequestData = BuildRequestParaToString(sParaTemp, encoder);
            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = encoder.GetBytes(strRequestData);
            HttpWebResponse HttpWResp = null;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";
                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();  //获得请求流
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();
                //发送POST数据请求服务器                
                HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, encoder);
                StringBuilder responseData = new StringBuilder();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }
                //释放
                myStream.Close();
                result = responseData.ToString();
                return (int)HttpWResp.StatusCode;
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message);

                return 0;
            }

        }
        /// <summary>
        /// 生成要请求给银联的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        private static string BuildRequestParaToString(Dictionary<string, string> sParaTemp, Encoding code)
        {

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = CreateLinkstringUrlencode(sParaTemp, code);

            return strRequestData;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkstringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                //prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }
            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            //Replace方法为兼容目前文件下载接口只能接收大写转义符bug，后续可删除Replace
            // return prestr.ToString().Replace("%2b", "%2B").Replace("%3d", "%3D").Replace("%2f", "%2F");

            return prestr.ToString();
        }


        /// <summary>
        ///  将字符串key1=value1&key2=value2转换为Dictionary数据结构
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<string, string> CoverstringToDictionary(string data)
        {
            if (null == data || 0 == data.Length)
            {
                return null;
            }
            string[] arrray = data.Split(new char[] { '&' });
            Dictionary<string, string> res = new Dictionary<string, string>();
            foreach (string s in arrray)
            {
                int n = s.IndexOf("=");
                string key = s.Substring(0, n);
                string value = s.Substring(n + 1);
                Console.WriteLine(key + "=" + value);
                res.Add(key, value);
            }
            return res;
        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">抓取url</param>
        /// <param name="filePath">保存文件名</param>
        /// <returns></returns>
        public bool HttpDown(string url, string filePath)
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                // req.Referer = oldurl;
                req.UserAgent = @" Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36";
                req.ContentType = "application/octet-stream";
                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();
                // StreamReader readStream=new StreamReader 
                FileStream fs = File.Create(filePath);
                long length = response.ContentLength;
                int i = 0;
                do
                {
                    byte[] buffer = new byte[1024];
                    i = stream.Read(buffer, 0, 1024);
                    fs.Write(buffer, 0, i);
                } while (i > 0);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
