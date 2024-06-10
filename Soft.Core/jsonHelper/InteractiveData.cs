using System;
using Newtonsoft.Json;
using EncryptionKeyCore;
namespace Soft.Core
{
    /// <summary>
    /// 接口数据类 用来实现JSON字符串格式并包含验证函数
    /// 2017-06-02 Wanglei 强JSON序列化由System.Web.Script.Serialization 改为Newtonsoft.Json 解决 Datatable 等不能序列化问题
    /// </summary>
    [Serializable]
    public class InteractiveData
    {

        /// <summary>
        /// 1 调用医院退费接口 2 不调用医院退费接口  0 默认
        /// </summary>
        public int HOS_TF { get; set; }

        /// <summary>
        /// 业务名称 唯一 
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 响应码
        /// </summary>
        public Int32 Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 子错误码
        /// </summary>
        public string SubCode { get; set; }

        /// <summary>
        /// 子错误信息
        /// </summary>
        public string SubMsg { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 支付宝对应配置
        /// </summary>
        public string aliConfig { get; set; }

        /// <summary>
        /// 配置签名
        /// </summary>
        public string SignatureConfig { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 医院代码
        /// </summary>
        public string HOSID { get; set; }

        /// <summary>
        /// 验证签名 用于接收端
        /// </summary>
        /// <param name="AESKey">Key</param>
        /// <returns></returns>
        public bool CheckSignature(string AESKey)
        {
            string newsignature = Encode(AESKey);
            return newsignature.Equals(Signature);
        }

        /// <summary>
        ///  验证签名 用于接收端
        /// </summary>
        /// <returns></returns>
        public bool CheckSignature()
        {
            return KeyData.AESKEYCheck(HOSID, Body, Signature);
        }

        /// <summary>
        /// 生成签名 用于发送端
        /// </summary>
        /// <param name="AESKey"></param>
        public void BuilderSignature(string AESKey)
        {
            Signature = Encode(AESKey);
        }

        /// <summary>
        /// 生成签名 用于发送端
        /// </summary>
        public void BuilderSignature()
        {
            Signature = Encode(KeyData.AESKEY(HOSID));
        }



        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <param name="encryptKey">加密密钥,最长32位</param>
        /// <returns></returns>
        string Encode(string encryptKey)
        {
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Body + encryptKey, "MD5");

            return CommonFunction.Md5Hash(Body + encryptKey);
        }

        /// <summary>
        /// 设置业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetBusinessData<T>(T t)
        {
            if (string.IsNullOrEmpty(HOSID))
            {
                throw new Exception("通过HOSID签名的必须先给HOSID赋值");
            }
             
            Body = AESExample.Encrypt( JsonConvert.SerializeObject( t), KeyData.AESKEY(HOSID));
        }

        /// <summary>
        /// 设置业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="AESKey"></param>
        public void SetBusinessData<T>(T t, string AESKey)
        {
        
            Body = AESExample.Encrypt(JsonConvert.SerializeObject(t), AESKey);
        }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBusinessData<T>()
        {
            string strJson = AESExample.Decrypt(Body, KeyData.AESKEY(HOSID));
            
            return (T)JsonConvert.DeserializeObject(strJson, typeof(T));
        }
        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBusinessDataAliConfig<T>()
        {
            string strJson = AESExample.Decrypt(aliConfig, KeyData.AESKEY(HOSID));
            return (T)JsonConvert.DeserializeObject(strJson, typeof(T));
        }


        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <returns></returns>
        public string  GetBusinessData()
        {
            return   AESExample.Decrypt(Body, KeyData.AESKEY(HOSID));

        }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AESKey"></param>
        /// <returns></returns>
        public T GetBusinessData<T>(string AESKey)
        {
            string strJson = AESExample.Decrypt(Body, AESKey);

            return (T)JsonConvert.DeserializeObject(strJson, typeof(T));
            
        }



        /// <summary>
        /// 将所有数据生成JSON字符串，生成后的数据直接作为参数发送
        /// </summary>
        /// <returns></returns>
        public string BuildJson()
        {
             
            try
            {
                return JsonConvert.SerializeObject(this);// jss.Serialize(this);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        /// <summary>
        /// 将接收到的JSON字符串转换成InteractiveData
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static InteractiveData FromJson(string strJson)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //return (InteractiveData)JsonConvert.DeserializeObject(strJson, typeof(InteractiveData));

            return (InteractiveData)JsonConvert.DeserializeObject(strJson, typeof(InteractiveData));
        }
    }

    /// <summary>
    /// 接口数据类 用来实现JSON字符串格式并包含验证函数
    /// </summary>
    [Serializable]
    public class InteractiveDataOld
    {
        /// <summary>
        /// 业务名称 唯一 
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 响应码
        /// </summary>
        public Int32 Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 子错误码
        /// </summary>
        public string SubCode { get; set; }

        /// <summary>
        /// 子错误信息
        /// </summary>
        public string SubMsg { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 医院代码
        /// </summary>
        public string HOSID { get; set; }

        /// <summary>
        /// 验证签名 用于接收端
        /// </summary>
        /// <param name="AESKey">Key</param>
        /// <returns></returns>
        public bool CheckSignature(string AESKey)
        {
            string newsignature = Encode(AESKey);
            return newsignature.Equals(Signature);
        }

        /// <summary>
        ///  验证签名 用于接收端
        /// </summary>
        /// <returns></returns>
        public bool CheckSignature()
        {
            return KeyData.AESKEYCheck(HOSID, Body, Signature);
        }

        /// <summary>
        /// 生成签名 用于发送端
        /// </summary>
        /// <param name="AESKey"></param>
        public void BuilderSignature(string AESKey)
        {
            Signature = Encode(AESKey);
        }

        /// <summary>
        /// 生成签名 用于发送端
        /// </summary>
        public void BuilderSignature()
        {
            Signature = Encode(KeyData.AESKEY(HOSID));
        }



        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <param name="encryptKey">加密密钥,最长32位</param>
        /// <returns></returns>
        string Encode(string encryptKey)
        {
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Body + encryptKey, "MD5");
            return CommonFunction.Md5Hash(Body + encryptKey);
        }

        /// <summary>
        /// 设置业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void SetBusinessData<T>(T t)
        {
            if (string.IsNullOrEmpty(HOSID))
            {
                throw new Exception("通过HOSID签名的必须先给HOSID赋值");
            }
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //Body = AESExample.Encrypt(js.Serialize(t), KeyData.AESKEY(HOSID));
            Body = AESExample.Encrypt(JsonConvert.SerializeObject(t), KeyData.AESKEY(HOSID));
        }

        /// <summary>
        /// 设置业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="AESKey"></param>
        public void SetBusinessData<T>(T t, string AESKey)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //Body = AESExample.Encrypt(js.Serialize(t), AESKey);
            Body = AESExample.Encrypt(JsonConvert.SerializeObject(t), AESKey);
        }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetBusinessData<T>()
        {
            string strJson = AESExample.Decrypt(Body, KeyData.AESKEY(HOSID));
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //return js.Deserialize<T>(strJson);

            return JsonConvert.DeserializeObject<T>(strJson);
        }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <returns></returns>
        public string GetBusinessData()
        {
            return AESExample.Decrypt(Body, KeyData.AESKEY(HOSID));

        }

        /// <summary>
        /// 获取业务数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AESKey"></param>
        /// <returns></returns>
        public T GetBusinessData<T>(string AESKey)
        {
            string strJson = AESExample.Decrypt(Body, AESKey);
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //return js.Deserialize<T>(strJson);

            return JsonConvert.DeserializeObject<T>(strJson);
        }

        /// <summary>
        /// 将所有数据生成JSON字符串，生成后的数据直接作为参数发送
        /// </summary>
        /// <returns></returns>
        public string BuildJson()
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //return jss.Serialize(this);

                return JsonConvert.SerializeObject(this);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        /// <summary>
        /// 将接收到的JSON字符串转换成InteractiveData
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static InteractiveData FromJson(string strJson)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //return js.Deserialize<InteractiveData>(strJson);
            return JsonConvert.DeserializeObject<InteractiveData>(strJson);
        }
    }
}
