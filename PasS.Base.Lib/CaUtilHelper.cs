using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{


    public class CaUtilHelper
    {


        /** 默认编码字符集 */
        private const string DEFAULT_CHARSET = "UTF-8";//UTF-8 GBK


        /// <summary>
        /// 加密 使用RSA公钥
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="publicKey"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
      public   static string Encrypt(string plaintext, string publicKey, string input_charset = "UTF-8")
        {
            Encoding code = Encoding.GetEncoding(input_charset);

            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider RSACryptography = new RSACryptoServiceProvider();
            RSACryptography.ImportParameters(paraPub);

            Byte[] PlaintextData = code.GetBytes(plaintext);
            int MaxBlockSize = RSACryptography.KeySize / 8 - 11;    //加密块最大长度限制  
            if (PlaintextData.Length <= MaxBlockSize)
                return Convert.ToBase64String(RSACryptography.Encrypt(PlaintextData, false));
            using (MemoryStream PlaiStream = new MemoryStream(PlaintextData))
            using (MemoryStream CrypStream = new MemoryStream())
            {
                Byte[] Buffer = new Byte[MaxBlockSize];
                int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                while (BlockSize > 0)
                {
                    Byte[] ToEncrypt = new Byte[BlockSize];
                    Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);
                    Byte[] Cryptograph = RSACryptography.Encrypt(ToEncrypt, false);
                    CrypStream.Write(Cryptograph, 0, Cryptograph.Length);
                    BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                }
                return Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
            }

        }
        /// <summary>
        /// 加密 使用CA证书
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="pubcrt"></param>
        /// <returns></returns>
        static String CAEncrypt(String plaintext, X509Certificate2 pubcrt, string input_charset = "UTF-8")
        {
            X509Certificate2 _X509Certificate2 = pubcrt;
            using (RSACryptoServiceProvider RSACryptography = _X509Certificate2.PublicKey.Key as RSACryptoServiceProvider)
            {
                Byte[] PlaintextData = Encoding.GetEncoding(input_charset).GetBytes(plaintext);
                int MaxBlockSize = RSACryptography.KeySize / 8 - 11;    //加密块最大长度限制  
                if (PlaintextData.Length <= MaxBlockSize)
                    return Convert.ToBase64String(RSACryptography.Encrypt(PlaintextData, false));
                using (MemoryStream PlaiStream = new MemoryStream(PlaintextData))
                using (MemoryStream CrypStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                    while (BlockSize > 0)
                    {
                        Byte[] ToEncrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);
                        Byte[] Cryptograph = RSACryptography.Encrypt(ToEncrypt, false);
                        CrypStream.Write(Cryptograph, 0, Cryptograph.Length);
                        BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                    }
                    return Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
                }
            }
        }


        /// <summary>
        /// 解密 使用私钥 RSA
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="PrivateKey"></param>
        /// <returns></returns>
        public static String Decrypt(String ciphertext, string PrivateKey, string input_charset = "UTF-8")
        {

            using (RSACryptoServiceProvider RSACryptography = LoadCertificateString(PrivateKey, "RSA"))
            {
                Byte[] CiphertextData = Convert.FromBase64String(ciphertext);
                int MaxBlockSize = RSACryptography.KeySize / 8; //解密块最大长度限制  
                if (CiphertextData.Length <= MaxBlockSize)
                    return Encoding.UTF8.GetString(RSACryptography.Decrypt(CiphertextData, false));
                using (MemoryStream CrypStream = new MemoryStream(CiphertextData))
                using (MemoryStream PlaiStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    while (BlockSize > 0)
                    {
                        Byte[] ToDecrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);
                        Byte[] Plaintext = RSACryptography.Decrypt(ToDecrypt, false);
                        PlaiStream.Write(Plaintext, 0, Plaintext.Length);
                        BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    }
                    return Encoding.GetEncoding(input_charset).GetString(PlaiStream.ToArray());
                }
            }
        }
        /// <summary>
        /// 解密 使用证书
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="prvpfx"></param>
        /// <returns></returns>
        public static String CADecrypt(String ciphertext, X509Certificate2 prvpfx, string input_charset = "UTF-8")
        {
            //string pubK = prvpfx.PublicKey.EncodedParameters.ToString();
            ////  string pubK=   Convert.ToBase64String(bytepub);
            //string keyPublic = prvpfx.PublicKey.Key.ToXmlString(false);  // 公钥  
            //string keyPrivate = prvpfx.PrivateKey.ToXmlString(true);  // 私钥  
            X509Certificate2 _X509Certificate2 = prvpfx;
            using (RSACryptoServiceProvider RSACryptography = _X509Certificate2.PrivateKey as RSACryptoServiceProvider)
            {
                Byte[] CiphertextData = Convert.FromBase64String(ciphertext);
                int MaxBlockSize = RSACryptography.KeySize / 8; //解密块最大长度限制  
                if (CiphertextData.Length <= MaxBlockSize)
                    return Encoding.UTF8.GetString(RSACryptography.Decrypt(CiphertextData, false));
                using (MemoryStream CrypStream = new MemoryStream(CiphertextData))
                using (MemoryStream PlaiStream = new MemoryStream())
                {
                    Byte[] Buffer = new Byte[MaxBlockSize];
                    int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    while (BlockSize > 0)
                    {
                        Byte[] ToDecrypt = new Byte[BlockSize];
                        Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);
                        Byte[] Plaintext = RSACryptography.Decrypt(ToDecrypt, false);
                        PlaiStream.Write(Plaintext, 0, Plaintext.Length);
                        BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                    }
                    return Encoding.GetEncoding(input_charset).GetString(PlaiStream.ToArray());
                }
            }
        }



        /// <summary>
        /// 签名RSA 使用使用私钥
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        public static string Sign(String Signtext, string PrivateKey)
        {
            try
            {

                return Sign(Signtext, PrivateKey, DEFAULT_CHARSET);
            }
            catch
            {
                return " Signtext:" + Signtext + " outKey:" + PrivateKey+ " DEFAULT_CHARSET"+ DEFAULT_CHARSET;
            }
        }

        /// <summary>
        /// 签名RSA 使用使用私钥
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        static string Sign(String Signtext, string PrivateKey, string input_charset)
        {
            string SS = "";
            try
            {
                using (RSACryptoServiceProvider RSACryptography = LoadCertificateString(PrivateKey, "RSA"))
                {
                    Encoding code = Encoding.GetEncoding(input_charset);
                    byte[] Data = code.GetBytes(Signtext);
                    SHA1 sh = new SHA1CryptoServiceProvider();
                    byte[] signData = RSACryptography.SignData(Data, sh);
                    return Convert.ToBase64String(signData, Base64FormattingOptions.None);
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 签名RSA 使用使用私钥 同等于Sign
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        public static string SignT(String Signtext, string PrivateKey, string input_charset)
        {
            using (RSACryptoServiceProvider RSACryptography = LoadCertificateString(PrivateKey, "RSA"))
            {
                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(Signtext);
                byte[] signData = RSACryptography.SignData(Data, "SHA1");
                return Convert.ToBase64String(signData, Base64FormattingOptions.None);
            }
        }

        /// <summary>
        /// 签名RSA2 使用使用私钥 
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        public static string SignRSA2(String Signtext, string PrivateKey)
        {
            return SignRSA2(Signtext, PrivateKey, DEFAULT_CHARSET);
        }

        /// <summary>
        /// 签名RSA2 使用使用私钥  对应验证签名需要用windowsAPI
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        static string SignRSA2(String Signtext, string PrivateKey, string input_charset)
        {
            using (RSACryptoServiceProvider RSACryptography = LoadCertificateString(PrivateKey, "RSA2"))
            {
                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(Signtext);
                SHA256 sh = new SHA256CryptoServiceProvider();
                byte[] signData = RSACryptography.SignData(Data, sh);
                return Convert.ToBase64String(signData, Base64FormattingOptions.None);
            }
        }
        /// <summary>
        /// 签名RSA2 使用使用私钥 同等于 SignRSA2  
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        static string SignRSA2T(String Signtext, string PrivateKey, string input_charset)
        {
            using (RSACryptoServiceProvider RSACryptography = LoadCertificateString(PrivateKey, "RSA2"))
            {
                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(Signtext);
                byte[] signData = RSACryptography.SignData(Data, "SHA256");
                return Convert.ToBase64String(signData, Base64FormattingOptions.None);
            }
        }


        /// <summary>
        /// 签名RSA 使用CA证书
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        static string CASign(String Signtext, X509Certificate2 prvpfx, string input_charset)
        {
            X509Certificate2 _X509Certificate2 = prvpfx;
            using (RSACryptoServiceProvider RSACryptography = _X509Certificate2.PrivateKey as RSACryptoServiceProvider)
            {

                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(Signtext);
                SHA1 sh = new SHA1CryptoServiceProvider();

                // SHA256 sh = new SHA256CryptoServiceProvider();
                byte[] signData = RSACryptography.SignData(Data, sh);
                return Convert.ToBase64String(signData, Base64FormattingOptions.None);
            }
        }
        /// <summary>
        /// 签名SHA256 使用CA证书
        /// </summary>
        /// <param name="Signtext"></param>
        /// <param name="prvpfx"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        static string CASignSHA256(String Signtext, X509Certificate2 prvpfx, string input_charset)
        {
            X509Certificate2 _X509Certificate2 = prvpfx;
            using (RSACryptoServiceProvider key = new RSACryptoServiceProvider())
            {

                key.FromXmlString(prvpfx.PrivateKey.ToXmlString(true));
                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(Signtext);
                byte[] signData = key.SignData(Data, CryptoConfig.MapNameToOID("SHA256"));
                return Convert.ToBase64String(signData, Base64FormattingOptions.None);
            }
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="SignContent">需要验证的内容</param>
        /// <param name="sign">签名结果</param>
        /// <returns></returns>
        public static bool CheckSign(string SignContent, string sign, string VerifySignFileName)
        {
            try
            {
                if (VerifySignFileName.Contains(".cer"))
                {
                    // X509Certificate2 pubcrt = new X509Certificate2(MedicalPayConfig.Cert_PathNJ, MedicalPayConfig.Cert_PassWordNJ);
                    // return CACheckSign(SignContent, sign, pubcrt, DEFAULT_CHARSET);
                    X509Certificate2 pubcrt = new X509Certificate2(VerifySignFileName);
                    return CACheckSign(SignContent, sign, pubcrt, DEFAULT_CHARSET);
                }
                else
                {

                    return CheckSign(SignContent, sign, VerifySignFileName, DEFAULT_CHARSET);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 验证签名RSA 使用公钥
        /// </summary>
        /// <param name="content">需要验证的内容</param>
        /// <param name="signedString">签名结果</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="input_charset">编码格式</param>
        /// <returns></returns>
        static bool CheckSign(string SignContent, string sign, string publicKey, string input_charset)
        {
            bool result = false;

            Encoding code = Encoding.GetEncoding(input_charset);
            byte[] Data = code.GetBytes(SignContent);
            byte[] data = Convert.FromBase64String(sign);
            RSAParameters paraPub = ConvertFromPublicKey(publicKey);
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            rsaPub.ImportParameters(paraPub);

            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(Data, sh, data);
            return result;
        }
        /// <summary>
        /// 验证签名RSA2 使用公钥  
        /// </summary>
        /// <param name="content">需要验证的内容</param>
        /// <param name="signedString">签名结果</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="input_charset">编码格式</param>
        /// <returns></returns>
        public static bool CheckSignRSA2(string SignContent, string sign, string publicKey)
        {
            try
            {
                if (publicKey.Contains(".cer"))
                {
                    // X509Certificate2 pubcrt = new X509Certificate2(MedicalPayConfig.Cert_PathNJ, MedicalPayConfig.Cert_PassWordNJ);
                    // return CACheckSign(SignContent, sign, pubcrt, DEFAULT_CHARSET);
                    X509Certificate2 pubcrt = new X509Certificate2(publicKey);
                    return CACheckSignSHA256(SignContent, sign, pubcrt, DEFAULT_CHARSET);
                }
                else
                {

                    return CheckSignRSA2T(SignContent, sign, publicKey, DEFAULT_CHARSET);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
          
        }
       

        /// <summary>
        /// 验证签名RSA2 使用公钥  需要调用Windows系统动态库  
        /// </summary>
        /// <param name="content">需要验证的内容</param>
        /// <param name="signedString">签名结果</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="input_charset">编码格式</param>
        /// <returns></returns>
        static bool CheckSignRSA2T(string SignContent, string sign, string publicKey, string input_charset)
        {
            if (string.IsNullOrEmpty(input_charset))
            {
                input_charset = "utf-8";
            }
            string sPublicKeyPEM = "-----BEGIN PUBLIC KEY-----\r\n";
            sPublicKeyPEM += publicKey;
            sPublicKeyPEM += "-----END PUBLIC KEY-----\r\n\r\n";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.PersistKeyInCsp = false;
            RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
            bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(input_charset).GetBytes(SignContent), "SHA256", Convert.FromBase64String(sign));
            return bVerifyResultOriginal;
        }

        /// <summary>
        /// 验证签名RSA 使用CA证书
        /// </summary>
        /// <param name="SignContent"></param>
        /// <param name="sign"></param>
        /// <param name="pubcrt"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        static bool CACheckSign(String SignContent, string sign, X509Certificate2 pubcrt, string input_charset)
        {
            X509Certificate2 _X509Certificate2 = pubcrt;

            using (RSACryptoServiceProvider RSACryptography = _X509Certificate2.PublicKey.Key as RSACryptoServiceProvider)
            {
                bool result = false;
                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(SignContent);
                byte[] data = Convert.FromBase64String(sign);

                SHA1 sh = new SHA1CryptoServiceProvider();
                result = RSACryptography.VerifyData(Data, sh, data);
                return result;
            }
        }
        /// <summary>
        /// 验证签名SHA256 使用CA证书
        /// </summary>
        /// <param name="SignContent"></param>
        /// <param name="sign"></param>
        /// <param name="pubcrt"></param>
        /// <param name="input_charset"></param>
        /// <returns></returns>
        static bool CACheckSignSHA256(String SignContent, string sign, X509Certificate2 pubcrt, string input_charset)
        {
            X509Certificate2 _X509Certificate2 = pubcrt;
            using (RSACryptoServiceProvider RSACryptography = _X509Certificate2.PublicKey.Key as RSACryptoServiceProvider)
            {
                bool result = false;
                Encoding code = Encoding.GetEncoding(input_charset);
                byte[] Data = code.GetBytes(SignContent);
                byte[] data = Convert.FromBase64String(sign);
                SHA256 sh = new SHA256CryptoServiceProvider();
                //SHA1 sh = new SHA1CryptoServiceProvider();
                result = RSACryptography.VerifyData(Data, sh, data);
                return result;
            }
        }


        /// <summary>
        /// 获取验签证书
        /// </summary>
        /// <param name="pubKeyFile"></param>
        /// <returns></returns>
        private static RSACryptoServiceProvider GetPublicKey(string pubKeyFile)
        {
            var pc = new X509Certificate2(pubKeyFile);
            return (RSACryptoServiceProvider)pc.PublicKey.Key;
        }

        /// <summary>
        /// 通过公钥字符串获取
        /// </summary>
        /// <param name="pemFileConent"></param>
        /// <returns></returns>
        private static RSAParameters ConvertFromPublicKey(string pemFileConent)
        {


            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162)
            {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus0 = new byte[29];
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 0, pemModulus, 0, 29);
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        private static RSACryptoServiceProvider LoadCertificateString(string strKey, string signType)
        {
            byte[] data = null;
            //读取带
            //ata = Encoding.Default.GetBytes(strKey);
            data = Convert.FromBase64String(strKey);
            //data = GetPem("RSA PRIVATE KEY", data);
            try
            {
                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, signType);
                return rsa;
            }
            catch (Exception ex)
            {
                //    throw new AopException("EncryptContent = woshihaoren,zheshiyigeceshi,wanerde", ex);
            }
            return null;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }


    public static class RSACryptoServiceProviderExtension
    {
        #region Methods
        public static void LoadPublicKeyDER(RSACryptoServiceProvider provider, byte[] DERData)
        {
            byte[] RSAData = RSACryptoServiceProviderExtension.GetRSAFromDER(DERData);
            byte[] publicKeyBlob = RSACryptoServiceProviderExtension.GetPublicKeyBlobFromRSA(RSAData);
            provider.ImportCspBlob(publicKeyBlob);
        }

        public static void LoadPublicKeyPEM(RSACryptoServiceProvider provider, string sPEM)
        {
            byte[] DERData = RSACryptoServiceProviderExtension.GetDERFromPEM(sPEM);
            RSACryptoServiceProviderExtension.LoadPublicKeyDER(provider, DERData);
        }

        internal static byte[] GetPublicKeyBlobFromRSA(byte[] RSAData)
        {
            byte[] data = null;
            UInt32 dwCertPublicKeyBlobSize = 0;
            if (RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING,
                new IntPtr((int)CRYPT_OUTPUT_TYPES.RSA_CSP_PUBLICKEYBLOB), RSAData, (UInt32)RSAData.Length, CRYPT_DECODE_FLAGS.NONE,
                data, ref dwCertPublicKeyBlobSize))
            {
                data = new byte[dwCertPublicKeyBlobSize];
                if (!RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING,
                    new IntPtr((int)CRYPT_OUTPUT_TYPES.RSA_CSP_PUBLICKEYBLOB), RSAData, (UInt32)RSAData.Length, CRYPT_DECODE_FLAGS.NONE,
                    data, ref dwCertPublicKeyBlobSize))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return data;
        }

        internal static byte[] GetRSAFromDER(byte[] DERData)
        {
            byte[] data = null;
            byte[] publicKey = null;
            CERT_PUBLIC_KEY_INFO info;
            UInt32 dwCertPublicKeyInfoSize = 0;
            IntPtr pCertPublicKeyInfo = IntPtr.Zero;
            if (RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING, new IntPtr((int)CRYPT_OUTPUT_TYPES.X509_PUBLIC_KEY_INFO),
                DERData, (UInt32)DERData.Length, CRYPT_DECODE_FLAGS.NONE, data, ref dwCertPublicKeyInfoSize))
            {
                data = new byte[dwCertPublicKeyInfoSize];
                if (RSACryptoServiceProviderExtension.CryptDecodeObject(CRYPT_ENCODING_FLAGS.X509_ASN_ENCODING | CRYPT_ENCODING_FLAGS.PKCS_7_ASN_ENCODING, new IntPtr((int)CRYPT_OUTPUT_TYPES.X509_PUBLIC_KEY_INFO),
                    DERData, (UInt32)DERData.Length, CRYPT_DECODE_FLAGS.NONE, data, ref dwCertPublicKeyInfoSize))
                {
                    GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    try
                    {
                        info = (CERT_PUBLIC_KEY_INFO)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(CERT_PUBLIC_KEY_INFO));
                        publicKey = new byte[info.PublicKey.cbData];
                        Marshal.Copy(info.PublicKey.pbData, publicKey, 0, publicKey.Length);
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
                else
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            else
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return publicKey;
        }

        internal static byte[] GetDERFromPEM(string sPEM)
        {
            UInt32 dwSkip, dwFlags;
            UInt32 dwBinarySize = 0;

            if (!RSACryptoServiceProviderExtension.CryptStringToBinary(sPEM, (UInt32)sPEM.Length, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, null, ref dwBinarySize, out dwSkip, out dwFlags))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            byte[] decodedData = new byte[dwBinarySize];
            if (!RSACryptoServiceProviderExtension.CryptStringToBinary(sPEM, (UInt32)sPEM.Length, CRYPT_STRING_FLAGS.CRYPT_STRING_BASE64HEADER, decodedData, ref dwBinarySize, out dwSkip, out dwFlags))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return decodedData;
        }
        #endregion Methods

        #region P/Invoke Constants
        internal enum CRYPT_ACQUIRE_CONTEXT_FLAGS : uint
        {
            CRYPT_NEWKEYSET = 0x8,
            CRYPT_DELETEKEYSET = 0x10,
            CRYPT_MACHINE_KEYSET = 0x20,
            CRYPT_SILENT = 0x40,
            CRYPT_DEFAULT_CONTAINER_OPTIONAL = 0x80,
            CRYPT_VERIFYCONTEXT = 0xF0000000
        }

        internal enum CRYPT_PROVIDER_TYPE : uint
        {
            PROV_RSA_FULL = 1
        }

        internal enum CRYPT_DECODE_FLAGS : uint
        {
            NONE = 0,
            CRYPT_DECODE_ALLOC_FLAG = 0x8000
        }

        internal enum CRYPT_ENCODING_FLAGS : uint
        {
            PKCS_7_ASN_ENCODING = 0x00010000,
            X509_ASN_ENCODING = 0x00000001,
        }

        internal enum CRYPT_OUTPUT_TYPES : int
        {
            X509_PUBLIC_KEY_INFO = 8,
            RSA_CSP_PUBLICKEYBLOB = 19,
            PKCS_RSA_PRIVATE_KEY = 43,
            PKCS_PRIVATE_KEY_INFO = 44
        }

        internal enum CRYPT_STRING_FLAGS : uint
        {
            CRYPT_STRING_BASE64HEADER = 0,
            CRYPT_STRING_BASE64 = 1,
            CRYPT_STRING_BINARY = 2,
            CRYPT_STRING_BASE64REQUESTHEADER = 3,
            CRYPT_STRING_HEX = 4,
            CRYPT_STRING_HEXASCII = 5,
            CRYPT_STRING_BASE64_ANY = 6,
            CRYPT_STRING_ANY = 7,
            CRYPT_STRING_HEX_ANY = 8,
            CRYPT_STRING_BASE64X509CRLHEADER = 9,
            CRYPT_STRING_HEXADDR = 10,
            CRYPT_STRING_HEXASCIIADDR = 11,
            CRYPT_STRING_HEXRAW = 12,
            CRYPT_STRING_NOCRLF = 0x40000000,
            CRYPT_STRING_NOCR = 0x80000000
        }
        #endregion P/Invoke Constants

        #region P/Invoke Structures
        [StructLayout(LayoutKind.Sequential)]
        internal struct CRYPT_OBJID_BLOB
        {
            internal UInt32 cbData;
            internal IntPtr pbData;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CRYPT_ALGORITHM_IDENTIFIER
        {
            internal IntPtr pszObjId;
            internal CRYPT_OBJID_BLOB Parameters;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CRYPT_BIT_BLOB
        {
            internal UInt32 cbData;
            internal IntPtr pbData;
            internal UInt32 cUnusedBits;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CERT_PUBLIC_KEY_INFO
        {
            internal CRYPT_ALGORITHM_IDENTIFIER Algorithm;
            internal CRYPT_BIT_BLOB PublicKey;
        }
        #endregion P/Invoke Structures

        #region P/Invoke Functions
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDestroyKey(IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptImportKey(IntPtr hProv, byte[] pbKeyData, UInt32 dwDataLen, IntPtr hPubKey, UInt32 dwFlags, ref IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, CRYPT_PROVIDER_TYPE dwProvType, CRYPT_ACQUIRE_CONTEXT_FLAGS dwFlags);

        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptStringToBinary(string sPEM, UInt32 sPEMLength, CRYPT_STRING_FLAGS dwFlags, [Out] byte[] pbBinary, ref UInt32 pcbBinary, out UInt32 pdwSkip, out UInt32 pdwFlags);

        [DllImport("crypt32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDecodeObjectEx(CRYPT_ENCODING_FLAGS dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, UInt32 cbEncoded, CRYPT_DECODE_FLAGS dwFlags, IntPtr pDecodePara, ref byte[] pvStructInfo, ref UInt32 pcbStructInfo);

        [DllImport("crypt32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDecodeObject(CRYPT_ENCODING_FLAGS dwCertEncodingType, IntPtr lpszStructType, byte[] pbEncoded, UInt32 cbEncoded, CRYPT_DECODE_FLAGS flags, [In, Out] byte[] pvStructInfo, ref UInt32 cbStructInfo);
        #endregion P/Invoke Functions
    }


}
