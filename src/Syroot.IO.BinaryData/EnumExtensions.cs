using System;
using System.Linq;
using System.Reflection;

namespace Syroot.IO
{
    /// <summary>
    /// Represents a collection of methods extending enum types.
    /// </summary>
    internal static class EnumExtensions
    {
        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------
        
        /// <summary>
        /// Returns whether <paramref name="value"/> is a defined value in the enum of the given
        /// <paramref name="enumType"/> or a valid set of flags for enums decorated with the
        /// <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <param name="enumType">The enum type to check.</param>
        /// <param name="value">The value to check against the enum type.</param>
        /// <returns><c>true</c> if the value is valid; otherwise <c>false</c>.</returns>
        internal static bool IsValid(Type enumType, object value)
        {
            // For enumerations decorated with the FlagsAttribute, allow sets of flags.
            bool valid = Enum.IsDefined(enumType, value);
            if (!valid && enumType.GetTypeInfo().GetCustomAttributes(typeof(FlagsAttribute), true)?.Any() == true)
            {
                long mask = 0;
                foreach (object definedValue in Enum.GetValues(enumType))
                {
                    mask |= (long)definedValue;
                }
                long longValue = (long)value;
                valid = (mask & longValue) == longValue;
            }
            return valid;
        }
    }
}
