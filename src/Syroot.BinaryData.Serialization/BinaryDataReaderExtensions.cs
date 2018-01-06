using System;

namespace Syroot.BinaryData
{
    /// <summary>
    /// Represents a collection of extension methods for the <see cref="BinaryDataReader"/> class.
    /// </summary>
    public static class BinaryDataReaderExtensions
    {
        // ---- Object ----

        /// <summary>
        /// Reads an object of type <typeparamref name="T"/> from the current stream.
        /// </summary>
        /// <typeparam name="T">The type of the object to load.</typeparam>
        /// <param name="self">The extended <see cref="BinaryDataReader"/> instance.</param>
        /// <returns>The object read from the current stream.</returns>
        public static T ReadObject<T>(this BinaryDataReader self)
            => self.BaseStream.ReadObject<T>(self.ByteConverter);

        /// <summary>
        /// Reads the specified number of objects of type <typeparamref name="T"/> from the current stream.
        /// </summary>
        /// <typeparam name="T">The type of the objects to load.</typeparam>
        /// <param name="self">The extended <see cref="BinaryDataReader"/> instance.</param>
        /// <param name="count">The number of objects to read.</param>
        /// <returns>The objects array read from the current stream.</returns>
        public static T[] ReadObjects<T>(this BinaryDataReader self, int count)
            => self.BaseStream.ReadObjects<T>(count, self.ByteConverter);

        /// <summary>
        /// Reads an object of the given <paramref name="type"/> from the current stream.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryDataReader"/> instance.</param>
        /// <param name="type">The type of the object to load.</param>
        /// <returns>The object read from the current stream.</returns>
        public static object ReadObject(this BinaryDataReader self, Type type)
            => self.BaseStream.ReadObject(type, self.ByteConverter);

        /// <summary>
        /// Reads the specified number of objects of the given <paramref name="type"/> from the current stream.
        /// </summary>
        /// <param name="self">The extended <see cref="BinaryDataReader"/> instance.</param>
        /// <param name="type">The type of the object to load.</param>
        /// <param name="count">The number of objects to read.</param>
        /// <returns>The objects array read from the current stream.</returns>
        public static object[] ReadObjects(this BinaryDataReader self, Type type, int count)
            => self.BaseStream.ReadObjects(type, count, self.ByteConverter);
    }
}
