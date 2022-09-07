using System;
using System.Linq;
using System.Reflection;

namespace SC.App.Services.Bill.Lib.Extensions
{
    /// <summary>
    /// The enum extension
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Try parse string to enum
        /// </summary>
        /// <typeparam name="T">The type of enum</typeparam>
        /// <param name="input">The input</param>
        /// <param name="output">The output</param>
        /// <returns>True if can parse, Otherwise False</returns>
        public static bool TryParse<T>(this string input, out T output)
            where T : struct
        {
            return Enum.TryParse<T>(input, true, out output);
        }

        /// <summary>
        /// Get value
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>The value</returns>
        public static int GetValue(this Enum input)
        {
            return Convert.ToInt32(input);
        }

        /// <summary>
        /// Get description
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>The description</returns>
        public static string GetDescription(this Enum input)
        {
            Type enumType = input.GetType();
            MemberInfo[] memberInfo = enumType.GetMember(input.ToString());

            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Any()))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }

            return enumType.ToString();
        }

        /// <summary>
        /// Check value is match in enum
        /// </summary>
        /// <typeparam name="T">The type of enum</typeparam>
        /// <param name="input">The input</param>
        /// <param name="value">The value</param>
        /// <returns>True if match in enum, Othewise False</returns>
        public static bool IsDefined(this Type input, string value)
        {
            if (!input.IsEnum)
            {
                return false;
            }

            return Enum.IsDefined(input, value);
        }
    }
}