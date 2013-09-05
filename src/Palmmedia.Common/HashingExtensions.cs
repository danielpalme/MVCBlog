using System;
using System.Security.Cryptography;
using System.Text;

namespace Palmmedia.Common
{
    /// <summary>
    /// Contains extension methods for hashing of <see cref="string">strings</see>.
    /// </summary>
    public static class HashingExtensions
    {
        /// <summary>
        /// Encrypts the given <see cref="string"/> with SHA1.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns>The encrypted <see cref="string"/>.</returns>
        public static string EncryptSha1(this string value)
        {
            return Encrypt(value, string.Empty, SHA1.Create());
        }

        /// <summary>
        /// Encrypts the given <see cref="string"/> with SHA1.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The encrypted <see cref="string"/>.</returns>
        public static string EncryptSha1(this string value, string salt)
        {
            return Encrypt(value, salt, SHA1.Create());
        }

        /// <summary>
        /// Encrypts the given <see cref="string"/> with SHA512.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns>The encrypted <see cref="string"/>.</returns>
        public static string EncryptSha512(this string value)
        {
            return Encrypt(value, string.Empty, SHA512.Create());
        }

        /// <summary>
        /// Encrypts the given <see cref="string"/> with SHA512.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>The encrypted <see cref="string"/>.</returns>
        public static string EncryptSha512(this string value, string salt)
        {
            return Encrypt(value, salt, SHA512.Create());
        }

        /// <summary>
        /// Encrypts the given <see cref="string"/> with MD5.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns>The encrypted <see cref="string"/>.</returns>
        public static string EncryptMD5(this string value)
        {
            return Encrypt(value, string.Empty, MD5.Create());
        }

        /// <summary>
        /// Encrypts the given <see cref="string"/> with the given <see cref="HashAlgorithm"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <returns>The encrypted <see cref="string"/>.</returns>
        private static string Encrypt(string value, string salt, HashAlgorithm hashAlgorithm)
        {
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            byte[] toHash = new byte[valueBytes.Length + saltBytes.Length];
            Array.Copy(valueBytes, 0, toHash, 0, valueBytes.Length);
            Array.Copy(saltBytes, 0, toHash, valueBytes.Length, saltBytes.Length);

            var hash = hashAlgorithm.ComputeHash(toHash);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
