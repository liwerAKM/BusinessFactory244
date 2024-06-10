using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;

namespace Soft.Core
{
    /// <summary>
    /// 数据加解密
    /// </summary>
    public class DataSecret
    {
        private const string aesKey = "VNMYOclM*&34534";

        /// <summary>
        /// 通过病人PAT_ID进行身份证号码解密
        /// </summary>
        /// <param name="PAT_ID">病人唯一ID</param>
        /// <returns>解密后的身份证号码</returns>
        public static string DeSfzNoSecretByID(int PAT_ID)
        {
            //DataTable dtPat = GetPatData.GetPatInfo(PAT_ID);
            Model.PAT_INFO pAT_INFO = GetPatData.BasePatInfo(PAT_ID);

            string Secretsfz_no = "";
            if (pAT_INFO != null)
            {
                Secretsfz_no = pAT_INFO.SFZ_SECRET; //dtPat.Rows[0]["SFZ_SECRET"].ToString().Trim();
            }
            //14789位再进行加密
            string decry_result = AESExample.Decrypt(Secretsfz_no, CommonFunction.GetSecretKEY(aesKey));

            if (decry_result.Contains("|"))
            {
                decry_result = decry_result.Substring(0, decry_result.IndexOf('|'));
            }
            else
            {
                decry_result = decry_result.Substring(0, decry_result.Length - 5);
            }
            return decry_result;
        }
        /// <summary>
        /// 通过病人PAT_ID进行手机号码和身份证号码解密
        /// </summary>
        /// <param name="PAT_ID"></param>
        /// <returns>身份证和手机号码dic集合</returns>
        public static Dictionary<string, string> DeMobileAndSfzSecretByID(int PAT_ID)
        {
            //DataTable dtPat = GetPatData.GetPatInfo(PAT_ID);

            Model.PAT_INFO pAT_INFO = GetPatData.BasePatInfo(PAT_ID);


            Dictionary<string, string> dic = new Dictionary<string, string>();
            string SecretMobile = "";
            string Secretsfz_no = "";
            string SecretGsfz_no = "";//监护人身份证号码
            if (pAT_INFO!=null)
            {
                SecretMobile = pAT_INFO.MOBILE_SECRET; //dtPat.Rows[0]["MOBILE_SECRET"].ToString().Trim();
                Secretsfz_no = pAT_INFO.SFZ_SECRET; //dtPat.Rows[0]["SFZ_SECRET"].ToString().Trim();
                SecretGsfz_no = pAT_INFO.G_SFZ_SECRET; //dtPat.Rows[0]["G_SFZ_SECRET"].ToString().Trim();
            }
            string decry_result = AESExample.Decrypt(SecretMobile, CommonFunction.GetSecretKEY(aesKey));
            if (decry_result.Contains("|"))
            {
                decry_result = decry_result.Substring(0, decry_result.IndexOf('|'));
            }
            else if (decry_result == "")
            {

            }
            else
            {
                decry_result = decry_result.Substring(0, decry_result.Length - 4);
            }
            dic.Add("MOBILE_NO", decry_result);
            decry_result = AESExample.Decrypt(Secretsfz_no, CommonFunction.GetSecretKEY(aesKey));
            if (decry_result.Contains("|"))
            {
                decry_result = decry_result.Substring(0, decry_result.IndexOf('|'));
            }
            else if (decry_result == "")
            {

            }
            else
            {
                decry_result = decry_result.Substring(0, decry_result.Length - 5);
            }
            dic.Add("SFZ_NO", decry_result);
            decry_result = SecretGsfz_no == "" ? "" : AESExample.Decrypt(SecretGsfz_no, CommonFunction.GetSecretKEY(aesKey));
            if (decry_result.Contains("|"))
            {
                decry_result = decry_result.Substring(0, decry_result.IndexOf('|'));
            }
            else if (decry_result == "")
            {

            }
            else
            {
                decry_result = decry_result.Substring(0, decry_result.Length - 5);
            }
            dic.Add("GUARDIAN_SFZ_NO", decry_result);
            return dic;
        }
        /// <summary>
        /// 根据病人PAT_ID获取手机号码明文
        /// </summary>
        /// <param name="PAT_ID">病人PAT_ID</param>
        /// <returns>手机号码明文</returns>
        public static string DeMobileSecretByID(int PAT_ID)
        {
            //DataTable dtPat = GetPatData.GetPatInfo(PAT_ID);
            Model.PAT_INFO pAT_INFO = GetPatData.BasePatInfo(PAT_ID);
            string SecretMobile = "";
            if (pAT_INFO!=null)
            {
                SecretMobile = pAT_INFO.MOBILE_SECRET;// dtPat.Rows[0]["MOBILE_SECRET"].ToString().Trim();
            }
            string decry_result = AESExample.Decrypt(SecretMobile, CommonFunction.GetSecretKEY(aesKey));
            if (decry_result.Contains("|"))
            {
                decry_result = decry_result.Substring(0, decry_result.IndexOf('|'));
            }
            else
            {
                decry_result = decry_result.Substring(0, decry_result.Length - 4);
            }
            return decry_result;
            //return decry_result.Substring(0, decry_result.Length - 4);
        }
        /// <summary>
        /// 对手机号码密文进行解密
        /// </summary>
        /// <param name="AesMobile_no"></param>
        /// <returns>手机号码明文</returns>
        public static string DeMobileSecretByAes(string AesMobile_no)
        {
            if (string.IsNullOrEmpty(AesMobile_no))
            {
                return "";
            }
            try
            {
                string decry_result = AESExample.Decrypt(AesMobile_no, CommonFunction.GetSecretKEY(aesKey));
                if (decry_result.Contains("|"))
                {
                    decry_result = decry_result.Substring(0, decry_result.IndexOf('|'));
                }
                else
                {
                    decry_result = decry_result.Substring(0, decry_result.Length - 4);
                }
                return decry_result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 对手机号码明文进行加密
        /// </summary>
        /// <param name="Mobile_no"></param>
        /// <returns>手机号码密文</returns>
        public static string GetMobileSecret(string Mobile_no)
        {
            if (Mobile_no != "" && Mobile_no.Length < 11)
            {
                Mobile_no = Mobile_no + "|";
                Mobile_no = Mobile_no.PadRight(11, '0');
            }
            //14789位再进行加密
            string encry_result = AESExample.AESEncrypt(Mobile_no + Mobile_no[0] + Mobile_no[3] + Mobile_no[6] + Mobile_no[7], CommonFunction.GetSecretKEY(aesKey));
            return encry_result;
        }
        /// <summary>
        /// 对身份证明文进行加密
        /// </summary>
        /// <param name="sfz_no">身份证明文</param>
        /// <returns>身份证密文</returns>
        public static string GetSfzNoSecret(string sfz_no)
        {
            if (sfz_no != "" && sfz_no.Length < 15)
            {
                sfz_no = sfz_no + "|";
                sfz_no = sfz_no.PadRight(15, '0');
            }
            try
            {
                //25689位再进行加密
                string encry_result = AESExample.AESEncrypt(sfz_no + sfz_no[1] + sfz_no[4] + sfz_no[5] + sfz_no[7] + sfz_no[8], CommonFunction.GetSecretKEY(aesKey));
                return encry_result;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 对身份证密文进行解密
        /// </summary>
        /// <param name="AesSfz_no"></param>
        /// <returns>身份证明文</returns>
        public static string DeSfzNoSecretByAes(string AesSfz_no)
        {
            if (string.IsNullOrEmpty(AesSfz_no))
            {
                return "";
            }
            try
            {
                //14789位再进行加密
                string decry_result = AESExample.Decrypt(AesSfz_no, CommonFunction.GetSecretKEY(aesKey));
                if (decry_result.Contains("|"))
                {
                    decry_result = decry_result.Substring(0, decry_result.IndexOf('|'));
                }
                else
                {
                    decry_result = decry_result.Substring(0, decry_result.Length - 5);
                }
                return decry_result;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
