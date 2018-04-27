using System;
using System.IO;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents extension methods for read and write operations on <see cref="Stream"/> instances.
    /// </summary>
    public static class BinaryStreamExtensions
    {
        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        // ---- Read ----

        /// <summary>
        /// Returns an instance of type <typeparamref name="T"/> read from the underlying stream.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/> instance.</param>
        /// <returns>The value read from the current stream.</returns>
        public static T ReadObject<T>(this BinaryStream self)
            => self.ReadObject<T>(self.ByteConverter);

        /// <summary>
        /// Returns an array of instances of type <typeparamref name="T"/> read from the underlying stream.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/> instance.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of values read from the current stream.</returns>
        public static T[] ReadObjects<T>(this BinaryStream self, int count)
            => self.ReadObjects<T>(count, self.ByteConverter);

        // ---- Write ----

        /// <summary>
        /// Writes the given instance value to the underlying stream.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryStream"/> instance.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteObject(this BinaryStream self, Object value)
            => self.WriteObject(value, self.ByteConverter);
    }
}
