using System;
using System.Collections.Generic;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a converter for reading and writing custom binary values.
    /// </summary>
    public abstract class BinaryConverter
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static readonly Dictionary<Type, BinaryConverter> _cache = new Dictionary<Type, BinaryConverter>();

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Reads the value from the given <paramref name="reader"/> and returns it to set the corresponding member.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryDataReader"/> to read the value from.</param>
        /// <param name="instance">The instance to which the value belongs.</param>
        /// <param name="memberAttribute">The <see cref="BinaryMemberAttribute"/> containing configuration which can be
        /// used to modify the behavior of the converter.</param>
        /// <returns>The read value.</returns>
        public abstract object Read(BinaryDataReader reader, object instance, BinaryMemberAttribute memberAttribute);

        /// <summary>
        /// Writes the value with the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryDataWriter"/> to write the value with.</param>
        /// <param name="instance">The instance to which the value belongs.</param>
        /// <param name="memberAttribute">The <see cref="BinaryMemberAttribute"/> containing configuration which can be
        /// used to modify the behavior of the converter.</param>
        /// <param name="value">The value to write.</param>
        public abstract void Write(BinaryDataWriter writer, object instance, BinaryMemberAttribute memberAttribute,
            object value);

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        /// <summary>
        /// Gets a possibly cached instance of a <see cref="BinaryConverter"/> of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the <see cref="BinaryConverter"/> to return.</param>
        /// <returns>An instance of the <see cref="BinaryConverter"/>.</returns>
        internal static BinaryConverter GetConverter(Type type)
        {
            if (!_cache.TryGetValue(type, out BinaryConverter converter))
            {
                converter = (BinaryConverter)Activator.CreateInstance(type);
                _cache.Add(type, converter);
            }
            return converter;
        }
    }
}
