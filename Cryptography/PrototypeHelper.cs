﻿using System;
using System.Security.Cryptography;
using XiaoFeng.Cryptography;
namespace XiaoFeng
{
    /// <summary>
    /// 加密解密扩展属性方法操作类
    /// Version : 1.0.0
    /// Author : jacky
    /// Site : www.zhuovi.com
    /// QQ : 7092734
    /// Email : jacky@zhuovi.com
    /// Description:
    /// </summary>
    public static partial class PrototypeHelper
    {
        #region AES
        /// <summary>AES加密</summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(this string _, string key = "") => _.IsNullOrEmpty() ? string.Empty : new AESEncryption().Encrypt(_, key, "", OutputMode.Base64);
        /// <summary>AES解密</summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(this string _, string key = "") => _.IsNullOrEmpty() ? string.Empty : new AESEncryption().Decrypt(_, key, "", OutputMode.Base64);

        /// <summary>AES加密</summary>
        /// <param name="_">字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移量</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingModel">填充类型</param>
        /// <param name="outputMode">输出模式</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(this string _, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingModel = PaddingMode.PKCS7, OutputMode outputMode = OutputMode.Base64)
        {
            return _.IsNullOrEmpty() ? string.Empty : new AESEncryption().Encrypt(_, key, iv, cipherMode, paddingModel, outputMode);
        }
        /// <summary>AES解密</summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移量</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingModel">填充类型</param>
        /// <param name="outputMode">输出模式</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(this string _, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingModel = PaddingMode.PKCS7, OutputMode outputMode = OutputMode.Base64)
        {
            return _.IsNullOrEmpty() ? string.Empty : new AESEncryption().Decrypt(_, key, iv, cipherMode, paddingModel, outputMode);
        }
        #endregion

        #region DES
        /// <summary>DES加密</summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DESEncrypt(this string _, string key = "") => _.IsNullOrEmpty() ? string.Empty : new DESEncryption().Encrypt(_, key, "", OutputMode.Base64);
        /// <summary>DES解密</summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DESDecrypt(this string _, string key = "") => _.IsNullOrEmpty() ? string.Empty : new DESEncryption().Decrypt(_, key, "", OutputMode.Base64);
        /// <summary>DES加密</summary>
        /// <param name="_">字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移量</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingModel">填充类型</param>
        /// <param name="outputMode">输出模式</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(this string _, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingModel = PaddingMode.PKCS7, OutputMode outputMode = OutputMode.Base64)
        {
            return _.IsNullOrEmpty() ? string.Empty : new DESEncryption().Encrypt(_, key, iv, cipherMode, paddingModel, outputMode);
        }

        /// <summary>DES解密</summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移量</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingModel">填充类型</param>
        /// <param name="outputMode">输出模式</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(this string _, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingModel = PaddingMode.PKCS7, OutputMode outputMode = OutputMode.Base64)
        {
            return _.IsNullOrEmpty() ? string.Empty : new DESEncryption().Decrypt(_, key, iv, cipherMode, paddingModel, outputMode);
        }

        #endregion

        #region DES3
        /// <summary>DES3加密</summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DES3Encrypt(this string _, string key = "") => _.IsNullOrEmpty() ? string.Empty : new DES3Encryption().Encrypt(_, key, "", OutputMode.Base64);
        /// <summary>3DES解密</summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DES3Decrypt(this string _, string key = "") => _.IsNullOrEmpty() ? string.Empty : new DES3Encryption().Decrypt(_, key, "", OutputMode.Base64);
        /// <summary>DES3加密</summary>
        /// <param name="_">字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移量</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingModel">填充类型</param>
        /// <param name="outputMode">输出模式</param>
        /// <returns>密文</returns>
        public static string DES3Encrypt(this string _, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingModel = PaddingMode.PKCS7, OutputMode outputMode = OutputMode.Base64)
        {
            return _.IsNullOrEmpty() ? string.Empty : new DES3Encryption().Encrypt(_, key, iv, cipherMode, paddingModel, outputMode);
        }
        /// <summary>DES3解密</summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">偏移量</param>
        /// <param name="cipherMode">密码模式</param>
        /// <param name="paddingModel">填充类型</param>
        /// <param name="outputMode">输出模式</param>
        /// <returns>明文</returns>
        public static string DES3Decrypt(this string _, string key, string iv, CipherMode cipherMode = CipherMode.CBC, PaddingMode paddingModel = PaddingMode.PKCS7, OutputMode outputMode = OutputMode.Base64)
        {
            return _.IsNullOrEmpty() ? string.Empty : new DES3Encryption().Decrypt(_, key, iv, cipherMode, paddingModel, outputMode);
        }
        #endregion

        #region RC4
        /// <summary>
        /// RC4加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string RC4Encrypt(this string _, string key = "")
        {
            //return RC4Crypto.RC4.Encrypt(_, key);
            return new RC4Encryption().Encrypt(_, key);
        }
        /// <summary>
        /// RC4解密
        /// </summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string RC4Decrypt(this string _, string key = "")
        {
            //return RC4Crypto.RC4.Decrypt(_, key);
            return new RC4Encryption().Decrypt(_, key);
        }
        #endregion

