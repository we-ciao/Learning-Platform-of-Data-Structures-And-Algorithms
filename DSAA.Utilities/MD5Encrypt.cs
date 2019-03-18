using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DSAA.Utilities
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public static class MD5Encrypt
    {
        private const String SALT = "__WTU__";

        /// <summary>
        /// 对指定用户名和密码进行加密
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">原始密码</param>
        /// <returns>加密后的密码</returns>
        public static String Encrypt(String userName, String passWord)
        {
            String temp = EncryptToHexString(userName + passWord, true) + SALT;

            return EncryptToHexString(temp, true);
        }

        /// <summary>
        /// 返回16位MD5加密字节数组
        /// </summary>
        /// <param name="origin">待加密的数据</param>
        /// <returns>16位MD5加密字节数组</returns>
        public static Byte[] EncryptToByteArray(Byte[] origin)
        {
            if (origin == null)
            {
                return new Byte[0];
            }

            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            Byte[] data = hasher.ComputeHash(origin);

            return data;
        }

        /// <summary>
        /// 返回16位MD5加密字节数组
        /// </summary>
        /// <param name="origin">待加密的字符串</param>
        /// <returns>16位MD5加密字节数组</returns>
        public static Byte[] EncryptToByteArray(String origin)
        {
            if (String.IsNullOrEmpty(origin))
            {
                return new Byte[0];
            }

            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            Byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(origin));

            return data;
        }

        /// <summary>
        /// 返回16位MD5十六进制加密字符串
        /// </summary>
        /// <param name="origin">待加密的数据</param>
        /// <param name="upperCase">是否大写</param>
        /// <returns>16位MD5十六进制加密字符串</returns>
        public static String EncryptToHexString(Byte[] origin, Boolean upperCase)
        {
            Byte[] data = EncryptToByteArray(origin);

            StringBuilder sb = new StringBuilder();
            String format = (upperCase ? "X2" : "x2");
            for (Int32 i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(format));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 返回16位MD5十六进制加密字符串
        /// </summary>
        /// <param name="origin">待加密的字符串</param>
        /// <param name="upperCase">是否大写</param>
        /// <returns>16位MD5十六进制加密字符串</returns>
        public static String EncryptToHexString(String origin, Boolean upperCase)
        {
            Byte[] data = EncryptToByteArray(origin);

            StringBuilder sb = new StringBuilder();
            String format = (upperCase ? "X2" : "x2");
            for (Int32 i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(format));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 返回16位MD5加密Base64字符串
        /// </summary>
        /// <param name="origin">待加密的数据</param>
        /// <returns>MD5 Base64加密字符串</returns>
        public static String EncryptToBase64String(Byte[] origin)
        {
            Byte[] data = EncryptToByteArray(origin);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 返回16位MD5加密Base64字符串
        /// </summary>
        /// <param name="origin">待加密的字符串</param>
        /// <returns>MD5 Base64加密字符串</returns>
        public static String EncryptToBase64String(String origin)
        {
            Byte[] data = EncryptToByteArray(origin);
            return Convert.ToBase64String(data);
        }
    }
}