using System;
using System.Collections.Generic;
using System.Linq;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a collection of methods extending enum types.
    /// </summary>
    internal static class EnumTools
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static Dictionary<Type, bool> _flagEnums = new Dictionary<Type, bool>();

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Returns whether <paramref name="value"/> is a defined value in the enum of the given
        /// <paramref name="enumType"/> or a valid set of flags for enums decorated with the
        /// <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <param name="enumType">The type of the enum.</param>
        /// <param name="value">The value to check against the enum type.</param>
        /// <returns><c>true</c> if the value is valid; otherwise <c>false</c>.</returns>
        public static bool IsValid(Type enumType, object value)
        {
            // For enumerations decorated with the FlagsAttribute, allow sets of flags.
            bool valid = Enum.IsDefined(enumType, value);
            if (!valid && IsFlagsEnum(enumType))
            {
                long mask = 0;
                foreach (object definedValue in Enum.GetValues(enumType))
                    mask |= Convert.ToInt64(definedValue);
                long longValue = Convert.ToInt64(value);
                valid = (mask & longValue) == longValue;
            }
            return valid;
        }

        // ---- METHODS (PRIVATE) --------------------------------------------------------------------------------------

        private static bool IsFlagsEnum(Type type)
        {
            if (!_flagEnums.TryGetValue(type, out bool value))
            {
                value = type.GetCustomAttributes(typeof(FlagsAttribute), true)?.Any() == true;
                _flagEnums.Add(type, value);
            }
            return value;
        }
    }
}
