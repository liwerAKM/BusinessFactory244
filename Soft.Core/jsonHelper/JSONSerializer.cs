using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Soft.Core
{
    // 2017-06-02 Wanglei 强JSON序列化由System.Web.Script.Serialization 改为Newtonsoft.Json 解决 Datatable 等不能序列化问题

    public class JSONSerializer
    {
        /// <summary>
        /// 序列号一个对象(将对象转换为字符串)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //return js.Serialize(t);
          return   JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 反序列化 (将字符串转换为对象)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string strJson)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            //return js.Deserialize<T>(strJson);

            return (T)JsonConvert.DeserializeObject(strJson, typeof(T));
        }
    }
}
