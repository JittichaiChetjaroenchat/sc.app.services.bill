using System.Collections.Generic;
using System.Linq;

namespace SC.App.Services.Bill.Lib.Extensions
{
    /// <summary>
    /// The collection extension
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// Check is collection is null or empty
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="value">The value</param>
        /// <returns>True if collection is null or empty, Otherwise False</returns>
        public static bool IsEmpty<T>(this ICollection<T> value)
            where T : class
        {
            if (value == null || !value.Any())
            {
                return true;
            }

            return false;
        }
    }
}