        #region ELF
        /// <summary>
        /// ELF加密
        /// </summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string ELFEncrypt(this string _, string key = "")
        {
            //return ELFCrypto.ELF.Encrypt(_, key);
            return new ELFEncryption().Encrypt(_, key);
        }
        /// <summary>
        /// ELF解密
        /// </summary>
        /// <param name="_">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string ELFDecrypt(this string _, string key = "")
        {
            //return ELFCrypto.ELF.Decrypt(_, key);
            return new ELFEncryption().Decrypt(_, key);
        }
        #endregion

        #region SHA
        /// <summary>
        /// SHA加密
        /// </summary>
        /// <param name="_">原文</param>
        /// <param name="type">加密类型</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string SHAEncrypt(this string _, SHAType type = SHAType.SHA1, OutputMode mode = OutputMode.Hex) => new SHAEncryption().Encrypt(_, type, mode);
        /// <summary>
        /// 字符串md5加密
        /// </summary>
        /// <param name="_">字符串</param>
        /// <param name="length">长度 16或32</param>
        /// <param name="ignoreCase">大写小写 true为小写 false 为大写</param>
        /// <returns></returns>
        public static string MD5(this string _, int length = 32, Boolean ignoreCase = true)
        {
            var data = _.SHAEncrypt(SHAType.MD5);
            if (length == 16) data = data.Substring(0, 16);
            return ignoreCase ? data.ToLower() : data.ToUpper();
        }
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string SHA1Encrypt(this string _, OutputMode mode = OutputMode.Hex) => _.SHAEncrypt(SHAType.SHA1, mode);
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string SHA256Encrypt(this string _, OutputMode mode = OutputMode.Hex) => _.SHAEncrypt(SHAType.SHA256, mode);
        /// <summary>
        /// SHA384加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string SHA384Encrypt(this string _, OutputMode mode = OutputMode.Hex) => _.SHAEncrypt(SHAType.SHA384, mode);
        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string SHA512Encrypt(this string _, OutputMode mode = OutputMode.Hex) => _.SHAEncrypt(SHAType.SHA512, mode);
        #endregion

        #region HMACSHA
        /// <summary>
        /// HMAC加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="type">加密类型</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACEncrypt(this string _, string key, HMACType type = HMACType.SHA1, OutputMode mode = OutputMode.Hex) => new HMACEncryption().Encrypt(_, key, type, mode);
        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACSHA1Encrypt(this string _, string key, OutputMode mode = OutputMode.Hex) => _.HMACEncrypt(key, HMACType.SHA1, mode);
        /// <summary>
        /// HMACRIPEMD160加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACRIPEMD160Encrypt(this string _, string key, OutputMode mode = OutputMode.Hex) => _.HMACEncrypt(key, HMACType.RIPEMD160, mode);
        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACSHA256Encrypt(this string _, string key, OutputMode mode = OutputMode.Hex) => _.HMACEncrypt(key, HMACType.SHA256, mode);
        /// <summary>
        /// HMACSHA384加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACSHA384Encrypt(this string _, string key, OutputMode mode = OutputMode.Hex) => _.HMACEncrypt(key, HMACType.SHA384, mode);
        /// <summary>
        /// HMACSHA512加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACSHA512Encrypt(this string _, string key, OutputMode mode = OutputMode.Hex) => _.HMACEncrypt(key, HMACType.SHA512, mode);
        /// <summary>
        /// HMACMD5加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACMD5Encrypt(this string _, string key, OutputMode mode = OutputMode.Hex) => _.HMACEncrypt(key, HMACType.MD5, mode);
        /// <summary>
        /// HMACMD5加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="key">密钥</param>
        /// <param name="mode">输出模式</param>
        /// <returns></returns>
        public static string HMACMACTripleDESEncrypt(this string _, string key, OutputMode mode = OutputMode.Hex) => _.HMACEncrypt(key, HMACType.MACTripleDES, mode);
        #endregion

        #region RSA
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="_">明文</param>
        /// <param name="publicKey">私钥</param>
        /// <returns></returns>
        public static string RSAEncrypt(this string _, string publicKey = "")
        {
            //return RSACrypto.RSA.Encrypt(_, publicKey);
            return new RSAEncryption().Encrypt(_, publicKey);
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="_">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public static string RSADecrypt(this string _, string privateKey = "")
        {
            //return RSACrypto.RSA.Decrypt(_, privateKey);
            return new RSAEncryption().Decrypt(_, privateKey);
        }
        #endregion

        #region 转字符串
        /// <summary>
        /// 转字符串
        /// </summary>
        /// <param name="data">数字</param>
        /// <param name="outputMode">输出类型</param>
        /// <returns></returns>
        public static string ToString(this byte[] data, OutputMode outputMode = OutputMode.Base64)
        {
            if (data == null || data.Length == 0) return string.Empty;
            return outputMode == OutputMode.Base64 ? data.ToBase64String() : data.ByteToHexString().RemovePattern(@"\s+");
        }
        #endregion

    }
}