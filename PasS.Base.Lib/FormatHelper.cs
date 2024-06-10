using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PasS.Base.Lib
{
  public   class FormatHelper
    {
        
        /// <summary>
        ///  验证电话号码的 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static  bool IsTelephone(string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        ///  验证手机号码 
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,5]+\d{9}");
        }
        /// <summary>
        ///  验证身份证号 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsIDcard(string str_idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }
        /// <summary>
        /// 验证输入为数字
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsNumber(string str_number)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
        }
        /// <summary>
        ///  验证邮编 
        /// </summary>
        /// <param name="str_postalcode"></param>
        public static bool IsPostalcode(string str_postalcode)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }
    }

    public static class BooleanExtend
    {
        /// <summary>
        /// True 为 1 ；False 为0
        /// </summary>
        /// <param name="bll"></param>
        /// <returns></returns>
        public static string ToString1(this Boolean bll)
        {
            return bll ? "1" : "0";
        }
    }
    public static class DateTimeExtend
    {

        /// <summary>   
        /// 根据条件过滤表   
        /// </summary>   
        /// <param name="dt">未过滤之前的表</param>   
        /// <param name="filter">过滤条件</param>   
        /// <returns>返回过滤后的表</returns>   
        static public DataTable GetNewTable(this DataTable dt, string filter)
        {
            #region 根据条件过滤表
            DataTable newTable = dt.Clone();
            DataRow[] drs = dt.Select(filter);
            foreach (DataRow dr in drs)
            {
                object[] arr = dr.ItemArray;
                DataRow newrow = newTable.NewRow();
                for (int i = 0; i < arr.Length; i++)
                    newrow[i] = arr[i];
                newTable.Rows.Add(newrow);
            }
            return newTable;
            #endregion
        }
        /// <summary>
        /// 获取一个时间和2020-01-01 :00:00:00之间的毫秒差
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTotalMilliseconds(this DateTime time)
        {
            return (long)(time - DateTime.Parse("2020-01-01")).TotalMilliseconds;

        }
        /// <summary>
        /// 获取一个时间和2020-01-01 :00:00:00之间的毫秒差
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime  FromTotalMilliseconds(this long Milliseconds)
        {
            return DateTime.Parse("2020-01-01").AddMilliseconds(Milliseconds);

        }
        /// <summary>
        /// HH:mm
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToTime1(this DateTime dt)
        {
            return dt.ToString("HH:mm");
        }
        /// <summary>
        /// HH:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToTime2(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }
        /// <summary>
        /// yyyy.MM.dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDate1(this DateTime dt)
        {
            return dt.ToString("yyyy.MM.dd");
        }
        /// <summary>
        ///获取当天的开始时间yyyy-MM-dd 00:00:00.000
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateStart(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd 00:00:00.000");
        }
        /// <summary>
        ///获取当天的结束时间yyyy-MM-dd 23:59:59.998
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateEnd(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd 23:59:59.998");
        }
        /// <summary>
        ///获取当前时间yyyyMMddHHmmssfff
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateTime1(this DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        ///DateTime转Unix时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ToUnixDate(this DateTime dt)
        {
            return    (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        
    }
    public static class StringExtend
    {

        /// <summary>
        ///Unix时间戳转DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime UnixToDateTime(this string UnixTime)
        {
            try
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                return startTime.AddSeconds(double.Parse(UnixTime));
            }
            catch
            {
                return TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            }
        }
        /// <summary>
        /// 右对齐此实例中的字符 中文为两个字符，在左边用指定的 Unicode 字符填充以达到指定的总长度。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="totalByteCount"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string PadLeftEx(this string str, int totalByteCount, char c)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadLeft(totalByteCount - dcount, c);
            return w;
        }
        /// <summary>
        /// 左对齐此字符串中的字符 中文为两个字符，在右边用指定的 Unicode 字符填充以达到指定的总长度。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="totalByteCount">结果字符串中的字符数，等于原始字符数加上任何其他填充字符。</param>
        /// <param name="c">填充字符</param>
        /// <returns> 等效于此实例的一个新 System.String，但它是左对齐的，并在右边用达到 totalWidth 长度所需数目的 paddingChar 字符进行填充。如果 totalWidth 小于此实例的长度，则为与此实例相同的新 System.String。</returns>
        public static string PadRightEx(this string str, int totalByteCount, char c)
        {

            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount, c);
            return w;
        }
        /// <summary>
        /// 右对齐此字符串中的字符 中文为两个字符，在右边用指定的 Unicode 字符填充以达到指定的总长度。如果长度超过则截取。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="totalByteCount">结果字符串中的字符数，等于原始字符数加上任何其他填充字符。</param>
        /// <param name="c">填充字符</param>
        /// <returns> 等效于此实例的一个新 System.String，但它是左对齐的，并在右边用达到 totalWidth 长度所需数目的 paddingChar 字符进行填充。如果 totalWidth 小于此实例的长度，则为与此实例相同的新 System.String。</returns>
        public static string PadRightExSub(this string str, int totalByteCount, char c)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            int Lennow = 0;
            string newstr = "";
            foreach (char ch in str.ToCharArray())
            {
                Lennow += coding.GetByteCount(ch.ToString());
                if (Lennow > totalByteCount)
                {
                    return newstr;
                }
                newstr += ch.ToString();
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount, c);
            return w;
        }


        /// <summary>
        /// 获取中英文混排字符串的实际长度(字节数)
        /// </summary>
        /// <param name="str">要获取长度的字符串</param>
        /// <returns>字符串的实际长度值（字节数）</returns>
        public static int LengthEx(this string str)
        {
            if (str.Equals(string.Empty))

                return 0;
            int strlen = 0;

            ASCIIEncoding strData = new ASCIIEncoding();

            byte[] strBytes = strData.GetBytes(str);

            for (int i = 0; i <= strBytes.Length - 1; i++)
            {
                if (strBytes[i] == 63)
                    strlen++;
                strlen++;

            }
            return strlen;
        }
        /// <summary>
        /// 获取字符串中指定位置开始的指定长度的字符串，支持汉字英文混合 汉字为2字节计数
        /// </summary>
        /// <param name="strSub">输入中英混合字符串</param>
        /// <param name="start">开始截取的起始位置</param>
        /// <param name="length">要截取的字符串长度</param>
        /// <returns></returns>
        public static string SubStringEx(this string strSub, int start, int length)
        {
            string temp = strSub;
            int j = 0, k = 0, p = 0;

            CharEnumerator ce = temp.GetEnumerator();
            while (ce.MoveNext())
            {
                j += (ce.Current > 0 && ce.Current < 255) ? 1 : 2;

                if (j <= start)
                {
                    p++;
                }
                else
                {
                    if (j == temp.LengthEx())
                    {
                        temp = temp.Substring(p, k + 1);
                        break;
                    }
                    if (j <= length + start)
                    {
                        k++;
                    }
                    else
                    {
                        temp = temp.Substring(p, k);
                        break;
                    }
                }
            }

            return temp;
        }

        /// <summary>
        /// 转换为半角字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }


        /// <summary>
        ///  验证电话号码的 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool IsTelephone(this string str_telephone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        ///  验证手机号码 
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsMobileNo(this string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,4,5,7,8,9]+\d{9}");
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static bool IsEmail(this string strIn)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }


        /// <summary>
        /// 验证IP地址是否合法
        /// </summary>
        /// <param name="IP">要验证的IP地址</param>
        public static bool IsIP(this string IP)
        {
            //如果为空，认为验证不合法
            if (string.IsNullOrWhiteSpace(IP))
            {
                return false;
            }
            return Regex.IsMatch(IP, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 防SQL注入过滤危险字符信息 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSQLChar(this string str)
        {
            if (str == String.Empty)
                return String.Empty;
            str = str.Replace("'", "");
            str = str.Replace(";", "");
            str = str.Replace(",", "");
            str = str.Replace("?", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("@", "");
            str = str.Replace("=", "");
            str = str.Replace("+", "");
            str = str.Replace("*", "");
            str = str.Replace("&", "");
            str = str.Replace("#", "");
            str = str.Replace("%", "");
            str = str.Replace("$", "");
            str = str.Replace("$", "");
            
            //删除与数据库相关的词
            str = Regex.Replace(str, "select", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "insert", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete from", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "count", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop table", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "asc", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "char", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "exec master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            //str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net user", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "or", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "-", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "script", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "update", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "chr", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "declare", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "create", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "tables", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, ";", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "sleep", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "create", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "alter", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "grant", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "1=1", "", RegexOptions.IgnoreCase);

            str = Regex.Replace(str, " ", "", RegexOptions.IgnoreCase);
            return str;
        }

        /// <summary>
        /// 防SQL注入过滤where 后条件中的危险字符信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSQLWhere(this string str)
        {
            if (str == String.Empty)
                return String.Empty;
           

            //删除与数据库相关的词
            str = Regex.Replace(str, "select ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "insert ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete from ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "count ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop table ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "asc", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "char", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "exec master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net user", "", RegexOptions.IgnoreCase);
            //str = Regex.Replace(str, "net", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "-", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "script ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "update ", "", RegexOptions.IgnoreCase);
          //  str = Regex.Replace(str, "and", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "chr", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "master ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "declare ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "create ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "tables ", "", RegexOptions.IgnoreCase);
           // str = Regex.Replace(str, ";", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "sleep", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "alter", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "grant", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "1=1", "", RegexOptions.IgnoreCase);
            return str;
        }


        /// <summary>
        /// 防SQL注入过滤 危险字符信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSQLSimple(this string str)
        {
            if (str == String.Empty)
                return String.Empty;


            //删除与数据库相关的词
            str = Regex.Replace(str, "select ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "show  ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "drop ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "delete ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "truncate ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "exec master", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "net localgroup administrators", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, " 1=1", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "sleep", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "update ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "script ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "grant ", "", RegexOptions.IgnoreCase);
            str = Regex.Replace(str, "mid", "", RegexOptions.IgnoreCase);
            
            return str;
        }

        /// <summary>
        ///获取32位Md5, UTF8格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(this string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static string GetMD5(this string value)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToUpper();
            }
        }
    }

}
