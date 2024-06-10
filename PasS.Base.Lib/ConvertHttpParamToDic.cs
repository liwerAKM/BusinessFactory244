using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 将字符串key1=value1&key2=value2转换为Dictionary数据结构
    /// </summary>
    public class ConvertHttpParamToDic
    {
        public Dictionary<string, string>  GetDic()
        {
            return res;
        }
        Dictionary<string, string> res = new Dictionary<string, string>();
        /// <summary>
        ///  将字符串key1=value1&key2=value2转换为Dictionary数据结构
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ConvertHttpParamToDic(string data)
        {
            if (null == data || 0 == data.Length)
            {
                return;
            }
            if (data.StartsWith("?") && data.Length > 1)
                data = data.Substring(1);
            string[] arrray = data.Split(new char[] { '&' });
            res = new Dictionary<string, string>();
            foreach (string s in arrray)
            {
                int n = s.IndexOf("=");
                if (n > 0)
                {
                    string key = s.Substring(0, n);
                    string value = s.Substring(n + 1);
                    // Console.WriteLine(key + "=" + value);
                    res.Add(key, value);
                }
            }
        }
        public string GetValue(string Key)
        {
            if (res.ContainsKey(Key))
            {
                return res[Key];
            }
            return "";
        }
        public string GetValue(int index)
        {
            if (res.Count > index)
                return res.ElementAt(0).Value;
            return "";
        }
    }

    /// <summary>
    /// 将字符串key1=value1&key2=value2转换为SortedDictionary数据结构
    /// </summary>
    public class ConvertHttpParamToSortedDic
    {
        public SortedDictionary<string, string> GetDic()
        {
            return res;
        }
        SortedDictionary<string, string> res = new SortedDictionary<string, string>();
        /// <summary>
        ///  将字符串key1=value1&key2=value2转换为Dictionary数据结构
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ConvertHttpParamToSortedDic(string data)
        {
            if (null == data || 0 == data.Length)
            {
                return;
            }
            if (data.StartsWith("?") && data.Length > 1)
                data = data.Substring(1);
            string[] arrray = data.Split(new char[] { '&' });
            res = new SortedDictionary<string, string>();
            foreach (string s in arrray)
            {
                int n = s.IndexOf("=");
                if (n > 0)
                {
                    string key = s.Substring(0, n);
                    string value = s.Substring(n + 1);
                    // Console.WriteLine(key + "=" + value);
                    res.Add(key, value);
                }
            }
        }
        public string GetValue(string Key)
        {
            if (res.ContainsKey(Key))
            {
                return res[Key];
            }
            return "";
        }
        public string GetValue(int index)
        {
            if (res.Count > index)
                return res.ElementAt(0).Value;
            return "";
        }
    }

}
