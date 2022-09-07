using System.IO;
using System.Text;

namespace SC.App.Services.Bill.Lib.Extensions
{
    /// <summary>
    /// The string extension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// The default encoding
        /// </summary>
        public static Encoding DefaultEncoding => Encoding.UTF8;

        /// <summary>
        /// Check is string is null or empty
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>True if string is null or empty, Otherwise False</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Convert string to array of byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>The array of byte</returns>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            encoding = encoding == null ? DefaultEncoding : encoding;

            return value.ToBytesInternal(encoding);
        }

        /// <summary>
        /// Convert string to array of byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>The array of byte</returns>
        private static byte[] ToBytesInternal(this string value, Encoding encoding)
        {
            if (value.IsEmpty())
            {
                return new byte[] { };
            }

            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Convet string to stream
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>The stream</returns>
        public static Stream ToStream(this string value, Encoding encoding = null)
        {
            encoding = encoding == null ? DefaultEncoding : encoding;

            return value.ToStreamInternal(encoding);
        }

        /// <summary>
        /// Convet string to stream
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>The stream</returns>
        private static Stream ToStreamInternal(this string value, Encoding encoding)
        {
            byte[] bytes = value.ToBytes(encoding);

            return new MemoryStream(bytes);
        }
    }
}