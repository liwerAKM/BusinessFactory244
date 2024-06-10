using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionKeyCore
{
    public class MD5Helper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Md5(string s)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToUpper();
            }
        }
    }
}
