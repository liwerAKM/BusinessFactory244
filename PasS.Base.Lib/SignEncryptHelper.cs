using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 签名加密解密
    /// </summary>
    public class SignEncryptHelper
    {
        static string _RSAPrivateKey = null;
        /// <summary>
        /// 平台用RSA私钥  用于签名和解密
        /// </summary>
        public static string RSAPrivateKey
        {
            get
            {
                if (_RSAPrivateKey == null)
                {
                    _RSAPrivateKey = MyPubConstant.RSAPrivateKey;
                }
                return _RSAPrivateKey;
            }
            set
            {
                _RSAPrivateKey = value;
            }
        }

        static string _RSAPriDeprecated = null;
        /// <summary>
        /// 平台用RSA私钥  用于签名和解密
        /// </summary>
        public static string RSAPriDeprecated
        {
            get
            {
                if (_RSAPriDeprecated == null)
                {
                    _RSAPriDeprecated = MyPubConstant.RSAPriDeprecated;
                }
                return _RSAPriDeprecated;
            }
            set
            {
                _RSAPriDeprecated = value;
            }
        }
        static string _RSAPublicKey = null;
        /// <summary>
        /// 平台用RSA公钥  给用户获取
        /// </summary>
        public static string RSAPublicKey
        {
            get
            {
                if (_RSAPublicKey == null)
                {
                    _RSAPublicKey = MyPubConstant.RSAPublicKey;
                }
                return _RSAPublicKey;
            }
            set
            {
                _RSAPublicKey = value;
            }
        }



        /// <summary>
        /// 加密内部使用
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] cipher, int EncryptType, string SEID)
        {
            if (cipher == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return cipher;
            }
            else if (EncryptType == 1)
            {
                if (string.IsNullOrWhiteSpace(SEID) || SEID == "def")
                {
                    return AESHelper.EncryptDefault(cipher);
                }
                else
                {
                    return AESHelper.Encrypt(cipher, KeyData.AESKEY(SEID));
                }
            }
            return cipher;
        }
        /// <summary>
        /// 加密内部使用
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>>
        /// <returns></returns>
        public static string Encrypt(string cipher, int EncryptType, string SEID)
        {
            if (cipher == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return cipher;
            }
            else if (EncryptType == 1)
            {
                if (string.IsNullOrWhiteSpace(SEID) || SEID == "def")
                {
                    return AESHelper.EncryptDefault(cipher);
                }
                else
                {
                    return AESHelper.Encrypt(cipher, KeyData.AESKEY(SEID));
                }
            }

            return cipher;
        }
        /// <summary>
        /// 加密内部使用Java
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>>
        /// <returns></returns>
        public static string EncryptJava(string cipher, int EncryptType, string SEID)
        {
            if (cipher == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return cipher;
            }
            else if (EncryptType == 1)
            {
                if (string.IsNullOrWhiteSpace(SEID) || SEID == "def")
                {
                    return AESHelper.EncryptJava(cipher);
                }
                else
                {
                    return AESHelper.EncryptJava(cipher, KeyData.AESKEY(SEID));
                }
            }
            return cipher;
        }

        /// <summary>
        /// 加密使用Java
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="sAPIUserInfo">sAPIUserInfo </param>>
        /// <returns></returns>
        public static string EncryptJava(string cipher, int EncryptType, SAPIUserInfoCKey sAPIUserInfo, int RSAuserID)
        {
            if (cipher == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return cipher;
            }
            else if (EncryptType == 1)
            {
                // return AESHelper.EncryptJava(cipher, sAPIUserInfo.AESKey);
                if (RSAuserID == 0)
                {
                    return AESHelper.EncryptJava(cipher, sAPIUserInfo.AESKey);
                }
                else if (sAPIUserInfo.AESList != null && sAPIUserInfo.AESList.ContainsKey(RSAuserID))
                {
                    return AESHelper.EncryptJava(cipher, sAPIUserInfo.AESList[RSAuserID]);
                }
            }
            else if (EncryptType == 2)//RSA
            {
                try
                {
                    if (RSAuserID == sAPIUserInfo.RSAPubID)
                    { return CaUtilHelper.Encrypt(cipher, sAPIUserInfo.RSAPubKey); }
                    else
                    {
                        if (sAPIUserInfo.UserRsaList != null && sAPIUserInfo.UserRsaList.ContainsKey(RSAuserID))
                        { return CaUtilHelper.Encrypt(cipher, sAPIUserInfo.UserRsaList[RSAuserID]); }
                    }

                }
                catch (Exception ex)
                {
                }
            }
            return cipher;
        }


        /// <summary>
        /// 解密内部使用
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] plain, int EncryptType, string SEID)
        {
            if (plain == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return plain;
            }
            else if (EncryptType == 1)
            {
                if (string.IsNullOrWhiteSpace(SEID) || SEID == "def")
                {
                    return AESHelper.DecryptDefault(plain);
                }
                else
                {
                    return AESHelper.Decrypt(plain, KeyData.AESKEY(SEID));
                }
            }
            return plain;
        }


        /// <summary>
        /// 解密内部使用
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>
        /// <returns></returns>
        public static string Decrypt(string plain, int EncryptType, string SEID)
        {
            if (plain == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return plain;
            }
            else if (EncryptType == 1)
            {
                if (string.IsNullOrWhiteSpace(SEID) || SEID == "def")
                {
                    return AESHelper.DecryptDefault(plain);
                }
                else
                {
                    return AESHelper.Decrypt(plain, KeyData.AESKEY(SEID));
                }
            }
            return plain;
        }
        /// <summary>
        /// 使用平台RSA
        /// </summary>
        const int ID_RTypeRSAPrivateKey = -1;
        /// <summary>
        /// 使用平台过期RSA
        /// </summary>
        const int ID_RSAPriDeprecated = -2;

        /// <summary>
        /// AES解密（与java一致）
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>
        /// <returns></returns>
        public static string DecryptJave(string plain, int EncryptType, string SEID)
        {
            if (plain == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return plain;
            }
            else if (EncryptType == 1)
            {
                if (string.IsNullOrWhiteSpace(SEID) || SEID == "def")
                {
                    return AESHelper.DecryptJava(plain);
                }
                else
                {
                    return AESHelper.DecryptJava(plain, KeyData.AESKEY(SEID));
                }
            }
            return plain;
        }
        /// <summary>
        /// AES解密（与java一致） （用RSA私钥）
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>
        /// <param name="AESID">因AES平台和客户端签名加密是使用同一个，所以如果验证签名时已经判断了AESID 这里就可以直接使用</param>
        /// <returns></returns>
        public static string DecryptJave(string plain, int EncryptType, SAPIUserInfoCKey sAPIUserInfo, out int RSAPriID, int AESID = 0)
        {
            RSAPriID = 0;
            if (plain == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return plain;
            }
            else if (EncryptType == 1)
            {
                if (AESID == 0)//0表示当前AES Key
                {
                    return AESHelper.DecryptJava(plain, sAPIUserInfo.AESKey);
                }
                else if (sAPIUserInfo.AESList != null && sAPIUserInfo.AESList.ContainsKey(AESID))
                {
                    RSAPriID = AESID;
                    return AESHelper.DecryptJava(plain, sAPIUserInfo.AESList[AESID]);
                }
                else
                {
                    return AESHelper.DecryptJava(plain, sAPIUserInfo.AESKey);
                }
            }
            else if (EncryptType == 2)//RSA解密
            {
                if (string.IsNullOrEmpty(sAPIUserInfo.SpringRSAPriKey))//私钥为空，使用默认RSA
                {
                    RSAPriID = ID_RTypeRSAPrivateKey;
                    try
                    {
                        return CaUtilHelper.Decrypt(plain, RSAPrivateKey);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.StartsWith("参数错误。") && !string.IsNullOrEmpty(RSAPriDeprecated))//当前私钥无法解密，尝试用过期私钥解密
                        {
                            try
                            {
                                RSAPriID = ID_RSAPriDeprecated;
                                return CaUtilHelper.Decrypt(plain, RSAPriDeprecated);
                            }
                            catch
                            {
                                throw ex;
                            }
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
                else//使用独有私钥
                {
                    try
                    {//尝试使用当前默认私钥
                        RSAPriID = sAPIUserInfo.SpringRSAID;
                        return CaUtilHelper.Decrypt(plain, sAPIUserInfo.SpringRSAPriKey);
                    }
                    catch (Exception ex)
                    {
                        foreach (int IDRType in sAPIUserInfo.SpringRsaList.Keys)
                        {
                            if (IDRType != sAPIUserInfo.SpringRSAID)
                            {
                                try
                                {
                                    RSAPriID = IDRType;
                                    return CaUtilHelper.Decrypt(plain, sAPIUserInfo.SpringRsaList[IDRType]);
                                }
                                catch
                                {
                                }
                            }
                        }
                        throw ex;
                    }
                }
            }
            return plain;
        }


        /// <summary>
        /// 签名 （用RSA私钥）
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="EncryptType">签名类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="sAPIUserInfo"></param>
        /// <returns></returns>
        public static string Sign(string encryptString, int EncryptType, SAPIUserInfoCKey sAPIUserInfo, int RSAPriID)
        {
            if (EncryptType == 1)
            {
                // return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptString + sAPIUserInfo.AESKey, "MD5");
                if (RSAPriID == 0)
                {
                    return MD5Hash(encryptString, sAPIUserInfo.AESKey);
                }
                else if (sAPIUserInfo.AESList != null && sAPIUserInfo.AESList.ContainsKey(RSAPriID))
                {
                    return MD5Hash(encryptString, sAPIUserInfo.AESList[RSAPriID]);
                }
            }
            else if (EncryptType == 2)
            {
                if (string.IsNullOrEmpty(sAPIUserInfo.SpringRSAPriKey))//私钥为空，使用系统默认RSA
                {
                    if (RSAPriID == ID_RTypeRSAPrivateKey)
                        return CaUtilHelper.Sign(encryptString, RSAPrivateKey);
                    else
                        return CaUtilHelper.Sign(encryptString, RSAPriDeprecated);
                }
                else//使用独有私钥
                {
                    if (RSAPriID == sAPIUserInfo.SpringRSAID)//当前私钥
                    {
                        return CaUtilHelper.Sign(encryptString, sAPIUserInfo.SpringRSAPriKey);
                    }
                    else
                    {
                        if (sAPIUserInfo.SpringRsaList != null && sAPIUserInfo.SpringRsaList.ContainsKey(RSAPriID))
                        {
                            return CaUtilHelper.Sign(encryptString, sAPIUserInfo.SpringRsaList[RSAPriID]);
                        }
                    }

                }
            }
            return "";
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="signature"></param>
        /// <param name="EncryptType">签名类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="sAPIUserInfo"></param>
        /// <returns></returns>
        public static bool SignCheck(string encryptString, string signature, int EncryptType, SAPIUserInfoCKey sAPIUserInfo, out int RSAuserID)
        {
            RSAuserID = 0;
            if (EncryptType == 1)
            {
                //  return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptString + sAPIUserInfo.AESKey, "MD5").Equals(signature));
                if (CheckMD5Sign(encryptString, sAPIUserInfo.AESKey, signature))
                {
                    return true;
                }
                else if (sAPIUserInfo.AESList != null && sAPIUserInfo.AESList.Count > 0)
                {
                    foreach (int key in sAPIUserInfo.AESList.Keys)
                    {
                        if (CheckMD5Sign(encryptString, sAPIUserInfo.AESList[key], signature))
                        {
                            RSAuserID = key;
                            return true;
                        }
                    }
                }
            }
            else if (EncryptType == 2)
            {
                if (CaUtilHelper.CheckSign(encryptString, signature, sAPIUserInfo.RSAPubKey))
                {
                    RSAuserID = sAPIUserInfo.RSAPubID;
                    return true;
                }
                else if (sAPIUserInfo.UserRsaList != null && sAPIUserInfo.UserRsaList.Count > 0)
                {
                    foreach (int key in sAPIUserInfo.UserRsaList.Keys)
                    {
                        if (key != sAPIUserInfo.RSAPubID)
                        {
                            if (CaUtilHelper.CheckSign(encryptString, signature, sAPIUserInfo.UserRsaList[key]))
                            {
                                RSAuserID = key;
                                return true;
                            }
                        }
                    }
                }

            }
            return false;
        }


        /// <summary>
        /// 加密使用指定Key或者公钥 （客户端使用）
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="PubKey">Key  或者公钥</param>>
        /// <returns></returns>
        public static string ClientEncryptJava(string cipher, int EncryptType, string PubKey)
        {
            if (cipher == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return cipher;
            }
            else if (EncryptType == 1)
            {
                return AESHelper.EncryptJava(cipher, PubKey);
            }
            else if (EncryptType == 2)//RSA
            {
                return CaUtilHelper.Encrypt(cipher, PubKey);
            }
            return cipher;
        }
        /// <summary>
        /// 解密（与java一致） （用RSA私钥）（客户端使用）
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="PriKey">Key 或 私钥</param>
        /// <returns></returns>
        public static string ClientDecryptJave(string plain, int EncryptType, string PriKey)
        {
            if (plain == null)
            {
                return null;
            }
            if (EncryptType == 0)
            {
                return plain;
            }
            else if (EncryptType == 1)
            {
                return AESHelper.DecryptJava(plain, PriKey);
            }
            else if (EncryptType == 2)//RSA解密
            {
                return CaUtilHelper.Decrypt(plain, PriKey);
            }
            return plain;
        }


        /// <summary>
        /// 签名 （用RSA私钥） （客户端使用）
        /// </summary>
        /// <param name="encryptString"></param>
        /// <param name="EncryptType">签名类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="PriKey">Key 或私钥 </param>
        /// <returns></returns>
        public static string ClientSign(string encryptString, int EncryptType, string PriKey)
        {
            if (EncryptType == 1)
            {
                return MD5Hash(encryptString, PriKey);
            }
            else if (EncryptType == 2)
            {
                return CaUtilHelper.Sign(encryptString, PriKey);
            }
            return "";
        }
        /// <summary>
        /// 验证签名（客户端使用）
        /// </summary>
        /// <param name="encryptString">待验签字符串</param>
        /// <param name="signature">签名</param>
        /// <param name="EncryptType">签名类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="PubKey">Key  或者公钥</param>
        /// <returns></returns>
        public static bool ClientSignCheck(string encryptString, string signature, int EncryptType, string PubKey)
        {
            if (EncryptType == 1)
            {
                return CheckMD5Sign(encryptString, PubKey, signature);
            }
            else if (EncryptType == 2)
            {
                return CaUtilHelper.CheckSign(encryptString, signature, PubKey);

            }
            return false;
        }


        static bool CheckMD5Sign(string encryptString, string AESKey, string signature)
        {
            return AESHelper.GetMD5(encryptString + AESKey).Equals(signature);
        }

        static string MD5Hash(string encryptString, string AESKey)
        {
            return AESHelper.GetMD5(encryptString + AESKey);
        }


    }
    public class KeyData
    {
        /// <summary>
        /// 获取各医院及APP AES加密KEY
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string AESKEY(string ID)
        {
            if (string.IsNullOrEmpty(ID) || ID.Trim() == "")
            {
                return "";
            }
            string Key;
            switch (ID.Trim().ToUpper())
            {
                case "JSQH": Key = "8478CEFB711D4C00294CBD9BD76D4DFD"; break;//启航用户APP 固定不能修改
                default: Key = AESHelper.GetMD5(ID.Trim().ToUpper() + "J@WK0"); ; break;
            }
            return Key;
        }


        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="encryptString"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static bool AESKEYCheck(string ID, string encryptString, string signature)
        {
            if (string.IsNullOrEmpty(ID) || ID.Trim() == "" || string.IsNullOrEmpty(encryptString))
            {
                return false;
            }
            string Key = AESKEY(ID);

            return AESHelper.GetMD5(encryptString + Key).Equals(signature);
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="encryptString"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public static string Sign(string ID, string encryptString)
        {
            if (string.IsNullOrEmpty(ID) || ID.Trim() == "" || string.IsNullOrEmpty(encryptString))
            {
                return "";
            }
            string Key = AESKEY(ID);
            return AESHelper.GetMD5(encryptString + Key);
        }

    }
}
