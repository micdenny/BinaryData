using System;
using System.Collections.Generic;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a cache for <see cref="IDataConverter"/> instances.
    /// </summary>
    internal static class ConverterCache
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private static readonly Dictionary<Type, IDataConverter> _cache = new Dictionary<Type, IDataConverter>();

        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        /// <summary>
        /// Gets a possibly cached instance of a <see cref="IDataConverter"/> of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the <see cref="IDataConverter"/> to return.</param>
        /// <returns>An instance of the <see cref="IDataConverter"/>.</returns>
        internal static IDataConverter GetConverter(Type type)
        {
            if (!_cache.TryGetValue(type, out IDataConverter converter))
            {
                converter = (IDataConverter)Activator.CreateInstance(type);
                _cache.Add(type, converter);
            }
            return converter;
        }
    }
}
