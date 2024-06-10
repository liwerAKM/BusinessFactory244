﻿using System;
using System.Web;
using System.Xml;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace OnlineBusHos153_Report
{
    /// <summary>
    ///  利用WebRequest/WebResponse进行WebService调用的类
    /// </summary>
    public class WebServiceHelper1
    {
        //<webServices>
        //  <protocols>
        //    <add name="HttpGet"/>
        //    <add name="HttpPost"/>
        //  </protocols>
        //</webServices>
        private static Hashtable _xmlNamespaces = new Hashtable();//缓存xmlNamespace，避免重复调用GetNamespace

        /// <summary>
        /// 需要WebService支持Post调用
        /// </summary>
        public static XmlDocument QueryPostWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            byte[] data = EncodePars(Pars);
            WriteRequestData(request, data);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>
        /// 需要WebService支持Get调用
        /// </summary>
        public static XmlDocument QueryGetWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName + "?" + ParsToString(Pars));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            return ReadXmlResponse(request.GetResponse());
        }


        /// <summary>
        /// 通用WebService调用(Soap),参数Pars为String类型的参数名、参数值
        /// </summary>
        public static XmlDocument QuerySoapWebService(String URL, String MethodName, Hashtable Pars,string ss)
        {
            if (_xmlNamespaces.ContainsKey(URL))
            {
                return QuerySoapWebService(URL, MethodName, Pars, _xmlNamespaces[URL].ToString(),ss);
            }
            else
            {
                return QuerySoapWebService(URL, MethodName, Pars, GetNamespace(URL),ss);
            }
        }

        /// <summary>
        /// 通用WebService调用(Soap)
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="MethodName"></param>
        /// <param name="Pars"></param>
        /// <param name="XmlNs"></param>
        /// <returns></returns>
        private static XmlDocument QuerySoapWebService(String URL, String MethodName, Hashtable Pars, string XmlNs, string ss)
        {
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "入参x", ss);
            _xmlNamespaces[URL] = XmlNs;//加入缓存，提高效率
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"" + XmlNs + (XmlNs.EndsWith("/") ? "" : "/") + MethodName + "\"");
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "入参xx", ss);
            SetWebRequest(request);
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "入参xxx", ss);
            byte[] data = EncodeParsToSoap(Pars, XmlNs, MethodName);
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "入参xxxx", ss);
            WriteRequestData(request, data);
            XmlDocument doc = new XmlDocument(), doc2 = new XmlDocument();
            string tmp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            GlobalVar.WriteLog("WebService", tmp + "入参", ss);
            WebResponse response = request.GetResponse();
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "出参", ss);
            doc = ReadXmlResponse(response);
            request.Abort();
            response.Close();
            return doc;
        }

        /// <summary>
        /// 通过WebService的WSDL获取XML名称空间
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private static string GetNamespace(String URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + "?WSDL");
            SetWebRequest(request);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();
            return doc.SelectSingleNode("//@targetNamespace").Value;
        }

        /// <summary>
        /// 动态生成SOP请求报文内容
        /// </summary>
        /// <param name="Pars"></param>
        /// <param name="XmlNs"></param>
        /// <param name="MethodName"></param>
        /// <returns></returns>
        private static byte[] EncodeParsToSoap(Hashtable Pars, String XmlNs, String MethodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            AddDelaration(doc);
            XmlElement soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            XmlElement soapMethod = doc.CreateElement(MethodName);
            soapMethod.SetAttribute("xmlns", XmlNs);
            foreach (string k in Pars.Keys)
            {
                XmlElement soapPar = doc.CreateElement(k);
                soapPar.InnerXml = ObjectToSoapXml(Pars[k]);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        /// <summary>
        /// 将对象转换成XML节点格式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }

        /// <summary>
        /// 设置WEB请求
        /// </summary>
        /// <param name="request"></param>
        private static void SetWebRequest(HttpWebRequest request)
        {
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "入参xx", "Credentials");
            request.Credentials = CredentialCache.DefaultCredentials;
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "入参xx", "Timeout");
            request.Timeout = 120000;
            GlobalVar.WriteLog("WebService", DateTime.Now.ToString("yyyyMMddHHmmssfff") + "入参xx", "ServicePoint.ConnectionLimit");
            request.ServicePoint.ConnectionLimit = 200;
        }

        /// <summary>
        /// 设置请求数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        /// <summary>
        /// 获取字符串的UTF8码字符串
        /// </summary>
        /// <param name="Pars"></param>
        /// <returns></returns>
        private static byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }

        /// <summary>
        /// 将Hashtable转换成WEB请求键值对字符串
        /// </summary>
        /// <param name="Pars"></param>
        /// <returns></returns>
        private static String ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取Webservice响应报文XML
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        /// <summary>
        /// 设置XML文档版本声明
        /// </summary>
        /// <param name="doc"></param>
        private static void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
        }
    }
}
