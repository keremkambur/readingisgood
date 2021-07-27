using System;

namespace ReadingIsGood.Utils.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Tries to convert a string into a given enum type.
        /// </summary>
        /// <typeparam name="TEnumType">Target enum type.</typeparam>
        /// <param name="string">Source string.</param>
        /// <param name="fallback">returned if string could not be converted.</param>
        /// <param name="ignoreCase">if true, ignores the case of the string.</param>
        /// <returns>Enum Type value</returns>
        /// <exception cref="ArgumentException">GenericType is no Enum.</exception>
        public static TEnumType ConvertToEnum<TEnumType>(this string @string, TEnumType fallback,
            bool ignoreCase = true) where TEnumType : struct, IConvertible
        {
            if (!typeof(TEnumType).IsEnum)
                throw new ArgumentException($"{nameof(TEnumType)} must be an enumerated type!");

            if (string.IsNullOrEmpty(@string) || string.IsNullOrWhiteSpace(@string)) return fallback;

            return Enum.TryParse(@string, ignoreCase, out TEnumType result)
                    ? result
                    : fallback
                ;
        }
    }
}