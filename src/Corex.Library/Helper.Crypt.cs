#region License

// Copyright(c) 2023 GrappTec
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable UnusedMember.Global

namespace Corex.Library;

public partial class XHelper
{
    public static class Crypt
    {
        private static readonly Provider LocalProvider;

        private static readonly HashAlgorithm DefaultHashAlgorithm = SHA512.Create();

        static Crypt()
        {
            LocalProvider = new Provider();
        }

        public static T DecriptAndDeserialize<T>(byte[] encriptData) => Serializers.Json.Deserialize<T>(LocalProvider.DecryptString(encriptData));

        public static byte[] Decrypt(byte[] data) => LocalProvider.Decrypt(data);

        public static string Decrypt(string data) => LocalProvider.DecryptString(data);

        public static string Encrypt(string data) => LocalProvider.EncryptToString(data);

        public static byte[] Encrypt(byte[] data) => LocalProvider.Encrypt(data);

        public static byte[] GetHash(string input, HashAlgorithm hashAlgorithm = null)
        {
            var bytes = Encoding.ASCII.GetBytes(input);

            var hashBytes = (hashAlgorithm ?? DefaultHashAlgorithm).ComputeHash(bytes);

            return hashBytes;
        }

        public static byte[] SerializeAndEncrypt(object objSerializeAndCript) => LocalProvider.Encrypt(Serializers.Json.Serialize(objSerializeAndCript));

        private class Provider
        {
            private readonly ICryptoTransform _decryptorTransform;
            private readonly ICryptoTransform _encryptorTransform;

            private readonly UTF8Encoding _utfEncoder;

            public Provider() : this(null, null) { }

            private Provider(string rgbKey, string rgbIv)
            {
                rgbKey ??= _internalInfo1;
                rgbIv ??= _internalInfo2;

                var parts1 = rgbKey.Split(',');
                var key = new byte[parts1.Length];

                for (var i = 0; i < parts1.Length; i++)
                {
                    key[i] = byte.Parse(parts1[i], NumberStyles.Integer, CultureInfo.InvariantCulture);
                }

                var parts2 = rgbIv.Split(',');
                var vector = new byte[parts2.Length];

                for (var i = 0; i < parts2.Length; i++)
                {
                    key[i] = byte.Parse(parts2[i], NumberStyles.Integer, CultureInfo.InvariantCulture);
                }

                //This is our encryption method
                var rm = new RijndaelManaged();

                //Create an encryptor and a decryptor using our encryption method, key, and vector.
                _encryptorTransform = rm.CreateEncryptor(key, vector);
                _decryptorTransform = rm.CreateDecryptor(key, vector);

                //Used to translate bytes to text and vice versa
                _utfEncoder = new UTF8Encoding();
            }

// ReSharper disable MemberHidesStaticFromOuterClass
            public byte[] Decrypt(byte[] encryptedValue)
// ReSharper restore MemberHidesStaticFromOuterClass
            {
                #region Write the encrypted value to the decryption stream

                var encryptedStream = new MemoryStream();
                var decryptStream = new CryptoStream(encryptedStream, _decryptorTransform, CryptoStreamMode.Write);
                decryptStream.Write(encryptedValue, 0, encryptedValue.Length);
                decryptStream.FlushFinalBlock();

                #endregion

                #region Read the decrypted value from the stream.

                encryptedStream.Position = 0;
                var decryptedBytes = new byte[encryptedStream.Length];
                encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                encryptedStream.Close();

                #endregion

                return decryptedBytes;
            }

            public string DecryptString(byte[] encryptedString) => _utfEncoder.GetString(Decrypt(encryptedString));

            public string DecryptString(string encryptedString) => _utfEncoder.GetString(Decrypt(StrToByteArray(encryptedString)));

            // ReSharper disable MemberHidesStaticFromOuterClass
            // ReSharper disable MemberCanBePrivate.Local
            public byte[] Encrypt(string textValue)
                // ReSharper restore MemberCanBePrivate.Local
                // ReSharper restore MemberHidesStaticFromOuterClass
                =>
                    Encrypt(_utfEncoder.GetBytes(textValue));

            // ReSharper disable MemberHidesStaticFromOuterClass
            public byte[] Encrypt(byte[] bytes)
                // ReSharper restore MemberHidesStaticFromOuterClass
            {
                //Used to stream the data in and out of the CryptoStream.
                using var memoryStream = new MemoryStream();
                /*
                 * We will have to write the unencrypted bytes to the stream,
                 * then read the encrypted result back from the stream.
                 */

                #region Write the decrypted value to the encryption stream

                var cs = new CryptoStream(memoryStream, _encryptorTransform, CryptoStreamMode.Write);
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();

                #endregion

                #region Read encrypted value back out of the stream

                memoryStream.Position = 0;
                var encrypted = new byte[memoryStream.Length];
                memoryStream.Read(encrypted, 0, encrypted.Length);

                #endregion

                return encrypted;
            }

            public string EncryptToString(string textValue) => ByteArrToString(Encrypt(textValue));

            [UsedImplicitly]
            public static byte[] GenerateEncryptionKey()
            {
                //Generate a Key.
                var rm = new RijndaelManaged();
                rm.GenerateKey();

                return rm.Key;
            }

            [UsedImplicitly]
            public static byte[] GenerateEncryptionVector()
            {
                //Generate a Vector
                var rm = new RijndaelManaged();
                rm.GenerateIV();

                return rm.IV;
            }

            private static string ByteArrToString(byte[] byteArr)
            {
                var tempStr = string.Empty;

                for (var i = 0; i <= byteArr.GetUpperBound(0); i++)
                {
                    var val = byteArr[i];
                    if (val < 10)
                    {
                        tempStr += "00" + val.ToString(CultureInfo.InvariantCulture);
                    }
                    else if (val < 100)
                    {
                        tempStr += "0" + val.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        tempStr += val.ToString(CultureInfo.InvariantCulture);
                    }
                }

                return tempStr;
            }

            private static byte[] StrToByteArray(string str)
            {
                if (str.Length == 0)
                {
                    throw new Exception("Invalid string value in StrToByteArray");
                }

                var byteArr = new byte[str.Length / 3];
                var i = 0;
                var j = 0;
                do
                {
                    var val = byte.Parse(str.Substring(i, 3));
                    byteArr[j++] = val;
                    i += 3;
                } while (i < str.Length);

                return byteArr;
            }

            // ReSharper disable ConvertToConstant.Local
            private readonly string _internalInfo1 = "123, 217, 89, 71, 64, 56, 45, 45, 134, 124, 17, 162, 37, 112, 222, 209, 241, 24,175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209".Replace(" ", string.Empty);

            private readonly string _internalInfo2 = "146, 64, 191, 211, 223, 23, 213, 219, 131, 131, 241, 152, 69, 72, 114, 156".Replace(" ", string.Empty);
            // ReSharper restore ConvertToConstant.Local
        }
    }
}