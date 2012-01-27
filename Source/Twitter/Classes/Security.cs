using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.IO;

namespace Twitter
{
    public class Security
    {
        private static byte[] c_sbAditionalEntropy = { 5, 14, 1, 10, 9, 8 };

        public static string ProtectString(string sToProtect)
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(sToProtect), c_sbAditionalEntropy, DataProtectionScope.CurrentUser));
        }

        public static string UnprotectString(string sToUnprotect)
        {
            return Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(sToUnprotect), c_sbAditionalEntropy, DataProtectionScope.CurrentUser));
        }

        public static string EncryptString(string sToEncrypt)
        {
            byte[] baKey = GetKey();
            byte[] baRawData = Encoding.UTF8.GetBytes(sToEncrypt);
            ICryptoTransform ictEncryptor = new TripleDESCryptoServiceProvider().CreateEncryptor(baKey, GetVector(baKey));
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, ictEncryptor, CryptoStreamMode.Write);
            cStream.Write(baRawData, 0, baRawData.Length);
            cStream.Close();
            mStream.Close();
            return Convert.ToBase64String(mStream.ToArray());
        }

        public static string DecryptString(string sToDecrypt)
        {
            byte[] baKey = GetKey();
            byte[] baRawData = Convert.FromBase64String(sToDecrypt);
            ICryptoTransform ictDecryptor = new TripleDESCryptoServiceProvider().CreateDecryptor(baKey, GetVector(baKey));
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, ictDecryptor, CryptoStreamMode.Write);
            cStream.Write(baRawData, 0, baRawData.Length);
            cStream.Close();
            mStream.Close();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        private static byte[] GetKey()
        {
            return FoldKey(GetMacAddressId());
        }

        private static byte[] GetVector(byte[] baKey)
        {
            return new SHA1CryptoServiceProvider().ComputeHash(baKey);
        }

        private static byte[] FoldKey(byte[] baKey, int iLength = 24)
        {
            byte[] baFoldedKey = new byte[iLength];

            for (int i = 0; i < baFoldedKey.Length; i++)
                baFoldedKey[i] = 0;

            for (int i = 0; i < baKey.Length; i++)
                baFoldedKey[i % baFoldedKey.Length] ^= baKey[i];

            return baFoldedKey;
        }

        private static byte[] GetMacAddressId()
        {
            List<string> lsAddresses = new List<string>();
            StringBuilder sbAllAddrs = new StringBuilder();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                lsAddresses.Add(nic.GetPhysicalAddress().ToString());

            lsAddresses.Sort();

            foreach (string sAddr in lsAddresses)
                sbAllAddrs.Append(sAddr);

            return Encoding.ASCII.GetBytes(sbAllAddrs.ToString());
        }
    }
}
