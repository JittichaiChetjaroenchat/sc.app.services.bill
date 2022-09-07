using System.Text;

namespace SC.App.Services.Bill.Lib.Extensions
{
    /// <summary>
    /// The byte extension
    /// </summary>
    public static class ByteExtension
    {
        /// <summary>
        /// The default encoding
        /// </summary>
        public static Encoding DefaultEncoding => Encoding.UTF8;

        /// <summary>
        /// Get string from array of byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>The string</returns>
        public static string GetString(this byte[] value, Encoding encoding = null)
        {
            encoding = encoding == null ? DefaultEncoding : encoding;

            return value.GetStringInternal(encoding);
        }

        /// <summary>
        /// Get string from array of byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="encoding">The encoding</param>
        /// <returns>The string</returns>
        private static string GetStringInternal(this byte[] value, Encoding encoding)
        {
            return encoding.GetString(value);
        }
    }
}