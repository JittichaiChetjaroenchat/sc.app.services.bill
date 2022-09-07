using System.IO;

namespace SC.App.Services.Bill.Lib.Extensions
{
    /// <summary>
    /// The stream extension
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// Get string from stream
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The string</returns>
        public static string GetString(this Stream value)
        {
            using (StreamReader reader = new StreamReader(value))
            {
                return reader.ReadToEnd();
            }
        }
    }
}